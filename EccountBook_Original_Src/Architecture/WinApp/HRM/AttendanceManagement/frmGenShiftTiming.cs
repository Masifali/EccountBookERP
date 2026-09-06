using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.AttendanceManagement;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.AttendanceManagement;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.EmployeeManagement;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.AttendanceManagement;

public class frmGenShiftTiming : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	public Rightsobjects formright = new Rightsobjects();

	private bool UpdateMode;

	private int RecId;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private DateTimePicker datHalfDayOutEndTime;

	private DateTimePicker datHalfDayInEndTime;

	private DateTimePicker datAbsentBeforeOutTime;

	private DateTimePicker datShortLeaveOutEndTime;

	private Label label27;

	private Label label25;

	private Label label29;

	private Label label23;

	private DateTimePicker datHalfDayOutStartTime;

	private Label label26;

	private DateTimePicker datHalfDayInStartTime;

	private DateTimePicker datAbsentAfterInTime;

	private Label label24;

	private Label label28;

	private DateTimePicker datShortLeaveOutStartTime;

	private Label label22;

	private DateTimePicker datShortLeaveInEndTime;

	private Label label21;

	private DateTimePicker datShortLeaveInStartTime;

	private Label label20;

	private GroupBox groupBox1;

	private Label label4;

	private DateTimePicker datEndDate;

	private Label label3;

	private DateTimePicker datStartDate;

	private TextBox txtDescription;

	private Label label1;

	private Panel panel5;

	private Panel panel7;

	private Panel panel6;

	internal GridEX grdShift;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton toolStripButton4;

	private Label label30;

	internal UltraCombo CmbShift;

	private CtrlGrdBar ctrlGrdBar1;

	private GroupBox groupBox3;

	private TextBox txtTotalDaysHours;

	private Label label35;

	private TextBox txtTotalHours;

	private Label label34;

	private TextBox txtOutHours;

	private Label label33;

	private TextBox txtInHours;

	private Label label32;

	private TextBox txtTotalShiftHours;

	private Label label31;

	private CheckBox ChkBoxIsClosedNextDay;

	private Label label9;

	private DateTimePicker datEndTime;

	private DateTimePicker datEndBreakTime;

	private Label label7;

	private Label label8;

	private DateTimePicker datStartBreakTime;

	private Label label6;

	private DateTimePicker datStartTime;

	private GroupBox groupBox5;

	private GroupBox groupBox4;

	private DateTimePicker datLateEndTime;

	private Label label37;

	private DateTimePicker datLateStartTime;

	private Label label36;

	private DateTimePicker datDepartureEndTime;

	private Label label5;

	private DateTimePicker datDepartureStarTime;

	private Label label2;

	private GroupBox groupBox2;

	internal GridEX grd;

	internal UltraCombo CmbShiftLocation;

	private Label label10;

	private ToolStripButton btnSaveAs;

	private Panel panel8;

	private Label label11;

	private Panel panel9;

	private Button brnShift;

	private Button btnShiftLocation;

	private Panel panel10;

	private Label label12;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnDutyRoasterForm;

	private ToolStripButton toolStripButton1;

	public frmGenShiftTiming(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGenShiftTiming_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(980, 700);
		formright = CommonServices.SetRightsValueInRightsObject("ShiftTiming");
		btnsave.Enabled = formright.DoHaveSaveRight;
		btnPrint.Enabled = formright.DoHavePrintRights;
		btnupdate.Enabled = formright.DoHaveUpdateRights;
		ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
		ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
		ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
		ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
		ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
		ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
		BindShift();
		WeekDaysFill();
		CmbShift.Focus();
	}

	private void BindShift()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = genShift.Getall(new genShift
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbShift, "ShiftId", "ShiftName", "ShiftName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindShiftLocation()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
			((UltraGridBase)CmbShiftLocation).DataSource = null;
			((Control)(object)CmbShiftLocation).Text = string.Empty;
			DataTable dt = genShiftLocation.Getall(new genShiftLocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ShiftId = Conversion.ToInt(CmbShift.Value),
				IsActive = true
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtcmb = new DataTable();
				dtcmb.Columns.Add("ShiftId");
				dtcmb.Columns.Add("LocationName");
				dtcmb.Columns.Add("ShiftName");
				dtcmb.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcmb.Rows.Add(dt.Rows[i]["ShiftLocationId"], dt.Rows[i]["LocationName"], dt.Rows[i]["ShiftName"]);
				}
				DDL.BindDDL(dtcmb, CmbShiftLocation, "ShiftId", "LocationName", "LocationName", ZeroIndex: false);
				((UltraGridBase)CmbShiftLocation).Rows[0].Activate();
			}
			else
			{
				((Control)(object)CmbShiftLocation).Text = string.Empty;
				((UltraGridBase)CmbShiftLocation).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeekDaysFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			DataTable dt = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsActive = true,
				ProfileTypeId = 60
			});
			if (dt.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("ProfileId");
				table.Columns.Add("ProfileName");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					table.Rows.Add(dt.Rows[i]["ProfileId"], dt.Rows[i]["ProfileName"]);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				WeekDaySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeekDaySetting()
	{
		try
		{
			grd.RootTable.Columns.Add("Wds");
			grd.RootTable.Columns["Wds"].ColumnType = (ColumnType)3;
			grd.RootTable.Columns["Wds"].ActAsSelector = true;
			grd.RootTable.Columns["Wds"].UseHeaderSelector = true;
			grd.RootTable.Columns["Wds"].Width = 40;
			grd.RootTable.Columns["ProfileName"].Width = 150;
			grd.RootTable.Columns["ProfileName"].Caption = "Day Name";
			grd.RootTable.Columns["ProfileId"].Visible = false;
			grd.RootTable.Columns["ProfileName"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbShift_Leave(object sender, EventArgs e)
	{
		try
		{
			BindShiftLocation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Expected O, but got Unknown
		try
		{
			if (!Validation())
			{
				return;
			}
			genShiftTiming timing = new genShiftTiming();
			if (grd.GetCheckedRows().Count() == 0)
			{
				MessageBox.Show("Please Select Week Days");
				return;
			}
			CheckShiftTiming(timing);
			if (Convert.ToDouble(txtTotalDaysHours.Text) != 24.0)
			{
				throw new Exception("Total Days Hours not less or gratter than 24 hours");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				timing.ShiftTimingId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			timing.OrganizationId = UserAccount.OrganizationId;
			timing.CompanyId = UserAccount.CompanyId;
			timing.TimingDescription = txtDescription.Text;
			timing.ShiftId = Conversion.ToInt(CmbShift.Value);
			timing.ShiftLocationId = Conversion.ToInt(CmbShiftLocation.Value);
			timing.StartDate = datStartDate.Value;
			timing.EndDate = datEndDate.Value;
			string StartDate = Conversion.ToString((object)timing.StartDate.ToString("dd/MMM/yyyy"));
			timing.StartTime = Convert.ToDateTime(StartDate + " " + datStartTime.Value.TimeOfDay.ToString());
			timing.StartBreakTime = Convert.ToDateTime(StartDate + " " + datStartBreakTime.Value.TimeOfDay.ToString());
			timing.EndBreakTime = Convert.ToDateTime(StartDate + " " + datEndBreakTime.Value.TimeOfDay.ToString());
			timing.EndTime = Convert.ToDateTime(StartDate + " " + datEndTime.Value.TimeOfDay.ToString());
			timing.WeekDayText = "";
			timing.GraceTime = Convert.ToDateTime(StartDate + " " + datLateStartTime.Value.TimeOfDay.ToString());
			timing.GraceInEndTime = Convert.ToDateTime(StartDate + " " + datLateEndTime.Value.TimeOfDay.ToString());
			timing.BeforeInHours = Conversion.ToInt((object)txtInHours.Text);
			timing.AfterOutHours = Conversion.ToInt((object)txtOutHours.Text);
			timing.ReportTime = DateTime.Now;
			timing.GraceOutStartTime = Convert.ToDateTime(StartDate + " " + datDepartureStarTime.Value.TimeOfDay.ToString());
			timing.GraceOutEndTime = Convert.ToDateTime(StartDate + " " + datDepartureEndTime.Value.TimeOfDay.ToString());
			timing.ShortLeaveInStartTime = Convert.ToDateTime(StartDate + " " + datShortLeaveInStartTime.Value.TimeOfDay.ToString());
			timing.ShortLeaveInEndTime = Convert.ToDateTime(StartDate + " " + datShortLeaveInEndTime.Value.TimeOfDay.ToString());
			timing.ShortLeaveOutStartTime = Convert.ToDateTime(StartDate + " " + datShortLeaveOutStartTime.Value.TimeOfDay.ToString());
			timing.ShortLeaveOutEndTime = Convert.ToDateTime(StartDate + " " + datShortLeaveOutEndTime.Value.TimeOfDay.ToString());
			timing.HalfDayInStartTime = Convert.ToDateTime(StartDate + " " + datHalfDayInStartTime.Value.TimeOfDay.ToString());
			timing.HalfDayOutStartTime = Convert.ToDateTime(StartDate + " " + datHalfDayOutStartTime.Value.TimeOfDay.ToString());
			timing.HalfDayOutEndTime = Convert.ToDateTime(StartDate + " " + datHalfDayOutEndTime.Value.TimeOfDay.ToString());
			timing.HalfDayInEndTime = Convert.ToDateTime(StartDate + " " + datHalfDayInEndTime.Value.TimeOfDay.ToString());
			timing.AbsentAfterInTime = Convert.ToDateTime(StartDate + " " + datAbsentAfterInTime.Value.TimeOfDay.ToString());
			timing.AbsentBeforeOutTime = Convert.ToDateTime(StartDate + " " + datAbsentBeforeOutTime.Value.TimeOfDay.ToString());
			timing.CreatedById = UserAccount.ID;
			timing.CreatedOn = DateTime.Now;
			timing.AlteredById = UserAccount.ID;
			timing.AlteredOn = DateTime.Now;
			timing.UserLogId = UserAccount.ID;
			if (ChkBoxIsClosedNextDay.Checked)
			{
				timing.IsNextDayClosed = true;
			}
			else
			{
				timing.IsNextDayClosed = false;
			}
			timing.GenShiftTimingWeekDaysDetailList = new List<genShiftTimingWeekDay>();
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				genShiftTimingWeekDay obj = new genShiftTimingWeekDay();
				obj.WeekDayProfileId = Conversion.ToInt(r.Cells[0].Value);
				obj.WeekDayName = Conversion.ToString(r.Cells[1].Value);
				obj.CreatedById = UserAccount.ID;
				obj.CreatedOn = DateTime.Now;
				obj.AlteredById = UserAccount.ID;
				obj.AlteredOn = DateTime.Now;
				obj.ActionTypeId = 1;
				obj.UserLogId = UserAccount.ID;
				timing.GenShiftTimingWeekDaysDetailList.Add(obj);
			}
			genShiftTiming.Save(timing);
			if (RecId == 0)
			{
				MessageBox.Show("Record Save Successfully");
			}
			else
			{
				MessageBox.Show("Record Update Successfully");
			}
			RefreshForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			genShiftTiming timing = new genShiftTiming();
			RecId = Id;
			timing = genShiftTiming.GetByID(RecId);
			if (timing != null)
			{
				tabControl1.SelectedIndex = 0;
				txtDescription.Text = timing.TimingDescription;
				CmbShift.Value = timing.ShiftId;
				CmbShift_Leave(null, null);
				CmbShiftLocation.Value = timing.ShiftLocationId;
				datStartDate.Value = timing.StartDate;
				datEndDate.Value = timing.EndDate;
				datStartTime.Value = timing.StartTime;
				datEndTime.Value = timing.EndTime;
				datStartBreakTime.Value = timing.StartBreakTime;
				datEndBreakTime.Value = timing.EndBreakTime;
				datLateStartTime.Value = timing.GraceTime;
				datLateEndTime.Value = timing.GraceInEndTime;
				txtInHours.Text = timing.BeforeInHours.ToString();
				txtOutHours.Text = timing.AfterOutHours.ToString();
				datDepartureStarTime.Value = timing.GraceOutStartTime;
				datDepartureEndTime.Value = timing.GraceOutEndTime;
				datShortLeaveInStartTime.Value = timing.ShortLeaveInStartTime;
				datShortLeaveInEndTime.Value = timing.ShortLeaveInEndTime;
				datShortLeaveOutStartTime.Value = timing.ShortLeaveOutStartTime;
				datShortLeaveOutEndTime.Value = timing.ShortLeaveOutEndTime;
				datHalfDayInStartTime.Value = timing.HalfDayInStartTime;
				datHalfDayOutStartTime.Value = timing.HalfDayOutStartTime;
				datHalfDayOutEndTime.Value = timing.HalfDayOutEndTime;
				datHalfDayInEndTime.Value = timing.HalfDayInEndTime;
				datAbsentAfterInTime.Value = timing.AbsentAfterInTime;
				datAbsentBeforeOutTime.Value = timing.AbsentBeforeOutTime;
				ChkBoxIsClosedNextDay.Checked = timing.IsNextDayClosed;
				btnsave.Visible = false;
				btnSaveAs.Visible = false;
				btnupdate.Visible = true;
				UpdateMode = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			RefreshForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefreshForm()
	{
		try
		{
			datStartDate.Value = DateTime.Now;
			datAbsentAfterInTime.Value = DateTime.Now;
			datAbsentBeforeOutTime.Value = DateTime.Now;
			datEndBreakTime.Value = DateTime.Now;
			datEndDate.Value = DateTime.Now;
			datEndTime.Value = DateTime.Now;
			((Control)(object)CmbShiftLocation).Text = string.Empty;
			datHalfDayInEndTime.Value = DateTime.Now;
			datHalfDayInStartTime.Value = DateTime.Now;
			datHalfDayOutEndTime.Value = DateTime.Now;
			datHalfDayOutStartTime.Value = DateTime.Now;
			datShortLeaveInEndTime.Value = DateTime.Now;
			datShortLeaveInStartTime.Value = DateTime.Now;
			datShortLeaveOutEndTime.Value = DateTime.Now;
			datShortLeaveOutStartTime.Value = DateTime.Now;
			datStartBreakTime.Value = DateTime.Now;
			datStartDate.Value = DateTime.Now;
			datStartTime.Value = DateTime.Now;
			txtDescription.Text = string.Empty;
			CmbShift.Value = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnSaveAs.Visible = false;
			UpdateMode = false;
			grd.ClearStructure();
			RecId = 0;
			GridBind();
			WeekDaysFill();
			CmbShift.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		BindShift();
		WeekDaysFill();
		BindShiftLocation();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				GridBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdShift_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)grdShift.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genShiftTiming.Getall(new genShiftTiming
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("LoacationName");
				dt.Columns.Add("ShiftName");
				dt.Columns.Add("ShiftTimingDesc", typeof(string));
				dt.Columns.Add("Location");
				dt.Columns.Add("WeekDay");
				dt.Columns.Add("StartDate", typeof(DateTime));
				dt.Columns.Add("EndDate", typeof(DateTime));
				dt.Columns.Add("StartTime", typeof(DateTime));
				dt.Columns.Add("EndTime", typeof(DateTime));
				dt.Columns.Add("startBreaktime", typeof(DateTime));
				dt.Columns.Add("EndBreaktime", typeof(DateTime));
				dt.Columns.Add("GraceInTime", typeof(DateTime));
				dt.Columns.Add("GraceEndTime", typeof(DateTime));
				dt.Columns.Add("BeforeInHourn", typeof(int));
				dt.Columns.Add("BeforeInMinute", typeof(int));
				dt.Columns.Add("AfterOutHourn", typeof(int));
				dt.Columns.Add("AfterOutMinute", typeof(int));
				dt.Columns.Add("ReprotTime", typeof(DateTime));
				dt.Columns.Add("ReportDay", typeof(int));
				dt.Columns.Add("ShortLeaveInStartTime", typeof(DateTime));
				dt.Columns.Add("ShortLeaveInEndTime", typeof(DateTime));
				dt.Columns.Add("ShortLeaveOutStartTime", typeof(DateTime));
				dt.Columns.Add("ShortLeaveOutEndTime", typeof(DateTime));
				dt.Columns.Add("HaldDayInStartTime", typeof(DateTime));
				dt.Columns.Add("HaldDayInEndTime", typeof(DateTime));
				dt.Columns.Add("HaldDayOutStartTime", typeof(DateTime));
				dt.Columns.Add("HaldDayOutEndTime", typeof(DateTime));
				dt.Columns.Add("GraceOutStartTime", typeof(DateTime));
				dt.Columns.Add("GraceOutEndTime", typeof(DateTime));
				dt.Columns.Add("AbsentAfterInStartTime", typeof(DateTime));
				dt.Columns.Add("AbsentBeforeOutTime", typeof(DateTime));
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["ShiftTimingId"], lst.Rows[i]["LocationName"], lst.Rows[i]["ShiftName"], lst.Rows[i]["TimingDescription"], lst.Rows[i]["ShiftLocationId"], lst.Rows[i]["WeekDayText"], Conversion.ToDateTime(lst.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(lst.Rows[i]["EndDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(lst.Rows[i]["StartTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["EndTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["StartBreakTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["EndBreakTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["GraceTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["GraceInEndTime"]).ToString("HH:mm"), lst.Rows[i]["BeforeInHours"], lst.Rows[i]["BeforeInMinutes"], lst.Rows[i]["AfterOutHours"], lst.Rows[i]["AfterOutMinutes"], Conversion.ToDateTime(lst.Rows[i]["ReportTime"]).ToString("HH:mm"), lst.Rows[i]["ReportDays"], Conversion.ToDateTime(lst.Rows[i]["ShortLeaveInStartTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["ShortLeaveInEndTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["ShortLeaveOutStartTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["ShortLeaveOutEndTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["HalfDayInStartTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["HalfDayInEndTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["HalfDayOutStartTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["HalfDayOutEndTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["GraceOutStartTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["GraceOutEndTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["AbsentAfterInTime"]).ToString("HH:mm"), Conversion.ToDateTime(lst.Rows[i]["AbsentBeforeOutTime"]).ToString("HH:mm"));
				}
				grdShift.DataSource = dt;
				grdShift.RetrieveStructure();
				BindSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSetting()
	{
		try
		{
			grdShift.RootTable.Columns["ShiftTimingDesc"].EditType = (EditType)0;
			grdShift.RootTable.Columns["LoacationName"].EditType = (EditType)0;
			grdShift.RootTable.Columns["ShiftName"].EditType = (EditType)0;
			grdShift.RootTable.Columns["Location"].EditType = (EditType)0;
			grdShift.RootTable.Columns["WeekDay"].EditType = (EditType)0;
			grdShift.RootTable.Columns["StartDate"].EditType = (EditType)0;
			grdShift.RootTable.Columns["EndDate"].EditType = (EditType)0;
			grdShift.RootTable.Columns["StartTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["EndTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["startBreaktime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["EndBreaktime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["GraceInTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["GraceEndTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["BeforeInHourn"].EditType = (EditType)0;
			grdShift.RootTable.Columns["BeforeInMinute"].EditType = (EditType)0;
			grdShift.RootTable.Columns["AfterOutHourn"].EditType = (EditType)0;
			grdShift.RootTable.Columns["AfterOutMinute"].EditType = (EditType)0;
			grdShift.RootTable.Columns["ReprotTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["ReportDay"].EditType = (EditType)0;
			grdShift.RootTable.Columns["ShortLeaveInStartTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["ShortLeaveInEndTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["ShortLeaveOutStartTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["ShortLeaveOutEndTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["HaldDayInStartTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["HaldDayInEndTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["HaldDayOutStartTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["HaldDayOutEndTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["GraceOutStartTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["GraceOutEndTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["AbsentAfterInStartTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["AbsentBeforeOutTime"].EditType = (EditType)0;
			grdShift.RootTable.Columns["ShiftTimingDesc"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["LoacationName"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["ShiftName"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["Location"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["WeekDay"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["StartDate"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["EndDate"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["StartTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["EndTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["startBreaktime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["EndBreaktime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["GraceInTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["GraceEndTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["BeforeInHourn"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["BeforeInMinute"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["AfterOutHourn"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["AfterOutMinute"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["ReprotTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["ReportDay"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["ShortLeaveInStartTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["ShortLeaveInEndTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["ShortLeaveOutStartTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["ShortLeaveOutEndTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["HaldDayInStartTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["HaldDayInEndTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["HaldDayOutStartTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["HaldDayOutEndTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["GraceOutStartTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["GraceOutEndTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["AbsentAfterInStartTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["AbsentBeforeOutTime"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns.Add("Edit");
			grdShift.RootTable.Columns["Edit"].Caption = "Edit";
			grdShift.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdShift.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdShift.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdShift.RootTable.Columns["Edit"].Width = 50;
			grdShift.RootTable.Columns.Add("SaveAs");
			grdShift.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grdShift.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdShift.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grdShift.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grdShift.RootTable.Columns["SaveAs"].Width = 60;
			grdShift.RootTable.Columns["Edit"].Position = 0;
			grdShift.RootTable.Columns["SaveAs"].Position = 1;
			grdShift.FrozenColumns = 2;
			CommonServices.GridAutoAdjustment(AryColWidth, grdShift);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdShift_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt((object)grdShift.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "SaveAs")
			{
				ReadById(Conversion.ToInt((object)grdShift.CurrentRow.Cells["Id"].Value.ToString()));
				btnSaveAs.Visible = true;
				btnupdate.Visible = false;
				btnsave.Visible = false;
				UpdateMode = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGenShiftTiming_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnnew_Click(null, null);
				}
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdShift).Name);
			if (fs != null)
			{
				grdShift.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void TotalShifthours()
	{
		try
		{
			double TotalDaysHours = 0.0;
			double TotalShiftHours = 0.0;
			double InHours = 0.0;
			double OutHours = 0.0;
			double ToalHours = 0.0;
			if (Convert.ToDouble(datEndTime.Value.Hour) > Convert.ToDouble(datStartTime.Value.Hour))
			{
				txtTotalShiftHours.Text = Convert.ToString(Convert.ToDouble(datEndTime.Value.Hour) - Convert.ToDouble(datStartTime.Value.Hour));
				if (txtTotalShiftHours.Text != string.Empty)
				{
					TotalShiftHours = Convert.ToDouble(txtTotalShiftHours.Text);
				}
				if (txtInHours.Text != string.Empty)
				{
					InHours = Convert.ToDouble(txtInHours.Text);
				}
				if (txtOutHours.Text != string.Empty)
				{
					OutHours = Convert.ToDouble(txtOutHours.Text);
				}
				if (InHours != 0.0 || OutHours != 0.0)
				{
					ToalHours = OutHours + InHours;
					txtTotalHours.Text = ToalHours.ToString();
					TotalDaysHours = TotalShiftHours + ToalHours;
				}
				if (txtTotalHours.Text != string.Empty)
				{
					txtTotalDaysHours.Text = TotalDaysHours.ToString();
				}
			}
			else
			{
				double hour = Convert.ToDouble(datEndTime.Value.Hour) - Convert.ToDouble(datStartTime.Value.Hour);
				TotalShiftHours = 24.0 + hour;
				txtTotalShiftHours.Text = Conversion.ToString((object)TotalShiftHours);
				if (txtInHours.Text != string.Empty)
				{
					InHours = Convert.ToDouble(txtInHours.Text);
				}
				if (txtOutHours.Text != string.Empty)
				{
					OutHours = Convert.ToDouble(txtOutHours.Text);
					txtTotalHours.Text = (OutHours + InHours).ToString();
					TotalDaysHours = TotalShiftHours + InHours + OutHours;
					txtTotalDaysHours.Text = TotalDaysHours.ToString();
				}
				if (txtTotalHours.Text != string.Empty)
				{
					txtTotalDaysHours.Text = TotalDaysHours.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datStartTime_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalShifthours();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datEndTime_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalShifthours();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtInHours_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalShifthours();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOutHours_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalShifthours();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public bool Validation()
	{
		if (((UltraGridBase)CmbShift).ActiveRow == null)
		{
			MessageBox.Show("Shift Required!");
			CmbShift.Focus();
			return false;
		}
		if (((Control)(object)CmbShiftLocation).Text == string.Empty)
		{
			MessageBox.Show("Shift Location Required!");
			CmbShiftLocation.Focus();
			return false;
		}
		if (txtDescription.Text == string.Empty)
		{
			MessageBox.Show("Description Required!");
			txtDescription.Focus();
			return false;
		}
		return true;
	}

	private void CheckShiftTiming(genShiftTiming obj)
	{
		try
		{
			_ = (obj.EndTime - obj.StartTime).TotalHours;
			double totalMinutes = obj.BeforeInMinutes + obj.AfterOutMinutes;
			double num = obj.BeforeInHours + obj.AfterOutHours;
			double hours = totalMinutes / 60.0;
			if (Math.Abs(num + hours) > 24.0)
			{
				throw new Exception("Shift timing must be in 24 hour range \t" + obj.TimingDescription);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void brnShift_Click(object sender, EventArgs e)
	{
		try
		{
			new frmGenShift(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShiftLocation_Click(object sender, EventArgs e)
	{
		try
		{
			new frmShiftLocation(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDutyRoasterForm_Click(object sender, EventArgs e)
	{
		try
		{
			frmDutyRoasterNew obj = new frmDutyRoasterNew(UserAccount);
			obj.formright = CommonServices.SetRightsValueInRightsObject("DutyRoaster");
			if (obj.formright.DoHaveViewRight)
			{
				obj.Show();
			}
			else
			{
				MessageBox.Show("You don't have the right to view this form...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGenShiftTiming_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Expected O, but got Unknown
		//IL_428a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4294: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
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
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.AttendanceManagement.frmGenShiftTiming));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.grd = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.brnShift = new System.Windows.Forms.Button();
		this.btnShiftLocation = new System.Windows.Forms.Button();
		this.CmbShiftLocation = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbShift = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.datEndDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.datStartDate = new System.Windows.Forms.DateTimePicker();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.ChkBoxIsClosedNextDay = new System.Windows.Forms.CheckBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtTotalDaysHours = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtTotalHours = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtOutHours = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.txtInHours = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.txtTotalShiftHours = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.datEndTime = new System.Windows.Forms.DateTimePicker();
		this.datEndBreakTime = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.datStartBreakTime = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.datStartTime = new System.Windows.Forms.DateTimePicker();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.datDepartureEndTime = new System.Windows.Forms.DateTimePicker();
		this.datShortLeaveOutEndTime = new System.Windows.Forms.DateTimePicker();
		this.datHalfDayOutEndTime = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.datAbsentBeforeOutTime = new System.Windows.Forms.DateTimePicker();
		this.datDepartureStarTime = new System.Windows.Forms.DateTimePicker();
		this.datShortLeaveOutStartTime = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.datHalfDayOutStartTime = new System.Windows.Forms.DateTimePicker();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.datLateEndTime = new System.Windows.Forms.DateTimePicker();
		this.datHalfDayInEndTime = new System.Windows.Forms.DateTimePicker();
		this.label37 = new System.Windows.Forms.Label();
		this.datLateStartTime = new System.Windows.Forms.DateTimePicker();
		this.label36 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.datShortLeaveInStartTime = new System.Windows.Forms.DateTimePicker();
		this.datAbsentAfterInTime = new System.Windows.Forms.DateTimePicker();
		this.label21 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.datShortLeaveInEndTime = new System.Windows.Forms.DateTimePicker();
		this.label24 = new System.Windows.Forms.Label();
		this.datHalfDayInStartTime = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnDutyRoasterForm = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdShift = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label12 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShiftLocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShift).BeginInit();
		this.groupBox3.SuspendLayout();
		this.groupBox5.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdShift).BeginInit();
		this.panel7.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(961, 552);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(961, 552);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(953, 526);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(947, 520);
		this.panel2.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(947, 491);
		this.panel4.TabIndex = 1;
		this.panel9.Controls.Add(this.groupBox2);
		this.panel9.Controls.Add(this.groupBox1);
		this.panel9.Controls.Add(this.groupBox3);
		this.panel9.Controls.Add(this.groupBox5);
		this.panel9.Controls.Add(this.groupBox4);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 29);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(947, 462);
		this.panel9.TabIndex = 151;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(688, 6);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(257, 450);
		this.groupBox2.TabIndex = 4;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "WeekDays";
		this.grd.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 19);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(251, 428);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.brnShift);
		this.groupBox1.Controls.Add(this.btnShiftLocation);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShiftLocation);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShift);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.datEndDate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.datStartDate);
		this.groupBox1.Controls.Add(this.txtDescription);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.ChkBoxIsClosedNextDay);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(16, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(668, 101);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.brnShift.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.brnShift.Location = new System.Drawing.Point(313, 15);
		this.brnShift.Name = "brnShift";
		this.brnShift.Size = new System.Drawing.Size(28, 26);
		this.brnShift.TabIndex = 1;
		this.brnShift.TabStop = false;
		this.brnShift.UseVisualStyleBackColor = true;
		this.brnShift.Click += new System.EventHandler(brnShift_Click);
		this.btnShiftLocation.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnShiftLocation.Location = new System.Drawing.Point(313, 45);
		this.btnShiftLocation.Name = "btnShiftLocation";
		this.btnShiftLocation.Size = new System.Drawing.Size(28, 26);
		this.btnShiftLocation.TabIndex = 3;
		this.btnShiftLocation.TabStop = false;
		this.btnShiftLocation.UseVisualStyleBackColor = true;
		this.btnShiftLocation.Click += new System.EventHandler(btnShiftLocation_Click);
		this.CmbShiftLocation.AllowNull = (DefaultableBoolean)1;
		this.CmbShiftLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShiftLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShiftLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShiftLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbShiftLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShiftLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShiftLocation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShiftLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShiftLocation).Location = new System.Drawing.Point(87, 45);
		((System.Windows.Forms.Control)(object)this.CmbShiftLocation).Name = "CmbShiftLocation";
		((System.Windows.Forms.Control)(object)this.CmbShiftLocation).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbShiftLocation).TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 51);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(81, 15);
		this.label10.TabIndex = 204;
		this.label10.Text = "Shift Location";
		this.CmbShift.AllowNull = (DefaultableBoolean)1;
		this.CmbShift.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShift.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShift.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShift).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbShift).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShift).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShift).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShift).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbShift).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShift).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbShift).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShift).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbShift).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShift).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShift).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShift).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShift.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShift).Location = new System.Drawing.Point(87, 16);
		((System.Windows.Forms.Control)(object)this.CmbShift).Name = "CmbShift";
		((System.Windows.Forms.Control)(object)this.CmbShift).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbShift).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbShift).Leave += new System.EventHandler(CmbShift_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(5, 22);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(32, 15);
		this.label30.TabIndex = 177;
		this.label30.Text = "Shift";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(347, 51);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(55, 15);
		this.label4.TabIndex = 150;
		this.label4.Text = "End Date";
		this.datEndDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.datEndDate.Location = new System.Drawing.Point(410, 47);
		this.datEndDate.Name = "datEndDate";
		this.datEndDate.Size = new System.Drawing.Size(131, 23);
		this.datEndDate.TabIndex = 6;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(347, 22);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(60, 15);
		this.label3.TabIndex = 148;
		this.label3.Text = "Start Date";
		this.datStartDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.datStartDate.Location = new System.Drawing.Point(410, 18);
		this.datStartDate.Name = "datStartDate";
		this.datStartDate.Size = new System.Drawing.Size(131, 23);
		this.datStartDate.TabIndex = 5;
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(87, 74);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(454, 23);
		this.txtDescription.TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 78);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(68, 15);
		this.label1.TabIndex = 144;
		this.label1.Text = "Description";
		this.ChkBoxIsClosedNextDay.AutoSize = true;
		this.ChkBoxIsClosedNextDay.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxIsClosedNextDay.Location = new System.Drawing.Point(549, 74);
		this.ChkBoxIsClosedNextDay.Name = "ChkBoxIsClosedNextDay";
		this.ChkBoxIsClosedNextDay.Size = new System.Drawing.Size(113, 19);
		this.ChkBoxIsClosedNextDay.TabIndex = 7;
		this.ChkBoxIsClosedNextDay.Text = "Next Day Closed";
		this.ChkBoxIsClosedNextDay.UseVisualStyleBackColor = true;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.txtTotalDaysHours);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.txtTotalHours);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add(this.txtOutHours);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add(this.txtInHours);
		this.groupBox3.Controls.Add(this.label32);
		this.groupBox3.Controls.Add(this.txtTotalShiftHours);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.datEndTime);
		this.groupBox3.Controls.Add(this.datEndBreakTime);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add(this.datStartBreakTime);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.datStartTime);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(16, 109);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(668, 121);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Duty Timing";
		this.txtTotalDaysHours.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalDaysHours.Location = new System.Drawing.Point(399, 88);
		this.txtTotalDaysHours.Name = "txtTotalDaysHours";
		this.txtTotalDaysHours.ReadOnly = true;
		this.txtTotalDaysHours.Size = new System.Drawing.Size(260, 23);
		this.txtTotalDaysHours.TabIndex = 8;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(399, 69);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(97, 15);
		this.label35.TabIndex = 206;
		this.label35.Text = "Total Days Hours";
		this.txtTotalHours.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalHours.Location = new System.Drawing.Point(268, 88);
		this.txtTotalHours.Name = "txtTotalHours";
		this.txtTotalHours.ReadOnly = true;
		this.txtTotalHours.Size = new System.Drawing.Size(127, 23);
		this.txtTotalHours.TabIndex = 7;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(268, 69);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(68, 15);
		this.label34.TabIndex = 206;
		this.label34.Text = "Total Hours";
		this.txtOutHours.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutHours.Location = new System.Drawing.Point(136, 88);
		this.txtOutHours.Name = "txtOutHours";
		this.txtOutHours.Size = new System.Drawing.Size(127, 23);
		this.txtOutHours.TabIndex = 6;
		this.txtOutHours.TextChanged += new System.EventHandler(txtOutHours_TextChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(136, 69);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(62, 15);
		this.label33.TabIndex = 206;
		this.label33.Text = "Out Hours";
		this.txtInHours.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInHours.Location = new System.Drawing.Point(6, 88);
		this.txtInHours.Name = "txtInHours";
		this.txtInHours.Size = new System.Drawing.Size(127, 23);
		this.txtInHours.TabIndex = 5;
		this.txtInHours.TextChanged += new System.EventHandler(txtInHours_TextChanged);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(6, 69);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(53, 15);
		this.label32.TabIndex = 206;
		this.label32.Text = "In Hours";
		this.txtTotalShiftHours.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalShiftHours.Location = new System.Drawing.Point(530, 41);
		this.txtTotalShiftHours.Name = "txtTotalShiftHours";
		this.txtTotalShiftHours.ReadOnly = true;
		this.txtTotalShiftHours.Size = new System.Drawing.Size(129, 23);
		this.txtTotalShiftHours.TabIndex = 4;
		this.txtTotalShiftHours.TabStop = false;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(532, 22);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(67, 15);
		this.label31.TabIndex = 204;
		this.label31.Text = "Shift Hours";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(399, 22);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 196;
		this.label9.Text = "End Time";
		this.datEndTime.CustomFormat = "hh:mm tt";
		this.datEndTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datEndTime.Location = new System.Drawing.Point(399, 41);
		this.datEndTime.Name = "datEndTime";
		this.datEndTime.Size = new System.Drawing.Size(127, 23);
		this.datEndTime.TabIndex = 3;
		this.datEndTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.datEndTime.Leave += new System.EventHandler(datEndTime_Leave);
		this.datEndBreakTime.CustomFormat = "hh:mm tt";
		this.datEndBreakTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEndBreakTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datEndBreakTime.Location = new System.Drawing.Point(268, 41);
		this.datEndBreakTime.Name = "datEndBreakTime";
		this.datEndBreakTime.Size = new System.Drawing.Size(127, 23);
		this.datEndBreakTime.TabIndex = 2;
		this.datEndBreakTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(268, 22);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(89, 15);
		this.label7.TabIndex = 195;
		this.label7.Text = "End Break Time";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(136, 22);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(94, 15);
		this.label8.TabIndex = 194;
		this.label8.Text = "Start Break Time";
		this.datStartBreakTime.CustomFormat = "hh:mm tt";
		this.datStartBreakTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datStartBreakTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datStartBreakTime.Location = new System.Drawing.Point(136, 41);
		this.datStartBreakTime.Name = "datStartBreakTime";
		this.datStartBreakTime.Size = new System.Drawing.Size(127, 23);
		this.datStartBreakTime.TabIndex = 1;
		this.datStartBreakTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 22);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(62, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Start Time";
		this.datStartTime.CustomFormat = "hh:mm tt";
		this.datStartTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datStartTime.Location = new System.Drawing.Point(6, 41);
		this.datStartTime.Name = "datStartTime";
		this.datStartTime.Size = new System.Drawing.Size(127, 23);
		this.datStartTime.TabIndex = 0;
		this.datStartTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.datStartTime.Leave += new System.EventHandler(datStartTime_Leave);
		this.groupBox5.BackColor = System.Drawing.Color.White;
		this.groupBox5.Controls.Add(this.datDepartureEndTime);
		this.groupBox5.Controls.Add(this.datShortLeaveOutEndTime);
		this.groupBox5.Controls.Add(this.datHalfDayOutEndTime);
		this.groupBox5.Controls.Add(this.label5);
		this.groupBox5.Controls.Add(this.label23);
		this.groupBox5.Controls.Add(this.datAbsentBeforeOutTime);
		this.groupBox5.Controls.Add(this.datDepartureStarTime);
		this.groupBox5.Controls.Add(this.datShortLeaveOutStartTime);
		this.groupBox5.Controls.Add(this.label2);
		this.groupBox5.Controls.Add(this.label27);
		this.groupBox5.Controls.Add(this.label22);
		this.groupBox5.Controls.Add(this.label29);
		this.groupBox5.Controls.Add(this.label26);
		this.groupBox5.Controls.Add(this.datHalfDayOutStartTime);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.ForeColor = System.Drawing.Color.Black;
		this.groupBox5.Location = new System.Drawing.Point(353, 236);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(331, 220);
		this.groupBox5.TabIndex = 3;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "OutTime";
		this.datDepartureEndTime.CustomFormat = "hh:mm tt";
		this.datDepartureEndTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDepartureEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDepartureEndTime.Location = new System.Drawing.Point(172, 186);
		this.datDepartureEndTime.Name = "datDepartureEndTime";
		this.datDepartureEndTime.Size = new System.Drawing.Size(150, 23);
		this.datDepartureEndTime.TabIndex = 6;
		this.datDepartureEndTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.datShortLeaveOutEndTime.CustomFormat = "hh:mm tt";
		this.datShortLeaveOutEndTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datShortLeaveOutEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datShortLeaveOutEndTime.Location = new System.Drawing.Point(172, 140);
		this.datShortLeaveOutEndTime.Name = "datShortLeaveOutEndTime";
		this.datShortLeaveOutEndTime.Size = new System.Drawing.Size(150, 23);
		this.datShortLeaveOutEndTime.TabIndex = 4;
		this.datShortLeaveOutEndTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.datHalfDayOutEndTime.CustomFormat = "hh:mm tt";
		this.datHalfDayOutEndTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datHalfDayOutEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datHalfDayOutEndTime.Location = new System.Drawing.Point(172, 90);
		this.datHalfDayOutEndTime.Name = "datHalfDayOutEndTime";
		this.datHalfDayOutEndTime.Size = new System.Drawing.Size(150, 23);
		this.datHalfDayOutEndTime.TabIndex = 2;
		this.datHalfDayOutEndTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(172, 168);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(113, 15);
		this.label5.TabIndex = 157;
		this.label5.Text = "Departure End Time";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(172, 121);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(122, 15);
		this.label23.TabIndex = 157;
		this.label23.Text = "Short Leave End Time";
		this.datAbsentBeforeOutTime.CustomFormat = "hh:mm tt";
		this.datAbsentBeforeOutTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datAbsentBeforeOutTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datAbsentBeforeOutTime.Location = new System.Drawing.Point(9, 42);
		this.datAbsentBeforeOutTime.Name = "datAbsentBeforeOutTime";
		this.datAbsentBeforeOutTime.Size = new System.Drawing.Size(151, 23);
		this.datAbsentBeforeOutTime.TabIndex = 0;
		this.datAbsentBeforeOutTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.datDepartureStarTime.CustomFormat = "hh:mm tt";
		this.datDepartureStarTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDepartureStarTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDepartureStarTime.Location = new System.Drawing.Point(9, 186);
		this.datDepartureStarTime.Name = "datDepartureStarTime";
		this.datDepartureStarTime.Size = new System.Drawing.Size(151, 23);
		this.datDepartureStarTime.TabIndex = 5;
		this.datDepartureStarTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.datShortLeaveOutStartTime.CustomFormat = "hh:mm tt";
		this.datShortLeaveOutStartTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datShortLeaveOutStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datShortLeaveOutStartTime.Location = new System.Drawing.Point(9, 140);
		this.datShortLeaveOutStartTime.Name = "datShortLeaveOutStartTime";
		this.datShortLeaveOutStartTime.Size = new System.Drawing.Size(151, 23);
		this.datShortLeaveOutStartTime.TabIndex = 3;
		this.datShortLeaveOutStartTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 168);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(118, 15);
		this.label2.TabIndex = 162;
		this.label2.Text = "Departure Start Time";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(172, 70);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(106, 15);
		this.label27.TabIndex = 154;
		this.label27.Text = "Half Day End Time";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(9, 121);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(127, 15);
		this.label22.TabIndex = 162;
		this.label22.Text = "Short Leave Start Time";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(9, 23);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(134, 15);
		this.label29.TabIndex = 156;
		this.label29.Text = "Absent Before Out Time";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(9, 70);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(111, 15);
		this.label26.TabIndex = 158;
		this.label26.Text = "Half Day Start Time";
		this.datHalfDayOutStartTime.CustomFormat = "hh:mm tt";
		this.datHalfDayOutStartTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datHalfDayOutStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datHalfDayOutStartTime.Location = new System.Drawing.Point(9, 90);
		this.datHalfDayOutStartTime.Name = "datHalfDayOutStartTime";
		this.datHalfDayOutStartTime.Size = new System.Drawing.Size(151, 23);
		this.datHalfDayOutStartTime.TabIndex = 1;
		this.datHalfDayOutStartTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.groupBox4.BackColor = System.Drawing.Color.White;
		this.groupBox4.Controls.Add(this.datLateEndTime);
		this.groupBox4.Controls.Add(this.datHalfDayInEndTime);
		this.groupBox4.Controls.Add(this.label37);
		this.groupBox4.Controls.Add(this.datLateStartTime);
		this.groupBox4.Controls.Add(this.label36);
		this.groupBox4.Controls.Add(this.label20);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.datShortLeaveInStartTime);
		this.groupBox4.Controls.Add(this.datAbsentAfterInTime);
		this.groupBox4.Controls.Add(this.label21);
		this.groupBox4.Controls.Add(this.label28);
		this.groupBox4.Controls.Add(this.datShortLeaveInEndTime);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.datHalfDayInStartTime);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(16, 236);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(325, 220);
		this.groupBox4.TabIndex = 2;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "In Time";
		this.datLateEndTime.CustomFormat = "hh:mm tt";
		this.datLateEndTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datLateEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datLateEndTime.Location = new System.Drawing.Point(173, 42);
		this.datLateEndTime.Name = "datLateEndTime";
		this.datLateEndTime.Size = new System.Drawing.Size(147, 23);
		this.datLateEndTime.TabIndex = 1;
		this.datLateEndTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.datHalfDayInEndTime.CustomFormat = "hh:mm tt";
		this.datHalfDayInEndTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datHalfDayInEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datHalfDayInEndTime.Location = new System.Drawing.Point(173, 140);
		this.datHalfDayInEndTime.Name = "datHalfDayInEndTime";
		this.datHalfDayInEndTime.Size = new System.Drawing.Size(147, 23);
		this.datHalfDayInEndTime.TabIndex = 5;
		this.datHalfDayInEndTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(173, 23);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(82, 15);
		this.label37.TabIndex = 157;
		this.label37.Text = "Late End Time";
		this.datLateStartTime.CustomFormat = "hh:mm tt";
		this.datLateStartTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datLateStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datLateStartTime.Location = new System.Drawing.Point(9, 42);
		this.datLateStartTime.Name = "datLateStartTime";
		this.datLateStartTime.Size = new System.Drawing.Size(156, 23);
		this.datLateStartTime.TabIndex = 0;
		this.datLateStartTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(6, 23);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(87, 15);
		this.label36.TabIndex = 155;
		this.label36.Text = "Late Start Time";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(9, 70);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(127, 15);
		this.label20.TabIndex = 153;
		this.label20.Text = "Short Leave Start Time";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(173, 121);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(106, 15);
		this.label25.TabIndex = 155;
		this.label25.Text = "Half Day End Time";
		this.datShortLeaveInStartTime.CustomFormat = "hh:mm tt";
		this.datShortLeaveInStartTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datShortLeaveInStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datShortLeaveInStartTime.Location = new System.Drawing.Point(9, 90);
		this.datShortLeaveInStartTime.Name = "datShortLeaveInStartTime";
		this.datShortLeaveInStartTime.Size = new System.Drawing.Size(156, 23);
		this.datShortLeaveInStartTime.TabIndex = 2;
		this.datShortLeaveInStartTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.datAbsentAfterInTime.CustomFormat = "hh:mm tt";
		this.datAbsentAfterInTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datAbsentAfterInTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datAbsentAfterInTime.Location = new System.Drawing.Point(9, 186);
		this.datAbsentAfterInTime.Name = "datAbsentAfterInTime";
		this.datAbsentAfterInTime.Size = new System.Drawing.Size(156, 23);
		this.datAbsentAfterInTime.TabIndex = 6;
		this.datAbsentAfterInTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(173, 70);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(122, 15);
		this.label21.TabIndex = 159;
		this.label21.Text = "Short Leave End Time";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(9, 168);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(117, 15);
		this.label28.TabIndex = 161;
		this.label28.Text = "Absent After In Time";
		this.datShortLeaveInEndTime.CustomFormat = "hh:mm tt";
		this.datShortLeaveInEndTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datShortLeaveInEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datShortLeaveInEndTime.Location = new System.Drawing.Point(173, 90);
		this.datShortLeaveInEndTime.Name = "datShortLeaveInEndTime";
		this.datShortLeaveInEndTime.Size = new System.Drawing.Size(147, 23);
		this.datShortLeaveInEndTime.TabIndex = 3;
		this.datShortLeaveInEndTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(9, 121);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(111, 15);
		this.label24.TabIndex = 160;
		this.label24.Text = "Half Day Start Time";
		this.datHalfDayInStartTime.CustomFormat = "hh:mm tt";
		this.datHalfDayInStartTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datHalfDayInStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datHalfDayInStartTime.Location = new System.Drawing.Point(9, 140);
		this.datHalfDayInStartTime.Name = "datHalfDayInStartTime";
		this.datHalfDayInStartTime.Size = new System.Drawing.Size(156, 23);
		this.datHalfDayInStartTime.TabIndex = 4;
		this.datHalfDayInStartTime.Value = new System.DateTime(2023, 7, 20, 18, 41, 54, 0);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(947, 29);
		this.panel8.TabIndex = 150;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(5, 5);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(221, 19);
		this.label11.TabIndex = 34;
		this.label11.Text = "Shift Timing Define           ";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(947, 29);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnsave, this.btnSaveAs, this.btnupdate, this.btnPrint, this.toolStripButton1, this.toolStripSeparator1, this.btnDutyRoasterForm });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(947, 27);
		this.toolStrip1.TabIndex = 9;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.toolStripButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(78, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnDutyRoasterForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDutyRoasterForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDutyRoasterForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDutyRoasterForm.Name = "btnDutyRoasterForm";
		this.btnDutyRoasterForm.Size = new System.Drawing.Size(97, 24);
		this.btnDutyRoasterForm.Text = "Duty Roaster";
		this.btnDutyRoasterForm.Click += new System.EventHandler(btnDutyRoasterForm_Click);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(953, 526);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(947, 520);
		this.panel5.TabIndex = 0;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdShift);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 58);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(947, 462);
		this.panel10.TabIndex = 7;
		this.grdShift.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdShift).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdShift.FilterMode = (FilterMode)2;
		this.grdShift.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdShift).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdShift.GroupTotals = (GroupTotals)2;
		this.grdShift.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdShift).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdShift).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdShift).Name = "grdShift";
		((ScrollableBase)this.grdShift).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdShift).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdShift).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdShift).Size = new System.Drawing.Size(947, 462);
		((System.Windows.Forms.Control)(object)this.grdShift).TabIndex = 6;
		this.grdShift.TotalRow = (InheritableBoolean)1;
		this.grdShift.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdShift).VisualStyle = (VisualStyle)4;
		this.grdShift.ColumnButtonClick += new ColumnActionEventHandler(grdShift_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdShift).DoubleClick += new System.EventHandler(grdShift_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label12);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 31);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(947, 27);
		this.panel7.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(899, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdShift;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 11;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(5, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(196, 19);
		this.label12.TabIndex = 35;
		this.label12.Text = "Shift Timing History     ";
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(947, 31);
		this.panel6.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripSeparator2, this.toolStripButton4 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(947, 25);
		this.toolStrip2.TabIndex = 9;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.toolStripButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(57, 24);
		this.toolStripButton4.Text = "&Print";
		this.toolStripButton4.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(961, 552);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmGenShiftTiming";
		this.Text = "frmGenShiftTiming";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmGenShiftTiming_FormClosing);
		base.Load += new System.EventHandler(frmGenShiftTiming_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGenShiftTiming_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShiftLocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShift).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdShift).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
