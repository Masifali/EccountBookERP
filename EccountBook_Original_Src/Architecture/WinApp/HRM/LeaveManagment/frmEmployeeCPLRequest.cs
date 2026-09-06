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
using Architecture.BLL.HRM.LeaveManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.LeaveManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.EmployeeManagement;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.LeaveManagment;

public class frmEmployeeCPLRequest : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private int RecId;

	private int updateDetailIndex;

	private List<genEmployeeLeaveDetail> Deletelst = new List<genEmployeeLeaveDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel6;

	private Panel panel9;

	internal GridEX DataGridHistory;

	private Panel panel8;

	private Panel panel2;

	private Panel panel5;

	internal GridEX grd;

	internal UltraCombo cmbLocation;

	private Label label9;

	internal UltraCombo cmbSection;

	internal UltraCombo cmbDepartment;

	internal UltraCombo cmbDesignation;

	private DateTimePicker txtToTime;

	private Label label8;

	private DateTimePicker txtFromTime;

	private Label label7;

	private Panel panel4;

	private Label label15;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStrip miniToolStrip;

	private Label label13;

	private Panel panel7;

	private Panel panel10;

	private Panel panel12;

	private Panel panel11;

	internal GridEX datagrid;

	private Label label16;

	private Button btnAdd;

	private DateTimePicker datLeaveDate;

	private Label label10;

	private TextBox txtNoofDays;

	private GroupBox groupBox2;

	private Button btnEmployeeRegister;

	private RichTextBox txtReason;

	private Label label12;

	internal UltraCombo cmbEmployee;

	private Label label17;

	private Label label22;

	private GroupBox groupBox1;

	private TextBox txtCPLBalance;

	private Label label23;

	private Label label24;

	private Label label25;

	private Label label26;

	private TextBox txtEmployeeNo;

	private Label label27;

	private Panel panel16;

	private Panel panel15;

	private Panel panel14;

	private Panel panel13;

	private Label label1;

	private Label label2;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar3;

	public frmEmployeeCPLRequest(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmItemPricingSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			dtGrid.Columns.Add("EmployeeLeaveDetailId");
			dtGrid.Columns.Add("LeaveDate");
			datagrid.DataSource = dtGrid;
			datagrid.RetrieveStructure();
			EmployeeNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dt = new DataTable
			{
				Columns = { "Id", "Name" }
			};
			DataTable dtItemCategory = genEmployee.Getall(val);
			if (dtItemCategory.Rows.Count > 0)
			{
				for (int i = 0; i < dtItemCategory.Rows.Count; i++)
				{
					dt.Rows.Add(dtItemCategory.Rows[i]["EmployeeId"], dtItemCategory.Rows[i]["EmployeeName"]);
				}
				DDL.BindDDL(dt, cmbEmployee, "Id", "Name", "Employee Name", ZeroIndex: false);
				((UltraGridBase)cmbEmployee).DisplayLayout.Bands[0].Columns[1].Width = 261;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetEmployeeHistory(int EmployeeId)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		try
		{
			if (EmployeeId <= 0)
			{
				return;
			}
			DataTable dtDesignation = new DataTable();
			dtDesignation.Columns.Add("Id");
			dtDesignation.Columns.Add("Name");
			DataTable dtDepartment = new DataTable();
			dtDepartment.Columns.Add("Id");
			dtDepartment.Columns.Add("Name");
			DataTable dtSection = new DataTable();
			dtSection.Columns.Add("Id");
			dtSection.Columns.Add("Name");
			DataTable dtLocation = new DataTable();
			dtLocation.Columns.Add("Id");
			dtLocation.Columns.Add("Name");
			DataTable dtEmployeeHistory = genEmployeeHistory.GetAllActiveEmployee(new genEmployeeHistory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtEmployeeHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtEmployeeHistory.Rows.Count; i++)
				{
					if (EmployeeId == Conversion.ToInt(dtEmployeeHistory.Rows[i]["EmployeeId"]))
					{
						txtEmployeeNo.Text = Conversion.ToString(dtEmployeeHistory.Rows[i]["EmployeeNo"]);
						dtDesignation.Rows.Add(dtEmployeeHistory.Rows[i]["DesignationId"], dtEmployeeHistory.Rows[i]["DesignationName"]);
						dtDepartment.Rows.Add(dtEmployeeHistory.Rows[i]["DepartmentId"], dtEmployeeHistory.Rows[i]["DepartmentName"]);
						dtSection.Rows.Add(dtEmployeeHistory.Rows[i]["SectionId"], dtEmployeeHistory.Rows[i]["SectionName"]);
						dtLocation.Rows.Add(dtEmployeeHistory.Rows[i]["LocationId"], dtEmployeeHistory.Rows[i]["LocationName"]);
						EmployeeId = 0;
					}
				}
				DDL.BindDDL(dtDesignation, cmbDesignation, "Id", "Name", "Designation", ZeroIndex: false);
				((UltraGridBase)cmbDesignation).Rows[0].Activate();
				DDL.BindDDL(dtDepartment, cmbDepartment, "Id", "Name", "Department", ZeroIndex: false);
				((UltraGridBase)cmbDepartment).Rows[0].Activate();
				DDL.BindDDL(dtSection, cmbSection, "Id", "Name", "Section", ZeroIndex: false);
				((UltraGridBase)cmbSection).Rows[0].Activate();
				DDL.BindDDL(dtLocation, cmbLocation, "Id", "Name", "Location", ZeroIndex: false);
				((UltraGridBase)cmbLocation).Rows[0].Activate();
			}
			else
			{
				txtEmployeeNo.Text = string.Empty;
				((UltraGridBase)cmbDesignation).DataSource = null;
				((Control)(object)cmbDesignation).Text = string.Empty;
				((UltraGridBase)cmbDepartment).DataSource = null;
				((Control)(object)cmbDepartment).Text = string.Empty;
				((UltraGridBase)cmbSection).DataSource = null;
				((Control)(object)cmbSection).Text = string.Empty;
				((UltraGridBase)cmbLocation).DataSource = null;
				((Control)(object)cmbLocation).Text = string.Empty;
			}
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

	private void Reset()
	{
		try
		{
			((Control)(object)cmbEmployee).Text = string.Empty;
			txtFromTime.Value = DateTime.Now;
			txtToTime.Value = DateTime.Now;
			txtReason.Text = string.Empty;
			txtEmployeeNo.Text = string.Empty;
			((Control)(object)cmbDesignation).Text = string.Empty;
			((Control)(object)cmbDepartment).Text = string.Empty;
			((Control)(object)cmbSection).Text = string.Empty;
			((Control)(object)cmbLocation).Text = string.Empty;
			txtNoofDays.Text = string.Empty;
			txtCPLBalance.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			RecId = 0;
			datagrid.ClearStructure();
			grd.ClearStructure();
			grd.DataSource = null;
			datagrid.DataSource = null;
			cmbEmployee.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Expected O, but got Unknown
		try
		{
			double CplQuota = 0.0;
			GridEXRow[] rows;
			if (grd.GetRows().Count() > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToBool(item.Cells["Active"].Value))
					{
						CplQuota += Conversion.ToDouble(item.Cells["CPLQuota"].Value);
					}
				}
			}
			int num = datagrid.GetRows().Count();
			if (num == 0)
			{
				throw new Exception("Please add record first in grid");
			}
			if ((double)num != CplQuota)
			{
				throw new Exception("Noofleaves cannot be equal to sum of CPL of grid");
			}
			if (!FormValiadation())
			{
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
			genEmployeeLeave obj = new genEmployeeLeave();
			if (RecId > 0)
			{
				obj.EmployeeLeaveId = RecId;
			}
			obj.LocationId = Conversion.ToInt(cmbLocation.Value);
			obj.DocMovementId = 1;
			obj.EmployeeId = Conversion.ToInt(cmbEmployee.Value);
			obj.LeaveTypeProfileId = 13;
			obj.EmployeeLeaveQuotaId = 0L;
			obj.FromDate = Conversion.ToDateTime((object)txtFromTime.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToTime.Value);
			obj.NoOfDays = Conversion.ToInt((object)(txtToTime.Value.Day - txtFromTime.Value.Day));
			obj.Reason = Conversion.ToString((object)txtReason.Text);
			obj.ApprovedById = UserAccount.ID;
			obj.ApprovedOn = DateTime.Now;
			obj.CreatedById = UserAccount.ID;
			obj.CreatedOn = DateTime.Now;
			obj.AlteredById = UserAccount.ID;
			obj.AlteredOn = DateTime.Now;
			obj.ActionTypeId = 1;
			obj.UserLogId = UserAccount.ID;
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.genEmployeeLeaveDetailList = new List<genEmployeeLeaveDetail>();
			obj.CPLEmployeeLeaveDetailslist = new List<CPLEmployeeLeaveDetail>();
			rows = datagrid.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				genEmployeeLeaveDetail obj3 = new genEmployeeLeaveDetail();
				obj3.EmployeeLeaveDetailId = Conversion.ToInt64(r2.Cells["EmployeeLeaveDetailId"].Value);
				obj3.LeaveDate = Conversion.ToDateTime(r2.Cells["LeaveDate"].Value);
				obj3.IsApproved = false;
				obj3.CreatedById = UserAccount.ID;
				obj3.CreatedOn = DateTime.Now;
				obj3.AlteredById = UserAccount.ID;
				obj3.AlteredOn = DateTime.Now;
				obj3.UserLogId = UserAccount.ID;
				if (obj3.EmployeeLeaveDetailId > 0)
				{
					obj3.ActionTypeId = 2;
				}
				else
				{
					obj3.ActionTypeId = 1;
				}
				obj.genEmployeeLeaveDetailList.Add(obj3);
			}
			rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				CPLEmployeeLeaveDetail obj2 = new CPLEmployeeLeaveDetail();
				if (Conversion.ToBool(r.Cells["Active"].Value))
				{
					obj2.CPLAttendanceId = Conversion.ToInt64(r.Cells["CPLAttendanceId"].Value);
					obj2.ActionTypeId = 1;
					obj.CPLEmployeeLeaveDetailslist.Add(obj2);
				}
			}
			if (Deletelst.Count > 0)
			{
				for (int i = 0; i < Deletelst.Count; i++)
				{
					obj.genEmployeeLeaveDetailList.Add(Deletelst[i]);
				}
			}
			long Id = genEmployeeLeave.Save(obj);
			if (RecId == 0 && Id > 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DatabaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmItemPricingSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
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
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetByItemId(int EmployeeLeaveId)
	{
		try
		{
			genEmployeeLeave obj = genEmployeeLeave.GetByID(EmployeeLeaveId);
			if (obj != null)
			{
				RecId = Conversion.ToInt((object)obj.EmployeeLeaveId);
				cmbEmployee.Value = Conversion.ToInt((object)obj.EmployeeId);
				txtFromTime.Value = Conversion.ToDateTime((object)obj.FromDate);
				txtToTime.Value = Conversion.ToDateTime((object)obj.ToDate);
				txtReason.Text = Conversion.ToString((object)obj.Reason);
				dtGrid = new DataTable();
				dtGrid.Columns.Add("EmployeeLeaveDetailId");
				dtGrid.Columns.Add("LeaveDate");
				for (int i = 0; i < obj.genEmployeeLeaveDetailList.Count; i++)
				{
					dtGrid.Rows.Add(obj.genEmployeeLeaveDetailList[i].EmployeeLeaveDetailId, obj.genEmployeeLeaveDetailList[i].LeaveDate);
				}
				datagrid.DataSource = dtGrid;
				datagrid.RetrieveStructure();
				if (obj.CPLEmployeeLeaveDetailslist.Count > 0)
				{
					grd.DataSource = obj.CPLEmployeeLeaveDetailslist;
					grd.RetrieveStructure();
					GridSetting();
				}
				datagrid.RootTable.Columns["EmployeeLeaveDetailId"].Visible = false;
				btnUpdate.Visible = true;
				btnsave.Visible = false;
				tabControl1.SelectedIndex = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			DateTime getFromDate = Conversion.ToDateTime((object)txtFromTime.Value);
			DateTime getToDate = Conversion.ToDateTime((object)txtToTime.Value);
			DateTime getLeaveDate = Conversion.ToDateTime((object)datLeaveDate.Value);
			DateTime FromDate = new DateTime(getFromDate.Year, getFromDate.Month, getFromDate.Day);
			DateTime ToDate = new DateTime(getToDate.Year, getToDate.Month, getToDate.Day);
			DateTime LeaveDate = new DateTime(getLeaveDate.Year, getLeaveDate.Month, getLeaveDate.Day);
			if (btnAdd.Text == "Add")
			{
				if (!(LeaveDate >= FromDate) || !(LeaveDate <= ToDate))
				{
					throw new Exception("LeaveDate must between FromDate And ToDate Please Check");
				}
				dtGrid.Rows.Add(0, LeaveDate);
			}
			if (btnAdd.Text == "Update")
			{
				if (!(LeaveDate >= FromDate) || !(LeaveDate <= ToDate))
				{
					throw new Exception("LeaveDate must between FromDate And ToDate Please Check");
				}
				dtGrid.Rows[updateDetailIndex]["LeaveDate"] = getLeaveDate;
				btnAdd.Text = "Add";
			}
			datagrid.DataSource = dtGrid;
			datagrid.RetrieveStructure();
			datagrid.RootTable.Columns["EmployeeLeaveDetailId"].Visible = false;
			datagrid.RootTable.Columns.Add("Delete");
			datagrid.RootTable.Columns["Delete"].Caption = "Delete";
			datagrid.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			datagrid.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			datagrid.RootTable.Columns["Delete"].Image = Resources.icons8_delete_bin_16;
			datagrid.RootTable.Columns["Delete"].Width = 35;
			datLeaveDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void GridSetting()
	{
		try
		{
			grd.ColumnAutoResize = true;
			grd.AllowEdit = (InheritableBoolean)1;
			double CPLBalance = 0.0;
			grd.RootTable.Columns["EmployeeLeaveId"].Visible = false;
			grd.RootTable.Columns["CPLEmployeeLeaveId"].Visible = false;
			grd.RootTable.Columns["CPLAttendanceId"].Visible = false;
			grd.RootTable.Columns["ActionTypeId"].Visible = false;
			grd.RootTable.Columns["CPLAttendanceId"].EditType = (EditType)0;
			grd.RootTable.Columns["CPLQuota"].EditType = (EditType)0;
			grd.RootTable.Columns["WeekDayName"].EditType = (EditType)0;
			grd.RootTable.Columns["CPLDate"].EditType = (EditType)0;
			grd.RootTable.Columns["CPLEmployeeLeaveId"].EditType = (EditType)0;
			grd.RootTable.Columns["EmployeeLeaveId"].EditType = (EditType)0;
			grd.RootTable.Columns["ActionTypeId"].EditType = (EditType)0;
			CPLBalance = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["CPLQuota"], (AggregateFunction)2));
			txtCPLBalance.Text = CPLBalance.ToString();
			grd.RootTable.Columns["Active"].EditType = (EditType)8;
			grd.RootTable.Columns["Active"].UseHeaderSelector = true;
			grd.AllowDelete = (InheritableBoolean)2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void datagrid_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = datagrid.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				datLeaveDate.Value = Conversion.ToDateTime(item.Cells["LeaveDate"].Value);
				btnAdd.Text = "Update";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datagrid_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		try
		{
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow r = datagrid.CurrentRow;
			if (r != null)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt64(r.Cells["EmployeeLeaveDetailId"].Value) > 0)
				{
					genEmployeeLeaveDetail obj1 = new genEmployeeLeaveDetail();
					obj1.EmployeeLeaveDetailId = Conversion.ToInt64(r.Cells["EmployeeLeaveDetailId"].Value);
					obj1.LeaveDate = Conversion.ToDateTime(r.Cells["LeaveDate"].Value);
					obj1.IsApproved = false;
					obj1.CreatedById = UserAccount.ID;
					obj1.CreatedOn = DateTime.Now;
					obj1.AlteredById = UserAccount.ID;
					obj1.AlteredOn = DateTime.Now;
					obj1.UserLogId = UserAccount.ID;
					obj1.ActionTypeId = 3;
					Deletelst.Add(obj1);
				}
			}
			dtGrid.Rows[r.RowIndex].Delete();
			if (r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DataBaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void cmbEmployee_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GetEmployeeHistory(Conversion.ToInt(cmbEmployee.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbEmployee_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			DataTable dt = CPLAttendance.GetCPLRecordForEmployeeLeaveRequest(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EmployeeId = Conversion.ToInt(cmbEmployee.Value)
			});
			if (dt.Rows.Count > 0)
			{
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private bool FormValiadation()
	{
		if (((UltraGridBase)cmbEmployee).ActiveRow == null)
		{
			cmbEmployee.Focus();
			MessageBox.Show("Employee Required");
			return false;
		}
		if (txtReason.Text == string.Empty)
		{
			txtReason.Focus();
			MessageBox.Show("Reason Required");
			return false;
		}
		return true;
	}

	private void FormHistory()
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("EmployeeLeaveId");
			dt.Columns.Add("EmployeeName");
			dt.Columns.Add("LocationName");
			dt.Columns.Add("LeaveType");
			dt.Columns.Add("FromDate");
			dt.Columns.Add("ToDate");
			dt.Columns.Add("NoOfDays");
			dt.Columns.Add("Reason");
			DataTable dtHistory = genEmployeeLeave.ReadAll(new genEmployeeLeave
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				LeaveTypeProfileId = 13
			});
			if (dtHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["EmployeeLeaveId"], dtHistory.Rows[i]["EmployeeName"], dtHistory.Rows[i]["LocationName"], dtHistory.Rows[i]["LeaveType"], dtHistory.Rows[i]["FromDate"], dtHistory.Rows[i]["ToDate"], dtHistory.Rows[i]["NoOfDays"], dtHistory.Rows[i]["Reason"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["EmployeeLeaveId"].Visible = false;
			ctrlGrdBar3_Load(null, null);
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
			if (tabControl1.SelectedIndex == 1)
			{
				FormHistory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				GetByItemId(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["EmployeeLeaveId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromTime_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			int NoOfDays = Conversion.ToInt((object)(txtToTime.Value.Day - txtFromTime.Value.Day)) + 1;
			txtNoofDays.Text = NoOfDays.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtToTime_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			int NoOfDays = Conversion.ToInt((object)(txtToTime.Value.Day - txtFromTime.Value.Day)) + 1;
			txtNoofDays.Text = NoOfDays.ToString();
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)datagrid).Name);
			if (fs != null)
			{
				datagrid.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnEmployeeRegister_Click(object sender, EventArgs e)
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

	private void frmEmployeeCPLRequest_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Expected O, but got Unknown
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Expected O, but got Unknown
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Expected O, but got Unknown
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Expected O, but got Unknown
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		//IL_0ec1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecb: Expected O, but got Unknown
		//IL_3afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b06: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout datagrid_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.LeaveManagment.frmEmployeeCPLRequest));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.datagrid = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnEmployeeRegister = new System.Windows.Forms.Button();
		this.txtReason = new System.Windows.Forms.RichTextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtNoofDays = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.cmbEmployee = new UltraCombo();
		this.datLeaveDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtFromTime = new System.Windows.Forms.DateTimePicker();
		this.txtToTime = new System.Windows.Forms.DateTimePicker();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtCPLBalance = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtEmployeeNo = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.cmbDesignation = new UltraCombo();
		this.cmbDepartment = new UltraCombo();
		this.cmbLocation = new UltraCombo();
		this.cmbSection = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label13 = new System.Windows.Forms.Label();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel15.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.datagrid).BeginInit();
		this.panel13.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel8.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1025, 609);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1025, 609);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1017, 583);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1011, 577);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 56);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1011, 521);
		this.panel5.TabIndex = 2;
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 185);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1011, 336);
		this.panel10.TabIndex = 610;
		this.panel12.Controls.Add(this.panel16);
		this.panel12.Controls.Add(this.panel15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(487, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(524, 336);
		this.panel12.TabIndex = 2;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(0, 25);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(524, 311);
		this.panel16.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(524, 311);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(524, 25);
		this.panel15.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(478, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 28;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(96, 17);
		this.label1.TabIndex = 27;
		this.label1.Text = "CPL Request";
		this.panel11.Controls.Add(this.panel14);
		this.panel11.Controls.Add(this.panel13);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(487, 336);
		this.panel11.TabIndex = 1;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.datagrid);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 25);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(487, 311);
		this.panel14.TabIndex = 2;
		this.datagrid.AllowDelete = (InheritableBoolean)1;
		this.datagrid.AllowEdit = (InheritableBoolean)2;
		this.datagrid.ColumnAutoResize = true;
		datagrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.datagrid.DesignTimeLayout = datagrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.datagrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.datagrid.DynamicFiltering = true;
		this.datagrid.FilterMode = (FilterMode)2;
		this.datagrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.datagrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.datagrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.datagrid).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.datagrid).Name = "datagrid";
		((ScrollableBase)this.datagrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagrid).Size = new System.Drawing.Size(487, 311);
		((System.Windows.Forms.Control)(object)this.datagrid).TabIndex = 0;
		this.datagrid.TotalRow = (InheritableBoolean)1;
		this.datagrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagrid).VisualStyle = (VisualStyle)4;
		this.datagrid.ColumnButtonClick += new ColumnActionEventHandler(datagrid_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.datagrid).DoubleClick += new System.EventHandler(datagrid_DoubleClick);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar2);
		this.panel13.Controls.Add(this.label2);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(487, 25);
		this.panel13.TabIndex = 1;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(441, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.datagrid;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar2.TabIndex = 29;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(7, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(96, 17);
		this.label2.TabIndex = 28;
		this.label2.Text = "CPL Request";
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Controls.Add(this.groupBox1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1011, 185);
		this.panel7.TabIndex = 609;
		this.groupBox2.Controls.Add(this.btnEmployeeRegister);
		this.groupBox2.Controls.Add(this.txtReason);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.txtNoofDays);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.btnAdd);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployee);
		this.groupBox2.Controls.Add(this.datLeaveDate);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.txtFromTime);
		this.groupBox2.Controls.Add(this.txtToTime);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 8);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(601, 168);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Advance Information";
		this.btnEmployeeRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnEmployeeRegister.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeRegister.Location = new System.Drawing.Point(174, 42);
		this.btnEmployeeRegister.Name = "btnEmployeeRegister";
		this.btnEmployeeRegister.Size = new System.Drawing.Size(27, 23);
		this.btnEmployeeRegister.TabIndex = 1;
		this.btnEmployeeRegister.TabStop = false;
		this.btnEmployeeRegister.UseVisualStyleBackColor = true;
		this.btnEmployeeRegister.Click += new System.EventHandler(btnEmployeeRegister_Click);
		this.txtReason.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReason.Location = new System.Drawing.Point(7, 91);
		this.txtReason.Name = "txtReason";
		this.txtReason.Size = new System.Drawing.Size(488, 71);
		this.txtReason.TabIndex = 6;
		this.txtReason.Text = "";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(494, 20);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(65, 15);
		this.label16.TabIndex = 619;
		this.label16.Text = "Leave Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(422, 20);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(66, 15);
		this.label10.TabIndex = 621;
		this.label10.Text = "No of Days";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(94, 15);
		this.label12.TabIndex = 19;
		this.label12.Text = "Employee Name";
		this.txtNoofDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoofDays.Location = new System.Drawing.Point(422, 42);
		this.txtNoofDays.Name = "txtNoofDays";
		this.txtNoofDays.Size = new System.Drawing.Size(69, 23);
		this.txtNoofDays.TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(204, 20);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(63, 15);
		this.label7.TabIndex = 593;
		this.label7.Text = "From Date";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(313, 20);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(48, 15);
		this.label8.TabIndex = 595;
		this.label8.Text = "To Date";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(501, 133);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(67, 29);
		this.btnAdd.TabIndex = 7;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.cmbEmployee.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Location = new System.Drawing.Point(7, 40);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Name = "cmbEmployee";
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TabIndex = 0;
		this.datLeaveDate.CustomFormat = "dd-MMM-yy";
		this.datLeaveDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datLeaveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datLeaveDate.Location = new System.Drawing.Point(494, 42);
		this.datLeaveDate.Name = "datLeaveDate";
		this.datLeaveDate.Size = new System.Drawing.Size(104, 23);
		this.datLeaveDate.TabIndex = 5;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(7, 71);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(45, 15);
		this.label17.TabIndex = 29;
		this.label17.Text = "Reason";
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.White;
		this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(96, 23);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(15, 20);
		this.label22.TabIndex = 103;
		this.label22.Text = "*";
		this.txtFromTime.CustomFormat = "dd-MMM-yy";
		this.txtFromTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromTime.Location = new System.Drawing.Point(204, 42);
		this.txtFromTime.Name = "txtFromTime";
		this.txtFromTime.Size = new System.Drawing.Size(106, 23);
		this.txtFromTime.TabIndex = 2;
		this.txtFromTime.ValueChanged += new System.EventHandler(txtFromTime_ValueChanged);
		this.txtToTime.CustomFormat = "dd-MMM-yy";
		this.txtToTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToTime.Location = new System.Drawing.Point(313, 42);
		this.txtToTime.Name = "txtToTime";
		this.txtToTime.Size = new System.Drawing.Size(106, 23);
		this.txtToTime.TabIndex = 3;
		this.txtToTime.ValueChanged += new System.EventHandler(txtToTime_ValueChanged);
		this.groupBox1.Controls.Add(this.txtCPLBalance);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtEmployeeNo);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDesignation);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocation);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSection);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(610, 8);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(351, 168);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Employee Information";
		this.txtCPLBalance.BackColor = System.Drawing.Color.White;
		this.txtCPLBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCPLBalance.Location = new System.Drawing.Point(261, 20);
		this.txtCPLBalance.Name = "txtCPLBalance";
		this.txtCPLBalance.ReadOnly = true;
		this.txtCPLBalance.Size = new System.Drawing.Size(86, 23);
		this.txtCPLBalance.TabIndex = 1;
		this.txtCPLBalance.TabStop = false;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(166, 24);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(71, 15);
		this.label23.TabIndex = 29;
		this.label23.Text = "CPL Balance";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(5, 111);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(47, 15);
		this.label24.TabIndex = 29;
		this.label24.Text = "Section";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(5, 52);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(71, 15);
		this.label25.TabIndex = 29;
		this.label25.Text = "Department";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(5, 81);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(71, 15);
		this.label26.TabIndex = 29;
		this.label26.Text = "Desgination";
		this.txtEmployeeNo.BackColor = System.Drawing.Color.White;
		this.txtEmployeeNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmployeeNo.Location = new System.Drawing.Point(83, 20);
		this.txtEmployeeNo.Name = "txtEmployeeNo";
		this.txtEmployeeNo.ReadOnly = true;
		this.txtEmployeeNo.Size = new System.Drawing.Size(80, 23);
		this.txtEmployeeNo.TabIndex = 0;
		this.txtEmployeeNo.TabStop = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 24);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(78, 15);
		this.label27.TabIndex = 29;
		this.label27.Text = "Employee No";
		this.cmbDesignation.AllowNull = (DefaultableBoolean)1;
		this.cmbDesignation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDesignation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDesignation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDesignation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Location = new System.Drawing.Point(83, 75);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Name = "cmbDesignation";
		this.cmbDesignation.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).TabStop = false;
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(83, 46);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		this.cmbDepartment.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabStop = false;
		this.cmbLocation.AllowNull = (DefaultableBoolean)1;
		this.cmbLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocation).Location = new System.Drawing.Point(83, 136);
		((System.Windows.Forms.Control)(object)this.cmbLocation).Name = "cmbLocation";
		this.cmbLocation.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbLocation).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocation).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbLocation).TabStop = false;
		this.cmbSection.AllowNull = (DefaultableBoolean)1;
		this.cmbSection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSection).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Location = new System.Drawing.Point(83, 105);
		((System.Windows.Forms.Control)(object)this.cmbSection).Name = "cmbSection";
		this.cmbSection.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.cmbSection).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbSection).TabStop = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 142);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(53, 15);
		this.label9.TabIndex = 607;
		this.label9.Text = "Location";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1011, 28);
		this.panel4.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(7, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(112, 19);
		this.label15.TabIndex = 26;
		this.label15.Text = "CPL Request";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1011, 28);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1011, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1017, 583);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "history";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1011, 577);
		this.panel6.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 31);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1011, 546);
		this.panel9.TabIndex = 1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.ColumnAutoResize = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1011, 546);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar3);
		this.panel8.Controls.Add(this.label13);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1011, 31);
		this.panel8.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(964, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar3.TabIndex = 28;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(5, 4);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(175, 19);
		this.label13.TabIndex = 27;
		this.label13.Text = "CPL Request History";
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(194, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1257, 27);
		this.miniToolStrip.TabIndex = 7;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1025, 609);
		base.Controls.Add(this.panel1);
		base.Name = "frmEmployeeCPLRequest";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEmployeeCPLRequest_FormClosing);
		base.Load += new System.EventHandler(frmItemPricingSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemPricingSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.datagrid).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		base.ResumeLayout(false);
	}
}
