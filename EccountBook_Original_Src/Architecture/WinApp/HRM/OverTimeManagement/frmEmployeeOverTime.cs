using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.OverTimeManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.OverTimeManagement;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.OverTimeManagement;

public class frmEmployeeOverTime : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private List<EmployeeOverTime> detailList = new List<EmployeeOverTime>();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label9;

	private Label label10;

	internal UltraCombo cmbYear;

	internal UltraCombo cmbMonth;

	private Label label3;

	private GroupBox groupBox1;

	private Button btnShow;

	private Label label1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel6;

	private Panel panel5;

	private Panel panel2;

	internal GridEX grdEmployeeOt;

	internal GridEX grdEmployeeDetail;

	private Panel panel3;

	private Label lblEmpName;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label2;

	private Button btnAddEmployeeDetail;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private Panel panel4;

	private Label label4;

	private Panel panel7;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	internal UltraCombo cmbMonthHistory;

	private Label label5;

	private Label label6;

	internal UltraCombo cmbYearHistory;

	internal GridEX grdHistory;

	private Label label7;

	internal UltraCombo cmbEmployeeHistory;

	private ToolStripButton BtnRefreshHistory;

	private ToolStripButton btnShortcutkeys;

	private ToolStripButton btnPrint1011;

	public frmEmployeeOverTime(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void EmployeeFamilyInfo_Load(object sender, EventArgs e)
	{
		try
		{
			YearFill();
			MonthFill();
			DropDownBindsForHistory();
			btnsave.Visible = true;
			btnupdate.Visible = false;
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
			DataTable dty = CommonServices.GetMonths();
			DDL.BindDDL(dty, cmbMonth, "Id", "Name", "Month", ZeroIndex: false);
			((UltraGridBase)cmbMonth).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void YearFill()
	{
		try
		{
			DataTable dty = CommonServices.GetYears();
			DDL.BindDDL(dty, cmbYear, "Id", "Year", "Year", ZeroIndex: false);
			((UltraGridBase)cmbYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DropDownBindsForHistory()
	{
		try
		{
			DataTable dt = EmployeeOverTime.GetDataForDropDown(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtMonth = new DataTable();
			dtMonth.Columns.Add("Id");
			dtMonth.Columns.Add("name");
			DataTable dtYear = new DataTable();
			dtYear.Columns.Add("Id");
			dtYear.Columns.Add("name");
			DataTable dtEmployee = new DataTable();
			dtEmployee.Columns.Add("Id");
			dtEmployee.Columns.Add("name");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "OverTimeMonth")
				{
					dtMonth.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "OverTimeYear")
				{
					dtYear.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Employee")
				{
					dtEmployee.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtMonth, cmbMonthHistory, "Id", "name", "Month", ZeroIndex: false);
			((UltraGridBase)cmbMonthHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtYear, cmbYearHistory, "Id", "name", "Year", ZeroIndex: false);
			((UltraGridBase)cmbYearHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtEmployee, cmbEmployeeHistory, "Id", "name", "Employee", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeFamilyInfo_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else
				{
					btnShowHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnNewHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.R && tabControl1.SelectedIndex == 1)
			{
				BtnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (((Control)(object)grdEmployeeOt).Focused)
					{
						((Control)(object)grdEmployeeDetail).Focus();
					}
					else if (((Control)(object)grdEmployeeDetail).Focused)
					{
						((Control)(object)grdEmployeeOt).Focus();
					}
					else
					{
						((Control)(object)grdEmployeeOt).Focus();
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					cmbMonth.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					cmbMonthHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					tabControl1.SelectedIndex = 1;
					cmbMonthHistory.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					cmbMonth.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbMonth.Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible)
			{
				_ = btnupdate.Enabled;
			}
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
			RecId = 0;
			cmbMonth.Value = string.Empty;
			((Control)(object)cmbYear).Text = string.Empty;
			detailList.Clear();
			lblEmpName.Text = "( )";
			btnsave.Visible = true;
			btnupdate.Visible = false;
			grdEmployeeDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RetrivedData(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			EmployeeOverTime obj = new EmployeeOverTime();
			RecId = Id;
			obj = EmployeeOverTime.GetByID(RecId);
			if (obj != null)
			{
				cmbMonth.Value = obj.OverTimeMonth;
				cmbYear.Value = obj.OverTimeYear;
				btnupdate.Visible = true;
				btnsave.Visible = false;
			}
			else
			{
				MessageBox.Show("Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			if (detailList.Count <= 0)
			{
				MessageBox.Show("No Record Found", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			EmployeeOverTime obj = new EmployeeOverTime();
			obj.EmployeeOverTimeList = new List<EmployeeOverTime>();
			if (detailList.Count > 0)
			{
				for (int i = 0; i < detailList.Count; i++)
				{
					obj.EmployeeOverTimeList.Add(detailList[i]);
				}
			}
			EmployeeOverTime.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Saved Successfully");
			}
			Reset();
			GetEmployeesData();
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdEmployeeOt).Name);
			if (fs != null)
			{
				grdEmployeeOt.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GetEmployeesData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetEmployeesData()
	{
		try
		{
			DataTable dt = EmployeeOverTime.getOTEmployees(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbMonth.Value), Conversion.ToInt(cmbYear.Value));
			if (dt.Rows.Count > 0)
			{
				DataTable dtEmployee = new DataTable();
				dtEmployee.Columns.Add("Id", typeof(long));
				dtEmployee.Columns.Add("EmployeeName");
				dtEmployee.Columns.Add("Designation");
				dtEmployee.Columns.Add("Branch");
				dtEmployee.Columns.Add("Department");
				dtEmployee.Columns.Add("Section");
				dtEmployee.Columns.Add("Mobile");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtEmployee.Rows.Add(dt.Rows[i]["EmployeeId"], dt.Rows[i]["EmployeeName"], dt.Rows[i]["Designation"], dt.Rows[i]["Branch"], dt.Rows[i]["Department"], dt.Rows[i]["Section"], dt.Rows[i]["Mobile1"]);
				}
				grdEmployeeOt.DataSource = dtEmployee;
				grdEmployeeOt.RetrieveStructure();
				grdEmployeeOt.RootTable.Columns["Id"].Visible = false;
				grdEmployeeOt.RootTable.Columns["EmployeeName"].ColumnType = (ColumnType)5;
				CommonServices.GridAutoAdjustmentNew(grdEmployeeOt);
				ctrlGrdBar1_Load(null, null);
			}
			else
			{
				grdEmployeeOt.ClearStructure();
				grdEmployeeDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeOt_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdEmployeeOt.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				if (e.Column.Key == "EmployeeName")
				{
					long EmpId = Conversion.ToInt64(row.Cells["Id"].Value);
					lblEmpName.Text = "( " + row.Cells["EmployeeName"].Value?.ToString() + " )";
					GetDetailDataByEmployee(EmpId);
				}
			}
			else
			{
				lblEmpName.Text = "( )";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailDataByEmployee(long empId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EmployeeId = empId;
			obj.MonthValue = Conversion.ToInt(cmbMonth.Value);
			obj.Year = Conversion.ToInt(cmbYear.Value);
			DataTable dt = EmployeeOverTime.GetEmployeeAttendanceMonthWise(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable detail = new DataTable();
				detail.Columns.Add("EmpOvertimeId", typeof(long));
				detail.Columns.Add("OvertimeDetailId", typeof(long));
				detail.Columns.Add("EmpAttendanceId", typeof(long));
				detail.Columns.Add("EmployeeId", typeof(long));
				detail.Columns.Add("OvertimeMonth", typeof(int));
				detail.Columns.Add("OvertimeYear", typeof(int));
				detail.Columns.Add("Employee", typeof(string));
				detail.Columns.Add("Date", typeof(int));
				detail.Columns.Add("Day", typeof(string));
				detail.Columns.Add("FromTime", typeof(DateTime));
				detail.Columns.Add("ToTime", typeof(DateTime));
				detail.Columns.Add("Timing", typeof(string));
				detail.Columns.Add("WorkingHour", typeof(double));
				detail.Columns.Add("ReqOTHours", typeof(double));
				detail.Columns.Add("ActualOTHour", typeof(double));
				detail.Columns.Add("ShortHours", typeof(double));
				detail.Columns.Add("OTRate", typeof(double));
				detail.Columns.Add("OTHours", typeof(double));
				detail.Columns.Add("AddLess", typeof(double));
				detail.Columns.Add("NetOTHours", typeof(double));
				detail.Columns.Add("Amount", typeof(double));
				int i;
				for (i = 0; i < dt.Rows.Count; i++)
				{
					if (!detailList.Any((EmployeeOverTime r) => r.OverTimeRequestDetailId == Conversion.ToInt64(dt.Rows[i]["OverTimeRequestDetailId"])))
					{
						detail.Rows.Add(dt.Rows[i]["EmployeeOverTimeId"], dt.Rows[i]["OverTimeRequestDetailId"], dt.Rows[i]["EmployeeAttendanceId"], dt.Rows[i]["EmployeeId"], dt.Rows[i]["OverTimeMonth"], dt.Rows[i]["OverTimeYear"], dt.Rows[i]["Employee"], dt.Rows[i]["Date"], dt.Rows[i]["Day"], dt.Rows[i]["FromDateTime"], dt.Rows[i]["ToDateTime"], dt.Rows[i]["Timing"], dt.Rows[i]["Working Hour"], dt.Rows[i]["ReqOTHours"], dt.Rows[i]["ActualOTHour"], dt.Rows[i]["ShortHour"], Math.Round(Conversion.ToDouble(dt.Rows[i]["OT Rate"]), 3), dt.Rows[i]["ApproveOTHour"], dt.Rows[i]["AddLessOTHours"], Conversion.ToDouble(dt.Rows[i]["ApproveOTHour"]) + Conversion.ToDouble(dt.Rows[i]["AddLessOTHours"]), (Conversion.ToDouble(dt.Rows[i]["ApproveOTHour"]) + Conversion.ToDouble(dt.Rows[i]["AddLessOTHours"])) * Math.Round(Conversion.ToDouble(dt.Rows[i]["OT Rate"]), 3));
					}
				}
				grdEmployeeDetail.DataSource = detail;
				grdEmployeeDetail.RetrieveStructure();
				GridSettingForEmpDetail();
			}
			else
			{
				grdEmployeeDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForEmpDetail()
	{
		try
		{
			grdEmployeeDetail.AllowEdit = (InheritableBoolean)1;
			grdEmployeeDetail.RootTable.Columns["EmpOvertimeId"].Visible = false;
			grdEmployeeDetail.RootTable.Columns["OvertimeDetailId"].Visible = false;
			grdEmployeeDetail.RootTable.Columns["EmpAttendanceId"].Visible = false;
			grdEmployeeDetail.RootTable.Columns["EmployeeId"].Visible = false;
			grdEmployeeDetail.RootTable.Columns["OvertimeMonth"].Visible = false;
			grdEmployeeDetail.RootTable.Columns["OvertimeYear"].Visible = false;
			grdEmployeeDetail.RootTable.Columns["ActualOTHour"].Visible = false;
			grdEmployeeDetail.RootTable.Columns["ReqOTHours"].Visible = false;
			grdEmployeeDetail.RootTable.Columns["ShortHours"].Visible = false;
			grdEmployeeDetail.RootTable.Columns["EmpOvertimeId"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["OvertimeDetailId"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["EmpAttendanceId"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["EmployeeId"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["OvertimeMonth"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["OvertimeYear"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["Employee"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["Date"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["Day"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["FromTime"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["ToTime"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["Timing"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["WorkingHour"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["ReqOTHours"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["ActualOTHour"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["ShortHours"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["OTRate"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["OTHours"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["NetOTHours"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdEmployeeDetail.RootTable.Columns["EmpOvertimeId"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["OvertimeDetailId"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["EmpAttendanceId"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["EmployeeId"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["OvertimeMonth"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["OvertimeYear"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["Employee"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["Date"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["Day"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["FromTime"].FilterEditType = (FilterEditType)6;
			grdEmployeeDetail.RootTable.Columns["ToTime"].FilterEditType = (FilterEditType)6;
			grdEmployeeDetail.RootTable.Columns["Timing"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["WorkingHour"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["ReqOTHours"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["ActualOTHour"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["ShortHours"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["OTRate"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["OTHours"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["NetOTHours"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["AddLess"].FilterEditType = (FilterEditType)1;
			grdEmployeeDetail.RootTable.Columns["FromTime"].FormatString = "hh:mm tt";
			grdEmployeeDetail.RootTable.Columns["ToTime"].FormatString = "hh:mm tt";
			grdEmployeeDetail.RootTable.Columns["WorkingHour"].TextAlignment = (TextAlignment)3;
			grdEmployeeDetail.RootTable.Columns["ReqOTHours"].TextAlignment = (TextAlignment)3;
			grdEmployeeDetail.RootTable.Columns["ActualOTHour"].TextAlignment = (TextAlignment)3;
			grdEmployeeDetail.RootTable.Columns["ShortHours"].TextAlignment = (TextAlignment)3;
			grdEmployeeDetail.RootTable.Columns["OTRate"].TextAlignment = (TextAlignment)3;
			grdEmployeeDetail.RootTable.Columns["OTRate"].FormatString = "#,##0.###";
			grdEmployeeDetail.RootTable.Columns["OTHours"].TextAlignment = (TextAlignment)3;
			grdEmployeeDetail.RootTable.Columns["NetOTHours"].TextAlignment = (TextAlignment)3;
			grdEmployeeDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdEmployeeDetail.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			grdEmployeeDetail.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			grdEmployeeDetail.RootTable.Columns["AddLess"].FormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdEmployeeDetail);
			grdEmployeeDetail.RootTable.Columns.Add("Select");
			grdEmployeeDetail.RootTable.Columns["Select"].ActAsSelector = true;
			grdEmployeeDetail.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdEmployeeDetail.RootTable.Columns["Select"].Width = 40;
			grdEmployeeDetail.RootTable.Columns["Select"].Position = 0;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddEmployeeDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdEmployeeDetail.GetCheckedRows().Count() == 0)
			{
				MessageBox.Show("No Rows Selected", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (MessageBox.Show("Are you sure to Add?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				addDataToDetailList();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void addDataToDetailList()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			GridEXRow[] checkedRows = grdEmployeeDetail.GetCheckedRows();
			foreach (GridEXRow row in checkedRows)
			{
				if (!detailList.Any((EmployeeOverTime r) => r.OverTimeRequestDetailId == Conversion.ToInt64(row.Cells["OvertimeDetailId"].Value)))
				{
					EmployeeOverTime obj = new EmployeeOverTime();
					obj.ActionTypeId = 1;
					obj.AlteredById = UserAccount.ID;
					obj.CreatedById = UserAccount.ID;
					obj.ApprovedById = UserAccount.ID;
					obj.CreatedOn = DateTime.Now;
					obj.AlteredOn = DateTime.Now;
					obj.ApprovedOn = DateTime.Now;
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.CompanyId = UserAccount.CompanyId;
					obj.EmployeeAttendanceId = Conversion.ToInt64(row.Cells["EmpAttendanceId"].Value);
					obj.OverTimeRequestDetailId = Conversion.ToInt64(row.Cells["OvertimeDetailId"].Value);
					obj.EmployeeId = Conversion.ToInt64(row.Cells["EmployeeId"].Value);
					obj.OverTimeMonth = Conversion.ToInt(row.Cells["OvertimeMonth"].Value);
					obj.OverTimeYear = Conversion.ToInt(row.Cells["OvertimeYear"].Value);
					obj.OTHours = Conversion.ToDecimal(row.Cells["ActualOTHour"].Value);
					obj.ShortHours = Conversion.ToDecimal(row.Cells["ShortHours"].Value);
					obj.OTRate = Conversion.ToDecimal(row.Cells["OTRate"].Value);
					obj.NetOTHours = Conversion.ToDecimal(row.Cells["OTHours"].Value);
					obj.AddLessOTHours = Conversion.ToDecimal(row.Cells["AddLess"].Value);
					obj.ApprovedHours = Conversion.ToDecimal(row.Cells["NetOTHours"].Value);
					obj.IsApproved = true;
					detailList.Add(obj);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeDetail_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "AddLess" && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdEmployeeDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdEmployeeDetail.UpdateData();
			GridEXRow item = grdEmployeeDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "AddLess" && item.Cells["AddLess"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble(item.Cells["OTRate"].Value);
				double AddLess = Conversion.ToDouble(item.Cells["AddLess"].Value);
				double num2 = Conversion.ToDouble(item.Cells["OTHours"].Value);
				double NetOt = 0.0;
				NetOt = num2 + AddLess;
				item.Cells["NetOTHours"].Value = NetOt;
				double amount = Math.Round(num * NetOt, 3);
				item.Cells["Amount"].Value = amount;
			}
			grdEmployeeDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdEmployeeDetail).Name);
			if (fs != null)
			{
				grdEmployeeDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbMonthHistory.Value) <= 0)
			{
				MessageBox.Show("Month Required", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbMonthHistory.Focus();
				return;
			}
			if (Conversion.ToInt(cmbYearHistory.Value) <= 0)
			{
				MessageBox.Show("Year Required", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbYearHistory.Focus();
				return;
			}
			DataTable dt = EmployeeOverTime.FormHistory(new EmployeeOverTime
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				OverTimeMonth = Conversion.ToInt(cmbMonthHistory.Value),
				OverTimeYear = Conversion.ToInt(cmbYearHistory.Value),
				EmployeeId = Conversion.ToInt(cmbEmployeeHistory.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable history = new DataTable();
				history.Columns.Add("Id", typeof(long));
				history.Columns.Add("EmployeeName", typeof(string));
				history.Columns.Add("OverTimeDate", typeof(DateTime));
				history.Columns.Add("InTime", typeof(string));
				history.Columns.Add("OutTime", typeof(string));
				history.Columns.Add("TimeDuration", typeof(string));
				history.Columns.Add("ActualOTHours", typeof(double));
				history.Columns.Add("OTHours", typeof(double));
				history.Columns.Add("AddLess", typeof(double));
				history.Columns.Add("ApprovedHours", typeof(double));
				history.Columns.Add("OTRate", typeof(double));
				history.Columns.Add("ApprovedAmount", typeof(double));
				history.Columns.Add("EntryDate", typeof(DateTime));
				history.Columns.Add("EntryUser", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					history.Rows.Add(dt.Rows[i]["EmployeeOverTimeId"], dt.Rows[i]["EmployeeName"], dt.Rows[i]["OverTimeDate"], dt.Rows[i]["InTime"], dt.Rows[i]["OutTime"], dt.Rows[i]["TimeDuration"], dt.Rows[i]["OTHours"], dt.Rows[i]["NetOTHours"], dt.Rows[i]["AddLessOTHours"], dt.Rows[i]["ApprovedHours"], Math.Round(Conversion.ToDouble(dt.Rows[i]["OTRate"]), 3), dt.Rows[i]["ApprovedAmount"], dt.Rows[i]["CreatedOn"], dt.Rows[i]["CreatedBy"]);
				}
				grdHistory.DataSource = history;
				grdHistory.RetrieveStructure();
				grdHistory.RootTable.Columns["Id"].Visible = false;
				grdHistory.RootTable.Columns["ActualOTHours"].Visible = false;
				grdHistory.RootTable.Columns["ActualOTHours"].TextAlignment = (TextAlignment)3;
				grdHistory.RootTable.Columns["OTHours"].TextAlignment = (TextAlignment)3;
				grdHistory.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
				grdHistory.RootTable.Columns["ApprovedHours"].TextAlignment = (TextAlignment)3;
				grdHistory.RootTable.Columns["OTRate"].TextAlignment = (TextAlignment)3;
				grdHistory.RootTable.Columns["OTRate"].FormatString = "#,##0.###";
				grdHistory.RootTable.Columns["ApprovedAmount"].TextAlignment = (TextAlignment)3;
				grdHistory.RootTable.Columns["ApprovedAmount"].FormatString = "#,##0.###";
				grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
				CommonServices.GridAutoAdjustmentNew(grdHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbMonthHistory).Text = string.Empty;
			((Control)(object)cmbYearHistory).Text = string.Empty;
			((Control)(object)cmbEmployeeHistory).Text = string.Empty;
			grdHistory.ClearStructure();
			cmbMonthHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			DropDownBindsForHistory();
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
			if (tabControl1.SelectedIndex == 0)
			{
				cmbMonth.Focus();
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				cmbMonthHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Month");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Toggle in Grids in Form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Month when in Form tab and For focus on Month in history tab");
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

	private void btnPrint1011_Click(object sender, EventArgs e)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Conversion.ToInt(cmbMonthHistory.Value) <= 0)
			{
				MessageBox.Show("Month Required", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbMonthHistory.Focus();
				return;
			}
			if (Conversion.ToInt(cmbYearHistory.Value) <= 0)
			{
				MessageBox.Show("Year Required", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbYearHistory.Focus();
				return;
			}
			DataTable dt = EmployeeOverTime.EmployeeOverTime_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				MonthValue = Conversion.ToInt(cmbMonthHistory.Value),
				Year = Conversion.ToInt(cmbYearHistory.Value),
				EmployeeId = Conversion.ToInt64((object)cmbEmployeeHistory)
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1011-EmployeeOverTimeSlip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmEmployeeOverTime_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Expected O, but got Unknown
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected O, but got Unknown
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Expected O, but got Unknown
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c79: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.OverTimeManagement.frmEmployeeOverTime));
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
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdEmployeeDetail = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnAddEmployeeDetail = new System.Windows.Forms.Button();
		this.lblEmpName = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.grdEmployeeOt = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbMonth = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbYear = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbEmployeeHistory = new UltraCombo();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.cmbMonthHistory = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbYearHistory = new UltraCombo();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnPrint1011 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeDetail).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeOt).BeginInit();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMonthHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYearHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1095, 632);
		this.panel1.TabIndex = 2;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1095, 632);
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeDetail);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeOt);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1087, 606);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdEmployeeDetail.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdEmployeeDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeDetail.FilterMode = (FilterMode)2;
		this.grdEmployeeDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeDetail.GroupByBoxVisible = false;
		this.grdEmployeeDetail.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeDetail).Location = new System.Drawing.Point(3, 402);
		((System.Windows.Forms.Control)(object)this.grdEmployeeDetail).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeDetail).Name = "grdEmployeeDetail";
		((ScrollableBase)this.grdEmployeeDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeDetail).Size = new System.Drawing.Size(1081, 201);
		((System.Windows.Forms.Control)(object)this.grdEmployeeDetail).TabIndex = 13;
		this.grdEmployeeDetail.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeDetail).VisualStyle = (VisualStyle)4;
		this.grdEmployeeDetail.CellUpdated += new ColumnActionEventHandler(grdEmployeeDetail_CellUpdated);
		this.grdEmployeeDetail.UpdatingCell += new UpdatingCellEventHandler(grdEmployeeDetail_UpdatingCell);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.btnAddEmployeeDetail);
		this.panel3.Controls.Add(this.lblEmpName);
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 375);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1081, 27);
		this.panel3.TabIndex = 12;
		this.btnAddEmployeeDetail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnAddEmployeeDetail.BackColor = System.Drawing.Color.Yellow;
		this.btnAddEmployeeDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddEmployeeDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddEmployeeDetail.ForeColor = System.Drawing.Color.Teal;
		this.btnAddEmployeeDetail.Location = new System.Drawing.Point(980, 2);
		this.btnAddEmployeeDetail.Name = "btnAddEmployeeDetail";
		this.btnAddEmployeeDetail.Size = new System.Drawing.Size(57, 23);
		this.btnAddEmployeeDetail.TabIndex = 24;
		this.btnAddEmployeeDetail.Text = "Add";
		this.btnAddEmployeeDetail.UseVisualStyleBackColor = false;
		this.btnAddEmployeeDetail.Click += new System.EventHandler(btnAddEmployeeDetail_Click);
		this.lblEmpName.AutoSize = true;
		this.lblEmpName.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEmpName.ForeColor = System.Drawing.Color.Yellow;
		this.lblEmpName.Location = new System.Drawing.Point(366, 4);
		this.lblEmpName.Name = "lblEmpName";
		this.lblEmpName.Size = new System.Drawing.Size(19, 16);
		this.lblEmpName.TabIndex = 23;
		this.lblEmpName.Text = "()";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1041, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdEmployeeDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(2, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(365, 19);
		this.label2.TabIndex = 22;
		this.label2.Text = "Above Selected Employee's Overtime Detail";
		this.grdEmployeeOt.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdEmployeeOt).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdEmployeeOt.FilterMode = (FilterMode)2;
		this.grdEmployeeOt.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeOt).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeOt.GroupByBoxVisible = false;
		this.grdEmployeeOt.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeOt).Location = new System.Drawing.Point(3, 165);
		((System.Windows.Forms.Control)(object)this.grdEmployeeOt).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeOt).Name = "grdEmployeeOt";
		((ScrollableBase)this.grdEmployeeOt).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeOt).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeOt).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeOt).Size = new System.Drawing.Size(1081, 210);
		((System.Windows.Forms.Control)(object)this.grdEmployeeOt).TabIndex = 11;
		this.grdEmployeeOt.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeOt.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeOt).VisualStyle = (VisualStyle)4;
		this.grdEmployeeOt.LinkClicked += new ColumnActionEventHandler(grdEmployeeOt_LinkClicked);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 138);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1081, 27);
		this.panel6.TabIndex = 10;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1041, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdEmployeeOt;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(2, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(165, 19);
		this.label1.TabIndex = 22;
		this.label1.Text = "Employee's BY  OT ";
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 58);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1081, 80);
		this.panel5.TabIndex = 9;
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMonth);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbYear);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(356, 74);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(289, 37);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(60, 25);
		this.btnShow.TabIndex = 114;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbMonth.AllowNull = (DefaultableBoolean)1;
		this.cmbMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMonth).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMonth).Location = new System.Drawing.Point(6, 37);
		((System.Windows.Forms.Control)(object)this.cmbMonth).Name = "cmbMonth";
		((System.Windows.Forms.Control)(object)this.cmbMonth).Size = new System.Drawing.Size(139, 26);
		((System.Windows.Forms.Control)(object)this.cmbMonth).TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 18);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(43, 15);
		this.label9.TabIndex = 111;
		this.label9.Text = "Month";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(147, 18);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(29, 15);
		this.label10.TabIndex = 113;
		this.label10.Text = "Year";
		this.cmbYear.AllowNull = (DefaultableBoolean)1;
		this.cmbYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbYear).Location = new System.Drawing.Point(147, 37);
		((System.Windows.Forms.Control)(object)this.cmbYear).Name = "cmbYear";
		((System.Windows.Forms.Control)(object)this.cmbYear).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.cmbYear).TabIndex = 2;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 30);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1081, 28);
		this.panel2.TabIndex = 8;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(173, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Employee Overtime ";
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnupdate, this.btnsave, this.btnPrint, this.btnShortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1081, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(76, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(63, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(57, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.btnShortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutkeys.Name = "btnShortcutkeys";
		this.btnShortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutkeys.Text = "ShortCut Keys";
		this.btnShortcutkeys.Click += new System.EventHandler(btnShortcutkeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add(this.toolStrip2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1087, 606);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		this.grdHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 134);
		((System.Windows.Forms.Control)(object)this.grdHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1081, 469);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 11;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 58);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1081, 76);
		this.panel7.TabIndex = 10;
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeHistory);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMonthHistory);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbYearHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(7, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(620, 68);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(259, 16);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(59, 15);
		this.label7.TabIndex = 116;
		this.label7.Text = "Employee";
		this.cmbEmployeeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeHistory).Location = new System.Drawing.Point(259, 36);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeHistory).Name = "cmbEmployeeHistory";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeHistory).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeHistory).TabIndex = 115;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(553, 37);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(60, 25);
		this.btnShowHistory.TabIndex = 114;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.cmbMonthHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbMonthHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMonthHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMonthHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonthHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonthHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMonthHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMonthHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbMonthHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMonthHistory).Location = new System.Drawing.Point(6, 36);
		((System.Windows.Forms.Control)(object)this.cmbMonthHistory).Name = "cmbMonthHistory";
		((System.Windows.Forms.Control)(object)this.cmbMonthHistory).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.cmbMonthHistory).TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 16);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(43, 15);
		this.label5.TabIndex = 111;
		this.label5.Text = "Month";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(143, 16);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(29, 15);
		this.label6.TabIndex = 113;
		this.label6.Text = "Year";
		this.cmbYearHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbYearHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbYearHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbYearHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbYearHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbYearHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbYearHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbYearHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbYearHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbYearHistory).Location = new System.Drawing.Point(143, 36);
		((System.Windows.Forms.Control)(object)this.cmbYearHistory).Name = "cmbYearHistory";
		((System.Windows.Forms.Control)(object)this.cmbYearHistory).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.cmbYearHistory).TabIndex = 2;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1081, 28);
		this.panel4.TabIndex = 9;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(231, 19);
		this.label4.TabIndex = 0;
		this.label4.Text = "Employee Overtime History";
		this.toolStrip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.BtnRefreshHistory, this.btnPrint1011 });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1081, 27);
		this.toolStrip2.TabIndex = 8;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(58, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(78, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.btnPrint1011.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint1011.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint1011.Name = "btnPrint1011";
		this.btnPrint1011.Size = new System.Drawing.Size(89, 24);
		this.btnPrint1011.Text = "Print-1011";
		this.btnPrint1011.Click += new System.EventHandler(btnPrint1011_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1095, 632);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmEmployeeOverTime";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEmployeeOverTime_FormClosing);
		base.Load += new System.EventHandler(EmployeeFamilyInfo_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EmployeeFamilyInfo_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeOt).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMonthHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYearHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
