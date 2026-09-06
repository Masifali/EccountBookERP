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
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.AttendanceManagement;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.EmployeeManagement;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.AttendanceManagement;

public class frmDailyAttendance : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtgrid = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private int Id;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel6;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnRefresh;

	private DateTimePicker txtDate;

	internal UltraCombo cmbDepartment;

	private Label label1;

	private Button btnLoad;

	internal UltraCombo cmbSection;

	internal UltraCombo cmbShift;

	private Label label10;

	private Label label9;

	private Label label2;

	internal GridEX grd;

	internal UltraCombo cmbEmployee;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnGenerate;

	private Label label4;

	private Label label5;

	private DateTimePicker txtOutTime;

	private DateTimePicker txtInTime;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private Label label12;

	private Label label11;

	private Label label6;

	internal UltraCombo CmbEmployeeGroup;

	internal UltraCombo CmbEmployeeCategory;

	internal UltraCombo CmbLocation;

	internal UltraCombo CmbDesignation;

	private Label label7;

	private CheckBox ChkRestDay;

	private Panel panel5;

	private Label label23;

	private Panel panel8;

	private Panel panel7;

	private Label label8;

	private Button btnEmployeeDefine;

	public frmDailyAttendance(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmDailyAttendance_Load(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(980, 500);
			formright = CommonServices.SetRightsValueInRightsObject("DailyAttendance");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtgrid.Columns.Add("ManualAttendanceId");
			dtgrid.Columns.Add("Day");
			dtgrid.Columns.Add("EmployeeNo");
			dtgrid.Columns.Add("EmployeeId");
			dtgrid.Columns.Add("LocationId");
			dtgrid.Columns.Add("DepartmentId");
			dtgrid.Columns.Add("ShiftId");
			dtgrid.Columns.Add("EmployeeName");
			dtgrid.Columns.Add("DutyDate");
			dtgrid.Columns.Add("ShiftTiming");
			dtgrid.Columns.Add("InTime", typeof(DateTime));
			dtgrid.Columns.Add("OutTime", typeof(DateTime));
			dtgrid.Columns.Add("InEntryMode");
			dtgrid.Columns.Add("OutEntryMode");
			dtgrid.Columns.Add("Status");
			dtgrid.Columns.Add("Description");
			dtgrid.Columns.Add("FlagInTime");
			dtgrid.Columns.Add("FlagOutTime");
			dtgrid.Columns.Add("IsPresent");
			dtgrid.Columns.Add("IsLeave");
			dtgrid.Columns.Add("IsHoliday");
			dtgrid.Columns.Add("IsSuspended");
			dtgrid.Columns.Add("IsTerminate");
			dtgrid.Columns.Add("CPLQuota");
			dtgrid.Columns.Add("IsRestDay", typeof(bool));
			dtgrid.Columns.Add("IsAbsent", typeof(bool));
			dtgrid.Columns.Add("IsCPL", typeof(bool));
			grd.DataSource = dtgrid;
			grd.RetrieveStructure();
			grdSetting();
			ShiftNameComboFill();
			SectionFill();
			DepartmentNameComboFill();
			EmployeeName();
			LocationFilterFill();
			DesignationNameComboFill();
			EmployeeGroupComboFill();
			EmployeeCategoryFill();
			txtDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShiftNameComboFill()
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
				DDL.BindDDLNew(dt, cmbShift, "ShiftId", "ShiftName", "ShiftName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepartmentNameComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			genDepartment val = new genDepartment
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dt = new DataTable
			{
				Columns = { "Id", "DepartmentName" }
			};
			DataTable dtDepartment = genDepartment.Getall(val);
			for (int i = 0; i < dtDepartment.Rows.Count; i++)
			{
				dt.Rows.Add(dtDepartment.Rows[i]["DepartmentId"], dtDepartment.Rows[i]["DepartmentName"]);
				DDL.BindDDL(dt, cmbDepartment, "Id", "DepartmentName", "Department Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		try
		{
			DataTable dtemployee = genEmployeeHistory.GetAllEmployeesActive(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SectionId = Conversion.ToInt(cmbSection.Value),
				ShiftId = Conversion.ToInt(cmbShift.Value),
				EmployeeCategoryId = Conversion.ToInt(CmbEmployeeCategory.Value),
				EmployeeGroupId = Conversion.ToInt(CmbEmployeeGroup.Value),
				PartyLocationId = Conversion.ToInt(CmbLocation.Value),
				DesignationId = Conversion.ToInt(CmbDesignation.Value),
				DepartmentId = Conversion.ToInt(cmbDepartment.Value)
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EmployeeName");
			for (int i = 0; i < dtemployee.Rows.Count; i++)
			{
				dt.Rows.Add(dtemployee.Rows[i]["EmployeeId"], dtemployee.Rows[i]["EmployeeName"]);
			}
			DDL.BindDDL(dt, cmbEmployee, "Id", "EmployeeName", "Employee", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SectionFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtSection = genSection.Getall(new genSection
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtSection.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSection, cmbSection, "SectionId", "SectionName", "Section Name", false);
				((UltraGridBase)cmbSection).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationFilterFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtLocation = genLocation.Getall(new genLocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtLocation.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtLocation, CmbLocation, "LocationId", "LocationName", "Location Name", false);
				((UltraGridBase)CmbLocation).DisplayLayout.Bands[0].Columns[1].Width = 179;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DesignationNameComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDesignation = genDesignation.Getall(new genDesignation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDesignation.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDesignation, CmbDesignation, "DesignationId", "DesignationName", "Designation Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeGroupComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			DataTable dtGroup = genEmployeeGroup.Getall(new genEmployeeGroup
			{
				OrginizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGroup.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtGroup, CmbEmployeeGroup, "EmployeeGroupId", "EmployeeGroupName", "Employee Group", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeCategoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			DataTable dt = genEmployeeCategory.Getall(new genEmployeeCategory
			{
				OrginizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbEmployeeCategory, "EmployeeCategoryId", "EmployeeCategoryName", "Employee Category", false);
				((UltraGridBase)CmbEmployeeCategory).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtgrid.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.SectionId = Conversion.ToInt(cmbSection.Value);
			obj.EmployeeId = Conversion.ToInt(cmbEmployee.Value);
			obj.DepartmentId = Conversion.ToInt(cmbDepartment.Value);
			obj.DesignationId = Conversion.ToInt(CmbDesignation.Value);
			obj.ShiftId = Conversion.ToInt(cmbShift.Value);
			obj.PartyLocationId = Conversion.ToInt(CmbLocation.Value);
			obj.EmployeeGroupId = Conversion.ToInt(CmbEmployeeGroup.Value);
			obj.EmployeeCategoryId = Conversion.ToInt(CmbEmployeeCategory.Value);
			obj.DocDate = Conversion.ToDateTime((object)txtDate.Value);
			if (ChkRestDay.Checked)
			{
				obj.IsRestDay = true;
			}
			else
			{
				obj.ApprovedFilter = "All";
			}
			DataTable dt = hrmManualAttendance.genEmployeeAttendanceLoad(obj);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtgrid.Rows.Add(dt.Rows[i]["ManualAttendanceId"], Conversion.ToDateTime(dt.Rows[i]["DutyDate"]).DayOfWeek, dt.Rows[i]["EmployeeNo"], dt.Rows[i]["EmployeeId"], dt.Rows[i]["LocationId"], dt.Rows[i]["DepartmentId"], dt.Rows[i]["ShiftId"], dt.Rows[i]["EmployeeName"], dt.Rows[i]["DutyDate"], dt.Rows[i]["Timing"], dt.Rows[i]["InDateTime"], dt.Rows[i]["OutDateTime"], dt.Rows[i]["InEntryMode"], dt.Rows[i]["OutEntryMode"], "", dt.Rows[i]["Decsription"], "", "", dt.Rows[i]["IsPresent"], dt.Rows[i]["IsLeave"], dt.Rows[i]["IsHoliday"], dt.Rows[i]["IsSuspended"], dt.Rows[i]["IsTerminate"], dt.Rows[i]["CPLQuota"], dt.Rows[i]["IsRestDay"], dt.Rows[i]["IsAbsent"], dt.Rows[i]["IsCPL"]);
				}
				grd.DataSource = dtgrid;
				grd.RetrieveStructure();
				grdSetting();
			}
			else
			{
				grd.ClearStructure();
				MessageBox.Show("Record Not Found...");
			}
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
			grd.AllowEdit = (InheritableBoolean)1;
			grd.RootTable.Columns["InTime"].FormatString = "hh:mm tt";
			grd.RootTable.Columns["OutTime"].FormatString = "hh:mm tt";
			grd.RootTable.Columns["ManualAttendanceId"].Visible = false;
			grd.RootTable.Columns["Status"].Visible = false;
			grd.RootTable.Columns["IsTerminate"].Visible = false;
			grd.RootTable.Columns["IsSuspended"].Visible = false;
			grd.RootTable.Columns["IsHoliday"].Visible = false;
			grd.RootTable.Columns["IsLeave"].Visible = false;
			grd.RootTable.Columns["IsPresent"].Visible = false;
			grd.RootTable.Columns["EmployeeId"].Visible = false;
			grd.RootTable.Columns["LocationId"].Visible = false;
			grd.RootTable.Columns["DepartmentId"].Visible = false;
			grd.RootTable.Columns["ShiftId"].Visible = false;
			grd.RootTable.Columns["FlagInTime"].Visible = false;
			grd.RootTable.Columns["FlagOutTime"].Visible = false;
			grd.RootTable.Columns["IsCPL"].Visible = false;
			grd.RootTable.Columns["Day"].EditType = (EditType)0;
			grd.RootTable.Columns["EmployeeNo"].EditType = (EditType)0;
			grd.RootTable.Columns["EmployeeId"].EditType = (EditType)0;
			grd.RootTable.Columns["LocationId"].EditType = (EditType)0;
			grd.RootTable.Columns["DepartmentId"].EditType = (EditType)0;
			grd.RootTable.Columns["ShiftId"].EditType = (EditType)0;
			grd.RootTable.Columns["EmployeeName"].EditType = (EditType)0;
			grd.RootTable.Columns["DutyDate"].EditType = (EditType)0;
			grd.RootTable.Columns["ShiftTiming"].EditType = (EditType)0;
			grd.RootTable.Columns["FlagInTime"].EditType = (EditType)0;
			grd.RootTable.Columns["FlagOutTime"].EditType = (EditType)0;
			grd.RootTable.Columns["IsPresent"].EditType = (EditType)0;
			grd.RootTable.Columns["IsLeave"].EditType = (EditType)0;
			grd.RootTable.Columns["IsHoliday"].EditType = (EditType)0;
			grd.RootTable.Columns["IsSuspended"].EditType = (EditType)0;
			grd.RootTable.Columns["IsTerminate"].EditType = (EditType)0;
			grd.RootTable.Columns["CPLQuota"].EditType = (EditType)1;
			grd.RootTable.Columns["InEntryMode"].EditType = (EditType)0;
			grd.RootTable.Columns["OutEntryMode"].EditType = (EditType)0;
			grd.RootTable.Columns["ManualAttendanceId"].EditType = (EditType)0;
			grd.RootTable.Columns["IsAbsent"].EditType = (EditType)8;
			grd.RootTable.Columns["IsCPL"].EditType = (EditType)8;
			grd.RootTable.Columns["IsRestDay"].EditType = (EditType)8;
			grd.RootTable.Columns["IsRestDay"].EditType = (EditType)0;
			GridFormattingFuntion();
			grd.RootTable.Columns["InTime"].EditType = (EditType)3;
			grd.RootTable.Columns["OutTime"].EditType = (EditType)3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFormattingFuntion()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		try
		{
			for (int i = 0; i < grd.RowCount; i++)
			{
				bool num = Conversion.ToBool(grd.GetRow(i).Cells["IsRestDay"].Value);
				bool IsLeave = Conversion.ToBool(grd.GetRow(i).Cells["IsLeave"].Value);
				bool IsHoliday = Conversion.ToBool(grd.GetRow(i).Cells["IsHoliday"].Value);
				if (num)
				{
					GridEXFormatStyle rowcol3 = new GridEXFormatStyle();
					rowcol3.BackColor = Color.FromArgb(228, 194, 146);
					grd.GetRow(i).RowStyle = rowcol3;
				}
				if (IsHoliday)
				{
					GridEXFormatStyle rowcol2 = new GridEXFormatStyle();
					rowcol2.BackColor = Color.FromArgb(211, 208, 69);
					grd.GetRow(i).RowStyle = rowcol2;
				}
				if (IsLeave)
				{
					GridEXFormatStyle rowcol = new GridEXFormatStyle();
					rowcol.BackColor = Color.FromArgb(150, 226, 188);
					grd.GetRow(i).RowStyle = rowcol;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "InTime")
			{
				if (!(item.Cells["InEntryMode"].Text != "A"))
				{
					throw new Exception("You cannot change Auto Attendance Please Check");
				}
				item.BeginEdit();
				item.Cells["InEntryMode"].Value = "M";
				item.Cells["FlagInTime"].Value = 1;
				item.Cells["IsAbsent"].Value = 0;
				item.EndEdit();
			}
			if (e.Column.Key == "OutTime")
			{
				if (!(item.Cells["OutEntryMode"].Text != "A"))
				{
					throw new Exception("You cannot change Auto Attendance Please Check");
				}
				item.BeginEdit();
				item.Cells["OutEntryMode"].Value = "M";
				item.Cells["FlagOutTime"].Value = 1;
				item.Cells["IsAbsent"].Value = 0;
				item.EndEdit();
			}
			if (e.Column.Key == "IsCPL")
			{
				if (!Conversion.ToBool(item.Cells["IsRestDay"].Value))
				{
					item.Cells["IsCPL"].Value = 0;
					item.EndEdit();
					throw new Exception("CPL can only be Incremented against RestDay");
				}
				if (!Conversion.ToBool(item.Cells["IsCPL"].Value))
				{
					item.Cells["CPLQuota"].Value = 0;
					item.EndEdit();
				}
			}
			if (e.Column.Key == "CPLQuota" && !Conversion.ToBool(item.Cells["IsRestDay"].Value))
			{
				item.Cells["CPLQuota"].Value = 0;
				item.Cells["IsCPL"].Value = 0;
				item.EndEdit();
				throw new Exception("CPL can only be Incremented against RestDay");
			}
			if (e.Column.Key == "IsAbsent" && Conversion.ToBool(item.Cells["IsRestDay"].Value))
			{
				item.Cells["IsAbsent"].Value = 0;
				item.EndEdit();
				throw new Exception("You cannot Update IsAbsent because it is RestDay");
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
		try
		{
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_FormattingRow(object sender, RowLoadEventArgs e)
	{
	}

	private void btnLoad_Click(object sender, EventArgs e)
	{
		try
		{
			grdFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerate_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (txtInTime.Checked && Conversion.ToString(r.Cells["InEntryMode"].Value) != "A")
					{
						r.Cells["InTime"].Value = Conversion.ToDateTime((object)txtInTime.Value).ToString("HH:mm tt");
						r.Cells["InEntryMode"].Value = Conversion.ToString((object)"M");
						r.Cells["FlagInTime"].Value = 1;
						r.Cells["IsAbsent"].Value = 0;
					}
					if (txtOutTime.Checked && Conversion.ToString(r.Cells["OutEntryMode"].Value) != "A")
					{
						r.Cells["OutTime"].Value = Conversion.ToDateTime((object)txtOutTime.Value).ToString("HH:mm tt");
						r.Cells["OutEntryMode"].Value = Conversion.ToString((object)"M");
						r.Cells["FlagOutTime"].Value = 1;
						r.Cells["IsAbsent"].Value = 0;
					}
					r.EndEdit();
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Length == 0)
			{
				throw new Exception("Grid Record Not Found Please Check");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			hrmManualAttendance objh = new hrmManualAttendance();
			objh.OrganizationId = UserAccount.OrganizationId;
			objh.CompanyId = UserAccount.CompanyId;
			objh.listHrmManualAttendances = new List<hrmManualAttendance>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				if ((r.Cells["InEntryMode"].Text == "M" || r.Cells["OutEntryMode"].Text == "M") && (Conversion.ToString(r.Cells["InTime"].Value) != string.Empty || Conversion.ToString(r.Cells["OutTime"].Value) != string.Empty))
				{
					hrmManualAttendance obj = new hrmManualAttendance();
					obj.DutyDate = Conversion.ToDateTime(r.Cells["DutyDate"].Value);
					string DutyDate = obj.DutyDate.ToShortDateString();
					if (Conversion.ToString(r.Cells["InTime"].Value) != string.Empty)
					{
						obj.InDateTime = Conversion.ToDateTime(r.Cells["InTime"].Value);
						string DutyInTimeOnly = Conversion.ToDateTime((object)obj.InDateTime).ToString("hh:mm tt");
						obj.InDateTime = Convert.ToDateTime(DutyDate + " " + DutyInTimeOnly);
					}
					if (Conversion.ToString(r.Cells["OutTime"].Value) != string.Empty)
					{
						obj.OutDateTime = Conversion.ToDateTime(r.Cells["OutTime"].Value);
						string DutyoutTimeOnly = Conversion.ToDateTime((object)obj.OutDateTime).ToString("hh:mm tt");
						obj.OutDateTime = Convert.ToDateTime(DutyDate + " " + DutyoutTimeOnly);
					}
					obj.AlteredById = UserAccount.ID;
					obj.CreatedById = UserAccount.ID;
					obj.EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
					obj.CompanyId = UserAccount.CompanyId;
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.AttendanceStatus = Conversion.ToString((object)r.Cells["Status"].Text);
					obj.Description = Conversion.ToString((object)r.Cells["Description"].Text);
					obj.LocationId = Conversion.ToInt(r.Cells["LocationId"].Value);
					obj.DepartmentId = Conversion.ToInt(r.Cells["DepartmentId"].Value);
					obj.ShiftId = Conversion.ToInt(r.Cells["ShiftId"].Value);
					obj.ManualAttendanceId = Conversion.ToInt(r.Cells["ManualAttendanceId"].Value);
					if (obj.ManualAttendanceId == 0)
					{
						obj.ActionTypeId = 1;
					}
					else
					{
						obj.ActionTypeId = 2;
					}
					if (Conversion.ToBool(r.Cells["IsAbsent"].Value))
					{
						obj.ActionTypeId = 3;
					}
					obj.ManualCPLQuota = Conversion.ToDecimal(r.Cells["CPLQuota"].Value);
					obj.AlteredOn = DateTime.Now;
					obj.CreatedOn = DateTime.Now;
					objh.listHrmManualAttendances.Add(obj);
				}
			}
			hrmManualAttendance.Save(objh);
			MessageBox.Show("Saved Seccessfully");
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DatabaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ShiftNameComboFill();
			SectionFill();
			DepartmentNameComboFill();
			EmployeeName();
			LocationFilterFill();
			DesignationNameComboFill();
			EmployeeGroupComboFill();
			EmployeeCategoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			txtDate.Value = DateTime.Now;
			((Control)(object)cmbDepartment).Text = string.Empty;
			((Control)(object)cmbSection).Text = string.Empty;
			((Control)(object)cmbShift).Text = string.Empty;
			((Control)(object)cmbEmployee).Text = string.Empty;
			((Control)(object)CmbDesignation).Text = string.Empty;
			((Control)(object)CmbLocation).Text = string.Empty;
			((Control)(object)CmbEmployeeCategory).Text = string.Empty;
			((Control)(object)CmbEmployeeGroup).Text = string.Empty;
			grd.ClearStructure();
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
			Reset();
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataSet ds = HRM_Reports.DailyAttendanceRpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EmployeeCategoryId = Conversion.ToInt(CmbEmployeeCategory.Value),
				ShiftId = Conversion.ToInt(cmbShift.Value),
				DepartmentId = Conversion.ToInt(cmbDepartment.Value),
				DesignationId = Conversion.ToInt(CmbDesignation.Value),
				EmployeeId = Conversion.ToInt(cmbEmployee.Value),
				DocDate = Conversion.ToDateTime((object)txtDate.Value),
				PartyLocationId = Conversion.ToInt(CmbLocation.Value),
				SectionId = Conversion.ToInt(cmbSection.Value),
				ApprovedFilter = "All"
			});
			DataTable dtGrid = ds?.Tables[0]?.Copy();
			if (dtGrid.Rows.Count > 0 && dtGrid != null)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtgrid, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				new Reporting().ShowReportWithDataTable(dtGrid, "1003-DailyAttendance.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnEmployeeDefine_Click(object sender, EventArgs e)
	{
		try
		{
			frmEmployeeRegistration obj = new frmEmployeeRegistration(UserAccount);
			obj.formright = CommonServices.SetRightsValueInRightsObject("EmployeeRegistration");
			if (obj.formright.DoHaveViewRight)
			{
				obj.Show();
			}
			else
			{
				MessageBox.Show("You Dont Have rights View Of This Form..");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDailyAttendance_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbShift_Leave(object sender, EventArgs e)
	{
		try
		{
			EmployeeName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbLocation_Leave(object sender, EventArgs e)
	{
		try
		{
			EmployeeName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDepartment_Leave(object sender, EventArgs e)
	{
		try
		{
			EmployeeName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDesignation_Leave(object sender, EventArgs e)
	{
		try
		{
			EmployeeName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbSection_Leave(object sender, EventArgs e)
	{
		try
		{
			EmployeeName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbEmployeeCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			EmployeeName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbEmployeeGroup_Leave(object sender, EventArgs e)
	{
		try
		{
			EmployeeName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDailyAttendance_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_09b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.AttendanceManagement.frmDailyAttendance));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtInTime = new System.Windows.Forms.DateTimePicker();
		this.txtOutTime = new System.Windows.Forms.DateTimePicker();
		this.btnGenerate = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnEmployeeDefine = new System.Windows.Forms.Button();
		this.ChkRestDay = new System.Windows.Forms.CheckBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbEmployeeGroup = new UltraCombo();
		this.CmbEmployeeCategory = new UltraCombo();
		this.CmbLocation = new UltraCombo();
		this.CmbDesignation = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbShift = new UltraCombo();
		this.cmbSection = new UltraCombo();
		this.btnLoad = new System.Windows.Forms.Button();
		this.cmbEmployee = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbDepartment = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEmployeeGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEmployeeCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDesignation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 443);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1034, 443);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1026, 417);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1020, 411);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 216);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1020, 195);
		this.panel6.TabIndex = 3;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 30);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1020, 165);
		this.panel8.TabIndex = 159;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1020, 165);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).Tag = "grd";
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1020, 30);
		this.panel7.TabIndex = 158;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(971, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(8, 5);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 19);
		this.label8.TabIndex = 1;
		this.label8.Text = "Detail";
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1020, 188);
		this.panel4.TabIndex = 1;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1020, 30);
		this.panel5.TabIndex = 157;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(149, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Daily Attendance";
		this.groupBox2.Controls.Add(this.txtInTime);
		this.groupBox2.Controls.Add(this.txtOutTime);
		this.groupBox2.Controls.Add(this.btnGenerate);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(750, 36);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(273, 137);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Gernerate Time";
		this.txtInTime.CustomFormat = "hh:mm tt";
		this.txtInTime.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtInTime.Location = new System.Drawing.Point(61, 20);
		this.txtInTime.Name = "txtInTime";
		this.txtInTime.ShowCheckBox = true;
		this.txtInTime.Size = new System.Drawing.Size(204, 23);
		this.txtInTime.TabIndex = 0;
		this.txtOutTime.CustomFormat = "hh:mm tt";
		this.txtOutTime.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOutTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtOutTime.Location = new System.Drawing.Point(61, 50);
		this.txtOutTime.Name = "txtOutTime";
		this.txtOutTime.ShowCheckBox = true;
		this.txtOutTime.Size = new System.Drawing.Size(204, 23);
		this.txtOutTime.TabIndex = 1;
		this.btnGenerate.BackColor = System.Drawing.Color.Teal;
		this.btnGenerate.FlatAppearance.BorderSize = 0;
		this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGenerate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerate.ForeColor = System.Drawing.Color.White;
		this.btnGenerate.Location = new System.Drawing.Point(61, 78);
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.Size = new System.Drawing.Size(204, 26);
		this.btnGenerate.TabIndex = 2;
		this.btnGenerate.Text = "Generate";
		this.btnGenerate.UseVisualStyleBackColor = false;
		this.btnGenerate.Click += new System.EventHandler(btnGenerate_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(2, 24);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 153;
		this.label5.Text = "In Time";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(2, 54);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(57, 15);
		this.label4.TabIndex = 154;
		this.label4.Text = "Out Time";
		this.groupBox1.Controls.Add(this.btnEmployeeDefine);
		this.groupBox1.Controls.Add(this.ChkRestDay);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEmployeeGroup);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEmployeeCategory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLocation);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDesignation);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDate);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShift);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSection);
		this.groupBox1.Controls.Add(this.btnLoad);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployee);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 36);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(741, 137);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter Box";
		this.btnEmployeeDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeDefine.Location = new System.Drawing.Point(698, 78);
		this.btnEmployeeDefine.Name = "btnEmployeeDefine";
		this.btnEmployeeDefine.Size = new System.Drawing.Size(28, 26);
		this.btnEmployeeDefine.TabIndex = 9;
		this.btnEmployeeDefine.TabStop = false;
		this.btnEmployeeDefine.UseVisualStyleBackColor = true;
		this.btnEmployeeDefine.Click += new System.EventHandler(btnEmployeeDefine_Click);
		this.ChkRestDay.AutoSize = true;
		this.ChkRestDay.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkRestDay.Location = new System.Drawing.Point(535, 111);
		this.ChkRestDay.Name = "ChkRestDay";
		this.ChkRestDay.Size = new System.Drawing.Size(69, 19);
		this.ChkRestDay.TabIndex = 10;
		this.ChkRestDay.Text = "RestDay";
		this.ChkRestDay.UseVisualStyleBackColor = true;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(453, 54);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(67, 15);
		this.label12.TabIndex = 160;
		this.label12.Text = "Emp Group";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(453, 24);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(81, 15);
		this.label11.TabIndex = 161;
		this.label11.Text = "Emp Category";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(9, 84);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(53, 15);
		this.label6.TabIndex = 162;
		this.label6.Text = "Location";
		this.CmbEmployeeGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbEmployeeGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEmployeeGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEmployeeGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEmployeeGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeGroup).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbEmployeeGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeGroup).Location = new System.Drawing.Point(535, 48);
		((System.Windows.Forms.Control)(object)this.CmbEmployeeGroup).Name = "CmbEmployeeGroup";
		((System.Windows.Forms.Control)(object)this.CmbEmployeeGroup).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbEmployeeGroup).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeGroup).Leave += new System.EventHandler(CmbEmployeeGroup_Leave);
		this.CmbEmployeeCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbEmployeeCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEmployeeCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEmployeeCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEmployeeCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbEmployeeCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeCategory).Location = new System.Drawing.Point(535, 18);
		((System.Windows.Forms.Control)(object)this.CmbEmployeeCategory).Name = "CmbEmployeeCategory";
		((System.Windows.Forms.Control)(object)this.CmbEmployeeCategory).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbEmployeeCategory).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeCategory).Leave += new System.EventHandler(CmbEmployeeCategory_Leave);
		this.CmbLocation.AllowNull = (DefaultableBoolean)1;
		this.CmbLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance83;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLocation).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLocation).Location = new System.Drawing.Point(63, 78);
		((System.Windows.Forms.Control)(object)this.CmbLocation).Name = "CmbLocation";
		((System.Windows.Forms.Control)(object)this.CmbLocation).Size = new System.Drawing.Size(154, 26);
		((System.Windows.Forms.Control)(object)this.CmbLocation).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbLocation).Leave += new System.EventHandler(CmbLocation_Leave);
		this.CmbDesignation.AllowNull = (DefaultableBoolean)1;
		this.CmbDesignation.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDesignation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDesignation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDesignation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
		((SpecialBoxBase)((UltraGridBase)this.CmbDesignation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDesignation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDesignation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDesignation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDesignation).Location = new System.Drawing.Point(297, 48);
		((System.Windows.Forms.Control)(object)this.CmbDesignation).Name = "CmbDesignation";
		((System.Windows.Forms.Control)(object)this.CmbDesignation).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.CmbDesignation).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbDesignation).Leave += new System.EventHandler(CmbDesignation_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(224, 54);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(71, 15);
		this.label7.TabIndex = 159;
		this.label7.Text = "Designation";
		this.txtDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDate.Location = new System.Drawing.Point(63, 20);
		this.txtDate.Name = "txtDate";
		this.txtDate.Size = new System.Drawing.Size(154, 23);
		this.txtDate.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(224, 84);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(47, 15);
		this.label9.TabIndex = 133;
		this.label9.Text = "Section";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(9, 54);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(32, 15);
		this.label10.TabIndex = 134;
		this.label10.Text = "Shift";
		this.cmbShift.AllowNull = (DefaultableBoolean)1;
		this.cmbShift.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShift.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShift.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShift).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbShift).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShift).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShift).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShift.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShift).Location = new System.Drawing.Point(63, 48);
		((System.Windows.Forms.Control)(object)this.cmbShift).Name = "cmbShift";
		((System.Windows.Forms.Control)(object)this.cmbShift).Size = new System.Drawing.Size(154, 26);
		((System.Windows.Forms.Control)(object)this.cmbShift).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbShift).Leave += new System.EventHandler(cmbShift_Leave);
		this.cmbSection.AllowNull = (DefaultableBoolean)1;
		this.cmbSection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbSection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSection).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Location = new System.Drawing.Point(297, 78);
		((System.Windows.Forms.Control)(object)this.cmbSection).Name = "cmbSection";
		((System.Windows.Forms.Control)(object)this.cmbSection).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.cmbSection).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbSection).Leave += new System.EventHandler(cmbSection_Leave);
		this.btnLoad.BackColor = System.Drawing.Color.Teal;
		this.btnLoad.FlatAppearance.BorderSize = 0;
		this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoad.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoad.ForeColor = System.Drawing.Color.White;
		this.btnLoad.Location = new System.Drawing.Point(672, 107);
		this.btnLoad.Name = "btnLoad";
		this.btnLoad.Size = new System.Drawing.Size(54, 26);
		this.btnLoad.TabIndex = 11;
		this.btnLoad.Text = "Load";
		this.btnLoad.UseVisualStyleBackColor = false;
		this.btnLoad.Click += new System.EventHandler(btnLoad_Click);
		this.cmbEmployee.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbEmployee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Location = new System.Drawing.Point(535, 78);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Name = "cmbEmployee";
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TabIndex = 8;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(224, 24);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(71, 15);
		this.label1.TabIndex = 137;
		this.label1.Text = "Department";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(453, 84);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 15);
		this.label3.TabIndex = 141;
		this.label3.Text = "Employee";
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(297, 18);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Leave += new System.EventHandler(cmbDepartment_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(9, 24);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(32, 15);
		this.label2.TabIndex = 139;
		this.label2.Text = "Date";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1020, 28);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnPrint, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1020, 27);
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
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1003-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 443);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmDailyAttendance";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmDailyAttendance_FormClosed);
		base.Load += new System.EventHandler(frmDailyAttendance_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDailyAttendance_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEmployeeGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEmployeeCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDesignation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
