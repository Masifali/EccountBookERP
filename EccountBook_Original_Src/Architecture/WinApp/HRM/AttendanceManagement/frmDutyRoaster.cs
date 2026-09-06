using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.AttendanceManagement;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.AttendanceManagement;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.HRM.AttendanceManagement;

public class frmDutyRoaster : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtAllLookUp = new DataTable();

	private DataTable dtShift = new DataTable();

	private DataTable dt = new DataTable();

	private int ShiftId;

	private IContainer components;

	private ToolStripButton btnnew;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripLabel toolStripLabel1;

	private ToolStrip toolStrip1;

	internal UltraCombo cmbLocation;

	private Label label1;

	internal UltraCombo cmbEmployeeCategory;

	internal GridEX grdEmployee;

	private Panel panel3;

	private GroupBox groupBox1;

	internal UltraCombo cmbSection;

	private Label label9;

	internal UltraCombo cmbDepartment;

	private Label label8;

	private Label label3;

	private Label label7;

	private Label label6;

	private DateTimePicker txtTodate;

	private DateTimePicker txtFromDate;

	private Panel panel1;

	internal UltraCombo cmbShift;

	private Label label10;

	private Button btnLoad;

	private Label label12;

	private CheckBox chkAllSecurityNightShift;

	private CheckBox chkAllAdminShift;

	private CheckBox chkAllSecurityMorningShift;

	private CheckBox chkAllNonManagmentShift;

	private CheckBox chkAllManagmentShift;

	private ToolStripButton btnRefresh;

	internal GridEX GrdDutyRoaster;

	private Panel panel5;

	private Panel panel4;

	private TextBox txtDescription;

	private Panel panel7;

	private Panel panel6;

	internal UltraCombo cmbShiftDuty;

	private Label label2;

	private Label label23;

	private TabControl tabControl1;

	private TabPage tabPage3;

	private TabPage tabPage1;

	private Panel panel8;

	private Panel panel10;

	private Label label4;

	private CheckBox checkBox1;

	private CheckBox checkBox2;

	private CheckBox checkBox3;

	private CheckBox checkBox4;

	private CheckBox checkBox5;

	internal GridEX datagridhistoryDetail;

	internal GridEX datagridHistory;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnPrint;

	internal UltraCombo cmbLocationFilter;

	private Label label5;

	public frmDutyRoaster(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmDutyRoaster_Load(object sender, EventArgs e)
	{
		try
		{
			LocationFilterFill();
			AllLookUpFill();
			EmployeeCategoryFill();
			DepartmentFill();
			SectionFill();
			ShiftFill();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			LocationFilterFill();
			AllLookUpFill();
			LocationFill();
			EmployeeCategoryFill();
			DepartmentFill();
			SectionFill();
			ShiftFill();
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
			((Control)(object)cmbLocation).Text = string.Empty;
			((Control)(object)cmbEmployeeCategory).Text = string.Empty;
			((Control)(object)cmbDepartment).Text = string.Empty;
			((Control)(object)cmbSection).Text = string.Empty;
			((Control)(object)cmbShift).Text = string.Empty;
			((Control)(object)cmbShiftDuty).Text = string.Empty;
			((Control)(object)cmbLocationFilter).Text = string.Empty;
			txtDescription.Text = string.Empty;
			chkAllAdminShift.Checked = false;
			chkAllManagmentShift.Checked = false;
			chkAllNonManagmentShift.Checked = false;
			chkAllSecurityMorningShift.Checked = false;
			chkAllSecurityNightShift.Checked = false;
			cmbLocation.Focus();
			grdEmployee.ClearStructure();
			GrdDutyRoaster.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllLookUpFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			genProfile obj = new genProfile();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtAllLookUp = genProfile.GetByProfileTypeId(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dtLocation = genShiftLocation.Getall(new genShiftLocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ShiftId = Conversion.ToInt(cmbShiftDuty.Value)
			});
			if (dtLocation.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtLocation, cmbLocation, "LocationId", "LocationName", "Location Name", false);
				((UltraGridBase)cmbLocation).DisplayLayout.Bands[0].Columns[1].Width = 179;
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
				DDL.BindDDLNew(dtLocation, cmbLocationFilter, "LocationId", "LocationName", "Location Name", false);
				((UltraGridBase)cmbLocationFilter).DisplayLayout.Bands[0].Columns[1].Width = 179;
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
		//IL_0006: Expected O, but got Unknown
		try
		{
			genProfile obj = new genProfile();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ProfileTypeId = 23;
			dtAllLookUp = genProfile.GetByProfileTypeId(obj);
			if (dtAllLookUp.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAllLookUp, cmbEmployeeCategory, "ProfileId", "ProfileName", "Employee Category", false);
				((UltraGridBase)cmbEmployeeCategory).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepartmentFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDepartment = genDepartment.Getall(new genDepartment
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDepartment.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDepartment, cmbDepartment, "DepartmentId", "DepartmentName", "Department", false);
				((UltraGridBase)cmbDepartment).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SectionFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			genSection obj = new genSection();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtAllLookUp = genSection.Getall(obj);
			if (dtAllLookUp.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAllLookUp, cmbSection, "SectionId", "SectionName", "Section", false);
				((UltraGridBase)cmbSection).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShiftFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtShift = genShift.Getall(new genShift
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtShift.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtShift, cmbShift, "ShiftId", "ShiftName", "Shift", false);
				DDL.BindDDLNew(dtShift, cmbShiftDuty, "ShiftId", "ShiftName", "Shift", false);
				((UltraGridBase)cmbShift).DisplayLayout.Bands[0].Columns[1].Width = 165;
				((UltraGridBase)cmbShiftDuty).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoad_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		try
		{
			DataTable dtEmployee = genEmployeeHistory.EmployeeLoadForDutyRoaster(new genEmployeeHistory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ShiftId = Conversion.ToInt(cmbShift.Value),
				SectionId = Conversion.ToInt(cmbSection.Value),
				DepartmentId = Conversion.ToInt(cmbDepartment.Value),
				DesignationId = Conversion.ToInt(cmbEmployeeCategory.Value),
				LocationId = Conversion.ToInt(cmbLocationFilter.Value)
			});
			if (dtEmployee.Rows.Count > 0)
			{
				grdEmployee.DataSource = dtEmployee;
				grdEmployee.RetrieveStructure();
				grdSetting();
			}
			else
			{
				grdEmployee.DataSource = null;
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
			grdEmployee.RootTable.Columns["EmployeeHistoryId"].Visible = false;
			grdEmployee.RootTable.Columns["EmployeeId"].Visible = false;
			grdEmployee.RootTable.Columns["ShiftId"].Visible = false;
			grdEmployee.RootTable.Columns.Add("Active");
			grdEmployee.RootTable.Columns["Active"].ActAsSelector = true;
			grdEmployee.RootTable.Columns["Active"].UseHeaderSelector = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbShiftDuty).ActiveRow == null)
			{
				MessageBox.Show("Shift field required");
				return;
			}
			if (((UltraGridBase)cmbLocation).ActiveRow == null)
			{
				MessageBox.Show("Location field required");
				return;
			}
			if (grdEmployee.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Employee select first");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			double TotalDays = GetDateDifference(Convert.ToDateTime(txtFromDate.Value), Convert.ToDateTime(txtTodate.Value));
			int TotalDaysDiff = Conversion.ToInt((object)TotalDays);
			if (TotalDaysDiff == 0 || TotalDaysDiff < 0)
			{
				throw new Exception("DutyRoasterDetail not found Please Check");
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Date", typeof(DateTime));
			dt.Columns.Add("Day");
			dt.Columns.Add("EmployeeId");
			dt.Columns.Add("EmployeeHistoryId");
			dt.Rows.Clear();
			GridEXRow[] checkedRows = grdEmployee.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				DateTime x2 = txtFromDate.Value;
				for (int j = 0; (double)j <= TotalDays; j++)
				{
					DateTime result = txtFromDate.Value.AddDays(j);
					if (x2 == result)
					{
						dt.Rows.Add(x2, result.DayOfWeek, Conversion.ToInt(item.Cells["EmployeeId"].Value), Conversion.ToInt(item.Cells["EmployeeHistoryId"].Value));
					}
					else
					{
						dt.Rows.Add(x2.AddDays(1.0), result.DayOfWeek, Conversion.ToInt(item.Cells["EmployeeId"].Value), Conversion.ToInt(item.Cells["EmployeeHistoryId"].Value));
					}
					x2 = x2.AddDays(1.0);
				}
			}
			genDutyRoaster obj = new genDutyRoaster();
			obj.DutyRoasterId = 0L;
			obj.FromDate = txtFromDate.Value;
			obj.ToDate = txtTodate.Value;
			obj.CreatedOn = DateTime.Now;
			obj.AlteredOn = DateTime.Now;
			obj.CreatedById = UserAccount.ID;
			obj.AlteredById = UserAccount.ID;
			obj.Description = txtDescription.Text.Trim();
			obj.LocationId = Conversion.ToInt(cmbLocation.Value);
			obj.SectionId = Conversion.ToInt(cmbSection.Value);
			obj.ShiftId = Conversion.ToInt(cmbShiftDuty.Value);
			obj.EmployeeCategoryId = Conversion.ToInt(cmbEmployeeCategory.Value);
			obj.DepartmentId = Conversion.ToInt(cmbDepartment.Value);
			obj.GenDutyRoasterDetailsList = new List<genDutyRoasterDetail>();
			DataTable grdduty = new DataTable();
			grdduty.Columns.Add("Date");
			grdduty.Columns.Add("Day");
			grdduty.Columns.Add("Holiday");
			checkedRows = GrdDutyRoaster.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				grdduty.Rows.Add(r.Cells["Date"].Value, r.Cells["Day"].Value, r.Cells["Is Off Duty"].Value);
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				genDutyRoasterDetail genDuty = new genDutyRoasterDetail();
				genDuty.EmployeeId = Conversion.ToInt(dt.Rows[i]["EmployeeId"]);
				genDuty.EmployeeHistoryId = Conversion.ToInt(dt.Rows[i]["EmployeeHistoryId"]);
				genDuty.ShiftId = Conversion.ToInt(cmbShiftDuty.Value);
				genDuty.DutyDate = Conversion.ToDateTime(dt.Rows[i]["Date"]);
				genDuty.WeekDayName = Conversion.ToString(dt.Rows[i]["Day"]);
				for (int x = 0; x < grdduty.Rows.Count; x++)
				{
					if (Conversion.ToDateTime(grdduty.Rows[x]["Date"]) == Conversion.ToDateTime(dt.Rows[i]["Date"]) && Conversion.ToString(grdduty.Rows[x]["Day"]) == Conversion.ToString(dt.Rows[i]["Day"]))
					{
						genDuty.IsOffDuty = Conversion.ToBool(grdduty.Rows[x]["Holiday"]);
					}
				}
				if (obj.DutyRoasterId == 0L)
				{
					genDuty.ActionTypeId = 1;
				}
				else
				{
					genDuty.ActionTypeId = 2;
				}
				genDuty.CreatedOn = DateTime.Now;
				genDuty.AlteredOn = DateTime.Now;
				genDuty.CreatedById = UserAccount.ID;
				genDuty.AlteredById = UserAccount.ID;
				obj.GenDutyRoasterDetailsList.Add(genDuty);
			}
			if (genDutyRoaster.Save(obj) > 0)
			{
				MessageBox.Show("Record Saved");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployee_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		try
		{
			MannualDataSetInDutyRoaster();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeOffDaysRecords(genDutyRoaster obj)
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		try
		{
			var getEmployeesCount = from x in obj.GenDutyRoasterDetailsList
				group x by new { x.EmployeeId } into lg
				select new
				{
					GroupColumn = lg.Key,
					TotalCount = lg.Count()
				};
			if (getEmployeesCount == null)
			{
				return;
			}
			List<DateTime> lstOfRangeDate = GetRangeList(obj);
			foreach (var employe in getEmployeesCount)
			{
				foreach (DateTime date in lstOfRangeDate)
				{
					if (obj.GenDutyRoasterDetailsList.Where((genDutyRoasterDetail x) => x.EmployeeId == employe.GroupColumn.EmployeeId && x.DutyDate == date).FirstOrDefault() == null)
					{
						genDutyRoasterDetail objDutyRoasterDetail = new genDutyRoasterDetail();
						objDutyRoasterDetail.EmployeeId = employe.GroupColumn.EmployeeId;
						objDutyRoasterDetail.DutyRoasterDetailLineId = obj.GenDutyRoasterDetailsList.Max((genDutyRoasterDetail x) => x.DutyRoasterDetailLineId + 1);
						objDutyRoasterDetail.EmployeeHistoryId = obj.GenDutyRoasterDetailsList.Max((genDutyRoasterDetail x) => x.EmployeeHistoryId);
						objDutyRoasterDetail.DutyDate = date;
						objDutyRoasterDetail.StartTime = null;
						objDutyRoasterDetail.EndTime = null;
						objDutyRoasterDetail.RangeInDateTime = null;
						objDutyRoasterDetail.RangeOutDateTime = null;
						objDutyRoasterDetail.GraceInDateTime = null;
						objDutyRoasterDetail.GraceOutDateTime = null;
						objDutyRoasterDetail.IsOffDuty = true;
						objDutyRoasterDetail.ActionTypeId = 1;
						objDutyRoasterDetail.CreatedOn = DateTime.Now;
						objDutyRoasterDetail.AlteredOn = DateTime.Now;
						objDutyRoasterDetail.CreatedById = UserAccount.ID;
						objDutyRoasterDetail.AlteredById = UserAccount.ID;
						obj.GenDutyRoasterDetailsList.Add(objDutyRoasterDetail);
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double GetDateDifference(DateTime FromDate, DateTime ToDate)
	{
		double totalDays = 0.0;
		try
		{
			totalDays = (ToDate - FromDate).TotalDays;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return totalDays;
	}

	private List<DateTime> GetRangeList(genDutyRoaster obj)
	{
		List<DateTime> lst = null;
		try
		{
			double getNoOfDays = GetDateDifference(obj.FromDate, obj.ToDate);
			if (getNoOfDays > 0.0)
			{
				lst = new List<DateTime>();
				for (int i = 0; (double)i <= getNoOfDays; i++)
				{
					lst.Add(obj.FromDate.AddDays(i));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return lst;
	}

	private void MannualDataSetInDutyRoaster()
	{
		try
		{
			this.dt.Rows.Clear();
			GrdDutyRoaster.ClearStructure();
			double TotalDays = GetDateDifference(txtFromDate.Value, txtTodate.Value);
			GridEXRow[] checkedRows = grdEmployee.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Date", typeof(DateTime));
				dt.Columns.Add("Day");
				dt.Columns.Add("EmployeeId");
				dt.Columns.Add("EmployeeHistoryId");
				DateTime x = txtFromDate.Value;
				for (int i = 0; (double)i <= TotalDays; i++)
				{
					DateTime result = txtFromDate.Value.AddDays(i);
					if (x == result)
					{
						dt.Rows.Add(x, result.DayOfWeek, Conversion.ToInt(r.Cells["EmployeeId"].Value), Conversion.ToInt(r.Cells["EmployeeHistoryId"].Value));
					}
					else
					{
						dt.Rows.Add(x.AddDays(1.0), result.DayOfWeek, Conversion.ToInt(r.Cells["EmployeeId"].Value), Conversion.ToInt(r.Cells["EmployeeHistoryId"].Value));
					}
					x = x.AddDays(1.0);
				}
				GrdDutyRoaster.DataSource = dt;
				GrdDutyRoaster.RetrieveStructure();
				GrdDutyRoaster.RootTable.Columns[0].FormatString = "dd";
				GrdDutyRoaster.RootTable.Columns.Add("Is Off Duty");
				GrdDutyRoaster.RootTable.Columns["Is Off Duty"].EditType = (EditType)8;
				GrdDutyRoaster.RootTable.Columns["Is Off Duty"].FilterEditType = (FilterEditType)7;
				GrdDutyRoaster.RootTable.Columns["Is Off Duty"].ColumnType = (ColumnType)3;
				GrdDutyRoaster.RootTable.Columns["Is Off Duty"].ActAsSelector = true;
				GrdDutyRoaster.RootTable.Columns["Is Off Duty"].UseHeaderSelector = true;
				GrdDutyRoaster.RootTable.Columns["EmployeeId"].Visible = false;
				GrdDutyRoaster.RootTable.Columns["EmployeeHistoryId"].Visible = false;
				GrdDutyRoaster.RootTable.Columns[0].EditType = (EditType)0;
				GrdDutyRoaster.RootTable.Columns[0].FilterEditType = (FilterEditType)1;
				GrdDutyRoaster.RootTable.Columns[1].FilterEditType = (FilterEditType)1;
				GrdDutyRoaster.RootTable.Columns[1].EditType = (EditType)0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbShiftDuty_Leave(object sender, EventArgs e)
	{
		LocationFill();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			HistoryGridFill();
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtgrd = genDutyRoaster.GetHistoryHeaderData(new genDutyRoaster
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtgrd.Rows.Count > 0)
			{
				datagridHistory.DataSource = dtgrd;
				datagridHistory.RetrieveStructure();
				datagridHistory.RootTable.Columns["Id"].Visible = false;
			}
			else
			{
				datagridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datagridHistory_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (datagridHistory.SelectedItems.Count <= 0)
			{
				return;
			}
			GridEXRow item = datagridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				DataTable dt = genDutyRoaster.GetHistoryDetailData(Conversion.ToInt(datagridHistory.CurrentRow.Cells[0].Value));
				if (dt.Rows.Count > 0)
				{
					datagridhistoryDetail.DataSource = dt;
					datagridhistoryDetail.RetrieveStructure();
				}
				else
				{
					datagridhistoryDetail.ClearStructure();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
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
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
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
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Expected O, but got Unknown
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_14ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b8: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.AttendanceManagement.frmDutyRoaster));
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
		GridEXLayout GrdDutyRoaster_DesignTimeLayout = new GridEXLayout();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
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
		GridEXLayout datagridhistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout datagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.cmbLocation = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbEmployeeCategory = new UltraCombo();
		this.GrdDutyRoaster = new GridEX();
		this.grdEmployee = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.cmbShiftDuty = new UltraCombo();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnLoad = new System.Windows.Forms.Button();
		this.cmbShift = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbSection = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbDepartment = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtTodate = new System.Windows.Forms.DateTimePicker();
		this.chkAllSecurityNightShift = new System.Windows.Forms.CheckBox();
		this.chkAllAdminShift = new System.Windows.Forms.CheckBox();
		this.chkAllSecurityMorningShift = new System.Windows.Forms.CheckBox();
		this.chkAllNonManagmentShift = new System.Windows.Forms.CheckBox();
		this.chkAllManagmentShift = new System.Windows.Forms.CheckBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.datagridhistoryDetail = new GridEX();
		this.datagridHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.checkBox2 = new System.Windows.Forms.CheckBox();
		this.checkBox3 = new System.Windows.Forms.CheckBox();
		this.checkBox4 = new System.Windows.Forms.CheckBox();
		this.checkBox5 = new System.Windows.Forms.CheckBox();
		this.cmbLocationFilter = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.GrdDutyRoaster).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdEmployee).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbShiftDuty).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.datagridhistoryDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.datagridHistory).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationFilter).BeginInit();
		base.SuspendLayout();
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripLabel1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(78, 24);
		this.toolStripLabel1.Text = "Duty Roaster";
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripLabel1, this.toolStripSeparator1, this.btnnew, this.btnsave, this.btnupdate, this.btnPrint, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(788, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(77, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(58, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(59, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.cmbLocation.AllowNull = (DefaultableBoolean)1;
		this.cmbLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocation).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocation).Location = new System.Drawing.Point(203, 55);
		((System.Windows.Forms.Control)(object)this.cmbLocation).Name = "cmbLocation";
		((System.Windows.Forms.Control)(object)this.cmbLocation).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocation).TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(203, 35);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(53, 15);
		this.label1.TabIndex = 19;
		this.label1.Text = "Location";
		this.cmbEmployeeCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Location = new System.Drawing.Point(7, 36);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Name = "cmbEmployeeCategory";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).TabIndex = 0;
		((ScrollableBase)this.GrdDutyRoaster).BackColor = System.Drawing.Color.White;
		GrdDutyRoaster_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDutyRoaster.DesignTimeLayout = GrdDutyRoaster_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDutyRoaster).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdDutyRoaster.FilterMode = (FilterMode)2;
		this.GrdDutyRoaster.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDutyRoaster).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDutyRoaster.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdDutyRoaster).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdDutyRoaster).Name = "GrdDutyRoaster";
		((ScrollableBase)this.GrdDutyRoaster).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDutyRoaster).Size = new System.Drawing.Size(413, 532);
		((System.Windows.Forms.Control)(object)this.GrdDutyRoaster).TabIndex = 0;
		this.GrdDutyRoaster.TotalRow = (InheritableBoolean)1;
		this.GrdDutyRoaster.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDutyRoaster).VisualStyle = (VisualStyle)4;
		this.grdEmployee.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployee.FilterMode = (FilterMode)2;
		this.grdEmployee.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployee.GroupByBoxVisible = false;
		this.grdEmployee.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployee).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployee).Name = "grdEmployee";
		((ScrollableBase)this.grdEmployee).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployee).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployee).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Size = new System.Drawing.Size(788, 311);
		((System.Windows.Forms.Control)(object)this.grdEmployee).TabIndex = 0;
		this.grdEmployee.TotalRow = (InheritableBoolean)1;
		this.grdEmployee.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployee).VisualStyle = (VisualStyle)4;
		this.grdEmployee.CellValueChanged += new ColumnActionEventHandler(grdEmployee_CellValueChanged);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(788, 571);
		this.panel3.TabIndex = 1;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployee);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 260);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(788, 311);
		this.panel5.TabIndex = 1;
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShiftDuty);
		this.panel4.Controls.Add(this.txtDescription);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocation);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtFromDate);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtTodate);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(788, 260);
		this.panel4.TabIndex = 0;
		this.cmbShiftDuty.AllowNull = (DefaultableBoolean)1;
		this.cmbShiftDuty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShiftDuty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShiftDuty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShiftDuty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbShiftDuty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShiftDuty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShiftDuty).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShiftDuty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShiftDuty).Location = new System.Drawing.Point(9, 55);
		((System.Windows.Forms.Control)(object)this.cmbShiftDuty).Name = "cmbShiftDuty";
		((System.Windows.Forms.Control)(object)this.cmbShiftDuty).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbShiftDuty).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbShiftDuty).Leave += new System.EventHandler(cmbShiftDuty_Leave);
		this.txtDescription.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(9, 103);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(513, 23);
		this.txtDescription.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(9, 35);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(66, 15);
		this.label2.TabIndex = 27;
		this.label2.Text = "Shift Name";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationFilter);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.btnLoad);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShift);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSection);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Location = new System.Drawing.Point(9, 134);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(513, 121);
		this.groupBox1.TabIndex = 5;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter Box";
		this.btnLoad.BackColor = System.Drawing.Color.Silver;
		this.btnLoad.FlatAppearance.BorderSize = 0;
		this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoad.Location = new System.Drawing.Point(341, 89);
		this.btnLoad.Name = "btnLoad";
		this.btnLoad.Size = new System.Drawing.Size(46, 23);
		this.btnLoad.TabIndex = 5;
		this.btnLoad.Text = "Load";
		this.btnLoad.UseVisualStyleBackColor = false;
		this.btnLoad.Click += new System.EventHandler(btnLoad_Click);
		this.cmbShift.AllowNull = (DefaultableBoolean)1;
		this.cmbShift.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShift.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShift.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShift).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbShift).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShift).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShift).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShift.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShift).Location = new System.Drawing.Point(7, 89);
		((System.Windows.Forms.Control)(object)this.cmbShift).Name = "cmbShift";
		((System.Windows.Forms.Control)(object)this.cmbShift).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbShift).TabIndex = 3;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 68);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(32, 15);
		this.label10.TabIndex = 23;
		this.label10.Text = "Shift";
		this.cmbSection.AllowNull = (DefaultableBoolean)1;
		this.cmbSection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbSection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSection).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Location = new System.Drawing.Point(341, 36);
		((System.Windows.Forms.Control)(object)this.cmbSection).Name = "cmbSection";
		((System.Windows.Forms.Control)(object)this.cmbSection).Size = new System.Drawing.Size(158, 26);
		((System.Windows.Forms.Control)(object)this.cmbSection).TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(341, 14);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(46, 15);
		this.label9.TabIndex = 21;
		this.label9.Text = "Section";
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(174, 36);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(174, 18);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(71, 15);
		this.label8.TabIndex = 19;
		this.label8.Text = "Department";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(110, 15);
		this.label3.TabIndex = 17;
		this.label3.Text = "Employee Category";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(9, 85);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(69, 15);
		this.label12.TabIndex = 32;
		this.label12.Text = "Description";
		this.txtFromDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDate.Location = new System.Drawing.Point(408, 57);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(110, 23);
		this.txtFromDate.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(408, 35);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(63, 15);
		this.label7.TabIndex = 29;
		this.label7.Text = "From Date";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(520, 35);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(47, 15);
		this.label6.TabIndex = 28;
		this.label6.Text = "To Date";
		this.txtTodate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTodate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtTodate.Location = new System.Drawing.Point(522, 57);
		this.txtTodate.Name = "txtTodate";
		this.txtTodate.Size = new System.Drawing.Size(108, 23);
		this.txtTodate.TabIndex = 3;
		this.chkAllSecurityNightShift.AutoSize = true;
		this.chkAllSecurityNightShift.Location = new System.Drawing.Point(264, 96);
		this.chkAllSecurityNightShift.Name = "chkAllSecurityNightShift";
		this.chkAllSecurityNightShift.Size = new System.Drawing.Size(130, 17);
		this.chkAllSecurityNightShift.TabIndex = 4;
		this.chkAllSecurityNightShift.Text = "All Security Night Shift";
		this.chkAllSecurityNightShift.UseVisualStyleBackColor = true;
		this.chkAllSecurityNightShift.Visible = false;
		this.chkAllAdminShift.AutoSize = true;
		this.chkAllAdminShift.Location = new System.Drawing.Point(16, 76);
		this.chkAllAdminShift.Name = "chkAllAdminShift";
		this.chkAllAdminShift.Size = new System.Drawing.Size(93, 17);
		this.chkAllAdminShift.TabIndex = 0;
		this.chkAllAdminShift.Text = "All Admin Shift";
		this.chkAllAdminShift.UseVisualStyleBackColor = true;
		this.chkAllAdminShift.Visible = false;
		this.chkAllSecurityMorningShift.AutoSize = true;
		this.chkAllSecurityMorningShift.Location = new System.Drawing.Point(115, 74);
		this.chkAllSecurityMorningShift.Name = "chkAllSecurityMorningShift";
		this.chkAllSecurityMorningShift.Size = new System.Drawing.Size(143, 17);
		this.chkAllSecurityMorningShift.TabIndex = 1;
		this.chkAllSecurityMorningShift.Text = "All Security Morning Shift";
		this.chkAllSecurityMorningShift.UseVisualStyleBackColor = true;
		this.chkAllSecurityMorningShift.Visible = false;
		this.chkAllNonManagmentShift.AutoSize = true;
		this.chkAllNonManagmentShift.Location = new System.Drawing.Point(264, 73);
		this.chkAllNonManagmentShift.Name = "chkAllNonManagmentShift";
		this.chkAllNonManagmentShift.Size = new System.Drawing.Size(143, 17);
		this.chkAllNonManagmentShift.TabIndex = 2;
		this.chkAllNonManagmentShift.Text = "All Non Managment Shift";
		this.chkAllNonManagmentShift.UseVisualStyleBackColor = true;
		this.chkAllNonManagmentShift.Visible = false;
		this.chkAllManagmentShift.AutoSize = true;
		this.chkAllManagmentShift.Location = new System.Drawing.Point(409, 73);
		this.chkAllManagmentShift.Name = "chkAllManagmentShift";
		this.chkAllManagmentShift.Size = new System.Drawing.Size(120, 17);
		this.chkAllManagmentShift.TabIndex = 3;
		this.chkAllManagmentShift.Text = "All Managment Shift";
		this.chkAllManagmentShift.UseVisualStyleBackColor = true;
		this.chkAllManagmentShift.Visible = false;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1209, 597);
		this.panel1.TabIndex = 1;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1209, 597);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage3.Controls.Add(this.panel7);
		this.tabPage3.Controls.Add(this.panel6);
		this.tabPage3.Controls.Add(this.panel3);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1201, 571);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Form";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDutyRoaster);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(788, 39);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(413, 532);
		this.panel7.TabIndex = 12;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Controls.Add(this.chkAllSecurityMorningShift);
		this.panel6.Controls.Add(this.chkAllAdminShift);
		this.panel6.Controls.Add(this.chkAllNonManagmentShift);
		this.panel6.Controls.Add(this.chkAllSecurityNightShift);
		this.panel6.Controls.Add(this.chkAllManagmentShift);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(788, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(413, 39);
		this.panel6.TabIndex = 11;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(10, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(167, 24);
		this.label23.TabIndex = 5;
		this.label23.Text = "Duty Roaster Detail";
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Size = new System.Drawing.Size(1201, 571);
		this.tabPage1.TabIndex = 1;
		this.tabPage1.Text = "History";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.datagridhistoryDetail);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.datagridHistory);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1201, 571);
		this.panel8.TabIndex = 0;
		this.datagridhistoryDetail.AllowDelete = (InheritableBoolean)1;
		datagridhistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.datagridhistoryDetail.DesignTimeLayout = datagridhistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.datagridhistoryDetail.DynamicFiltering = true;
		this.datagridhistoryDetail.FilterMode = (FilterMode)2;
		this.datagridhistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.datagridhistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Location = new System.Drawing.Point(0, 303);
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Name = "datagridhistoryDetail";
		((ScrollableBase)this.datagridhistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Size = new System.Drawing.Size(1201, 268);
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).TabIndex = 46;
		this.datagridhistoryDetail.TotalRow = (InheritableBoolean)1;
		this.datagridhistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagridhistoryDetail).VisualStyle = (VisualStyle)4;
		this.datagridHistory.AllowDelete = (InheritableBoolean)1;
		datagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.datagridHistory.DesignTimeLayout = datagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.datagridHistory.DynamicFiltering = true;
		this.datagridHistory.FilterMode = (FilterMode)2;
		this.datagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.datagridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Location = new System.Drawing.Point(0, 39);
		((System.Windows.Forms.Control)(object)this.datagridHistory).Name = "datagridHistory";
		((ScrollableBase)this.datagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Size = new System.Drawing.Size(1201, 264);
		((System.Windows.Forms.Control)(object)this.datagridHistory).TabIndex = 45;
		this.datagridHistory.TotalRow = (InheritableBoolean)1;
		this.datagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Click += new System.EventHandler(datagridHistory_Click);
		this.panel10.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.panel10.Controls.Add(this.label4);
		this.panel10.Controls.Add(this.checkBox1);
		this.panel10.Controls.Add(this.checkBox2);
		this.panel10.Controls.Add(this.checkBox3);
		this.panel10.Controls.Add(this.checkBox4);
		this.panel10.Controls.Add(this.checkBox5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1201, 39);
		this.panel10.TabIndex = 12;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(10, 8);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(246, 24);
		this.label4.TabIndex = 5;
		this.label4.Text = "Duty Roaster History Header";
		this.checkBox1.AutoSize = true;
		this.checkBox1.Location = new System.Drawing.Point(115, 74);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(172, 20);
		this.checkBox1.TabIndex = 1;
		this.checkBox1.Text = "All Security Morning Shift";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.checkBox1.Visible = false;
		this.checkBox2.AutoSize = true;
		this.checkBox2.Location = new System.Drawing.Point(16, 76);
		this.checkBox2.Name = "checkBox2";
		this.checkBox2.Size = new System.Drawing.Size(111, 20);
		this.checkBox2.TabIndex = 0;
		this.checkBox2.Text = "All Admin Shift";
		this.checkBox2.UseVisualStyleBackColor = true;
		this.checkBox2.Visible = false;
		this.checkBox3.AutoSize = true;
		this.checkBox3.Location = new System.Drawing.Point(264, 73);
		this.checkBox3.Name = "checkBox3";
		this.checkBox3.Size = new System.Drawing.Size(172, 20);
		this.checkBox3.TabIndex = 2;
		this.checkBox3.Text = "All Non Managment Shift";
		this.checkBox3.UseVisualStyleBackColor = true;
		this.checkBox3.Visible = false;
		this.checkBox4.AutoSize = true;
		this.checkBox4.Location = new System.Drawing.Point(264, 96);
		this.checkBox4.Name = "checkBox4";
		this.checkBox4.Size = new System.Drawing.Size(155, 20);
		this.checkBox4.TabIndex = 4;
		this.checkBox4.Text = "All Security Night Shift";
		this.checkBox4.UseVisualStyleBackColor = true;
		this.checkBox4.Visible = false;
		this.checkBox5.AutoSize = true;
		this.checkBox5.Location = new System.Drawing.Point(409, 73);
		this.checkBox5.Name = "checkBox5";
		this.checkBox5.Size = new System.Drawing.Size(144, 20);
		this.checkBox5.TabIndex = 3;
		this.checkBox5.Text = "All Managment Shift";
		this.checkBox5.UseVisualStyleBackColor = true;
		this.checkBox5.Visible = false;
		this.cmbLocationFilter.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationFilter).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationFilter).Location = new System.Drawing.Point(174, 89);
		((System.Windows.Forms.Control)(object)this.cmbLocationFilter).Name = "cmbLocationFilter";
		((System.Windows.Forms.Control)(object)this.cmbLocationFilter).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationFilter).TabIndex = 24;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(174, 71);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(53, 15);
		this.label5.TabIndex = 25;
		this.label5.Text = "Location";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1209, 597);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmDutyRoaster";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmDutyRoaster_Load);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.GrdDutyRoaster).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdEmployee).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbShiftDuty).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabPage1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.datagridhistoryDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.datagridHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationFilter).EndInit();
		base.ResumeLayout(false);
	}
}
