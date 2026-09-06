// // using Architecture.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using HierarchicalMode = Janus.Windows.GridEX.HierarchicalMode;
using SelfReferencingAggregationMode = Janus.Windows.GridEX.SelfReferencingAggregationMode;
using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using ConditionOperator = Janus.Windows.GridEX.ConditionOperator;
using GroupInterval = Janus.Windows.GridEX.GroupInterval;
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

public class frmTrialBalancesAllLevel : Form
{
	private WaitForm wf = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private DataTable data = new DataTable();

	private DataTable dtFinal = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private DataSet dataSet;

	private int lastId;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Button btnshow;

	private DateTimePicker txtToDate;

	private DateTimePicker txtFromDate;

	private Label label33;

	private Label label27;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbViewOption;

	private Label label29;

	private RadioButton rdGroupeExpand;

	private RadioButton rdGroupCollapsed;

	private CheckBox ChkSixColumns;

	private CheckBox ChkFourColumns;

	private ToolStripDropDownButton tsDropDown;

	private CheckBox SkipZero;

	private Label toolStripLabel1;

	private Label label1;

	public frmTrialBalancesAllLevel(UserAccount userAccount)
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
			wf.Show();
			ChkFourColumns.Checked = true;
			ViewOptionsFill();
			txtFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			GridBind();
			rdGroupeExpand.Checked = true;
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

	private void ReportsLoad()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("HararicalTrialBalance");
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

	private void ViewOptionsFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Upto 1st Level");
			dt.Rows.Add(2, "Upto 2nd Level");
			dt.Rows.Add(3, "Upto 3rd Level");
			dt.Rows.Add(4, "Upto 4th Level");
			dt.Rows.Add(5, "Upto All(5) Levels");
			DDL.BindDDL(dt, cmbViewOption, "Id", "Name", "View Option", ZeroIndex: false);
			((UltraGridBase)cmbViewOption).Rows[4].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		try
		{
			DataTable dtMain = VoucherReports.HararicalTrialBalance(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtFromDate.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				SkipZero = Conversion.ToInt((object)SkipZero.Checked)
			});
			if (dtMain.Rows.Count > 0)
			{
				dtFinal = new DataTable();
				dtFinal.Columns.Add("RowId", typeof(int));
				dtFinal.Columns.Add("ParentId", typeof(int));
				dtFinal.Columns.Add("AccountId", typeof(int));
				dtFinal.Columns.Add("AccountTitle", typeof(string));
				dtFinal.Columns.Add("AccountCode", typeof(string));
				dtFinal.Columns.Add("AcLevel", typeof(int));
				dtFinal.Columns.Add("AccountClass", typeof(int));
				dtFinal.Columns.Add("IsGroupDetail", typeof(string));
				dtFinal.Columns.Add("Opening", typeof(double));
				dtFinal.Columns.Add("OpeningDr", typeof(double));
				dtFinal.Columns.Add("OpeningCr", typeof(double));
				dtFinal.Columns.Add("Debit", typeof(double));
				dtFinal.Columns.Add("Credit", typeof(double));
				dtFinal.Columns.Add("Closing", typeof(double));
				dtFinal.Columns.Add("ClosingDr", typeof(double));
				dtFinal.Columns.Add("ClosingCr", typeof(double));
				dtFinal.Columns.Add("CurDbGt", typeof(double));
				dtFinal.Columns.Add("CurCrGt", typeof(double));
				dtFinal.Columns.Add("ObGt", typeof(double));
				dtFinal.Columns.Add("ClGt", typeof(double));
				dtFinal.Columns.Add("ObDrGt", typeof(double));
				dtFinal.Columns.Add("ObCrGt", typeof(double));
				dtFinal.Columns.Add("ClDrGt", typeof(double));
				dtFinal.Columns.Add("ClCrGt", typeof(double));
				int RowId = 0;
				object ParentId2ndLevel = 0;
				object ParentId3rdLevel = 0;
				object ParentId4thLevel = 0;
				object ParentId5thLevel = 0;
				for (int i = 0; i < dtMain.Rows.Count; i++)
				{
					RowId++;
					if (Conversion.ToInt(cmbViewOption.Value) == 1)
					{
						if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 1)
						{
							ParentId2ndLevel = RowId;
							dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
					}
					else if (Conversion.ToInt(cmbViewOption.Value) == 2)
					{
						if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 1)
						{
							ParentId2ndLevel = RowId;
							dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
						else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 2)
						{
							ParentId3rdLevel = RowId;
							dtFinal.Rows.Add(RowId, ParentId2ndLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
					}
					else if (Conversion.ToInt(cmbViewOption.Value) == 3)
					{
						if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 2)
						{
							ParentId3rdLevel = RowId;
							dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
						else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 3)
						{
							ParentId4thLevel = RowId;
							dtFinal.Rows.Add(RowId, ParentId3rdLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
					}
					else if (Conversion.ToInt(cmbViewOption.Value) == 4)
					{
						if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 3)
						{
							ParentId4thLevel = RowId;
							dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
						else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 4)
						{
							ParentId5thLevel = RowId;
							dtFinal.Rows.Add(RowId, ParentId4thLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
						else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 5)
						{
							dtFinal.Rows.Add(RowId, ParentId5thLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
					}
					else if (Conversion.ToInt(cmbViewOption.Value) == 5)
					{
						if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 1)
						{
							ParentId2ndLevel = RowId;
							dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
						else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 2)
						{
							ParentId3rdLevel = RowId;
							dtFinal.Rows.Add(RowId, ParentId2ndLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
						else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 3)
						{
							ParentId4thLevel = RowId;
							dtFinal.Rows.Add(RowId, ParentId3rdLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
						else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 4)
						{
							ParentId5thLevel = RowId;
							dtFinal.Rows.Add(RowId, ParentId4thLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
						else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 5)
						{
							dtFinal.Rows.Add(RowId, ParentId5thLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
						}
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

	private DataTable CalcuateTotals(DataTable dt, List<string> Columns, bool CalculateAgainstSpecificProperty, string PropertyName = null, string PropertyValue = null)
	{
		List<ListTypeForCalculatingSums> Sums = new List<ListTypeForCalculatingSums>();
		DataTable SumsToReturn = new DataTable();
		try
		{
			if (Columns.Count <= 0)
			{
				throw new Exception("You must Provide list of Column Names to Calculate Totals From!");
			}
			for (int k = 0; k < Columns.Count; k++)
			{
				ListTypeForCalculatingSums obj = new ListTypeForCalculatingSums();
				obj.PropertyName = Columns[k];
				obj.PropertyTotal = 0.0;
				Sums.Add(obj);
				SumsToReturn.Columns.Add(Columns[k]);
			}
			for (int j = 0; j < dt.Rows.Count; j++)
			{
				if (!CalculateAgainstSpecificProperty)
				{
					for (int l = 0; l < Columns.Count; l++)
					{
						if (Sums[0].PropertyName == Columns[l])
						{
							Sums[0].PropertyTotal += Conversion.ToDouble(dt.Rows[j][Columns[l]]);
						}
					}
				}
				else
				{
					if (!(Conversion.ToString(dt.Rows[j][PropertyName]) == Conversion.ToString((object)PropertyValue)))
					{
						continue;
					}
					for (int m = 0; m < Columns.Count; m++)
					{
						for (int f = 0; f < Sums.Count; f++)
						{
							if (Sums[f].PropertyName == Columns[m])
							{
								Sums[f].PropertyTotal = Sums[f].PropertyTotal + Conversion.ToDouble(dt.Rows[j][Columns[m]]);
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		DataRow dr1 = SumsToReturn.NewRow();
		for (int i = 0; i < Sums.Count; i++)
		{
			dr1[Sums[i].PropertyName] = Sums[i].PropertyTotal;
		}
		SumsToReturn.Rows.Add(dr1.ItemArray);
		return SumsToReturn;
	}

	private bool CheckIfRecordAlreadyExist(DataTable dt, string PropertyName, string PropertyValue)
	{
		if (dt.Rows.Count <= 0)
		{
			return false;
		}
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i][PropertyName]) == Conversion.ToString((object)PropertyValue))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void grdHistorySetting()
	{
		try
		{
			grdHistory.RootTable.Columns["ParentId"].Visible = false;
			grdHistory.RootTable.Columns["RowId"].Visible = false;
			grdHistory.RootTable.Columns["AccountId"].Visible = false;
			grdHistory.RootTable.Columns["CurDbGt"].Visible = false;
			grdHistory.RootTable.Columns["CurCrGt"].Visible = false;
			grdHistory.RootTable.Columns["ObGt"].Visible = false;
			grdHistory.RootTable.Columns["ClGt"].Visible = false;
			grdHistory.RootTable.Columns["ObDrGt"].Visible = false;
			grdHistory.RootTable.Columns["ObCrGt"].Visible = false;
			grdHistory.RootTable.Columns["ClDrGt"].Visible = false;
			grdHistory.RootTable.Columns["ClCrGt"].Visible = false;
			grdHistory.RootTable.Columns["AccountClass"].Visible = false;
			if (ChkFourColumns.Checked)
			{
				grdHistory.RootTable.Columns["OpeningDr"].Visible = false;
				grdHistory.RootTable.Columns["OpeningCr"].Visible = false;
				grdHistory.RootTable.Columns["ClosingDr"].Visible = false;
				grdHistory.RootTable.Columns["ClosingCr"].Visible = false;
			}
			else
			{
				grdHistory.RootTable.Columns["Opening"].Visible = false;
				grdHistory.RootTable.Columns["Closing"].Visible = false;
			}
			grdHistory.RootTable.Columns["IsGroupDetail"].Caption = "AccountType";
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
			grdHistory.RootTable.Columns["AccountTitle"].Width = 350;
			grdHistory.ColumnAutoResize = true;
			grdHistory.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["OpeningDr"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["OpeningDr"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["OpeningDr"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["OpeningDr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["OpeningCr"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["OpeningCr"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["OpeningCr"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["OpeningCr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["ClosingDr"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["ClosingDr"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["ClosingDr"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["ClosingDr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["ClosingCr"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["ClosingCr"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["ClosingCr"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["ClosingCr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
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
			MessageBox.Show(ex.ToString());
		}
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			((UltraGridBase)cmbViewOption).Rows[0].Activate();
			txtFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtToDate.Value = DateTime.Now;
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

	private void CreateDummyData()
	{
		dataSet = new DataSet();
		DataTable contacts = new DataTable("Contacts");
		contacts.Columns.Add("ContactId", typeof(int));
		contacts.Columns.Add("Name", typeof(string));
		contacts.Columns.Add("Phone", typeof(string));
		contacts.PrimaryKey = new DataColumn[1] { contacts.Columns["ContactId"] };
		DataTable categories = new DataTable("Categories");
		categories.Columns.Add("CategoryId", typeof(int));
		categories.Columns.Add("CategoryName", typeof(string));
		categories.PrimaryKey = new DataColumn[1] { categories.Columns["CategoryId"] };
		DataTable contactCategories = new DataTable("ContactCategories");
		contactCategories.Columns.Add("ContactId", typeof(int));
		contactCategories.Columns.Add("CategoryId", typeof(int));
		contactCategories.PrimaryKey = new DataColumn[2]
		{
			contactCategories.Columns["ContactId"],
			contactCategories.Columns["CategoryId"]
		};
		dataSet.Tables.Add(contacts);
		dataSet.Tables.Add(categories);
		dataSet.Tables.Add(contactCategories);
		dataSet.Relations.Add("Contacts_ContactCategories", contacts.Columns["ContactId"], contactCategories.Columns["ContactId"]);
		dataSet.Relations.Add("Categories_ContactCategories", categories.Columns["CategoryId"], contactCategories.Columns["CategoryId"]);
		contacts.Rows.Add(1, "Paul Wagner", "(466) 123-5532");
		contacts.Rows.Add(2, "Stephanie Welles", "(423) 324-9836");
		contacts.Rows.Add(3, "James Jones", "(432) 566-8814");
		categories.Rows.Add(1, "Family");
		categories.Rows.Add(2, "Business");
		categories.Rows.Add(3, "Personal");
		categories.Rows.Add(4, "VIP");
		categories.Rows.Add(5, "Customers");
		categories.Rows.Add(6, "Suppliers");
		contactCategories.Rows.Add(1, 2);
		contactCategories.Rows.Add(1, 4);
		contactCategories.Rows.Add(1, 5);
		contactCategories.Rows.Add(2, 1);
		contactCategories.Rows.Add(3, 2);
		contactCategories.Rows.Add(3, 6);
	}

	private void GridSettingsForDataSet()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		grdHistory.SetDataBinding((object)dataSet, "Contacts");
		grdHistory.RetrieveStructure();
		((ScrollableBase)grdHistory).VisualStyle = (VisualStyle)2;
		GridEXColumn col = new GridEXColumn();
		col.Key = "Categories";
		col.MultipleValues = true;
		col.DataMember = "Contacts_ContactCategories";
		col.EditType = (EditType)4;
		col.Caption = "Categories";
		col.Width = 200;
		col.MultipleValueDataMember = "CategoryId";
		col.HasValueList = true;
		col.ValueList.PopulateValueList((IEnumerable)dataSet.Tables["Categories"].DefaultView, "CategoryId", "CategoryName");
		col.DefaultGroupInterval = (GroupInterval)2;
		col.CompareTarget = (ColumnCompareTarget)1;
		grdHistory.RootTable.Columns.Add(col);
	}

	private void CreateDummySource()
	{
		data = new DataTable("DummyData");
		data.Columns.Add("MessageID", typeof(int));
		data.Columns.Add("Subject", typeof(string));
		data.Columns.Add("From", typeof(string));
		data.Columns.Add("Date", typeof(DateTime));
		data.Columns.Add("ParentMessageID", typeof(int));
		AddMessage("FirstLevel-2", "Paul", new DateTime(2005, 12, 9), DBNull.Value);
		AddMessage("FirstLevelChild-2-1 , 3nd Parent", "John", new DateTime(2005, 12, 9), 1);
		AddMessage("SecondLevel-1", "John", new DateTime(2005, 12, 10), DBNull.Value);
		AddMessage("SecondLevelChild-1", "John", new DateTime(2005, 12, 10), 3);
	}

	private int AddMessage(string subject, string from, DateTime date, object parentId)
	{
		lastId++;
		data.Rows.Add(lastId, subject, from, date, parentId);
		return lastId;
	}

	private void SetGridSettingsChild()
	{
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		grdHistory.DataSource = data;
		grdHistory.RetrieveStructure();
		((ScrollableBase)grdHistory).VisualStyle = (VisualStyle)2;
		grdHistory.GridLines = (GridLines)1;
		grdHistory.HideSelection = (HideSelection)2;
		grdHistory.RootTable.HierarchicalMode = (HierarchicalMode)1;
		grdHistory.RootTable.SelfReferencingSettings.ParentDataMember = "MessageID";
		grdHistory.RootTable.SelfReferencingSettings.ChildDataMember = "ParentMessageID";
		grdHistory.RootTable.SelfReferencingSettings.ExpandColumn = grdHistory.RootTable.Columns["Subject"];
		grdHistory.RootTable.Columns["Subject"].ColumnType = (ColumnType)2;
		grdHistory.RootTable.Columns["Subject"].ImageIndex = 0;
		grdHistory.RootTable.Columns["Subject"].Width = 300;
		grdHistory.RootTable.Columns["From"].Width = 100;
		grdHistory.RootTable.Columns["Date"].Width = 100;
		foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
		{
			item.Selectable = false;
		}
		grdHistory.ColumnAutoResize = true;
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

	private void ChkFourColumns_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (ChkFourColumns.Checked)
			{
				ChkSixColumns.Checked = false;
			}
			else
			{
				ChkFourColumns.Checked = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkSixColumns_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (ChkSixColumns.Checked)
			{
				ChkFourColumns.Checked = false;
			}
			else
			{
				ChkSixColumns.Checked = false;
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

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdHistory.CurrentRow;
			int Id = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			if (Conversion.ToString(currentRow.Cells["IsGroupDetail"].Value) == "Group")
			{
				SelectedTrialBalance selectedTrialBalance = new SelectedTrialBalance(UserAccount);
				SelectedTrialBalance.FromDate = txtFromDate.Value;
				SelectedTrialBalance.ToDate = txtToDate.Value;
				SelectedTrialBalance.GroupId = Id;
				selectedTrialBalance.ShowDialog();
				selectedTrialBalance.BringToFront();
			}
			else
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(Id, txtFromDate.Value, txtToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmTrialBalancesAllLevel_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Expected O, but got Unknown
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.SkipZero = new System.Windows.Forms.CheckBox();
		this.ChkSixColumns = new System.Windows.Forms.CheckBox();
		this.ChkFourColumns = new System.Windows.Forms.CheckBox();
		this.rdGroupeExpand = new System.Windows.Forms.RadioButton();
		this.rdGroupCollapsed = new System.Windows.Forms.RadioButton();
		this.cmbViewOption = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.tsDropDown = new System.Windows.Forms.ToolStripDropDownButton();
		this.toolStripLabel1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbViewOption).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(884, 461);
		this.panel1.TabIndex = 0;
		this.grdHistory.AlternatingColors = true;
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 141);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(884, 320);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 118);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(884, 23);
		this.panel4.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(834, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(132, 18);
		this.label1.TabIndex = 115;
		this.label1.Text = "Filtered Records";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.SkipZero);
		this.panel3.Controls.Add(this.ChkSixColumns);
		this.panel3.Controls.Add(this.ChkFourColumns);
		this.panel3.Controls.Add(this.rdGroupeExpand);
		this.panel3.Controls.Add(this.rdGroupCollapsed);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbViewOption);
		this.panel3.Controls.Add(this.label29);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add(this.txtToDate);
		this.panel3.Controls.Add(this.txtFromDate);
		this.panel3.Controls.Add(this.label33);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 51);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(884, 67);
		this.panel3.TabIndex = 0;
		this.SkipZero.AutoSize = true;
		this.SkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SkipZero.Location = new System.Drawing.Point(508, 43);
		this.SkipZero.Margin = new System.Windows.Forms.Padding(2);
		this.SkipZero.Name = "SkipZero";
		this.SkipZero.Size = new System.Drawing.Size(84, 21);
		this.SkipZero.TabIndex = 6;
		this.SkipZero.Text = "Skip Zero";
		this.SkipZero.UseVisualStyleBackColor = true;
		this.ChkSixColumns.AutoSize = true;
		this.ChkSixColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkSixColumns.Location = new System.Drawing.Point(508, 24);
		this.ChkSixColumns.Name = "ChkSixColumns";
		this.ChkSixColumns.Size = new System.Drawing.Size(101, 21);
		this.ChkSixColumns.TabIndex = 5;
		this.ChkSixColumns.Text = "Six Columns";
		this.ChkSixColumns.UseVisualStyleBackColor = true;
		this.ChkSixColumns.CheckedChanged += new System.EventHandler(ChkSixColumns_CheckedChanged);
		this.ChkFourColumns.AutoSize = true;
		this.ChkFourColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkFourColumns.Location = new System.Drawing.Point(508, 5);
		this.ChkFourColumns.Name = "ChkFourColumns";
		this.ChkFourColumns.Size = new System.Drawing.Size(112, 21);
		this.ChkFourColumns.TabIndex = 4;
		this.ChkFourColumns.Text = "Four Columns";
		this.ChkFourColumns.UseVisualStyleBackColor = true;
		this.ChkFourColumns.CheckedChanged += new System.EventHandler(ChkFourColumns_CheckedChanged);
		this.rdGroupeExpand.AutoSize = true;
		this.rdGroupeExpand.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupeExpand.Location = new System.Drawing.Point(635, 24);
		this.rdGroupeExpand.Name = "rdGroupeExpand";
		this.rdGroupeExpand.Size = new System.Drawing.Size(71, 21);
		this.rdGroupeExpand.TabIndex = 8;
		this.rdGroupeExpand.TabStop = true;
		this.rdGroupeExpand.Text = "Expand";
		this.rdGroupeExpand.UseVisualStyleBackColor = true;
		this.rdGroupeExpand.CheckedChanged += new System.EventHandler(rdGroupeExpand_CheckedChanged);
		this.rdGroupCollapsed.AutoSize = true;
		this.rdGroupCollapsed.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupCollapsed.Location = new System.Drawing.Point(635, 5);
		this.rdGroupCollapsed.Name = "rdGroupCollapsed";
		this.rdGroupCollapsed.Size = new System.Drawing.Size(76, 21);
		this.rdGroupCollapsed.TabIndex = 7;
		this.rdGroupCollapsed.TabStop = true;
		this.rdGroupCollapsed.Text = "Collapse";
		this.rdGroupCollapsed.UseVisualStyleBackColor = true;
		this.rdGroupCollapsed.CheckedChanged += new System.EventHandler(rdGroupCollapsed_CheckedChanged);
		this.cmbViewOption.AllowNull = (DefaultableBoolean)1;
		this.cmbViewOption.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbViewOption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbViewOption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbViewOption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbViewOption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbViewOption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbViewOption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbViewOption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbViewOption).Location = new System.Drawing.Point(301, 6);
		((System.Windows.Forms.Control)(object)this.cmbViewOption).Name = "cmbViewOption";
		((System.Windows.Forms.Control)(object)this.cmbViewOption).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbViewOption).TabIndex = 2;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(222, 12);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(78, 15);
		this.label29.TabIndex = 118;
		this.label29.Text = "View Options";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(444, 34);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(74, 36);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(144, 23);
		this.txtToDate.TabIndex = 1;
		this.txtFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.CustomFormat = "dd-MMM-yy";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(74, 8);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(145, 23);
		this.txtFromDate.TabIndex = 0;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(7, 40);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 115;
		this.label33.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(7, 12);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 116;
		this.label27.Text = "From Date";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Controls.Add(this.toolStripLabel1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(884, 51);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.tsDropDown });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(884, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.tsDropDown.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDown.Name = "tsDropDown";
		this.tsDropDown.Size = new System.Drawing.Size(101, 24);
		this.tsDropDown.Text = "135-Print";
		this.tsDropDown.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDown_DropDownItemClicked);
		this.toolStripLabel1.AutoSize = true;
		this.toolStripLabel1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.ForeColor = System.Drawing.Color.White;
		this.toolStripLabel1.Location = new System.Drawing.Point(2, 30);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(199, 19);
		this.toolStripLabel1.TabIndex = 115;
		this.toolStripLabel1.Text = "Trial Balances All Level";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(884, 461);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmTrialBalancesAllLevel";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmTrialBalancesAllLevel_FormClosing);
		base.Load += new System.EventHandler(frmTrialBalancesAllLevel_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbViewOption).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
