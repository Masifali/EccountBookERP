using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Common;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class ActicitySummery : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	public DateTime FromDate;

	public DateTime ToDate;

	private DataTable dtReport = new DataTable();

	private DataTable dtDocType = new DataTable();

	private bool SubsidiaryAllown;

	private string DocumenTypeIds = "";

	public int CompanyIdFromBreakUp;

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	public string BranchesIds = "";

	private UserAccount UserAccount = new UserAccount();

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

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private ToolStripButton btnActivityDetail;

	internal UltraCombo CmbDocumentType;

	private Label label1;

	private Button btnshow;

	private CheckBox ChkBoxPost;

	private ToolStripButton btnActivitysummaryMultisubPrint;

	private ToolStripButton btnReport110B;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnshortcutkey;

	private Label label9;

	internal UltraCombo cmbDateType;

	private GroupBox groupBox1;

	private Panel panel7;

	private Label label2;

	private RadioButton rdSixColumns;

	private RadioButton rdFourColumns;

	private Label labelReportype;

	internal UltraCombo CmbReportType;

	private GroupBox groupBox2;

	private RadioButton rdEntryFromToDate;

	private RadioButton rdDocFromToDate;

	internal UltraCombo CmbLanguage;

	private Label lblLanguage;

	internal UltraCombo CmbBranches;

	private Label Branches;

	public ActicitySummery(UserAccount user)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			UserAccount.CompanyId = ((!UserAccount.IsHeadOffice) ? UserAccount.CompanyId : ((CompanyIdFromBreakUp > 0) ? CompanyIdFromBreakUp : UserAccount.CompanyId));
			((Control)(object)CmbReportType).Visible = false;
			labelReportype.Visible = false;
			((Control)(object)CmbReportType).Text = string.Empty;
			((UltraGridBase)CmbReportType).DataSource = null;
			SubsidiaryAllown = CommonServices.GetERPFeatureById(4);
			if (SubsidiaryAllown)
			{
				((Control)(object)CmbReportType).Visible = true;
				labelReportype.Visible = true;
				ReportTypefill();
			}
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
			DocumenttypeFill();
			Datetypefill();
			MultiLanguagesGetAll();
			if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
			{
				fromdate.Value = Conversion.ToDateTime((object)FromDate);
				todate.Value = Conversion.ToDateTime((object)ToDate);
				HistoryFill();
			}
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

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateType, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateType).Rows[0].Activate();
			((UltraGridBase)cmbDateType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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

	private void ReportTypefill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ReportType");
			dt.Rows.Add(1, "ChartofAccount_Wise");
			dt.Rows.Add(2, "Subsidiary_Wise");
			DDL.BindDDL(dt, CmbReportType, "Id", "ReportType", "ReportType", ZeroIndex: false);
			((UltraGridBase)CmbReportType).Rows[0].Activate();
			((UltraGridBase)CmbReportType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DocumenttypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			VoucherHead obj = new VoucherHead();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtDocType = VoucherHead.GetDocumentTypesFromVouchers(obj);
			if (dtDocType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDocType, CmbDocumentType, "Id", "DocumentTypeDescription", "Document Type", false);
				UltraGridColumn obj2 = ((UltraGridBase)CmbDocumentType).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				CmbDocumentType.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbDocumentType.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbDocumentType.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbDocumentType.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
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

	private void HistoryFill()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			dtReport.Rows.Clear();
			DocumenTypeIds = "";
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (rdDocFromToDate.Checked)
			{
				vh.FromDate = Conversion.ToDateTime((object)fromdate.Value);
				vh.ToDate = Conversion.ToDateTime((object)todate.Value);
			}
			else if (rdEntryFromToDate.Checked)
			{
				vh.EntryDateFrom = Conversion.ToDateTime((object)fromdate.Value);
				vh.EntryDateTo = Conversion.ToDateTime((object)todate.Value);
			}
			if (!ChkBoxPost.Checked)
			{
				vh.IsApproved = true;
			}
			else
			{
				vh.ApprovedFilter = "All";
			}
			if (SubsidiaryAllown)
			{
				vh.ReportTypeId = Conversion.ToInt(CmbReportType.Value);
			}
			if (Conversion.ToString((object)((Control)(object)CmbDocumentType).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbDocumentType).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtDocType.Select("DocumentTypeDescription='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						DocumenTypeIds = DocumenTypeIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				vh.Ids = DocumenTypeIds;
			}
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			vh.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			vh.LanguageId = Conversion.ToInt(CmbLanguage.Value);
			dtReport = VoucherReports.ActivitySummaryReport(vh);
			if (dtReport.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("BranchesId", typeof(int));
				dt.Columns.Add("BranchName", typeof(string));
				dt.Columns.Add("ParentAccount", typeof(string));
				dt.Columns.Add("ParentAccountTitle", typeof(string));
				dt.Columns.Add("AccountCode", typeof(string));
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("SubsidiaryAccountTitle", typeof(string));
				dt.Columns.Add("OpeningDr", typeof(double));
				dt.Columns.Add("OpeningCr", typeof(double));
				dt.Columns.Add("Opening", typeof(double));
				dt.Columns.Add("Debit", typeof(double));
				dt.Columns.Add("Credit", typeof(double));
				dt.Columns.Add("Closing", typeof(double));
				dt.Columns.Add("ClosingDr", typeof(double));
				dt.Columns.Add("ClosingCr", typeof(double));
				dt.Columns.Add("SubsidiaryGroupAccount", typeof(string));
				dt.Clear();
				for (int i = 0; i < dtReport.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtReport.Rows[i]["AccountId"]), Conversion.ToInt(dtReport.Rows[i]["BranchesId"]), Conversion.ToString(dtReport.Rows[i]["BranchName"]), Conversion.ToString(dtReport.Rows[i]["ParentAccount"]), Conversion.ToString(dtReport.Rows[i]["ParentAccountTitle"]), Conversion.ToString(dtReport.Rows[i]["AccountCode"]), Conversion.ToString(dtReport.Rows[i]["AccountTitle"]), Conversion.ToString(dtReport.Rows[i]["SubsidiaryAccountTitle"]), Conversion.ToDouble(dtReport.Rows[i]["OpeningDr"]), Conversion.ToDouble(dtReport.Rows[i]["OpeningCr"]), Conversion.ToDouble(dtReport.Rows[i]["Opening"]), Conversion.ToDouble(dtReport.Rows[i]["Debit"]), Conversion.ToDouble(dtReport.Rows[i]["Credit"]), Conversion.ToDouble(dtReport.Rows[i]["Closing"]), Conversion.ToDouble(dtReport.Rows[i]["ClosingDr"]), Conversion.ToDouble(dtReport.Rows[i]["ClosingCr"]), Conversion.ToString(dtReport.Rows[i]["SubsidiaryGroupAccount"]));
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSettings();
			}
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
			HistoryFill();
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

	private void AddConditionalFormatting()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		try
		{
			GridEXColumn column = new GridEXColumn();
			column = grd.RootTable.Columns["Closing"];
			GridEXFormatCondition fc = new GridEXFormatCondition(column, (ConditionOperator)3, (object)0);
			fc.FormatStyle.FontStrikeout = (TriState)2;
			fc.FormatStyle.ForeColor = SystemColors.GrayText;
			grd.RootTable.FormatConditions.Add(fc);
			column = grd.RootTable.Columns["Opening"];
			fc = new GridEXFormatCondition(column, (ConditionOperator)0, (object)0);
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Red;
			grd.RootTable.FormatConditions.Add(fc);
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
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grd.RootTable.Columns["ParentAccount"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["BranchesId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountTitle"].Visible = false;
			grd.RootTable.Columns["SubsidiaryGroupAccount"].Visible = false;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["ParentAccount"].Width = 130;
			grd.RootTable.Columns["ParentAccountTitle"].Width = 160;
			grd.RootTable.Columns["AccountCode"].Width = 90;
			grd.RootTable.Columns["AccountTitle"].Width = 400;
			grd.RootTable.Columns["SubsidiaryAccountTitle"].Width = 200;
			grd.RootTable.Columns["OpeningDr"].Width = 90;
			grd.RootTable.Columns["OpeningCr"].Width = 90;
			grd.RootTable.Columns["Opening"].Width = 90;
			grd.RootTable.Columns["Debit"].Width = 100;
			grd.RootTable.Columns["Credit"].Width = 100;
			grd.RootTable.Columns["Closing"].Width = 100;
			grd.RootTable.Columns["ClosingDr"].Width = 100;
			grd.RootTable.Columns["ClosingCr"].Width = 100;
			grd.RootTable.Columns["SubsidiaryGroupAccount"].Width = 160;
			if (rdFourColumns.Checked)
			{
				grd.RootTable.Columns["OpeningDr"].Visible = false;
				grd.RootTable.Columns["OpeningCr"].Visible = false;
				grd.RootTable.Columns["ClosingDr"].Visible = false;
				grd.RootTable.Columns["ClosingCr"].Visible = false;
				grd.RootTable.Columns["Opening"].Visible = true;
				grd.RootTable.Columns["Closing"].Visible = true;
			}
			else if (rdSixColumns.Checked)
			{
				grd.RootTable.Columns["Opening"].Visible = false;
				grd.RootTable.Columns["Closing"].Visible = false;
				grd.RootTable.Columns["OpeningDr"].Visible = true;
				grd.RootTable.Columns["OpeningCr"].Visible = true;
				grd.RootTable.Columns["ClosingDr"].Visible = true;
				grd.RootTable.Columns["ClosingCr"].Visible = true;
			}
			grd.RootTable.Groups.Add("ParentAccountTitle");
			grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grd.GroupTotals = (GroupTotals)2;
			grd.RootTable.Columns["OpeningDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OpeningDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OpeningDr"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["OpeningDr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["OpeningCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OpeningCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OpeningCr"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["OpeningCr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClosingDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ClosingDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClosingDr"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClosingDr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClosingCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ClosingCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClosingCr"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClosingCr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			if (Conversion.ToInt(CmbReportType.Value) == 2)
			{
				grd.RootTable.Columns["SubsidiaryGroupAccount"].Visible = true;
			}
			grd.RootTable.Columns.Add("Slip_116");
			grd.RootTable.Columns["Slip_116"].Caption = "Slip_116";
			grd.RootTable.Columns["Slip_116"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Slip_116"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Slip_116"].ButtonText = "Slip_116";
			grd.RootTable.Columns["Slip_116"].Width = 80;
			grd.RootTable.Columns["Slip_116"].Position = 0;
			if (SubsidiaryAllown && Conversion.ToInt(CmbReportType.Value) == 1)
			{
				grd.RootTable.Columns.Add("BreakUp");
				grd.RootTable.Columns["BreakUp"].Caption = "AcBreakUp";
				grd.RootTable.Columns["BreakUp"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["BreakUp"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["BreakUp"].ButtonText = "AcBreakUp";
				grd.RootTable.Columns["BreakUp"].Width = 80;
				grd.RootTable.Columns["BreakUp"].Position = 1;
				grd.FrozenColumns = 2;
			}
			else
			{
				grd.FrozenColumns = 1;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Alt)
			{
				if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
				{
					ShowReport();
				}
				else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
				{
					btnActivityDetail_Click(null, null);
				}
				else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
				{
					btnActivitysummaryMultisubPrint_Click(null, null);
				}
				else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
				{
					btnReport110B_Click(null, null);
				}
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
			if (e.Control && e.KeyCode == Keys.L)
			{
				lblLanguage.Visible = true;
				((Control)(object)CmbLanguage).Visible = true;
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Alt + 1", "For print of 110");
			dt.Rows.Add("Alt + 2", "For print of 116");
			dt.Rows.Add("Alt + 3", "For print of 110A");
			dt.Rows.Add("Alt + 4", "For print of 110B");
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
			wf.Close();
			MessageBox.Show(ex.ToString());
		}
	}

	private void ShowReport()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtReport == null || dtReport.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtReport, "110-AcRptActivitySummery.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		try
		{
			ShowReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnActivityDetail_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (rdDocFromToDate.Checked)
			{
				vh.FromDate = Conversion.ToDateTime((object)fromdate.Value);
				vh.ToDate = Conversion.ToDateTime((object)todate.Value);
			}
			else if (rdEntryFromToDate.Checked)
			{
				vh.EntryDateFrom = Conversion.ToDateTime((object)fromdate.Value);
				vh.EntryDateTo = Conversion.ToDateTime((object)todate.Value);
			}
			if (ChkBoxPost.Checked)
			{
				vh.ApprovedFilter = "All";
			}
			else
			{
				vh.IsApproved = false;
			}
			vh.AccountId = Conversion.ToInt(grd.CurrentRow.Cells["AccountId"].Value);
			DataTable dt = VoucherReports.ActivitySummaryDetail(vh);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "116-AcRptAccountsActivityDetail.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_FormattingRow(object sender, RowLoadEventArgs e)
	{
	}

	private void toolStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
	}

	private void btnActivitysummaryMultisubPrint_Click(object sender, EventArgs e)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtReport.Rows.Count > 0 && dtReport != null)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtReport.AsEnumerable()
					where r.Field<double>("Debit") > 0.0
					select r;
				DataTable dtDebit = (rows.Any() ? rows.CopyToDataTable() : dtReport.Clone());
				EnumerableRowCollection<DataRow> rowscr = from r in dtReport.AsEnumerable()
					where r.Field<double>("Credit") > 0.0
					select r;
				DataTable obj = (rowscr.Any() ? rowscr.CopyToDataTable() : dtReport.Clone());
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject DayBookSubReport_DebitTransactions = new SubReportObject(dtDebit, "ActivitySummeryDebitSubReport.rpt");
				SubReportObject DayBookSubReport_CreditTransactions = new SubReportObject(obj, "ActivitySummeryCreditSubReport.rpt");
				lstSubReports.Add(DayBookSubReport_DebitTransactions);
				lstSubReports.Add(DayBookSubReport_CreditTransactions);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dtReport, "110A-AcRptActivitySummery.rpt", lstSubReports);
				return;
			}
			throw new Exception("Record not found for Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReprt110B()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string DocumenTypeIds = "";
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (rdDocFromToDate.Checked)
			{
				vh.FromDate = Conversion.ToDateTime((object)fromdate.Value);
				vh.ToDate = Conversion.ToDateTime((object)todate.Value);
			}
			else if (rdEntryFromToDate.Checked)
			{
				vh.EntryDateFrom = Conversion.ToDateTime((object)fromdate.Value);
				vh.EntryDateTo = Conversion.ToDateTime((object)todate.Value);
			}
			if (SubsidiaryAllown)
			{
				vh.ReportTypeId = Conversion.ToInt(CmbReportType.Value);
			}
			if (!ChkBoxPost.Checked)
			{
				vh.IsApproved = true;
			}
			else
			{
				vh.ApprovedFilter = "All";
			}
			if (Conversion.ToString((object)((Control)(object)CmbDocumentType).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbDocumentType).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtDocType.Select("DocumentTypeDescription='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						DocumenTypeIds = DocumenTypeIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				vh.Ids = DocumenTypeIds;
			}
			DataTable dtreport = VoucherReports.ActivitySummaryDocumentTypeWise(vh);
			if (dtreport.Rows.Count > 0 && dtreport != null)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtreport.AsEnumerable()
					where r.Field<double>("Debit") > 0.0
					select r;
				DataTable dtDebit = (rows.Any() ? rows.CopyToDataTable() : dtreport.Clone());
				EnumerableRowCollection<DataRow> rowscr = from r in dtreport.AsEnumerable()
					where r.Field<double>("Credit") > 0.0
					select r;
				DataTable obj = (rowscr.Any() ? rowscr.CopyToDataTable() : dtreport.Clone());
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject DayBookSubReport_DebitTransactions = new SubReportObject(dtDebit, "ActivitySummeryDebitSubReportForB.rpt");
				SubReportObject DayBookSubReport_CreditTransactions = new SubReportObject(obj, "ActivitySummeryCreditSubReportForB.rpt");
				lstSubReports.Add(DayBookSubReport_DebitTransactions);
				lstSubReports.Add(DayBookSubReport_CreditTransactions);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dtreport, "110B-AcRptActivitySummery.rpt", lstSubReports);
				return;
			}
			throw new Exception("Record not found for Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReport110B_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReprt110B();
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
			int branchId = Conversion.ToInt(grd.CurrentRow.Cells["BranchesId"].Value);
			CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(grd.CurrentRow.Cells["AccountId"].Value), fromdate.Value, todate.Value, 0, branchId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ActicitySummery_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			DocumenttypeFill();
			MultiLanguagesGetAll();
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

	private void reset()
	{
		try
		{
			((Control)(object)CmbDocumentType).Text = string.Empty;
			cmbDateType.Focus();
			Datetypefill();
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grd.CurrentColumn != null)
			{
				Conversion.ToInt((object)item.Cells["AccountId"].Value.ToString());
				if (grd.CurrentColumn.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(grd.CurrentRow.Cells["AccountId"].Value), fromdate.Value, todate.Value);
				}
			}
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
			if (base.ActiveControl != null && base.ActiveControl.Name == "cmbDateType")
			{
				cmbDateType.Focus();
			}
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
				fromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				fromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				fromdate.Value = first;
				todate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				fromdate.Value = first2;
				todate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdFourColumns_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtReport.Rows.Count > 0)
			{
				if (rdFourColumns.Checked)
				{
					grd.RootTable.Columns["OpeningDr"].Visible = false;
					grd.RootTable.Columns["OpeningCr"].Visible = false;
					grd.RootTable.Columns["ClosingDr"].Visible = false;
					grd.RootTable.Columns["ClosingCr"].Visible = false;
					grd.RootTable.Columns["Opening"].Visible = true;
					grd.RootTable.Columns["Closing"].Visible = true;
				}
				else if (rdSixColumns.Checked)
				{
					grd.RootTable.Columns["Opening"].Visible = false;
					grd.RootTable.Columns["Closing"].Visible = false;
					grd.RootTable.Columns["OpeningDr"].Visible = true;
					grd.RootTable.Columns["OpeningCr"].Visible = true;
					grd.RootTable.Columns["ClosingDr"].Visible = true;
					grd.RootTable.Columns["ClosingCr"].Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (e.Column.Key == "AcBreakUp")
			{
				SubsidiaryActivitySummaryDetail obj = new SubsidiaryActivitySummaryDetail(UserAccount);
				obj.FromDate = Conversion.ToDateTime((object)fromdate.Value);
				obj.ToDate = Conversion.ToDateTime((object)todate.Value);
				obj.ParentTitle = Conversion.ToString(r.Cells["AccountTitle"].Value);
				obj.GetData(obj.FromDate, obj.ToDate, Conversion.ToInt(r.Cells["AccountId"].Value), DocumenTypeIds);
				obj.Show();
			}
			else if (e.Column.Key == "Slip_116")
			{
				btnActivityDetail_Click(null, null);
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
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_0c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3e: Expected O, but got Unknown
		//IL_0c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Expected O, but got Unknown
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6c: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
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
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbLanguage = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdEntryFromToDate = new System.Windows.Forms.RadioButton();
		this.rdDocFromToDate = new System.Windows.Forms.RadioButton();
		this.rdSixColumns = new System.Windows.Forms.RadioButton();
		this.rdFourColumns = new System.Windows.Forms.RadioButton();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.ChkBoxPost = new System.Windows.Forms.CheckBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.labelReportype = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbReportType = new UltraCombo();
		this.CmbDocumentType = new UltraCombo();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnActivityDetail = new System.Windows.Forms.ToolStripButton();
		this.btnActivitysummaryMultisubPrint = new System.Windows.Forms.ToolStripButton();
		this.btnReport110B = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkey = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentType).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel3.Controls.Add(this.Branches);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(895, 31);
		this.panel3.TabIndex = 4;
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
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
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
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(233, 3);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 118;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.White;
		this.Branches.Location = new System.Drawing.Point(168, 8);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(63, 17);
		this.Branches.TabIndex = 119;
		this.Branches.Text = "Branches";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(152, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Activity Summary";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 58);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(895, 585);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 110);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(895, 475);
		this.panel6.TabIndex = 108;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(895, 449);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.FormattingRow += new RowLoadEventHandler(grd_FormattingRow);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(895, 26);
		this.panel7.TabIndex = 5;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(854, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(2, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(142, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "Filtered Records";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(895, 110);
		this.panel5.TabIndex = 107;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.groupBox1.Controls.Add(this.lblLanguage);
		this.groupBox1.Controls.Add(this.groupBox2);
		this.groupBox1.Controls.Add(this.rdSixColumns);
		this.groupBox1.Controls.Add(this.rdFourColumns);
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.ChkBoxPost);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.todate);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.labelReportype);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReportType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentType);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(802, 105);
		this.groupBox1.TabIndex = 125;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbLanguage.AllowNull = (DefaultableBoolean)1;
		this.CmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(302, 71);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 125;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Visible = false;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblLanguage.Location = new System.Drawing.Point(229, 77);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(59, 15);
		this.lblLanguage.TabIndex = 126;
		this.lblLanguage.Text = "Language";
		this.lblLanguage.Visible = false;
		this.groupBox2.Controls.Add(this.rdEntryFromToDate);
		this.groupBox2.Controls.Add(this.rdDocFromToDate);
		this.groupBox2.Location = new System.Drawing.Point(689, 9);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(107, 65);
		this.groupBox2.TabIndex = 9;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Type";
		this.rdEntryFromToDate.AutoSize = true;
		this.rdEntryFromToDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdEntryFromToDate.Location = new System.Drawing.Point(6, 44);
		this.rdEntryFromToDate.Name = "rdEntryFromToDate";
		this.rdEntryFromToDate.Size = new System.Drawing.Size(80, 19);
		this.rdEntryFromToDate.TabIndex = 1;
		this.rdEntryFromToDate.Text = "Entry Date";
		this.rdEntryFromToDate.UseVisualStyleBackColor = true;
		this.rdDocFromToDate.AutoSize = true;
		this.rdDocFromToDate.Checked = true;
		this.rdDocFromToDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdDocFromToDate.Location = new System.Drawing.Point(6, 21);
		this.rdDocFromToDate.Name = "rdDocFromToDate";
		this.rdDocFromToDate.Size = new System.Drawing.Size(75, 19);
		this.rdDocFromToDate.TabIndex = 0;
		this.rdDocFromToDate.TabStop = true;
		this.rdDocFromToDate.Text = "Doc Date";
		this.rdDocFromToDate.UseVisualStyleBackColor = true;
		this.rdSixColumns.AutoSize = true;
		this.rdSixColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSixColumns.Location = new System.Drawing.Point(579, 48);
		this.rdSixColumns.Name = "rdSixColumns";
		this.rdSixColumns.Size = new System.Drawing.Size(91, 19);
		this.rdSixColumns.TabIndex = 7;
		this.rdSixColumns.TabStop = true;
		this.rdSixColumns.Text = "Six Columns";
		this.rdSixColumns.UseVisualStyleBackColor = true;
		this.rdFourColumns.AutoSize = true;
		this.rdFourColumns.Checked = true;
		this.rdFourColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdFourColumns.Location = new System.Drawing.Point(579, 21);
		this.rdFourColumns.Name = "rdFourColumns";
		this.rdFourColumns.Size = new System.Drawing.Size(99, 19);
		this.rdFourColumns.TabIndex = 6;
		this.rdFourColumns.TabStop = true;
		this.rdFourColumns.Text = "Four Columns";
		this.rdFourColumns.UseVisualStyleBackColor = true;
		this.rdFourColumns.CheckedChanged += new System.EventHandler(rdFourColumns_CheckedChanged);
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(70, 46);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(152, 23);
		this.fromdate.TabIndex = 1;
		this.ChkBoxPost.AutoSize = true;
		this.ChkBoxPost.Checked = true;
		this.ChkBoxPost.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBoxPost.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPost.Location = new System.Drawing.Point(579, 76);
		this.ChkBoxPost.Name = "ChkBoxPost";
		this.ChkBoxPost.Size = new System.Drawing.Size(172, 19);
		this.ChkBoxPost.TabIndex = 8;
		this.ChkBoxPost.Text = "Include UnPosted Vouchers";
		this.ChkBoxPost.UseVisualStyleBackColor = true;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 23);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(60, 15);
		this.label9.TabIndex = 124;
		this.label9.Text = "Date Type";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 50);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(70, 17);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 0;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(5, 77);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(70, 73);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(152, 23);
		this.todate.TabIndex = 2;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(515, 71);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(60, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.labelReportype.AutoSize = true;
		this.labelReportype.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelReportype.Location = new System.Drawing.Point(229, 50);
		this.labelReportype.Name = "labelReportype";
		this.labelReportype.Size = new System.Drawing.Size(70, 15);
		this.labelReportype.TabIndex = 110;
		this.labelReportype.Text = "Report Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(229, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 15);
		this.label1.TabIndex = 110;
		this.label1.Text = "Doc_Types";
		this.CmbReportType.AllowNull = (DefaultableBoolean)1;
		this.CmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbReportType).Location = new System.Drawing.Point(302, 44);
		((System.Windows.Forms.Control)(object)this.CmbReportType).Name = "CmbReportType";
		((System.Windows.Forms.Control)(object)this.CmbReportType).Size = new System.Drawing.Size(273, 26);
		((System.Windows.Forms.Control)(object)this.CmbReportType).TabIndex = 4;
		this.CmbDocumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Location = new System.Drawing.Point(302, 17);
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Name = "CmbDocumentType";
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Size = new System.Drawing.Size(273, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).TabIndex = 3;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(895, 643);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(895, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.btnRefresh, this.print, this.btnActivityDetail, this.btnActivitysummaryMultisubPrint, this.btnReport110B, this.btnshortcutkey });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(895, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStrip3.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(toolStrip3_ItemClicked);
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
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "110-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btnActivityDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnActivityDetail.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnActivityDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnActivityDetail.Name = "btnActivityDetail";
		this.btnActivityDetail.Size = new System.Drawing.Size(150, 24);
		this.btnActivityDetail.Text = "116-ActivityDetail";
		this.btnActivityDetail.Visible = false;
		this.btnActivityDetail.Click += new System.EventHandler(btnActivityDetail_Click);
		this.btnActivitysummaryMultisubPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnActivitysummaryMultisubPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnActivitysummaryMultisubPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnActivitysummaryMultisubPrint.Name = "btnActivitysummaryMultisubPrint";
		this.btnActivitysummaryMultisubPrint.Size = new System.Drawing.Size(94, 24);
		this.btnActivitysummaryMultisubPrint.Text = "110A&Print";
		this.btnActivitysummaryMultisubPrint.Click += new System.EventHandler(btnActivitysummaryMultisubPrint_Click);
		this.btnReport110B.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReport110B.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnReport110B.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReport110B.Name = "btnReport110B";
		this.btnReport110B.Size = new System.Drawing.Size(93, 24);
		this.btnReport110B.Text = "110B&Print";
		this.btnReport110B.Click += new System.EventHandler(btnReport110B_Click);
		this.btnshortcutkey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkey.Name = "btnshortcutkey";
		this.btnshortcutkey.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkey.Text = "ShortCut Keys";
		this.btnshortcutkey.Click += new System.EventHandler(btnshortcutkey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(895, 643);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ActicitySummery";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ActicitySummery_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentType).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
