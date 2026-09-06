using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production.Regular;

public class frmProductionFohReportCreate : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtFohAllocationFromDb = new DataTable();

	private DataTable dtDetailFohExpense = new DataTable();

	private DataTable dtPlantSummaryFohExpense = new DataTable();

	private DataTable dtHistoryFohExpense = new DataTable();

	private DataTable dtAllocationType = new DataTable();

	private bool FormInitialized;

	private string ScreenName = "frmProductionFohReportCreate";

	private int DocumentTypeId = 922;

	private int RecIdFohExpense;

	private int updateDetailIndex = -1;

	private string CustomAttachmentsValues = "";

	private string AttachmentsValues = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<ProdcutionFohAllocateToJobOrderDetail> lstRemoveRecordFohExpense = new List<ProdcutionFohAllocateToJobOrderDetail>();

	private IContainer components;

	private TabControl tabMain;

	private TabPage tabFohAllocation;

	private TabPage tabFohExpense;

	private Panel panel3;

	private Panel panel2;

	private Panel panel1;

	internal GridEX grdJobPlantFohAllocation;

	private Panel panel4;

	internal GridEX grdPlantFohAllocation;

	private Panel panel5;

	private DateTimePicker txtMonthDateFohAllocation;

	private Label label1;

	private Button btnShowFohAllocation;

	private Panel panel6;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnNewFohAloocation;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton BtnRefresh;

	private ToolStripButton btnShortCutKeyFohAllocation;

	private Label label3;

	private Label label2;

	private ToolStripButton btnPrintFohallocation;

	private Panel panel7;

	private Button BtnShowFohExpense;

	private Label label4;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNewFohExpense;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnRefreshFohExpense;

	private ToolStripButton BtnPrintFohExpense;

	private ToolStripButton BtnShortCutKeyFohExpense;

	internal UltraCombo CmbAccountFohExpense;

	private Label label9;

	private TextBox txtAccountNetAmountFohExpense;

	private Label label10;

	private Label label11;

	internal UltraCombo CmbMethodTypeFohExpense;

	internal UltraCombo CmbMonthDateFohExpense;

	private TabControl tabControl1;

	private TabPage tabFormFohExpense;

	private TabPage tabHistoryFohExpense;

	internal GridEX grdFohExpense;

	private Panel panel16;

	private Label label7;

	internal GridEX grdPlantWiseSummaryFohExpense;

	private Panel panel14;

	private Label label6;

	private ToolStripButton BtnSaveFohExpense;

	private ToolStripButton btnUpdateFohExpense;

	private ToolStripButton BtnDeleteFohExpense;

	private ToolStripButton btnAttachmentFohExpense;

	internal GridEX grdHistoryFohExpense;

	private Panel panel13;

	private Label label59;

	internal GridEX grdDetailHistoryFohExpense;

	private Panel panel29;

	private Label label65;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel18;

	private GroupBox groupBox5;

	private RadioButton rdapproveddate;

	private Button btnShowHistoryFohExpense;

	private RadioButton rdmodifydate;

	private Label label67;

	private RadioButton rdentrydate;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbMonthHistoryFohExpense;

	private DateTimePicker ToDateHistoryFohExpense;

	private DateTimePicker FromDateHistoryFohExpense;

	private Panel panel17;

	private Label label52;

	private ToolStrip toolStrip2;

	private ToolStripButton btnResetHistoryFohExpense;

	private ToolStripButton btnRefreshHistoryFohExpense;

	public frmProductionFohReportCreate(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(980, 650);
			if (dtAllocationType.Columns.Count == 0)
			{
				dtAllocationType.Columns.Add("Id", typeof(int));
				dtAllocationType.Columns.Add("Name", typeof(string));
				dtAllocationType.Rows.Add(1, "MachineHours");
				dtAllocationType.Rows.Add(2, "UsageBased");
			}
			dtDetailFohExpense.Columns.Add("Id", typeof(int));
			dtDetailFohExpense.Columns.Add("Month", typeof(string));
			dtDetailFohExpense.Columns.Add("AccountId", typeof(int));
			dtDetailFohExpense.Columns.Add("JobOrderId", typeof(int));
			dtDetailFohExpense.Columns.Add("JobOrderNo", typeof(string));
			dtDetailFohExpense.Columns.Add("PlantId", typeof(int));
			dtDetailFohExpense.Columns.Add("PlantName", typeof(string));
			dtDetailFohExpense.Columns.Add("ConsumedHours", typeof(double));
			dtDetailFohExpense.Columns.Add("%OfTotalHours", typeof(double));
			dtDetailFohExpense.Columns.Add("ConsumedFohAmount", typeof(double));
			dtDetailFohExpense.Columns.Add("Remarks", typeof(string));
			dtPlantSummaryFohExpense.Columns.Add("PlantId", typeof(int));
			dtPlantSummaryFohExpense.Columns.Add("PlantName", typeof(string));
			dtPlantSummaryFohExpense.Columns.Add("ConsumedHours", typeof(double));
			dtPlantSummaryFohExpense.Columns.Add("%OfTotalHours", typeof(double));
			dtPlantSummaryFohExpense.Columns.Add("ConsumedFohAmount", typeof(double));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			ScreenName = Conversion.ToString(base.Tag);
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			return new { };
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				GetConfigurationsFromGlobal();
				_ = t.Result;
				BtnSaveFohExpense.Enabled = formrights.DoHaveSaveRight;
				btnUpdateFohExpense.Enabled = formrights.DoHaveUpdateRights;
				BtnDeleteFohExpense.Enabled = formrights.DoHaveCanDelete;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formrights);
				_ = RecIdFohExpense;
				InfragisticsHelper.FinancialYearMonthsComboBind(CmbMonthDateFohExpense);
				MethodTypeBind();
				StatusBind();
				BindGridsFohExpense();
				InfragisticsHelper.FinancialYearMonthsComboBind(CmbMonthHistoryFohExpense);
				FormInitialized = true;
				txtMonthDateFohAllocation.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmProductionFohReportCreate_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnShow_Click(object sender, EventArgs e)
	{
		BindGrid();
	}

	public void BindGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.MonthDate = txtMonthDateFohAllocation.Value;
			dtFohAllocationFromDb = DailyPlantConsumedHours.FormHistoryAndReport(Obj);
			if (dtFohAllocationFromDb.Rows.Count > 0)
			{
				double totalConsumedHours = dtFohAllocationFromDb.AsEnumerable().Sum((DataRow row) => Convert.ToDouble(row["ConsumedHours"]));
				var list = (from row in dtFohAllocationFromDb.AsEnumerable()
					group row by new
					{
						JobOrderId = row["JobOrderId"],
						PlantId = row["PlantId"]
					}).Select(g =>
				{
					DataRow dataRow2 = g.FirstOrDefault();
					return new
					{
						JobOrderId = g.Key.JobOrderId,
						JobOrderNo = dataRow2?["JobOrderNo"]?.ToString(),
						PlantId = g.Key.PlantId,
						PlantName = dataRow2?["PlantName"]?.ToString(),
						ConsumedHours = g.Sum((DataRow r) => (r["ConsumedHours"] == DBNull.Value) ? 0.0 : Convert.ToDouble(r["ConsumedHours"]))
					};
				}).ToList();
				DataTable dtJobPlantWise = new DataTable
				{
					Columns = 
					{
						{
							"JobOrderId",
							typeof(int)
						},
						"JobOrderNo",
						{
							"PlantId",
							typeof(int)
						},
						"PlantName",
						{
							"ConsumedHours",
							typeof(double)
						},
						{
							"%OfTotalHours",
							typeof(double)
						}
					}
				};
				foreach (var item2 in list)
				{
					double percent2 = item2.ConsumedHours / totalConsumedHours * 100.0;
					dtJobPlantWise.Rows.Add(item2.JobOrderId, item2.JobOrderNo, item2.PlantId, item2.PlantName, item2.ConsumedHours, percent2);
				}
				var list2 = (from row in dtFohAllocationFromDb.AsEnumerable()
					group row by new
					{
						PlantId = row["PlantId"]
					}).Select(g =>
				{
					DataRow dataRow = g.FirstOrDefault();
					return new
					{
						PlantId = g.Key.PlantId,
						PlantName = dataRow?["PlantName"]?.ToString(),
						ConsumedHours = g.Sum((DataRow r) => (r["ConsumedHours"] == DBNull.Value) ? 0.0 : Convert.ToDouble(r["ConsumedHours"]))
					};
				}).ToList();
				DataTable dtJobWise = new DataTable
				{
					Columns = 
					{
						{
							"PlantId",
							typeof(int)
						},
						"PlantName",
						{
							"ConsumedHours",
							typeof(double)
						},
						{
							"%OfTotalHours",
							typeof(double)
						}
					}
				};
				foreach (var item in list2)
				{
					double percent = item.ConsumedHours / totalConsumedHours * 100.0;
					dtJobWise.Rows.Add(item.PlantId, item.PlantName, item.ConsumedHours, percent);
				}
				grdJobPlantFohAllocation.DataSource = dtJobPlantWise;
				grdJobPlantFohAllocation.RetrieveStructure();
				grdJobPlantFohAllocation.RootTable.Columns["JobOrderId"].Visible = false;
				grdJobPlantFohAllocation.RootTable.Columns["PlantId"].Visible = false;
				GridEX_Helper.GridWrappingAndColumnSettings(grdJobPlantFohAllocation, 2);
				grdJobPlantFohAllocation.RootTable.Columns["JobOrderNo"].Width = 140;
				grdJobPlantFohAllocation.RootTable.Columns["PlantName"].Width = 110;
				grdJobPlantFohAllocation.RootTable.Columns["ConsumedHours"].Width = 100;
				grdJobPlantFohAllocation.RootTable.Columns["%OfTotalHours"].Width = 75;
				grdPlantFohAllocation.DataSource = dtJobWise;
				grdPlantFohAllocation.RetrieveStructure();
				grdPlantFohAllocation.RootTable.Columns["PlantId"].Visible = false;
				GridEX_Helper.GridWrappingAndColumnSettings(grdPlantFohAllocation, 2);
				grdPlantFohAllocation.RootTable.Columns["PlantName"].Width = 110;
				grdPlantFohAllocation.RootTable.Columns["ConsumedHours"].Width = 100;
				grdPlantFohAllocation.RootTable.Columns["%OfTotalHours"].Width = 75;
			}
			else
			{
				grdJobPlantFohAllocation.DataSource = null;
				grdPlantFohAllocation.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewFohAloocation_Click(object sender, EventArgs e)
	{
		try
		{
			grdPlantFohAllocation.DataSource = null;
			grdJobPlantFohAllocation.DataSource = null;
			txtMonthDateFohAllocation.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtFohAllocationFromDb == null || dtFohAllocationFromDb.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtFohAllocationFromDb, "666_01_MonthlyPlantConsumedHours_Report.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtMonthDateFohExpense_Leave(object sender, EventArgs e)
	{
		try
		{
			DateTime Date = Conversion.ToDateTime(CmbMonthDateFohExpense.Value);
			AccountsBindByMonthDateDbCall(Date);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsBindByMonthDateDbCall(DateTime DocDate)
	{
		try
		{
			DataTable dt = ProdcutionFohAllocateToJobOrder.GetExpensesAccountForFOHAllocation(UserAccount.OrganizationId, UserAccount.CompanyId, DocDate);
			InfragisticsHelper.BindAndRetainSelection(CmbAccountFohExpense, dt, "AccountId", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbAccountFohExpense.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbAccountFohExpense).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbAccountFohExpense).DisplayLayout.Bands[0].Columns[6].Hidden = true;
			}
			CmbAccountFohExpense_Leave(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbAccountFohExpense_Leave(object sender, EventArgs e)
	{
		try
		{
			double Netamount = 0.0;
			int MethodTypeId = 0;
			int Id = Conversion.ToInt(CmbAccountFohExpense.Value);
			if (((UltraGridBase)CmbAccountFohExpense).ActiveRow != null && Id > 0)
			{
				Netamount = Conversion.ToDouble(((UltraDropDownBase)CmbAccountFohExpense).SelectedRow.Cells["NetAmount"].Value);
			}
			txtAccountNetAmountFohExpense.Text = Netamount.ToString(clsGlobalVariables.stringFormatsingle);
			if (MethodTypeId > 0)
			{
				CmbMethodTypeFohExpense.Value = MethodTypeId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MethodTypeBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbMethodTypeFohExpense, dtAllocationType, "Id", "Name", "Method Type", AllColumns: true, Conversion.ToInt(CmbMethodTypeFohExpense.Value));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void StatusBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Type", typeof(string));
			dt.Rows.Add(1, "Complete");
			dt.Rows.Add(2, "InProgress");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnShowFohExpense_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbMonthDateFohExpense, "Month", FormHelper.ValidType.String),
				((Control)(object)CmbAccountFohExpense, "Account", FormHelper.ValidType.INT),
				((Control)(object)CmbMethodTypeFohExpense, "Method Type", FormHelper.ValidType.INT)
			}))
			{
				if (grdFohExpense.RowCount == 0)
				{
					FillForFohExpenseData();
				}
				else
				{
					UpdateFohExpenseCalculations();
				}
				PlantWiseSummaryGridFillFohExpense(dtDetailFohExpense);
				BindGridsFohExpense();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridsFohExpense()
	{
		try
		{
			if (dtDetailFohExpense != null && dtDetailFohExpense.Columns.Count > 0)
			{
				grdFohExpense.DataSource = dtDetailFohExpense;
				grdFohExpense.RetrieveStructure();
				grdSettingsFohExpense();
			}
			if (dtPlantSummaryFohExpense != null && dtPlantSummaryFohExpense.Columns.Count > 0)
			{
				grdPlantWiseSummaryFohExpense.DataSource = dtPlantSummaryFohExpense;
				grdPlantWiseSummaryFohExpense.RetrieveStructure();
				grdPlantWiseSummarySettingsFohExpense();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateFohExpenseCalculations()
	{
		try
		{
			if (dtDetailFohExpense.Rows.Count == 0)
			{
				return;
			}
			double netAmount = Conversion.ToDouble((object)txtAccountNetAmountFohExpense.Text);
			double totalConsumedHours = dtDetailFohExpense.AsEnumerable().Sum((DataRow row) => (row["ConsumedHours"] == DBNull.Value) ? 0.0 : Convert.ToDouble(row["ConsumedHours"]));
			foreach (DataRow row2 in dtDetailFohExpense.Rows)
			{
				double consumedHours = ((row2["ConsumedHours"] != DBNull.Value) ? Convert.ToDouble(row2["ConsumedHours"]) : 0.0);
				double hourRatio = ((totalConsumedHours > 0.0) ? (consumedHours / totalConsumedHours) : 0.0);
				double recalculatedAmount = netAmount * hourRatio;
				row2["ConsumedFohAmount"] = recalculatedAmount;
			}
			((Control)(object)grdFohExpense).Refresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error updating FOH calculations: " + ex.Message);
		}
	}

	private void FillForFohExpenseData()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			if (CmbMonthDateFohExpense.Value == null)
			{
				return;
			}
			DateTime selectedDate = Conversion.ToDateTime(CmbMonthDateFohExpense.Value);
			DataTable dataTable = DailyPlantConsumedHours.FormHistoryAndReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				MonthDate = selectedDate
			});
			if (dataTable.Rows.Count == 0)
			{
				grdFohExpense.DataSource = null;
				grdPlantWiseSummaryFohExpense.DataSource = null;
				return;
			}
			dtDetailFohExpense.Clear();
			double netAmount = Conversion.ToDouble((object)txtAccountNetAmountFohExpense.Text);
			var list = (from row in dataTable.AsEnumerable()
				group row by new
				{
					JobOrderId = Convert.ToInt32(row["JobOrderId"]),
					PlantId = Convert.ToInt32(row["PlantId"])
				}).Select(g =>
			{
				DataRow dataRow = g.First();
				double consumedHours = g.Sum((DataRow r) => (r["ConsumedHours"] == DBNull.Value) ? 0.0 : Convert.ToDouble(r["ConsumedHours"]));
				return new
				{
					JobOrderId = g.Key.JobOrderId,
					JobOrderNo = dataRow["JobOrderNo"]?.ToString(),
					PlantId = g.Key.PlantId,
					PlantName = dataRow["PlantName"]?.ToString(),
					ConsumedHours = consumedHours
				};
			}).ToList();
			double totalConsumedHours = list.Sum(r => r.ConsumedHours);
			Dictionary<int, double> plantHours = (from x in list
				group x by x.PlantId).ToDictionary(g => g.Key, g => g.Sum(x => x.ConsumedHours));
			foreach (var item in list)
			{
				double plantTotal = plantHours[item.PlantId];
				double plantPercentage = ((plantTotal > 0.0) ? (item.ConsumedHours / plantTotal * 100.0) : 0.0);
				double globalHourRatio = ((totalConsumedHours > 0.0) ? (item.ConsumedHours / totalConsumedHours) : 0.0);
				double allocatedAmount = netAmount * globalHourRatio;
				dtDetailFohExpense.Rows.Add(null, Conversion.ToString((object)((Control)(object)CmbMonthDateFohExpense).Text), Conversion.ToInt(CmbAccountFohExpense.Value), item.JobOrderId, item.JobOrderNo, item.PlantId, item.PlantName, item.ConsumedHours, plantPercentage, allocatedAmount, string.Empty);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsFohExpense()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "Month", "AccountId", "JobOrderId", "PlantId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"Remarks",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdFohExpense, 2, 3, hiddenColumns, editableColumns);
			grdFohExpense.RootTable.Groups.Add("PlantName");
			grdFohExpense.GroupTotals = (GroupTotals)2;
			grdFohExpenseWidthSet(grdFohExpense);
			grdFohExpense.RootTable.Columns["ConsumedFohAmount"].EditType = (EditType)(Conversion.ToInt(CmbMethodTypeFohExpense.Value) == 2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFohExpenseWidthSet(GridEX grid)
	{
		try
		{
			grid.RootTable.Columns["JobOrderNo"].Width = Constants.InventoryConstants.ProductionNo;
			grid.RootTable.Columns["PlantName"].Width = Constants.InventoryConstants.PlantName;
			grid.RootTable.Columns["ConsumedHours"].Width = Constants.InventoryConstants.QtyConstant + 10;
			grid.RootTable.Columns["%OfTotalHours"].Width = Constants.InventoryConstants.PercentOfTotal + 20;
			grid.RootTable.Columns["ConsumedFohAmount"].Width = Constants.InventoryConstants.AmountConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFohExpense_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFohExpense.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Column.Key == "ConsumedFohAmount" && Conversion.ToDouble(r.Cells["ConsumedFohAmount"].Value) < 0.0)
			{
				MessageBox.Show("Consumed FOH Amount cannot be negative");
				double consumedHours = Conversion.ToDouble(r.Cells["ConsumedHours"].Value);
				double netAmount = Conversion.ToDouble((object)txtAccountNetAmountFohExpense.Text);
				double totalConsumedHours = dtDetailFohExpense.AsEnumerable().Sum((DataRow row) => row.Field<double>("ConsumedHours"));
				if (totalConsumedHours > 0.0)
				{
					double hourRatio = consumedHours / totalConsumedHours;
					double recalculatedAmount = netAmount * hourRatio;
					r.Cells["ConsumedFohAmount"].Value = Math.Round(recalculatedAmount, 2);
				}
				else
				{
					r.Cells["ConsumedFohAmount"].Value = 0;
				}
			}
			grdFohExpense.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlantWiseSummaryGridFillFohExpense(DataTable dtdetail)
	{
		try
		{
			if (dtdetail.Rows.Count == 0)
			{
				grdPlantWiseSummaryFohExpense.DataSource = null;
				return;
			}
			dtPlantSummaryFohExpense.Clear();
			var list = (from row in dtdetail.AsEnumerable()
				group row by new
				{
					PlantId = row.Field<int>("PlantId"),
					PlantName = row.Field<string>("PlantName")
				}).Select(g =>
			{
				double consumedHours = g.Sum((DataRow r) => Convert.ToDouble(r["ConsumedHours"]));
				double consumedFohAmount = g.Sum((DataRow r) => Convert.ToDouble(r["ConsumedFohAmount"]));
				return new
				{
					PlantId = g.Key.PlantId,
					PlantName = g.Key.PlantName,
					ConsumedHours = consumedHours,
					ConsumedFohAmount = consumedFohAmount
				};
			}).ToList();
			double totalConsumedHours = list.AsEnumerable().Sum(r => r.ConsumedHours);
			foreach (var plant in list)
			{
				double percent = ((totalConsumedHours > 0.0) ? (plant.ConsumedHours / totalConsumedHours * 100.0) : 0.0);
				dtPlantSummaryFohExpense.Rows.Add(plant.PlantId, plant.PlantName, plant.ConsumedHours, percent, plant.ConsumedFohAmount);
			}
			grdPlantWiseSummaryFohExpense.DataSource = dtPlantSummaryFohExpense;
			grdPlantWiseSummaryFohExpense.RetrieveStructure();
			grdPlantWiseSummarySettingsFohExpense();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdPlantWiseSummarySettingsFohExpense()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PlantId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPlantWiseSummaryFohExpense, 2, 3, hiddenColumns);
			grdPlantWiseSummaryFohExpense.RootTable.Columns["PlantName"].Width = Constants.InventoryConstants.PlantName;
			grdPlantWiseSummaryFohExpense.RootTable.Columns["ConsumedHours"].Width = Constants.InventoryConstants.QtyConstant + 10;
			grdPlantWiseSummaryFohExpense.RootTable.Columns["%OfTotalHours"].Width = Constants.InventoryConstants.PercentOfTotal + 20;
			grdPlantWiseSummaryFohExpense.RootTable.Columns["ConsumedFohAmount"].Width = Constants.InventoryConstants.AmountConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertFohExpense()
	{
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		try
		{
			if (grdFohExpense.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbMonthDateFohExpense, "Month", FormHelper.ValidType.String),
				((Control)(object)CmbAccountFohExpense, "Account", FormHelper.ValidType.INT),
				((Control)(object)CmbMethodTypeFohExpense, "Method Type", FormHelper.ValidType.INT)
			}) || !FormHelper.ConfirmAction((RecIdFohExpense == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			double sumAmt = GridEX_Helper.GetColumnSum(grdFohExpense, "ConsumedFohAmount");
			if (Math.Round(Conversion.ToDouble((object)txtAccountNetAmountFohExpense.Text)) != Math.Round(sumAmt))
			{
				throw new Exception("Grid Amount not equal To Net Amount");
			}
			string selectedMonth = Conversion.ToString((object)((Control)(object)CmbMonthDateFohExpense).Text);
			int selectedAccountId = Conversion.ToInt(CmbAccountFohExpense.Value);
			int gridAccountId = 0;
			string gridMonth = "";
			GridEXRow[] dataRows = grdFohExpense.GetDataRows();
			foreach (GridEXRow obj2 in dataRows)
			{
				gridAccountId = Conversion.ToInt(obj2.Cells["AccountId"].Value);
				gridMonth = Conversion.ToString((object)obj2.Cells["Month"].Text);
				if (!string.IsNullOrWhiteSpace(gridMonth) && gridAccountId > 0)
				{
					break;
				}
			}
			if (!string.Equals(selectedMonth, gridMonth, StringComparison.OrdinalIgnoreCase))
			{
				if (MessageBox.Show("The selected month does not match the month in the detail data.\nDo you want to update the selected month to match the detail data's month value?", "Month Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
					return;
				}
				((Control)(object)CmbMonthDateFohExpense).Text = gridMonth;
			}
			if (selectedAccountId != gridAccountId)
			{
				if (MessageBox.Show("The selected account does not match the account in the detail data.\nDo you want to update the selected account to match the detail data's account value?", "Account Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
					return;
				}
				CmbAccountFohExpense.Value = gridAccountId;
			}
			ProdcutionFohAllocateToJobOrder obj = new ProdcutionFohAllocateToJobOrder();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.Id = RecIdFohExpense;
			obj.Month = Conversion.ToString((object)((Control)(object)CmbMonthDateFohExpense).Text);
			DateTime selectedDate = Conversion.ToDateTime((object)((Control)(object)CmbMonthDateFohExpense).Text).Date;
			DateTime startDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
			DateTime endDate = startDate.AddMonths(1).AddDays(-1.0);
			obj.FromDateMonth = startDate;
			obj.ToDateMonth = endDate;
			obj.AccountId = Conversion.ToInt(CmbAccountFohExpense.Value);
			obj.NetAmount = Conversion.ToDouble((object)txtAccountNetAmountFohExpense.Text);
			obj.MethodTypeId = Conversion.ToInt(CmbMethodTypeFohExpense.Value);
			obj.StatusId = 1;
			obj.Status = "Complete";
			obj.RemarksHeader = "";
			obj.ProdcutionFohAllocateToJobOrderDetailList = new List<ProdcutionFohAllocateToJobOrderDetail>();
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			if (lstRemoveRecordFohExpense.Count > 0 && RecIdFohExpense > 0)
			{
				for (int i = 0; i < lstRemoveRecordFohExpense.Count; i++)
				{
					obj.ProdcutionFohAllocateToJobOrderDetailList.Add(lstRemoveRecordFohExpense[i]);
				}
			}
			string RemarksFromDtail = "";
			dataRows = grdFohExpense.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				ProdcutionFohAllocateToJobOrderDetail vd = new ProdcutionFohAllocateToJobOrderDetail();
				vd.Id = ((RecIdFohExpense != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDeleteFohExpense(vd, r);
				FormHelper.ValidateField(vd.JobOrderId, "JobOrder", r.RowIndex);
				FormHelper.ValidateField(vd.PlantId, "Plant", r.RowIndex);
				FormHelper.ValidateField(vd.ConsumedHours, "ConsumedHours", r.RowIndex);
				FormHelper.ValidateField(vd.ConsumedFohAmount, "ConsumedFohAmount", r.RowIndex);
				if (string.IsNullOrWhiteSpace(obj.RemarksHeader))
				{
					RemarksFromDtail = vd.Remarks;
				}
				obj.ProdcutionFohAllocateToJobOrderDetailList.Add(vd);
			}
			if (obj.RemarksHeader == "")
			{
				obj.RemarksHeader = RemarksFromDtail;
			}
			FormHelper.UpdateAttachmentsForObject<ProdcutionFohAllocateToJobOrder>(obj, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			ProdcutionFohAllocateToJobOrder.Save(obj);
			if (RecIdFohExpense == 0)
			{
				MessageBox.Show("Record Save Successfully");
			}
			else
			{
				MessageBox.Show("Record Update Successfully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecIdFohExpense, ScreenName, DocumentTypeId, 0);
				}
			}
			ResetFohExpense();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnSaveFohExpense_Click(object sender, EventArgs e)
	{
		RecIdFohExpense = 0;
		InsertFohExpense();
	}

	private void btnUpdateFohExpense_Click(object sender, EventArgs e)
	{
		if (RecIdFohExpense == 0)
		{
			throw new Exception("RecId not Found");
		}
		InsertFohExpense();
	}

	private void BtnDeleteFohExpense_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecIdFohExpense > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ProdcutionFohAllocateToJobOrder.DeleteByID(RecIdFohExpense, UserAccount.ID);
					MessageBox.Show("Delete Record Successfully");
					ResetFohExpense();
				}
				return;
			}
			throw new Exception("RecordId Not Found.....");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDeleteFohExpense(ProdcutionFohAllocateToJobOrderDetail vd, GridEXRow r)
	{
		vd.JobOrderId = Conversion.ToInt(r.Cells["JobOrderId"].Value);
		vd.JobOrderNo = Conversion.ToString(r.Cells["JobOrderNo"].Value);
		vd.PlantId = Conversion.ToInt(r.Cells["PlantId"].Value);
		vd.PlantName = Conversion.ToString(r.Cells["PlantName"].Value);
		vd.ConsumedHours = Conversion.ToDouble(r.Cells["ConsumedHours"].Value);
		vd.ConsumedFohAmount = Conversion.ToDouble(r.Cells["ConsumedFohAmount"].Value);
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	public void ReadByIdFohExpense(int ID)
	{
		try
		{
			ResetFohExpense();
			RecIdFohExpense = ID;
			lstRemoveRecordFohExpense.Clear();
			ProdcutionFohAllocateToJobOrder Obj = ProdcutionFohAllocateToJobOrder.GetByID(ID);
			if (Obj != null)
			{
				tabControl1.SelectedIndex = 0;
				BtnSaveFohExpense.Visible = false;
				btnUpdateFohExpense.Visible = true;
				BtnDeleteFohExpense.Visible = true;
				((Control)(object)CmbMonthDateFohExpense).Text = Conversion.ToString((object)Obj.Month);
				txtMonthDateFohExpense_Leave(null, null);
				CmbAccountFohExpense.Value = Obj.AccountId;
				txtAccountNetAmountFohExpense.Text = Obj.NetAmount.ToString(clsGlobalVariables.stringFormatsingle);
				CmbMethodTypeFohExpense.Value = Obj.MethodTypeId;
				AttachmentsValues = Obj.AttachmentsValues;
				CustomAttachmentsValues = Obj.CustomAttachmentsValues;
				dtDetailFohExpense.Rows.Clear();
				FillDetailFromListCommonForReadById(Obj, dtDetailFohExpense);
				PlantWiseSummaryGridFillFohExpense(dtDetailFohExpense);
				BindGridsFohExpense();
				FormHelper.LoadAttachmentsForObject(AT, RecIdFohExpense, ScreenName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(ProdcutionFohAllocateToJobOrder invGrn, DataTable dt)
	{
		if (invGrn == null || invGrn.ProdcutionFohAllocateToJobOrderDetailList == null || invGrn.ProdcutionFohAllocateToJobOrderDetailList.Count == 0)
		{
			return;
		}
		Dictionary<int, double> plantTotalHours = (from x in invGrn.ProdcutionFohAllocateToJobOrderDetailList
			group x by x.PlantId).ToDictionary((IGrouping<int, ProdcutionFohAllocateToJobOrderDetail> g) => g.Key, (IGrouping<int, ProdcutionFohAllocateToJobOrderDetail> g) => g.Sum((ProdcutionFohAllocateToJobOrderDetail x) => x.ConsumedHours));
		foreach (ProdcutionFohAllocateToJobOrderDetail detail in invGrn.ProdcutionFohAllocateToJobOrderDetailList)
		{
			double totalConsumedHoursForPlant = (plantTotalHours.ContainsKey(detail.PlantId) ? plantTotalHours[detail.PlantId] : 0.0);
			double percent = ((totalConsumedHoursForPlant > 0.0) ? (detail.ConsumedHours / totalConsumedHoursForPlant * 100.0) : 0.0);
			List<object> rowValues = new List<object>
			{
				detail.Id, invGrn.Month, invGrn.AccountId, detail.JobOrderId, detail.JobOrderNo, detail.PlantId, detail.PlantName, detail.ConsumedHours, percent, detail.ConsumedFohAmount,
				detail.Remarks
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void BtnRefreshFohExpense_Click(object sender, EventArgs e)
	{
		try
		{
			GetConfigurationsFromGlobal();
			InfragisticsHelper.FinancialYearMonthsComboBind(CmbMonthDateFohExpense);
			MethodTypeBind();
			StatusBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetFohExpense()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecordFohExpense.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			BtnSaveFohExpense.Visible = true;
			btnUpdateFohExpense.Visible = false;
			BtnDeleteFohExpense.Visible = false;
			RecIdFohExpense = 0;
			((UltraGridBase)CmbAccountFohExpense).DataSource = null;
			((Control)(object)CmbAccountFohExpense).Text = string.Empty;
			txtAccountNetAmountFohExpense.Text = string.Empty;
			dtDetailFohExpense.Rows.Clear();
			dtPlantSummaryFohExpense.Rows.Clear();
			BindGridsFohExpense();
			tabMain.SelectedIndex = 1;
			tabControl1.SelectedIndex = 0;
			CmbMonthDateFohExpense.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewFohExpense_Click(object sender, EventArgs e)
	{
		ResetFohExpense();
	}

	private void btnResetHistoryFohExpense_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistoryFohExpense.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistoryFohExpense.Value = DateTime.Now;
			((Control)(object)CmbMonthHistoryFohExpense).Text = string.Empty;
			grdHistoryFohExpense.DataSource = null;
			grdDetailHistoryFohExpense.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistoryFohExpense_Click(object sender, EventArgs e)
	{
		try
		{
			InfragisticsHelper.FinancialYearMonthsComboBind(CmbMonthHistoryFohExpense);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistoryFohExpense_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (rdentrydate.Checked)
			{
				if (FromDateHistoryFohExpense.Checked)
				{
					obj.EntryDateFrom = FromDateHistoryFohExpense.Value;
				}
				if (ToDateHistoryFohExpense.Checked)
				{
					obj.EntryDateTo = ToDateHistoryFohExpense.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistoryFohExpense.Checked)
				{
					obj.ModifyDateFrom = FromDateHistoryFohExpense.Value;
				}
				if (ToDateHistoryFohExpense.Checked)
				{
					obj.ModifyDateTo = ToDateHistoryFohExpense.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistoryFohExpense.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistoryFohExpense.Value;
				}
				if (ToDateHistoryFohExpense.Checked)
				{
					obj.ApprovedDateTo = ToDateHistoryFohExpense.Value;
				}
			}
			obj.Month = Conversion.ToString(CmbMonthHistoryFohExpense.Value);
			DataTable dt = ProdcutionFohAllocateToJobOrder.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				if (dtHistoryFohExpense.Columns.Count == 0)
				{
					dtHistoryFohExpense.Columns.Add("Id", typeof(int));
					dtHistoryFohExpense.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryFohExpense.Columns.Add("FromDateMonth", typeof(string));
					dtHistoryFohExpense.Columns.Add("ToDateMonth", typeof(string));
					dtHistoryFohExpense.Columns.Add("Month", typeof(string));
					dtHistoryFohExpense.Columns.Add("AccountId", typeof(int));
					dtHistoryFohExpense.Columns.Add("AccountTitle", typeof(string));
					dtHistoryFohExpense.Columns.Add("NetAmount", typeof(double));
					dtHistoryFohExpense.Columns.Add("MethodType", typeof(string));
					dtHistoryFohExpense.Columns.Add("Status", typeof(string));
					dtHistoryFohExpense.Columns.Add("RemarksHeader", typeof(string));
					dtHistoryFohExpense.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryFohExpense.Columns.Add("EntryUser", typeof(string));
					dtHistoryFohExpense.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryFohExpense.Columns.Add("ModifyUser", typeof(string));
					dtHistoryFohExpense.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistoryFohExpense.Columns.Add("ApprovedUser", typeof(string));
					dtHistoryFohExpense.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistoryFohExpense.Rows.Clear();
				foreach (DataRow row in dt.Rows)
				{
					dtHistoryFohExpense.Rows.Add(row["Id"], row["DocumentTypeId"], row["FromDateMonth"], row["ToDateMonth"], row["Month"], row["AccountId"], row["AccountTitle"], row["NetAmount"], row["MethodType"], row["Status"], row["RemarksHeader"], row["EntryDate"], row["EntryUser"], row["ModifyDate"], row["ModifyUser"], row["ApprovedDate"], row["ApprovedUser"], row["NoOfAttachments"]);
				}
				grdHistoryFohExpense.DataSource = dtHistoryFohExpense;
				grdHistoryFohExpense.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdHistoryFohExpense.ClearStructure();
				grdDetailHistoryFohExpense.ClearStructure();
			}
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "AccountId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryFohExpense, 2, 3, hiddenColumns);
			grdHistoryFohExpense.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistoryFohExpense.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistoryFohExpense.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistoryFohExpense.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GridEX_Helper.AddButton(grdHistoryFohExpense, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(grdHistoryFohExpense, "Print", "Print", 50, 1);
			grdHistoryFohExpense.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryFohExpense_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = grdHistoryFohExpense.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				RecIdFohExpense = Conversion.ToInt(r.Cells["Id"].Value);
				ReadByIdFohExpense(RecIdFohExpense);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryFohExpense_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = grdHistoryFohExpense.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Print")
				{
					CommonServices.ProdcutionFohAllocateToJobOrderPrint922(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Edit")
				{
					int Id = Conversion.ToInt(r.Cells["Id"].Value);
					ReadByIdFohExpense(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryFohExpense_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryFohExpense.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			ProdcutionFohAllocateToJobOrder Obj = ProdcutionFohAllocateToJobOrder.GetByID(Id);
			if (Obj != null)
			{
				List<ProdcutionFohAllocateToJobOrderDetail> prodcutionFohAllocateToJobOrderDetailList = Obj.ProdcutionFohAllocateToJobOrderDetailList;
				if (prodcutionFohAllocateToJobOrderDetailList == null || prodcutionFohAllocateToJobOrderDetailList.Count != 0)
				{
					DataTable dtDetailHistory = dtDetailFohExpense.Clone();
					FillDetailFromListCommonForReadById(Obj, dtDetailHistory);
					grdDetailHistoryFohExpense.DataSource = dtDetailHistory;
					grdDetailHistoryFohExpense.RetrieveStructure();
					grddetailhistorySettings();
					return;
				}
			}
			grdDetailHistoryFohExpense.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "JobOrderId", "PlantId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetailHistoryFohExpense, 2, 3, hiddenColumns);
			grdFohExpenseWidthSet(grdDetailHistoryFohExpense);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachmentFohExpense_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void BtnPrintFohExpense_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ProdcutionFohAllocateToJobOrderPrint922(RecIdFohExpense);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmProductionFohReportCreate_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabMain.SelectedIndex == 1)
				{
					tabMain.SelectedIndex = 0;
					txtMonthDateFohAllocation.Focus();
				}
				else
				{
					tabMain.SelectedIndex = 1;
				}
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtMonthDateFohAllocation.Focus();
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 121;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNewFohAloocation_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 82;
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabMain.SelectedIndex == 0)
				{
					txtMonthDateFohAllocation.Focus();
				}
				else
				{
					txtMonthDateFohAllocation.Focus();
				}
			}
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Month");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Grids");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Column in Detail for Entry");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void grdHistoryFohExpense_KeyDown(object sender, KeyEventArgs e)
	{
	}

	private void BtnShortCutKeyFohExpense_Click(object sender, EventArgs e)
	{
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
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Expected O, but got Unknown
		//IL_14df: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e9: Expected O, but got Unknown
		//IL_3025: Unknown result type (might be due to invalid IL or missing references)
		//IL_302f: Expected O, but got Unknown
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
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
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.Regular.frmProductionFohReportCreate));
		GridEXLayout grdHistoryFohExpense_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetailHistoryFohExpense_DesignTimeLayout = new GridEXLayout();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		this.tabMain = new System.Windows.Forms.TabControl();
		this.tabFohAllocation = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdJobPlantFohAllocation = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdPlantFohAllocation = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnShowFohAllocation = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.txtMonthDateFohAllocation = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnNewFohAloocation = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrintFohallocation = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeyFohAllocation = new System.Windows.Forms.ToolStripButton();
		this.tabFohExpense = new System.Windows.Forms.TabPage();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabFormFohExpense = new System.Windows.Forms.TabPage();
		this.grdFohExpense = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.grdPlantWiseSummaryFohExpense = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.CmbMonthDateFohExpense = new UltraCombo();
		this.txtAccountNetAmountFohExpense = new System.Windows.Forms.TextBox();
		this.BtnShowFohExpense = new System.Windows.Forms.Button();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbMethodTypeFohExpense = new UltraCombo();
		this.CmbAccountFohExpense = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNewFohExpense = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshFohExpense = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnSaveFohExpense = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateFohExpense = new System.Windows.Forms.ToolStripButton();
		this.BtnDeleteFohExpense = new System.Windows.Forms.ToolStripButton();
		this.btnAttachmentFohExpense = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintFohExpense = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutKeyFohExpense = new System.Windows.Forms.ToolStripButton();
		this.tabHistoryFohExpense = new System.Windows.Forms.TabPage();
		this.grdHistoryFohExpense = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.grdDetailHistoryFohExpense = new GridEX();
		this.panel29 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.CmbMonthHistoryFohExpense = new UltraCombo();
		this.ToDateHistoryFohExpense = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistoryFohExpense = new System.Windows.Forms.DateTimePicker();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistoryFohExpense = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label67 = new System.Windows.Forms.Label();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistoryFohExpense = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistoryFohExpense = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabMain.SuspendLayout();
		this.tabFohAllocation.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdJobPlantFohAllocation).BeginInit();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPlantFohAllocation).BeginInit();
		this.panel5.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabFohExpense.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabFormFohExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFohExpense).BeginInit();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPlantWiseSummaryFohExpense).BeginInit();
		this.panel14.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMonthDateFohExpense).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMethodTypeFohExpense).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountFohExpense).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabHistoryFohExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryFohExpense).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistoryFohExpense).BeginInit();
		this.panel29.SuspendLayout();
		this.panel18.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMonthHistoryFohExpense).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabMain.Controls.Add(this.tabFohAllocation);
		this.tabMain.Controls.Add(this.tabFohExpense);
		this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabMain.Location = new System.Drawing.Point(0, 0);
		this.tabMain.Name = "tabMain";
		this.tabMain.SelectedIndex = 0;
		this.tabMain.Size = new System.Drawing.Size(964, 561);
		this.tabMain.TabIndex = 0;
		this.tabFohAllocation.Controls.Add(this.panel3);
		this.tabFohAllocation.Controls.Add(this.panel4);
		this.tabFohAllocation.Controls.Add(this.panel1);
		this.tabFohAllocation.Controls.Add(this.panel6);
		this.tabFohAllocation.Location = new System.Drawing.Point(4, 25);
		this.tabFohAllocation.Name = "tabFohAllocation";
		this.tabFohAllocation.Padding = new System.Windows.Forms.Padding(3);
		this.tabFohAllocation.Size = new System.Drawing.Size(956, 532);
		this.tabFohAllocation.TabIndex = 0;
		this.tabFohAllocation.Text = "FOH Allocation";
		this.tabFohAllocation.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdJobPlantFohAllocation);
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(313, 66);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(450, 463);
		this.panel3.TabIndex = 2;
		this.grdJobPlantFohAllocation.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdJobPlantFohAllocation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdJobPlantFohAllocation.DynamicFiltering = true;
		this.grdJobPlantFohAllocation.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdJobPlantFohAllocation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdJobPlantFohAllocation.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdJobPlantFohAllocation).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdJobPlantFohAllocation).Name = "grdJobPlantFohAllocation";
		((ScrollableBase)this.grdJobPlantFohAllocation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdJobPlantFohAllocation).Size = new System.Drawing.Size(450, 442);
		((System.Windows.Forms.Control)(object)this.grdJobPlantFohAllocation).TabIndex = 0;
		this.grdJobPlantFohAllocation.TotalRow = (InheritableBoolean)1;
		this.grdJobPlantFohAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdJobPlantFohAllocation).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(450, 21);
		this.panel2.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(172, 16);
		this.label2.TabIndex = 1;
		this.label2.Text = "Job Order And Plant Wise";
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPlantFohAllocation);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(3, 66);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(310, 463);
		this.panel4.TabIndex = 3;
		this.grdPlantFohAllocation.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdPlantFohAllocation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPlantFohAllocation.DynamicFiltering = true;
		this.grdPlantFohAllocation.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdPlantFohAllocation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPlantFohAllocation.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPlantFohAllocation).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdPlantFohAllocation).Name = "grdPlantFohAllocation";
		((ScrollableBase)this.grdPlantFohAllocation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPlantFohAllocation).Size = new System.Drawing.Size(310, 442);
		((System.Windows.Forms.Control)(object)this.grdPlantFohAllocation).TabIndex = 0;
		this.grdPlantFohAllocation.TotalRow = (InheritableBoolean)1;
		this.grdPlantFohAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPlantFohAllocation).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(310, 21);
		this.panel5.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(2, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(75, 16);
		this.label3.TabIndex = 1;
		this.label3.Text = "Plant Wise";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.btnShowFohAllocation);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.txtMonthDateFohAllocation);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 30);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(950, 36);
		this.panel1.TabIndex = 0;
		this.btnShowFohAllocation.BackColor = System.Drawing.Color.Teal;
		this.btnShowFohAllocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowFohAllocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowFohAllocation.ForeColor = System.Drawing.Color.White;
		this.btnShowFohAllocation.Location = new System.Drawing.Point(162, 7);
		this.btnShowFohAllocation.Name = "btnShowFohAllocation";
		this.btnShowFohAllocation.Size = new System.Drawing.Size(59, 24);
		this.btnShowFohAllocation.TabIndex = 1;
		this.btnShowFohAllocation.Text = "Show";
		this.btnShowFohAllocation.UseVisualStyleBackColor = false;
		this.btnShowFohAllocation.Click += new System.EventHandler(btnShow_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(43, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "Month";
		this.txtMonthDateFohAllocation.CustomFormat = "MMM-yyyy";
		this.txtMonthDateFohAllocation.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtMonthDateFohAllocation.Location = new System.Drawing.Point(49, 7);
		this.txtMonthDateFohAllocation.Name = "txtMonthDateFohAllocation";
		this.txtMonthDateFohAllocation.Size = new System.Drawing.Size(111, 23);
		this.txtMonthDateFohAllocation.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.toolStrip3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 27);
		this.panel6.TabIndex = 4;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.BtnNewFohAloocation, this.toolStripSeparator2, this.BtnRefresh, this.btnPrintFohallocation, this.btnShortCutKeyFohAllocation });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(950, 27);
		this.toolStrip3.TabIndex = 58;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnNewFohAloocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewFohAloocation.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewFohAloocation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewFohAloocation.Name = "BtnNewFohAloocation";
		this.BtnNewFohAloocation.Size = new System.Drawing.Size(59, 24);
		this.BtnNewFohAloocation.Text = "&New";
		this.BtnNewFohAloocation.Click += new System.EventHandler(BtnNewFohAloocation_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Visible = false;
		this.btnPrintFohallocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintFohallocation.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintFohallocation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintFohallocation.Name = "btnPrintFohallocation";
		this.btnPrintFohallocation.Size = new System.Drawing.Size(116, 24);
		this.btnPrintFohallocation.Text = "Print-666_01";
		this.btnPrintFohallocation.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortCutKeyFohAllocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeyFohAllocation.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeyFohAllocation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeyFohAllocation.Name = "btnShortCutKeyFohAllocation";
		this.btnShortCutKeyFohAllocation.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeyFohAllocation.Text = "ShortCut Keys";
		this.btnShortCutKeyFohAllocation.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabFohExpense.Controls.Add(this.tabControl1);
		this.tabFohExpense.Location = new System.Drawing.Point(4, 25);
		this.tabFohExpense.Name = "tabFohExpense";
		this.tabFohExpense.Padding = new System.Windows.Forms.Padding(3);
		this.tabFohExpense.Size = new System.Drawing.Size(956, 532);
		this.tabFohExpense.TabIndex = 1;
		this.tabFohExpense.Text = "FOH Expense";
		this.tabFohExpense.UseVisualStyleBackColor = true;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabFormFohExpense);
		this.tabControl1.Controls.Add(this.tabHistoryFohExpense);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(3, 3);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(950, 526);
		this.tabControl1.TabIndex = 66;
		this.tabFormFohExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdFohExpense);
		this.tabFormFohExpense.Controls.Add(this.panel16);
		this.tabFormFohExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdPlantWiseSummaryFohExpense);
		this.tabFormFohExpense.Controls.Add(this.panel14);
		this.tabFormFohExpense.Controls.Add(this.panel7);
		this.tabFormFohExpense.Controls.Add(this.toolStrip1);
		this.tabFormFohExpense.Location = new System.Drawing.Point(4, 4);
		this.tabFormFohExpense.Name = "tabFormFohExpense";
		this.tabFormFohExpense.Padding = new System.Windows.Forms.Padding(3);
		this.tabFormFohExpense.Size = new System.Drawing.Size(942, 497);
		this.tabFormFohExpense.TabIndex = 0;
		this.tabFormFohExpense.Text = "Form";
		this.tabFormFohExpense.UseVisualStyleBackColor = true;
		this.grdFohExpense.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdFohExpense).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdFohExpense.DynamicFiltering = true;
		this.grdFohExpense.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdFohExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdFohExpense).Location = new System.Drawing.Point(3, 103);
		((System.Windows.Forms.Control)(object)this.grdFohExpense).Name = "grdFohExpense";
		((ScrollableBase)this.grdFohExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFohExpense).Size = new System.Drawing.Size(936, 186);
		((System.Windows.Forms.Control)(object)this.grdFohExpense).TabIndex = 1;
		this.grdFohExpense.TotalRow = (InheritableBoolean)1;
		this.grdFohExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFohExpense).VisualStyle = (VisualStyle)4;
		this.grdFohExpense.CellUpdated += new ColumnActionEventHandler(grdFohExpense_CellUpdated);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label7);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel16.Location = new System.Drawing.Point(3, 289);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(936, 21);
		this.panel16.TabIndex = 65;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(2, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(139, 16);
		this.label7.TabIndex = 1;
		this.label7.Text = "Plant Wise Summary";
		this.grdPlantWiseSummaryFohExpense.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdPlantWiseSummaryFohExpense).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdPlantWiseSummaryFohExpense.DynamicFiltering = true;
		this.grdPlantWiseSummaryFohExpense.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdPlantWiseSummaryFohExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPlantWiseSummaryFohExpense.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPlantWiseSummaryFohExpense).Location = new System.Drawing.Point(3, 310);
		((System.Windows.Forms.Control)(object)this.grdPlantWiseSummaryFohExpense).Name = "grdPlantWiseSummaryFohExpense";
		((ScrollableBase)this.grdPlantWiseSummaryFohExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPlantWiseSummaryFohExpense).Size = new System.Drawing.Size(936, 184);
		((System.Windows.Forms.Control)(object)this.grdPlantWiseSummaryFohExpense).TabIndex = 2;
		this.grdPlantWiseSummaryFohExpense.TotalRow = (InheritableBoolean)1;
		this.grdPlantWiseSummaryFohExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPlantWiseSummaryFohExpense).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label6);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 82);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(936, 21);
		this.panel14.TabIndex = 63;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(2, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 16);
		this.label6.TabIndex = 1;
		this.label6.Text = "Detail Data";
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMonthDateFohExpense);
		this.panel7.Controls.Add(this.txtAccountNetAmountFohExpense);
		this.panel7.Controls.Add(this.BtnShowFohExpense);
		this.panel7.Controls.Add(this.label11);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMethodTypeFohExpense);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountFohExpense);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(936, 52);
		this.panel7.TabIndex = 0;
		this.CmbMonthDateFohExpense.AllowNull = (DefaultableBoolean)1;
		this.CmbMonthDateFohExpense.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMonthDateFohExpense.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMonthDateFohExpense.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMonthDateFohExpense).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMonthDateFohExpense).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMonthDateFohExpense.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMonthDateFohExpense).Location = new System.Drawing.Point(4, 22);
		((System.Windows.Forms.Control)(object)this.CmbMonthDateFohExpense).Name = "CmbMonthDateFohExpense";
		((System.Windows.Forms.Control)(object)this.CmbMonthDateFohExpense).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbMonthDateFohExpense).TabIndex = 0;
		this.CmbMonthDateFohExpense.ValueChanged += new System.EventHandler(txtMonthDateFohExpense_Leave);
		((System.Windows.Forms.Control)(object)this.CmbMonthDateFohExpense).Leave += new System.EventHandler(txtMonthDateFohExpense_Leave);
		this.txtAccountNetAmountFohExpense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAccountNetAmountFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccountNetAmountFohExpense.Location = new System.Drawing.Point(377, 24);
		this.txtAccountNetAmountFohExpense.Name = "txtAccountNetAmountFohExpense";
		this.txtAccountNetAmountFohExpense.ReadOnly = true;
		this.txtAccountNetAmountFohExpense.Size = new System.Drawing.Size(120, 23);
		this.txtAccountNetAmountFohExpense.TabIndex = 2;
		this.txtAccountNetAmountFohExpense.TabStop = false;
		this.txtAccountNetAmountFohExpense.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.BtnShowFohExpense.BackColor = System.Drawing.Color.Teal;
		this.BtnShowFohExpense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowFohExpense.ForeColor = System.Drawing.Color.White;
		this.BtnShowFohExpense.Location = new System.Drawing.Point(763, 22);
		this.BtnShowFohExpense.Name = "BtnShowFohExpense";
		this.BtnShowFohExpense.Size = new System.Drawing.Size(50, 26);
		this.BtnShowFohExpense.TabIndex = 5;
		this.BtnShowFohExpense.Text = "Run";
		this.BtnShowFohExpense.UseVisualStyleBackColor = false;
		this.BtnShowFohExpense.Click += new System.EventHandler(BtnShowFohExpense_Click);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(499, 6);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(77, 15);
		this.label11.TabIndex = 199;
		this.label11.Text = "Method Type";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(377, 6);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(113, 15);
		this.label10.TabIndex = 199;
		this.label10.Text = "Total Amount (Net) \r\n";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 6);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(43, 15);
		this.label4.TabIndex = 1;
		this.label4.Text = "Month";
		this.CmbMethodTypeFohExpense.AllowNull = (DefaultableBoolean)1;
		this.CmbMethodTypeFohExpense.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMethodTypeFohExpense.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMethodTypeFohExpense.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMethodTypeFohExpense).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMethodTypeFohExpense).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMethodTypeFohExpense.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMethodTypeFohExpense).Location = new System.Drawing.Point(499, 22);
		((System.Windows.Forms.Control)(object)this.CmbMethodTypeFohExpense).Name = "CmbMethodTypeFohExpense";
		((System.Windows.Forms.Control)(object)this.CmbMethodTypeFohExpense).Size = new System.Drawing.Size(258, 26);
		((System.Windows.Forms.Control)(object)this.CmbMethodTypeFohExpense).TabIndex = 3;
		this.CmbAccountFohExpense.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountFohExpense.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountFohExpense.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountFohExpense.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountFohExpense).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountFohExpense).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountFohExpense.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountFohExpense).Location = new System.Drawing.Point(125, 22);
		((System.Windows.Forms.Control)(object)this.CmbAccountFohExpense).Name = "CmbAccountFohExpense";
		((System.Windows.Forms.Control)(object)this.CmbAccountFohExpense).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountFohExpense).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbAccountFohExpense).Leave += new System.EventHandler(CmbAccountFohExpense_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(125, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 15);
		this.label9.TabIndex = 195;
		this.label9.Text = "Account";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.BtnNewFohExpense, this.BtnRefreshFohExpense, this.toolStripSeparator1, this.BtnSaveFohExpense, this.btnUpdateFohExpense, this.BtnDeleteFohExpense, this.btnAttachmentFohExpense, this.BtnPrintFohExpense, this.BtnShortCutKeyFohExpense });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(936, 27);
		this.toolStrip1.TabIndex = 60;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNewFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewFohExpense.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewFohExpense.Name = "BtnNewFohExpense";
		this.BtnNewFohExpense.Size = new System.Drawing.Size(59, 24);
		this.BtnNewFohExpense.Text = "&New";
		this.BtnNewFohExpense.Click += new System.EventHandler(BtnNewFohExpense_Click);
		this.BtnRefreshFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshFohExpense.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshFohExpense.Name = "BtnRefreshFohExpense";
		this.BtnRefreshFohExpense.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshFohExpense.Text = "Refresh";
		this.BtnRefreshFohExpense.Click += new System.EventHandler(BtnRefreshFohExpense_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnSaveFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveFohExpense.Image = (System.Drawing.Image)resources.GetObject("BtnSaveFohExpense.Image");
		this.BtnSaveFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveFohExpense.Name = "BtnSaveFohExpense";
		this.BtnSaveFohExpense.Size = new System.Drawing.Size(64, 24);
		this.BtnSaveFohExpense.Text = "&Save";
		this.BtnSaveFohExpense.Click += new System.EventHandler(BtnSaveFohExpense_Click);
		this.btnUpdateFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateFohExpense.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateFohExpense.Name = "btnUpdateFohExpense";
		this.btnUpdateFohExpense.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateFohExpense.Text = "Update";
		this.btnUpdateFohExpense.Visible = false;
		this.btnUpdateFohExpense.Click += new System.EventHandler(btnUpdateFohExpense_Click);
		this.BtnDeleteFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDeleteFohExpense.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDeleteFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDeleteFohExpense.Name = "BtnDeleteFohExpense";
		this.BtnDeleteFohExpense.Size = new System.Drawing.Size(73, 24);
		this.BtnDeleteFohExpense.Text = "Delete";
		this.BtnDeleteFohExpense.Visible = false;
		this.BtnDeleteFohExpense.Click += new System.EventHandler(BtnDeleteFohExpense_Click);
		this.btnAttachmentFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachmentFohExpense.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachmentFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachmentFohExpense.Name = "btnAttachmentFohExpense";
		this.btnAttachmentFohExpense.Size = new System.Drawing.Size(109, 24);
		this.btnAttachmentFohExpense.Text = "Attachment";
		this.btnAttachmentFohExpense.Click += new System.EventHandler(btnAttachmentFohExpense_Click);
		this.BtnPrintFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintFohExpense.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintFohExpense.Name = "BtnPrintFohExpense";
		this.BtnPrintFohExpense.Size = new System.Drawing.Size(92, 24);
		this.BtnPrintFohExpense.Text = "Print-922";
		this.BtnPrintFohExpense.Click += new System.EventHandler(BtnPrintFohExpense_Click);
		this.BtnShortCutKeyFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutKeyFohExpense.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutKeyFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutKeyFohExpense.Name = "BtnShortCutKeyFohExpense";
		this.BtnShortCutKeyFohExpense.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutKeyFohExpense.Text = "ShortCut Keys";
		this.BtnShortCutKeyFohExpense.Click += new System.EventHandler(BtnShortCutKeyFohExpense_Click);
		this.tabHistoryFohExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryFohExpense);
		this.tabHistoryFohExpense.Controls.Add(this.panel13);
		this.tabHistoryFohExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailHistoryFohExpense);
		this.tabHistoryFohExpense.Controls.Add(this.panel29);
		this.tabHistoryFohExpense.Controls.Add(this.panel18);
		this.tabHistoryFohExpense.Controls.Add(this.panel17);
		this.tabHistoryFohExpense.Location = new System.Drawing.Point(4, 4);
		this.tabHistoryFohExpense.Name = "tabHistoryFohExpense";
		this.tabHistoryFohExpense.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistoryFohExpense.Size = new System.Drawing.Size(942, 497);
		this.tabHistoryFohExpense.TabIndex = 1;
		this.tabHistoryFohExpense.Text = "History";
		this.tabHistoryFohExpense.UseVisualStyleBackColor = true;
		this.grdHistoryFohExpense.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryFohExpense).BackColor = System.Drawing.Color.White;
		this.grdHistoryFohExpense.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistoryFohExpense_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryFohExpense.DesignTimeLayout = grdHistoryFohExpense_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryFohExpense).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryFohExpense.FilterMode = (FilterMode)2;
		this.grdHistoryFohExpense.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryFohExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryFohExpense.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryFohExpense).Location = new System.Drawing.Point(3, 125);
		((System.Windows.Forms.Control)(object)this.grdHistoryFohExpense).Name = "grdHistoryFohExpense";
		((ScrollableBase)this.grdHistoryFohExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryFohExpense).Size = new System.Drawing.Size(936, 159);
		((System.Windows.Forms.Control)(object)this.grdHistoryFohExpense).TabIndex = 0;
		this.grdHistoryFohExpense.TotalRow = (InheritableBoolean)1;
		this.grdHistoryFohExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryFohExpense).VisualStyle = (VisualStyle)4;
		this.grdHistoryFohExpense.ColumnButtonClick += new ColumnActionEventHandler(grdHistoryFohExpense_ColumnButtonClick);
		this.grdHistoryFohExpense.SelectionChanged += new System.EventHandler(grdHistoryFohExpense_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdHistoryFohExpense).DoubleClick += new System.EventHandler(grdHistoryFohExpense_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistoryFohExpense).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistoryFohExpense_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label59);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(3, 284);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(936, 20);
		this.panel13.TabIndex = 346;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(5, 2);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(228, 17);
		this.label59.TabIndex = 562;
		this.label59.Text = "Detail of the selected above row";
		this.grdDetailHistoryFohExpense.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetailHistoryFohExpense).BackColor = System.Drawing.Color.White;
		this.grdDetailHistoryFohExpense.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdDetailHistoryFohExpense_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailHistoryFohExpense.DesignTimeLayout = grdDetailHistoryFohExpense_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailHistoryFohExpense).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetailHistoryFohExpense.FilterMode = (FilterMode)2;
		this.grdDetailHistoryFohExpense.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailHistoryFohExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailHistoryFohExpense.GroupByBoxVisible = false;
		this.grdDetailHistoryFohExpense.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetailHistoryFohExpense).Location = new System.Drawing.Point(3, 304);
		((System.Windows.Forms.Control)(object)this.grdDetailHistoryFohExpense).Name = "grdDetailHistoryFohExpense";
		((ScrollableBase)this.grdDetailHistoryFohExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailHistoryFohExpense).Size = new System.Drawing.Size(936, 193);
		((System.Windows.Forms.Control)(object)this.grdDetailHistoryFohExpense).TabIndex = 1;
		this.grdDetailHistoryFohExpense.TotalRow = (InheritableBoolean)1;
		this.grdDetailHistoryFohExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailHistoryFohExpense).VisualStyle = (VisualStyle)4;
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.label65);
		this.panel29.Controls.Add(this.ctrlGrdBar1);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(3, 101);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(936, 24);
		this.panel29.TabIndex = 353;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.White;
		this.label65.Location = new System.Drawing.Point(4, 2);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(142, 19);
		this.label65.TabIndex = 562;
		this.label65.Text = "Filtered Records";
		this.panel18.Controls.Add(this.groupBox5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 56);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(936, 45);
		this.panel18.TabIndex = 350;
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMonthHistoryFohExpense);
		this.groupBox5.Controls.Add(this.ToDateHistoryFohExpense);
		this.groupBox5.Controls.Add(this.FromDateHistoryFohExpense);
		this.groupBox5.Controls.Add(this.rdapproveddate);
		this.groupBox5.Controls.Add(this.btnShowHistoryFohExpense);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.label67);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.label90);
		this.groupBox5.Controls.Add(this.label91);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox5.Location = new System.Drawing.Point(4, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(893, 43);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.CmbMonthHistoryFohExpense.AllowNull = (DefaultableBoolean)1;
		this.CmbMonthHistoryFohExpense.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMonthHistoryFohExpense.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMonthHistoryFohExpense.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMonthHistoryFohExpense).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMonthHistoryFohExpense).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMonthHistoryFohExpense.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMonthHistoryFohExpense).Location = new System.Drawing.Point(423, 12);
		((System.Windows.Forms.Control)(object)this.CmbMonthHistoryFohExpense).Name = "CmbMonthHistoryFohExpense";
		((System.Windows.Forms.Control)(object)this.CmbMonthHistoryFohExpense).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbMonthHistoryFohExpense).TabIndex = 2;
		this.ToDateHistoryFohExpense.CustomFormat = "dd-MMM-yy";
		this.ToDateHistoryFohExpense.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistoryFohExpense.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistoryFohExpense.Location = new System.Drawing.Point(245, 14);
		this.ToDateHistoryFohExpense.Name = "ToDateHistoryFohExpense";
		this.ToDateHistoryFohExpense.ShowCheckBox = true;
		this.ToDateHistoryFohExpense.Size = new System.Drawing.Size(130, 22);
		this.ToDateHistoryFohExpense.TabIndex = 1;
		this.FromDateHistoryFohExpense.CustomFormat = "dd-MMM-yy";
		this.FromDateHistoryFohExpense.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistoryFohExpense.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistoryFohExpense.Location = new System.Drawing.Point(62, 14);
		this.FromDateHistoryFohExpense.Name = "FromDateHistoryFohExpense";
		this.FromDateHistoryFohExpense.ShowCheckBox = true;
		this.FromDateHistoryFohExpense.Size = new System.Drawing.Size(130, 22);
		this.FromDateHistoryFohExpense.TabIndex = 0;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(783, 16);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 6;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.btnShowHistoryFohExpense.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistoryFohExpense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistoryFohExpense.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnShowHistoryFohExpense.ForeColor = System.Drawing.Color.White;
		this.btnShowHistoryFohExpense.Location = new System.Drawing.Point(544, 12);
		this.btnShowHistoryFohExpense.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistoryFohExpense.Name = "btnShowHistoryFohExpense";
		this.btnShowHistoryFohExpense.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistoryFohExpense.TabIndex = 3;
		this.btnShowHistoryFohExpense.Text = "Show";
		this.btnShowHistoryFohExpense.UseVisualStyleBackColor = false;
		this.btnShowHistoryFohExpense.Click += new System.EventHandler(btnShowHistoryFohExpense_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(688, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 5;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(379, 18);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(43, 15);
		this.label67.TabIndex = 87;
		this.label67.Text = "Month";
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Checked = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(607, 16);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 4;
		this.rdentrydate.TabStop = true;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(195, 18);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(2, 18);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label52);
		this.panel17.Controls.Add(this.toolStrip2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 3);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(936, 53);
		this.panel17.TabIndex = 351;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(5, 30);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(178, 19);
		this.label52.TabIndex = 562;
		this.label52.Text = "FOH Expense History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistoryFohExpense, this.btnRefreshHistoryFohExpense });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip2.Size = new System.Drawing.Size(936, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistoryFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistoryFohExpense.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistoryFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistoryFohExpense.Name = "btnResetHistoryFohExpense";
		this.btnResetHistoryFohExpense.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistoryFohExpense.Text = "&New";
		this.btnResetHistoryFohExpense.Click += new System.EventHandler(btnResetHistoryFohExpense_Click);
		this.btnRefreshHistoryFohExpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistoryFohExpense.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistoryFohExpense.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistoryFohExpense.Name = "btnRefreshHistoryFohExpense";
		this.btnRefreshHistoryFohExpense.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistoryFohExpense.Text = "Refresh";
		this.btnRefreshHistoryFohExpense.Click += new System.EventHandler(btnRefreshHistoryFohExpense_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(885, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.ctrlGrdBar1.MyGrid = this.grdHistoryFohExpense;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(48, 23);
		this.ctrlGrdBar1.TabIndex = 8;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(964, 561);
		base.Controls.Add(this.tabMain);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmProductionFohReportCreate";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmProductionFohReportCreate_KeyDown);
		this.tabMain.ResumeLayout(false);
		this.tabFohAllocation.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdJobPlantFohAllocation).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPlantFohAllocation).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabFohExpense.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabFormFohExpense.ResumeLayout(false);
		this.tabFormFohExpense.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFohExpense).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPlantWiseSummaryFohExpense).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMonthDateFohExpense).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMethodTypeFohExpense).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountFohExpense).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistoryFohExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryFohExpense).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistoryFohExpense).EndInit();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMonthHistoryFohExpense).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
