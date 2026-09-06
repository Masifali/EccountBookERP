using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.ExportReports;

public class frmExportContractSchedulePeriodic : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtByDate = new DataTable();

	private DataTable dtByMonth = new DataTable();

	private DataTable dtByPorts = new DataTable();

	private DataTable dtByItem = new DataTable();

	private DataTable dtByParty = new DataTable();

	private DataTable dtDateCombo = new DataTable();

	private DataTable dtDetailByDocType = new DataTable();

	private DataTable dtDetailByContractor = new DataTable();

	private DateTime LastScheduleLoadingDate = DateTime.Now;

	private IContainer components;

	private Panel PanelDetail;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrint;

	private Panel panel3;

	private Button btnShowByMonths;

	internal UltraCombo cmbFromMonth;

	private Label label10;

	private Label label2;

	private DateTimePicker txtToDate;

	private Label label1;

	private DateTimePicker txtDateFrom;

	internal UltraCombo CmbToMonth;

	private Label label3;

	private Panel panel16;

	private Panel panel18;

	private Label label6;

	internal GridEX grdByPorts;

	private TextBox tztAddDays;

	private Button BtnShowbyDates;

	private Label label12;

	private TextBox txtAddMonths;

	private Label label9;

	private Panel panel1;

	private Panel panel11;

	internal GridEX grdByParty;

	private Panel panel12;

	private Label label8;

	private Panel panel6;

	internal GridEX grdByDate;

	private Panel panel7;

	private Label label5;

	private Label label4;

	private Label label11;

	private ToolStripButton btnShortCutKeys;

	private Panel panel9;

	internal GridEX grdByItem;

	private Panel panel10;

	private Label label7;

	internal GridEX grdByMonths;

	private Panel panel2;

	private Panel panel4;

	private Panel panel5;

	public frmExportContractSchedulePeriodic(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmEvaulationDetailSalesReports_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1265, 500);
			dtDateCombo.Columns.Add("Id");
			dtDateCombo.Columns.Add("Month");
			MonthFill();
			DateTime FromMonth = DateTime.Now;
			DateTime ToMonth = DateTime.Now;
			LastScheduleLoadingDate = ExImLcOrderShipmentSchedule.GetLatestAttentiveLoadDate(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (LastScheduleLoadingDate > DateTime.Now)
			{
				tztAddDays.Text = (LastScheduleLoadingDate - DateTime.Now).Days.ToString();
			}
			else
			{
				txtDateFrom.Value = LastScheduleLoadingDate;
				FromMonth = txtDateFrom.Value;
				txtDateFrom.Value = LastScheduleLoadingDate.AddDays(-7.0);
			}
			txtToDate.Value = LastScheduleLoadingDate;
			ToMonth = LastScheduleLoadingDate;
			dtByDate.Columns.Add("Date", typeof(string));
			dtByDate.Columns.Add("Fcl", typeof(decimal));
			dtByDate.Columns.Add("MTons", typeof(decimal));
			dtByMonth.Columns.Add("Month", typeof(string));
			dtByMonth.Columns.Add("Fcl", typeof(decimal));
			dtByMonth.Columns.Add("MTons", typeof(decimal));
			dtByPorts.Columns.Add("Port", typeof(string));
			dtByPorts.Columns.Add("Fcl", typeof(decimal));
			dtByPorts.Columns.Add("MTons", typeof(decimal));
			dtByItem.Columns.Add("Item", typeof(string));
			dtByItem.Columns.Add("Fcl", typeof(decimal));
			dtByItem.Columns.Add("MTons", typeof(decimal));
			dtByParty.Columns.Add("Customer", typeof(string));
			dtByParty.Columns.Add("Fcl", typeof(decimal));
			dtByParty.Columns.Add("MTons", typeof(decimal));
			if (dtDateCombo.Rows.Count > 0)
			{
				DateTime from = FromMonth;
				string fromMonthValue = from.ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + fromMonthValue + "'").Length != 0)
				{
					((Control)(object)cmbFromMonth).Text = fromMonthValue;
				}
				DateTime to = ToMonth;
				string toMonthValue = to.ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + toMonthValue + "'").Length != 0)
				{
					((Control)(object)CmbToMonth).Text = toMonthValue;
				}
			}
			cmbFromMonth.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MonthFill()
	{
		try
		{
			dtDateCombo.Clear();
			DateTime start_Period = clsGlobalVariables.ActiveYr.Start_Period;
			DateTime financialEndDate = clsGlobalVariables.ActiveYr.End_Period;
			List<DateTime> monthsList = new List<DateTime>();
			DateTime date = start_Period;
			while (date <= financialEndDate)
			{
				monthsList.Add(date);
				date = date.AddMonths(1);
			}
			foreach (DateTime month in monthsList)
			{
				dtDateCombo.Rows.Add(month.ToString("MMM yyyy"), month.ToString("MMM yyyy"));
			}
			if (dtDateCombo.Rows.Count > 0)
			{
				DDL.BindDDL(dtDateCombo, cmbFromMonth, "Id", "Month", "Month", ZeroIndex: false);
				DDL.BindDDL(dtDateCombo, CmbToMonth, "Id", "Month", "Month", ZeroIndex: false);
				((UltraGridBase)cmbFromMonth).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbToMonth).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			ShowDataByDates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowDataByDates()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			DataSet DtSet = ExImLcOrderShipmentSchedule.ExportContractSchedulePeriodicA(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FromDate = Conversion.ToDateTime((object)txtDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value)
			});
			FillAllGrids(DtSet);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowByMonths_Click(object sender, EventArgs e)
	{
		try
		{
			ShowDataByMonths();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowDataByMonths()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			};
			DateTime FromDate = DateTime.ParseExact(((Control)(object)cmbFromMonth).Text, "MMM yyyy", CultureInfo.InvariantCulture);
			DateTime toDate = DateTime.ParseExact(((Control)(object)CmbToMonth).Text, "MMM yyyy", CultureInfo.InvariantCulture);
			DateTime lastDateOfMonth = new DateTime(day: DateTime.DaysInMonth(toDate.Year, toDate.Month), year: toDate.Year, month: toDate.Month);
			val.FromDate = Conversion.ToDateTime((object)FromDate);
			val.ToDate = Conversion.ToDateTime((object)lastDateOfMonth);
			DataSet DtSet = ExImLcOrderShipmentSchedule.ExportContractSchedulePeriodicA(val);
			FillAllGrids(DtSet);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private static int GetMonthNumber(string monthString)
	{
		if (DateTime.TryParseExact(monthString, "MMMMM", null, DateTimeStyles.None, out var date))
		{
			return date.Month;
		}
		return -1;
	}

	public void FillAllGrids(DataSet DtSet)
	{
		try
		{
			if (DtSet != null)
			{
				if (DtSet.Tables[0].Rows.Count > 0)
				{
					DataTable dt1 = DtSet.Tables[0];
					dtByDate.Rows.Clear();
					for (int m = 0; m < dt1.Rows.Count; m++)
					{
						dtByDate.Rows.Add(Conversion.ToDateTime(dt1.Rows[m]["WeekStart"]).ToString("dd-MMM-yy") + " to " + Conversion.ToDateTime(dt1.Rows[m]["WeekEnd"]).ToString("dd-MMM-yy"), dt1.Rows[m]["Fcl"], dt1.Rows[m]["MTon"]);
					}
					if (dtByDate.Rows.Count > 0)
					{
						grdByDate.DataSource = dtByDate;
						grdByDate.RetrieveStructure();
						GrdByDateSetting();
					}
					else
					{
						grdByDate.DataSource = null;
					}
				}
				else
				{
					grdByDate.DataSource = null;
				}
				if (DtSet.Tables[1].Rows.Count > 0)
				{
					DataTable dt2 = DtSet.Tables[1];
					dtByParty.Rows.Clear();
					for (int l = 0; l < dt2.Rows.Count; l++)
					{
						dtByParty.Rows.Add(dt2.Rows[l]["CompanyName"], dt2.Rows[l]["Fcl"], dt2.Rows[l]["MTon"]);
					}
					if (dtByParty.Rows.Count > 0)
					{
						grdByParty.DataSource = dtByParty;
						grdByParty.RetrieveStructure();
						GrdByPartySetting();
					}
					else
					{
						grdByParty.DataSource = null;
					}
				}
				else
				{
					grdByParty.DataSource = null;
				}
				if (DtSet.Tables[2].Rows.Count > 0)
				{
					DataTable dt3 = DtSet.Tables[2];
					dtByPorts.Rows.Clear();
					for (int k = 0; k < dt3.Rows.Count; k++)
					{
						dtByPorts.Rows.Add(dt3.Rows[k]["PortName"], dt3.Rows[k]["Fcl"], dt3.Rows[k]["MTon"]);
					}
					if (dtByPorts.Rows.Count > 0)
					{
						grdByPorts.DataSource = dtByPorts;
						grdByPorts.RetrieveStructure();
						GrdByPortsSetting();
					}
					else
					{
						grdByPorts.DataSource = null;
					}
				}
				else
				{
					grdByPorts.DataSource = null;
				}
				if (DtSet.Tables[3].Rows.Count > 0)
				{
					DataTable dt4 = DtSet.Tables[3];
					dtByItem.Rows.Clear();
					for (int j = 0; j < dt4.Rows.Count; j++)
					{
						dtByItem.Rows.Add(dt4.Rows[j]["ItemName"], dt4.Rows[j]["Fcl"], dt4.Rows[j]["MTons"]);
					}
					if (dtByItem.Rows.Count > 0)
					{
						grdByItem.DataSource = dtByItem;
						grdByItem.RetrieveStructure();
						GrdByItemSetting();
					}
					else
					{
						grdByItem.DataSource = null;
					}
				}
				else
				{
					grdByItem.DataSource = null;
				}
				if (DtSet.Tables[4].Rows.Count > 0)
				{
					DataTable dt5 = DtSet.Tables[4];
					dtByMonth.Rows.Clear();
					for (int i = 0; i < dt5.Rows.Count; i++)
					{
						dtByMonth.Rows.Add(dt5.Rows[i]["ScheduleMonth"], dt5.Rows[i]["Fcl"], dt5.Rows[i]["MTons"]);
					}
					if (dtByMonth.Rows.Count > 0)
					{
						grdByMonths.DataSource = dtByMonth;
						grdByMonths.RetrieveStructure();
						GrdByMonthsSetting();
					}
					else
					{
						grdByMonths.DataSource = null;
					}
				}
				else
				{
					grdByMonths.DataSource = null;
				}
			}
			else
			{
				grdByDate.DataSource = null;
				grdByParty.DataSource = null;
				grdByPorts.DataSource = null;
				grdByItem.DataSource = null;
				grdByMonths.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdByDateSetting()
	{
		try
		{
			grdByDate.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdByDate.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdByDate.RootTable.Columns["Fcl"].FormatString = "0,0";
			grdByDate.RootTable.Columns["Fcl"].TotalFormatString = "#,##0.##";
			grdByDate.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdByDate.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdByDate.RootTable.Columns["MTons"].FormatString = "0,0";
			grdByDate.RootTable.Columns["MTons"].TotalFormatString = "#,##0.##";
			grdByDate.ColumnAutoResize = true;
			grdByDate.RootTable.Columns["Date"].Width = 170;
			grdByDate.RootTable.Columns["MTons"].Width = 70;
			grdByDate.RootTable.Columns["Fcl"].Width = 50;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdByPartySetting()
	{
		try
		{
			grdByParty.ColumnAutoResize = false;
			grdByParty.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdByParty.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdByParty.RootTable.Columns["Fcl"].FormatString = "0,0";
			grdByParty.RootTable.Columns["Fcl"].TotalFormatString = "#,##0.##";
			grdByParty.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdByParty.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdByParty.RootTable.Columns["MTons"].FormatString = "0,0";
			grdByParty.RootTable.Columns["MTons"].TotalFormatString = "#,##0.##";
			grdByParty.RootTable.Columns["Customer"].Width = 155;
			grdByParty.RootTable.Columns["MTons"].Width = 70;
			grdByParty.RootTable.Columns["Fcl"].Width = 50;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdByPortsSetting()
	{
		try
		{
			grdByPorts.ColumnAutoResize = false;
			grdByPorts.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdByPorts.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdByPorts.RootTable.Columns["Fcl"].FormatString = "0,0";
			grdByPorts.RootTable.Columns["Fcl"].TotalFormatString = "#,##0.##";
			grdByPorts.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdByPorts.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdByPorts.RootTable.Columns["MTons"].FormatString = "0,0";
			grdByPorts.RootTable.Columns["MTons"].TotalFormatString = "#,##0.##";
			grdByPorts.RootTable.Columns["Port"].Width = 180;
			grdByPorts.RootTable.Columns["MTons"].Width = 70;
			grdByPorts.RootTable.Columns["Fcl"].Width = 50;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdByItemSetting()
	{
		try
		{
			grdByItem.ColumnAutoResize = false;
			grdByItem.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdByItem.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdByItem.RootTable.Columns["Fcl"].FormatString = "0,0";
			grdByItem.RootTable.Columns["Fcl"].TotalFormatString = "#,##0.##";
			grdByItem.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdByItem.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdByItem.RootTable.Columns["MTons"].FormatString = "0,0";
			grdByItem.RootTable.Columns["MTons"].TotalFormatString = "#,##0.##";
			grdByItem.RootTable.Columns["Item"].Width = 220;
			grdByItem.RootTable.Columns["MTons"].Width = 70;
			grdByItem.RootTable.Columns["Fcl"].Width = 50;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdByMonthsSetting()
	{
		try
		{
			grdByMonths.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdByMonths.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdByMonths.RootTable.Columns["Fcl"].FormatString = "0,0";
			grdByMonths.RootTable.Columns["Fcl"].TotalFormatString = "#,##0.##";
			grdByMonths.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdByMonths.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdByMonths.RootTable.Columns["MTons"].FormatString = "0,0";
			grdByMonths.RootTable.Columns["MTons"].TotalFormatString = "#,##0.##";
			grdByMonths.ColumnAutoResize = true;
			grdByMonths.RootTable.Columns["MTons"].Width = 70;
			grdByMonths.RootTable.Columns["Fcl"].Width = 50;
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
			DateTime FromMonth = DateTime.Now;
			DateTime ToMonth = DateTime.Now;
			if (LastScheduleLoadingDate > DateTime.Now)
			{
				tztAddDays.Text = (LastScheduleLoadingDate - DateTime.Now).Days.ToString();
			}
			else
			{
				txtDateFrom.Value = LastScheduleLoadingDate;
				FromMonth = txtDateFrom.Value;
				txtDateFrom.Value = LastScheduleLoadingDate.AddDays(-7.0);
			}
			txtToDate.Value = LastScheduleLoadingDate;
			ToMonth = LastScheduleLoadingDate;
			if (dtDateCombo.Rows.Count > 0)
			{
				DateTime from = FromMonth;
				string fromMonthValue = from.ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + fromMonthValue + "'").Length != 0)
				{
					((Control)(object)cmbFromMonth).Text = fromMonthValue;
				}
				DateTime to = ToMonth;
				string toMonthValue = to.ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + toMonthValue + "'").Length != 0)
				{
					((Control)(object)CmbToMonth).Text = toMonthValue;
				}
			}
			cmbFromMonth.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWagesDashboardReport_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Focus On From Date combo in First Filter Box");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo From Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On From Date combo in First Filter Box");
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

	private void frmExportContractSchedulePeriodic_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowByMonths_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbFromMonth.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdByMonths).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbFromMonth.Focus();
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

	private void txtAddMonths_KeyPress(object sender, KeyPressEventArgs e)
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

	private void tztAddDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void tztAddDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime ToDate = Conversion.ToDateTime((object)txtDateFrom.Value).AddDays(Conversion.ToInt((object)tztAddDays.Text));
			txtToDate.Value = ToDate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddMonths_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime toMonth = DateTime.ParseExact(((Control)(object)cmbFromMonth).Text, "MMM yyyy", CultureInfo.InvariantCulture).AddMonths(Conversion.ToInt((object)txtAddMonths.Text));
			List<DateTime> monthsList = new List<DateTime>();
			DateTime date = toMonth;
			while (date <= toMonth)
			{
				monthsList.Add(date);
				date = date.AddMonths(1);
			}
			string toMonthValue = monthsList[0].ToString("MMM yyyy");
			if (toMonthValue != null && toMonthValue != string.Empty)
			{
				if (dtDateCombo.Select("Month='" + toMonthValue + "'").Length != 0)
				{
					((Control)(object)CmbToMonth).Text = toMonthValue;
				}
				else
				{
					MessageBox.Show("Selected month does not exist in the List.");
				}
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.frmExportContractSchedulePeriodic));
		GridEXLayout grdByMonths_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdByParty_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		GridEXLayout grdByPorts_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdByItem_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdByDate_DesignTimeLayout = new GridEXLayout();
		this.PanelDetail = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.grdByMonths = new GridEX();
		this.grdByParty = new GridEX();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.cmbFromMonth = new UltraCombo();
		this.tztAddDays = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbToMonth = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtAddMonths = new System.Windows.Forms.TextBox();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.BtnShowbyDates = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.btnShowByMonths = new System.Windows.Forms.Button();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdByPorts = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdByItem = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdByDate = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.PanelDetail.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByMonths).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdByParty).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFromMonth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbToMonth).BeginInit();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByPorts).BeginInit();
		this.panel18.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByItem).BeginInit();
		this.panel10.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByDate).BeginInit();
		this.panel7.SuspendLayout();
		base.SuspendLayout();
		this.PanelDetail.BackColor = System.Drawing.Color.Teal;
		this.PanelDetail.Controls.Add(this.label11);
		this.PanelDetail.Controls.Add(this.toolStrip3);
		this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelDetail.Location = new System.Drawing.Point(0, 0);
		this.PanelDetail.Name = "PanelDetail";
		this.PanelDetail.Size = new System.Drawing.Size(1328, 55);
		this.PanelDetail.TabIndex = 0;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(5, 32);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(300, 19);
		this.label11.TabIndex = 0;
		this.label11.Text = "Export Contract Scheduling Reports";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnPrint, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1328, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKeys.Image");
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(101, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.grdByMonths.AllowEdit = (InheritableBoolean)2;
		this.grdByMonths.ColumnAutoResize = true;
		this.grdByMonths.DataSource = this.grdByParty.Layouts;
		grdByMonths_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByMonths.DesignTimeLayout = grdByMonths_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByMonths).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByMonths.DynamicFiltering = true;
		this.grdByMonths.FilterMode = (FilterMode)2;
		this.grdByMonths.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByMonths).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByMonths.GroupByBoxVisible = false;
		this.grdByMonths.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdByMonths).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdByMonths).Name = "grdByMonths";
		((ScrollableBase)this.grdByMonths).RecordNavigator = true;
		((ScrollableBase)this.grdByMonths).ScrollBars = (ScrollBars)3;
		((System.Windows.Forms.Control)(object)this.grdByMonths).Size = new System.Drawing.Size(235, 251);
		((System.Windows.Forms.Control)(object)this.grdByMonths).TabIndex = 167;
		this.grdByMonths.TotalRow = (InheritableBoolean)1;
		this.grdByMonths.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByMonths).VisualStyle = (VisualStyle)4;
		this.grdByParty.AllowEdit = (InheritableBoolean)2;
		grdByParty_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByParty.DesignTimeLayout = grdByParty_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByParty).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByParty.DynamicFiltering = true;
		this.grdByParty.FilterMode = (FilterMode)2;
		this.grdByParty.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByParty).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByParty.GroupByBoxVisible = false;
		this.grdByParty.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdByParty).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdByParty).Name = "grdByParty";
		((ScrollableBase)this.grdByParty).RecordNavigator = true;
		((ScrollableBase)this.grdByParty).ScrollBars = (ScrollBars)3;
		((System.Windows.Forms.Control)(object)this.grdByParty).Size = new System.Drawing.Size(277, 485);
		((System.Windows.Forms.Control)(object)this.grdByParty).TabIndex = 0;
		this.grdByParty.TotalRow = (InheritableBoolean)1;
		this.grdByParty.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByParty).VisualStyle = (VisualStyle)4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(149, 18);
		this.label4.TabIndex = 0;
		this.label4.Text = "By Schedule Month";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(235, 512);
		this.panel3.TabIndex = 1;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdByMonths);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 232);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(235, 280);
		this.panel2.TabIndex = 167;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(235, 29);
		this.panel4.TabIndex = 168;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFromMonth);
		this.panel5.Controls.Add(this.tztAddDays);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbToMonth);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add(this.txtAddMonths);
		this.panel5.Controls.Add(this.txtDateFrom);
		this.panel5.Controls.Add(this.BtnShowbyDates);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtToDate);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.btnShowByMonths);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(235, 232);
		this.panel5.TabIndex = 166;
		this.cmbFromMonth.AllowNull = (DefaultableBoolean)1;
		this.cmbFromMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFromMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFromMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFromMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbFromMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFromMonth).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFromMonth).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFromMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFromMonth).Location = new System.Drawing.Point(80, 4);
		((System.Windows.Forms.Control)(object)this.cmbFromMonth).Name = "cmbFromMonth";
		((System.Windows.Forms.Control)(object)this.cmbFromMonth).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.cmbFromMonth).TabIndex = 0;
		this.tztAddDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tztAddDays.Location = new System.Drawing.Point(80, 146);
		this.tztAddDays.Name = "tztAddDays";
		this.tztAddDays.Size = new System.Drawing.Size(150, 23);
		this.tztAddDays.TabIndex = 5;
		this.tztAddDays.TextChanged += new System.EventHandler(tztAddDays_TextChanged);
		this.tztAddDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tztAddDays_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(4, 37);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(73, 15);
		this.label9.TabIndex = 165;
		this.label9.Text = "Add Months";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(4, 125);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "From Date";
		this.CmbToMonth.AllowNull = (DefaultableBoolean)1;
		this.CmbToMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbToMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbToMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbToMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbToMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbToMonth).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbToMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbToMonth).Location = new System.Drawing.Point(80, 59);
		((System.Windows.Forms.Control)(object)this.CmbToMonth).Name = "CmbToMonth";
		((System.Windows.Forms.Control)(object)this.CmbToMonth).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbToMonth).TabIndex = 2;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(4, 150);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(58, 15);
		this.label12.TabIndex = 165;
		this.label12.Text = "Add Days";
		this.txtAddMonths.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddMonths.Location = new System.Drawing.Point(80, 33);
		this.txtAddMonths.Name = "txtAddMonths";
		this.txtAddMonths.Size = new System.Drawing.Size(150, 23);
		this.txtAddMonths.TabIndex = 1;
		this.txtAddMonths.TextChanged += new System.EventHandler(txtAddMonths_TextChanged);
		this.txtAddMonths.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddMonths_KeyPress);
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateFrom.Location = new System.Drawing.Point(80, 119);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(150, 23);
		this.txtDateFrom.TabIndex = 4;
		this.BtnShowbyDates.BackColor = System.Drawing.Color.Teal;
		this.BtnShowbyDates.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnShowbyDates.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowbyDates.ForeColor = System.Drawing.Color.White;
		this.BtnShowbyDates.Location = new System.Drawing.Point(165, 203);
		this.BtnShowbyDates.Name = "BtnShowbyDates";
		this.BtnShowbyDates.Size = new System.Drawing.Size(64, 26);
		this.BtnShowbyDates.TabIndex = 7;
		this.BtnShowbyDates.Text = "Show";
		this.BtnShowbyDates.UseVisualStyleBackColor = false;
		this.BtnShowbyDates.Click += new System.EventHandler(btnShow_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(4, 65);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 15);
		this.label3.TabIndex = 165;
		this.label3.Text = "To Month";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(80, 174);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(150, 23);
		this.txtToDate.TabIndex = 6;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(4, 10);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(74, 15);
		this.label10.TabIndex = 159;
		this.label10.Text = "From Month";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(4, 178);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 3;
		this.label2.Text = "To Date";
		this.btnShowByMonths.BackColor = System.Drawing.Color.Teal;
		this.btnShowByMonths.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnShowByMonths.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowByMonths.ForeColor = System.Drawing.Color.White;
		this.btnShowByMonths.Location = new System.Drawing.Point(165, 87);
		this.btnShowByMonths.Name = "btnShowByMonths";
		this.btnShowByMonths.Size = new System.Drawing.Size(64, 26);
		this.btnShowByMonths.TabIndex = 3;
		this.btnShowByMonths.Text = "Show";
		this.btnShowByMonths.UseVisualStyleBackColor = false;
		this.btnShowByMonths.Click += new System.EventHandler(btnShowByMonths_Click);
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdByPorts);
		this.panel16.Controls.Add(this.panel18);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(915, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(303, 512);
		this.panel16.TabIndex = 169;
		this.grdByPorts.AllowEdit = (InheritableBoolean)2;
		grdByPorts_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByPorts.DesignTimeLayout = grdByPorts_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByPorts).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByPorts.DynamicFiltering = true;
		this.grdByPorts.FilterMode = (FilterMode)2;
		this.grdByPorts.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByPorts).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByPorts.GroupByBoxVisible = false;
		this.grdByPorts.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdByPorts).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdByPorts).Name = "grdByPorts";
		((ScrollableBase)this.grdByPorts).RecordNavigator = true;
		((ScrollableBase)this.grdByPorts).ScrollBars = (ScrollBars)3;
		((System.Windows.Forms.Control)(object)this.grdByPorts).Size = new System.Drawing.Size(303, 485);
		((System.Windows.Forms.Control)(object)this.grdByPorts).TabIndex = 0;
		this.grdByPorts.TotalRow = (InheritableBoolean)1;
		this.grdByPorts.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByPorts).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label6);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(303, 27);
		this.panel18.TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 5);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(71, 18);
		this.label6.TabIndex = 0;
		this.label6.Text = "By Ports";
		this.panel1.Controls.Add(this.panel16);
		this.panel1.Controls.Add(this.panel9);
		this.panel1.Controls.Add(this.panel11);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(235, 55);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1093, 512);
		this.panel1.TabIndex = 2;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdByItem);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(577, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(338, 512);
		this.panel9.TabIndex = 173;
		this.grdByItem.AllowEdit = (InheritableBoolean)2;
		grdByItem_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByItem.DesignTimeLayout = grdByItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByItem).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByItem.DynamicFiltering = true;
		this.grdByItem.FilterMode = (FilterMode)2;
		this.grdByItem.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByItem).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByItem.GroupByBoxVisible = false;
		this.grdByItem.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdByItem).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdByItem).Name = "grdByItem";
		((ScrollableBase)this.grdByItem).RecordNavigator = true;
		((ScrollableBase)this.grdByItem).ScrollBars = (ScrollBars)3;
		((System.Windows.Forms.Control)(object)this.grdByItem).Size = new System.Drawing.Size(338, 485);
		((System.Windows.Forms.Control)(object)this.grdByItem).TabIndex = 0;
		this.grdByItem.TotalRow = (InheritableBoolean)1;
		this.grdByItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByItem).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(338, 27);
		this.panel10.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(3, 5);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(70, 18);
		this.label7.TabIndex = 0;
		this.label7.Text = " By Item";
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdByParty);
		this.panel11.Controls.Add(this.panel12);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(300, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(277, 512);
		this.panel11.TabIndex = 172;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label8);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(277, 27);
		this.panel12.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 5);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(76, 18);
		this.label8.TabIndex = 0;
		this.label8.Text = " By Party";
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdByDate);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(300, 512);
		this.panel6.TabIndex = 171;
		this.grdByDate.AllowEdit = (InheritableBoolean)2;
		grdByDate_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByDate.DesignTimeLayout = grdByDate_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByDate).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByDate.DynamicFiltering = true;
		this.grdByDate.FilterMode = (FilterMode)2;
		this.grdByDate.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByDate).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByDate.GroupByBoxVisible = false;
		this.grdByDate.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdByDate).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdByDate).Name = "grdByDate";
		((ScrollableBase)this.grdByDate).RecordNavigator = true;
		((ScrollableBase)this.grdByDate).ScrollBars = (ScrollBars)3;
		((System.Windows.Forms.Control)(object)this.grdByDate).Size = new System.Drawing.Size(300, 485);
		((System.Windows.Forms.Control)(object)this.grdByDate).TabIndex = 0;
		this.grdByDate.TotalRow = (InheritableBoolean)1;
		this.grdByDate.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByDate).VisualStyle = (VisualStyle)4;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(300, 27);
		this.panel7.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(2, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(142, 18);
		this.label5.TabIndex = 0;
		this.label5.Text = " By Schedule Date";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1328, 567);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.PanelDetail);
		base.KeyPreview = true;
		base.Name = "frmExportContractSchedulePeriodic";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmWagesDashboardReport_FormClosing);
		base.Load += new System.EventHandler(frmEvaulationDetailSalesReports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportContractSchedulePeriodic_KeyDown);
		this.PanelDetail.ResumeLayout(false);
		this.PanelDetail.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByMonths).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdByParty).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFromMonth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbToMonth).EndInit();
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByPorts).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByItem).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByDate).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		base.ResumeLayout(false);
	}
}
