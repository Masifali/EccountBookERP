using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.InventoryReports;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class PayablesAndPaymentSchedule : Form
{
	private UserAccount UserAccount = new UserAccount();

	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtrpt = new DataTable();

	private DataTable dtGridMain = new DataTable();

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private Panel panel3;

	private Label label23;

	private Panel panel5;

	private Button btnshow;

	private Label label1;

	private Label label27;

	private Label label2;

	private Label label3;

	internal UltraCombo CmbParentCategory;

	private Label label9;

	private Label label4;

	private Label label6;

	private GroupBox groupBox1;

	private Panel panel4;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdPayablesAndSchedule;

	private ToolStripButton btnNew;

	private ToolStripButton btnShortCut;

	private ToolStripButton print;

	public DateTimePicker datPurchaseFrom;

	public DateTimePicker datDueUpto;

	public DateTimePicker datDueFrom;

	public DateTimePicker datDateTo;

	public DateTimePicker datDateFrom;

	public DateTimePicker datPurchaseTo;

	private Label label5;

	private TextBox txtIntervalDays;

	internal UltraCombo CmbBranches;

	private Label Branches;

	internal UltraCombo CmbCustomGroups;

	private Label label7;

	internal UltraCombo CmbCustomerGroup;

	private Label label10;

	internal UltraCombo cmbAccount;

	private Label label11;

	private Panel panel2;

	internal GridEX grdSummary;

	private Label label12;

	private TextBox txtAgingDays;

	private Label label13;

	public PayablesAndPaymentSchedule(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void PayablesAndPaymentSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(978, 732);
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
			if (!BranchFeature)
			{
				Branches.Visible = false;
				((Control)(object)CmbBranches).Visible = false;
				label12.Location = new Point(459, 77);
				txtAgingDays.Location = new Point(555, 72);
				btnshow.Location = new Point(697, 72);
			}
			else
			{
				BranchesFill();
			}
			datDateFrom.Focus();
			txtIntervalDays.Text = 30.ToString();
			datDateFrom.Value = DateTime.Now.AddDays(-30.0);
			datPurchaseFrom.Value = DateTime.Now.AddDays(-Conversion.ToInt((object)txtIntervalDays.Text));
			CustomGroupsFill();
			CustomerGroupFill();
			ControlAccountFill3rdLevel();
			ParentCategoryFill();
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
			wf.Show();
			gridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	private void gridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			obj.InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value);
			if (datDateFrom.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datDateFrom.Value);
			}
			if (datDateTo.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datDateTo.Value);
			}
			if (datDueFrom.Checked)
			{
				obj.DueDateFrom = Conversion.ToDateTime((object)datDueFrom.Value);
			}
			if (datDateTo.Checked)
			{
				obj.DueDateTo = Conversion.ToDateTime((object)datDueUpto.Value);
			}
			if (datPurchaseFrom.Checked)
			{
				obj.EffectedDate = Conversion.ToDateTime((object)datPurchaseFrom.Value);
			}
			if (datPurchaseTo.Checked)
			{
				obj.EffectedDateTo = Conversion.ToDateTime((object)datPurchaseTo.Value);
			}
			obj.CustomerGroupId = Conversion.ToInt(CmbCustomerGroup.Value);
			obj.CustomGroupId = Conversion.ToInt(CmbCustomGroups.Value);
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			obj.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			string GroupAccounts = "";
			if (Conversion.ToString((object)((Control)(object)cmbAccount).Text) != string.Empty)
			{
				DataTable dt = (DataTable)((UltraGridBase)cmbAccount).DataSource;
				GroupAccounts = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)cmbAccount).Text, dt, "AccountTitle");
				obj.ControlAccountIds = GroupAccounts;
			}
			obj.AgingDays = Conversion.ToInt((object)txtAgingDays.Text);
			DataSet dtNew = VoucherReports.PayablesAndPaymentSchedule(obj);
			if (dtNew.Tables.Count == 0)
			{
				wf.Close();
				grdSummary.DataSource = null;
				grdPayablesAndSchedule.DataSource = null;
				return;
			}
			dtrpt = dtNew.Tables[0];
			if (dtrpt.Rows.Count == 0)
			{
				wf.Close();
				grdPayablesAndSchedule.DataSource = null;
				MessageBox.Show("No Record Found For Display", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				dtGridMain.Rows.Clear();
				if (dtGridMain.Columns.Count == 0)
				{
					dtGridMain.Columns.Add("AccountId", typeof(int));
					dtGridMain.Columns.Add("AccountTypeId", typeof(int));
					dtGridMain.Columns.Add("BranchesId", typeof(int));
					dtGridMain.Columns.Add("BranchName", typeof(string));
					dtGridMain.Columns.Add("AccountClass", typeof(string));
					dtGridMain.Columns.Add("ParentAccount", typeof(string));
					dtGridMain.Columns.Add("AccountCode", typeof(string));
					dtGridMain.Columns.Add("AccountTitle", typeof(string));
					dtGridMain.Columns.Add("AccountType", typeof(string));
					dtGridMain.Columns.Add("Opening", typeof(double));
					dtGridMain.Columns.Add("CurrDebit", typeof(double));
					dtGridMain.Columns.Add("CurrCredit", typeof(double));
					dtGridMain.Columns.Add("Closing", typeof(double));
					dtGridMain.Columns.Add("DueBalance", typeof(double));
					dtGridMain.Columns.Add("NotYetDue", typeof(double));
					dtGridMain.Columns.Add("PayToday", typeof(double));
					dtGridMain.Columns.Add("Short/Excess", typeof(double));
					dtGridMain.Columns.Add("Increase/Decrease", typeof(string));
					dtGridMain.Columns.Add("LastBillDate", typeof(DateTime));
					dtGridMain.Columns.Add("LastBillsAmount", typeof(double));
					dtGridMain.Columns.Add("BillDays", typeof(int));
					dtGridMain.Columns.Add("LastPaymentDate", typeof(DateTime));
					dtGridMain.Columns.Add("LastPaymentAmount", typeof(double));
					dtGridMain.Columns.Add("PaymentDays", typeof(int));
					dtGridMain.Columns.Add("PurchaseQty", typeof(double));
					dtGridMain.Columns.Add("PurchaseWeight", typeof(double));
					dtGridMain.Columns.Add("PurchaseAmount", typeof(double));
					dtGridMain.Columns.Add("OrderQty", typeof(double));
					dtGridMain.Columns.Add("ReceivedQty", typeof(double));
					dtGridMain.Columns.Add("BalQty", typeof(double));
					dtGridMain.Columns.Add("OrderWeight", typeof(double));
					dtGridMain.Columns.Add("ReceivedWeight", typeof(double));
					dtGridMain.Columns.Add("BalWeight", typeof(double));
					dtGridMain.Columns.Add("FromDate", typeof(DateTime));
					dtGridMain.Columns.Add("ToDate", typeof(DateTime));
					dtGridMain.Columns.Add("DueDateFrom", typeof(DateTime));
					dtGridMain.Columns.Add("DueDateTo", typeof(DateTime));
					dtGridMain.Columns.Add("PurchaseFromDate", typeof(DateTime));
					dtGridMain.Columns.Add("PurchaseToDate", typeof(DateTime));
				}
				dtGridMain.BeginLoadData();
				foreach (DataRow row in dtrpt.Rows)
				{
					dtGridMain.Rows.Add(row["AccountId"], row["AccountTypeId"], row["BranchesId"], row["BranchName"], row["ClassName"], row["ParentAccount"], row["AccountCode"], row["AccountTitle"], row["AccountType"], row["Opening"], row["CurrDebit"], row["CurrCredit"], row["Closing"], row["DueBalance"], row["NotYetDue"], row["PayToday"], row["Short/Excess"], (Conversion.ToDouble(row["Short/Excess"]) < 0.0) ? "Increase" : "Decrease", Conversion.ToDateTime(row["LastBillDate"]), row["LastBillsAmount"], row["BillDays"], Conversion.ToDateTime(row["LastPaymentDate"]), row["LastPaymentAmount"], row["PaymentDays"], row["PurchaseQty"], row["PurchaseWeight"], row["PurchaseAmount"], row["OrderQty"], row["ReceivedQty"], row["BalQty"], row["OrderWeight"], row["ReceivedWeight"], row["BalWeight"], Conversion.ToDateTime(row["FromDate"]), Conversion.ToDateTime(row["ToDate"]), Conversion.ToDateTime(row["DueDateFrom"]), Conversion.ToDateTime(row["DueDateTo"]), Conversion.ToDateTime(row["PurchaseFromDate"]), Conversion.ToDateTime(row["PurchaseToDate"]));
				}
				dtGridMain.EndLoadData();
				grdPayablesAndSchedule.DataSource = dtGridMain;
				grdPayablesAndSchedule.RetrieveStructure();
				GridSetting();
			}
			DataTable dtSummaryDbRecord = dtNew.Tables[1];
			if (dtSummaryDbRecord.Rows.Count == 0)
			{
				wf.Close();
				grdSummary.DataSource = null;
				return;
			}
			DataTable dtSummary = new DataTable();
			dtSummary.Columns.Add("Description", typeof(string));
			dtSummary.Columns.Add("Amount", typeof(double));
			dtSummary.Columns.Add(Conversion.ToString(dtSummaryDbRecord.Rows[0]["IstIntervale"]), typeof(double));
			dtSummary.Columns.Add(Conversion.ToString(dtSummaryDbRecord.Rows[0]["ScnInterval"]), typeof(double));
			dtSummary.Columns.Add(Conversion.ToString(dtSummaryDbRecord.Rows[0]["TrdIntarval"]), typeof(double));
			dtSummary.Columns.Add(Conversion.ToString(dtSummaryDbRecord.Rows[0]["Above"]), typeof(double));
			for (int i = 0; i < dtSummaryDbRecord.Rows.Count; i++)
			{
				dtSummary.Rows.Add(dtSummaryDbRecord.Rows[i]["Description"], dtSummaryDbRecord.Rows[i]["Amount"], dtSummaryDbRecord.Rows[i]["Value_1"], dtSummaryDbRecord.Rows[i]["Value_2"], dtSummaryDbRecord.Rows[i]["Value_3"], dtSummaryDbRecord.Rows[i]["Value_4"]);
			}
			grdSummary.DataSource = dtSummary;
			grdSummary.RetrieveStructure();
			grdSummary.RootTable.Columns["Description"].Width = 125;
			grdSummary.RootTable.Columns["Amount"].Width = 100;
			grdSummary.RootTable.Columns[Conversion.ToString(dtSummaryDbRecord.Rows[0]["IstIntervale"])].Width = 100;
			grdSummary.RootTable.Columns[Conversion.ToString(dtSummaryDbRecord.Rows[0]["ScnInterval"])].Width = 100;
			grdSummary.RootTable.Columns[Conversion.ToString(dtSummaryDbRecord.Rows[0]["TrdIntarval"])].Width = 100;
			grdSummary.RootTable.Columns[Conversion.ToString(dtSummaryDbRecord.Rows[0]["Above"])].Width = 100;
			grdSummary.GroupByBoxVisible = false;
			grdSummary.TotalRow = (InheritableBoolean)2;
			grdSummary.FilterMode = (FilterMode)0;
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

	private void GridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"BranchesId", "AccountClass", "AccountTypeId", "AccountId", "ParentAccount", "LastBillDate", "LastBillsAmount", "BillDays", "ReceivedQty", "OrderWeight",
				"PurchaseWeight", "ReceivedWeight", "BalWeight", "FromDate", "ToDate", "DueDateFrom", "DueDateTo", "PurchaseFromDate", "PurchaseToDate", "OrderQty"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"PayToday",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPayablesAndSchedule, 2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, hiddenColumns, editableColumns);
			grdPayablesAndSchedule.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdPayablesAndSchedule.RootTable.Groups.Remove(grdPayablesAndSchedule.RootTable.Groups["ParentAccount"]);
			grdPayablesAndSchedule.RootTable.Groups.Add("ParentAccount");
			grdPayablesAndSchedule.RootTable.Columns["ParentAccount"].Caption = "Parent Account Title";
			grdPayablesAndSchedule.RootTable.Columns["AccountTitle"].Caption = "Account Title / Party";
			grdPayablesAndSchedule.RootTable.Columns["Opening"].Caption = "Opening Balance";
			grdPayablesAndSchedule.RootTable.Columns["CurrDebit"].Caption = "Current Debit";
			grdPayablesAndSchedule.RootTable.Columns["CurrCredit"].Caption = "Current Credit";
			grdPayablesAndSchedule.RootTable.Columns["Closing"].Caption = "Closing Balance";
			grdPayablesAndSchedule.RootTable.Columns["LastPaymentDate"].FormatString = "dd-MMM-yy";
			grdPayablesAndSchedule.RootTable.Columns["FromDate"].FormatString = "dd-MMM-yy";
			grdPayablesAndSchedule.RootTable.Columns["ToDate"].FormatString = "dd-MMM-yy";
			grdPayablesAndSchedule.RootTable.Columns["DueDateFrom"].FormatString = "dd-MMM-yy";
			grdPayablesAndSchedule.RootTable.Columns["DueDateTo"].FormatString = "dd-MMM-yy";
			grdPayablesAndSchedule.RootTable.Columns["PurchaseFromDate"].FormatString = "dd-MMM-yy";
			grdPayablesAndSchedule.RootTable.Columns["PurchaseToDate"].FormatString = "dd-MMM-yy";
			grdPayablesAndSchedule.RootTable.Columns["BalQty"].Caption = "Order Bal Qty";
			grdPayablesAndSchedule.RootTable.Columns["CurrDebit"].Caption = "Debit";
			grdPayablesAndSchedule.RootTable.Columns["CurrCredit"].Caption = "Credit";
			grdPayablesAndSchedule.RootTable.Columns["CurrDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["CurrCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["DueBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["NotYetDue"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["Short/Excess"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["CurrDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["CurrCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["DueBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["NotYetDue"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["Short/Excess"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPayablesAndSchedule.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdPayablesAndSchedule.RootTable.Columns["ParentAccount"].Width = 120;
			grdPayablesAndSchedule.RootTable.Columns["AccountTitle"].Width = 150;
			grdPayablesAndSchedule.RootTable.Columns["AccountClass"].Width = 65;
			grdPayablesAndSchedule.RootTable.Columns["AccountType"].Width = 65;
			grdPayablesAndSchedule.RootTable.Columns["AccountCode"].Width = 90;
			grdPayablesAndSchedule.RootTable.Columns["Opening"].Width = 103;
			grdPayablesAndSchedule.RootTable.Columns["Closing"].Width = 100;
			grdPayablesAndSchedule.RootTable.Columns["CurrDebit"].Width = 90;
			grdPayablesAndSchedule.RootTable.Columns["CurrCredit"].Width = 92;
			grdPayablesAndSchedule.RootTable.Columns["Short/Excess"].Width = 90;
			grdPayablesAndSchedule.RootTable.Columns["Increase/Decrease"].Width = 70;
			grdPayablesAndSchedule.RootTable.Columns["DueBalance"].Width = 80;
			grdPayablesAndSchedule.RootTable.Columns["NotYetDue"].Width = 90;
			grdPayablesAndSchedule.RootTable.Columns["PayToday"].Width = 55;
			grdPayablesAndSchedule.RootTable.Columns["PaymentDays"].Width = 63;
			grdPayablesAndSchedule.RootTable.Columns["LastPaymentDate"].Width = 90;
			grdPayablesAndSchedule.RootTable.Columns["LastPaymentAmount"].Width = 90;
			grdPayablesAndSchedule.RootTable.Columns["PurchaseQty"].Width = 65;
			grdPayablesAndSchedule.RootTable.Columns["OrderQty"].Width = 55;
			grdPayablesAndSchedule.RootTable.Columns["BalQty"].Width = 55;
			grdPayablesAndSchedule.RootTable.Columns["PurchaseAmount"].Width = 90;
			grdPayablesAndSchedule.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grdPayablesAndSchedule.RootTable.Columns["PurchaseAmount"].ColumnType = (ColumnType)5;
			grdPayablesAndSchedule.RootTable.Columns["BalQty"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ControlAccountFill3rdLevel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			DataTable dtAccount = ChartofAccount.ReadAllAccountgroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Account_Level = 3,
				AccountTypeId = 3,
				AccouuntClassId = 3
			});
			if (dtAccount.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("AccountTitle");
				for (int i = 0; i < dtAccount.Rows.Count; i++)
				{
					dt.Rows.Add(dtAccount.Rows[i]["AccountCode"], dtAccount.Rows[i]["AccountTitle"]);
				}
				DDL.BindDDL(dt, cmbAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
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

	private void CustomGroupsFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCustomGroups.Value);
			DataTable dtCustom = CommonServices.CustomeGroupsDefine(1);
			if (dtCustom.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCustom, CmbCustomGroups, "Id", "AcLookUpsDescription", "Custom Group", false);
				((UltraGridBase)CmbCustomGroups).DisplayLayout.Bands[0].Columns[1].Width = 336;
				if (Id > 0)
				{
					if (dtCustom.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCustomGroups.Value = Id;
					}
					else
					{
						((Control)(object)CmbCustomGroups).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCustomGroups).Text = string.Empty;
				((UltraGridBase)CmbCustomGroups).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerGroupFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCG = CustomerGroup.GetAll(new CustomerGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCG.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("Description");
				for (int i = 0; i < dtCG.Rows.Count; i++)
				{
					dt.Rows.Add(dtCG.Rows[i]["Id"], dtCG.Rows[i]["Description"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbCustomerGroup, "Id", "Description", "Customer Group", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void ParentCategoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtAllData = InvPurchaseInvoice.GetDataForPurchaseInvoiceRegisterDropdownBind(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtAllData.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtParCat = new DataTable();
			dtParCat.Columns.Add("ParentCategoryId");
			dtParCat.Columns.Add("ParentCategory");
			for (int i = 0; i < dtAllData.Rows.Count; i++)
			{
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "ItemParentCategory")
				{
					dtParCat.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
			}
			DDL.BindDDL(dtParCat, CmbParentCategory, "ParentCategoryId", "ParentCategory", "Parent Category", ZeroIndex: false);
			((UltraGridBase)CmbParentCategory).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			((Control)(object)CmbParentCategory).Text = string.Empty;
			datDateFrom.Value = DateTime.Now.AddDays(-30.0);
			datDateFrom.Focus();
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
			Reset();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPayablesAndSchedule).Name);
			if (fs != null)
			{
				grdPayablesAndSchedule.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PayablesAndPaymentSchedule_KeyDown(object sender, KeyEventArgs e)
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
				print_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				datDateFrom.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				datDateTo.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdPayablesAndSchedule).Focus();
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

	private void btnShortCut_Click(object sender, EventArgs e)
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

	private void print_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtrpt.Rows.Count > 0 && dtrpt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dtrpt, "121_01_PayablesAndPaymentSchedule.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PayablesAndPaymentSchedule_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnclose_Click(object sender, EventArgs e)
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

	private void txtIntervalDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			datPurchaseFrom.Value = DateTime.Now.AddDays(-Conversion.ToInt((object)txtIntervalDays.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPayablesAndSchedule_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdPayablesAndSchedule.CurrentRow;
			int AccountId = Conversion.ToInt(item.Cells["AccountId"].Value);
			int branchesId = Conversion.ToInt(item.Cells["BranchesId"].Value);
			if (e.Column.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(AccountId, Conversion.ToDateTime((object)datDateFrom.Value), Conversion.ToDateTime((object)datDateTo.Value), 0, branchesId);
			}
			if (e.Column.Key == "BalQty")
			{
				bool OpenReport2 = false;
				if (!(UserAccount.RoleName == "Admin") && !CommonServices.CheckScreenRight("PurchaseOrderHistory"))
				{
					throw new Exception("You don't have screen right!");
				}
				if (Conversion.ToDouble(item.Cells["BalQty"].Value) != 0.0)
				{
					int supplierId2 = 0;
					DataTable dt2 = SupplierCustomer.GetIdbyGlAccountId(new SupplierCustomer
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						GlAccountId = AccountId
					});
					if (dt2.Rows.Count > 0)
					{
						supplierId2 = Conversion.ToInt((object)Conversion.ToInt(dt2.Rows[0]["Id"]));
					}
					PurchaseOrderHistory purchaseOrderHistory = new PurchaseOrderHistory(UserAccount);
					purchaseOrderHistory.FromDate = Conversion.ToDateTime((object)datPurchaseFrom.Value);
					purchaseOrderHistory.ToDate = Conversion.ToDateTime((object)datPurchaseTo.Value);
					purchaseOrderHistory.SupplierId = supplierId2;
					purchaseOrderHistory.radAllApprovedAndUnApprovedDetail.Checked = true;
					purchaseOrderHistory.RadAllOpenCompCancelDetail.Checked = true;
					purchaseOrderHistory.datFromDate.Checked = false;
					purchaseOrderHistory.updateFromDate = false;
					purchaseOrderHistory.ShowDialog();
				}
			}
			if (!(e.Column.Key == "PurchaseAmount"))
			{
				return;
			}
			bool OpenReport = false;
			if (UserAccount.RoleName == "Admin" || CommonServices.CheckScreenRight("PurchaseRegisterNew"))
			{
				if (Conversion.ToDouble(item.Cells["PurchaseAmount"].Value) != 0.0)
				{
					int supplierId = 0;
					DataTable dt = SupplierCustomer.GetIdbyGlAccountId(new SupplierCustomer
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						GlAccountId = AccountId
					});
					if (dt.Rows.Count > 0)
					{
						supplierId = Conversion.ToInt((object)Conversion.ToInt(dt.Rows[0]["Id"]));
					}
					PurchaseRegisterNew purchaseRegisterNew = new PurchaseRegisterNew(UserAccount);
					purchaseRegisterNew.FromDate = Conversion.ToDateTime((object)datPurchaseFrom.Value);
					purchaseRegisterNew.ToDate = Conversion.ToDateTime((object)datPurchaseTo.Value);
					purchaseRegisterNew.SupplierId = supplierId;
					purchaseRegisterNew.ShowDialog();
				}
				return;
			}
			throw new Exception("You don't have screen right!");
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
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		GridEXLayout grdPayablesAndSchedule_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.PayablesAndPaymentSchedule));
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
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdPayablesAndSchedule = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdSummary = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbAccount = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtAgingDays = new System.Windows.Forms.TextBox();
		this.txtIntervalDays = new System.Windows.Forms.TextBox();
		this.CmbCustomerGroup = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.datPurchaseTo = new System.Windows.Forms.DateTimePicker();
		this.datDueUpto = new System.Windows.Forms.DateTimePicker();
		this.CmbCustomGroups = new UltraCombo();
		this.datDueFrom = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.datDateTo = new System.Windows.Forms.DateTimePicker();
		this.datDateFrom = new System.Windows.Forms.DateTimePicker();
		this.datPurchaseFrom = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbParentCategory = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCut = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPayablesAndSchedule).BeginInit();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummary).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroups).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdPayablesAndSchedule);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1376, 693);
		this.panel1.TabIndex = 0;
		this.grdPayablesAndSchedule.AllowEdit = (InheritableBoolean)2;
		this.grdPayablesAndSchedule.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPayablesAndSchedule_DesignTimeLayout.LayoutString = resources.GetString("grdPayablesAndSchedule_DesignTimeLayout.LayoutString");
		this.grdPayablesAndSchedule.DesignTimeLayout = grdPayablesAndSchedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPayablesAndSchedule).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPayablesAndSchedule.DynamicFiltering = true;
		this.grdPayablesAndSchedule.FilterMode = (FilterMode)2;
		this.grdPayablesAndSchedule.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPayablesAndSchedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPayablesAndSchedule.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPayablesAndSchedule).Location = new System.Drawing.Point(0, 203);
		((System.Windows.Forms.Control)(object)this.grdPayablesAndSchedule).Name = "grdPayablesAndSchedule";
		((ScrollableBase)this.grdPayablesAndSchedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPayablesAndSchedule).Size = new System.Drawing.Size(1376, 490);
		((System.Windows.Forms.Control)(object)this.grdPayablesAndSchedule).TabIndex = 0;
		this.grdPayablesAndSchedule.TotalRow = (InheritableBoolean)1;
		this.grdPayablesAndSchedule.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPayablesAndSchedule).VisualStyle = (VisualStyle)4;
		this.grdPayablesAndSchedule.LinkClicked += new ColumnActionEventHandler(grdPayablesAndSchedule_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 179);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1376, 24);
		this.panel4.TabIndex = 25;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(4, 2);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(159, 19);
		this.label8.TabIndex = 1;
		this.label8.Text = "Payment Schedule";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1333, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdPayablesAndSchedule;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 22);
		this.ctrlGrdBar1.TabIndex = 3;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.panel2);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 51);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1376, 128);
		this.panel5.TabIndex = 6;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummary);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(763, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(613, 128);
		this.panel2.TabIndex = 153;
		this.grdSummary.AllowEdit = (InheritableBoolean)2;
		this.grdSummary.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdSummary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummary.DynamicFiltering = true;
		this.grdSummary.FilterMode = (FilterMode)2;
		this.grdSummary.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummary.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSummary).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSummary).Name = "grdSummary";
		((ScrollableBase)this.grdSummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummary).Size = new System.Drawing.Size(613, 128);
		((System.Windows.Forms.Control)(object)this.grdSummary).TabIndex = 0;
		this.grdSummary.TotalRow = (InheritableBoolean)1;
		this.grdSummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummary).VisualStyle = (VisualStyle)4;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccount);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.groupBox1.Controls.Add(this.Branches);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtAgingDays);
		this.groupBox1.Controls.Add(this.txtIntervalDays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerGroup);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.datPurchaseTo);
		this.groupBox1.Controls.Add(this.datDueUpto);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroups);
		this.groupBox1.Controls.Add(this.datDueFrom);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.datDateTo);
		this.groupBox1.Controls.Add(this.datDateFrom);
		this.groupBox1.Controls.Add(this.datPurchaseFrom);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(763, 128);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
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
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
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
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccount).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Location = new System.Drawing.Point(555, 45);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Name = "cmbAccount";
		((System.Windows.Forms.Control)(object)this.cmbAccount).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccount).TabIndex = 9;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(459, 51);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(94, 15);
		this.label11.TabIndex = 163;
		this.label11.Text = "Control Account";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(697, 99);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(60, 25);
		this.btnshow.TabIndex = 12;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
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
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(555, 72);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 10;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(459, 77);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(63, 17);
		this.Branches.TabIndex = 161;
		this.Branches.Text = "Branches";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(459, 103);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(77, 17);
		this.label12.TabIndex = 159;
		this.label12.Text = "Aging Days";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(218, 50);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 17);
		this.label5.TabIndex = 159;
		this.label5.Text = "Interval Days";
		this.txtAgingDays.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAgingDays.Location = new System.Drawing.Point(555, 100);
		this.txtAgingDays.Name = "txtAgingDays";
		this.txtAgingDays.Size = new System.Drawing.Size(135, 23);
		this.txtAgingDays.TabIndex = 11;
		this.txtAgingDays.Text = "30";
		this.txtAgingDays.TextChanged += new System.EventHandler(txtIntervalDays_TextChanged);
		this.txtIntervalDays.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIntervalDays.Location = new System.Drawing.Point(319, 47);
		this.txtIntervalDays.Name = "txtIntervalDays";
		this.txtIntervalDays.Size = new System.Drawing.Size(135, 23);
		this.txtIntervalDays.TabIndex = 5;
		this.txtIntervalDays.Text = "30";
		this.txtIntervalDays.TextChanged += new System.EventHandler(txtIntervalDays_TextChanged);
		this.CmbCustomerGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).Location = new System.Drawing.Point(555, 18);
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).Name = "CmbCustomerGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).TabIndex = 8;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(459, 24);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(94, 15);
		this.label10.TabIndex = 156;
		this.label10.Text = "Customer Group";
		this.datPurchaseTo.CustomFormat = "dd-MMM-yy";
		this.datPurchaseTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datPurchaseTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datPurchaseTo.Location = new System.Drawing.Point(319, 74);
		this.datPurchaseTo.Name = "datPurchaseTo";
		this.datPurchaseTo.ShowCheckBox = true;
		this.datPurchaseTo.Size = new System.Drawing.Size(135, 23);
		this.datPurchaseTo.TabIndex = 6;
		this.datDueUpto.CustomFormat = "dd-MMM-yy";
		this.datDueUpto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueUpto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDueUpto.Location = new System.Drawing.Point(79, 100);
		this.datDueUpto.Name = "datDueUpto";
		this.datDueUpto.ShowCheckBox = true;
		this.datDueUpto.Size = new System.Drawing.Size(135, 23);
		this.datDueUpto.TabIndex = 3;
		this.CmbCustomGroups.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroups.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroups.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroups.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroups).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroups).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroups).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroups).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroups).Location = new System.Drawing.Point(319, 98);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroups).Name = "CmbCustomGroups";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroups).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroups).TabIndex = 7;
		this.datDueFrom.Checked = false;
		this.datDueFrom.CustomFormat = "dd-MMM-yy";
		this.datDueFrom.Enabled = false;
		this.datDueFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDueFrom.Location = new System.Drawing.Point(79, 74);
		this.datDueFrom.Name = "datDueFrom";
		this.datDueFrom.ShowCheckBox = true;
		this.datDueFrom.Size = new System.Drawing.Size(135, 23);
		this.datDueFrom.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(218, 104);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(84, 15);
		this.label7.TabIndex = 154;
		this.label7.Text = "Custom Group";
		this.datDateTo.CustomFormat = "dd-MMM-yy";
		this.datDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDateTo.Location = new System.Drawing.Point(79, 47);
		this.datDateTo.Name = "datDateTo";
		this.datDateTo.ShowCheckBox = true;
		this.datDateTo.Size = new System.Drawing.Size(135, 23);
		this.datDateTo.TabIndex = 1;
		this.datDateFrom.CustomFormat = "dd-MMM-yy";
		this.datDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDateFrom.Location = new System.Drawing.Point(79, 20);
		this.datDateFrom.Name = "datDateFrom";
		this.datDateFrom.ShowCheckBox = true;
		this.datDateFrom.Size = new System.Drawing.Size(135, 23);
		this.datDateFrom.TabIndex = 0;
		this.datPurchaseFrom.CustomFormat = "dd-MMM-yy";
		this.datPurchaseFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datPurchaseFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datPurchaseFrom.Location = new System.Drawing.Point(319, 20);
		this.datPurchaseFrom.Name = "datPurchaseFrom";
		this.datPurchaseFrom.ShowCheckBox = true;
		this.datPurchaseFrom.Size = new System.Drawing.Size(135, 23);
		this.datPurchaseFrom.TabIndex = 4;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(7, 23);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(72, 17);
		this.label27.TabIndex = 103;
		this.label27.Text = "Date From";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 50);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(54, 17);
		this.label1.TabIndex = 103;
		this.label1.Text = "Date To";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 77);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(68, 17);
		this.label3.TabIndex = 105;
		this.label3.Text = "Due From";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(7, 103);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 17);
		this.label2.TabIndex = 104;
		this.label2.Text = "Due Up To";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(218, 77);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(81, 17);
		this.label4.TabIndex = 150;
		this.label4.Text = "Purchase To";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(218, 23);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(99, 17);
		this.label6.TabIndex = 151;
		this.label6.Text = "Purchase From";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 24f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.Green;
		this.label13.Location = new System.Drawing.Point(242, 46);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(33, 45);
		this.label13.TabIndex = 159;
		this.label13.Text = "-";
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(15, 161);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Visible = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(15, 143);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(91, 15);
		this.label9.TabIndex = 148;
		this.label9.Text = "Parent Category";
		this.label9.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1376, 24);
		this.panel3.TabIndex = 5;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(274, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Payables And Payment Schedule";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.print, this.btnShortCut });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1376, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(114, 24);
		this.print.Text = "121_01 Print";
		this.print.Click += new System.EventHandler(print_Click);
		this.btnShortCut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCut.Name = "btnShortCut";
		this.btnShortCut.Size = new System.Drawing.Size(105, 24);
		this.btnShortCut.Text = "ShortCut Keys";
		this.btnShortCut.Click += new System.EventHandler(btnShortCut_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1376, 693);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "PayablesAndPaymentSchedule";
		this.Text = "EccountBook ERP";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PayablesAndPaymentSchedule_FormClosing);
		base.Load += new System.EventHandler(PayablesAndPaymentSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PayablesAndPaymentSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPayablesAndSchedule).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummary).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroups).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
