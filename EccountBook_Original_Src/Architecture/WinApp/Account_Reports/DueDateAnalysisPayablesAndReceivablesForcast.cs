using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class DueDateAnalysisPayablesAndReceivablesForcast : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtFilteredData = new DataTable();

	private DateTime startDate = DateTime.Now;

	private DateTime endDate = DateTime.Now;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Label lblComparison;

	private Panel panel5;

	private Label label27;

	private Label label33;

	private Button btnshow;

	private Panel panel12;

	private Label txtNotYetPayables;

	private Label label13;

	private Panel panel11;

	private Label txtOverDueReceivables;

	private Label label11;

	private Panel panel10;

	private Label txtNetReceivable;

	private Label label9;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Label label7;

	private FlowLayoutPanel ItemWiseflowlayout;

	private Panel panel4;

	private Label label8;

	private TabPage tabPage4;

	private Panel panel9;

	public DateTimePicker todate;

	public DateTimePicker fromdate;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewSummary;

	private ToolStripButton btnPrintSumm;

	private ToolStripButton btnShortcut;

	private Panel panel6;

	internal GridEX grdDueWise;

	private Label label1;

	private TextBox txtIntervalDays;

	private Panel panel13;

	private Label txtOverDuePayables;

	private Label label10;

	private Panel panel8;

	private Label txtNetPayables;

	private Label label5;

	private Panel panel7;

	private Label txtNotYetReceivables;

	private Label label3;

	public DueDateAnalysisPayablesAndReceivablesForcast(UserAccount Puser)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void DueDateAnalysisPayablesAndReceivablesForcast_Load(object sender, EventArgs e)
	{
		try
		{
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 500);
			panel1.AutoScroll = true;
			fromdate.Value = DateTime.Now;
			fromdate.Enabled = false;
			txtIntervalDays.Text = "8";
			todate.Checked = false;
			fromdate.Focus();
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
			FillData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void FillData(int SortNo = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0bc4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.FromDate = fromdate.Value;
			if (todate.Checked)
			{
				obj.ToDate = todate.Value;
			}
			obj.SortNo = SortNo;
			obj.IntervalDays = Conversion.ToInt((object)txtIntervalDays.Text);
			DataSet ds = GeneralReprots.DueDateAnalysisPayablesAndReceivablesForcast(obj);
			dtFilteredData = ds.Tables[0];
			DataTable dtDueWise = ds.Tables[1];
			if (dtFilteredData.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("DueDate", typeof(DateTime));
				dtcol.Columns.Add("SaleReceivable", typeof(decimal));
				dtcol.Columns.Add("ExportReceivable", typeof(decimal));
				dtcol.Columns.Add("TotalReceivables", typeof(decimal));
				dtcol.Columns.Add("RunningTotalReceivables", typeof(double));
				dtcol.Columns.Add("PurchasePayable", typeof(decimal));
				dtcol.Columns.Add("ImportPayable", typeof(decimal));
				dtcol.Columns.Add("TotalPayables", typeof(decimal));
				dtcol.Columns.Add("RunningTotalPayables", typeof(decimal));
				dtcol.Columns.Add("ReceivablesMinusPayables", typeof(decimal));
				dtcol.Columns.Add("RunningReceivables", typeof(double));
				dtcol.Rows.Clear();
				for (int j = 0; j < dtFilteredData.Rows.Count; j++)
				{
					dtcol.Rows.Add(dtFilteredData.Rows[j]["DueDate"], dtFilteredData.Rows[j]["SaleReceivable"], dtFilteredData.Rows[j]["ExportReceivable"], dtFilteredData.Rows[j]["TotalReceivables"], dtFilteredData.Rows[j]["RunningTotalReceivables"], 0.0 - Conversion.ToDouble(dtFilteredData.Rows[j]["PurchasePayable"]), 0.0 - Conversion.ToDouble(dtFilteredData.Rows[j]["ImportPayable"]), 0.0 - Conversion.ToDouble(dtFilteredData.Rows[j]["TotalPayables"]), 0.0 - Conversion.ToDouble(dtFilteredData.Rows[j]["RunningTotalPayables"]), dtFilteredData.Rows[j]["ReceivablesMinusPayables"], dtFilteredData.Rows[j]["RunningReceivables"]);
				}
				grd.DataSource = dtcol;
				grd.RetrieveStructure();
				GridSettings();
				ItemWiseflowlayout.Controls.Clear();
				DueDateAnalysisReceivableAndPayableCard[] CurrCards = new DueDateAnalysisReceivableAndPayableCard[dtcol.Rows.Count];
				for (int k = 0; k < CurrCards.Length; k++)
				{
					CurrCards[k] = new DueDateAnalysisReceivableAndPayableCard();
					CurrCards[k].Date = Conversion.ToDateTime(dtcol.Rows[k]["DueDate"]).ToShortDateString();
					CurrCards[k].SaleReceivables = Conversion.ToDouble(dtcol.Rows[k]["SaleReceivable"]).ToString("#,#;(#,#);0");
					CurrCards[k].ExportReceivables = Conversion.ToDouble(dtcol.Rows[k]["ExportReceivable"]).ToString("#,#;(#,#);0");
					CurrCards[k].TotalReceivables = Conversion.ToDouble(dtcol.Rows[k]["TotalReceivables"]).ToString("#,#;(#,#);0");
					CurrCards[k].TotalRunningReceivables = Conversion.ToDouble(dtcol.Rows[k]["RunningTotalReceivables"]).ToString("#,#;(#,#);0");
					CurrCards[k].PurchasePayables = Conversion.ToDouble(dtcol.Rows[k]["PurchasePayable"]).ToString("#,#;(#,#);0");
					CurrCards[k].ImportPayables = Conversion.ToDouble(dtcol.Rows[k]["ImportPayable"]).ToString("#,#;(#,#);0");
					CurrCards[k].TotalPayables = Conversion.ToDouble(dtcol.Rows[k]["TotalPayables"]).ToString("#,#;(#,#);0");
					CurrCards[k].TotalRunningPayables = Conversion.ToDouble(dtcol.Rows[k]["RunningTotalPayables"]).ToString("#,#;(#,#);0");
					CurrCards[k].ReceivablesMinusPayables = Conversion.ToDouble(dtcol.Rows[k]["ReceivablesMinusPayables"]).ToString("#,#;(#,#);0");
					CurrCards[k].RunningReceivables = Conversion.ToDouble(dtcol.Rows[k]["RunningReceivables"]).ToString("#,#;(#,#);0");
					ItemWiseflowlayout.Controls.Add(CurrCards[k]);
				}
				todate.Value = Conversion.ToDateTime(dtFilteredData.Rows[0]["MaxDueDate"]);
				decimal NetReceivables = Conversion.ToDecimal(dtFilteredData.Rows[0]["NetReceivables"]);
				decimal OverdueReceivables = Conversion.ToDecimal(dtFilteredData.Rows[0]["ReceivablesOverDue"]);
				decimal NotYetReceivables = -Conversion.ToDecimal(dtFilteredData.Rows[0]["ReceivablesNotYetDue"]);
				decimal NetPayables = -Conversion.ToDecimal(dtFilteredData.Rows[0]["NetPayables"]);
				decimal OverduePayables = -Conversion.ToDecimal(dtFilteredData.Rows[0]["PayablesOverDue"]);
				decimal NotYetPayables = -Conversion.ToDecimal(dtFilteredData.Rows[0]["PayablesNotYetDue"]);
				txtNetReceivable.Text = NetReceivables.ToString("#,#;(#,#);0");
				txtOverDueReceivables.Text = OverdueReceivables.ToString("#,#;(#,#);0");
				txtNotYetReceivables.Text = NotYetReceivables.ToString("#,#;(#,#);0");
				txtNetPayables.Text = NetPayables.ToString("#,#;(#,#);0");
				txtOverDuePayables.Text = OverduePayables.ToString("#,#;(#,#);0");
				txtNotYetPayables.Text = NotYetPayables.ToString("#,#;(#,#);0");
			}
			else
			{
				grd.DataSource = null;
				ItemWiseflowlayout.Controls.Clear();
				txtNetReceivable.Text = "0";
				txtOverDueReceivables.Text = "0";
				txtNotYetReceivables.Text = "0";
				txtNetPayables.Text = "0";
				txtOverDuePayables.Text = "0";
				txtNotYetPayables.Text = "0";
			}
			if (dtDueWise.Rows.Count > 0)
			{
				if (SortNo != 0)
				{
					return;
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("SortNo", typeof(int));
				dt.Columns.Add("Description", typeof(string));
				dt.Columns.Add("SaleReceivable", typeof(double));
				dt.Columns.Add("ExportReceivable", typeof(double));
				dt.Columns.Add("TotalReceivables", typeof(double));
				dt.Columns.Add("CumulativeReceivables", typeof(double));
				dt.Columns.Add("PurchasePayable", typeof(double));
				dt.Columns.Add("ImportPayable", typeof(double));
				dt.Columns.Add("TotalPayables", typeof(double));
				dt.Columns.Add("CumulativePayables", typeof(double));
				dt.Columns.Add("NetBalance", typeof(double));
				dt.Columns.Add("CumulativeBalance", typeof(double));
				for (int i = 0; i < dtDueWise.Rows.Count; i++)
				{
					dt.Rows.Add(dtDueWise.Rows[i]["SortNo"], dtDueWise.Rows[i]["Description"], dtDueWise.Rows[i]["SaleReceivable"], dtDueWise.Rows[i]["ExportReceivable"], dtDueWise.Rows[i]["TotalReceivables"], dtDueWise.Rows[i]["ComulativeReceivables"], 0.0 - Conversion.ToDouble(dtDueWise.Rows[i]["PurchasePayable"]), 0.0 - Conversion.ToDouble(dtDueWise.Rows[i]["ImportPayable"]), 0.0 - Conversion.ToDouble(dtDueWise.Rows[i]["TotalPayables"]), 0.0 - Conversion.ToDouble(dtDueWise.Rows[i]["ComulativePayables"]), dtDueWise.Rows[i]["NetBalance"], dtDueWise.Rows[i]["ComulativeBalance"]);
				}
				grdDueWise.DataSource = dt;
				grdDueWise.RetrieveStructure();
				grdDueWise.TotalRow = (InheritableBoolean)1;
				grdDueWise.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)grdDueWise.RootTable.Columns)
				{
					item.HeaderAlignment = (TextAlignment)2;
				}
				grdDueWise.FilterMode = (FilterMode)0;
				grdDueWise.RootTable.Columns["SortNo"].Visible = false;
				grdDueWise.RootTable.Columns["Description"].Width = 325;
				grdDueWise.RootTable.Columns["SaleReceivable"].Width = 110;
				grdDueWise.RootTable.Columns["ExportReceivable"].Width = 110;
				grdDueWise.RootTable.Columns["CumulativeReceivables"].Width = 110;
				grdDueWise.RootTable.Columns["PurchasePayable"].Width = 110;
				grdDueWise.RootTable.Columns["ImportPayable"].Width = 110;
				grdDueWise.RootTable.Columns["TotalPayables"].Width = 110;
				grdDueWise.RootTable.Columns["CumulativePayables"].Width = 110;
				grdDueWise.RootTable.Columns["NetBalance"].Width = 110;
				grdDueWise.RootTable.Columns["CumulativeBalance"].Width = 130;
				grdDueWise.RootTable.Columns["SaleReceivable"].AggregateFunction = (AggregateFunction)2;
				grdDueWise.RootTable.Columns["SaleReceivable"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["SaleReceivable"].FormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["SaleReceivable"].TotalFormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["ExportReceivable"].AggregateFunction = (AggregateFunction)2;
				grdDueWise.RootTable.Columns["ExportReceivable"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["ExportReceivable"].FormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["ExportReceivable"].TotalFormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["TotalReceivables"].AggregateFunction = (AggregateFunction)2;
				grdDueWise.RootTable.Columns["TotalReceivables"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["TotalReceivables"].FormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["TotalReceivables"].TotalFormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["PurchasePayable"].AggregateFunction = (AggregateFunction)2;
				grdDueWise.RootTable.Columns["PurchasePayable"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["PurchasePayable"].FormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["PurchasePayable"].TotalFormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["ImportPayable"].AggregateFunction = (AggregateFunction)2;
				grdDueWise.RootTable.Columns["ImportPayable"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["ImportPayable"].FormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["ImportPayable"].TotalFormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["TotalPayables"].AggregateFunction = (AggregateFunction)2;
				grdDueWise.RootTable.Columns["TotalPayables"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["TotalPayables"].FormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["TotalPayables"].TotalFormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["NetBalance"].AggregateFunction = (AggregateFunction)2;
				grdDueWise.RootTable.Columns["NetBalance"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["NetBalance"].FormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["NetBalance"].TotalFormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["CumulativeReceivables"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["CumulativeReceivables"].FormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["CumulativePayables"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["CumulativePayables"].FormatString = "#,#;(#,#);0";
				grdDueWise.RootTable.Columns["CumulativeBalance"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["CumulativeBalance"].FormatString = "#,#;(#,#);0";
			}
			else
			{
				grdDueWise.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		try
		{
			grd.AllowDelete = (InheritableBoolean)2;
			grd.AllowEdit = (InheritableBoolean)2;
			grd.RootTable.HeaderLines = 3;
			grd.RootTable.Columns["RunningTotalReceivables"].Caption = "Cumulative Receivables";
			grd.RootTable.Columns["RunningTotalPayables"].Caption = "Cumulative Payables";
			grd.RootTable.Columns["ReceivablesMinusPayables"].Caption = "Net Balance (Receivables - Payables)";
			grd.RootTable.Columns["RunningReceivables"].Caption = "Cumulative Balance";
			grd.RootTable.Columns["RunningTotalReceivables"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RunningTotalReceivables"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["RunningReceivables"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RunningReceivables"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["RunningTotalPayables"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RunningTotalPayables"].FormatString = clsGlobalVariables.stringFormatboth;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					column.Width = Constants.InventoryConstants.AmountForReportConstant + 20;
					ConfigureNumericalColumn(column);
				}
				else if (column.Type == typeof(DateTime) || column.Type == typeof(TimeSpan))
				{
					column.Width = Constants.InventoryConstants.DateConstant;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigureNumericalColumn(GridEXColumn column)
	{
		if (column.Key != "RunningReceivables" && column.Key != "RunningTotalReceivables" && column.Key != "RunningTotalPayables")
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.stringFormatboth;
			column.TotalFormatString = clsGlobalVariables.stringFormatboth;
		}
	}

	private void grdItemWise_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdItemWise_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdItemWise_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			GridEXColumn RateCut = grd.RootTable.Columns["RunningReceivables"];
			if (RateCut != null)
			{
				GridEXCell CutCell = e.Row.Cells[((JanusCollectionElement)RateCut).Index];
				if (Conversion.ToDouble(CutCell.Value) < 0.0)
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

	private void DueDateAnalysisPayablesAndReceivablesForcast_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnNewSummary_Click(object sender, EventArgs e)
	{
		try
		{
			fromdate.Value = DateTime.Now.AddDays(-30.0);
			todate.Value = DateTime.Now;
			fromdate.Focus();
			grd.DataSource = null;
			txtNetReceivable.Text = "";
			txtOverDueReceivables.Text = "";
			txtNotYetPayables.Text = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintSumm_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtFilteredData == null || dtFilteredData.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtFilteredData, "840_DueDateAnalysisPayablesAndReceivablesForcast.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDateAnalysisPayablesAndReceivablesForcast_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNewSummary_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				fromdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				fromdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus On FromDate ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On FromDate");
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

	private void btnShortcut_Click(object sender, EventArgs e)
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grd.CurrentColumn != null && e.Control)
			{
				_ = e.KeyCode;
				_ = 32;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDueWise_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			if (grdDueWise.RowCount <= 0)
			{
				return;
			}
			GridEXColumn RateCut = grdDueWise.RootTable.Columns["CumulativeBalance"];
			if (RateCut != null)
			{
				GridEXCell CutCell = e.Row.Cells[((JanusCollectionElement)RateCut).Index];
				if (Conversion.ToDouble(CutCell.Value) < 0.0)
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

	private void txtIntervalDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grdDueWise_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDueWise.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int SortNo = Conversion.ToInt(r.Cells["SortNo"].Value);
				FillData(SortNo);
				r.CheckState = (RowCheckState)0;
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
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Expected O, but got Unknown
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Expected O, but got Unknown
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_0b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b86: Expected O, but got Unknown
		GridEXLayout grdDueWise_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.label8 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ItemWiseflowlayout = new System.Windows.Forms.FlowLayoutPanel();
		this.label7 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdDueWise = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.panel12 = new System.Windows.Forms.Panel();
		this.txtNotYetPayables = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.panel11 = new System.Windows.Forms.Panel();
		this.txtOverDueReceivables = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.txtNetReceivable = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSumm = new System.Windows.Forms.ToolStripButton();
		this.btnShortcut = new System.Windows.Forms.ToolStripButton();
		this.lblComparison = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.txtIntervalDays = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.txtNotYetReceivables = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.txtNetPayables = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.txtOverDuePayables = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.tabPage2.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDueWise).BeginInit();
		this.panel6.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel13.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1734, 730);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 283);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1734, 447);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1726, 418);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Tabular View";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1720, 412);
		this.panel4.TabIndex = 1;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 22);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1720, 390);
		this.panel9.TabIndex = 49;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1720, 390);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.FormattingRow += new RowLoadEventHandler(grdItemWise_FormattingRow);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grdItemWise_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grdItemWise_LinkClicked);
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Dock = System.Windows.Forms.DockStyle.Top;
		this.label8.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(0, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(1720, 22);
		this.label8.TabIndex = 1;
		this.label8.Text = "Filtered Records";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1058, 292);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Card View";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.ItemWiseflowlayout);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1052, 286);
		this.panel2.TabIndex = 0;
		this.ItemWiseflowlayout.AutoScroll = true;
		this.ItemWiseflowlayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ItemWiseflowlayout.Location = new System.Drawing.Point(0, 23);
		this.ItemWiseflowlayout.Name = "ItemWiseflowlayout";
		this.ItemWiseflowlayout.Size = new System.Drawing.Size(1052, 263);
		this.ItemWiseflowlayout.TabIndex = 1;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(1052, 23);
		this.label7.TabIndex = 0;
		this.label7.Text = "Filtered Records";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdDueWise);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 53);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1734, 230);
		this.panel5.TabIndex = 0;
		this.grdDueWise.AllowEdit = (InheritableBoolean)2;
		this.grdDueWise.AlternatingColors = true;
		grdDueWise_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDueWise.DesignTimeLayout = grdDueWise_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDueWise).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDueWise.DynamicFiltering = true;
		this.grdDueWise.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDueWise).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDueWise.GroupByBoxVisible = false;
		this.grdDueWise.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdDueWise).Location = new System.Drawing.Point(0, 55);
		((System.Windows.Forms.Control)(object)this.grdDueWise).Name = "grdDueWise";
		((ScrollableBase)this.grdDueWise).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDueWise).Size = new System.Drawing.Size(1734, 175);
		((System.Windows.Forms.Control)(object)this.grdDueWise).TabIndex = 1;
		this.grdDueWise.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDueWise).VisualStyle = (VisualStyle)4;
		this.grdDueWise.FormattingRow += new RowLoadEventHandler(grdDueWise_FormattingRow);
		((System.Windows.Forms.Control)(object)this.grdDueWise).DoubleClick += new System.EventHandler(grdDueWise_DoubleClick);
		this.panel6.Controls.Add(this.panel13);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.label33);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Controls.Add(this.label27);
		this.panel6.Controls.Add(this.txtIntervalDays);
		this.panel6.Controls.Add(this.fromdate);
		this.panel6.Controls.Add(this.panel12);
		this.panel6.Controls.Add(this.todate);
		this.panel6.Controls.Add(this.panel11);
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Controls.Add(this.btnshow);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1734, 55);
		this.panel6.TabIndex = 167;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(127, 6);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 106;
		this.label33.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(7, 6);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 107;
		this.label27.Text = "From Date";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(7, 25);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(119, 23);
		this.fromdate.TabIndex = 0;
		this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel12.Controls.Add(this.txtNotYetPayables);
		this.panel12.Controls.Add(this.label13);
		this.panel12.Location = new System.Drawing.Point(1109, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(143, 50);
		this.panel12.TabIndex = 3;
		this.panel12.TabStop = true;
		this.txtNotYetPayables.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNotYetPayables.Location = new System.Drawing.Point(-1, 22);
		this.txtNotYetPayables.Name = "txtNotYetPayables";
		this.txtNotYetPayables.Size = new System.Drawing.Size(143, 23);
		this.txtNotYetPayables.TabIndex = 2;
		this.txtNotYetPayables.Text = "0";
		this.txtNotYetPayables.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(-1, -1);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(143, 23);
		this.label13.TabIndex = 1;
		this.label13.Text = "Not Yet Due";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Checked = false;
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(127, 25);
		this.todate.Name = "todate";
		this.todate.ShowCheckBox = true;
		this.todate.Size = new System.Drawing.Size(136, 23);
		this.todate.TabIndex = 2;
		this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel11.Controls.Add(this.txtOverDueReceivables);
		this.panel11.Controls.Add(this.label11);
		this.panel11.Location = new System.Drawing.Point(529, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(143, 50);
		this.panel11.TabIndex = 3;
		this.panel11.TabStop = true;
		this.txtOverDueReceivables.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverDueReceivables.Location = new System.Drawing.Point(-1, 23);
		this.txtOverDueReceivables.Name = "txtOverDueReceivables";
		this.txtOverDueReceivables.Size = new System.Drawing.Size(143, 23);
		this.txtOverDueReceivables.TabIndex = 2;
		this.txtOverDueReceivables.Text = "0";
		this.txtOverDueReceivables.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label11.BackColor = System.Drawing.Color.Teal;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(-1, -1);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(143, 23);
		this.label11.TabIndex = 1;
		this.label11.Text = "Over Due";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel10.Controls.Add(this.label9);
		this.panel10.Controls.Add(this.txtNetReceivable);
		this.panel10.Location = new System.Drawing.Point(384, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(143, 50);
		this.panel10.TabIndex = 0;
		this.panel10.TabStop = true;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(-1, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(143, 23);
		this.label9.TabIndex = 1;
		this.label9.Text = "Net Receivables";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtNetReceivable.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetReceivable.Location = new System.Drawing.Point(2, 23);
		this.txtNetReceivable.Name = "txtNetReceivable";
		this.txtNetReceivable.Size = new System.Drawing.Size(140, 23);
		this.txtNetReceivable.TabIndex = 2;
		this.txtNetReceivable.Text = "0";
		this.txtNetReceivable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(321, 24);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Controls.Add(this.lblComparison);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1734, 53);
		this.panel3.TabIndex = 1;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNewSummary, this.btnPrintSumm, this.btnShortcut });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1734, 27);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewSummary.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewSummary.Name = "BtnNewSummary";
		this.BtnNewSummary.Size = new System.Drawing.Size(59, 24);
		this.BtnNewSummary.Text = "&New";
		this.BtnNewSummary.Click += new System.EventHandler(BtnNewSummary_Click);
		this.btnPrintSumm.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSumm.Name = "btnPrintSumm";
		this.btnPrintSumm.Size = new System.Drawing.Size(92, 24);
		this.btnPrintSumm.Text = "840-Print";
		this.btnPrintSumm.Click += new System.EventHandler(btnPrintSumm_Click);
		this.btnShortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcut.Name = "btnShortcut";
		this.btnShortcut.Size = new System.Drawing.Size(126, 24);
		this.btnShortcut.Text = "ShortCut Keys";
		this.btnShortcut.Click += new System.EventHandler(btnShortcut_Click);
		this.lblComparison.AutoSize = true;
		this.lblComparison.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblComparison.ForeColor = System.Drawing.Color.White;
		this.lblComparison.Location = new System.Drawing.Point(3, 31);
		this.lblComparison.Name = "lblComparison";
		this.lblComparison.Size = new System.Drawing.Size(448, 19);
		this.lblComparison.TabIndex = 1;
		this.lblComparison.Text = "Due Date Analysis Payables And Receivables Forecast";
		this.tabPage4.Location = new System.Drawing.Point(0, 0);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(200, 100);
		this.tabPage4.TabIndex = 0;
		this.tabPage4.Text = "Supplier Wise Card View";
		this.txtIntervalDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIntervalDays.Location = new System.Drawing.Point(264, 25);
		this.txtIntervalDays.Margin = new System.Windows.Forms.Padding(2);
		this.txtIntervalDays.Name = "txtIntervalDays";
		this.txtIntervalDays.Size = new System.Drawing.Size(56, 23);
		this.txtIntervalDays.TabIndex = 1;
		this.txtIntervalDays.Text = "0";
		this.txtIntervalDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtIntervalDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtIntervalDays_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(264, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(47, 15);
		this.label1.TabIndex = 166;
		this.label1.Text = "Interval";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1678, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar1.TabIndex = 48;
		this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel7.Controls.Add(this.txtNotYetReceivables);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Location = new System.Drawing.Point(674, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(143, 50);
		this.panel7.TabIndex = 4;
		this.panel7.TabStop = true;
		this.txtNotYetReceivables.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNotYetReceivables.Location = new System.Drawing.Point(-1, 23);
		this.txtNotYetReceivables.Name = "txtNotYetReceivables";
		this.txtNotYetReceivables.Size = new System.Drawing.Size(143, 23);
		this.txtNotYetReceivables.TabIndex = 2;
		this.txtNotYetReceivables.Text = "0";
		this.txtNotYetReceivables.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(-1, -1);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(143, 23);
		this.label3.TabIndex = 1;
		this.label3.Text = "Not Yet Due";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel8.Controls.Add(this.txtNetPayables);
		this.panel8.Controls.Add(this.label5);
		this.panel8.Location = new System.Drawing.Point(819, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(143, 50);
		this.panel8.TabIndex = 167;
		this.panel8.TabStop = true;
		this.txtNetPayables.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetPayables.Location = new System.Drawing.Point(-1, 22);
		this.txtNetPayables.Name = "txtNetPayables";
		this.txtNetPayables.Size = new System.Drawing.Size(143, 23);
		this.txtNetPayables.TabIndex = 2;
		this.txtNetPayables.Text = "0";
		this.txtNetPayables.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(-1, -1);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(143, 23);
		this.label5.TabIndex = 1;
		this.label5.Text = "Net Payables";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel13.Controls.Add(this.txtOverDuePayables);
		this.panel13.Controls.Add(this.label10);
		this.panel13.Location = new System.Drawing.Point(964, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(143, 50);
		this.panel13.TabIndex = 168;
		this.panel13.TabStop = true;
		this.txtOverDuePayables.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverDuePayables.Location = new System.Drawing.Point(-1, 22);
		this.txtOverDuePayables.Name = "txtOverDuePayables";
		this.txtOverDuePayables.Size = new System.Drawing.Size(143, 23);
		this.txtOverDuePayables.TabIndex = 2;
		this.txtOverDuePayables.Text = "0";
		this.txtOverDuePayables.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(-1, -1);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(143, 23);
		this.label10.TabIndex = 1;
		this.label10.Text = "Over Due";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1734, 730);
		base.Controls.Add(this.panel1);
		base.Name = "DueDateAnalysisPayablesAndReceivablesForcast";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DueDateAnalysisPayablesAndReceivablesForcast_FormClosing);
		base.Load += new System.EventHandler(DueDateAnalysisPayablesAndReceivablesForcast_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DueDateAnalysisPayablesAndReceivablesForcast_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.tabPage2.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDueWise).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
