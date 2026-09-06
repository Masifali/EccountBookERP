using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.AttendanceManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.HRM_Reports;

public class AttendenceStatusByDate : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtEmployeeData = new DataTable();

	private DataTable dtEmployeeInfo = new DataTable();

	private DataTable dtAttendencebyDepartmentWise = new DataTable();

	private IContainer components;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel1;

	private Label label1;

	private DateTimePicker txtDate;

	private Label label2;

	private Panel panel2;

	private Panel panel3;

	private Label lblMonthWorkingDays;

	private Label lblMonthHolidayAndRest;

	private Label lblMonthDays;

	private Label label6;

	private Label label5;

	private Panel panel4;

	private Label label3;

	private Label label4;

	private Panel panel5;

	private Label label11;

	private Label label14;

	private Label label13;

	private Label label12;

	private Label label10;

	private Label lblLate;

	private Label lblAbsent;

	private Label lblPresent;

	private Panel panel6;

	private Panel panel7;

	private Label lblPreviousWorkingDays;

	private Label lblPreviousHolidayAndRest;

	private Label lblPreviousDays;

	private Label label21;

	private Label label22;

	private Label label23;

	private Panel panel8;

	private Label label24;

	private Button btnShow;

	private Panel panel9;

	private Label lblTotalTeams;

	private Label label25;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private PictureBox btnLate;

	private PictureBox btnAbsent;

	private PictureBox btnPresent;

	internal GridEX grd;

	private FlowLayoutPanel departmentFlowLayout;

	private Panel panel10;

	private Label lblTodayLate;

	private Label lblTodayAbsent;

	private Label lblTodayPresent;

	private Label label15;

	private Label label16;

	private Label label17;

	private Label label18;

	private Label label19;

	public AttendenceStatusByDate(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void AttendemceStatusByDate_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 650);
			GetAllEmployeeData();
			GetAttendeceData();
			GetAttendeceDataDepartmentWise();
			dtEmployeeInfo.Columns.Add("Id");
			dtEmployeeInfo.Columns.Add("EmployeeName");
			dtEmployeeInfo.Columns.Add("ThisMonthPresents");
			dtEmployeeInfo.Columns.Add("ThisMonthAbsents");
			dtEmployeeInfo.Columns.Add("ThisMonthLates");
			dtEmployeeInfo.Columns.Add("PreviousMonthPresents");
			dtEmployeeInfo.Columns.Add("PreviousMonthAbsents");
			dtEmployeeInfo.Columns.Add("PreviousMonthLates");
			grd.DataSource = dtEmployeeInfo;
			grd.RetrieveStructure();
			grdSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["EmployeeName"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns["EmployeeName"].Width = 250;
			grd.RootTable.Columns["ThisMonthPresents"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ThisMonthPresents"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ThisMonthAbsents"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ThisMonthAbsents"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ThisMonthLates"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ThisMonthLates"].FormatString = "#,##0.###";
			grd.RootTable.Columns["PreviousMonthPresents"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PreviousMonthPresents"].FormatString = "#,##0.###";
			grd.RootTable.Columns["PreviousMonthAbsents"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PreviousMonthAbsents"].FormatString = "#,##0.###";
			grd.RootTable.Columns["PreviousMonthLates"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PreviousMonthLates"].FormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAttendeceData()
	{
		try
		{
			DataTable dt = hrmManualAttendance.EmployeeAttendenceStatusByDate(UserAccount.OrganizationId, UserAccount.CompanyId, txtDate.Value, (string)null);
			if (dt.Rows.Count > 0)
			{
				lblTotalTeams.Text = dt.Rows[0]["TotalTeam"].ToString();
				lblPresent.Text = dt.Rows[0]["TotalPresent"].ToString();
				lblAbsent.Text = dt.Rows[0]["TotalAbsents"].ToString();
				lblLate.Text = dt.Rows[0]["TotalLate"].ToString();
				lblMonthDays.Text = dt.Rows[0]["ThisMonthTotalDays"].ToString();
				lblMonthHolidayAndRest.Text = (Conversion.ToInt(dt.Rows[0]["ThisMonthHoliDays"]) + Conversion.ToInt(dt.Rows[0]["ThisMonthRestDays"])).ToString();
				lblMonthWorkingDays.Text = dt.Rows[0]["ThisMonthWorkingDays"].ToString();
				lblPreviousDays.Text = dt.Rows[0]["Last30TotalDays"].ToString();
				lblPreviousHolidayAndRest.Text = (Conversion.ToInt(dt.Rows[0]["Last30HoliDays"]) + Conversion.ToInt(dt.Rows[0]["Last30RestDays"])).ToString();
				lblPreviousWorkingDays.Text = dt.Rows[0]["Last30WorkingDays"].ToString();
				int TodayPresent = (from row in dtEmployeeData.AsEnumerable()
					where row.Field<string>("TodayPresent") == "Present"
					select row).Count();
				lblTodayPresent.Text = TodayPresent.ToString();
				int TodayAbsent = (from row in dtEmployeeData.AsEnumerable()
					where row.Field<string>("TodayAbsent") == "Absent"
					select row).Count();
				lblTodayAbsent.Text = TodayAbsent.ToString();
				int TodayLate = (from row in dtEmployeeData.AsEnumerable()
					where row.Field<string>("TodayLate") == "Late"
					select row).Count();
				lblTodayLate.Text = TodayLate.ToString();
			}
			else
			{
				lblTotalTeams.Text = "0";
				lblPresent.Text = "0";
				lblAbsent.Text = "0";
				lblLate.Text = "0";
				lblTodayPresent.Text = "0";
				lblTodayAbsent.Text = "0";
				lblTodayLate.Text = "0";
				lblMonthDays.Text = "0";
				lblMonthHolidayAndRest.Text = "0";
				lblMonthWorkingDays.Text = "0";
				lblPreviousDays.Text = "0";
				lblPreviousHolidayAndRest.Text = "0";
				lblPreviousWorkingDays.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAttendeceDataDepartmentWise()
	{
		try
		{
			dtAttendencebyDepartmentWise.Rows.Clear();
			departmentFlowLayout.Controls.Clear();
			dtAttendencebyDepartmentWise = hrmManualAttendance.EmployeeAttendenceStatusByDate(UserAccount.OrganizationId, UserAccount.CompanyId, txtDate.Value, "DepartmentWise");
			if (dtAttendencebyDepartmentWise.Rows.Count > 0)
			{
				DescriptionCard3[] cards = new DescriptionCard3[dtAttendencebyDepartmentWise.Rows.Count];
				for (int i = 0; i < cards.Length; i++)
				{
					cards[i] = new DescriptionCard3(UserAccount);
					cards[i].Id = Conversion.ToInt(dtAttendencebyDepartmentWise.Rows[i]["DepartmentId"]);
					cards[i].Description = dtAttendencebyDepartmentWise.Rows[i]["DepartmentName"].ToString();
					double present = Conversion.ToDouble(dtAttendencebyDepartmentWise.Rows[i]["TotalPresent"]);
					double Absent = Conversion.ToDouble(dtAttendencebyDepartmentWise.Rows[i]["TotalAbsent"]);
					double Late = Conversion.ToDouble(dtAttendencebyDepartmentWise.Rows[i]["TotalLate"]);
					double Total = present + Absent + Late;
					cards[i].lable1 = present.ToString();
					cards[i].lable2 = Absent.ToString();
					cards[i].lable3 = Late.ToString();
					cards[i].lable4 = Total.ToString();
					cards[i].DepData = dtAttendencebyDepartmentWise;
					cards[i].FilterDate = txtDate.Value;
					departmentFlowLayout.Controls.Add(cards[i]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllEmployeeData()
	{
		try
		{
			dtEmployeeData.Rows.Clear();
			dtEmployeeData = hrmManualAttendance.EmployeeAttandanceSummaryStatus(UserAccount.OrganizationId, UserAccount.CompanyId, txtDate.Value, 0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPresent_Click(object sender, EventArgs e)
	{
		try
		{
			dtEmployeeInfo.Rows.Clear();
			if (Conversion.ToInt((object)lblPresent.Text) > 0)
			{
				if (dtEmployeeData.Rows.Count > 0)
				{
					GetDataByEmployeeStatus("TodayPresent", "Present");
				}
				else
				{
					grd.ClearStructure();
				}
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

	private void GetDataByEmployeeStatus(string Status, string Value)
	{
		try
		{
			EnumerableRowCollection<DataRow> rows = from row in dtEmployeeData.AsEnumerable()
				where row.Field<string>(Status) == Value
				select row;
			DataTable PresentEmployees = (rows.Any() ? rows.CopyToDataTable() : dtEmployeeData.Clone());
			for (int i = 0; i < PresentEmployees.Rows.Count; i++)
			{
				dtEmployeeInfo.Rows.Add(PresentEmployees.Rows[i]["EmployeeId"], PresentEmployees.Rows[i]["EmployeeName"], PresentEmployees.Rows[i]["Present"], PresentEmployees.Rows[i]["Absent"], PresentEmployees.Rows[i]["ThisMonthLate"], PresentEmployees.Rows[i]["Last30DaysPresent"], PresentEmployees.Rows[i]["Last30DaysAbsent"], PresentEmployees.Rows[i]["Last30DaysLate"]);
			}
			grd.DataSource = dtEmployeeInfo;
			grd.RetrieveStructure();
			grdSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Clicked(object sender, EventArgs e)
	{
	}

	private void btnAbsent_Click(object sender, EventArgs e)
	{
		try
		{
			dtEmployeeInfo.Rows.Clear();
			if (Conversion.ToInt((object)lblPresent.Text) > 0)
			{
				if (dtEmployeeData.Rows.Count > 0)
				{
					GetDataByEmployeeStatus("TodayAbsent", "Absent");
				}
				else
				{
					grd.ClearStructure();
				}
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

	private void btnLate_Click(object sender, EventArgs e)
	{
		try
		{
			dtEmployeeInfo.Rows.Clear();
			if (Conversion.ToInt((object)lblPresent.Text) > 0)
			{
				if (dtEmployeeData.Rows.Count > 0)
				{
					GetDataByEmployeeStatus("TodayLate", "Late");
				}
				else
				{
					grd.ClearStructure();
				}
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

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GetAllEmployeeData();
			GetAttendeceData();
			GetAttendeceDataDepartmentWise();
			grd.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0 && e.Column.Key == "EmployeeName")
			{
				AttendenceStatusByEmployeeAndDepartment attendenceStatusByEmployeeAndDepartment = new AttendenceStatusByEmployeeAndDepartment(UserAccount);
				attendenceStatusByEmployeeAndDepartment.RequestByEmployee = true;
				attendenceStatusByEmployeeAndDepartment.RequestForMonthlyStatus = true;
				attendenceStatusByEmployeeAndDepartment.EmployeeId = Conversion.ToInt(row.Cells["Id"].Value);
				attendenceStatusByEmployeeAndDepartment.EmployeeName = Conversion.ToString(row.Cells["EmployeeName"].Value);
				attendenceStatusByEmployeeAndDepartment.WindowState = FormWindowState.Maximized;
				attendenceStatusByEmployeeAndDepartment.Show();
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
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_21b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_21bb: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDate = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lblTotalTeams = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.label25 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.lblMonthWorkingDays = new System.Windows.Forms.Label();
		this.lblMonthHolidayAndRest = new System.Windows.Forms.Label();
		this.lblMonthDays = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblLate = new System.Windows.Forms.Label();
		this.lblAbsent = new System.Windows.Forms.Label();
		this.lblPresent = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.btnLate = new System.Windows.Forms.PictureBox();
		this.btnAbsent = new System.Windows.Forms.PictureBox();
		this.btnPresent = new System.Windows.Forms.PictureBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.lblTodayLate = new System.Windows.Forms.Label();
		this.lblTodayAbsent = new System.Windows.Forms.Label();
		this.lblTodayPresent = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.lblPreviousWorkingDays = new System.Windows.Forms.Label();
		this.lblPreviousHolidayAndRest = new System.Windows.Forms.Label();
		this.lblPreviousDays = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.departmentFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnLate).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnAbsent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnPresent).BeginInit();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel9.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.tabPage2.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(991, 25);
		this.toolStrip3.TabIndex = 96;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Visible = false;
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 25);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(991, 27);
		this.panel1.TabIndex = 97;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(7, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(164, 19);
		this.label2.TabIndex = 98;
		this.label2.Text = "Attendence Report";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(44, 21);
		this.label1.TabIndex = 0;
		this.label1.Text = "Date";
		this.txtDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDate.Location = new System.Drawing.Point(50, 6);
		this.txtDate.Name = "txtDate";
		this.txtDate.Size = new System.Drawing.Size(135, 23);
		this.txtDate.TabIndex = 1;
		this.panel2.Controls.Add(this.lblTotalTeams);
		this.panel2.Controls.Add(this.btnShow);
		this.panel2.Controls.Add(this.txtDate);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.label25);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 52);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(991, 37);
		this.panel2.TabIndex = 98;
		this.lblTotalTeams.AutoSize = true;
		this.lblTotalTeams.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalTeams.Location = new System.Drawing.Point(627, 6);
		this.lblTotalTeams.Name = "lblTotalTeams";
		this.lblTotalTeams.Size = new System.Drawing.Size(19, 21);
		this.lblTotalTeams.TabIndex = 103;
		this.lblTotalTeams.Text = "0";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(188, 4);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(64, 26);
		this.btnShow.TabIndex = 101;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(493, 6);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(138, 21);
		this.label25.TabIndex = 102;
		this.label25.Text = "Total Employee's: ";
		this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel3.Controls.Add(this.lblMonthWorkingDays);
		this.panel3.Controls.Add(this.lblMonthHolidayAndRest);
		this.panel3.Controls.Add(this.lblMonthDays);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Location = new System.Drawing.Point(9, 6);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(178, 112);
		this.panel3.TabIndex = 99;
		this.lblMonthWorkingDays.AutoSize = true;
		this.lblMonthWorkingDays.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMonthWorkingDays.Location = new System.Drawing.Point(148, 88);
		this.lblMonthWorkingDays.Name = "lblMonthWorkingDays";
		this.lblMonthWorkingDays.Size = new System.Drawing.Size(15, 17);
		this.lblMonthWorkingDays.TabIndex = 6;
		this.lblMonthWorkingDays.Text = "0";
		this.lblMonthHolidayAndRest.AutoSize = true;
		this.lblMonthHolidayAndRest.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMonthHolidayAndRest.Location = new System.Drawing.Point(148, 65);
		this.lblMonthHolidayAndRest.Name = "lblMonthHolidayAndRest";
		this.lblMonthHolidayAndRest.Size = new System.Drawing.Size(15, 17);
		this.lblMonthHolidayAndRest.TabIndex = 5;
		this.lblMonthHolidayAndRest.Text = "0";
		this.lblMonthDays.AutoSize = true;
		this.lblMonthDays.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMonthDays.Location = new System.Drawing.Point(148, 42);
		this.lblMonthDays.Name = "lblMonthDays";
		this.lblMonthDays.Size = new System.Drawing.Size(15, 17);
		this.lblMonthDays.TabIndex = 4;
		this.lblMonthDays.Text = "0";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(4, 87);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(93, 17);
		this.label6.TabIndex = 3;
		this.label6.Text = "Working Days";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 64);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(97, 17);
		this.label5.TabIndex = 2;
		this.label5.Text = "Holiday + Rest";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 41);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(70, 17);
		this.label4.TabIndex = 1;
		this.label4.Text = "Total Days";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.ForeColor = System.Drawing.Color.White;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(176, 38);
		this.panel4.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(49, 11);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(77, 16);
		this.label3.TabIndex = 0;
		this.label3.Text = "This Month";
		this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel5.Controls.Add(this.lblLate);
		this.panel5.Controls.Add(this.lblAbsent);
		this.panel5.Controls.Add(this.lblPresent);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.label14);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Location = new System.Drawing.Point(382, 6);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(376, 52);
		this.panel5.TabIndex = 100;
		this.lblLate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLate.Location = new System.Drawing.Point(296, 31);
		this.lblLate.Name = "lblLate";
		this.lblLate.Size = new System.Drawing.Size(64, 17);
		this.lblLate.TabIndex = 104;
		this.lblLate.Text = "0";
		this.lblLate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblAbsent.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAbsent.Location = new System.Drawing.Point(208, 31);
		this.lblAbsent.Name = "lblAbsent";
		this.lblAbsent.Size = new System.Drawing.Size(64, 17);
		this.lblAbsent.TabIndex = 103;
		this.lblAbsent.Text = "0";
		this.lblAbsent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPresent.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPresent.Location = new System.Drawing.Point(118, 31);
		this.lblPresent.Name = "lblPresent";
		this.lblPresent.Size = new System.Drawing.Size(64, 17);
		this.lblPresent.TabIndex = 102;
		this.lblPresent.Text = "0";
		this.lblPresent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label11.BackColor = System.Drawing.Color.Black;
		this.label11.Location = new System.Drawing.Point(109, 25);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(250, 1);
		this.label11.TabIndex = 101;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(310, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(33, 17);
		this.label14.TabIndex = 8;
		this.label14.Text = "Late";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(212, 4);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(51, 17);
		this.label13.TabIndex = 7;
		this.label13.Text = "Absent";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(124, 4);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(54, 17);
		this.label12.TabIndex = 3;
		this.label12.Text = "Present";
		this.label10.Dock = System.Windows.Forms.DockStyle.Left;
		this.label10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Teal;
		this.label10.Location = new System.Drawing.Point(0, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(103, 50);
		this.label10.TabIndex = 2;
		this.label10.Text = "This Month Employee's";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnLate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnLate.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLate.Location = new System.Drawing.Point(338, 4);
		this.btnLate.Name = "btnLate";
		this.btnLate.Size = new System.Drawing.Size(21, 17);
		this.btnLate.TabIndex = 107;
		this.btnLate.TabStop = false;
		this.btnLate.Click += new System.EventHandler(btnLate_Click);
		this.btnAbsent.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAbsent.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnAbsent.Location = new System.Drawing.Point(259, 4);
		this.btnAbsent.Name = "btnAbsent";
		this.btnAbsent.Size = new System.Drawing.Size(21, 17);
		this.btnAbsent.TabIndex = 106;
		this.btnAbsent.TabStop = false;
		this.btnAbsent.Click += new System.EventHandler(btnAbsent_Click);
		this.btnPresent.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPresent.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnPresent.Location = new System.Drawing.Point(175, 4);
		this.btnPresent.Name = "btnPresent";
		this.btnPresent.Size = new System.Drawing.Size(21, 17);
		this.btnPresent.TabIndex = 105;
		this.btnPresent.TabStop = false;
		this.btnPresent.Click += new System.EventHandler(btnPresent_Click);
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Controls.Add(this.panel5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 89);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(991, 123);
		this.panel6.TabIndex = 101;
		this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel10.Controls.Add(this.btnLate);
		this.panel10.Controls.Add(this.lblTodayLate);
		this.panel10.Controls.Add(this.btnAbsent);
		this.panel10.Controls.Add(this.lblTodayAbsent);
		this.panel10.Controls.Add(this.btnPresent);
		this.panel10.Controls.Add(this.lblTodayPresent);
		this.panel10.Controls.Add(this.label15);
		this.panel10.Controls.Add(this.label16);
		this.panel10.Controls.Add(this.label17);
		this.panel10.Controls.Add(this.label18);
		this.panel10.Controls.Add(this.label19);
		this.panel10.Location = new System.Drawing.Point(382, 66);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(376, 52);
		this.panel10.TabIndex = 108;
		this.lblTodayLate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTodayLate.Location = new System.Drawing.Point(296, 31);
		this.lblTodayLate.Name = "lblTodayLate";
		this.lblTodayLate.Size = new System.Drawing.Size(64, 17);
		this.lblTodayLate.TabIndex = 104;
		this.lblTodayLate.Text = "0";
		this.lblTodayLate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTodayAbsent.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTodayAbsent.Location = new System.Drawing.Point(208, 31);
		this.lblTodayAbsent.Name = "lblTodayAbsent";
		this.lblTodayAbsent.Size = new System.Drawing.Size(64, 17);
		this.lblTodayAbsent.TabIndex = 103;
		this.lblTodayAbsent.Text = "0";
		this.lblTodayAbsent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTodayPresent.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTodayPresent.Location = new System.Drawing.Point(118, 31);
		this.lblTodayPresent.Name = "lblTodayPresent";
		this.lblTodayPresent.Size = new System.Drawing.Size(64, 17);
		this.lblTodayPresent.TabIndex = 102;
		this.lblTodayPresent.Text = "0";
		this.lblTodayPresent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label15.BackColor = System.Drawing.Color.Black;
		this.label15.Location = new System.Drawing.Point(109, 25);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(250, 1);
		this.label15.TabIndex = 101;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(310, 4);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(33, 17);
		this.label16.TabIndex = 8;
		this.label16.Text = "Late";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(212, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(51, 17);
		this.label17.TabIndex = 7;
		this.label17.Text = "Absent";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(124, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(54, 17);
		this.label18.TabIndex = 3;
		this.label18.Text = "Present";
		this.label19.Dock = System.Windows.Forms.DockStyle.Left;
		this.label19.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Teal;
		this.label19.Location = new System.Drawing.Point(0, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(103, 50);
		this.label19.TabIndex = 2;
		this.label19.Text = "Today";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel7.Controls.Add(this.lblPreviousWorkingDays);
		this.panel7.Controls.Add(this.lblPreviousHolidayAndRest);
		this.panel7.Controls.Add(this.lblPreviousDays);
		this.panel7.Controls.Add(this.label21);
		this.panel7.Controls.Add(this.label22);
		this.panel7.Controls.Add(this.label23);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Location = new System.Drawing.Point(200, 6);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(178, 112);
		this.panel7.TabIndex = 100;
		this.lblPreviousWorkingDays.AutoSize = true;
		this.lblPreviousWorkingDays.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPreviousWorkingDays.Location = new System.Drawing.Point(148, 88);
		this.lblPreviousWorkingDays.Name = "lblPreviousWorkingDays";
		this.lblPreviousWorkingDays.Size = new System.Drawing.Size(15, 17);
		this.lblPreviousWorkingDays.TabIndex = 6;
		this.lblPreviousWorkingDays.Text = "0";
		this.lblPreviousHolidayAndRest.AutoSize = true;
		this.lblPreviousHolidayAndRest.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPreviousHolidayAndRest.Location = new System.Drawing.Point(148, 65);
		this.lblPreviousHolidayAndRest.Name = "lblPreviousHolidayAndRest";
		this.lblPreviousHolidayAndRest.Size = new System.Drawing.Size(15, 17);
		this.lblPreviousHolidayAndRest.TabIndex = 5;
		this.lblPreviousHolidayAndRest.Text = "0";
		this.lblPreviousDays.AutoSize = true;
		this.lblPreviousDays.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPreviousDays.Location = new System.Drawing.Point(148, 42);
		this.lblPreviousDays.Name = "lblPreviousDays";
		this.lblPreviousDays.Size = new System.Drawing.Size(15, 17);
		this.lblPreviousDays.TabIndex = 4;
		this.lblPreviousDays.Text = "0";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(4, 87);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(93, 17);
		this.label21.TabIndex = 3;
		this.label21.Text = "Working Days";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(4, 64);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(97, 17);
		this.label22.TabIndex = 2;
		this.label22.Text = "Holiday + Rest";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(4, 41);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(70, 17);
		this.label23.TabIndex = 1;
		this.label23.Text = "Total Days";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label24);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.ForeColor = System.Drawing.Color.White;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(176, 38);
		this.panel8.TabIndex = 0;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(33, 11);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(108, 16);
		this.label24.TabIndex = 0;
		this.label24.Text = "Previous Month";
		this.panel9.Controls.Add(this.tabControl1);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 212);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(991, 392);
		this.panel9.TabIndex = 102;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(991, 392);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Location = new System.Drawing.Point(4, 23);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(983, 365);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "All ";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(977, 359);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.tabPage2.Controls.Add(this.departmentFlowLayout);
		this.tabPage2.Location = new System.Drawing.Point(4, 23);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(983, 365);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Department Wise";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.departmentFlowLayout.AutoScroll = true;
		this.departmentFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.departmentFlowLayout.Location = new System.Drawing.Point(3, 3);
		this.departmentFlowLayout.Name = "departmentFlowLayout";
		this.departmentFlowLayout.Size = new System.Drawing.Size(977, 359);
		this.departmentFlowLayout.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(991, 604);
		base.Controls.Add(this.panel9);
		base.Controls.Add(this.panel6);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip3);
		base.Name = "AttendenceStatusByDate";
		this.Text = "AttendemceStatusByDate";
		base.Load += new System.EventHandler(AttendemceStatusByDate_Load);
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnLate).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnAbsent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnPresent).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.tabPage2.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
