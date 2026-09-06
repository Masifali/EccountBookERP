using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp.Account_Reports;

public class BalanceSheet : Form
{
	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private DataTable dt = new DataTable();

	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker todate;

	private Label label33;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel5;

	private Panel panel6;

	private Button btnshow;

	private Label lblShortTermBorrowingCaption;

	private Label lblShareCapitalCaption;

	private Label lblCapitalShareAmt;

	private Label lblShorttermBorrowingAmt;

	private Label lblSalariesPayablesAmt;

	private Label lblSalariesPayableCaption;

	private Label lblFinancialLeaseAmt;

	private Label lblfinancialLeaseCaption;

	private Label lblongTermLoansAmt;

	private Label lblLongTermLoanCaption;

	private GroupBox groupBox1;

	private Label label1;

	private GroupBox groupBox2;

	private GroupBox groupBox4;

	private Label lblToCurrLiabilitiesAmt;

	private Label label16;

	private Label label17;

	private Label lblToCapitalAndLiabilitiesAmt;

	private Label lblToNonCurrLiabilitiesAmt;

	private Label label9;

	private Label label29;

	private Label label21;

	private Label lblToOwnerEquityAmt;

	private Label label7;

	private Label label35;

	private Label lblTotalAssestsAmt;

	private GroupBox groupBox5;

	private Label lblAsAdvancesOtherAmt;

	private Label lblAdvanceOtherCaption;

	private Label lblTradeDebitorsLocalAmt;

	private Label lblTradeLocalCaption;

	private Label lblStockInTradeAmt;

	private Label lblStockTradeCaption;

	private Label label58;

	private Label lblToCurrAssestsAmt;

	private Label label60;

	private Label lblCashBankBlncCaption;

	private Label lblAdvPrePaymentsAmt;

	private Label lblCashBankAmt;

	private Label lblAdvancePrePaymCaption;

	private GroupBox groupBox6;

	private Label lblCWIPCaption;

	private Label lblAcCWIPAmt;

	private Label lblPropPlantEquipCaption;

	private Label lblAsPropertyPlantAmt;

	private Label lblLongTermInvestCaption;

	private Label lblAsLongTrmInvestmentAmt;

	private Label label71;

	private Label lblToNonCurrAssestsAmt;

	private Label label73;

	private Label lblIntangibleassetCaption;

	private Label lblIntangibleAssestAmt;

	private Label lblLoanAndAdvancesAmt;

	private Label lblLoanAndAdvanceCaption;

	private Label lblOtherPayablesAmt;

	private Label lbltradeOthPayableCaption;

	private Label lblOtherCreditorsAmt;

	private Label lblOtherCreditorCaption;

	private Label lblCreditorsSupplierAmt;

	private Label lblCreditorSuppCaption;

	private Label lblTradeCreditorsServiceAmt;

	private Label lblTradeCreditorServiceCaption;

	private Label lblTaxationAmt;

	private Label lblUnAppropriatedCaption;

	private Label lblUnAppropriatedAmt;

	private Label lblSharDepositCaption;

	private Label lblShareDepositAmt;

	private Label lblReserveCaption;

	private Label lblReserveAmt;

	private Label lblTradeDebitorExportAmt;

	private Label lblTradeExportCaption;

	private Label lblSundaryDebitorAmt;

	private Label lblSundryDebitorCaption;

	private Label lblAccuredLiabiltyAmt;

	private Label lblAccuredLiabiltyCaption;

	private Label lblCreditorStoreAmt;

	private Label lblCreditorStoreCaption;

	private Label lblStoreSpareAmt;

	private Label lblStoreSparePartsCaption;

	private ToolStripButton toolStripButton1;

	private Label lblTaxationCaption;

	private Label lblNetPLCaption;

	private Label lblNetPLAmount;

	private Panel panel7;

	private Label lblBranch;

	internal UltraCombo cmbbranch;

	public BalanceSheet(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void reset()
	{
		try
		{
			todate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void BindDatainGrid()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			dt.Rows.Clear();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, cmbbranch, "BranchName", wf);
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.ToDate = todate.Value;
			vh.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			dt = VoucherReports.AccountsBalanceSheetStandardFormatII(vh);
			double IntangibleAssets = 0.0;
			double CWIP = 0.0;
			double StockInTrade = 0.0;
			double TradeDebtorLocal = 0.0;
			double TradeDebtorsExport = 0.0;
			double SundryDebtors = 0.0;
			double CashandBankBalances = 0.0;
			double PropertyPlantEquipment = 0.0;
			double AdvanceOthers = 0.0;
			double LoanAndAdvances = 0.0;
			double Taxation = 0.0;
			double AdvancesAndPrePayments = 0.0;
			double LongTermLoans = 0.0;
			double ShortTermBorrowings = 0.0;
			double SaleriesPayables = 0.0;
			double CreditorsPaddyRice = 0.0;
			double TradeCreditorService = 0.0;
			double CreditorsStore = 0.0;
			double OtherCreditors = 0.0;
			double TradeOthersPayables = 0.0;
			double ShareCapital = 0.0;
			double LongTermInvestment = 0.0;
			double UnAppropriated = 0.0;
			double Reserves = 0.0;
			double SharedDeposit = 0.0;
			double FinancialLease = 0.0;
			double CreditorsAccruedandOtherLiabilities = 0.0;
			double StoresSparePartsandLooseTools = 0.0;
			double NetProfitLoss = 0.0;
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToDouble(dt.Rows[0]["NetProfitLoss"]) != 0.0)
				{
					NetProfitLoss = Conversion.ToDouble(dt.Rows[0]["NetProfitLoss"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 10)
				{
					IntangibleAssets += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblIntangibleassetCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 11)
				{
					CWIP += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblCWIPCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 12)
				{
					StockInTrade += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblStockTradeCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 13)
				{
					TradeDebtorLocal += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblTradeLocalCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 14)
				{
					TradeDebtorsExport += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblTradeExportCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 15)
				{
					SundryDebtors += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblSundryDebitorCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 16)
				{
					LoanAndAdvances += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblLoanAndAdvanceCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 17)
				{
					Taxation += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblTaxationCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 18)
				{
					AdvanceOthers += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblAdvanceOtherCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 19)
				{
					AdvancesAndPrePayments += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblAdvancePrePaymCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 20)
				{
					CashandBankBalances += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblCashBankBlncCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 21)
				{
					LongTermLoans += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblLongTermLoanCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 22)
				{
					ShortTermBorrowings += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblShortTermBorrowingCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 23)
				{
					SaleriesPayables += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblSalariesPayableCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 24)
				{
					CreditorsPaddyRice += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblCreditorSuppCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 25)
				{
					TradeCreditorService += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblTradeCreditorServiceCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 26)
				{
					CreditorsStore += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblCreditorStoreCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 27)
				{
					OtherCreditors += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblOtherCreditorCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 28)
				{
					TradeOthersPayables += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lbltradeOthPayableCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 29)
				{
					ShareCapital += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblShareCapitalCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 30)
				{
					PropertyPlantEquipment += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblPropPlantEquipCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 31)
				{
					LongTermInvestment += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblLongTermInvestCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 34)
				{
					UnAppropriated += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblUnAppropriatedCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 35)
				{
					Reserves += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblReserveCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 36)
				{
					SharedDeposit += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblSharDepositCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 37)
				{
					FinancialLease += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblfinancialLeaseCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 38)
				{
					CreditorsAccruedandOtherLiabilities += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblAccuredLiabiltyCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["AccountNoteId"]) == 39)
				{
					StoresSparePartsandLooseTools += Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lblStoreSparePartsCaption.Text = Conversion.ToString(dt.Rows[i]["AccountsNotes"]);
				}
			}
			lblCapitalShareAmt.Text = ShareCapital.ToString(clsGlobalVariables.stringFormatboth);
			lblShareDepositAmt.Text = SharedDeposit.ToString(clsGlobalVariables.stringFormatboth);
			lblUnAppropriatedAmt.Text = UnAppropriated.ToString(clsGlobalVariables.stringFormatboth);
			lblReserveAmt.Text = Reserves.ToString(clsGlobalVariables.stringFormatboth);
			lblNetPLAmount.Text = NetProfitLoss.ToString(clsGlobalVariables.stringFormatboth);
			lblFinancialLeaseAmt.Text = FinancialLease.ToString(clsGlobalVariables.stringFormatboth);
			lblongTermLoansAmt.Text = LongTermLoans.ToString(clsGlobalVariables.stringFormatboth);
			lblTaxationAmt.Text = Taxation.ToString(clsGlobalVariables.stringFormatboth);
			lblShorttermBorrowingAmt.Text = ShortTermBorrowings.ToString(clsGlobalVariables.stringFormatboth);
			lblSalariesPayablesAmt.Text = SaleriesPayables.ToString(clsGlobalVariables.stringFormatboth);
			lblCreditorsSupplierAmt.Text = CreditorsPaddyRice.ToString(clsGlobalVariables.stringFormatboth);
			lblTradeCreditorsServiceAmt.Text = TradeCreditorService.ToString(clsGlobalVariables.stringFormatboth);
			lblOtherCreditorsAmt.Text = OtherCreditors.ToString(clsGlobalVariables.stringFormatboth);
			lblOtherPayablesAmt.Text = TradeOthersPayables.ToString(clsGlobalVariables.stringFormatboth);
			lblLoanAndAdvancesAmt.Text = LoanAndAdvances.ToString(clsGlobalVariables.stringFormatboth);
			lblAccuredLiabiltyAmt.Text = CreditorsAccruedandOtherLiabilities.ToString(clsGlobalVariables.stringFormatboth);
			lblStoreSpareAmt.Text = StoresSparePartsandLooseTools.ToString(clsGlobalVariables.stringFormatboth);
			lblSundaryDebitorAmt.Text = SundryDebtors.ToString(clsGlobalVariables.stringFormatboth);
			lblCreditorStoreAmt.Text = CreditorsStore.ToString(clsGlobalVariables.stringFormatboth);
			lblIntangibleAssestAmt.Text = IntangibleAssets.ToString(clsGlobalVariables.stringFormatboth);
			lblAsLongTrmInvestmentAmt.Text = LongTermInvestment.ToString(clsGlobalVariables.stringFormatboth);
			lblAsPropertyPlantAmt.Text = PropertyPlantEquipment.ToString(clsGlobalVariables.stringFormatboth);
			lblAcCWIPAmt.Text = CWIP.ToString(clsGlobalVariables.stringFormatboth);
			lblCashBankAmt.Text = CashandBankBalances.ToString(clsGlobalVariables.stringFormatboth);
			lblAdvPrePaymentsAmt.Text = AdvancesAndPrePayments.ToString(clsGlobalVariables.stringFormatboth);
			lblStockInTradeAmt.Text = StockInTrade.ToString(clsGlobalVariables.stringFormatboth);
			lblAsAdvancesOtherAmt.Text = AdvanceOthers.ToString(clsGlobalVariables.stringFormatboth);
			lblTradeDebitorsLocalAmt.Text = TradeDebtorLocal.ToString(clsGlobalVariables.stringFormatboth);
			lblTradeDebitorExportAmt.Text = TradeDebtorsExport.ToString(clsGlobalVariables.stringFormatboth);
			lblToOwnerEquityAmt.Text = (ShareCapital + SharedDeposit + UnAppropriated + Reserves + NetProfitLoss).ToString(clsGlobalVariables.stringFormatboth);
			lblToNonCurrLiabilitiesAmt.Text = (LongTermLoans + FinancialLease + Taxation).ToString(clsGlobalVariables.stringFormatboth);
			lblToCurrLiabilitiesAmt.Text = (ShortTermBorrowings + SaleriesPayables + CreditorsPaddyRice + TradeCreditorService + OtherCreditors + CreditorsAccruedandOtherLiabilities + TradeOthersPayables + LoanAndAdvances + CreditorsStore).ToString(clsGlobalVariables.stringFormatboth);
			lblToCapitalAndLiabilitiesAmt.Text = (LongTermLoans + FinancialLease + Taxation + ShortTermBorrowings + SaleriesPayables + CreditorsPaddyRice + TradeCreditorService + OtherCreditors + CreditorsAccruedandOtherLiabilities + TradeOthersPayables + LoanAndAdvances + CreditorsStore + ShareCapital + SharedDeposit + UnAppropriated + Reserves + NetProfitLoss).ToString(clsGlobalVariables.stringFormatboth);
			lblToNonCurrAssestsAmt.Text = (IntangibleAssets + (LongTermInvestment + (PropertyPlantEquipment + CWIP))).ToString(clsGlobalVariables.stringFormatboth);
			lblToCurrAssestsAmt.Text = (CashandBankBalances + AdvancesAndPrePayments + StockInTrade + SundryDebtors + StoresSparePartsandLooseTools + AdvanceOthers + TradeDebtorLocal + TradeDebtorsExport).ToString(clsGlobalVariables.stringFormatboth);
			lblTotalAssestsAmt.Text = (CashandBankBalances + SundryDebtors + StoresSparePartsandLooseTools + AdvancesAndPrePayments + StockInTrade + AdvanceOthers + TradeDebtorLocal + TradeDebtorsExport + IntangibleAssets + LongTermInvestment + PropertyPlantEquipment + CWIP).ToString(clsGlobalVariables.stringFormatboth);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			BindDatainGrid();
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.AccountsBalanceSheetStandardForCrystal(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				ToDate = Conversion.ToDateTime((object)todate.Value),
				Ids = "1,2,3",
				BranchesId = (BranchFeature ? Conversion.ToInt(cmbbranch.Value) : 0)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "153-BalanceSheetStatementRpt.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BalanceSheet_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			todate.Focus();
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
			if (!BranchFeature)
			{
				lblBranch.Visible = false;
				((Control)(object)cmbbranch).Visible = false;
				btnshow.Location = new Point(194, 7);
			}
			else
			{
				BranchFill();
			}
			BindDatainGrid();
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

	private void AccountNotefilter(string Notes)
	{
		try
		{
			DataRow[] dr = dt.Select("AccountsNotes='" + Notes + "'");
			if (dr.Length != 0)
			{
				int NoteId = Conversion.ToInt(dr[0].ItemArray[5]);
				GetBsBreakup(NoteId, dt);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetBsBreakup(int NoteId, DataTable dtpl)
	{
		try
		{
			BSandPLBreakup bSandPLBreakup = new BSandPLBreakup(UserAccount, dtpl);
			bSandPLBreakup.ToDate = todate.Value;
			bSandPLBreakup.NoteId = NoteId;
			bSandPLBreakup.ShowDialog();
			bSandPLBreakup.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblAdvancePrePaymCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblAdvancePrePaymCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblStockTradeCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblStockTradeCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblAdvanceOtherCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblAdvanceOtherCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblTradeLocalCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblTradeLocalCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblTradeExportCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblTradeExportCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblSundryDebitorCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblSundryDebitorCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblStoreSparePartsCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblStoreSparePartsCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblIntangibleassetCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblIntangibleassetCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblLongTermInvestCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblLongTermInvestCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblPropPlantEquipCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblPropPlantEquipCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblCWIPCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblCWIPCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblShareCapitalCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblShareCapitalCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblSharDepositCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblSharDepositCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblUnAppropriatedCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblUnAppropriatedCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblReserveCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblReserveCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblfinancialLeaseCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblfinancialLeaseCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblLongTermLoanCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblLongTermLoanCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblTaxationCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblTaxationCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblShortTermBorrowingCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblShortTermBorrowingCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblSalariesPayableCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblSalariesPayableCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblCreditorSuppCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblCreditorSuppCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblTradeCreditorServiceCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblTradeCreditorServiceCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblOtherCreditorCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblOtherCreditorCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lbltradeOthPayableCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lbltradeOthPayableCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblLoanAndAdvanceCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblLoanAndAdvanceCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblCreditorStoreCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblCreditorStoreCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblAccuredLiabiltyCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblAccuredLiabiltyCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblCashBankBlncCaption_Click(object sender, EventArgs e)
	{
		try
		{
			AccountNotefilter(Conversion.ToString((object)lblCashBankBlncCaption.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChangeCaptionColorOnLeave(Label Caption)
	{
		try
		{
			Caption.ForeColor = Color.White;
			Caption.BackColor = Color.Teal;
			Caption.BorderStyle = BorderStyle.None;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
	}

	private void ChangeCaptionColorOnHover(Label Caption)
	{
		try
		{
			Caption.ForeColor = Color.Blue;
			Caption.BackColor = Color.White;
			Caption.BorderStyle = BorderStyle.None;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
	}

	private void lblCashBankBlncCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblCashBankBlncCaption);
	}

	private void lblCashBankBlncCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblCashBankBlncCaption);
	}

	private void lblAdvancePrePaymCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblAdvancePrePaymCaption);
	}

	private void lblAdvancePrePaymCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblAdvancePrePaymCaption);
	}

	private void lblStockTradeCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblStockTradeCaption);
	}

	private void lblStockTradeCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblStockTradeCaption);
	}

	private void lblAdvanceOtherCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblAdvanceOtherCaption);
	}

	private void lblAdvanceOtherCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblAdvanceOtherCaption);
	}

	private void lblTradeLocalCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblTradeLocalCaption);
	}

	private void lblTradeLocalCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblTradeLocalCaption);
	}

	private void lblTradeExportCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblTradeExportCaption);
	}

	private void lblTradeExportCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblTradeExportCaption);
	}

	private void lblSundryDebitorCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblSundryDebitorCaption);
	}

	private void lblSundryDebitorCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblSundryDebitorCaption);
	}

	private void lblStoreSparePartsCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblStoreSparePartsCaption);
	}

	private void lblStoreSparePartsCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblStoreSparePartsCaption);
	}

	private void lblIntangibleassetCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblIntangibleassetCaption);
	}

	private void lblIntangibleassetCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblIntangibleassetCaption);
	}

	private void lblLongTermInvestCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblLongTermInvestCaption);
	}

	private void lblLongTermInvestCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblLongTermInvestCaption);
	}

	private void lblPropPlantEquipCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblPropPlantEquipCaption);
	}

	private void lblPropPlantEquipCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblPropPlantEquipCaption);
	}

	private void lblCWIPCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblCWIPCaption);
	}

	private void lblCWIPCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblCWIPCaption);
	}

	private void lblShareCapitalCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblShareCapitalCaption);
	}

	private void lblShareCapitalCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblShareCapitalCaption);
	}

	private void lblSharDepositCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblSharDepositCaption);
	}

	private void lblSharDepositCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblSharDepositCaption);
	}

	private void lblUnAppropriatedCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblUnAppropriatedCaption);
	}

	private void lblUnAppropriatedCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblUnAppropriatedCaption);
	}

	private void lblReserveCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblReserveCaption);
	}

	private void lblReserveCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblReserveCaption);
	}

	private void lblfinancialLeaseCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblfinancialLeaseCaption);
	}

	private void lblfinancialLeaseCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblfinancialLeaseCaption);
	}

	private void lblLongTermLoanCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblLongTermLoanCaption);
	}

	private void lblLongTermLoanCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblLongTermLoanCaption);
	}

	private void lblTaxationCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblTaxationCaption);
	}

	private void lblTaxationCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblTaxationCaption);
	}

	private void lblShortTermBorrowingCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblShortTermBorrowingCaption);
	}

	private void lblShortTermBorrowingCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblShortTermBorrowingCaption);
	}

	private void lblSalariesPayableCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblSalariesPayableCaption);
	}

	private void lblSalariesPayableCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblSalariesPayableCaption);
	}

	private void lblCreditorSuppCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblCreditorSuppCaption);
	}

	private void lblCreditorSuppCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblCreditorSuppCaption);
	}

	private void lblTradeCreditorServiceCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblTradeCreditorServiceCaption);
	}

	private void lblTradeCreditorServiceCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblTradeCreditorServiceCaption);
	}

	private void lblOtherCreditorCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblOtherCreditorCaption);
	}

	private void lblOtherCreditorCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblOtherCreditorCaption);
	}

	private void lbltradeOthPayableCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lbltradeOthPayableCaption);
	}

	private void lbltradeOthPayableCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lbltradeOthPayableCaption);
	}

	private void lblLoanAndAdvanceCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblLoanAndAdvanceCaption);
	}

	private void lblLoanAndAdvanceCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblLoanAndAdvanceCaption);
	}

	private void lblCreditorStoreCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblCreditorStoreCaption);
	}

	private void lblCreditorStoreCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblCreditorStoreCaption);
	}

	private void lblAccuredLiabiltyCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblAccuredLiabiltyCaption);
	}

	private void lblAccuredLiabiltyCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblAccuredLiabiltyCaption);
	}

	private void lblNetPLCaption_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblNetPLCaption);
	}

	private void lblNetPLCaption_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblNetPLCaption);
	}

	private void lblTotalAssestsAmt_Click(object sender, EventArgs e)
	{
	}

	private void label35_Click(object sender, EventArgs e)
	{
	}

	private void label17_Click(object sender, EventArgs e)
	{
	}

	private void lblNetPLCaption_Click(object sender, EventArgs e)
	{
		try
		{
			new frmProfitLossHararical(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BalanceSheet_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
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
		this.panel7 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.lblToCapitalAndLiabilitiesAmt = new System.Windows.Forms.Label();
		this.lblTotalAssestsAmt = new System.Windows.Forms.Label();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.lblStoreSpareAmt = new System.Windows.Forms.Label();
		this.lblStoreSparePartsCaption = new System.Windows.Forms.Label();
		this.lblSundaryDebitorAmt = new System.Windows.Forms.Label();
		this.lblSundryDebitorCaption = new System.Windows.Forms.Label();
		this.lblTradeDebitorExportAmt = new System.Windows.Forms.Label();
		this.lblTradeExportCaption = new System.Windows.Forms.Label();
		this.lblAsAdvancesOtherAmt = new System.Windows.Forms.Label();
		this.lblAdvanceOtherCaption = new System.Windows.Forms.Label();
		this.lblTradeDebitorsLocalAmt = new System.Windows.Forms.Label();
		this.lblTradeLocalCaption = new System.Windows.Forms.Label();
		this.lblStockInTradeAmt = new System.Windows.Forms.Label();
		this.lblStockTradeCaption = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.lblToCurrAssestsAmt = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.lblCashBankBlncCaption = new System.Windows.Forms.Label();
		this.lblAdvPrePaymentsAmt = new System.Windows.Forms.Label();
		this.lblCashBankAmt = new System.Windows.Forms.Label();
		this.lblAdvancePrePaymCaption = new System.Windows.Forms.Label();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.lblCWIPCaption = new System.Windows.Forms.Label();
		this.lblAcCWIPAmt = new System.Windows.Forms.Label();
		this.lblPropPlantEquipCaption = new System.Windows.Forms.Label();
		this.lblAsPropertyPlantAmt = new System.Windows.Forms.Label();
		this.lblLongTermInvestCaption = new System.Windows.Forms.Label();
		this.lblAsLongTrmInvestmentAmt = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.lblToNonCurrAssestsAmt = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.lblIntangibleassetCaption = new System.Windows.Forms.Label();
		this.lblIntangibleAssestAmt = new System.Windows.Forms.Label();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.lblAccuredLiabiltyAmt = new System.Windows.Forms.Label();
		this.lblAccuredLiabiltyCaption = new System.Windows.Forms.Label();
		this.lblCreditorStoreAmt = new System.Windows.Forms.Label();
		this.lblCreditorStoreCaption = new System.Windows.Forms.Label();
		this.lblLoanAndAdvancesAmt = new System.Windows.Forms.Label();
		this.lblLoanAndAdvanceCaption = new System.Windows.Forms.Label();
		this.lblOtherPayablesAmt = new System.Windows.Forms.Label();
		this.lbltradeOthPayableCaption = new System.Windows.Forms.Label();
		this.lblOtherCreditorsAmt = new System.Windows.Forms.Label();
		this.lblOtherCreditorCaption = new System.Windows.Forms.Label();
		this.lblCreditorsSupplierAmt = new System.Windows.Forms.Label();
		this.lblCreditorSuppCaption = new System.Windows.Forms.Label();
		this.lblTradeCreditorsServiceAmt = new System.Windows.Forms.Label();
		this.lblTradeCreditorServiceCaption = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.lblToCurrLiabilitiesAmt = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.lblShortTermBorrowingCaption = new System.Windows.Forms.Label();
		this.lblShorttermBorrowingAmt = new System.Windows.Forms.Label();
		this.lblSalariesPayablesAmt = new System.Windows.Forms.Label();
		this.lblSalariesPayableCaption = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.lblTaxationAmt = new System.Windows.Forms.Label();
		this.lblTaxationCaption = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.lblToNonCurrLiabilitiesAmt = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.lblfinancialLeaseCaption = new System.Windows.Forms.Label();
		this.lblongTermLoansAmt = new System.Windows.Forms.Label();
		this.lblFinancialLeaseAmt = new System.Windows.Forms.Label();
		this.lblLongTermLoanCaption = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.lblNetPLCaption = new System.Windows.Forms.Label();
		this.lblNetPLAmount = new System.Windows.Forms.Label();
		this.lblUnAppropriatedCaption = new System.Windows.Forms.Label();
		this.lblUnAppropriatedAmt = new System.Windows.Forms.Label();
		this.lblSharDepositCaption = new System.Windows.Forms.Label();
		this.lblShareDepositAmt = new System.Windows.Forms.Label();
		this.lblReserveCaption = new System.Windows.Forms.Label();
		this.lblReserveAmt = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.lblToOwnerEquityAmt = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.lblShareCapitalCaption = new System.Windows.Forms.Label();
		this.lblCapitalShareAmt = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblBranch = new System.Windows.Forms.Label();
		this.cmbbranch = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox5.SuspendLayout();
		this.groupBox6.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(936, 25);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(125, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Balance Sheet";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 52);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(936, 682);
		this.panel4.TabIndex = 5;
		this.panel6.AutoScroll = true;
		this.panel6.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.groupBox5);
		this.panel6.Controls.Add(this.groupBox6);
		this.panel6.Controls.Add(this.groupBox4);
		this.panel6.Controls.Add(this.groupBox2);
		this.panel6.Controls.Add(this.groupBox1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 36);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(936, 646);
		this.panel6.TabIndex = 108;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label17);
		this.panel7.Controls.Add(this.label35);
		this.panel7.Controls.Add(this.lblToCapitalAndLiabilitiesAmt);
		this.panel7.Controls.Add(this.lblTotalAssestsAmt);
		this.panel7.Location = new System.Drawing.Point(0, 600);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(904, 33);
		this.panel7.TabIndex = 150;
		this.label17.BackColor = System.Drawing.Color.Transparent;
		this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12.25f, System.Drawing.FontStyle.Bold);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(25, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(255, 23);
		this.label17.TabIndex = 126;
		this.label17.Text = "Total Capital and Liabilites";
		this.label17.Click += new System.EventHandler(label17_Click);
		this.label35.BackColor = System.Drawing.Color.Transparent;
		this.label35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label35.Font = new System.Drawing.Font("Tahoma", 12.25f, System.Drawing.FontStyle.Bold);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(502, 4);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(216, 23);
		this.label35.TabIndex = 132;
		this.label35.Text = "Total Assets";
		this.label35.Click += new System.EventHandler(label35_Click);
		this.lblToCapitalAndLiabilitiesAmt.BackColor = System.Drawing.Color.Teal;
		this.lblToCapitalAndLiabilitiesAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToCapitalAndLiabilitiesAmt.Font = new System.Drawing.Font("Tahoma", 12.25f, System.Drawing.FontStyle.Bold);
		this.lblToCapitalAndLiabilitiesAmt.ForeColor = System.Drawing.Color.White;
		this.lblToCapitalAndLiabilitiesAmt.Location = new System.Drawing.Point(324, 4);
		this.lblToCapitalAndLiabilitiesAmt.Name = "lblToCapitalAndLiabilitiesAmt";
		this.lblToCapitalAndLiabilitiesAmt.Size = new System.Drawing.Size(157, 23);
		this.lblToCapitalAndLiabilitiesAmt.TabIndex = 117;
		this.lblToCapitalAndLiabilitiesAmt.Text = "0";
		this.lblToCapitalAndLiabilitiesAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTotalAssestsAmt.BackColor = System.Drawing.Color.Teal;
		this.lblTotalAssestsAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotalAssestsAmt.Font = new System.Drawing.Font("Tahoma", 12.25f, System.Drawing.FontStyle.Bold);
		this.lblTotalAssestsAmt.ForeColor = System.Drawing.Color.White;
		this.lblTotalAssestsAmt.Location = new System.Drawing.Point(744, 4);
		this.lblTotalAssestsAmt.Name = "lblTotalAssestsAmt";
		this.lblTotalAssestsAmt.Size = new System.Drawing.Size(156, 23);
		this.lblTotalAssestsAmt.TabIndex = 131;
		this.lblTotalAssestsAmt.Text = "0";
		this.lblTotalAssestsAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTotalAssestsAmt.Click += new System.EventHandler(lblTotalAssestsAmt_Click);
		this.groupBox5.Controls.Add(this.lblStoreSpareAmt);
		this.groupBox5.Controls.Add(this.lblStoreSparePartsCaption);
		this.groupBox5.Controls.Add(this.lblSundaryDebitorAmt);
		this.groupBox5.Controls.Add(this.lblSundryDebitorCaption);
		this.groupBox5.Controls.Add(this.lblTradeDebitorExportAmt);
		this.groupBox5.Controls.Add(this.lblTradeExportCaption);
		this.groupBox5.Controls.Add(this.lblAsAdvancesOtherAmt);
		this.groupBox5.Controls.Add(this.lblAdvanceOtherCaption);
		this.groupBox5.Controls.Add(this.lblTradeDebitorsLocalAmt);
		this.groupBox5.Controls.Add(this.lblTradeLocalCaption);
		this.groupBox5.Controls.Add(this.lblStockInTradeAmt);
		this.groupBox5.Controls.Add(this.lblStockTradeCaption);
		this.groupBox5.Controls.Add(this.label58);
		this.groupBox5.Controls.Add(this.lblToCurrAssestsAmt);
		this.groupBox5.Controls.Add(this.label60);
		this.groupBox5.Controls.Add(this.lblCashBankBlncCaption);
		this.groupBox5.Controls.Add(this.lblAdvPrePaymentsAmt);
		this.groupBox5.Controls.Add(this.lblCashBankAmt);
		this.groupBox5.Controls.Add(this.lblAdvancePrePaymCaption);
		this.groupBox5.Font = new System.Drawing.Font("Calibri", 14f, System.Drawing.FontStyle.Bold);
		this.groupBox5.ForeColor = System.Drawing.Color.MediumBlue;
		this.groupBox5.Location = new System.Drawing.Point(496, 196);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(408, 399);
		this.groupBox5.TabIndex = 134;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Current assets";
		this.lblStoreSpareAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStoreSpareAmt.ForeColor = System.Drawing.Color.Black;
		this.lblStoreSpareAmt.Location = new System.Drawing.Point(256, 184);
		this.lblStoreSpareAmt.Name = "lblStoreSpareAmt";
		this.lblStoreSpareAmt.Size = new System.Drawing.Size(145, 19);
		this.lblStoreSpareAmt.TabIndex = 149;
		this.lblStoreSpareAmt.Text = "0";
		this.lblStoreSpareAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblStoreSparePartsCaption.BackColor = System.Drawing.Color.Teal;
		this.lblStoreSparePartsCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblStoreSparePartsCaption.ForeColor = System.Drawing.Color.White;
		this.lblStoreSparePartsCaption.Location = new System.Drawing.Point(6, 183);
		this.lblStoreSparePartsCaption.Name = "lblStoreSparePartsCaption";
		this.lblStoreSparePartsCaption.Size = new System.Drawing.Size(199, 38);
		this.lblStoreSparePartsCaption.TabIndex = 148;
		this.lblStoreSparePartsCaption.Text = "Store Spare Parts and Loose Tools";
		this.lblStoreSparePartsCaption.Click += new System.EventHandler(lblStoreSparePartsCaption_Click);
		this.lblStoreSparePartsCaption.MouseLeave += new System.EventHandler(lblStoreSparePartsCaption_MouseLeave);
		this.lblStoreSparePartsCaption.MouseHover += new System.EventHandler(lblStoreSparePartsCaption_MouseHover);
		this.lblSundaryDebitorAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSundaryDebitorAmt.ForeColor = System.Drawing.Color.Black;
		this.lblSundaryDebitorAmt.Location = new System.Drawing.Point(256, 160);
		this.lblSundaryDebitorAmt.Name = "lblSundaryDebitorAmt";
		this.lblSundaryDebitorAmt.Size = new System.Drawing.Size(145, 19);
		this.lblSundaryDebitorAmt.TabIndex = 147;
		this.lblSundaryDebitorAmt.Text = "0";
		this.lblSundaryDebitorAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblSundryDebitorCaption.BackColor = System.Drawing.Color.Teal;
		this.lblSundryDebitorCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblSundryDebitorCaption.ForeColor = System.Drawing.Color.White;
		this.lblSundryDebitorCaption.Location = new System.Drawing.Point(6, 159);
		this.lblSundryDebitorCaption.Name = "lblSundryDebitorCaption";
		this.lblSundryDebitorCaption.Size = new System.Drawing.Size(199, 19);
		this.lblSundryDebitorCaption.TabIndex = 146;
		this.lblSundryDebitorCaption.Text = "Sundry Debitors";
		this.lblSundryDebitorCaption.Click += new System.EventHandler(lblSundryDebitorCaption_Click);
		this.lblSundryDebitorCaption.MouseLeave += new System.EventHandler(lblSundryDebitorCaption_MouseLeave);
		this.lblSundryDebitorCaption.MouseHover += new System.EventHandler(lblSundryDebitorCaption_MouseHover);
		this.lblTradeDebitorExportAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTradeDebitorExportAmt.ForeColor = System.Drawing.Color.Black;
		this.lblTradeDebitorExportAmt.Location = new System.Drawing.Point(255, 139);
		this.lblTradeDebitorExportAmt.Name = "lblTradeDebitorExportAmt";
		this.lblTradeDebitorExportAmt.Size = new System.Drawing.Size(145, 19);
		this.lblTradeDebitorExportAmt.TabIndex = 145;
		this.lblTradeDebitorExportAmt.Text = "0";
		this.lblTradeDebitorExportAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTradeExportCaption.BackColor = System.Drawing.Color.Teal;
		this.lblTradeExportCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTradeExportCaption.ForeColor = System.Drawing.Color.White;
		this.lblTradeExportCaption.Location = new System.Drawing.Point(6, 138);
		this.lblTradeExportCaption.Name = "lblTradeExportCaption";
		this.lblTradeExportCaption.Size = new System.Drawing.Size(199, 19);
		this.lblTradeExportCaption.TabIndex = 144;
		this.lblTradeExportCaption.Text = "Trade Debtors -Export";
		this.lblTradeExportCaption.Click += new System.EventHandler(lblTradeExportCaption_Click);
		this.lblTradeExportCaption.MouseLeave += new System.EventHandler(lblTradeExportCaption_MouseLeave);
		this.lblTradeExportCaption.MouseHover += new System.EventHandler(lblTradeExportCaption_MouseHover);
		this.lblAsAdvancesOtherAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAsAdvancesOtherAmt.ForeColor = System.Drawing.Color.Black;
		this.lblAsAdvancesOtherAmt.Location = new System.Drawing.Point(255, 94);
		this.lblAsAdvancesOtherAmt.Name = "lblAsAdvancesOtherAmt";
		this.lblAsAdvancesOtherAmt.Size = new System.Drawing.Size(145, 19);
		this.lblAsAdvancesOtherAmt.TabIndex = 143;
		this.lblAsAdvancesOtherAmt.Text = "0";
		this.lblAsAdvancesOtherAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblAdvanceOtherCaption.BackColor = System.Drawing.Color.Teal;
		this.lblAdvanceOtherCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblAdvanceOtherCaption.ForeColor = System.Drawing.Color.White;
		this.lblAdvanceOtherCaption.Location = new System.Drawing.Point(6, 93);
		this.lblAdvanceOtherCaption.Name = "lblAdvanceOtherCaption";
		this.lblAdvanceOtherCaption.Size = new System.Drawing.Size(199, 19);
		this.lblAdvanceOtherCaption.TabIndex = 142;
		this.lblAdvanceOtherCaption.Text = "Advance Others";
		this.lblAdvanceOtherCaption.Click += new System.EventHandler(lblAdvanceOtherCaption_Click);
		this.lblAdvanceOtherCaption.MouseLeave += new System.EventHandler(lblAdvanceOtherCaption_MouseLeave);
		this.lblAdvanceOtherCaption.MouseHover += new System.EventHandler(lblAdvanceOtherCaption_MouseHover);
		this.lblTradeDebitorsLocalAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTradeDebitorsLocalAmt.ForeColor = System.Drawing.Color.Black;
		this.lblTradeDebitorsLocalAmt.Location = new System.Drawing.Point(255, 116);
		this.lblTradeDebitorsLocalAmt.Name = "lblTradeDebitorsLocalAmt";
		this.lblTradeDebitorsLocalAmt.Size = new System.Drawing.Size(145, 19);
		this.lblTradeDebitorsLocalAmt.TabIndex = 141;
		this.lblTradeDebitorsLocalAmt.Text = "0";
		this.lblTradeDebitorsLocalAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTradeLocalCaption.BackColor = System.Drawing.Color.Teal;
		this.lblTradeLocalCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTradeLocalCaption.ForeColor = System.Drawing.Color.White;
		this.lblTradeLocalCaption.Location = new System.Drawing.Point(6, 115);
		this.lblTradeLocalCaption.Name = "lblTradeLocalCaption";
		this.lblTradeLocalCaption.Size = new System.Drawing.Size(199, 19);
		this.lblTradeLocalCaption.TabIndex = 140;
		this.lblTradeLocalCaption.Text = "Trade Debtors -Local";
		this.lblTradeLocalCaption.Click += new System.EventHandler(lblTradeLocalCaption_Click);
		this.lblTradeLocalCaption.MouseLeave += new System.EventHandler(lblTradeLocalCaption_MouseLeave);
		this.lblTradeLocalCaption.MouseHover += new System.EventHandler(lblTradeLocalCaption_MouseHover);
		this.lblStockInTradeAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStockInTradeAmt.ForeColor = System.Drawing.Color.Black;
		this.lblStockInTradeAmt.Location = new System.Drawing.Point(255, 71);
		this.lblStockInTradeAmt.Name = "lblStockInTradeAmt";
		this.lblStockInTradeAmt.Size = new System.Drawing.Size(145, 19);
		this.lblStockInTradeAmt.TabIndex = 139;
		this.lblStockInTradeAmt.Text = "0";
		this.lblStockInTradeAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblStockTradeCaption.AccessibleName = "12";
		this.lblStockTradeCaption.BackColor = System.Drawing.Color.Teal;
		this.lblStockTradeCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblStockTradeCaption.ForeColor = System.Drawing.Color.White;
		this.lblStockTradeCaption.Location = new System.Drawing.Point(6, 70);
		this.lblStockTradeCaption.Name = "lblStockTradeCaption";
		this.lblStockTradeCaption.Size = new System.Drawing.Size(199, 19);
		this.lblStockTradeCaption.TabIndex = 138;
		this.lblStockTradeCaption.Text = "Stock - In -Trade";
		this.lblStockTradeCaption.Click += new System.EventHandler(lblStockTradeCaption_Click);
		this.lblStockTradeCaption.MouseLeave += new System.EventHandler(lblStockTradeCaption_MouseLeave);
		this.lblStockTradeCaption.MouseHover += new System.EventHandler(lblStockTradeCaption_MouseHover);
		this.label58.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label58.Location = new System.Drawing.Point(4, 318);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(402, 2);
		this.label58.TabIndex = 135;
		this.lblToCurrAssestsAmt.BackColor = System.Drawing.Color.Teal;
		this.lblToCurrAssestsAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToCurrAssestsAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToCurrAssestsAmt.ForeColor = System.Drawing.Color.White;
		this.lblToCurrAssestsAmt.Location = new System.Drawing.Point(253, 321);
		this.lblToCurrAssestsAmt.Name = "lblToCurrAssestsAmt";
		this.lblToCurrAssestsAmt.Size = new System.Drawing.Size(150, 23);
		this.lblToCurrAssestsAmt.TabIndex = 131;
		this.lblToCurrAssestsAmt.Text = "0";
		this.lblToCurrAssestsAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label60.BackColor = System.Drawing.Color.LightBlue;
		this.label60.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label60.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label60.ForeColor = System.Drawing.Color.Black;
		this.label60.Location = new System.Drawing.Point(6, 321);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(210, 23);
		this.label60.TabIndex = 131;
		this.label60.Text = "Total Current Assests";
		this.lblCashBankBlncCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCashBankBlncCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblCashBankBlncCaption.ForeColor = System.Drawing.Color.White;
		this.lblCashBankBlncCaption.Location = new System.Drawing.Point(6, 26);
		this.lblCashBankBlncCaption.Name = "lblCashBankBlncCaption";
		this.lblCashBankBlncCaption.Size = new System.Drawing.Size(199, 19);
		this.lblCashBankBlncCaption.TabIndex = 122;
		this.lblCashBankBlncCaption.Text = "Cash and Bank Balances";
		this.lblCashBankBlncCaption.Click += new System.EventHandler(lblCashBankBlncCaption_Click);
		this.lblCashBankBlncCaption.MouseLeave += new System.EventHandler(lblCashBankBlncCaption_MouseLeave);
		this.lblCashBankBlncCaption.MouseHover += new System.EventHandler(lblCashBankBlncCaption_MouseHover);
		this.lblAdvPrePaymentsAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAdvPrePaymentsAmt.ForeColor = System.Drawing.Color.Black;
		this.lblAdvPrePaymentsAmt.Location = new System.Drawing.Point(255, 49);
		this.lblAdvPrePaymentsAmt.Name = "lblAdvPrePaymentsAmt";
		this.lblAdvPrePaymentsAmt.Size = new System.Drawing.Size(145, 19);
		this.lblAdvPrePaymentsAmt.TabIndex = 128;
		this.lblAdvPrePaymentsAmt.Text = "0";
		this.lblAdvPrePaymentsAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCashBankAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCashBankAmt.ForeColor = System.Drawing.Color.Black;
		this.lblCashBankAmt.Location = new System.Drawing.Point(255, 26);
		this.lblCashBankAmt.Name = "lblCashBankAmt";
		this.lblCashBankAmt.Size = new System.Drawing.Size(145, 19);
		this.lblCashBankAmt.TabIndex = 123;
		this.lblCashBankAmt.Text = "0";
		this.lblCashBankAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblAdvancePrePaymCaption.BackColor = System.Drawing.Color.Teal;
		this.lblAdvancePrePaymCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblAdvancePrePaymCaption.ForeColor = System.Drawing.Color.White;
		this.lblAdvancePrePaymCaption.Location = new System.Drawing.Point(6, 48);
		this.lblAdvancePrePaymCaption.Name = "lblAdvancePrePaymCaption";
		this.lblAdvancePrePaymCaption.Size = new System.Drawing.Size(199, 19);
		this.lblAdvancePrePaymCaption.TabIndex = 127;
		this.lblAdvancePrePaymCaption.Text = "Advances & PrePayments";
		this.lblAdvancePrePaymCaption.Click += new System.EventHandler(lblAdvancePrePaymCaption_Click);
		this.lblAdvancePrePaymCaption.MouseLeave += new System.EventHandler(lblAdvancePrePaymCaption_MouseLeave);
		this.lblAdvancePrePaymCaption.MouseHover += new System.EventHandler(lblAdvancePrePaymCaption_MouseHover);
		this.groupBox6.Controls.Add(this.lblCWIPCaption);
		this.groupBox6.Controls.Add(this.lblAcCWIPAmt);
		this.groupBox6.Controls.Add(this.lblPropPlantEquipCaption);
		this.groupBox6.Controls.Add(this.lblAsPropertyPlantAmt);
		this.groupBox6.Controls.Add(this.lblLongTermInvestCaption);
		this.groupBox6.Controls.Add(this.lblAsLongTrmInvestmentAmt);
		this.groupBox6.Controls.Add(this.label71);
		this.groupBox6.Controls.Add(this.lblToNonCurrAssestsAmt);
		this.groupBox6.Controls.Add(this.label73);
		this.groupBox6.Controls.Add(this.lblIntangibleassetCaption);
		this.groupBox6.Controls.Add(this.lblIntangibleAssestAmt);
		this.groupBox6.Font = new System.Drawing.Font("Calibri", 14f, System.Drawing.FontStyle.Bold);
		this.groupBox6.ForeColor = System.Drawing.Color.FromArgb(0, 0, 192);
		this.groupBox6.Location = new System.Drawing.Point(496, 5);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(408, 185);
		this.groupBox6.TabIndex = 133;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Non Current Assets";
		this.lblCWIPCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCWIPCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblCWIPCaption.ForeColor = System.Drawing.Color.White;
		this.lblCWIPCaption.Location = new System.Drawing.Point(6, 106);
		this.lblCWIPCaption.Name = "lblCWIPCaption";
		this.lblCWIPCaption.Size = new System.Drawing.Size(199, 21);
		this.lblCWIPCaption.TabIndex = 144;
		this.lblCWIPCaption.Text = "Capital Work In Progress";
		this.lblCWIPCaption.Click += new System.EventHandler(lblCWIPCaption_Click);
		this.lblCWIPCaption.MouseLeave += new System.EventHandler(lblCWIPCaption_MouseLeave);
		this.lblCWIPCaption.MouseHover += new System.EventHandler(lblCWIPCaption_MouseHover);
		this.lblAcCWIPAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAcCWIPAmt.ForeColor = System.Drawing.Color.Black;
		this.lblAcCWIPAmt.Location = new System.Drawing.Point(255, 106);
		this.lblAcCWIPAmt.Name = "lblAcCWIPAmt";
		this.lblAcCWIPAmt.Size = new System.Drawing.Size(145, 19);
		this.lblAcCWIPAmt.TabIndex = 145;
		this.lblAcCWIPAmt.Text = "0";
		this.lblAcCWIPAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblPropPlantEquipCaption.BackColor = System.Drawing.Color.Teal;
		this.lblPropPlantEquipCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblPropPlantEquipCaption.ForeColor = System.Drawing.Color.White;
		this.lblPropPlantEquipCaption.Location = new System.Drawing.Point(6, 81);
		this.lblPropPlantEquipCaption.Name = "lblPropPlantEquipCaption";
		this.lblPropPlantEquipCaption.Size = new System.Drawing.Size(199, 21);
		this.lblPropPlantEquipCaption.TabIndex = 142;
		this.lblPropPlantEquipCaption.Text = "Property, Plant  Equipment";
		this.lblPropPlantEquipCaption.Click += new System.EventHandler(lblPropPlantEquipCaption_Click);
		this.lblPropPlantEquipCaption.MouseLeave += new System.EventHandler(lblPropPlantEquipCaption_MouseLeave);
		this.lblPropPlantEquipCaption.MouseHover += new System.EventHandler(lblPropPlantEquipCaption_MouseHover);
		this.lblAsPropertyPlantAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAsPropertyPlantAmt.ForeColor = System.Drawing.Color.Black;
		this.lblAsPropertyPlantAmt.Location = new System.Drawing.Point(255, 81);
		this.lblAsPropertyPlantAmt.Name = "lblAsPropertyPlantAmt";
		this.lblAsPropertyPlantAmt.Size = new System.Drawing.Size(145, 19);
		this.lblAsPropertyPlantAmt.TabIndex = 143;
		this.lblAsPropertyPlantAmt.Text = "0";
		this.lblAsPropertyPlantAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblLongTermInvestCaption.BackColor = System.Drawing.Color.Teal;
		this.lblLongTermInvestCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblLongTermInvestCaption.ForeColor = System.Drawing.Color.White;
		this.lblLongTermInvestCaption.Location = new System.Drawing.Point(6, 56);
		this.lblLongTermInvestCaption.Name = "lblLongTermInvestCaption";
		this.lblLongTermInvestCaption.Size = new System.Drawing.Size(199, 21);
		this.lblLongTermInvestCaption.TabIndex = 140;
		this.lblLongTermInvestCaption.Text = "Long Term Investment";
		this.lblLongTermInvestCaption.Click += new System.EventHandler(lblLongTermInvestCaption_Click);
		this.lblLongTermInvestCaption.MouseLeave += new System.EventHandler(lblLongTermInvestCaption_MouseLeave);
		this.lblLongTermInvestCaption.MouseHover += new System.EventHandler(lblLongTermInvestCaption_MouseHover);
		this.lblAsLongTrmInvestmentAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAsLongTrmInvestmentAmt.ForeColor = System.Drawing.Color.Black;
		this.lblAsLongTrmInvestmentAmt.Location = new System.Drawing.Point(255, 56);
		this.lblAsLongTrmInvestmentAmt.Name = "lblAsLongTrmInvestmentAmt";
		this.lblAsLongTrmInvestmentAmt.Size = new System.Drawing.Size(145, 19);
		this.lblAsLongTrmInvestmentAmt.TabIndex = 141;
		this.lblAsLongTrmInvestmentAmt.Text = "0";
		this.lblAsLongTrmInvestmentAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label71.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label71.Location = new System.Drawing.Point(6, 129);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(398, 3);
		this.label71.TabIndex = 137;
		this.lblToNonCurrAssestsAmt.BackColor = System.Drawing.Color.Teal;
		this.lblToNonCurrAssestsAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToNonCurrAssestsAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToNonCurrAssestsAmt.ForeColor = System.Drawing.Color.White;
		this.lblToNonCurrAssestsAmt.Location = new System.Drawing.Point(253, 131);
		this.lblToNonCurrAssestsAmt.Name = "lblToNonCurrAssestsAmt";
		this.lblToNonCurrAssestsAmt.Size = new System.Drawing.Size(150, 23);
		this.lblToNonCurrAssestsAmt.TabIndex = 137;
		this.lblToNonCurrAssestsAmt.Text = "0";
		this.lblToNonCurrAssestsAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label73.BackColor = System.Drawing.Color.LightBlue;
		this.label73.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label73.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label73.ForeColor = System.Drawing.Color.Black;
		this.label73.Location = new System.Drawing.Point(6, 131);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(210, 23);
		this.label73.TabIndex = 115;
		this.label73.Text = "Total Non Current Assest";
		this.lblIntangibleassetCaption.BackColor = System.Drawing.Color.Teal;
		this.lblIntangibleassetCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblIntangibleassetCaption.ForeColor = System.Drawing.Color.White;
		this.lblIntangibleassetCaption.Location = new System.Drawing.Point(6, 31);
		this.lblIntangibleassetCaption.Name = "lblIntangibleassetCaption";
		this.lblIntangibleassetCaption.Size = new System.Drawing.Size(199, 21);
		this.lblIntangibleassetCaption.TabIndex = 104;
		this.lblIntangibleassetCaption.Text = "Intangible Assets";
		this.lblIntangibleassetCaption.Click += new System.EventHandler(lblIntangibleassetCaption_Click);
		this.lblIntangibleassetCaption.MouseLeave += new System.EventHandler(lblIntangibleassetCaption_MouseLeave);
		this.lblIntangibleassetCaption.MouseHover += new System.EventHandler(lblIntangibleassetCaption_MouseHover);
		this.lblIntangibleAssestAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblIntangibleAssestAmt.ForeColor = System.Drawing.Color.Black;
		this.lblIntangibleAssestAmt.Location = new System.Drawing.Point(255, 31);
		this.lblIntangibleAssestAmt.Name = "lblIntangibleAssestAmt";
		this.lblIntangibleAssestAmt.Size = new System.Drawing.Size(145, 19);
		this.lblIntangibleAssestAmt.TabIndex = 113;
		this.lblIntangibleAssestAmt.Text = "0";
		this.lblIntangibleAssestAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.groupBox4.Controls.Add(this.lblAccuredLiabiltyAmt);
		this.groupBox4.Controls.Add(this.lblAccuredLiabiltyCaption);
		this.groupBox4.Controls.Add(this.lblCreditorStoreAmt);
		this.groupBox4.Controls.Add(this.lblCreditorStoreCaption);
		this.groupBox4.Controls.Add(this.lblLoanAndAdvancesAmt);
		this.groupBox4.Controls.Add(this.lblLoanAndAdvanceCaption);
		this.groupBox4.Controls.Add(this.lblOtherPayablesAmt);
		this.groupBox4.Controls.Add(this.lbltradeOthPayableCaption);
		this.groupBox4.Controls.Add(this.lblOtherCreditorsAmt);
		this.groupBox4.Controls.Add(this.lblOtherCreditorCaption);
		this.groupBox4.Controls.Add(this.lblCreditorsSupplierAmt);
		this.groupBox4.Controls.Add(this.lblCreditorSuppCaption);
		this.groupBox4.Controls.Add(this.lblTradeCreditorsServiceAmt);
		this.groupBox4.Controls.Add(this.lblTradeCreditorServiceCaption);
		this.groupBox4.Controls.Add(this.label29);
		this.groupBox4.Controls.Add(this.lblToCurrLiabilitiesAmt);
		this.groupBox4.Controls.Add(this.label16);
		this.groupBox4.Controls.Add(this.lblShortTermBorrowingCaption);
		this.groupBox4.Controls.Add(this.lblShorttermBorrowingAmt);
		this.groupBox4.Controls.Add(this.lblSalariesPayablesAmt);
		this.groupBox4.Controls.Add(this.lblSalariesPayableCaption);
		this.groupBox4.Font = new System.Drawing.Font("Calibri", 14f, System.Drawing.FontStyle.Bold);
		this.groupBox4.ForeColor = System.Drawing.Color.MediumBlue;
		this.groupBox4.Location = new System.Drawing.Point(22, 319);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(468, 276);
		this.groupBox4.TabIndex = 130;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Current Liabilities";
		this.lblAccuredLiabiltyAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccuredLiabiltyAmt.ForeColor = System.Drawing.Color.Black;
		this.lblAccuredLiabiltyAmt.Location = new System.Drawing.Point(312, 216);
		this.lblAccuredLiabiltyAmt.Name = "lblAccuredLiabiltyAmt";
		this.lblAccuredLiabiltyAmt.Size = new System.Drawing.Size(145, 19);
		this.lblAccuredLiabiltyAmt.TabIndex = 151;
		this.lblAccuredLiabiltyAmt.Text = "0";
		this.lblAccuredLiabiltyAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblAccuredLiabiltyCaption.BackColor = System.Drawing.Color.Teal;
		this.lblAccuredLiabiltyCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccuredLiabiltyCaption.ForeColor = System.Drawing.Color.White;
		this.lblAccuredLiabiltyCaption.Location = new System.Drawing.Point(4, 216);
		this.lblAccuredLiabiltyCaption.Name = "lblAccuredLiabiltyCaption";
		this.lblAccuredLiabiltyCaption.Size = new System.Drawing.Size(258, 21);
		this.lblAccuredLiabiltyCaption.TabIndex = 150;
		this.lblAccuredLiabiltyCaption.Text = "Accrued Liabilty";
		this.lblAccuredLiabiltyCaption.Click += new System.EventHandler(lblAccuredLiabiltyCaption_Click);
		this.lblAccuredLiabiltyCaption.MouseLeave += new System.EventHandler(lblAccuredLiabiltyCaption_MouseLeave);
		this.lblAccuredLiabiltyCaption.MouseHover += new System.EventHandler(lblAccuredLiabiltyCaption_MouseHover);
		this.lblCreditorStoreAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditorStoreAmt.ForeColor = System.Drawing.Color.Black;
		this.lblCreditorStoreAmt.Location = new System.Drawing.Point(312, 192);
		this.lblCreditorStoreAmt.Name = "lblCreditorStoreAmt";
		this.lblCreditorStoreAmt.Size = new System.Drawing.Size(145, 19);
		this.lblCreditorStoreAmt.TabIndex = 149;
		this.lblCreditorStoreAmt.Text = "0";
		this.lblCreditorStoreAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCreditorStoreCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCreditorStoreCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditorStoreCaption.ForeColor = System.Drawing.Color.White;
		this.lblCreditorStoreCaption.Location = new System.Drawing.Point(4, 192);
		this.lblCreditorStoreCaption.Name = "lblCreditorStoreCaption";
		this.lblCreditorStoreCaption.Size = new System.Drawing.Size(258, 21);
		this.lblCreditorStoreCaption.TabIndex = 148;
		this.lblCreditorStoreCaption.Text = "Creditor Store";
		this.lblCreditorStoreCaption.Click += new System.EventHandler(lblCreditorStoreCaption_Click);
		this.lblCreditorStoreCaption.MouseLeave += new System.EventHandler(lblCreditorStoreCaption_MouseLeave);
		this.lblCreditorStoreCaption.MouseHover += new System.EventHandler(lblCreditorStoreCaption_MouseHover);
		this.lblLoanAndAdvancesAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLoanAndAdvancesAmt.ForeColor = System.Drawing.Color.Black;
		this.lblLoanAndAdvancesAmt.Location = new System.Drawing.Point(312, 169);
		this.lblLoanAndAdvancesAmt.Name = "lblLoanAndAdvancesAmt";
		this.lblLoanAndAdvancesAmt.Size = new System.Drawing.Size(145, 19);
		this.lblLoanAndAdvancesAmt.TabIndex = 147;
		this.lblLoanAndAdvancesAmt.Text = "0";
		this.lblLoanAndAdvancesAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblLoanAndAdvanceCaption.BackColor = System.Drawing.Color.Teal;
		this.lblLoanAndAdvanceCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLoanAndAdvanceCaption.ForeColor = System.Drawing.Color.White;
		this.lblLoanAndAdvanceCaption.Location = new System.Drawing.Point(4, 169);
		this.lblLoanAndAdvanceCaption.Name = "lblLoanAndAdvanceCaption";
		this.lblLoanAndAdvanceCaption.Size = new System.Drawing.Size(258, 21);
		this.lblLoanAndAdvanceCaption.TabIndex = 146;
		this.lblLoanAndAdvanceCaption.Text = "Loan And Advances";
		this.lblLoanAndAdvanceCaption.Click += new System.EventHandler(lblLoanAndAdvanceCaption_Click);
		this.lblLoanAndAdvanceCaption.MouseLeave += new System.EventHandler(lblLoanAndAdvanceCaption_MouseLeave);
		this.lblLoanAndAdvanceCaption.MouseHover += new System.EventHandler(lblLoanAndAdvanceCaption_MouseHover);
		this.lblOtherPayablesAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOtherPayablesAmt.ForeColor = System.Drawing.Color.Black;
		this.lblOtherPayablesAmt.Location = new System.Drawing.Point(312, 146);
		this.lblOtherPayablesAmt.Name = "lblOtherPayablesAmt";
		this.lblOtherPayablesAmt.Size = new System.Drawing.Size(145, 19);
		this.lblOtherPayablesAmt.TabIndex = 145;
		this.lblOtherPayablesAmt.Text = "0";
		this.lblOtherPayablesAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lbltradeOthPayableCaption.BackColor = System.Drawing.Color.Teal;
		this.lbltradeOthPayableCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltradeOthPayableCaption.ForeColor = System.Drawing.Color.White;
		this.lbltradeOthPayableCaption.Location = new System.Drawing.Point(4, 146);
		this.lbltradeOthPayableCaption.Name = "lbltradeOthPayableCaption";
		this.lbltradeOthPayableCaption.Size = new System.Drawing.Size(258, 21);
		this.lbltradeOthPayableCaption.TabIndex = 144;
		this.lbltradeOthPayableCaption.Text = "Trade Others Payables";
		this.lbltradeOthPayableCaption.Click += new System.EventHandler(lbltradeOthPayableCaption_Click);
		this.lbltradeOthPayableCaption.MouseLeave += new System.EventHandler(lbltradeOthPayableCaption_MouseLeave);
		this.lbltradeOthPayableCaption.MouseHover += new System.EventHandler(lbltradeOthPayableCaption_MouseHover);
		this.lblOtherCreditorsAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOtherCreditorsAmt.ForeColor = System.Drawing.Color.Black;
		this.lblOtherCreditorsAmt.Location = new System.Drawing.Point(312, 123);
		this.lblOtherCreditorsAmt.Name = "lblOtherCreditorsAmt";
		this.lblOtherCreditorsAmt.Size = new System.Drawing.Size(145, 19);
		this.lblOtherCreditorsAmt.TabIndex = 143;
		this.lblOtherCreditorsAmt.Text = "0";
		this.lblOtherCreditorsAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblOtherCreditorCaption.BackColor = System.Drawing.Color.Teal;
		this.lblOtherCreditorCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOtherCreditorCaption.ForeColor = System.Drawing.Color.White;
		this.lblOtherCreditorCaption.Location = new System.Drawing.Point(4, 123);
		this.lblOtherCreditorCaption.Name = "lblOtherCreditorCaption";
		this.lblOtherCreditorCaption.Size = new System.Drawing.Size(258, 21);
		this.lblOtherCreditorCaption.TabIndex = 142;
		this.lblOtherCreditorCaption.Text = "Other Creditors";
		this.lblOtherCreditorCaption.Click += new System.EventHandler(lblOtherCreditorCaption_Click);
		this.lblOtherCreditorCaption.MouseLeave += new System.EventHandler(lblOtherCreditorCaption_MouseLeave);
		this.lblOtherCreditorCaption.MouseHover += new System.EventHandler(lblOtherCreditorCaption_MouseHover);
		this.lblCreditorsSupplierAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditorsSupplierAmt.ForeColor = System.Drawing.Color.Black;
		this.lblCreditorsSupplierAmt.Location = new System.Drawing.Point(312, 78);
		this.lblCreditorsSupplierAmt.Name = "lblCreditorsSupplierAmt";
		this.lblCreditorsSupplierAmt.Size = new System.Drawing.Size(145, 19);
		this.lblCreditorsSupplierAmt.TabIndex = 141;
		this.lblCreditorsSupplierAmt.Text = "0";
		this.lblCreditorsSupplierAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCreditorSuppCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCreditorSuppCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditorSuppCaption.ForeColor = System.Drawing.Color.White;
		this.lblCreditorSuppCaption.Location = new System.Drawing.Point(4, 78);
		this.lblCreditorSuppCaption.Name = "lblCreditorSuppCaption";
		this.lblCreditorSuppCaption.Size = new System.Drawing.Size(258, 21);
		this.lblCreditorSuppCaption.TabIndex = 140;
		this.lblCreditorSuppCaption.Text = "Crediters Suppliers";
		this.lblCreditorSuppCaption.Click += new System.EventHandler(lblCreditorSuppCaption_Click);
		this.lblCreditorSuppCaption.MouseLeave += new System.EventHandler(lblCreditorSuppCaption_MouseLeave);
		this.lblCreditorSuppCaption.MouseHover += new System.EventHandler(lblCreditorSuppCaption_MouseHover);
		this.lblTradeCreditorsServiceAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTradeCreditorsServiceAmt.ForeColor = System.Drawing.Color.Black;
		this.lblTradeCreditorsServiceAmt.Location = new System.Drawing.Point(312, 100);
		this.lblTradeCreditorsServiceAmt.Name = "lblTradeCreditorsServiceAmt";
		this.lblTradeCreditorsServiceAmt.Size = new System.Drawing.Size(145, 19);
		this.lblTradeCreditorsServiceAmt.TabIndex = 139;
		this.lblTradeCreditorsServiceAmt.Text = "0";
		this.lblTradeCreditorsServiceAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTradeCreditorServiceCaption.BackColor = System.Drawing.Color.Teal;
		this.lblTradeCreditorServiceCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTradeCreditorServiceCaption.ForeColor = System.Drawing.Color.White;
		this.lblTradeCreditorServiceCaption.Location = new System.Drawing.Point(4, 100);
		this.lblTradeCreditorServiceCaption.Name = "lblTradeCreditorServiceCaption";
		this.lblTradeCreditorServiceCaption.Size = new System.Drawing.Size(258, 21);
		this.lblTradeCreditorServiceCaption.TabIndex = 138;
		this.lblTradeCreditorServiceCaption.Text = "Trade Creditors -Service";
		this.lblTradeCreditorServiceCaption.Click += new System.EventHandler(lblTradeCreditorServiceCaption_Click);
		this.lblTradeCreditorServiceCaption.MouseLeave += new System.EventHandler(lblTradeCreditorServiceCaption_MouseLeave);
		this.lblTradeCreditorServiceCaption.MouseHover += new System.EventHandler(lblTradeCreditorServiceCaption_MouseHover);
		this.label29.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label29.Location = new System.Drawing.Point(4, 245);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(400, 3);
		this.label29.TabIndex = 137;
		this.lblToCurrLiabilitiesAmt.BackColor = System.Drawing.Color.Teal;
		this.lblToCurrLiabilitiesAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToCurrLiabilitiesAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToCurrLiabilitiesAmt.ForeColor = System.Drawing.Color.White;
		this.lblToCurrLiabilitiesAmt.Location = new System.Drawing.Point(309, 249);
		this.lblToCurrLiabilitiesAmt.Name = "lblToCurrLiabilitiesAmt";
		this.lblToCurrLiabilitiesAmt.Size = new System.Drawing.Size(150, 23);
		this.lblToCurrLiabilitiesAmt.TabIndex = 118;
		this.lblToCurrLiabilitiesAmt.Text = "0";
		this.lblToCurrLiabilitiesAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label16.BackColor = System.Drawing.Color.LightBlue;
		this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label16.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.Black;
		this.label16.Location = new System.Drawing.Point(4, 249);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(258, 21);
		this.label16.TabIndex = 117;
		this.label16.Text = "Total Current Liabilities";
		this.lblShortTermBorrowingCaption.BackColor = System.Drawing.Color.Teal;
		this.lblShortTermBorrowingCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblShortTermBorrowingCaption.ForeColor = System.Drawing.Color.White;
		this.lblShortTermBorrowingCaption.Location = new System.Drawing.Point(4, 32);
		this.lblShortTermBorrowingCaption.Name = "lblShortTermBorrowingCaption";
		this.lblShortTermBorrowingCaption.Size = new System.Drawing.Size(258, 21);
		this.lblShortTermBorrowingCaption.TabIndex = 110;
		this.lblShortTermBorrowingCaption.Text = "Short Term Borrowings";
		this.lblShortTermBorrowingCaption.Click += new System.EventHandler(lblShortTermBorrowingCaption_Click);
		this.lblShortTermBorrowingCaption.MouseLeave += new System.EventHandler(lblShortTermBorrowingCaption_MouseLeave);
		this.lblShortTermBorrowingCaption.MouseHover += new System.EventHandler(lblShortTermBorrowingCaption_MouseHover);
		this.lblShorttermBorrowingAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblShorttermBorrowingAmt.ForeColor = System.Drawing.Color.Black;
		this.lblShorttermBorrowingAmt.Location = new System.Drawing.Point(312, 32);
		this.lblShorttermBorrowingAmt.Name = "lblShorttermBorrowingAmt";
		this.lblShorttermBorrowingAmt.Size = new System.Drawing.Size(145, 19);
		this.lblShorttermBorrowingAmt.TabIndex = 121;
		this.lblShorttermBorrowingAmt.Text = "0";
		this.lblShorttermBorrowingAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblSalariesPayablesAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSalariesPayablesAmt.ForeColor = System.Drawing.Color.Black;
		this.lblSalariesPayablesAmt.Location = new System.Drawing.Point(312, 55);
		this.lblSalariesPayablesAmt.Name = "lblSalariesPayablesAmt";
		this.lblSalariesPayablesAmt.Size = new System.Drawing.Size(145, 19);
		this.lblSalariesPayablesAmt.TabIndex = 125;
		this.lblSalariesPayablesAmt.Text = "0";
		this.lblSalariesPayablesAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblSalariesPayableCaption.BackColor = System.Drawing.Color.Teal;
		this.lblSalariesPayableCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSalariesPayableCaption.ForeColor = System.Drawing.Color.White;
		this.lblSalariesPayableCaption.Location = new System.Drawing.Point(4, 55);
		this.lblSalariesPayableCaption.Name = "lblSalariesPayableCaption";
		this.lblSalariesPayableCaption.Size = new System.Drawing.Size(258, 21);
		this.lblSalariesPayableCaption.TabIndex = 124;
		this.lblSalariesPayableCaption.Text = "Salaries Payables";
		this.lblSalariesPayableCaption.Click += new System.EventHandler(lblSalariesPayableCaption_Click);
		this.lblSalariesPayableCaption.MouseLeave += new System.EventHandler(lblSalariesPayableCaption_MouseLeave);
		this.lblSalariesPayableCaption.MouseHover += new System.EventHandler(lblSalariesPayableCaption_MouseHover);
		this.groupBox2.Controls.Add(this.lblTaxationAmt);
		this.groupBox2.Controls.Add(this.lblTaxationCaption);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.lblToNonCurrLiabilitiesAmt);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.lblfinancialLeaseCaption);
		this.groupBox2.Controls.Add(this.lblongTermLoansAmt);
		this.groupBox2.Controls.Add(this.lblFinancialLeaseAmt);
		this.groupBox2.Controls.Add(this.lblLongTermLoanCaption);
		this.groupBox2.Font = new System.Drawing.Font("Calibri", 14f, System.Drawing.FontStyle.Bold);
		this.groupBox2.ForeColor = System.Drawing.Color.MediumBlue;
		this.groupBox2.Location = new System.Drawing.Point(22, 196);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(468, 122);
		this.groupBox2.TabIndex = 129;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Non Current Liabilities";
		this.lblTaxationAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxationAmt.ForeColor = System.Drawing.Color.Black;
		this.lblTaxationAmt.Location = new System.Drawing.Point(311, 71);
		this.lblTaxationAmt.Name = "lblTaxationAmt";
		this.lblTaxationAmt.Size = new System.Drawing.Size(145, 19);
		this.lblTaxationAmt.TabIndex = 139;
		this.lblTaxationAmt.Text = "0";
		this.lblTaxationAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTaxationCaption.BackColor = System.Drawing.Color.Teal;
		this.lblTaxationCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTaxationCaption.ForeColor = System.Drawing.Color.White;
		this.lblTaxationCaption.Location = new System.Drawing.Point(6, 70);
		this.lblTaxationCaption.Name = "lblTaxationCaption";
		this.lblTaxationCaption.Size = new System.Drawing.Size(258, 21);
		this.lblTaxationCaption.TabIndex = 138;
		this.lblTaxationCaption.Text = "Taxation";
		this.lblTaxationCaption.Click += new System.EventHandler(lblTaxationCaption_Click);
		this.lblTaxationCaption.MouseLeave += new System.EventHandler(lblTaxationCaption_MouseLeave);
		this.lblTaxationCaption.MouseHover += new System.EventHandler(lblTaxationCaption_MouseHover);
		this.label21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label21.Location = new System.Drawing.Point(5, 91);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(402, 2);
		this.label21.TabIndex = 135;
		this.lblToNonCurrLiabilitiesAmt.BackColor = System.Drawing.Color.Teal;
		this.lblToNonCurrLiabilitiesAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToNonCurrLiabilitiesAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToNonCurrLiabilitiesAmt.ForeColor = System.Drawing.Color.White;
		this.lblToNonCurrLiabilitiesAmt.Location = new System.Drawing.Point(309, 94);
		this.lblToNonCurrLiabilitiesAmt.Name = "lblToNonCurrLiabilitiesAmt";
		this.lblToNonCurrLiabilitiesAmt.Size = new System.Drawing.Size(150, 23);
		this.lblToNonCurrLiabilitiesAmt.TabIndex = 131;
		this.lblToNonCurrLiabilitiesAmt.Text = "0";
		this.lblToNonCurrLiabilitiesAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label9.BackColor = System.Drawing.Color.LightBlue;
		this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label9.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.Black;
		this.label9.Location = new System.Drawing.Point(6, 94);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(258, 21);
		this.label9.TabIndex = 131;
		this.label9.Text = "Total Non Curr Liabilities";
		this.lblfinancialLeaseCaption.BackColor = System.Drawing.Color.Teal;
		this.lblfinancialLeaseCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblfinancialLeaseCaption.ForeColor = System.Drawing.Color.White;
		this.lblfinancialLeaseCaption.Location = new System.Drawing.Point(6, 26);
		this.lblfinancialLeaseCaption.Name = "lblfinancialLeaseCaption";
		this.lblfinancialLeaseCaption.Size = new System.Drawing.Size(258, 21);
		this.lblfinancialLeaseCaption.TabIndex = 122;
		this.lblfinancialLeaseCaption.Text = "Financial Lease";
		this.lblfinancialLeaseCaption.Click += new System.EventHandler(lblfinancialLeaseCaption_Click);
		this.lblfinancialLeaseCaption.MouseLeave += new System.EventHandler(lblfinancialLeaseCaption_MouseLeave);
		this.lblfinancialLeaseCaption.MouseHover += new System.EventHandler(lblfinancialLeaseCaption_MouseHover);
		this.lblongTermLoansAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblongTermLoansAmt.ForeColor = System.Drawing.Color.Black;
		this.lblongTermLoansAmt.Location = new System.Drawing.Point(311, 49);
		this.lblongTermLoansAmt.Name = "lblongTermLoansAmt";
		this.lblongTermLoansAmt.Size = new System.Drawing.Size(145, 19);
		this.lblongTermLoansAmt.TabIndex = 128;
		this.lblongTermLoansAmt.Text = "0";
		this.lblongTermLoansAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblFinancialLeaseAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFinancialLeaseAmt.ForeColor = System.Drawing.Color.Black;
		this.lblFinancialLeaseAmt.Location = new System.Drawing.Point(311, 26);
		this.lblFinancialLeaseAmt.Name = "lblFinancialLeaseAmt";
		this.lblFinancialLeaseAmt.Size = new System.Drawing.Size(145, 19);
		this.lblFinancialLeaseAmt.TabIndex = 123;
		this.lblFinancialLeaseAmt.Text = "0";
		this.lblFinancialLeaseAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblLongTermLoanCaption.BackColor = System.Drawing.Color.Teal;
		this.lblLongTermLoanCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblLongTermLoanCaption.ForeColor = System.Drawing.Color.White;
		this.lblLongTermLoanCaption.Location = new System.Drawing.Point(6, 48);
		this.lblLongTermLoanCaption.Name = "lblLongTermLoanCaption";
		this.lblLongTermLoanCaption.Size = new System.Drawing.Size(258, 21);
		this.lblLongTermLoanCaption.TabIndex = 127;
		this.lblLongTermLoanCaption.Text = "Long Term Loans";
		this.lblLongTermLoanCaption.Click += new System.EventHandler(lblLongTermLoanCaption_Click);
		this.lblLongTermLoanCaption.MouseLeave += new System.EventHandler(lblLongTermLoanCaption_MouseLeave);
		this.lblLongTermLoanCaption.MouseHover += new System.EventHandler(lblLongTermLoanCaption_MouseHover);
		this.groupBox1.Controls.Add(this.lblNetPLCaption);
		this.groupBox1.Controls.Add(this.lblNetPLAmount);
		this.groupBox1.Controls.Add(this.lblUnAppropriatedCaption);
		this.groupBox1.Controls.Add(this.lblUnAppropriatedAmt);
		this.groupBox1.Controls.Add(this.lblSharDepositCaption);
		this.groupBox1.Controls.Add(this.lblShareDepositAmt);
		this.groupBox1.Controls.Add(this.lblReserveCaption);
		this.groupBox1.Controls.Add(this.lblReserveAmt);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.lblToOwnerEquityAmt);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.lblShareCapitalCaption);
		this.groupBox1.Controls.Add(this.lblCapitalShareAmt);
		this.groupBox1.Font = new System.Drawing.Font("Calibri", 14f, System.Drawing.FontStyle.Bold);
		this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 192);
		this.groupBox1.Location = new System.Drawing.Point(22, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(468, 185);
		this.groupBox1.TabIndex = 126;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Owner Equity";
		this.lblNetPLCaption.BackColor = System.Drawing.Color.Teal;
		this.lblNetPLCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblNetPLCaption.ForeColor = System.Drawing.Color.White;
		this.lblNetPLCaption.Location = new System.Drawing.Point(6, 130);
		this.lblNetPLCaption.Name = "lblNetPLCaption";
		this.lblNetPLCaption.Size = new System.Drawing.Size(258, 21);
		this.lblNetPLCaption.TabIndex = 144;
		this.lblNetPLCaption.Text = "Net Profit/Loss";
		this.lblNetPLCaption.Click += new System.EventHandler(lblNetPLCaption_Click);
		this.lblNetPLCaption.MouseLeave += new System.EventHandler(lblNetPLCaption_MouseLeave);
		this.lblNetPLCaption.MouseHover += new System.EventHandler(lblNetPLCaption_MouseHover);
		this.lblNetPLAmount.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNetPLAmount.ForeColor = System.Drawing.Color.Black;
		this.lblNetPLAmount.Location = new System.Drawing.Point(311, 130);
		this.lblNetPLAmount.Name = "lblNetPLAmount";
		this.lblNetPLAmount.Size = new System.Drawing.Size(145, 19);
		this.lblNetPLAmount.TabIndex = 145;
		this.lblNetPLAmount.Text = "0";
		this.lblNetPLAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblUnAppropriatedCaption.BackColor = System.Drawing.Color.Teal;
		this.lblUnAppropriatedCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblUnAppropriatedCaption.ForeColor = System.Drawing.Color.White;
		this.lblUnAppropriatedCaption.Location = new System.Drawing.Point(6, 81);
		this.lblUnAppropriatedCaption.Name = "lblUnAppropriatedCaption";
		this.lblUnAppropriatedCaption.Size = new System.Drawing.Size(258, 21);
		this.lblUnAppropriatedCaption.TabIndex = 142;
		this.lblUnAppropriatedCaption.Text = "Un-Appropriated P/L";
		this.lblUnAppropriatedCaption.Click += new System.EventHandler(lblUnAppropriatedCaption_Click);
		this.lblUnAppropriatedCaption.MouseLeave += new System.EventHandler(lblUnAppropriatedCaption_MouseLeave);
		this.lblUnAppropriatedCaption.MouseHover += new System.EventHandler(lblUnAppropriatedCaption_MouseHover);
		this.lblUnAppropriatedAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUnAppropriatedAmt.ForeColor = System.Drawing.Color.Black;
		this.lblUnAppropriatedAmt.Location = new System.Drawing.Point(311, 81);
		this.lblUnAppropriatedAmt.Name = "lblUnAppropriatedAmt";
		this.lblUnAppropriatedAmt.Size = new System.Drawing.Size(145, 19);
		this.lblUnAppropriatedAmt.TabIndex = 143;
		this.lblUnAppropriatedAmt.Text = "0";
		this.lblUnAppropriatedAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblSharDepositCaption.BackColor = System.Drawing.Color.Teal;
		this.lblSharDepositCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblSharDepositCaption.ForeColor = System.Drawing.Color.White;
		this.lblSharDepositCaption.Location = new System.Drawing.Point(6, 56);
		this.lblSharDepositCaption.Name = "lblSharDepositCaption";
		this.lblSharDepositCaption.Size = new System.Drawing.Size(258, 21);
		this.lblSharDepositCaption.TabIndex = 140;
		this.lblSharDepositCaption.Text = "Shared Deposit";
		this.lblSharDepositCaption.Click += new System.EventHandler(lblSharDepositCaption_Click);
		this.lblSharDepositCaption.MouseLeave += new System.EventHandler(lblSharDepositCaption_MouseLeave);
		this.lblSharDepositCaption.MouseHover += new System.EventHandler(lblSharDepositCaption_MouseHover);
		this.lblShareDepositAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblShareDepositAmt.ForeColor = System.Drawing.Color.Black;
		this.lblShareDepositAmt.Location = new System.Drawing.Point(311, 56);
		this.lblShareDepositAmt.Name = "lblShareDepositAmt";
		this.lblShareDepositAmt.Size = new System.Drawing.Size(145, 19);
		this.lblShareDepositAmt.TabIndex = 141;
		this.lblShareDepositAmt.Text = "0";
		this.lblShareDepositAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblReserveCaption.BackColor = System.Drawing.Color.Teal;
		this.lblReserveCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblReserveCaption.ForeColor = System.Drawing.Color.White;
		this.lblReserveCaption.Location = new System.Drawing.Point(6, 106);
		this.lblReserveCaption.Name = "lblReserveCaption";
		this.lblReserveCaption.Size = new System.Drawing.Size(258, 21);
		this.lblReserveCaption.TabIndex = 138;
		this.lblReserveCaption.Text = "Reserves";
		this.lblReserveCaption.Click += new System.EventHandler(lblReserveCaption_Click);
		this.lblReserveCaption.MouseLeave += new System.EventHandler(lblReserveCaption_MouseLeave);
		this.lblReserveCaption.MouseHover += new System.EventHandler(lblReserveCaption_MouseHover);
		this.lblReserveAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblReserveAmt.ForeColor = System.Drawing.Color.Black;
		this.lblReserveAmt.Location = new System.Drawing.Point(311, 106);
		this.lblReserveAmt.Name = "lblReserveAmt";
		this.lblReserveAmt.Size = new System.Drawing.Size(145, 19);
		this.lblReserveAmt.TabIndex = 139;
		this.lblReserveAmt.Text = "0";
		this.lblReserveAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label7.Location = new System.Drawing.Point(3, 153);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(398, 3);
		this.label7.TabIndex = 137;
		this.lblToOwnerEquityAmt.BackColor = System.Drawing.Color.Teal;
		this.lblToOwnerEquityAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToOwnerEquityAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToOwnerEquityAmt.ForeColor = System.Drawing.Color.White;
		this.lblToOwnerEquityAmt.Location = new System.Drawing.Point(309, 156);
		this.lblToOwnerEquityAmt.Name = "lblToOwnerEquityAmt";
		this.lblToOwnerEquityAmt.Size = new System.Drawing.Size(150, 23);
		this.lblToOwnerEquityAmt.TabIndex = 137;
		this.lblToOwnerEquityAmt.Text = "0";
		this.lblToOwnerEquityAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label1.BackColor = System.Drawing.Color.LightBlue;
		this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label1.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(6, 156);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(258, 23);
		this.label1.TabIndex = 115;
		this.label1.Text = "Total Owner Equity";
		this.lblShareCapitalCaption.BackColor = System.Drawing.Color.Teal;
		this.lblShareCapitalCaption.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblShareCapitalCaption.ForeColor = System.Drawing.Color.White;
		this.lblShareCapitalCaption.Location = new System.Drawing.Point(6, 31);
		this.lblShareCapitalCaption.Name = "lblShareCapitalCaption";
		this.lblShareCapitalCaption.Size = new System.Drawing.Size(258, 21);
		this.lblShareCapitalCaption.TabIndex = 104;
		this.lblShareCapitalCaption.Text = "Share Capital";
		this.lblShareCapitalCaption.Click += new System.EventHandler(lblShareCapitalCaption_Click);
		this.lblShareCapitalCaption.MouseLeave += new System.EventHandler(lblShareCapitalCaption_MouseLeave);
		this.lblShareCapitalCaption.MouseHover += new System.EventHandler(lblShareCapitalCaption_MouseHover);
		this.lblCapitalShareAmt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCapitalShareAmt.ForeColor = System.Drawing.Color.Black;
		this.lblCapitalShareAmt.Location = new System.Drawing.Point(311, 31);
		this.lblCapitalShareAmt.Name = "lblCapitalShareAmt";
		this.lblCapitalShareAmt.Size = new System.Drawing.Size(145, 19);
		this.lblCapitalShareAmt.TabIndex = 113;
		this.lblCapitalShareAmt.Text = "0";
		this.lblCapitalShareAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.lblBranch);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.todate);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(936, 36);
		this.panel5.TabIndex = 107;
		this.lblBranch.AutoSize = true;
		this.lblBranch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranch.Location = new System.Drawing.Point(194, 11);
		this.lblBranch.Name = "lblBranch";
		this.lblBranch.Size = new System.Drawing.Size(44, 15);
		this.lblBranch.TabIndex = 149;
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
		((System.Windows.Forms.Control)(object)this.cmbbranch).Location = new System.Drawing.Point(242, 5);
		((System.Windows.Forms.Control)(object)this.cmbbranch).Name = "cmbbranch";
		((System.Windows.Forms.Control)(object)this.cmbbranch).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabIndex = 1;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(448, 7);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(51, 23);
		this.btnshow.TabIndex = 2;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(59, 7);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(132, 23);
		this.todate.TabIndex = 0;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(5, 11);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(936, 734);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(936, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.toolStripButton1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(936, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(92, 24);
		this.toolStripButton1.Text = "153-Print";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(936, 734);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "BalanceSheet";
		this.Text = "Balance Sheet";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(BalanceSheet_FormClosing);
		base.Load += new System.EventHandler(BalanceSheet_Load);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox6.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
