// // using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using FilterEditType = Janus.Windows.GridEX.FilterEditType;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
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

public class ProfitLoss : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label33;

	private Label label27;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel5;

	private Panel panel6;

	private Button btnshow;

	private Label lblOperExp;

	private Label lblFinanExp;

	private Label lblAdmiExp;

	private Label lblSellingExp;

	private Label lblTaxexpense;

	private Label lblotherexpense;

	private Label lblcostofsale;

	private Label lblSalesnet;

	private Label lblsalesnetamt;

	private Label lblotherexpenseamt;

	private Label lblcostofsaleamt;

	private Label lblfinancialexpamt;

	private Label lbloperatingexpenseamt;

	private Label lbladministratorexpamt;

	private Label lblsellingexpamt;

	private Label lbltaxexpenseamt;

	private Label lblmarkuploadAmount;

	private Label lblMarkupLoan;

	private Label lblDiscountAllowedAmount;

	private Label label2;

	private Label lblotherincomeamt;

	private Label lblotherincome;

	private GroupBox groupBox1;

	private Label lblGrosProf;

	private GroupBox groupBox2;

	private GroupBox groupBox14;

	private GroupBox groupBox4;

	private Label lblTotalFinancialExpense;

	private Label lblTotFinanExp;

	private GroupBox groupBox3;

	private Label lblToOperatingExpense;

	private Label lblTotOperExp;

	private GroupBox groupBox10;

	private Label label53;

	private Label label54;

	private Label label55;

	private Label label56;

	private Label label57;

	private Label label58;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label62;

	private GroupBox groupBox9;

	private Label label47;

	private Label label48;

	private Label label49;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label17;

	private Label lblProfitAfterTax;

	private Label lblToNonOperatingIncome;

	private Label lblNoOperIncomeTotal;

	private Label label29;

	private Label label26;

	private Label label21;

	private Label lblGrossprofit;

	private Label lblProfitBeforeTax;

	private Label lblBeforTaxProfit;

	private Label lblTotalProfit;

	private Label lblToProfit;

	private Label lblToOperatingProfit;

	private Label lblTotOperatProfit;

	private Label label7;

	private Label lblTotalExpFoterAmt;

	private Label label12;

	private Label lblTotalIncomeFoterAmt;

	private Label label3;

	private Label lblNetProfitFoterAmt;

	private Label label14;

	private Label lblBranch;

	internal UltraCombo cmbbranch;

	public ProfitLoss(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			wf.Show();
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
			if (!BranchFeature)
			{
				lblBranch.Visible = false;
				((Control)(object)cmbbranch).Visible = false;
				btnshow.Location = new Point(243, 24);
			}
			else
			{
				BranchFill();
			}
			fromdate.Focus();
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			GridBind();
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

	private void BranchFill()
	{
		try
		{
			DataTable dt = VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, "", 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbbranch, "Id", "BranchName", "Branch Name", ZeroIndex: false);
				if (BranchFeature && BranchFeatureConsolidated)
				{
					UltraGridColumn obj = ((UltraGridBase)cmbbranch).DisplayLayout.Bands[0].Columns.Add();
					((KeyedSubObjectBase)obj).Key = "Selected";
					((HeaderBase)obj.Header).Caption = string.Empty;
					obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
					obj.DataType = typeof(bool);
					((HeaderBase)obj.Header).VisiblePosition = 0;
					cmbbranch.LimitToList = false;
					cmbbranch.CheckedListSettings.CheckStateMember = "Selected";
					((EditorCheckedListSettings)cmbbranch.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
					((EditorCheckedListSettings)cmbbranch.CheckedListSettings).ListSeparator = ",";
					((EditorCheckedListSettings)cmbbranch.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				}
				((Control)(object)cmbbranch).Text = UserAccount.BranchName;
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
			fromdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FromDate = Conversion.ToDateTime((object)fromdate.Value),
				ToDate = Conversion.ToDateTime((object)todate.Value),
				Ids = "4,5"
			};
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, cmbbranch, "BranchName", wf);
			val.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			DataTable dt = VoucherReports.Accounts_ProfitLoss_ForCrystal(val);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val2 = null;
			val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val2.ShowReportWithDataTable(dt, "166-ProfitLossNewReport.rpt");
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			GridBind();
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

	private void GridBind()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		try
		{
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, cmbbranch, "BranchName", wf);
			DataTable dt = VoucherReports.ProftLoss(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FromDate = fromdate.Value,
				ToDate = todate.Value,
				BranchesIds = ((BranchIds != "") ? BranchIds : "")
			});
			double salesNet = 0.0;
			double costofsale = 0.0;
			double otherexpense = 0.0;
			double taxexpense = 0.0;
			double sellingexpense = 0.0;
			double administratinoExpese = 0.0;
			double FinancialExpense = 0.0;
			double operatingexpense = 0.0;
			double discountAllowed = 0.0;
			double markupLoans = 0.0;
			double otherIncome = 0.0;
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 2)
				{
					salesNet += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblSalesnet.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 4)
				{
					costofsale += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblcostofsale.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 5)
				{
					otherexpense += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblotherexpense.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 6)
				{
					taxexpense += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblTaxexpense.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 7)
				{
					sellingexpense += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblSellingExp.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 8)
				{
					administratinoExpese += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblAdmiExp.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 9)
				{
					FinancialExpense += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblFinanExp.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 32)
				{
					operatingexpense += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblOperExp.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 33)
				{
					discountAllowed += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblDiscountAllowedAmount.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 1)
				{
					markupLoans += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblMarkupLoan.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 3)
				{
					otherIncome += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblotherincome.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
			}
			lblsalesnetamt.Text = salesNet.ToString(clsGlobalVariables.stringFormatboth);
			lblcostofsaleamt.Text = costofsale.ToString(clsGlobalVariables.stringFormatboth);
			lblotherexpenseamt.Text = otherexpense.ToString(clsGlobalVariables.stringFormatboth);
			lbltaxexpenseamt.Text = taxexpense.ToString(clsGlobalVariables.stringFormatboth);
			lblsellingexpamt.Text = sellingexpense.ToString(clsGlobalVariables.stringFormatboth);
			lbladministratorexpamt.Text = administratinoExpese.ToString(clsGlobalVariables.stringFormatboth);
			lblfinancialexpamt.Text = FinancialExpense.ToString(clsGlobalVariables.stringFormatboth);
			lbloperatingexpenseamt.Text = operatingexpense.ToString(clsGlobalVariables.stringFormatboth);
			lblDiscountAllowedAmount.Text = discountAllowed.ToString(clsGlobalVariables.stringFormatboth);
			lblmarkuploadAmount.Text = markupLoans.ToString(clsGlobalVariables.stringFormatboth);
			lblotherincomeamt.Text = otherIncome.ToString(clsGlobalVariables.stringFormatboth);
			lblGrossprofit.Text = (salesNet + costofsale).ToString(clsGlobalVariables.stringFormatboth);
			lblToNonOperatingIncome.Text = (discountAllowed + otherIncome).ToString(clsGlobalVariables.stringFormatboth);
			lblToOperatingExpense.Text = (otherexpense + sellingexpense + administratinoExpese + operatingexpense).ToString(clsGlobalVariables.stringFormatboth);
			lblTotalFinancialExpense.Text = (FinancialExpense + markupLoans).ToString(clsGlobalVariables.stringFormatboth);
			lblTotalProfit.Text = (salesNet + costofsale + (discountAllowed + otherIncome)).ToString(clsGlobalVariables.stringFormatboth);
			lblToOperatingProfit.Text = (salesNet + costofsale + (discountAllowed + otherIncome) + (otherexpense + sellingexpense + administratinoExpese + operatingexpense)).ToString(clsGlobalVariables.stringFormatboth);
			lblProfitBeforeTax.Text = (salesNet + costofsale + (discountAllowed + otherIncome) + (otherexpense + sellingexpense + administratinoExpese + operatingexpense) + (FinancialExpense + markupLoans)).ToString(clsGlobalVariables.stringFormatboth);
			lblProfitAfterTax.Text = (salesNet + costofsale + (discountAllowed + otherIncome) + (otherexpense + sellingexpense + administratinoExpese + operatingexpense) + (FinancialExpense + markupLoans) + taxexpense).ToString(clsGlobalVariables.stringFormatboth);
			double TotalIncome = Conversion.ToDouble((object)(salesNet + discountAllowed + otherIncome));
			double TotalExpenses = Conversion.ToDouble((object)(costofsale + otherexpense + sellingexpense + administratinoExpese + operatingexpense + FinancialExpense + markupLoans + taxexpense));
			lblTotalIncomeFoterAmt.Text = TotalIncome.ToString(clsGlobalVariables.stringFormatboth);
			lblTotalExpFoterAmt.Text = TotalExpenses.ToString(clsGlobalVariables.stringFormatboth);
			lblNetProfitFoterAmt.Text = (TotalIncome + TotalExpenses).ToString(clsGlobalVariables.stringFormatboth);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void label12_Click(object sender, EventArgs e)
	{
	}

	private void ProfitLoss_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Expected O, but got Unknown
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.lblNetProfitFoterAmt = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.lblTotalExpFoterAmt = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.lblTotalIncomeFoterAmt = new System.Windows.Forms.Label();
		this.lblToOperatingProfit = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.lblTotOperatProfit = new System.Windows.Forms.Label();
		this.lblTotalProfit = new System.Windows.Forms.Label();
		this.lblBeforTaxProfit = new System.Windows.Forms.Label();
		this.lblToProfit = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.lblProfitAfterTax = new System.Windows.Forms.Label();
		this.lblProfitBeforeTax = new System.Windows.Forms.Label();
		this.groupBox14 = new System.Windows.Forms.GroupBox();
		this.lblTaxexpense = new System.Windows.Forms.Label();
		this.lbltaxexpenseamt = new System.Windows.Forms.Label();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label29 = new System.Windows.Forms.Label();
		this.lblTotalFinancialExpense = new System.Windows.Forms.Label();
		this.lblTotFinanExp = new System.Windows.Forms.Label();
		this.lblFinanExp = new System.Windows.Forms.Label();
		this.lblfinancialexpamt = new System.Windows.Forms.Label();
		this.lblmarkuploadAmount = new System.Windows.Forms.Label();
		this.lblMarkupLoan = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label26 = new System.Windows.Forms.Label();
		this.lblToOperatingExpense = new System.Windows.Forms.Label();
		this.lblTotOperExp = new System.Windows.Forms.Label();
		this.lblotherexpense = new System.Windows.Forms.Label();
		this.lblSellingExp = new System.Windows.Forms.Label();
		this.lblAdmiExp = new System.Windows.Forms.Label();
		this.lblOperExp = new System.Windows.Forms.Label();
		this.lblotherexpenseamt = new System.Windows.Forms.Label();
		this.lblsellingexpamt = new System.Windows.Forms.Label();
		this.lbladministratorexpamt = new System.Windows.Forms.Label();
		this.lbloperatingexpenseamt = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label21 = new System.Windows.Forms.Label();
		this.lblToNonOperatingIncome = new System.Windows.Forms.Label();
		this.lblNoOperIncomeTotal = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.lblotherincomeamt = new System.Windows.Forms.Label();
		this.lblDiscountAllowedAmount = new System.Windows.Forms.Label();
		this.lblotherincome = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label7 = new System.Windows.Forms.Label();
		this.lblGrossprofit = new System.Windows.Forms.Label();
		this.groupBox10 = new System.Windows.Forms.GroupBox();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.lblGrosProf = new System.Windows.Forms.Label();
		this.lblSalesnet = new System.Windows.Forms.Label();
		this.groupBox9 = new System.Windows.Forms.GroupBox();
		this.label47 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.lblcostofsale = new System.Windows.Forms.Label();
		this.lblsalesnetamt = new System.Windows.Forms.Label();
		this.lblcostofsaleamt = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.lblBranch = new System.Windows.Forms.Label();
		this.cmbbranch = new UltraCombo();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox14.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox10.SuspendLayout();
		this.groupBox9.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).BeginInit();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(746, 25);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(95, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Profit Loss";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 52);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(746, 752);
		this.panel4.TabIndex = 5;
		this.panel6.AutoScroll = true;
		this.panel6.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.panel6.Controls.Add(this.lblNetProfitFoterAmt);
		this.panel6.Controls.Add(this.label14);
		this.panel6.Controls.Add(this.lblTotalExpFoterAmt);
		this.panel6.Controls.Add(this.label12);
		this.panel6.Controls.Add(this.lblTotalIncomeFoterAmt);
		this.panel6.Controls.Add(this.lblToOperatingProfit);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Controls.Add(this.lblTotOperatProfit);
		this.panel6.Controls.Add(this.lblTotalProfit);
		this.panel6.Controls.Add(this.lblBeforTaxProfit);
		this.panel6.Controls.Add(this.lblToProfit);
		this.panel6.Controls.Add(this.label17);
		this.panel6.Controls.Add(this.lblProfitAfterTax);
		this.panel6.Controls.Add(this.lblProfitBeforeTax);
		this.panel6.Controls.Add(this.groupBox14);
		this.panel6.Controls.Add(this.groupBox4);
		this.panel6.Controls.Add(this.groupBox3);
		this.panel6.Controls.Add(this.groupBox2);
		this.panel6.Controls.Add(this.groupBox1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 54);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(746, 698);
		this.panel6.TabIndex = 108;
		this.lblNetProfitFoterAmt.BackColor = System.Drawing.Color.Teal;
		this.lblNetProfitFoterAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblNetProfitFoterAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNetProfitFoterAmt.ForeColor = System.Drawing.Color.White;
		this.lblNetProfitFoterAmt.Location = new System.Drawing.Point(176, 661);
		this.lblNetProfitFoterAmt.Name = "lblNetProfitFoterAmt";
		this.lblNetProfitFoterAmt.Size = new System.Drawing.Size(126, 18);
		this.lblNetProfitFoterAmt.TabIndex = 146;
		this.lblNetProfitFoterAmt.Text = "0";
		this.lblNetProfitFoterAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label14.BackColor = System.Drawing.Color.LightBlue;
		this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label14.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(42, 661);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(133, 18);
		this.label14.TabIndex = 145;
		this.label14.Text = "Net Profit";
		this.lblTotalExpFoterAmt.BackColor = System.Drawing.Color.Teal;
		this.lblTotalExpFoterAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotalExpFoterAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalExpFoterAmt.ForeColor = System.Drawing.Color.White;
		this.lblTotalExpFoterAmt.Location = new System.Drawing.Point(176, 641);
		this.lblTotalExpFoterAmt.Name = "lblTotalExpFoterAmt";
		this.lblTotalExpFoterAmt.Size = new System.Drawing.Size(126, 18);
		this.lblTotalExpFoterAmt.TabIndex = 144;
		this.lblTotalExpFoterAmt.Text = "0";
		this.lblTotalExpFoterAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label12.BackColor = System.Drawing.Color.LightBlue;
		this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label12.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Black;
		this.label12.Location = new System.Drawing.Point(42, 641);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(133, 18);
		this.label12.TabIndex = 143;
		this.label12.Text = "Total Expense";
		this.label12.Click += new System.EventHandler(label12_Click);
		this.lblTotalIncomeFoterAmt.BackColor = System.Drawing.Color.Teal;
		this.lblTotalIncomeFoterAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotalIncomeFoterAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalIncomeFoterAmt.ForeColor = System.Drawing.Color.White;
		this.lblTotalIncomeFoterAmt.Location = new System.Drawing.Point(176, 620);
		this.lblTotalIncomeFoterAmt.Name = "lblTotalIncomeFoterAmt";
		this.lblTotalIncomeFoterAmt.Size = new System.Drawing.Size(126, 18);
		this.lblTotalIncomeFoterAmt.TabIndex = 139;
		this.lblTotalIncomeFoterAmt.Text = "0";
		this.lblTotalIncomeFoterAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblToOperatingProfit.BackColor = System.Drawing.Color.Teal;
		this.lblToOperatingProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToOperatingProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToOperatingProfit.ForeColor = System.Drawing.Color.White;
		this.lblToOperatingProfit.Location = new System.Drawing.Point(563, 389);
		this.lblToOperatingProfit.Name = "lblToOperatingProfit";
		this.lblToOperatingProfit.Size = new System.Drawing.Size(167, 23);
		this.lblToOperatingProfit.TabIndex = 142;
		this.lblToOperatingProfit.Text = "0";
		this.lblToOperatingProfit.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label3.BackColor = System.Drawing.Color.LightBlue;
		this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(42, 620);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(133, 18);
		this.label3.TabIndex = 138;
		this.label3.Text = "Total Income";
		this.lblTotOperatProfit.BackColor = System.Drawing.Color.SkyBlue;
		this.lblTotOperatProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotOperatProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTotOperatProfit.ForeColor = System.Drawing.Color.Black;
		this.lblTotOperatProfit.Location = new System.Drawing.Point(376, 389);
		this.lblTotOperatProfit.Name = "lblTotOperatProfit";
		this.lblTotOperatProfit.Size = new System.Drawing.Size(186, 23);
		this.lblTotOperatProfit.TabIndex = 141;
		this.lblTotOperatProfit.Text = "Total Operating Profit";
		this.lblTotalProfit.BackColor = System.Drawing.Color.Teal;
		this.lblTotalProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotalProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTotalProfit.ForeColor = System.Drawing.Color.White;
		this.lblTotalProfit.Location = new System.Drawing.Point(563, 200);
		this.lblTotalProfit.Name = "lblTotalProfit";
		this.lblTotalProfit.Size = new System.Drawing.Size(167, 23);
		this.lblTotalProfit.TabIndex = 140;
		this.lblTotalProfit.Text = "0";
		this.lblTotalProfit.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblBeforTaxProfit.BackColor = System.Drawing.Color.SkyBlue;
		this.lblBeforTaxProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblBeforTaxProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblBeforTaxProfit.ForeColor = System.Drawing.Color.Black;
		this.lblBeforTaxProfit.Location = new System.Drawing.Point(376, 534);
		this.lblBeforTaxProfit.Name = "lblBeforTaxProfit";
		this.lblBeforTaxProfit.Size = new System.Drawing.Size(186, 23);
		this.lblBeforTaxProfit.TabIndex = 138;
		this.lblBeforTaxProfit.Text = "Profit Before Taxation";
		this.lblToProfit.BackColor = System.Drawing.Color.SkyBlue;
		this.lblToProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToProfit.ForeColor = System.Drawing.Color.Black;
		this.lblToProfit.Location = new System.Drawing.Point(376, 200);
		this.lblToProfit.Name = "lblToProfit";
		this.lblToProfit.Size = new System.Drawing.Size(186, 23);
		this.lblToProfit.TabIndex = 139;
		this.lblToProfit.Text = "Total Profit";
		this.label17.BackColor = System.Drawing.Color.SkyBlue;
		this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label17.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Black;
		this.label17.Location = new System.Drawing.Point(376, 616);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(186, 23);
		this.label17.TabIndex = 126;
		this.label17.Text = "Profit After Taxation";
		this.lblProfitAfterTax.BackColor = System.Drawing.Color.Teal;
		this.lblProfitAfterTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblProfitAfterTax.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblProfitAfterTax.ForeColor = System.Drawing.Color.White;
		this.lblProfitAfterTax.Location = new System.Drawing.Point(563, 616);
		this.lblProfitAfterTax.Name = "lblProfitAfterTax";
		this.lblProfitAfterTax.Size = new System.Drawing.Size(170, 23);
		this.lblProfitAfterTax.TabIndex = 117;
		this.lblProfitAfterTax.Text = "0";
		this.lblProfitAfterTax.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblProfitBeforeTax.BackColor = System.Drawing.Color.Teal;
		this.lblProfitBeforeTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblProfitBeforeTax.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblProfitBeforeTax.ForeColor = System.Drawing.Color.White;
		this.lblProfitBeforeTax.Location = new System.Drawing.Point(563, 534);
		this.lblProfitBeforeTax.Name = "lblProfitBeforeTax";
		this.lblProfitBeforeTax.Size = new System.Drawing.Size(167, 23);
		this.lblProfitBeforeTax.TabIndex = 139;
		this.lblProfitBeforeTax.Text = "0";
		this.lblProfitBeforeTax.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.groupBox14.Controls.Add(this.lblTaxexpense);
		this.groupBox14.Controls.Add(this.lbltaxexpenseamt);
		this.groupBox14.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox14.ForeColor = System.Drawing.Color.MediumBlue;
		this.groupBox14.Location = new System.Drawing.Point(39, 562);
		this.groupBox14.Name = "groupBox14";
		this.groupBox14.Size = new System.Drawing.Size(694, 51);
		this.groupBox14.TabIndex = 127;
		this.groupBox14.TabStop = false;
		this.groupBox14.Text = "Taxation";
		this.lblTaxexpense.BackColor = System.Drawing.Color.Teal;
		this.lblTaxexpense.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxexpense.ForeColor = System.Drawing.Color.White;
		this.lblTaxexpense.Location = new System.Drawing.Point(4, 26);
		this.lblTaxexpense.Name = "lblTaxexpense";
		this.lblTaxexpense.Size = new System.Drawing.Size(263, 19);
		this.lblTaxexpense.TabIndex = 107;
		this.lblTaxexpense.Text = "Tax Expenses";
		this.lbltaxexpenseamt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltaxexpenseamt.ForeColor = System.Drawing.Color.Black;
		this.lbltaxexpenseamt.Location = new System.Drawing.Point(529, 26);
		this.lbltaxexpenseamt.Name = "lbltaxexpenseamt";
		this.lbltaxexpenseamt.Size = new System.Drawing.Size(159, 21);
		this.lbltaxexpenseamt.TabIndex = 116;
		this.lbltaxexpenseamt.Text = "0";
		this.lbltaxexpenseamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.groupBox4.Controls.Add(this.label29);
		this.groupBox4.Controls.Add(this.lblTotalFinancialExpense);
		this.groupBox4.Controls.Add(this.lblTotFinanExp);
		this.groupBox4.Controls.Add(this.lblFinanExp);
		this.groupBox4.Controls.Add(this.lblfinancialexpamt);
		this.groupBox4.Controls.Add(this.lblmarkuploadAmount);
		this.groupBox4.Controls.Add(this.lblMarkupLoan);
		this.groupBox4.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold);
		this.groupBox4.ForeColor = System.Drawing.Color.MediumBlue;
		this.groupBox4.Location = new System.Drawing.Point(39, 421);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(694, 105);
		this.groupBox4.TabIndex = 130;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Financial Expense";
		this.label29.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label29.Location = new System.Drawing.Point(4, 73);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(688, 3);
		this.label29.TabIndex = 137;
		this.lblTotalFinancialExpense.BackColor = System.Drawing.Color.Teal;
		this.lblTotalFinancialExpense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotalFinancialExpense.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTotalFinancialExpense.ForeColor = System.Drawing.Color.White;
		this.lblTotalFinancialExpense.Location = new System.Drawing.Point(524, 76);
		this.lblTotalFinancialExpense.Name = "lblTotalFinancialExpense";
		this.lblTotalFinancialExpense.Size = new System.Drawing.Size(167, 23);
		this.lblTotalFinancialExpense.TabIndex = 118;
		this.lblTotalFinancialExpense.Text = "0";
		this.lblTotalFinancialExpense.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTotFinanExp.BackColor = System.Drawing.Color.LightBlue;
		this.lblTotFinanExp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotFinanExp.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTotFinanExp.ForeColor = System.Drawing.Color.Black;
		this.lblTotFinanExp.Location = new System.Drawing.Point(337, 76);
		this.lblTotFinanExp.Name = "lblTotFinanExp";
		this.lblTotFinanExp.Size = new System.Drawing.Size(186, 23);
		this.lblTotFinanExp.TabIndex = 117;
		this.lblTotFinanExp.Text = "Total Financial Expense";
		this.lblFinanExp.BackColor = System.Drawing.Color.Teal;
		this.lblFinanExp.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFinanExp.ForeColor = System.Drawing.Color.White;
		this.lblFinanExp.Location = new System.Drawing.Point(4, 30);
		this.lblFinanExp.Name = "lblFinanExp";
		this.lblFinanExp.Size = new System.Drawing.Size(263, 19);
		this.lblFinanExp.TabIndex = 110;
		this.lblFinanExp.Text = "Financial Expenses";
		this.lblfinancialexpamt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblfinancialexpamt.ForeColor = System.Drawing.Color.Black;
		this.lblfinancialexpamt.Location = new System.Drawing.Point(529, 30);
		this.lblfinancialexpamt.Name = "lblfinancialexpamt";
		this.lblfinancialexpamt.Size = new System.Drawing.Size(159, 21);
		this.lblfinancialexpamt.TabIndex = 121;
		this.lblfinancialexpamt.Text = "0";
		this.lblfinancialexpamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblmarkuploadAmount.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblmarkuploadAmount.ForeColor = System.Drawing.Color.Black;
		this.lblmarkuploadAmount.Location = new System.Drawing.Point(529, 57);
		this.lblmarkuploadAmount.Name = "lblmarkuploadAmount";
		this.lblmarkuploadAmount.Size = new System.Drawing.Size(159, 21);
		this.lblmarkuploadAmount.TabIndex = 125;
		this.lblmarkuploadAmount.Text = "0";
		this.lblmarkuploadAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblMarkupLoan.BackColor = System.Drawing.Color.Teal;
		this.lblMarkupLoan.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMarkupLoan.ForeColor = System.Drawing.Color.White;
		this.lblMarkupLoan.Location = new System.Drawing.Point(4, 53);
		this.lblMarkupLoan.Name = "lblMarkupLoan";
		this.lblMarkupLoan.Size = new System.Drawing.Size(263, 19);
		this.lblMarkupLoan.TabIndex = 124;
		this.lblMarkupLoan.Text = "Mark Up On Loans ";
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.lblToOperatingExpense);
		this.groupBox3.Controls.Add(this.lblTotOperExp);
		this.groupBox3.Controls.Add(this.lblotherexpense);
		this.groupBox3.Controls.Add(this.lblSellingExp);
		this.groupBox3.Controls.Add(this.lblAdmiExp);
		this.groupBox3.Controls.Add(this.lblOperExp);
		this.groupBox3.Controls.Add(this.lblotherexpenseamt);
		this.groupBox3.Controls.Add(this.lblsellingexpamt);
		this.groupBox3.Controls.Add(this.lbladministratorexpamt);
		this.groupBox3.Controls.Add(this.lbloperatingexpenseamt);
		this.groupBox3.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.ForeColor = System.Drawing.Color.MediumBlue;
		this.groupBox3.Location = new System.Drawing.Point(39, 227);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(694, 159);
		this.groupBox3.TabIndex = 130;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Operating Expense";
		this.label26.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label26.Location = new System.Drawing.Point(4, 129);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(687, 3);
		this.label26.TabIndex = 134;
		this.lblToOperatingExpense.BackColor = System.Drawing.Color.Teal;
		this.lblToOperatingExpense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToOperatingExpense.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToOperatingExpense.ForeColor = System.Drawing.Color.White;
		this.lblToOperatingExpense.Location = new System.Drawing.Point(524, 132);
		this.lblToOperatingExpense.Name = "lblToOperatingExpense";
		this.lblToOperatingExpense.Size = new System.Drawing.Size(167, 23);
		this.lblToOperatingExpense.TabIndex = 118;
		this.lblToOperatingExpense.Text = "0";
		this.lblToOperatingExpense.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTotOperExp.BackColor = System.Drawing.Color.LightBlue;
		this.lblTotOperExp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotOperExp.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTotOperExp.ForeColor = System.Drawing.Color.Black;
		this.lblTotOperExp.Location = new System.Drawing.Point(337, 132);
		this.lblTotOperExp.Name = "lblTotOperExp";
		this.lblTotOperExp.Size = new System.Drawing.Size(186, 23);
		this.lblTotOperExp.TabIndex = 117;
		this.lblTotOperExp.Text = "Total Operating Expense";
		this.lblotherexpense.BackColor = System.Drawing.Color.Teal;
		this.lblotherexpense.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblotherexpense.ForeColor = System.Drawing.Color.White;
		this.lblotherexpense.Location = new System.Drawing.Point(4, 33);
		this.lblotherexpense.Name = "lblotherexpense";
		this.lblotherexpense.Size = new System.Drawing.Size(263, 19);
		this.lblotherexpense.TabIndex = 106;
		this.lblotherexpense.Text = "Other Expenses";
		this.lblSellingExp.BackColor = System.Drawing.Color.Teal;
		this.lblSellingExp.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSellingExp.ForeColor = System.Drawing.Color.White;
		this.lblSellingExp.Location = new System.Drawing.Point(4, 54);
		this.lblSellingExp.Name = "lblSellingExp";
		this.lblSellingExp.Size = new System.Drawing.Size(263, 19);
		this.lblSellingExp.TabIndex = 108;
		this.lblSellingExp.Text = "Selling Expenses";
		this.lblAdmiExp.BackColor = System.Drawing.Color.Teal;
		this.lblAdmiExp.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAdmiExp.ForeColor = System.Drawing.Color.White;
		this.lblAdmiExp.Location = new System.Drawing.Point(4, 85);
		this.lblAdmiExp.Name = "lblAdmiExp";
		this.lblAdmiExp.Size = new System.Drawing.Size(263, 19);
		this.lblAdmiExp.TabIndex = 109;
		this.lblAdmiExp.Text = "Administration Expnses";
		this.lblOperExp.BackColor = System.Drawing.Color.Teal;
		this.lblOperExp.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOperExp.ForeColor = System.Drawing.Color.White;
		this.lblOperExp.Location = new System.Drawing.Point(4, 108);
		this.lblOperExp.Name = "lblOperExp";
		this.lblOperExp.Size = new System.Drawing.Size(263, 19);
		this.lblOperExp.TabIndex = 112;
		this.lblOperExp.Text = "Operating Expense";
		this.lblotherexpenseamt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblotherexpenseamt.ForeColor = System.Drawing.Color.Black;
		this.lblotherexpenseamt.Location = new System.Drawing.Point(529, 33);
		this.lblotherexpenseamt.Name = "lblotherexpenseamt";
		this.lblotherexpenseamt.Size = new System.Drawing.Size(159, 21);
		this.lblotherexpenseamt.TabIndex = 115;
		this.lblotherexpenseamt.Text = "0";
		this.lblotherexpenseamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblsellingexpamt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblsellingexpamt.ForeColor = System.Drawing.Color.Black;
		this.lblsellingexpamt.Location = new System.Drawing.Point(529, 58);
		this.lblsellingexpamt.Name = "lblsellingexpamt";
		this.lblsellingexpamt.Size = new System.Drawing.Size(159, 21);
		this.lblsellingexpamt.TabIndex = 117;
		this.lblsellingexpamt.Text = "0";
		this.lblsellingexpamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lbladministratorexpamt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbladministratorexpamt.ForeColor = System.Drawing.Color.Black;
		this.lbladministratorexpamt.Location = new System.Drawing.Point(529, 85);
		this.lbladministratorexpamt.Name = "lbladministratorexpamt";
		this.lbladministratorexpamt.Size = new System.Drawing.Size(159, 21);
		this.lbladministratorexpamt.TabIndex = 118;
		this.lbladministratorexpamt.Text = "0";
		this.lbladministratorexpamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lbloperatingexpenseamt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloperatingexpenseamt.ForeColor = System.Drawing.Color.Black;
		this.lbloperatingexpenseamt.Location = new System.Drawing.Point(529, 108);
		this.lbloperatingexpenseamt.Name = "lbloperatingexpenseamt";
		this.lbloperatingexpenseamt.Size = new System.Drawing.Size(159, 21);
		this.lbloperatingexpenseamt.TabIndex = 120;
		this.lbloperatingexpenseamt.Text = "0";
		this.lbloperatingexpenseamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.lblToNonOperatingIncome);
		this.groupBox2.Controls.Add(this.lblNoOperIncomeTotal);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.lblotherincomeamt);
		this.groupBox2.Controls.Add(this.lblDiscountAllowedAmount);
		this.groupBox2.Controls.Add(this.lblotherincome);
		this.groupBox2.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.ForeColor = System.Drawing.Color.MediumBlue;
		this.groupBox2.Location = new System.Drawing.Point(39, 93);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(694, 102);
		this.groupBox2.TabIndex = 129;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Non Operating Income";
		this.label21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label21.Location = new System.Drawing.Point(4, 72);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(689, 3);
		this.label21.TabIndex = 135;
		this.lblToNonOperatingIncome.BackColor = System.Drawing.Color.Teal;
		this.lblToNonOperatingIncome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToNonOperatingIncome.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToNonOperatingIncome.ForeColor = System.Drawing.Color.White;
		this.lblToNonOperatingIncome.Location = new System.Drawing.Point(524, 75);
		this.lblToNonOperatingIncome.Name = "lblToNonOperatingIncome";
		this.lblToNonOperatingIncome.Size = new System.Drawing.Size(167, 23);
		this.lblToNonOperatingIncome.TabIndex = 131;
		this.lblToNonOperatingIncome.Text = "0";
		this.lblToNonOperatingIncome.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblNoOperIncomeTotal.BackColor = System.Drawing.Color.LightBlue;
		this.lblNoOperIncomeTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblNoOperIncomeTotal.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblNoOperIncomeTotal.ForeColor = System.Drawing.Color.Black;
		this.lblNoOperIncomeTotal.Location = new System.Drawing.Point(337, 75);
		this.lblNoOperIncomeTotal.Name = "lblNoOperIncomeTotal";
		this.lblNoOperIncomeTotal.Size = new System.Drawing.Size(186, 23);
		this.lblNoOperIncomeTotal.TabIndex = 131;
		this.lblNoOperIncomeTotal.Text = "T. Non Operating Income";
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(4, 29);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(263, 19);
		this.label2.TabIndex = 122;
		this.label2.Text = "Discount Allowed";
		this.lblotherincomeamt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblotherincomeamt.ForeColor = System.Drawing.Color.Black;
		this.lblotherincomeamt.Location = new System.Drawing.Point(529, 52);
		this.lblotherincomeamt.Name = "lblotherincomeamt";
		this.lblotherincomeamt.Size = new System.Drawing.Size(159, 19);
		this.lblotherincomeamt.TabIndex = 128;
		this.lblotherincomeamt.Text = "0";
		this.lblotherincomeamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblDiscountAllowedAmount.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDiscountAllowedAmount.ForeColor = System.Drawing.Color.Black;
		this.lblDiscountAllowedAmount.Location = new System.Drawing.Point(529, 29);
		this.lblDiscountAllowedAmount.Name = "lblDiscountAllowedAmount";
		this.lblDiscountAllowedAmount.Size = new System.Drawing.Size(159, 23);
		this.lblDiscountAllowedAmount.TabIndex = 123;
		this.lblDiscountAllowedAmount.Text = "0";
		this.lblDiscountAllowedAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblotherincome.BackColor = System.Drawing.Color.Teal;
		this.lblotherincome.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblotherincome.ForeColor = System.Drawing.Color.White;
		this.lblotherincome.Location = new System.Drawing.Point(4, 51);
		this.lblotherincome.Name = "lblotherincome";
		this.lblotherincome.Size = new System.Drawing.Size(263, 19);
		this.lblotherincome.TabIndex = 127;
		this.lblotherincome.Text = "Other Income";
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.lblGrossprofit);
		this.groupBox1.Controls.Add(this.groupBox10);
		this.groupBox1.Controls.Add(this.lblGrosProf);
		this.groupBox1.Controls.Add(this.lblSalesnet);
		this.groupBox1.Controls.Add(this.groupBox9);
		this.groupBox1.Controls.Add(this.lblcostofsale);
		this.groupBox1.Controls.Add(this.lblsalesnetamt);
		this.groupBox1.Controls.Add(this.lblcostofsaleamt);
		this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.groupBox1.Location = new System.Drawing.Point(39, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(694, 88);
		this.groupBox1.TabIndex = 126;
		this.groupBox1.TabStop = false;
		this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label7.Location = new System.Drawing.Point(3, 58);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(689, 3);
		this.label7.TabIndex = 137;
		this.lblGrossprofit.BackColor = System.Drawing.Color.Teal;
		this.lblGrossprofit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblGrossprofit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblGrossprofit.ForeColor = System.Drawing.Color.White;
		this.lblGrossprofit.Location = new System.Drawing.Point(524, 60);
		this.lblGrossprofit.Name = "lblGrossprofit";
		this.lblGrossprofit.Size = new System.Drawing.Size(167, 23);
		this.lblGrossprofit.TabIndex = 137;
		this.lblGrossprofit.Text = "0";
		this.lblGrossprofit.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.groupBox10.Controls.Add(this.label53);
		this.groupBox10.Controls.Add(this.label54);
		this.groupBox10.Controls.Add(this.label55);
		this.groupBox10.Controls.Add(this.label56);
		this.groupBox10.Controls.Add(this.label57);
		this.groupBox10.Controls.Add(this.label58);
		this.groupBox10.Controls.Add(this.label59);
		this.groupBox10.Controls.Add(this.label60);
		this.groupBox10.Controls.Add(this.label61);
		this.groupBox10.Controls.Add(this.label62);
		this.groupBox10.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox10.Location = new System.Drawing.Point(0, 215);
		this.groupBox10.Name = "groupBox10";
		this.groupBox10.Size = new System.Drawing.Size(694, 165);
		this.groupBox10.TabIndex = 130;
		this.groupBox10.TabStop = false;
		this.groupBox10.Text = "Operating Expense";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(590, 141);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(19, 21);
		this.label53.TabIndex = 118;
		this.label53.Text = "0";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(354, 141);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(42, 21);
		this.label54.TabIndex = 117;
		this.label54.Text = "Total";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(19, 28);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(117, 21);
		this.label55.TabIndex = 106;
		this.label55.Text = "Other Expenses";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(19, 53);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(124, 21);
		this.label56.TabIndex = 108;
		this.label56.Text = "Selling Expenses";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(19, 80);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(172, 21);
		this.label57.TabIndex = 109;
		this.label57.Text = "Administration Expnses";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(19, 103);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(140, 21);
		this.label58.TabIndex = 112;
		this.label58.Text = "Operating Expense";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(591, 28);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(19, 21);
		this.label59.TabIndex = 115;
		this.label59.Text = "0";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(591, 53);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(19, 21);
		this.label60.TabIndex = 117;
		this.label60.Text = "0";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(591, 80);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(19, 21);
		this.label61.TabIndex = 118;
		this.label61.Text = "0";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(591, 103);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(19, 21);
		this.label62.TabIndex = 120;
		this.label62.Text = "0";
		this.lblGrosProf.BackColor = System.Drawing.Color.LightBlue;
		this.lblGrosProf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblGrosProf.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblGrosProf.ForeColor = System.Drawing.Color.Black;
		this.lblGrosProf.Location = new System.Drawing.Point(337, 60);
		this.lblGrosProf.Name = "lblGrosProf";
		this.lblGrosProf.Size = new System.Drawing.Size(186, 23);
		this.lblGrosProf.TabIndex = 115;
		this.lblGrosProf.Text = "Gross Profit";
		this.lblSalesnet.BackColor = System.Drawing.Color.Teal;
		this.lblSalesnet.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSalesnet.ForeColor = System.Drawing.Color.White;
		this.lblSalesnet.Location = new System.Drawing.Point(4, 14);
		this.lblSalesnet.Name = "lblSalesnet";
		this.lblSalesnet.Size = new System.Drawing.Size(263, 21);
		this.lblSalesnet.TabIndex = 104;
		this.lblSalesnet.Text = "Sales Net";
		this.groupBox9.Controls.Add(this.label47);
		this.groupBox9.Controls.Add(this.label48);
		this.groupBox9.Controls.Add(this.label49);
		this.groupBox9.Controls.Add(this.label50);
		this.groupBox9.Controls.Add(this.label51);
		this.groupBox9.Controls.Add(this.label52);
		this.groupBox9.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox9.Location = new System.Drawing.Point(0, 103);
		this.groupBox9.Name = "groupBox9";
		this.groupBox9.Size = new System.Drawing.Size(694, 105);
		this.groupBox9.TabIndex = 129;
		this.groupBox9.TabStop = false;
		this.groupBox9.Text = "Non Operating Income";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(591, 73);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(19, 21);
		this.label47.TabIndex = 118;
		this.label47.Text = "0";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(19, 26);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(131, 21);
		this.label48.TabIndex = 122;
		this.label48.Text = "Discount Allowed";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(355, 73);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(42, 21);
		this.label49.TabIndex = 117;
		this.label49.Text = "Total";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(591, 51);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(19, 21);
		this.label50.TabIndex = 128;
		this.label50.Text = "0";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(591, 26);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(19, 21);
		this.label51.TabIndex = 123;
		this.label51.Text = "0";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(19, 51);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(105, 21);
		this.label52.TabIndex = 127;
		this.label52.Text = "Other Income";
		this.lblcostofsale.BackColor = System.Drawing.Color.Teal;
		this.lblcostofsale.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblcostofsale.ForeColor = System.Drawing.Color.White;
		this.lblcostofsale.Location = new System.Drawing.Point(4, 38);
		this.lblcostofsale.Name = "lblcostofsale";
		this.lblcostofsale.Size = new System.Drawing.Size(263, 19);
		this.lblcostofsale.TabIndex = 105;
		this.lblcostofsale.Text = "Cost Of Sale";
		this.lblsalesnetamt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblsalesnetamt.ForeColor = System.Drawing.Color.Black;
		this.lblsalesnetamt.Location = new System.Drawing.Point(529, 14);
		this.lblsalesnetamt.Name = "lblsalesnetamt";
		this.lblsalesnetamt.Size = new System.Drawing.Size(159, 21);
		this.lblsalesnetamt.TabIndex = 113;
		this.lblsalesnetamt.Text = "0";
		this.lblsalesnetamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblcostofsaleamt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblcostofsaleamt.ForeColor = System.Drawing.Color.Black;
		this.lblcostofsaleamt.Location = new System.Drawing.Point(529, 38);
		this.lblcostofsaleamt.Name = "lblcostofsaleamt";
		this.lblcostofsaleamt.Size = new System.Drawing.Size(159, 19);
		this.lblcostofsaleamt.TabIndex = 114;
		this.lblcostofsaleamt.Text = "0";
		this.lblcostofsaleamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.lblBranch);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.todate);
		this.panel5.Controls.Add(this.fromdate);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(746, 54);
		this.panel5.TabIndex = 107;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(463, 24);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(52, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(125, 26);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(117, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(7, 26);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(117, 23);
		this.fromdate.TabIndex = 0;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(125, 5);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(7, 5);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(746, 804);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(746, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.print });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(746, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "166-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.lblBranch.AutoSize = true;
		this.lblBranch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranch.Location = new System.Drawing.Point(246, 5);
		this.lblBranch.Name = "lblBranch";
		this.lblBranch.Size = new System.Drawing.Size(44, 15);
		this.lblBranch.TabIndex = 153;
		this.lblBranch.Text = "Branch";
		this.cmbbranch.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbbranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Location = new System.Drawing.Point(243, 24);
		((System.Windows.Forms.Control)(object)this.cmbbranch).Name = "cmbbranch";
		((System.Windows.Forms.Control)(object)this.cmbbranch).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabIndex = 152;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.ClientSize = new System.Drawing.Size(746, 804);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ProfitLoss";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ProfitLoss_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.groupBox14.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox10.ResumeLayout(false);
		this.groupBox10.PerformLayout();
		this.groupBox9.ResumeLayout(false);
		this.groupBox9.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).EndInit();
		base.ResumeLayout(false);
	}
}

}
