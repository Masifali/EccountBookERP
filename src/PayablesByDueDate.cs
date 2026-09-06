// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
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

public class PayablesByDueDate : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtrpt = new DataTable();

	private DataTable dtGroups = new DataTable();

	private string GroupIds = "";

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker datDueDateTo;

	private Label label27;

	private Label label29;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel6;

	private Panel panel5;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	internal UltraCombo CmbGroup;

	private DateTimePicker datDueDateFrom;

	private Label label1;

	private RadioButton rdPayablesByDueDateBetween;

	private RadioButton rdPayablesByDueDate;

	private TextBox txtBalTo;

	private Label label3;

	private TextBox txtBalFrom;

	private Label label2;

	private Label lbllang;

	internal UltraCombo CmbLanguage;

	private CheckBox chkblue;

	private CheckBox chkRed;

	private CheckBox chkGreen;

	private TextBox txtRed;

	private TextBox txtBlue;

	private TextBox txtGreen;

	private Label label5;

	private Label label9;

	internal UltraCombo cmbDateType;

	private GroupBox groupBox1;

	private Panel panel7;

	private Label label4;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnshortcutkey;

	public PayablesByDueDate(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.P)
			{
				print_Click_1(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbDateType.Focus();
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
			dt.Rows.Add("Print + P", "For print");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Filter");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerGroupFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			dtGroups.Rows.Clear();
			DataTable dt = CustomerGroup.GetAll(new CustomerGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["Id"]) != 7 && Conversion.ToInt(dt.Rows[i]["Id"]) != 9 && Conversion.ToInt(dt.Rows[i]["Id"]) != 10 && Conversion.ToInt(dt.Rows[i]["Id"]) != 12 && Conversion.ToInt(dt.Rows[i]["Id"]) != 13 && Conversion.ToInt(dt.Rows[i]["Id"]) != 14 && Conversion.ToInt(dt.Rows[i]["Id"]) != 15)
				{
					dtGroups.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["Description"]);
				}
			}
			DDL.BindDDLNew(dtGroups, CmbGroup, "Id", "Description", "CustomerGroup", false);
			UltraGridColumn obj = ((UltraGridBase)CmbGroup).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj).Key = "Selected";
			((HeaderBase)obj.Header).Caption = string.Empty;
			obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj.DataType = typeof(bool);
			((HeaderBase)obj.Header).VisiblePosition = 0;
			CmbGroup.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbGroup.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbGroup.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbGroup.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LanguageDropdownBind()
	{
		try
		{
			DataTable dt = CommonServices.MultiLanguagesGetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbLanguage, "Id", "LanguageDescription", "LanguageName", false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateType, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateType).Rows[2].Activate();
			((UltraGridBase)cmbDateType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(983, 700);
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			lbllang.Visible = false;
			((Control)(object)CmbLanguage).Visible = false;
			dtGroups.Columns.Add("Id");
			dtGroups.Columns.Add("Description");
			Datetypefill();
			CustomerGroupFill();
			LanguageDropdownBind();
			cmbDateType.Focus();
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

	private void reset()
	{
		try
		{
			lbllang.Visible = false;
			((Control)(object)CmbLanguage).Visible = false;
			Datetypefill();
			cmbDateType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtrpt.Rows.Count > 0 && dtrpt != null)
			{
				if (rdPayablesByDueDate.Checked)
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dtrpt, "142-DueByDatePayablesAndReceivables.rpt");
					return;
				}
				string AccountIds = "";
				if (grd.GetCheckedRows().Length != 0)
				{
					GridEXRow[] checkedRows = grd.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						AccountIds = AccountIds + Conversion.ToString(item.Cells["AccountId"].Value) + ",";
					}
				}
				DataTable dt = VoucherReports.USp_AccountsPayablesByDuedateBetweenPeriod(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DueDateFrom = datDueDateFrom.Value,
					DueDateTo = datDueDateTo.Value,
					Ids = GroupIds,
					AccountIds = AccountIds
				});
				if (dt.Rows.Count == 0 && dt == null)
				{
					throw new Exception("Not Record Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dt, "159-PayablesAndReceivablebetweenPeriod.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		try
		{
			wf.Show();
			GroupIds = "";
			if (((Control)(object)CmbGroup).Text != null && ((Control)(object)CmbGroup).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbGroup).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtGroups.Select("Description='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						GroupIds = GroupIds + Conversion.ToString(dr[0].ItemArray[0]) + ",";
					}
				}
			}
			if (rdPayablesByDueDate.Checked)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.UserId = UserAccount.ID;
				obj.DueDateTo = datDueDateTo.Value;
				obj.Ids = GroupIds;
				obj.FromDocNo = Conversion.ToInt((object)txtBalFrom.Text);
				obj.ToDocNo = Conversion.ToInt((object)txtBalTo.Text);
				obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
				dtrpt = VoucherReports.DueByDatePayablesAndReceivables(obj);
				if (dtrpt.Rows.Count <= 0)
				{
					grd.DataSource = null;
					wf.Close();
					throw new Exception("Record Not Found For Display");
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("ReportCriteria");
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("AccountCode", typeof(int));
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("OtherLanguageTitle", typeof(string));
				dt.Columns.Add("ParentAccount", typeof(string));
				dt.Columns.Add("AccountType", typeof(string));
				dt.Columns.Add("AccountClass", typeof(string));
				dt.Columns.Add("Opening", typeof(double));
				dt.Columns.Add("CurrDebit", typeof(double));
				dt.Columns.Add("CurrCredit", typeof(double));
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("DebitAmount", typeof(double));
				dt.Columns.Add("CreditAmount", typeof(double));
				dt.Columns.Add("DueAmount", typeof(double));
				dt.Columns.Add("LastPaymentDate", typeof(DateTime));
				dt.Columns.Add("LastPaymentAmount", typeof(double));
				dt.Columns.Add("PaymentDays", typeof(double));
				dt.Columns.Add("LastBillDate", typeof(DateTime));
				dt.Columns.Add("LastBillsAmount", typeof(double));
				dt.Columns.Add("BillDays", typeof(double));
				dt.Clear();
				for (int i = 0; i < dtrpt.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtrpt.Rows[i]["ReportCriteria"]), Conversion.ToInt(dtrpt.Rows[i]["AccountId"]), Conversion.ToInt(dtrpt.Rows[i]["AccountCode"]), Conversion.ToString(dtrpt.Rows[i]["AccountTitle"]), Conversion.ToString(dtrpt.Rows[i]["OtherLanguageTitle"]), Conversion.ToString(dtrpt.Rows[i]["ParentAccount"]), Conversion.ToString(dtrpt.Rows[i]["AccountType"]), Conversion.ToString(dtrpt.Rows[i]["ClassName"]), 0, 0, 0, Conversion.ToInt(dtrpt.Rows[i]["DueDays"]), Conversion.ToDateTime(dtrpt.Rows[i]["DueDate"]), Conversion.ToDouble(dtrpt.Rows[i]["DebitAmount"]), Conversion.ToDouble(dtrpt.Rows[i]["CreditAmount"]), Conversion.ToDouble(dtrpt.Rows[i]["DueAmount"]), Conversion.ToDateTime(dtrpt.Rows[i]["LastPaymentDate"]), Conversion.ToDouble(dtrpt.Rows[i]["LastPaymentAmount"]), Conversion.ToDouble(dtrpt.Rows[i]["PaymentDays"]), Conversion.ToDateTime(dtrpt.Rows[i]["LastBillDate"]), Conversion.ToDouble(dtrpt.Rows[i]["LastBillsAmount"]), Conversion.ToDouble(dtrpt.Rows[i]["BillDays"]));
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSettings();
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
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Expected O, but got Unknown
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Expected O, but got Unknown
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_093e: Expected O, but got Unknown
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
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["ReportCriteria"].Visible = false;
			grd.RootTable.Columns["OtherLanguageTitle"].Visible = false;
			grd.RootTable.Columns["Opening"].Visible = false;
			grd.RootTable.Columns["CurrDebit"].Visible = false;
			grd.RootTable.Columns["CurrCredit"].Visible = false;
			grd.RootTable.Columns["AccountClass"].Visible = false;
			grd.RootTable.Columns["DueDate"].Visible = true;
			grd.RootTable.Columns["DebitAmount"].Visible = true;
			grd.RootTable.Columns["CreditAmount"].Visible = true;
			grd.RootTable.Columns["DueAmount"].Visible = true;
			grd.RootTable.Columns["AccountCode"].Width = 100;
			grd.RootTable.Columns["AccountTitle"].Width = 200;
			grd.RootTable.Columns["ParentAccount"].Width = 150;
			grd.RootTable.Columns["AccountType"].Width = 65;
			grd.RootTable.Columns["AccountClass"].Width = 80;
			grd.RootTable.Columns["DueDays"].Width = 50;
			grd.RootTable.Columns["DueDate"].Width = 80;
			grd.RootTable.Columns["DebitAmount"].Width = 110;
			grd.RootTable.Columns["CreditAmount"].Width = 110;
			grd.RootTable.Columns["DueAmount"].Width = 110;
			grd.RootTable.Columns["LastPaymentDate"].Width = 90;
			grd.RootTable.Columns["LastPaymentAmount"].Width = 110;
			grd.RootTable.Columns["PaymentDays"].Width = 70;
			grd.RootTable.Columns["LastBillDate"].Width = 80;
			grd.RootTable.Columns["LastBillsAmount"].Width = 110;
			grd.RootTable.Columns["BillDays"].Width = 50;
			grd.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["LastPaymentDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["LastBillDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["DueDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PaymentDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["DueAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DueAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DueAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["DueAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastPaymentAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LastPaymentAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LastPaymentAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastPaymentAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastBillsAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LastBillsAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LastBillsAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastBillsAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			if (chkGreen.Checked)
			{
				new GridEXColumn();
				GridEXFormatCondition fc3 = new GridEXFormatCondition(grd.RootTable.Columns["DueDays"], (ConditionOperator)6, (object)Conversion.ToInt((object)txtBlue.Text), (object)Conversion.ToInt((object)txtGreen.Text));
				fc3.FormatStyle.FontBold = (TriState)2;
				fc3.FormatStyle.ForeColor = Color.Green;
				grd.RootTable.FormatConditions.Add(fc3);
			}
			if (chkblue.Checked)
			{
				new GridEXColumn();
				GridEXFormatCondition fc2 = new GridEXFormatCondition(grd.RootTable.Columns["DueDays"], (ConditionOperator)6, (object)Conversion.ToInt((object)txtRed.Text), (object)Conversion.ToInt((object)txtBlue.Text));
				fc2.FormatStyle.FontBold = (TriState)2;
				fc2.FormatStyle.ForeColor = Color.Blue;
				grd.RootTable.FormatConditions.Add(fc2);
			}
			if (chkRed.Checked)
			{
				new GridEXColumn();
				GridEXFormatCondition fc = new GridEXFormatCondition(grd.RootTable.Columns["DueDays"], (ConditionOperator)5, (object)Conversion.ToInt((object)txtRed.Text));
				fc.FormatStyle.FontBold = (TriState)2;
				fc.FormatStyle.ForeColor = Color.Red;
				grd.RootTable.FormatConditions.Add(fc);
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			wf.Close();
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingsDueDateBetween()
	{
		try
		{
			grd.RootTable.Columns["Opening"].Width = 90;
			grd.RootTable.Columns["CurrDebit"].Width = 90;
			grd.RootTable.Columns["CurrCredit"].Width = 90;
			grd.RootTable.Columns["LastPaymentDate"].Width = 90;
			grd.RootTable.Columns["LastPaymentAmount"].Width = 110;
			grd.RootTable.Columns["LastBillDate"].Width = 80;
			grd.RootTable.Columns["LastBillsAmount"].Width = 110;
			grd.RootTable.ColumnSets["ParentAccount"].Visible = true;
			grd.RootTable.ColumnSets["AccountType"].Visible = true;
			grd.RootTable.ColumnSets["AccountClass"].Visible = true;
			grd.RootTable.ColumnSets["Opening"].Visible = true;
			grd.RootTable.ColumnSets["Debit"].Visible = true;
			grd.RootTable.ColumnSets["Credit"].Visible = true;
			grd.RootTable.ColumnSets["DueDays"].Visible = false;
			grd.RootTable.ColumnSets["DueDate"].Visible = false;
			grd.RootTable.ColumnSets["DebitAmount"].Visible = false;
			grd.RootTable.ColumnSets["CreditAmount"].Visible = false;
			grd.RootTable.ColumnSets["DueAmount"].Visible = false;
			grd.RootTable.ColumnSets["PaymentDays"].Visible = false;
			grd.RootTable.ColumnSets["BillDays"].Visible = false;
			grd.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Opening"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Opening"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["CurrDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CurrDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CurrDebit"].FormatString = "#,#";
			grd.RootTable.Columns["CurrDebit"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["CurrCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CurrCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CurrCredit"].FormatString = "#,#";
			grd.RootTable.Columns["CurrCredit"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["LastPaymentAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LastPaymentAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LastPaymentAmount"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["LastPaymentAmount"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["LastBillsAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LastBillsAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LastBillsAmount"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["LastBillsAmount"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			wf.Close();
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(grd.CurrentRow.Cells["AccountId"].Value), datDueDateFrom.Value, datDueDateTo.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBalFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBalTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGreen_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBlue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRed_KeyPress(object sender, KeyPressEventArgs e)
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

	private void VisibleColorFilter()
	{
		try
		{
			if (rdPayablesByDueDateBetween.Checked)
			{
				txtRed.Visible = false;
				txtGreen.Visible = false;
				txtBlue.Visible = false;
				chkRed.Visible = false;
				chkGreen.Visible = false;
				chkblue.Visible = false;
				label5.Visible = false;
			}
			else
			{
				txtRed.Visible = true;
				txtGreen.Visible = true;
				txtBlue.Visible = true;
				chkRed.Visible = true;
				chkGreen.Visible = true;
				chkblue.Visible = true;
				label5.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdPayablesByDueDateBetween_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			VisibleColorFilter();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdPayablesByDueDate_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			VisibleColorFilter();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PayablesByDueDate_FormClosing(object sender, FormClosingEventArgs e)
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

	private void cmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateType.Value) == 1)
			{
				datDueDateFrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				datDueDateFrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				datDueDateFrom.Value = first;
				datDueDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				datDueDateFrom.Value = first2;
				datDueDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				datDueDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			CustomerGroupFill();
			LanguageDropdownBind();
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

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grd")
			{
				cmbDateType.Focus();
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
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.PayablesByDueDate));
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
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
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.datDueDateTo = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.chkblue = new System.Windows.Forms.CheckBox();
		this.datDueDateFrom = new System.Windows.Forms.DateTimePicker();
		this.chkRed = new System.Windows.Forms.CheckBox();
		this.label29 = new System.Windows.Forms.Label();
		this.chkGreen = new System.Windows.Forms.CheckBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtRed = new System.Windows.Forms.TextBox();
		this.CmbGroup = new UltraCombo();
		this.txtBlue = new System.Windows.Forms.TextBox();
		this.rdPayablesByDueDate = new System.Windows.Forms.RadioButton();
		this.txtGreen = new System.Windows.Forms.TextBox();
		this.rdPayablesByDueDateBetween = new System.Windows.Forms.RadioButton();
		this.label5 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.lbllang = new System.Windows.Forms.Label();
		this.txtBalFrom = new System.Windows.Forms.TextBox();
		this.CmbLanguage = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.txtBalTo = new System.Windows.Forms.TextBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkey = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.rdPayablesByDueDate);
		this.panel3.Controls.Add(this.rdPayablesByDueDateBetween);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(967, 27);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(266, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Accounts Payables By Due Date";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(920, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(967, 440);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 54);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(967, 607);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 140);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(967, 467);
		this.panel6.TabIndex = 110;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(967, 27);
		this.panel7.TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(4, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 19);
		this.label4.TabIndex = 1;
		this.label4.Text = "History";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(967, 140);
		this.panel5.TabIndex = 0;
		this.groupBox1.Controls.Add(this.datDueDateTo);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.chkblue);
		this.groupBox1.Controls.Add(this.datDueDateFrom);
		this.groupBox1.Controls.Add(this.chkRed);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.chkGreen);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.txtRed);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGroup);
		this.groupBox1.Controls.Add(this.txtBlue);
		this.groupBox1.Controls.Add(this.txtGreen);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.lbllang);
		this.groupBox1.Controls.Add(this.txtBalFrom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtBalTo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(960, 127);
		this.groupBox1.TabIndex = 127;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.datDueDateTo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueDateTo.CustomFormat = "dd-MMM-yy";
		this.datDueDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDueDateTo.Location = new System.Drawing.Point(92, 100);
		this.datDueDateTo.Name = "datDueDateTo";
		this.datDueDateTo.Size = new System.Drawing.Size(201, 23);
		this.datDueDateTo.TabIndex = 3;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 20);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(60, 15);
		this.label9.TabIndex = 126;
		this.label9.Text = "Date Type";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(4, 104);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(73, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "Due Date To";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(92, 14);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 0;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 76);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 15);
		this.label1.TabIndex = 103;
		this.label1.Text = "Due Date From";
		this.chkblue.AutoSize = true;
		this.chkblue.Checked = true;
		this.chkblue.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkblue.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkblue.Location = new System.Drawing.Point(647, 52);
		this.chkblue.Name = "chkblue";
		this.chkblue.Size = new System.Drawing.Size(49, 19);
		this.chkblue.TabIndex = 10;
		this.chkblue.Text = "Blue";
		this.chkblue.UseVisualStyleBackColor = true;
		this.datDueDateFrom.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueDateFrom.CustomFormat = "dd-MMM-yy";
		this.datDueDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDueDateFrom.Location = new System.Drawing.Point(92, 72);
		this.datDueDateFrom.Name = "datDueDateFrom";
		this.datDueDateFrom.Size = new System.Drawing.Size(201, 23);
		this.datDueDateFrom.TabIndex = 2;
		this.chkRed.AutoSize = true;
		this.chkRed.Checked = true;
		this.chkRed.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkRed.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkRed.Location = new System.Drawing.Point(647, 78);
		this.chkRed.Name = "chkRed";
		this.chkRed.Size = new System.Drawing.Size(46, 19);
		this.chkRed.TabIndex = 12;
		this.chkRed.Text = "Red";
		this.chkRed.UseVisualStyleBackColor = true;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(4, 47);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(70, 15);
		this.label29.TabIndex = 106;
		this.label29.Text = "Party Group";
		this.chkGreen.AutoSize = true;
		this.chkGreen.Checked = true;
		this.chkGreen.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkGreen.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkGreen.Location = new System.Drawing.Point(646, 26);
		this.chkGreen.Name = "chkGreen";
		this.chkGreen.Size = new System.Drawing.Size(57, 19);
		this.chkGreen.TabIndex = 8;
		this.chkGreen.Text = "Green";
		this.chkGreen.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(522, 97);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(57, 26);
		this.btnshow.TabIndex = 15;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.txtRed.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRed.Location = new System.Drawing.Point(597, 76);
		this.txtRed.Name = "txtRed";
		this.txtRed.Size = new System.Drawing.Size(44, 23);
		this.txtRed.TabIndex = 11;
		this.txtRed.Text = "0";
		this.txtRed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRed_KeyPress);
		this.CmbGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbGroup).Location = new System.Drawing.Point(92, 41);
		((System.Windows.Forms.Control)(object)this.CmbGroup).Name = "CmbGroup";
		((System.Windows.Forms.Control)(object)this.CmbGroup).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbGroup).TabIndex = 1;
		this.txtBlue.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBlue.Location = new System.Drawing.Point(597, 50);
		this.txtBlue.Name = "txtBlue";
		this.txtBlue.Size = new System.Drawing.Size(44, 23);
		this.txtBlue.TabIndex = 9;
		this.txtBlue.Text = "5";
		this.txtBlue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBlue_KeyPress);
		this.rdPayablesByDueDate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdPayablesByDueDate.AutoSize = true;
		this.rdPayablesByDueDate.Checked = true;
		this.rdPayablesByDueDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.rdPayablesByDueDate.ForeColor = System.Drawing.Color.White;
		this.rdPayablesByDueDate.Location = new System.Drawing.Point(808, 5);
		this.rdPayablesByDueDate.Name = "rdPayablesByDueDate";
		this.rdPayablesByDueDate.Size = new System.Drawing.Size(153, 19);
		this.rdPayablesByDueDate.TabIndex = 13;
		this.rdPayablesByDueDate.TabStop = true;
		this.rdPayablesByDueDate.Text = "Payables By DueDate To";
		this.rdPayablesByDueDate.UseVisualStyleBackColor = true;
		this.rdPayablesByDueDate.Visible = false;
		this.rdPayablesByDueDate.CheckedChanged += new System.EventHandler(rdPayablesByDueDate_CheckedChanged);
		this.txtGreen.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGreen.Location = new System.Drawing.Point(597, 24);
		this.txtGreen.Name = "txtGreen";
		this.txtGreen.Size = new System.Drawing.Size(44, 23);
		this.txtGreen.TabIndex = 7;
		this.txtGreen.Text = "10";
		this.txtGreen.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGreen_KeyPress);
		this.rdPayablesByDueDateBetween.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdPayablesByDueDateBetween.AutoSize = true;
		this.rdPayablesByDueDateBetween.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.rdPayablesByDueDateBetween.ForeColor = System.Drawing.Color.White;
		this.rdPayablesByDueDateBetween.Location = new System.Drawing.Point(567, 5);
		this.rdPayablesByDueDateBetween.Name = "rdPayablesByDueDateBetween";
		this.rdPayablesByDueDateBetween.Size = new System.Drawing.Size(225, 19);
		this.rdPayablesByDueDateBetween.TabIndex = 14;
		this.rdPayablesByDueDateBetween.TabStop = true;
		this.rdPayablesByDueDateBetween.Text = "Payables By Due Date Between Period";
		this.rdPayablesByDueDateBetween.UseVisualStyleBackColor = true;
		this.rdPayablesByDueDateBetween.Visible = false;
		this.rdPayablesByDueDateBetween.CheckedChanged += new System.EventHandler(rdPayablesByDueDateBetween_CheckedChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(599, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(74, 15);
		this.label5.TabIndex = 116;
		this.label5.Text = "Due Days To";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(297, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(79, 15);
		this.label2.TabIndex = 108;
		this.label2.Text = "Balance From";
		this.lbllang.AutoSize = true;
		this.lbllang.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lbllang.Location = new System.Drawing.Point(297, 76);
		this.lbllang.Name = "lbllang";
		this.lbllang.Size = new System.Drawing.Size(59, 15);
		this.lbllang.TabIndex = 113;
		this.lbllang.Text = "Language";
		this.txtBalFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalFrom.Location = new System.Drawing.Point(378, 16);
		this.txtBalFrom.Name = "txtBalFrom";
		this.txtBalFrom.Size = new System.Drawing.Size(201, 23);
		this.txtBalFrom.TabIndex = 4;
		this.txtBalFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalFrom_KeyPress);
		this.CmbLanguage.AllowNull = (DefaultableBoolean)1;
		this.CmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(378, 70);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 6;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(297, 47);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 110;
		this.label3.Text = "Balance To";
		this.txtBalTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalTo.Location = new System.Drawing.Point(378, 43);
		this.txtBalTo.Name = "txtBalTo";
		this.txtBalTo.Size = new System.Drawing.Size(201, 23);
		this.txtBalTo.TabIndex = 5;
		this.txtBalTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalTo_KeyPress);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(967, 661);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(967, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.print, this.btnshortcutkey });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(967, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(135, 24);
		this.print.Text = "142 && 159-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btnshortcutkey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnshortcutkey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkey.Image = (System.Drawing.Image)resources.GetObject("btnshortcutkey.Image");
		this.btnshortcutkey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkey.Name = "btnshortcutkey";
		this.btnshortcutkey.Size = new System.Drawing.Size(106, 24);
		this.btnshortcutkey.Text = "ShortCut Keys";
		this.btnshortcutkey.Click += new System.EventHandler(btnshortcutkey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(967, 661);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PayablesByDueDate";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PayablesByDueDate_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
