// using Architecture.Reports;
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
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
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

public class frmProfitLossHararical : Form
{
	private WaitForm wf = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtFinal = new DataTable();

	private DataTable dt = new DataTable();

	private DataTable dtGridFromDb = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripLabel tsProfiLossAndBalanceSheet;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnNew;

	private Button btnshow;

	private DateTimePicker txtToDate;

	private DateTimePicker txtFromDate;

	private Label label27;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private RadioButton rdGroupeExpand;

	private RadioButton rdGroupCollapsed;

	private ToolStripDropDownButton tsDropDown;

	private Panel panel6;

	private Panel panel5;

	private Label lblNetProfitFoterAmt;

	private Label label14;

	private Label lblTotalExpFoterAmt;

	private Label label12;

	private Label lblTotalIncomeFoterAmt;

	private Label lblToOperatingProfit;

	private Label label3;

	private Label lblTotOperatProfit;

	private Label lblTotalProfit;

	private Label lblBeforTaxProfit;

	private Label lblToProfit;

	private Label label17;

	private Label lblProfitAfterTax;

	private Label lblProfitBeforeTax;

	private GroupBox groupBox14;

	private Label lblTaxexpense;

	private Label lbltaxexpenseamt;

	private GroupBox groupBox4;

	private Label label29;

	private Label lblTotalFinancialExpense;

	private Label lblTotFinanExp;

	private Label lblFinanExp;

	private Label lblfinancialexpamt;

	private Label lblmarkuploadAmount;

	private Label lblMarkupLoan;

	private GroupBox groupBox3;

	private Label label26;

	private Label lblToOperatingExpense;

	private Label lblTotOperExp;

	private Label lblotherexpense;

	private Label lblSellingExp;

	private Label lblAdmiExp;

	private Label lblOperExp;

	private Label lblotherexpenseamt;

	private Label lblsellingexpamt;

	private Label lbladministratorexpamt;

	private Label lbloperatingexpenseamt;

	private GroupBox groupBox2;

	private Label label21;

	private Label lblToNonOperatingIncome;

	private Label lblNoOperIncomeTotal;

	private Label lblotherincomeamt;

	private Label lblotherincome;

	private GroupBox groupBox1;

	private Label label7;

	private Label lblGrossprofit;

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

	private Label lblGrosProf;

	private Label lblSalesnet;

	private GroupBox groupBox9;

	private Label label47;

	private Label label48;

	private Label label49;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label lblcostofsale;

	private Label lblsalesnetamt;

	private Label lblcostofsaleamt;

	private ToolStripButton btnProfitAndLoss;

	private Label lblBranch;

	internal UltraCombo cmbbranch;

	private ToolStripButton btnProfitLossBreakUp;

	public frmProfitLossHararical(UserAccount userAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmTrialBalancesAllLevel_Load(object sender, EventArgs e)
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
				label27.Location = new Point(7, 20);
				txtFromDate.Location = new Point(103, 15);
				txtToDate.Location = new Point(205, 15);
				btnshow.Location = new Point(304, 15);
			}
			else
			{
				BranchFill();
			}
			rdGroupCollapsed.Checked = true;
			txtFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, cmbbranch, "BranchName", wf);
			GridBind(BranchIds);
			ProfitAndLossStatic(BranchIds);
			ReportsLoad();
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
			else
			{
				((Control)(object)cmbbranch).Text = string.Empty;
				((UltraGridBase)cmbbranch).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportsLoad()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("HararicalProfitAndLoss");
			if (files != null && files.Length != 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					tsDropDown.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[i]));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridBind(string BranchIds)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtGridFromDb.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Conversion.ToDateTime((object)txtFromDate.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDate.Value);
			obj.Activity = "4,5";
			dtGridFromDb = VoucherReports.HararicalTrialBalance(obj);
			DataTable dataTable = dtGridFromDb;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				dtFinal = new DataTable();
				dtFinal.Columns.Add("RowId", typeof(int));
				dtFinal.Columns.Add("ParentId", typeof(int));
				dtFinal.Columns.Add("AccountId", typeof(int));
				dtFinal.Columns.Add("AccountCode", typeof(string));
				dtFinal.Columns.Add("AccountTitle", typeof(string));
				dtFinal.Columns.Add("AcLevel", typeof(int));
				dtFinal.Columns.Add("Debit", typeof(double));
				dtFinal.Columns.Add("Credit", typeof(double));
				dtFinal.Columns.Add("Amount", typeof(double));
				dtFinal.Columns.Add("IsGroupDetail", typeof(string));
				int RowId = 0;
				object ParentId2ndLevel = 0;
				object ParentId3rdLevel = 0;
				object ParentId4thLevel = 0;
				object ParentId5thLevel = 0;
				for (int i = 0; i < dtGridFromDb.Rows.Count; i++)
				{
					RowId++;
					if (Conversion.ToInt(dtGridFromDb.Rows[i]["AcLevel"]) == 1)
					{
						ParentId2ndLevel = RowId;
						dtFinal.Rows.Add(RowId, DBNull.Value, dtGridFromDb.Rows[i]["AccountId"], dtGridFromDb.Rows[i]["AccountCode"], dtGridFromDb.Rows[i]["AccountTitle"], dtGridFromDb.Rows[i]["AcLevel"], Conversion.ToDouble(dtGridFromDb.Rows[i]["Debit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Credit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Closing"]), dtGridFromDb.Rows[i]["IsGroupDetail"]);
					}
					else if (Conversion.ToInt(dtGridFromDb.Rows[i]["AcLevel"]) == 2)
					{
						ParentId3rdLevel = RowId;
						dtFinal.Rows.Add(RowId, ParentId2ndLevel, dtGridFromDb.Rows[i]["AccountId"], dtGridFromDb.Rows[i]["AccountCode"], dtGridFromDb.Rows[i]["AccountTitle"], dtGridFromDb.Rows[i]["AcLevel"], Conversion.ToDouble(dtGridFromDb.Rows[i]["Debit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Credit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Closing"]), dtGridFromDb.Rows[i]["IsGroupDetail"]);
					}
					else if (Conversion.ToInt(dtGridFromDb.Rows[i]["AcLevel"]) == 3)
					{
						ParentId4thLevel = RowId;
						dtFinal.Rows.Add(RowId, ParentId3rdLevel, dtGridFromDb.Rows[i]["AccountId"], dtGridFromDb.Rows[i]["AccountCode"], dtGridFromDb.Rows[i]["AccountTitle"], dtGridFromDb.Rows[i]["AcLevel"], Conversion.ToDouble(dtGridFromDb.Rows[i]["Debit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Credit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Closing"]), dtGridFromDb.Rows[i]["IsGroupDetail"]);
					}
					else if (Conversion.ToInt(dtGridFromDb.Rows[i]["AcLevel"]) == 4)
					{
						ParentId5thLevel = RowId;
						dtFinal.Rows.Add(RowId, ParentId4thLevel, dtGridFromDb.Rows[i]["AccountId"], dtGridFromDb.Rows[i]["AccountCode"], dtGridFromDb.Rows[i]["AccountTitle"], dtGridFromDb.Rows[i]["AcLevel"], Conversion.ToDouble(dtGridFromDb.Rows[i]["Debit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Credit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Closing"]), dtGridFromDb.Rows[i]["IsGroupDetail"]);
					}
					else if (Conversion.ToInt(dtGridFromDb.Rows[i]["AcLevel"]) == 5)
					{
						dtFinal.Rows.Add(RowId, ParentId5thLevel, dtGridFromDb.Rows[i]["AccountId"], dtGridFromDb.Rows[i]["AccountCode"], dtGridFromDb.Rows[i]["AccountTitle"], dtGridFromDb.Rows[i]["AcLevel"], Conversion.ToDouble(dtGridFromDb.Rows[i]["Debit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Credit"]), Conversion.ToDouble(dtGridFromDb.Rows[i]["Closing"]), dtGridFromDb.Rows[i]["IsGroupDetail"]);
					}
				}
				grdHistory.DataSource = dtFinal;
				grdHistory.RetrieveStructure();
				grdHistorySetting();
			}
			else
			{
				grdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistorySetting()
	{
		try
		{
			grdHistory.RootTable.Columns["ParentId"].Visible = false;
			grdHistory.RootTable.Columns["RowId"].Visible = false;
			grdHistory.RootTable.Columns["AccountId"].Visible = false;
			grdHistory.RootTable.Columns["AcLevel"].Visible = false;
			grdHistory.RootTable.Columns["IsGroupDetail"].Visible = false;
			grdHistory.RootTable.Columns["Debit"].Visible = false;
			grdHistory.RootTable.Columns["Credit"].Visible = false;
			grdHistory.RootTable.Columns["AccountCode"].Visible = false;
			((ScrollableBase)grdHistory).VisualStyle = (VisualStyle)2;
			grdHistory.GridLines = (GridLines)1;
			grdHistory.HideSelection = (HideSelection)2;
			grdHistory.AllowEdit = (InheritableBoolean)2;
			grdHistory.RootTable.HierarchicalMode = (HierarchicalMode)1;
			grdHistory.RootTable.SelfReferencingSettings.ParentDataMember = "RowId";
			grdHistory.RootTable.SelfReferencingSettings.ChildDataMember = "ParentId";
			grdHistory.RootTable.SelfReferencingSettings.ExpandColumn = grdHistory.RootTable.Columns["AccountTitle"];
			grdHistory.RootTable.SelfReferencingSettings.AggregationMode = (SelfReferencingAggregationMode)1;
			if (rdGroupCollapsed.Checked)
			{
				grdHistory.CollapseRecords();
			}
			else
			{
				grdHistory.ExpandRecords();
			}
			grdHistory.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)2;
			grdHistory.RootTable.Columns["AccountTitle"].ImageIndex = 0;
			grdHistory.ColumnAutoResize = true;
			grdHistory.RootTable.Columns["AccountTitle"].Width = 350;
			grdHistory.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			ctrlGrdBar1_Load(null, null);
			AddConditionalFormatting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddConditionalFormatting()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdHistory.RootTable.Columns["IsGroupDetail"], (ConditionOperator)0, (object)"Group");
			fc.FormatStyle.FontBold = (TriState)2;
			grdHistory.RootTable.FormatConditions.Add(fc);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, cmbbranch, "BranchName", wf);
			GridBind(BranchIds);
			ProfitAndLossStatic(BranchIds);
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

	private void ProfitAndLossStatic(string BranchIds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FromDate = txtFromDate.Value;
			vh.ToDate = txtToDate.Value;
			vh.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			dt = VoucherReports.ProftLoss(vh);
			double salesNet = 0.0;
			double costofsale = 0.0;
			double otherexpense = 0.0;
			double taxexpense = 0.0;
			double sellingexpense = 0.0;
			double administratinoExpese = 0.0;
			double FinancialExpense = 0.0;
			double operatingexpense = 0.0;
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
			lblmarkuploadAmount.Text = markupLoans.ToString(clsGlobalVariables.stringFormatboth);
			lblotherincomeamt.Text = otherIncome.ToString(clsGlobalVariables.stringFormatboth);
			lblGrossprofit.Text = (salesNet + costofsale).ToString(clsGlobalVariables.stringFormatboth);
			lblToNonOperatingIncome.Text = otherIncome.ToString(clsGlobalVariables.stringFormatboth);
			lblToOperatingExpense.Text = (otherexpense + sellingexpense + administratinoExpese + operatingexpense).ToString(clsGlobalVariables.stringFormatboth);
			lblTotalFinancialExpense.Text = (FinancialExpense + markupLoans).ToString(clsGlobalVariables.stringFormatboth);
			lblTotalProfit.Text = (salesNet + costofsale + otherIncome).ToString(clsGlobalVariables.stringFormatboth);
			lblToOperatingProfit.Text = (salesNet + costofsale + otherIncome + (otherexpense + sellingexpense + administratinoExpese + operatingexpense)).ToString(clsGlobalVariables.stringFormatboth);
			lblProfitBeforeTax.Text = (salesNet + costofsale + otherIncome + (otherexpense + sellingexpense + administratinoExpese + operatingexpense) + (FinancialExpense + markupLoans)).ToString(clsGlobalVariables.stringFormatboth);
			lblProfitAfterTax.Text = (salesNet + costofsale + otherIncome + (otherexpense + sellingexpense + administratinoExpese + operatingexpense) + (FinancialExpense + markupLoans) + taxexpense).ToString(clsGlobalVariables.stringFormatboth);
			double TotalIncome = Conversion.ToDouble((object)(salesNet + otherIncome));
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtToDate.Value = DateTime.Now;
			wf.Show();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, cmbbranch, "BranchName", wf);
			GridBind(BranchIds);
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
			if (rdGroupCollapsed.Checked)
			{
				grdHistory.CollapseRecords();
			}
			else
			{
				grdHistory.ExpandRecords();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdGroupeExpand_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rdGroupeExpand.Checked)
			{
				grdHistory.ExpandRecords();
			}
			else
			{
				grdHistory.CollapseRecords();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsDropDown_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtFinal.Rows.Count > 0 && dtFinal != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtFinal, e.ClickedItem.Text + ".rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnProfitAndLoss_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherReports.Accounts_ProfitLoss_ForCrystal(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FromDate = Conversion.ToDateTime((object)txtFromDate.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				Ids = "4,5",
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
			val.ShowReportWithDataTable(dt, "166-ProfitLossNewReport.rpt");
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

	private void GetBsBreakup(int NoteId, DataTable PLData)
	{
		try
		{
			BSandPLBreakup bSandPLBreakup = new BSandPLBreakup(UserAccount, PLData);
			bSandPLBreakup.FromDate = txtFromDate.Value;
			bSandPLBreakup.ToDate = txtToDate.Value;
			bSandPLBreakup.NoteId = NoteId;
			bSandPLBreakup.Status = 1;
			bSandPLBreakup.ShowDialog();
			bSandPLBreakup.BringToFront();
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
	}

	private void lblSalesnet_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblSalesnet.Text));
	}

	private void lblcostofsale_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblcostofsale.Text));
	}

	private void lblotherincome_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblotherincome.Text));
	}

	private void lblotherexpense_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblotherexpense.Text));
	}

	private void lblSellingExp_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblSellingExp.Text));
	}

	private void lblAdmiExp_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblAdmiExp.Text));
	}

	private void lblOperExp_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblOperExp.Text));
	}

	private void lblFinanExp_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblFinanExp.Text));
	}

	private void lblMarkupLoan_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblMarkupLoan.Text));
	}

	private void lblTaxexpense_Click(object sender, EventArgs e)
	{
		AccountNotefilter(Conversion.ToString((object)lblTaxexpense.Text));
	}

	private void lblSalesnet_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblSalesnet);
	}

	private void lblSalesnet_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblSalesnet);
	}

	private void lblcostofsale_MouseMove(object sender, MouseEventArgs e)
	{
		ChangeCaptionColorOnHover(lblcostofsale);
	}

	private void lblcostofsale_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblcostofsale);
	}

	private void lblotherincome_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblotherincome);
	}

	private void lblotherincome_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblotherincome);
	}

	private void lblotherexpense_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblotherexpense);
	}

	private void lblotherexpense_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblotherexpense);
	}

	private void lblSellingExp_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblSellingExp);
	}

	private void lblSellingExp_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblSellingExp);
	}

	private void lblAdmiExp_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblAdmiExp);
	}

	private void lblAdmiExp_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblAdmiExp);
	}

	private void lblOperExp_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblOperExp);
	}

	private void lblOperExp_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblOperExp);
	}

	private void lblFinanExp_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblFinanExp);
	}

	private void lblFinanExp_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblFinanExp);
	}

	private void lblMarkupLoan_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblMarkupLoan);
	}

	private void lblMarkupLoan_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblMarkupLoan);
	}

	private void lblTaxexpense_MouseHover(object sender, EventArgs e)
	{
		ChangeCaptionColorOnHover(lblTaxexpense);
	}

	private void lblTaxexpense_MouseLeave(object sender, EventArgs e)
	{
		ChangeCaptionColorOnLeave(lblTaxexpense);
	}

	private void frmProfitLossHararical_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnProfitLossBreakUp_Click(object sender, EventArgs e)
	{
		try
		{
			frmProfitLossBreakUp obj = new frmProfitLossBreakUp(UserAccount);
			obj.FromDate = txtFromDate.Value;
			obj.ToDate = txtToDate.Value;
			obj.BringToFront();
			obj.ShowDialog();
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
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
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
		this.lblotherincomeamt = new System.Windows.Forms.Label();
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
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblBranch = new System.Windows.Forms.Label();
		this.cmbbranch = new UltraCombo();
		this.rdGroupeExpand = new System.Windows.Forms.RadioButton();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.rdGroupCollapsed = new System.Windows.Forms.RadioButton();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.tsProfiLossAndBalanceSheet = new System.Windows.Forms.ToolStripLabel();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.tsDropDown = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnProfitAndLoss = new System.Windows.Forms.ToolStripButton();
		this.btnProfitLossBreakUp = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox14.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox10.SuspendLayout();
		this.groupBox9.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1152, 707);
		this.panel1.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.lblNetProfitFoterAmt);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.lblTotalExpFoterAmt);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.lblTotalIncomeFoterAmt);
		this.panel4.Controls.Add(this.lblToOperatingProfit);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.lblTotOperatProfit);
		this.panel4.Controls.Add(this.lblTotalProfit);
		this.panel4.Controls.Add(this.lblBeforTaxProfit);
		this.panel4.Controls.Add(this.lblToProfit);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.lblProfitAfterTax);
		this.panel4.Controls.Add(this.lblProfitBeforeTax);
		this.panel4.Controls.Add(this.groupBox14);
		this.panel4.Controls.Add(this.groupBox4);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(443, 26);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(709, 681);
		this.panel4.TabIndex = 2;
		this.lblNetProfitFoterAmt.BackColor = System.Drawing.Color.Teal;
		this.lblNetProfitFoterAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblNetProfitFoterAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNetProfitFoterAmt.ForeColor = System.Drawing.Color.White;
		this.lblNetProfitFoterAmt.Location = new System.Drawing.Point(143, 658);
		this.lblNetProfitFoterAmt.Name = "lblNetProfitFoterAmt";
		this.lblNetProfitFoterAmt.Size = new System.Drawing.Size(126, 18);
		this.lblNetProfitFoterAmt.TabIndex = 165;
		this.lblNetProfitFoterAmt.Text = "0";
		this.lblNetProfitFoterAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label14.BackColor = System.Drawing.Color.LightBlue;
		this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label14.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(9, 658);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(133, 18);
		this.label14.TabIndex = 164;
		this.label14.Text = "Net Profit";
		this.lblTotalExpFoterAmt.BackColor = System.Drawing.Color.Teal;
		this.lblTotalExpFoterAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotalExpFoterAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalExpFoterAmt.ForeColor = System.Drawing.Color.White;
		this.lblTotalExpFoterAmt.Location = new System.Drawing.Point(143, 638);
		this.lblTotalExpFoterAmt.Name = "lblTotalExpFoterAmt";
		this.lblTotalExpFoterAmt.Size = new System.Drawing.Size(126, 18);
		this.lblTotalExpFoterAmt.TabIndex = 163;
		this.lblTotalExpFoterAmt.Text = "0";
		this.lblTotalExpFoterAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label12.BackColor = System.Drawing.Color.LightBlue;
		this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label12.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Black;
		this.label12.Location = new System.Drawing.Point(9, 638);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(133, 18);
		this.label12.TabIndex = 162;
		this.label12.Text = "Total Expense";
		this.lblTotalIncomeFoterAmt.BackColor = System.Drawing.Color.Teal;
		this.lblTotalIncomeFoterAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotalIncomeFoterAmt.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalIncomeFoterAmt.ForeColor = System.Drawing.Color.White;
		this.lblTotalIncomeFoterAmt.Location = new System.Drawing.Point(143, 617);
		this.lblTotalIncomeFoterAmt.Name = "lblTotalIncomeFoterAmt";
		this.lblTotalIncomeFoterAmt.Size = new System.Drawing.Size(126, 18);
		this.lblTotalIncomeFoterAmt.TabIndex = 157;
		this.lblTotalIncomeFoterAmt.Text = "0";
		this.lblTotalIncomeFoterAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblToOperatingProfit.BackColor = System.Drawing.Color.Teal;
		this.lblToOperatingProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToOperatingProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToOperatingProfit.ForeColor = System.Drawing.Color.White;
		this.lblToOperatingProfit.Location = new System.Drawing.Point(530, 390);
		this.lblToOperatingProfit.Name = "lblToOperatingProfit";
		this.lblToOperatingProfit.Size = new System.Drawing.Size(167, 23);
		this.lblToOperatingProfit.TabIndex = 161;
		this.lblToOperatingProfit.Text = "0";
		this.lblToOperatingProfit.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label3.BackColor = System.Drawing.Color.LightBlue;
		this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(9, 617);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(133, 18);
		this.label3.TabIndex = 155;
		this.label3.Text = "Total Income";
		this.lblTotOperatProfit.BackColor = System.Drawing.Color.SkyBlue;
		this.lblTotOperatProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotOperatProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTotOperatProfit.ForeColor = System.Drawing.Color.Black;
		this.lblTotOperatProfit.Location = new System.Drawing.Point(343, 390);
		this.lblTotOperatProfit.Name = "lblTotOperatProfit";
		this.lblTotOperatProfit.Size = new System.Drawing.Size(186, 23);
		this.lblTotOperatProfit.TabIndex = 160;
		this.lblTotOperatProfit.Text = "Total Operating Profit";
		this.lblTotalProfit.BackColor = System.Drawing.Color.Teal;
		this.lblTotalProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotalProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblTotalProfit.ForeColor = System.Drawing.Color.White;
		this.lblTotalProfit.Location = new System.Drawing.Point(530, 182);
		this.lblTotalProfit.Name = "lblTotalProfit";
		this.lblTotalProfit.Size = new System.Drawing.Size(167, 23);
		this.lblTotalProfit.TabIndex = 159;
		this.lblTotalProfit.Text = "0";
		this.lblTotalProfit.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblBeforTaxProfit.BackColor = System.Drawing.Color.SkyBlue;
		this.lblBeforTaxProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblBeforTaxProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblBeforTaxProfit.ForeColor = System.Drawing.Color.Black;
		this.lblBeforTaxProfit.Location = new System.Drawing.Point(343, 535);
		this.lblBeforTaxProfit.Name = "lblBeforTaxProfit";
		this.lblBeforTaxProfit.Size = new System.Drawing.Size(186, 23);
		this.lblBeforTaxProfit.TabIndex = 154;
		this.lblBeforTaxProfit.Text = "Profit Before Taxation";
		this.lblToProfit.BackColor = System.Drawing.Color.SkyBlue;
		this.lblToProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToProfit.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToProfit.ForeColor = System.Drawing.Color.Black;
		this.lblToProfit.Location = new System.Drawing.Point(343, 182);
		this.lblToProfit.Name = "lblToProfit";
		this.lblToProfit.Size = new System.Drawing.Size(186, 23);
		this.lblToProfit.TabIndex = 156;
		this.lblToProfit.Text = "Total Profit";
		this.label17.BackColor = System.Drawing.Color.SkyBlue;
		this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label17.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Black;
		this.label17.Location = new System.Drawing.Point(343, 617);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(186, 23);
		this.label17.TabIndex = 149;
		this.label17.Text = "Profit After Taxation";
		this.lblProfitAfterTax.BackColor = System.Drawing.Color.Teal;
		this.lblProfitAfterTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblProfitAfterTax.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblProfitAfterTax.ForeColor = System.Drawing.Color.White;
		this.lblProfitAfterTax.Location = new System.Drawing.Point(530, 617);
		this.lblProfitAfterTax.Name = "lblProfitAfterTax";
		this.lblProfitAfterTax.Size = new System.Drawing.Size(170, 23);
		this.lblProfitAfterTax.TabIndex = 147;
		this.lblProfitAfterTax.Text = "0";
		this.lblProfitAfterTax.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblProfitBeforeTax.BackColor = System.Drawing.Color.Teal;
		this.lblProfitBeforeTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblProfitBeforeTax.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblProfitBeforeTax.ForeColor = System.Drawing.Color.White;
		this.lblProfitBeforeTax.Location = new System.Drawing.Point(530, 535);
		this.lblProfitBeforeTax.Name = "lblProfitBeforeTax";
		this.lblProfitBeforeTax.Size = new System.Drawing.Size(167, 23);
		this.lblProfitBeforeTax.TabIndex = 158;
		this.lblProfitBeforeTax.Text = "0";
		this.lblProfitBeforeTax.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.groupBox14.Controls.Add(this.lblTaxexpense);
		this.groupBox14.Controls.Add(this.lbltaxexpenseamt);
		this.groupBox14.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox14.ForeColor = System.Drawing.Color.MediumBlue;
		this.groupBox14.Location = new System.Drawing.Point(6, 563);
		this.groupBox14.Name = "groupBox14";
		this.groupBox14.Size = new System.Drawing.Size(694, 51);
		this.groupBox14.TabIndex = 150;
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
		this.lblTaxexpense.Click += new System.EventHandler(lblTaxexpense_Click);
		this.lblTaxexpense.MouseLeave += new System.EventHandler(lblTaxexpense_MouseLeave);
		this.lblTaxexpense.MouseHover += new System.EventHandler(lblTaxexpense_MouseHover);
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
		this.groupBox4.Location = new System.Drawing.Point(6, 422);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(694, 105);
		this.groupBox4.TabIndex = 153;
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
		this.lblFinanExp.Click += new System.EventHandler(lblFinanExp_Click);
		this.lblFinanExp.MouseLeave += new System.EventHandler(lblFinanExp_MouseLeave);
		this.lblFinanExp.MouseHover += new System.EventHandler(lblFinanExp_MouseHover);
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
		this.lblMarkupLoan.Click += new System.EventHandler(lblMarkupLoan_Click);
		this.lblMarkupLoan.MouseLeave += new System.EventHandler(lblMarkupLoan_MouseLeave);
		this.lblMarkupLoan.MouseHover += new System.EventHandler(lblMarkupLoan_MouseHover);
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
		this.groupBox3.Location = new System.Drawing.Point(6, 220);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(694, 167);
		this.groupBox3.TabIndex = 152;
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
		this.lblotherexpense.Click += new System.EventHandler(lblotherexpense_Click);
		this.lblotherexpense.MouseLeave += new System.EventHandler(lblotherexpense_MouseLeave);
		this.lblotherexpense.MouseHover += new System.EventHandler(lblotherexpense_MouseHover);
		this.lblSellingExp.BackColor = System.Drawing.Color.Teal;
		this.lblSellingExp.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSellingExp.ForeColor = System.Drawing.Color.White;
		this.lblSellingExp.Location = new System.Drawing.Point(4, 54);
		this.lblSellingExp.Name = "lblSellingExp";
		this.lblSellingExp.Size = new System.Drawing.Size(263, 19);
		this.lblSellingExp.TabIndex = 108;
		this.lblSellingExp.Text = "Selling Expenses";
		this.lblSellingExp.Click += new System.EventHandler(lblSellingExp_Click);
		this.lblSellingExp.MouseLeave += new System.EventHandler(lblSellingExp_MouseLeave);
		this.lblSellingExp.MouseHover += new System.EventHandler(lblSellingExp_MouseHover);
		this.lblAdmiExp.BackColor = System.Drawing.Color.Teal;
		this.lblAdmiExp.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAdmiExp.ForeColor = System.Drawing.Color.White;
		this.lblAdmiExp.Location = new System.Drawing.Point(4, 85);
		this.lblAdmiExp.Name = "lblAdmiExp";
		this.lblAdmiExp.Size = new System.Drawing.Size(263, 19);
		this.lblAdmiExp.TabIndex = 109;
		this.lblAdmiExp.Text = "Administration Expnses";
		this.lblAdmiExp.Click += new System.EventHandler(lblAdmiExp_Click);
		this.lblAdmiExp.MouseLeave += new System.EventHandler(lblAdmiExp_MouseLeave);
		this.lblAdmiExp.MouseHover += new System.EventHandler(lblAdmiExp_MouseHover);
		this.lblOperExp.BackColor = System.Drawing.Color.Teal;
		this.lblOperExp.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOperExp.ForeColor = System.Drawing.Color.White;
		this.lblOperExp.Location = new System.Drawing.Point(4, 108);
		this.lblOperExp.Name = "lblOperExp";
		this.lblOperExp.Size = new System.Drawing.Size(263, 19);
		this.lblOperExp.TabIndex = 112;
		this.lblOperExp.Text = "Operating Expense";
		this.lblOperExp.Click += new System.EventHandler(lblOperExp_Click);
		this.lblOperExp.MouseLeave += new System.EventHandler(lblOperExp_MouseLeave);
		this.lblOperExp.MouseHover += new System.EventHandler(lblOperExp_MouseHover);
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
		this.groupBox2.Controls.Add(this.lblotherincomeamt);
		this.groupBox2.Controls.Add(this.lblotherincome);
		this.groupBox2.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.ForeColor = System.Drawing.Color.MediumBlue;
		this.groupBox2.Location = new System.Drawing.Point(6, 94);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(694, 87);
		this.groupBox2.TabIndex = 151;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Non Operating Income";
		this.label21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label21.Location = new System.Drawing.Point(4, 54);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(689, 3);
		this.label21.TabIndex = 135;
		this.lblToNonOperatingIncome.BackColor = System.Drawing.Color.Teal;
		this.lblToNonOperatingIncome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblToNonOperatingIncome.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblToNonOperatingIncome.ForeColor = System.Drawing.Color.White;
		this.lblToNonOperatingIncome.Location = new System.Drawing.Point(524, 59);
		this.lblToNonOperatingIncome.Name = "lblToNonOperatingIncome";
		this.lblToNonOperatingIncome.Size = new System.Drawing.Size(167, 23);
		this.lblToNonOperatingIncome.TabIndex = 131;
		this.lblToNonOperatingIncome.Text = "0";
		this.lblToNonOperatingIncome.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblNoOperIncomeTotal.BackColor = System.Drawing.Color.LightBlue;
		this.lblNoOperIncomeTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblNoOperIncomeTotal.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.lblNoOperIncomeTotal.ForeColor = System.Drawing.Color.Black;
		this.lblNoOperIncomeTotal.Location = new System.Drawing.Point(337, 59);
		this.lblNoOperIncomeTotal.Name = "lblNoOperIncomeTotal";
		this.lblNoOperIncomeTotal.Size = new System.Drawing.Size(186, 23);
		this.lblNoOperIncomeTotal.TabIndex = 131;
		this.lblNoOperIncomeTotal.Text = "T. Non Operating Income";
		this.lblotherincomeamt.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblotherincomeamt.ForeColor = System.Drawing.Color.Black;
		this.lblotherincomeamt.Location = new System.Drawing.Point(529, 29);
		this.lblotherincomeamt.Name = "lblotherincomeamt";
		this.lblotherincomeamt.Size = new System.Drawing.Size(159, 19);
		this.lblotherincomeamt.TabIndex = 128;
		this.lblotherincomeamt.Text = "0";
		this.lblotherincomeamt.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblotherincome.BackColor = System.Drawing.Color.Teal;
		this.lblotherincome.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblotherincome.ForeColor = System.Drawing.Color.White;
		this.lblotherincome.Location = new System.Drawing.Point(4, 29);
		this.lblotherincome.Name = "lblotherincome";
		this.lblotherincome.Size = new System.Drawing.Size(263, 19);
		this.lblotherincome.TabIndex = 127;
		this.lblotherincome.Text = "Other Income";
		this.lblotherincome.Click += new System.EventHandler(lblotherincome_Click);
		this.lblotherincome.MouseLeave += new System.EventHandler(lblotherincome_MouseLeave);
		this.lblotherincome.MouseHover += new System.EventHandler(lblotherincome_MouseHover);
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
		this.groupBox1.Location = new System.Drawing.Point(6, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(694, 88);
		this.groupBox1.TabIndex = 148;
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
		this.lblSalesnet.Click += new System.EventHandler(lblSalesnet_Click);
		this.lblSalesnet.MouseLeave += new System.EventHandler(lblSalesnet_MouseLeave);
		this.lblSalesnet.MouseHover += new System.EventHandler(lblSalesnet_MouseHover);
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
		this.lblcostofsale.Click += new System.EventHandler(lblcostofsale_Click);
		this.lblcostofsale.MouseLeave += new System.EventHandler(lblcostofsale_MouseLeave);
		this.lblcostofsale.MouseMove += new System.Windows.Forms.MouseEventHandler(lblcostofsale_MouseMove);
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
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 26);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(443, 681);
		this.panel3.TabIndex = 1;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 63);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(443, 618);
		this.panel6.TabIndex = 1;
		this.grdHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		this.grdHistory.GroupTotals = (GroupTotals)2;
		this.grdHistory.HeaderFormatStyle.LineAlignment = (TextAlignment)2;
		this.grdHistory.HeaderFormatStyle.TextAlignment = (TextAlignment)2;
		this.grdHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(443, 618);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 47;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.panel5.Controls.Add(this.lblBranch);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch);
		this.panel5.Controls.Add(this.rdGroupeExpand);
		this.panel5.Controls.Add(this.txtFromDate);
		this.panel5.Controls.Add(this.rdGroupCollapsed);
		this.panel5.Controls.Add(this.txtToDate);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(443, 63);
		this.panel5.TabIndex = 0;
		this.lblBranch.AutoSize = true;
		this.lblBranch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranch.Location = new System.Drawing.Point(7, 38);
		this.lblBranch.Name = "lblBranch";
		this.lblBranch.Size = new System.Drawing.Size(44, 15);
		this.lblBranch.TabIndex = 151;
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
		((System.Windows.Forms.Control)(object)this.cmbbranch).Location = new System.Drawing.Point(103, 32);
		((System.Windows.Forms.Control)(object)this.cmbbranch).Name = "cmbbranch";
		((System.Windows.Forms.Control)(object)this.cmbbranch).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabIndex = 2;
		this.rdGroupeExpand.AutoSize = true;
		this.rdGroupeExpand.Location = new System.Drawing.Point(369, 37);
		this.rdGroupeExpand.Name = "rdGroupeExpand";
		this.rdGroupeExpand.Size = new System.Drawing.Size(61, 17);
		this.rdGroupeExpand.TabIndex = 5;
		this.rdGroupeExpand.TabStop = true;
		this.rdGroupeExpand.Text = "Expand";
		this.rdGroupeExpand.UseVisualStyleBackColor = true;
		this.rdGroupeExpand.CheckedChanged += new System.EventHandler(rdGroupeExpand_CheckedChanged);
		this.txtFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.CustomFormat = "dd-MMM-yy";
		this.txtFromDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(103, 7);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(99, 23);
		this.txtFromDate.TabIndex = 0;
		this.rdGroupCollapsed.AutoSize = true;
		this.rdGroupCollapsed.Location = new System.Drawing.Point(369, 13);
		this.rdGroupCollapsed.Name = "rdGroupCollapsed";
		this.rdGroupCollapsed.Size = new System.Drawing.Size(65, 17);
		this.rdGroupCollapsed.TabIndex = 4;
		this.rdGroupCollapsed.TabStop = true;
		this.rdGroupCollapsed.Text = "Collapse";
		this.rdGroupCollapsed.UseVisualStyleBackColor = true;
		this.rdGroupCollapsed.CheckedChanged += new System.EventHandler(rdGroupCollapsed_CheckedChanged);
		this.txtToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(205, 7);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(97, 23);
		this.txtToDate.TabIndex = 1;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(7, 10);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 116;
		this.label27.Text = "From && To Date";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(304, 32);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1152, 26);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1103, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsProfiLossAndBalanceSheet, this.toolStripSeparator1, this.btnNew, this.tsDropDown, this.btnProfitAndLoss, this.btnProfitLossBreakUp });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1152, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.tsProfiLossAndBalanceSheet.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tsProfiLossAndBalanceSheet.Name = "tsProfiLossAndBalanceSheet";
		this.tsProfiLossAndBalanceSheet.Size = new System.Drawing.Size(112, 24);
		this.tsProfiLossAndBalanceSheet.Text = "Profit And Loss";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.tsDropDown.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDown.Name = "tsDropDown";
		this.tsDropDown.Size = new System.Drawing.Size(88, 24);
		this.tsDropDown.Text = "143-Print";
		this.tsDropDown.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDown_DropDownItemClicked);
		this.btnProfitAndLoss.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnProfitAndLoss.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnProfitAndLoss.Name = "btnProfitAndLoss";
		this.btnProfitAndLoss.Size = new System.Drawing.Size(112, 24);
		this.btnProfitAndLoss.Text = "166-Profit & Loss";
		this.btnProfitAndLoss.Click += new System.EventHandler(btnProfitAndLoss_Click);
		this.btnProfitLossBreakUp.Image = Architecture.WinApp.Properties.Resources.Forward2;
		this.btnProfitLossBreakUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnProfitLossBreakUp.Name = "btnProfitLossBreakUp";
		this.btnProfitLossBreakUp.Size = new System.Drawing.Size(130, 24);
		this.btnProfitLossBreakUp.Text = "ProfitLoss BreakUp";
		this.btnProfitLossBreakUp.Click += new System.EventHandler(btnProfitLossBreakUp_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1152, 707);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProfitLossHararical";
		this.Text = "Profit & Loss";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmProfitLossHararical_FormClosing);
		base.Load += new System.EventHandler(frmTrialBalancesAllLevel_Load);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.groupBox14.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox10.ResumeLayout(false);
		this.groupBox10.PerformLayout();
		this.groupBox9.ResumeLayout(false);
		this.groupBox9.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
