// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
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

public class PayablesandReceivablesAging : Form
{
	private DropDownBind DDL = new DropDownBind();

	private int AcPayables;

	private int AcReceievables;

	private DataTable dtAccount = new DataTable();

	private DataTable dtCustome = new DataTable();

	private DataTable dtCustomeGroupAll = new DataTable();

	private DataTable dt = new DataTable();

	public int ReceivablePayblesStatus;

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtBillDetail = new DataTable();

	private DataTable dtreceive = new DataTable();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private Panel panel2;

	internal GridEX grd;

	private Panel panel7;

	private RadioButton RBAccountsReceievables;

	private RadioButton RBAccountsPayables;

	private RadioButton RBBalanceClassification;

	private RadioButton RBAccountClassification;

	private Label label8;

	internal UltraCombo cmbAccount;

	private Label label5;

	private Label label4;

	private TextBox txtNotEqualTo;

	private TextBox txtIntervalDays;

	private Button btnshow;

	private Label label33;

	private Label label27;

	private Panel panel3;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton1;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private DateTimePicker txtToDate;

	private DateTimePicker txtFromDate;

	private Label ReportParameters;

	private Panel panel1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private Panel panel4;

	private Button btnFilter;

	private Label label1;

	internal UltraCombo CmbCustomeGroups;

	private ToolStripButton btnRefresh;

	public PayablesandReceivablesAging(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void print_Click_1(object sender, EventArgs e)
	{
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtToDate.Value = Conversion.ToDateTime((object)DateTime.Now);
			txtIntervalDays.Text = string.Empty;
			txtNotEqualTo.Text = string.Empty;
			((Control)(object)CmbCustomeGroups).Text = string.Empty;
			((UltraGridBase)cmbAccount).DataSource = null;
			RBAccountClassification.Checked = false;
			RBAccountsPayables.Checked = false;
			RBAccountsReceievables.Checked = false;
			RBBalanceClassification.Checked = false;
			((Control)(object)cmbAccount).Text = string.Empty;
			((UltraGridBase)cmbAccount).DataSource = null;
			grd.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridBind()
	{
		try
		{
			if (Conversion.ToInt((object)txtNotEqualTo.Text) < 0)
			{
				MessageBox.Show("Please Check Not Equal To Field ! It should be Greater then '0'");
				return;
			}
			dt.Rows.Clear();
			if (((Control)(object)cmbAccount).Text != null && ((Control)(object)cmbAccount).Text != string.Empty)
			{
				DataTable dtcus2 = new DataTable();
				dtcus2.Columns.Add("Ac3LevelTitle");
				dtcus2.Rows.Clear();
				string[] array = (Conversion.ToString((object)((Control)(object)cmbAccount).Text.Trim()) + ",").ToString().Split(',');
				foreach (string author2 in array)
				{
					dtcus2.Rows.Add(author2);
				}
				if (dtreceive.Rows.Count > 0)
				{
					for (int m = 0; m < dtcus2.Rows.Count; m++)
					{
						for (int k = 0; k < dtreceive.Rows.Count; k++)
						{
							if (Conversion.ToString(dtcus2.Rows[m]["Ac3LevelTitle"]) == Conversion.ToString(dtreceive.Rows[k]["Ac3LevelTitle"]))
							{
								dt.Rows.Add(dtreceive.Rows[k]["ArApTypeDesc"], dtreceive.Rows[k]["Ac1LevelCode"], dtreceive.Rows[k]["Ac1LevelTitle"], dtreceive.Rows[k]["Ac2LevelCode"], dtreceive.Rows[k]["Ac2LevelTitle"], dtreceive.Rows[k]["Ac3LevelCode"], dtreceive.Rows[k]["Ac3LevelTitle"], dtreceive.Rows[k]["AcId"], dtreceive.Rows[k]["Ac4LevelCode"], dtreceive.Rows[k]["AccountTitle"], dtreceive.Rows[k]["Opening"], dtreceive.Rows[k]["Interval1St"], dtreceive.Rows[k]["Interval2nd"], dtreceive.Rows[k]["Interval3rd"], dtreceive.Rows[k]["IntervalAbove"], dtreceive.Rows[k]["Closing"], dtreceive.Rows[k]["RCounter"], dtreceive.Rows[k]["ReportCriteria"]);
							}
						}
					}
				}
			}
			if (((UltraGridBase)CmbCustomeGroups).ActiveRow != null)
			{
				DataTable dtcus = new DataTable();
				dtcus.Columns.Add("ChartOfAccountId");
				dtcus.Rows.Clear();
				string[] array = (Conversion.ToString((object)((Control)(object)CmbCustomeGroups).Text.Trim()) + ",").ToString().Split(',');
				foreach (string author in array)
				{
					if (dtCustomeGroupAll.Rows.Count <= 0)
					{
						continue;
					}
					for (int j = 0; j < dtCustomeGroupAll.Rows.Count; j++)
					{
						if (Conversion.ToString(dtCustomeGroupAll.Rows[j]["AcLookUpsDescription"]) == author)
						{
							dtcus.Rows.Add(dtCustomeGroupAll.Rows[j]["ChartOfAccountId"]);
						}
					}
				}
				if (dtCustomeGroupAll.Rows.Count > 0 && dtreceive.Rows.Count > 0)
				{
					for (int l = 0; l < dtcus.Rows.Count; l++)
					{
						for (int i = 0; i < dtreceive.Rows.Count; i++)
						{
							if (Conversion.ToString(dtcus.Rows[l]["ChartOfAccountId"]) == Conversion.ToString(dtreceive.Rows[i]["AcId"]))
							{
								dt.Rows.Add(dtreceive.Rows[i]["ArApTypeDesc"], dtreceive.Rows[i]["Ac1LevelCode"], dtreceive.Rows[i]["Ac1LevelTitle"], dtreceive.Rows[i]["Ac2LevelCode"], dtreceive.Rows[i]["Ac2LevelTitle"], dtreceive.Rows[i]["Ac3LevelCode"], dtreceive.Rows[i]["Ac3LevelTitle"], dtreceive.Rows[i]["AcId"], dtreceive.Rows[i]["Ac4LevelCode"], dtreceive.Rows[i]["AccountTitle"], dtreceive.Rows[i]["Opening"], dtreceive.Rows[i]["Interval1St"], dtreceive.Rows[i]["Interval2nd"], dtreceive.Rows[i]["Interval3rd"], dtreceive.Rows[i]["IntervalAbove"], dtreceive.Rows[i]["Closing"], dtreceive.Rows[i]["RCounter"], dtreceive.Rows[i]["ReportCriteria"]);
							}
						}
					}
				}
			}
			grd.DataSource = dt;
			grd.RetrieveStructure();
			ReportParameters.Text = Conversion.ToString((object)dtreceive.Rows[0]["ReportCriteria"].ToString());
			GridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridBindForShow()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt((object)txtNotEqualTo.Text) < 0)
			{
				MessageBox.Show("Please Check Not Equal To Field ! It should be Greater then '0'");
				return;
			}
			ReportsParameters vh = new ReportsParameters();
			if (RBAccountClassification.Checked)
			{
				vh.ItemClassId = 1;
			}
			if (RBBalanceClassification.Checked)
			{
				vh.ItemClassId = 2;
			}
			if (RBAccountsPayables.Checked)
			{
				vh.ItemTypeId = 1;
			}
			if (RBAccountsReceievables.Checked)
			{
				vh.ItemTypeId = 2;
			}
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.ToDate = txtToDate.Value;
			vh.IntervalDays = Conversion.ToInt((object)txtIntervalDays.Text);
			vh.DocNo = Conversion.ToInt((object)txtNotEqualTo.Text);
			dtreceive = VoucherHead.PayablesandReceivablesMenagment(vh);
			dt.Rows.Clear();
			for (int i = 0; i < dtreceive.Rows.Count; i++)
			{
				dt.Rows.Add(dtreceive.Rows[i]["ArApTypeDesc"], dtreceive.Rows[i]["Ac1LevelCode"], dtreceive.Rows[i]["Ac1LevelTitle"], dtreceive.Rows[i]["Ac2LevelCode"], dtreceive.Rows[i]["Ac2LevelTitle"], dtreceive.Rows[i]["Ac3LevelCode"], dtreceive.Rows[i]["Ac3LevelTitle"], dtreceive.Rows[i]["AcId"], dtreceive.Rows[i]["Ac4LevelCode"], dtreceive.Rows[i]["AccountTitle"], dtreceive.Rows[i]["Opening"], dtreceive.Rows[i]["Interval1St"], dtreceive.Rows[i]["Interval2nd"], dtreceive.Rows[i]["Interval3rd"], dtreceive.Rows[i]["IntervalAbove"], dtreceive.Rows[i]["Closing"], dtreceive.Rows[i]["RCounter"], dtreceive.Rows[i]["ReportCriteria"]);
			}
			grd.DataSource = dt;
			grd.RetrieveStructure();
			ReportParameters.Text = Conversion.ToString((object)dtreceive.Rows[0]["ReportCriteria"].ToString());
			GridSettings();
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
			DataGridBindForShow();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grd.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Closing"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Closing"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Opening"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Opening"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Interval1St"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Interval1St"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Interval1St"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Interval1St"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Interval2nd"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Interval2nd"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Interval2nd"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Interval2nd"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Interval3rd"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Interval3rd"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Interval3rd"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Interval3rd"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["IntervalAbove"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["IntervalAbove"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["IntervalAbove"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["IntervalAbove"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["ArApTypeDesc"].Visible = false;
			grd.RootTable.Columns["Ac1LevelCode"].Visible = false;
			grd.RootTable.Columns["Ac2LevelCode"].Visible = false;
			grd.RootTable.Columns["Ac3LevelCode"].Visible = false;
			grd.RootTable.Columns["AcId"].Visible = false;
			grd.RootTable.Columns["ReportCriteria"].Visible = false;
			grd.RootTable.Columns["RCounter"].Visible = false;
			grd.RootTable.Columns["Ac1LevelTitle"].Visible = false;
			grd.RootTable.Columns["Ac2LevelTitle"].Visible = false;
			grd.RootTable.Columns["Ac3LevelTitle"].Visible = false;
			grd.RootTable.Columns["Ac4LevelCode"].Caption = "AccountCode";
			grd.RootTable.Columns["AccountTitle"].Width = 300;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
			dt.Columns.Add("ArApTypeDesc");
			dt.Columns.Add("Ac1LevelCode");
			dt.Columns.Add("Ac1LevelTitle");
			dt.Columns.Add("Ac2LevelCode");
			dt.Columns.Add("Ac2LevelTitle");
			dt.Columns.Add("Ac3LevelCode");
			dt.Columns.Add("Ac3LevelTitle");
			dt.Columns.Add("AcId");
			dt.Columns.Add("Ac4LevelCode");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("Opening", typeof(double));
			dt.Columns.Add("Interval1St", typeof(double));
			dt.Columns.Add("Interval2nd", typeof(double));
			dt.Columns.Add("Interval3rd", typeof(double));
			dt.Columns.Add("IntervalAbove", typeof(double));
			dt.Columns.Add("Closing", typeof(double));
			dt.Columns.Add("RCounter");
			dt.Columns.Add("ReportCriteria");
			txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			DataGridBindForShow();
			CustomeGroupsDefine();
			CustomerGroupsData();
			if (ReceivablePayblesStatus == 1)
			{
				RBAccountClassification.Checked = true;
				RBAccountsReceievables.Checked = true;
				RBAccountsReceievables_Click(null, null);
			}
			if (ReceivablePayblesStatus == 2)
			{
				RBAccountClassification.Checked = true;
				RBAccountsPayables.Checked = true;
				RBAccountsPayables_Click(null, null);
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
			dtAccount = ChartofAccount.ReadAll3rdLevelAccountsForPayablesandReceeivablesAging(obj);
			if (dtAccount.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			((UltraGridBase)cmbAccount).DataSource = null;
			AcReceievables = 0;
			AcPayables = 0;
			if (RBAccountsPayables.Checked)
			{
				AcPayables = 1;
			}
			if (RBAccountsReceievables.Checked)
			{
				AcReceievables = 2;
			}
			for (int i = 0; i < dtAccount.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAccount.Rows[i]["TypeNo"]) == Conversion.ToInt((object)AcPayables))
				{
					dt.Rows.Add(dtAccount.Rows[i]["AccountCode"], dtAccount.Rows[i]["AccountTitle"]);
				}
				if (Conversion.ToInt(dtAccount.Rows[i]["TypeNo"]) == Conversion.ToInt((object)AcReceievables))
				{
					dt.Rows.Add(dtAccount.Rows[i]["AccountCode"], dtAccount.Rows[i]["AccountTitle"]);
				}
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
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomeGroupsDefine()
	{
		try
		{
			dtCustome = CommonServices.CustomeGroupsDefine(1);
			if (dtCustome.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCustome, CmbCustomeGroups, "Id", "AcLookUpsDescription", "Account Title", false);
				((UltraGridBase)CmbCustomeGroups).DisplayLayout.Bands[0].Columns[1].Width = 336;
				UltraGridColumn obj = ((UltraGridBase)CmbCustomeGroups).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbCustomeGroups.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbCustomeGroups.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbCustomeGroups.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbCustomeGroups.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerGroupsData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtCustomeGroupAll = AccountsCustomGroups.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsClassificationPayables()
	{
		try
		{
			if (dtreceive.Rows.Count <= 0)
			{
				return;
			}
			dt.Rows.Clear();
			for (int i = 0; i < dtreceive.Rows.Count; i++)
			{
				if (RBAccountsPayables.Checked && Conversion.ToInt(dtreceive.Rows[i]["accountClass"]) == 3)
				{
					dt.Rows.Add(dtreceive.Rows[i]["ArApTypeDesc"], dtreceive.Rows[i]["Ac1LevelCode"], dtreceive.Rows[i]["Ac1LevelTitle"], dtreceive.Rows[i]["Ac2LevelCode"], dtreceive.Rows[i]["Ac2LevelTitle"], dtreceive.Rows[i]["Ac3LevelCode"], dtreceive.Rows[i]["Ac3LevelTitle"], dtreceive.Rows[i]["AcId"], dtreceive.Rows[i]["Ac4LevelCode"], dtreceive.Rows[i]["AccountTitle"], dtreceive.Rows[i]["Opening"], dtreceive.Rows[i]["Interval1St"], dtreceive.Rows[i]["Interval2nd"], dtreceive.Rows[i]["Interval3rd"], dtreceive.Rows[i]["IntervalAbove"], dtreceive.Rows[i]["Closing"], dtreceive.Rows[i]["RCounter"], dtreceive.Rows[i]["ReportCriteria"]);
				}
				if (RBAccountsReceievables.Checked && Conversion.ToInt(dtreceive.Rows[i]["accountClass"]) == 2)
				{
					dt.Rows.Add(dtreceive.Rows[i]["ArApTypeDesc"], dtreceive.Rows[i]["Ac1LevelCode"], dtreceive.Rows[i]["Ac1LevelTitle"], dtreceive.Rows[i]["Ac2LevelCode"], dtreceive.Rows[i]["Ac2LevelTitle"], dtreceive.Rows[i]["Ac3LevelCode"], dtreceive.Rows[i]["Ac3LevelTitle"], dtreceive.Rows[i]["AcId"], dtreceive.Rows[i]["Ac4LevelCode"], dtreceive.Rows[i]["AccountTitle"], dtreceive.Rows[i]["Opening"], dtreceive.Rows[i]["Interval1St"], dtreceive.Rows[i]["Interval2nd"], dtreceive.Rows[i]["Interval3rd"], dtreceive.Rows[i]["IntervalAbove"], dtreceive.Rows[i]["Closing"], dtreceive.Rows[i]["RCounter"], dtreceive.Rows[i]["ReportCriteria"]);
				}
			}
			grd.DataSource = dt;
			grd.RetrieveStructure();
			ReportParameters.Text = Conversion.ToString((object)dtreceive.Rows[0]["ReportCriteria"].ToString());
			GridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BalanceClassificationPayablesandReceivables()
	{
		try
		{
			if (dtreceive.Rows.Count <= 0)
			{
				return;
			}
			dt.Rows.Clear();
			for (int i = 0; i < dtreceive.Rows.Count; i++)
			{
				if (RBAccountsPayables.Checked && Conversion.ToString(dtreceive.Rows[i]["ArApTypeDesc"]) == "Payables")
				{
					dt.Rows.Add(dtreceive.Rows[i]["ArApTypeDesc"], dtreceive.Rows[i]["Ac1LevelCode"], dtreceive.Rows[i]["Ac1LevelTitle"], dtreceive.Rows[i]["Ac2LevelCode"], dtreceive.Rows[i]["Ac2LevelTitle"], dtreceive.Rows[i]["Ac3LevelCode"], dtreceive.Rows[i]["Ac3LevelTitle"], dtreceive.Rows[i]["AcId"], dtreceive.Rows[i]["Ac4LevelCode"], dtreceive.Rows[i]["AccountTitle"], dtreceive.Rows[i]["Opening"], dtreceive.Rows[i]["Interval1St"], dtreceive.Rows[i]["Interval2nd"], dtreceive.Rows[i]["Interval3rd"], dtreceive.Rows[i]["IntervalAbove"], dtreceive.Rows[i]["Closing"], dtreceive.Rows[i]["RCounter"], dtreceive.Rows[i]["ReportCriteria"]);
				}
				if (RBAccountsReceievables.Checked && Conversion.ToString(dtreceive.Rows[i]["ArApTypeDesc"]) == "Receivables")
				{
					dt.Rows.Add(dtreceive.Rows[i]["ArApTypeDesc"], dtreceive.Rows[i]["Ac1LevelCode"], dtreceive.Rows[i]["Ac1LevelTitle"], dtreceive.Rows[i]["Ac2LevelCode"], dtreceive.Rows[i]["Ac2LevelTitle"], dtreceive.Rows[i]["Ac3LevelCode"], dtreceive.Rows[i]["Ac3LevelTitle"], dtreceive.Rows[i]["AcId"], dtreceive.Rows[i]["Ac4LevelCode"], dtreceive.Rows[i]["AccountTitle"], dtreceive.Rows[i]["Opening"], dtreceive.Rows[i]["Interval1St"], dtreceive.Rows[i]["Interval2nd"], dtreceive.Rows[i]["Interval3rd"], dtreceive.Rows[i]["IntervalAbove"], dtreceive.Rows[i]["Closing"], dtreceive.Rows[i]["RCounter"], dtreceive.Rows[i]["ReportCriteria"]);
				}
			}
			grd.DataSource = dt;
			grd.RetrieveStructure();
			ReportParameters.Text = Conversion.ToString((object)dtreceive.Rows[0]["ReportCriteria"].ToString());
			GridSettings();
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

	private void RBAccountsPayables_Click(object sender, EventArgs e)
	{
		try
		{
			AccountFill3rdLevel();
			if (RBAccountClassification.Checked && RBAccountsPayables.Checked)
			{
				AccountsClassificationPayables();
			}
			if (RBAccountClassification.Checked && RBAccountsReceievables.Checked)
			{
				AccountsClassificationPayables();
			}
			if (RBBalanceClassification.Checked && RBAccountsPayables.Checked)
			{
				BalanceClassificationPayablesandReceivables();
			}
			if (RBBalanceClassification.Checked && RBAccountsReceievables.Checked)
			{
				BalanceClassificationPayablesandReceivables();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RBAccountsReceievables_Click(object sender, EventArgs e)
	{
		try
		{
			AccountFill3rdLevel();
			if (RBAccountClassification.Checked && RBAccountsPayables.Checked)
			{
				AccountsClassificationPayables();
			}
			if (RBAccountClassification.Checked && RBAccountsReceievables.Checked)
			{
				AccountsClassificationPayables();
			}
			if (RBBalanceClassification.Checked && RBAccountsPayables.Checked)
			{
				BalanceClassificationPayablesandReceivables();
			}
			if (RBBalanceClassification.Checked && RBAccountsReceievables.Checked)
			{
				BalanceClassificationPayablesandReceivables();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RBAccountClassification_Click(object sender, EventArgs e)
	{
		try
		{
			if (RBAccountClassification.Checked && RBAccountsPayables.Checked)
			{
				AccountsClassificationPayables();
			}
			if (RBAccountClassification.Checked && RBAccountsReceievables.Checked)
			{
				AccountsClassificationPayables();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RBBalanceClassification_Click(object sender, EventArgs e)
	{
		if (RBBalanceClassification.Checked && RBAccountsPayables.Checked)
		{
			BalanceClassificationPayablesandReceivables();
		}
		if (RBBalanceClassification.Checked && RBAccountsReceievables.Checked)
		{
			BalanceClassificationPayablesandReceivables();
		}
	}

	private void grd3rdLevelAccounts_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
	}

	private void btnFilter_Click(object sender, EventArgs e)
	{
		DataGridBind();
	}

	private void CmbCustomeGroups_ValueChanged(object sender, EventArgs e)
	{
		((Control)(object)cmbAccount).Text = string.Empty;
	}

	private void cmbAccount_ValueChanged(object sender, EventArgs e)
	{
		((Control)(object)CmbCustomeGroups).Text = string.Empty;
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		DataGridBindForShow();
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "154-Payables&Receiveableaging-Rpt.rpt");
				return;
			}
			throw new Exception("Record not found for Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PayablesandReceivablesAging_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbCustomeGroups = new UltraCombo();
		this.btnFilter = new System.Windows.Forms.Button();
		this.RBAccountsPayables = new System.Windows.Forms.RadioButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.RBAccountClassification = new System.Windows.Forms.RadioButton();
		this.RBBalanceClassification = new System.Windows.Forms.RadioButton();
		this.label27 = new System.Windows.Forms.Label();
		this.ReportParameters = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.txtIntervalDays = new System.Windows.Forms.TextBox();
		this.RBAccountsReceievables = new System.Windows.Forms.RadioButton();
		this.txtNotEqualTo = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbAccount = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomeGroups).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(375, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1325, 27);
		this.miniToolStrip.TabIndex = 5;
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1136, 578);
		this.panel2.TabIndex = 1;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.panel5);
		this.panel7.Controls.Add(this.panel4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 29);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1136, 549);
		this.panel7.TabIndex = 112;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 148);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1136, 401);
		this.panel5.TabIndex = 129;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1136, 401);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomeGroups);
		this.panel4.Controls.Add(this.btnFilter);
		this.panel4.Controls.Add(this.RBAccountsPayables);
		this.panel4.Controls.Add(this.panel1);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.ReportParameters);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.txtToDate);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.txtFromDate);
		this.panel4.Controls.Add(this.txtIntervalDays);
		this.panel4.Controls.Add(this.RBAccountsReceievables);
		this.panel4.Controls.Add(this.txtNotEqualTo);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccount);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1136, 148);
		this.panel4.TabIndex = 128;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(623, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(94, 15);
		this.label1.TabIndex = 130;
		this.label1.Text = "CustomeGroups";
		this.CmbCustomeGroups.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomeGroups.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomeGroups.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomeGroups.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Location = new System.Drawing.Point(623, 33);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Name = "CmbCustomeGroups";
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).TabIndex = 129;
		this.CmbCustomeGroups.ValueChanged += new System.EventHandler(CmbCustomeGroups_ValueChanged);
		this.btnFilter.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnFilter.Location = new System.Drawing.Point(595, 88);
		this.btnFilter.Margin = new System.Windows.Forms.Padding(2);
		this.btnFilter.Name = "btnFilter";
		this.btnFilter.Size = new System.Drawing.Size(112, 23);
		this.btnFilter.TabIndex = 128;
		this.btnFilter.Text = "3rd Level Filter";
		this.btnFilter.UseVisualStyleBackColor = true;
		this.btnFilter.Click += new System.EventHandler(btnFilter_Click);
		this.RBAccountsPayables.AutoSize = true;
		this.RBAccountsPayables.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.RBAccountsPayables.Location = new System.Drawing.Point(172, 16);
		this.RBAccountsPayables.Name = "RBAccountsPayables";
		this.RBAccountsPayables.Size = new System.Drawing.Size(128, 19);
		this.RBAccountsPayables.TabIndex = 0;
		this.RBAccountsPayables.TabStop = true;
		this.RBAccountsPayables.Text = "Accounts Payables";
		this.RBAccountsPayables.UseVisualStyleBackColor = true;
		this.RBAccountsPayables.Click += new System.EventHandler(RBAccountsPayables_Click);
		this.panel1.Controls.Add(this.RBAccountClassification);
		this.panel1.Controls.Add(this.RBBalanceClassification);
		this.panel1.Location = new System.Drawing.Point(15, 10);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(151, 49);
		this.panel1.TabIndex = 127;
		this.RBAccountClassification.AutoSize = true;
		this.RBAccountClassification.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.RBAccountClassification.Location = new System.Drawing.Point(3, 6);
		this.RBAccountClassification.Name = "RBAccountClassification";
		this.RBAccountClassification.Size = new System.Drawing.Size(147, 19);
		this.RBAccountClassification.TabIndex = 0;
		this.RBAccountClassification.Text = "Account Classification";
		this.RBAccountClassification.UseVisualStyleBackColor = true;
		this.RBAccountClassification.Click += new System.EventHandler(RBAccountClassification_Click);
		this.RBBalanceClassification.AutoSize = true;
		this.RBBalanceClassification.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.RBBalanceClassification.Location = new System.Drawing.Point(3, 29);
		this.RBBalanceClassification.Name = "RBBalanceClassification";
		this.RBBalanceClassification.Size = new System.Drawing.Size(147, 19);
		this.RBBalanceClassification.TabIndex = 1;
		this.RBBalanceClassification.Text = "Balance Classification";
		this.RBBalanceClassification.UseVisualStyleBackColor = true;
		this.RBBalanceClassification.Click += new System.EventHandler(RBBalanceClassification_Click);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(153, 70);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.ReportParameters.AutoSize = true;
		this.ReportParameters.ForeColor = System.Drawing.Color.Blue;
		this.ReportParameters.Location = new System.Drawing.Point(21, 125);
		this.ReportParameters.Name = "ReportParameters";
		this.ReportParameters.Size = new System.Drawing.Size(35, 13);
		this.ReportParameters.TabIndex = 126;
		this.ReportParameters.Text = "label1";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(291, 70);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(47, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.txtToDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(291, 88);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(135, 23);
		this.txtToDate.TabIndex = 5;
		this.btnshow.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.Location = new System.Drawing.Point(536, 88);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(57, 23);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = " Show";
		this.btnshow.UseVisualStyleBackColor = true;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtFromDate.Enabled = false;
		this.txtFromDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDate.Location = new System.Drawing.Point(153, 88);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(135, 23);
		this.txtFromDate.TabIndex = 4;
		this.txtIntervalDays.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIntervalDays.Location = new System.Drawing.Point(21, 88);
		this.txtIntervalDays.Name = "txtIntervalDays";
		this.txtIntervalDays.Size = new System.Drawing.Size(128, 23);
		this.txtIntervalDays.TabIndex = 3;
		this.RBAccountsReceievables.AutoSize = true;
		this.RBAccountsReceievables.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.RBAccountsReceievables.Location = new System.Drawing.Point(172, 39);
		this.RBAccountsReceievables.Name = "RBAccountsReceievables";
		this.RBAccountsReceievables.Size = new System.Drawing.Size(149, 19);
		this.RBAccountsReceievables.TabIndex = 1;
		this.RBAccountsReceievables.TabStop = true;
		this.RBAccountsReceievables.Text = "Accounts Receievables";
		this.RBAccountsReceievables.UseVisualStyleBackColor = true;
		this.RBAccountsReceievables.Click += new System.EventHandler(RBAccountsReceievables_Click);
		this.txtNotEqualTo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNotEqualTo.Location = new System.Drawing.Point(429, 88);
		this.txtNotEqualTo.Name = "txtNotEqualTo";
		this.txtNotEqualTo.Size = new System.Drawing.Size(103, 23);
		this.txtNotEqualTo.TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(21, 70);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 15);
		this.label4.TabIndex = 114;
		this.label4.Text = "Interval Days";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(327, 10);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(113, 15);
		this.label8.TabIndex = 119;
		this.label8.Text = "Receivable Account";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(426, 70);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 15);
		this.label5.TabIndex = 115;
		this.label5.Text = "Not Equal To";
		this.cmbAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccount).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Location = new System.Drawing.Point(327, 32);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Name = "cmbAccount";
		((System.Windows.Forms.Control)(object)this.cmbAccount).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccount).TabIndex = 2;
		this.cmbAccount.ValueChanged += new System.EventHandler(cmbAccount_ValueChanged);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.toolStrip3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1136, 29);
		this.panel3.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1087, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 112;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripButton1, this.btnNew, this.print, this.btnRefresh });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1136, 27);
		this.toolStrip3.TabIndex = 111;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripButton1.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(233, 24);
		this.toolStripButton1.Text = "Payables and Receivables Aging ";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(79, 24);
		this.print.Text = "154-&Print";
		this.print.Click += new System.EventHandler(print_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1136, 578);
		base.Controls.Add(this.panel2);
		this.DoubleBuffered = true;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PayablesandReceivablesAging";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PayablesandReceivablesAging_FormClosing);
		base.Load += new System.EventHandler(Receivables_Load);
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomeGroups).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
