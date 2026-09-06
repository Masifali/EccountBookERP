// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.HRM_Reports
{

public class HRM_Reports : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private DataTable dtGridRegister = new DataTable();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel3;

	internal UltraCombo cmbEmployee;

	private Label label1;

	private DateTimePicker txtDutyDate;

	private Label label8;

	internal UltraCombo cmbShift;

	private Label label7;

	internal UltraCombo cmbPartyLocation;

	private Label label6;

	internal UltraCombo cmbSection;

	internal UltraCombo cmbDepartment;

	private Label label3;

	private DateTimePicker txtDutyYear;

	private Label label2;

	private DateTimePicker txtDutyMonth;

	private Button btnShow;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnprint101;

	private ToolStripButton btnPrint1002;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Label label9;

	internal GridEX DatagridHistory;

	private Panel panel2;

	private Label label4;

	private Panel panel1;

	private GroupBox groupBox1;

	private Label label10;

	private Label label12;

	private Panel panel4;

	private Label label5;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNewRegister;

	private ToolStripButton print;

	private Panel panel5;

	private GroupBox groupBox2;

	private DateTimePicker txtFromDateRegister;

	private Button btnShowRegister;

	internal UltraCombo cmbEmployeeRegister;

	private DateTimePicker txtDateToRegister;

	private Label label11;

	private Label label13;

	private Label label14;

	internal UltraCombo cmbDepartmentRegister;

	private Label label27;

	internal GridEX grd;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar3;

	public HRM_Reports(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void HRM_Reports_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			txtFromDateRegister.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			Employee();
			Department();
			PartyLocation();
			Shift();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Employee()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (tabControl1.SelectedIndex == 0)
			{
				obj.DepartmentId = Conversion.ToInt(cmbDepartment.Value);
				obj.ShiftId = Conversion.ToInt(cmbShift.Value);
				obj.SectionId = Conversion.ToInt(cmbSection.Value);
				obj.PartyLocationId = Conversion.ToInt(cmbPartyLocation.Value);
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				obj.DepartmentId = Conversion.ToInt(cmbDepartmentRegister.Value);
			}
			DataTable dt = genEmployeeHistory.GetAllEmployeesActive(obj);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbEmployee, "EmployeeId", "EmployeeName", "Employee", false);
				DDL.BindDDLNew(dt, cmbEmployeeRegister, "EmployeeId", "EmployeeName", "Employee", false);
			}
			else if (tabControl1.SelectedIndex == 0)
			{
				((UltraGridBase)cmbEmployee).DataSource = null;
				((Control)(object)cmbEmployee).Text = string.Empty;
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				((UltraGridBase)cmbEmployeeRegister).DataSource = null;
				((Control)(object)cmbEmployeeRegister).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Department()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genDepartment.Getall(new genDepartment
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DepartmentName");
			dt.Clear();
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["DepartmentId"], lst.Rows[i]["DepartmentName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbDepartment, "Id", "DepartmentName", "Department Name", ZeroIndex: false);
				DDL.BindDDL(dt, cmbDepartmentRegister, "Id", "DepartmentName", "Department Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Section(int departmentId)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			EnumerableRowCollection<DataRow> filteredRows = from r in genSection.Getall(new genSection
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				}).AsEnumerable()
				where r.Field<int>("DepartmentId") == departmentId
				select r;
			if (filteredRows.Any())
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("SectionName", typeof(string));
				foreach (DataRow row in filteredRows)
				{
					dt.Rows.Add(row["SectionId"], row["SectionName"]);
				}
				DDL.BindDDL(dt, cmbSection, "Id", "SectionName", "Section Name", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)cmbSection).DataSource = null;
				((Control)(object)cmbSection).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyLocation()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genPartyLocation.Getall(new genPartyLocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("PartyLocationName");
			dt.Clear();
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["PartyLocationId"], lst.Rows[i]["PartyLocationName"]);
				}
				DDL.BindDDL(dt, cmbPartyLocation, "Id", "PartyLocationName", "Party Location", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Shift()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genShift.Getall(new genShift
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ShiftName");
			dt.Clear();
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["ShiftId"], lst.Rows[i]["ShiftName"]);
				}
				DDL.BindDDL(dt, cmbShift, "Id", "ShiftName", "Shift Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDepartment_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			Section(Conversion.ToInt(cmbDepartment.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void genEmployeeAttendence_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnNewRegister_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnPrintRegister_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					print_Click(null, null);
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocDate = Conversion.ToDateTime((object)txtDutyDate.Value);
			obj.GetMonth = Conversion.ToInt((object)txtDutyMonth.Value.Month);
			obj.GetYear = Conversion.ToInt((object)txtDutyYear.Value.Year);
			obj.EmployeeId = Conversion.ToInt(cmbEmployee.Value);
			obj.DepartmentId = Conversion.ToInt(cmbDepartment.Value);
			obj.SectionId = Conversion.ToInt(cmbSection.Value);
			obj.PartyLocationId = Conversion.ToInt(cmbPartyLocation.Value);
			obj.ShiftId = Conversion.ToInt(cmbShift.Value);
			dtGrid = HRM_Reports.EmployeeMonthlyAttendenceRpt(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Sr#", typeof(int));
				dt.Columns.Add("RM", typeof(int));
				dt.Columns.Add("RY", typeof(int));
				dt.Columns.Add("EmployeeAttendanceId", typeof(int));
				dt.Columns.Add("DutyRoasterDetailId", typeof(int));
				dt.Columns.Add("ATD_DATE", typeof(DateTime));
				dt.Columns.Add("CurrentDate", typeof(DateTime));
				dt.Columns.Add("RangeInDateTime", typeof(DateTime));
				dt.Columns.Add("RangeOutDateTime", typeof(DateTime));
				dt.Columns.Add("DAYNAME", typeof(string));
				dt.Columns.Add("EmployeeId", typeof(int));
				dt.Columns.Add("EmployeeNo", typeof(int));
				dt.Columns.Add("EmployeeName", typeof(string));
				dt.Columns.Add("Mobile1", typeof(string));
				dt.Columns.Add("CNIC", typeof(string));
				dt.Columns.Add("Email", typeof(string));
				dt.Columns.Add("DepartmentName", typeof(string));
				dt.Columns.Add("DesignationName", typeof(string));
				dt.Columns.Add("InEntryMode", typeof(string));
				dt.Columns.Add("OutEntryMode", typeof(string));
				dt.Columns.Add("InDateTime", typeof(DateTime));
				dt.Columns.Add("OutDateTime", typeof(DateTime));
				dt.Columns.Add("SystemOutDateTime", typeof(DateTime));
				dt.Columns.Add("ShiftTiming", typeof(string));
				dt.Columns.Add("InTime", typeof(string));
				dt.Columns.Add("OutTime", typeof(string));
				dt.Columns.Add("TimeDuration", typeof(string));
				dt.Columns.Add("Late", typeof(string));
				dt.Columns.Add("Early", typeof(string));
				dt.Columns.Add("Status", typeof(string));
				dt.Columns.Add("IsLateArrival", typeof(bool));
				dt.Columns.Add("SelectedMonth", typeof(string));
				dt.Columns.Add("Timing", typeof(string));
				dt.Columns.Add("Shift", typeof(string));
				dt.Columns.Add("IsGazetted", typeof(bool));
				dt.Columns.Add("IsOffDay", typeof(bool));
				dt.Columns.Add("DRIsOffDuty", typeof(bool));
				dt.Columns.Add("DRShiftId", typeof(int));
				dt.Columns.Add("GraceInDateTime", typeof(DateTime));
				dt.Columns.Add("GraceOutDateTime", typeof(DateTime));
				dt.Columns.Add("StartTime", typeof(DateTime));
				dt.Columns.Add("EndTime", typeof(DateTime));
				dt.Columns.Add("Decsription", typeof(string));
				dt.Columns.Add("Short", typeof(string));
				dt.Columns.Add("OT", typeof(string));
				dt.Columns.Add("ShortHours", typeof(double));
				dt.Columns.Add("ShortMinutes", typeof(double));
				dt.Columns.Add("OTHours", typeof(double));
				dt.Columns.Add("OTMinutes", typeof(double));
				dt.Columns.Add("EmployeeHours", typeof(double));
				dt.Columns.Add("EmployeeMinutes", typeof(double));
				dt.Columns.Add("TotalShortHour", typeof(double));
				dt.Columns.Add("TotalShortMinutes", typeof(double));
				dt.Columns.Add("TotalOTMinutes", typeof(double));
				dt.Columns.Add("CPLQuota", typeof(string));
				dt.Columns.Add("CPLEntryMode", typeof(string));
				dt.Columns.Add("TOTAL_HOUR", typeof(double));
				dt.Columns.Add("Actual_HOUR", typeof(double));
				dt.Columns.Add("EMPLOYEE_WORKINGHOUR", typeof(double));
				dt.Columns.Add("OverTime", typeof(double));
				dt.Columns.Add("LateMinutes", typeof(int));
				dt.Columns.Add("IsRestDay", typeof(bool));
				dt.Columns.Add("IsHoliday", typeof(bool));
				dt.Columns.Add("IsLeave", typeof(bool));
				dt.Columns.Add("IsLeaveApproved", typeof(bool));
				dt.Columns.Add("IsSuspended", typeof(bool));
				dt.Columns.Add("IsAbsent", typeof(bool));
				dt.Columns.Add("IsPresent", typeof(bool));
				dt.Columns.Add("IsArrivalShortLeave", typeof(bool));
				dt.Columns.Add("IsDepartureShortLeave", typeof(bool));
				dt.Columns.Add("IsArrivalHalfDay", typeof(bool));
				dt.Columns.Add("IsDepartureHalfDay", typeof(bool));
				dt.Columns.Add("IsEarlyDeparture", typeof(bool));
				dt.Columns.Add("IsCPL", typeof(bool));
				dt.Columns.Add("AttendanceStatus", typeof(string));
				dt.Columns.Add("BenefitPrefix", typeof(string));
				dt.Rows.Clear();
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrid.Rows[i]["Sr#"], dtGrid.Rows[i]["RM"], dtGrid.Rows[i]["RY"], dtGrid.Rows[i]["EmployeeAttendanceId"], dtGrid.Rows[i]["DutyRoasterDetailId"], dtGrid.Rows[i]["ATD_DATE"], dtGrid.Rows[i]["CurrentDate"], dtGrid.Rows[i]["RangeInDateTime"], dtGrid.Rows[i]["RangeOutDateTime"], dtGrid.Rows[i]["DAYNAME"], dtGrid.Rows[i]["EmployeeId"], dtGrid.Rows[i]["EmployeeNo"], dtGrid.Rows[i]["EmployeeName"], dtGrid.Rows[i]["Mobile1"], dtGrid.Rows[i]["CNIC"], dtGrid.Rows[i]["Email"], dtGrid.Rows[i]["DepartmentName"], dtGrid.Rows[i]["DesignationName"], dtGrid.Rows[i]["InEntryMode"], dtGrid.Rows[i]["OutEntryMode"], dtGrid.Rows[i]["InDateTime"], dtGrid.Rows[i]["OutDateTime"], dtGrid.Rows[i]["SystemOutDateTime"], dtGrid.Rows[i]["ShiftTiming"], dtGrid.Rows[i]["InTime"], dtGrid.Rows[i]["OutTime"], dtGrid.Rows[i]["TimeDuration"], dtGrid.Rows[i]["Late"], dtGrid.Rows[i]["Early"], dtGrid.Rows[i]["Status"], dtGrid.Rows[i]["IsLateArrival"], dtGrid.Rows[i]["SelectedMonth"], dtGrid.Rows[i]["Timing"], dtGrid.Rows[i]["Shift"], dtGrid.Rows[i]["IsGazetted"], dtGrid.Rows[i]["IsOffDay"], dtGrid.Rows[i]["DRIsOffDuty"], dtGrid.Rows[i]["DRShiftId"], dtGrid.Rows[i]["GraceInDateTime"], dtGrid.Rows[i]["GraceOutDateTime"], dtGrid.Rows[i]["StartTime"], dtGrid.Rows[i]["EndTime"], dtGrid.Rows[i]["Decsription"], dtGrid.Rows[i]["Short"], dtGrid.Rows[i]["OT"], dtGrid.Rows[i]["ShortHours"], dtGrid.Rows[i]["ShortMinutes"], dtGrid.Rows[i]["OTHours"], dtGrid.Rows[i]["OTMinutes"], dtGrid.Rows[i]["EmployeeHours"], dtGrid.Rows[i]["EmployeeMinutes"], dtGrid.Rows[i]["TotalShortHour"], dtGrid.Rows[i]["TotalShortMinutes"], dtGrid.Rows[i]["TotalOTMinutes"], dtGrid.Rows[i]["CPLQuota"], dtGrid.Rows[i]["CPLEntryMode"], dtGrid.Rows[i]["TOTAL_HOUR"], dtGrid.Rows[i]["Actual_HOUR"], dtGrid.Rows[i]["EMPLOYEE_WORKINGHOUR"], dtGrid.Rows[i]["OverTime"], dtGrid.Rows[i]["LateMinutes"], dtGrid.Rows[i]["IsRestDay"], dtGrid.Rows[i]["IsHoliday"], dtGrid.Rows[i]["IsLeave"], dtGrid.Rows[i]["IsLeaveApproved"], dtGrid.Rows[i]["IsSuspended"], dtGrid.Rows[i]["IsAbsent"], dtGrid.Rows[i]["IsPresent"], dtGrid.Rows[i]["IsArrivalShortLeave"], dtGrid.Rows[i]["IsDepartureShortLeave"], dtGrid.Rows[i]["IsArrivalHalfDay"], dtGrid.Rows[i]["IsDepartureHalfDay"], dtGrid.Rows[i]["IsEarlyDeparture"], dtGrid.Rows[i]["IsCPL"], dtGrid.Rows[i]["AttendanceStatus"], dtGrid.Rows[i]["BenefitPrefix"]);
				}
				DatagridHistory.DataSource = dt;
				DatagridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DatagridHistory.ClearStructure();
				DatagridHistory.DataSource = null;
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
			DatagridHistory.RootTable.Columns["EmployeeAttendanceId"].Visible = false;
			DatagridHistory.RootTable.Columns["DutyRoasterDetailId"].Visible = false;
			DatagridHistory.RootTable.Columns["EmployeeId"].Visible = false;
			DatagridHistory.RootTable.Columns["Sr#"].Visible = false;
			DatagridHistory.RootTable.Columns["RM"].Visible = false;
			DatagridHistory.RootTable.Columns["RY"].Visible = false;
			DatagridHistory.RootTable.Columns["ATD_DATE"].Visible = false;
			DatagridHistory.RootTable.Columns["TotalShortHour"].Visible = false;
			DatagridHistory.RootTable.Columns["TotalShortMinutes"].Visible = false;
			DatagridHistory.RootTable.Columns["TotalOTMinutes"].Visible = false;
			DatagridHistory.RootTable.Columns["EMPLOYEE_WORKINGHOUR"].Visible = false;
			DatagridHistory.RootTable.Columns["ShortHours"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["ShortHours"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["ShortHours"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["ShortHours"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["ShortMinutes"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["ShortMinutes"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["ShortMinutes"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["ShortMinutes"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["OTHours"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["OTHours"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["OTHours"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["OTHours"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["OTMinutes"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["OTMinutes"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["OTMinutes"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["OTMinutes"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["EmployeeHours"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["EmployeeHours"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["EmployeeHours"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["EmployeeHours"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["EmployeeMinutes"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["EmployeeMinutes"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["EmployeeMinutes"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["EmployeeMinutes"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["TotalShortHour"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["TotalShortHour"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["TotalShortHour"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["TotalShortHour"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["TotalShortMinutes"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["TotalShortMinutes"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["TotalShortMinutes"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["TotalShortMinutes"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["TotalOTMinutes"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["TotalOTMinutes"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["TotalOTMinutes"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["TotalOTMinutes"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["TOTAL_HOUR"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["TOTAL_HOUR"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["TOTAL_HOUR"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["TOTAL_HOUR"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["Actual_HOUR"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["Actual_HOUR"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["Actual_HOUR"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["Actual_HOUR"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["EMPLOYEE_WORKINGHOUR"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["EMPLOYEE_WORKINGHOUR"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["EMPLOYEE_WORKINGHOUR"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["EMPLOYEE_WORKINGHOUR"].TotalFormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["OverTime"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["OverTime"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["OverTime"].FormatString = "#,##0.##";
			DatagridHistory.RootTable.Columns["OverTime"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, DatagridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintRegister_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid == null || dtGrid.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "1001-EmployeeAttendenceMonthly.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn1002Print_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = HRM_Reports.MonthlyAttendanceRegisterRpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DepartmentId = Conversion.ToInt(cmbDepartment.Value),
				GetMonth = Conversion.ToInt((object)txtDutyMonth.Value.Month),
				GetYear = Conversion.ToInt((object)txtDutyYear.Value.Year),
				EmployeeId = Conversion.ToInt(cmbEmployee.Value),
				DepartmentId = Conversion.ToInt(cmbDepartment.Value),
				SectionId = Conversion.ToInt(cmbSection.Value),
				PartyLocationId = Conversion.ToInt(cmbPartyLocation.Value),
				ShiftId = Conversion.ToInt(cmbShift.Value)
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1002-AttendanceRegister.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Employee();
		Department();
		PartyLocation();
		Shift();
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void btnShowRegister_Click(object sender, EventArgs e)
	{
		try
		{
			GridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		try
		{
			DataSet ds = HRM_Reports.genEmployeeAttendence(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Conversion.ToDateTime((object)txtFromDateRegister.Value),
				ToDate = Conversion.ToDateTime((object)txtDateToRegister.Value),
				EmployeeId = Conversion.ToInt(cmbEmployeeRegister.Value),
				DepartmentId = Conversion.ToInt(cmbDepartmentRegister.Value)
			});
			dtGridRegister = ds?.Tables[0]?.Copy();
			if (dtGridRegister.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtGridRegister, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("SRNo", typeof(int));
				dtcol.Columns.Add("EmployeeId", typeof(int));
				dtcol.Columns.Add("EmployeeNo", typeof(int));
				dtcol.Columns.Add("EmployeeName", typeof(string));
				dtcol.Columns.Add("DepartmentName", typeof(string));
				dtcol.Columns.Add("CNIC", typeof(string));
				dtcol.Columns.Add("Email", typeof(string));
				dtcol.Columns.Add("Mobile1", typeof(string));
				dtcol.Columns.Add("Mobile2", typeof(string));
				dtcol.Columns.Add("OutDateTime", typeof(DateTime));
				dtcol.Columns.Add("InDateTime", typeof(DateTime));
				dtcol.Columns.Add("InTime", typeof(string));
				dtcol.Columns.Add("OutTime", typeof(string));
				dtcol.Columns.Add("TimeDifference", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGridRegister.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGridRegister.Rows[i]["SRNo"], dtGridRegister.Rows[i]["EmployeeId"], dtGridRegister.Rows[i]["EmployeeNo"], dtGridRegister.Rows[i]["EmployeeName"], dtGridRegister.Rows[i]["DepartmentName"], dtGridRegister.Rows[i]["CNIC"], dtGridRegister.Rows[i]["Email"], dtGridRegister.Rows[i]["Mobile1"], dtGridRegister.Rows[i]["Mobile2"], dtGridRegister.Rows[i]["OutDateTime"], dtGridRegister.Rows[i]["InDateTime"], dtGridRegister.Rows[i]["InTime"], dtGridRegister.Rows[i]["OutTime"], dtGridRegister.Rows[i]["TimeDifference"]);
				}
				grd.DataSource = dtcol;
				grd.RetrieveStructure();
				GridRegisterSetting();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridRegisterSetting()
	{
		try
		{
			grd.RootTable.Columns["SRNo"].Visible = false;
			grd.RootTable.Columns["EmployeeId"].Visible = false;
			grd.RootTable.Columns["DepartmentName"].Visible = false;
			grd.RootTable.Columns["EmployeeName"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, null, null, null, ColumnSetting: false);
			grd.RootTable.Columns["SRNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["EmployeeNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			grd.RootTable.Columns["EmployeeName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["DepartmentName"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["CNIC"].Width = Constants.InventoryConstants.DriverCnicNoConstant + 30;
			grd.RootTable.Columns["Email"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["Mobile1"].Width = Constants.InventoryConstants.CellNo;
			grd.RootTable.Columns["Mobile2"].Width = Constants.InventoryConstants.CellNo;
			grd.RootTable.Columns["OutDateTime"].Width = Constants.InventoryConstants.DateConstant + 20;
			grd.RootTable.Columns["InDateTime"].Width = Constants.InventoryConstants.DateConstant + 20;
			grd.RootTable.Columns["InTime"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["OutTime"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["TimeDifference"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["OutDateTime"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["InDateTime"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Groups.Add("DepartmentName");
			grd.RootTable.Groups.Add("EmployeeName");
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewRegister_Click(object sender, EventArgs e)
	{
		Department();
		Employee();
		txtFromDateRegister.Focus();
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGridRegister.Rows.Count > 0 && dtGridRegister != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGridRegister, "1004-EmployeeAttendance.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex != 0)
			{
				_ = tabControl1.SelectedIndex;
				_ = 1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDepartment_Leave(object sender, EventArgs e)
	{
		Employee();
	}

	private void cmbDepartmentRegister_Leave(object sender, EventArgs e)
	{
		Employee();
	}

	private void cmbSection_Leave(object sender, EventArgs e)
	{
		Employee();
	}

	private void cmbShift_Leave(object sender, EventArgs e)
	{
		Employee();
	}

	private void cmbPartyLocation_Leave(object sender, EventArgs e)
	{
		Employee();
	}

	private void HRM_Reports_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Expected O, but got Unknown
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnprint101 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint1002 = new System.Windows.Forms.ToolStripButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbShift = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbPartyLocation = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbSection = new UltraCombo();
		this.cmbDepartment = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.txtDutyYear = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDutyMonth = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDutyDate = new System.Windows.Forms.DateTimePicker();
		this.cmbEmployee = new UltraCombo();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.DatagridHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label4 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtFromDateRegister = new System.Windows.Forms.DateTimePicker();
		this.btnShowRegister = new System.Windows.Forms.Button();
		this.cmbEmployeeRegister = new UltraCombo();
		this.txtDateToRegister = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbDepartmentRegister = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewRegister = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyLocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartmentRegister).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label9);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(972, 30);
		this.panel3.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(5, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(187, 19);
		this.label9.TabIndex = 118;
		this.label9.Text = "Employee Attendance";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1236, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(60, 32);
		this.ctrlGrdBar1.TabIndex = 62;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnprint101, this.btnPrint1002 });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(972, 27);
		this.toolStrip3.TabIndex = 95;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnprint101.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint101.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint101.Name = "btnprint101";
		this.btnprint101.Size = new System.Drawing.Size(100, 24);
		this.btnprint101.Text = "1001-Print";
		this.btnprint101.Click += new System.EventHandler(btnPrintRegister_Click);
		this.btnPrint1002.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint1002.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint1002.Name = "btnPrint1002";
		this.btnPrint1002.Size = new System.Drawing.Size(100, 24);
		this.btnPrint1002.Text = "1002-Print";
		this.btnPrint1002.Click += new System.EventHandler(btn1002Print_Click);
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(862, 75);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(64, 26);
		this.btnShow.TabIndex = 8;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(241, 81);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(32, 15);
		this.label8.TabIndex = 94;
		this.label8.Text = "Shift";
		this.cmbShift.AllowNull = (DefaultableBoolean)1;
		this.cmbShift.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShift.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShift.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShift).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbShift).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShift).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShift).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbShift.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShift).Location = new System.Drawing.Point(312, 75);
		((System.Windows.Forms.Control)(object)this.cmbShift).Name = "cmbShift";
		((System.Windows.Forms.Control)(object)this.cmbShift).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbShift).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbShift).Leave += new System.EventHandler(cmbShift_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(532, 23);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(83, 15);
		this.label7.TabIndex = 6;
		this.label7.Text = "Party Location";
		this.cmbPartyLocation.AllowNull = (DefaultableBoolean)1;
		this.cmbPartyLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPartyLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPartyLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPartyLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPartyLocation).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPartyLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPartyLocation).Location = new System.Drawing.Point(619, 17);
		((System.Windows.Forms.Control)(object)this.cmbPartyLocation).Name = "cmbPartyLocation";
		((System.Windows.Forms.Control)(object)this.cmbPartyLocation).Size = new System.Drawing.Size(307, 26);
		((System.Windows.Forms.Control)(object)this.cmbPartyLocation).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbPartyLocation).Leave += new System.EventHandler(cmbPartyLocation_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(241, 52);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(47, 15);
		this.label6.TabIndex = 90;
		this.label6.Text = "Section";
		this.cmbSection.AllowNull = (DefaultableBoolean)1;
		this.cmbSection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbSection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSection).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Location = new System.Drawing.Point(312, 46);
		((System.Windows.Forms.Control)(object)this.cmbSection).Name = "cmbSection";
		((System.Windows.Forms.Control)(object)this.cmbSection).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbSection).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbSection).Leave += new System.EventHandler(cmbSection_Leave);
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(312, 17);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 3;
		this.cmbDepartment.ValueChanged += new System.EventHandler(cmbDepartment_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Leave += new System.EventHandler(cmbDepartment_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(8, 81);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(58, 15);
		this.label3.TabIndex = 86;
		this.label3.Text = "Duty Year";
		this.txtDutyYear.CustomFormat = "yyyy";
		this.txtDutyYear.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDutyYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDutyYear.Location = new System.Drawing.Point(81, 77);
		this.txtDutyYear.Name = "txtDutyYear";
		this.txtDutyYear.Size = new System.Drawing.Size(153, 23);
		this.txtDutyYear.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(8, 52);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(72, 15);
		this.label2.TabIndex = 84;
		this.label2.Text = "Duty Month";
		this.txtDutyMonth.CustomFormat = "MMM";
		this.txtDutyMonth.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDutyMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDutyMonth.Location = new System.Drawing.Point(81, 48);
		this.txtDutyMonth.Name = "txtDutyMonth";
		this.txtDutyMonth.Size = new System.Drawing.Size(153, 23);
		this.txtDutyMonth.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(8, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(61, 15);
		this.label1.TabIndex = 82;
		this.label1.Text = "Duty Date";
		this.txtDutyDate.CustomFormat = "dd-MMM-yy";
		this.txtDutyDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDutyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDutyDate.Location = new System.Drawing.Point(81, 19);
		this.txtDutyDate.Name = "txtDutyDate";
		this.txtDutyDate.Size = new System.Drawing.Size(153, 23);
		this.txtDutyDate.TabIndex = 0;
		this.cmbEmployee.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbEmployee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Location = new System.Drawing.Point(619, 46);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Name = "cmbEmployee";
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Size = new System.Drawing.Size(307, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TabIndex = 7;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(986, 629);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.toolStrip3);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(978, 600);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail Register";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(3, 208);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(972, 389);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 0;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 179);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(972, 29);
		this.panel2.TabIndex = 98;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(931, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 119;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 177);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(972, 420);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(187, 19);
		this.label4.TabIndex = 118;
		this.label4.Text = "Employee Attendance";
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 60);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(972, 119);
		this.panel1.TabIndex = 97;
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDutyMonth);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployee);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShift);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtDutyDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPartyLocation);
		this.groupBox1.Controls.Add(this.txtDutyYear);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSection);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(944, 105);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(532, 52);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(59, 15);
		this.label10.TabIndex = 123;
		this.label10.Text = "Employee";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(241, 23);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(71, 15);
		this.label12.TabIndex = 122;
		this.label12.Text = "Department";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(978, 600);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Short Register";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel6.Location = new System.Drawing.Point(3, 152);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(972, 25);
		this.panel6.TabIndex = 4;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(923, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 60);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(972, 92);
		this.panel5.TabIndex = 2;
		this.groupBox2.Controls.Add(this.txtFromDateRegister);
		this.groupBox2.Controls.Add(this.btnShowRegister);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeRegister);
		this.groupBox2.Controls.Add(this.txtDateToRegister);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartmentRegister);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(944, 80);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.txtFromDateRegister.CustomFormat = "dd-MMM-yyyy";
		this.txtFromDateRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateRegister.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateRegister.Location = new System.Drawing.Point(75, 18);
		this.txtFromDateRegister.Name = "txtFromDateRegister";
		this.txtFromDateRegister.Size = new System.Drawing.Size(165, 23);
		this.txtFromDateRegister.TabIndex = 0;
		this.btnShowRegister.BackColor = System.Drawing.Color.Teal;
		this.btnShowRegister.FlatAppearance.BorderSize = 0;
		this.btnShowRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowRegister.ForeColor = System.Drawing.Color.White;
		this.btnShowRegister.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowRegister.Location = new System.Drawing.Point(653, 46);
		this.btnShowRegister.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowRegister.Name = "btnShowRegister";
		this.btnShowRegister.Size = new System.Drawing.Size(61, 26);
		this.btnShowRegister.TabIndex = 4;
		this.btnShowRegister.Text = "Show";
		this.btnShowRegister.UseVisualStyleBackColor = false;
		this.btnShowRegister.Click += new System.EventHandler(btnShowRegister_Click);
		this.cmbEmployeeRegister.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeRegister).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbEmployeeRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeRegister).Location = new System.Drawing.Point(357, 46);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeRegister).Name = "cmbEmployeeRegister";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeRegister).Size = new System.Drawing.Size(291, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeRegister).TabIndex = 3;
		this.txtDateToRegister.CustomFormat = "dd-MMM-yyyy";
		this.txtDateToRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateToRegister.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateToRegister.Location = new System.Drawing.Point(75, 48);
		this.txtDateToRegister.Name = "txtDateToRegister";
		this.txtDateToRegister.Size = new System.Drawing.Size(165, 23);
		this.txtDateToRegister.TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(264, 52);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(59, 15);
		this.label11.TabIndex = 123;
		this.label11.Text = "Employee";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(7, 52);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 114;
		this.label13.Text = "To Date";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(264, 22);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(71, 15);
		this.label14.TabIndex = 122;
		this.label14.Text = "Department";
		this.cmbDepartmentRegister.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartmentRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartmentRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartmentRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartmentRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartmentRegister).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbDepartmentRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartmentRegister).Location = new System.Drawing.Point(357, 16);
		((System.Windows.Forms.Control)(object)this.cmbDepartmentRegister).Name = "cmbDepartmentRegister";
		((System.Windows.Forms.Control)(object)this.cmbDepartmentRegister).Size = new System.Drawing.Size(291, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartmentRegister).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbDepartmentRegister).Leave += new System.EventHandler(cmbDepartmentRegister_Leave);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(7, 22);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 112;
		this.label27.Text = "From Date";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(972, 57);
		this.panel4.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 32);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(187, 19);
		this.label5.TabIndex = 117;
		this.label5.Text = "Employee Attendance";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewRegister, this.print });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(972, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewRegister.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnNewRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewRegister.Name = "btnNewRegister";
		this.btnNewRegister.Size = new System.Drawing.Size(80, 24);
		this.btnNewRegister.Text = "&Refresh";
		this.btnNewRegister.Click += new System.EventHandler(btnNewRegister_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(100, 24);
		this.print.Text = "1004-Print";
		this.print.Click += new System.EventHandler(print_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(986, 629);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "HRM_Reports";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(HRM_Reports_FormClosing);
		base.Load += new System.EventHandler(HRM_Reports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(genEmployeeAttendence_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyLocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartmentRegister).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
