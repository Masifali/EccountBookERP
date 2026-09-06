using System;
using System.Collections;
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
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.AttendanceManagement;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.EmployeeManagement;
using Architecture.WinApp.HRM.ProfileManagement;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.AttendanceManagement;

public class frmDutyRoasterOldVersion : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtShift = new DataTable();

	private DataTable dtWeekProfile = new DataTable();

	public Rightsobjects formright = new Rightsobjects();

	private DataTable dt = new DataTable();

	private List<genDutyRoasterDetail> lstDutyRoasterDetail = new List<genDutyRoasterDetail>();

	private long RecId;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel8;

	private Panel panel10;

	private Label label4;

	private CheckBox checkBox1;

	private CheckBox checkBox2;

	private CheckBox checkBox3;

	private CheckBox checkBox4;

	private CheckBox checkBox5;

	internal GridEX datagridHistory;

	private TabPage tabPage3;

	private Panel panel3;

	private Panel panel5;

	private Panel panel4;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnPrint;

	private ToolStripButton btnRefresh;

	private TextBox txtDescription;

	private GroupBox groupBox1;

	private Button btnLoad;

	internal UltraCombo cmbSection;

	private Label label9;

	internal UltraCombo cmbDepartment;

	private Label label8;

	internal UltraCombo cmbEmployeeCategory;

	private Label label3;

	private Label label12;

	private DateTimePicker txtFromDate;

	private Label label7;

	private Label label6;

	private DateTimePicker txtTodate;

	private ToolStrip miniToolStrip;

	internal GridEX grdEmployee;

	private Label lblDutyDate;

	private Panel panel12;

	internal GridEX grdGroup;

	internal GridEX grdDutyDate;

	private Button btnApply;

	internal UltraCombo CmbEmployeeName;

	private Label label1;

	internal UltraCombo CmbLocation;

	private Label label2;

	private Panel panel14;

	private Panel panel13;

	private Label label5;

	private GroupBox groupBox2;

	private GroupBox groupBox3;

	private Panel panel15;

	private Panel panel11;

	private Label label10;

	private Panel panel16;

	private Panel panel18;

	private Panel panel20;

	private Panel panel17;

	private Panel panel19;

	private Label label11;

	private Panel panel21;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX datagridhistoryDetail;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar4;

	private Label label14;

	private CheckBox checkBox6;

	private CheckBox checkBox7;

	private CheckBox checkBox8;

	private CheckBox checkBox9;

	private CheckBox checkBox10;

	private Button btnEmployeeDefine;

	private Button btnLocation;

	private Button btnSection;

	private Button btnDepartment;

	private Button btnEmployeeCategory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnShifttimingorm;

	private Label label13;

	private Button btnApplyAll;

	private ToolStripButton btnSaveAs;

	private Button BtnUncheckedAll;

	private Button BtnCheckAll;

	internal UltraCombo CmbShifts;

	private Label label15;

	public frmDutyRoasterOldVersion(UserAccount userAccount)
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
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 500);
			formright = CommonServices.SetRightsValueInRightsObject("DutyRoaster");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar4.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			txtFromDate.Focus();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnSaveAs.Visible = false;
			dtShift.Columns.Add("ShiftId");
			dtShift.Columns.Add("ShiftName");
			EmployeeCategoryFill();
			DepartmentFill();
			SectionFill();
			ShiftFill();
			WeekDaysFill();
			EmployeeName();
			LocationFilterFill();
			lblDutyDate.Text = string.Empty;
			DateTime currentMonthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			txtFromDate.Value = currentMonthStartDate;
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
			EmployeeCategoryFill();
			DepartmentFill();
			SectionFill();
			ShiftFill();
			EmployeeName();
			LocationFilterFill();
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
			RecId = 0L;
			((Control)(object)cmbEmployeeCategory).Text = string.Empty;
			((Control)(object)cmbDepartment).Text = string.Empty;
			((Control)(object)cmbSection).Text = string.Empty;
			txtDescription.Text = string.Empty;
			txtFromDate.Focus();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnSaveAs.Visible = false;
			lblDutyDate.Text = string.Empty;
			((Control)(object)CmbEmployeeName).Text = string.Empty;
			grdDutyDate.ClearStructure();
			grdGroup.ClearStructure();
			grdEmployee.ClearStructure();
			lstDutyRoasterDetail.Clear();
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
				DDL.BindDDLNew(dt, cmbEmployeeCategory, "EmployeeCategoryId", "EmployeeCategoryName", "Employee Category", false);
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

	private void ShiftFill()
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
				dtShift.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtShift.Rows.Add(dt.Rows[i]["ShiftId"], dt.Rows[i]["ShiftName"]);
				}
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
		//IL_0085: Expected O, but got Unknown
		try
		{
			DataTable dtemployee = genEmployeeHistory.GetAllEmployeesActive(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SectionId = Conversion.ToInt(cmbSection.Value),
				EmployeeCategoryId = Conversion.ToInt(cmbEmployeeCategory.Value),
				PartyLocationId = Conversion.ToInt(CmbLocation.Value),
				DepartmentId = Conversion.ToInt(cmbDepartment.Value)
			});
			if (dtemployee.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtemployee, CmbEmployeeName, "EmployeeId", "EmployeeName", "Employee Name", false);
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
		//IL_0006: Expected O, but got Unknown
		try
		{
			genProfile obj = new genProfile();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.IsActive = true;
			obj.ProfileTypeId = 60;
			dtWeekProfile = genProfile.GetByProfileTypeId(obj);
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

	private void cmbEmployeeCategory_Leave(object sender, EventArgs e)
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

	private void Insert()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		try
		{
			genDutyRoaster obj = new genDutyRoaster();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.DutyRoasterId = RecId;
				obj.ActionTypeId = 2;
			}
			else
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.ActionTypeId = 1;
			}
			lstDutyRoasterDetail.RemoveAll((genDutyRoasterDetail item) => !item.IsOnDuty);
			int TotalDaysDiff = Conversion.ToInt((object)GetDateDifference(Convert.ToDateTime(txtFromDate.Value), Convert.ToDateTime(txtTodate.Value)));
			if (TotalDaysDiff == 0 || TotalDaysDiff < 0)
			{
				MannualDataSetInDutyRoaster();
				throw new Exception("DutyRoasterDetail not found Please Check");
			}
			obj.FromDate = txtFromDate.Value;
			obj.ToDate = txtTodate.Value;
			obj.CreatedOn = DateTime.Now;
			obj.AlteredOn = DateTime.Now;
			obj.CreatedById = UserAccount.ID;
			obj.AlteredById = UserAccount.ID;
			obj.Description = txtDescription.Text.Trim();
			obj.EmployeeCategoryId = Conversion.ToInt(cmbEmployeeCategory.Value);
			obj.DepartmentId = Conversion.ToInt(cmbDepartment.Value);
			obj.SectionId = Conversion.ToInt(cmbSection.Value);
			obj.LocationId = Conversion.ToInt(CmbLocation.Value);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.GenDutyRoasterDetailsList = new List<genDutyRoasterDetail>();
			obj.DutyRoasterGroupsList = new List<DutyRoasterGroups>();
			obj.GenDutyRoasterDetailsList = lstDutyRoasterDetail;
			MakeOffDaysRecords(obj);
			if (RecId > 0)
			{
				DutyRoasterDetailIdsUpdateForUpdate(obj);
			}
			GridEXRow[] rows = grdGroup.GetRows();
			foreach (GridEXRow item2 in rows)
			{
				DutyRoasterGroups groups = new DutyRoasterGroups();
				if (Conversion.ToInt(item2.Cells["EmployeeGroupId"].Value) > 0 && Conversion.ToInt(item2.Cells["ShiftId"].Value) > 0 && Conversion.ToInt(item2.Cells["WeekDayName"].Value) > 0)
				{
					groups.EmployeeGroupId = Conversion.ToInt(item2.Cells["EmployeeGroupId"].Value);
					groups.ShiftId = Conversion.ToInt(item2.Cells["ShiftId"].Value);
					groups.WeekDayId = Conversion.ToInt(item2.Cells["WeekDayName"].Value);
					obj.DutyRoasterGroupsList.Add(groups);
				}
			}
			genDutyRoaster.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update");
			}
			else
			{
				MessageBox.Show("Record Saved");
			}
			Reset();
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
			RecId = 0L;
			Insert();
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
			RecId = 0L;
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

	private void btnLoad_Click(object sender, EventArgs e)
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

	private void MannualDataSetInDutyRoaster()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbShifts.Value);
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = txtFromDate.Value,
				ToDate = txtTodate.Value,
				EmployeeId = Conversion.ToInt(CmbEmployeeName.Value),
				DepartmentId = Conversion.ToInt(cmbDepartment.Value),
				SectionId = Conversion.ToInt(cmbSection.Value),
				EmployeeCategoryId = Conversion.ToInt(cmbEmployeeCategory.Value),
				EmployeeId = Conversion.ToInt(CmbEmployeeName.Value),
				PartyLocationId = Conversion.ToInt(CmbLocation.Value),
				DutyRoasterId = Conversion.ToInt((object)RecId)
			};
			lstDutyRoasterDetail = new List<genDutyRoasterDetail>();
			DataSet ds = genDutyRoaster.GetDutyDatesForDutyRoaster(val);
			if (ds.Tables.Count > 0)
			{
				DataTable dtDutyDate = ds.Tables[0];
				DataTable dtAll = ds.Tables[1];
				DataTable dtEmp = ds.Tables[2];
				DataTable dtGroup = ds.Tables[3];
				grdGroup.DataSource = dtGroup;
				grdGroup.RetrieveStructure();
				grdDutyDate.DataSource = dtDutyDate;
				grdDutyDate.RetrieveStructure();
				grdDutyDate.RootTable.Columns["HolidayDetail"].Visible = false;
				grdDutyDate.RootTable.Columns["IsOffDay"].Visible = false;
				grdDutyDate.RootTable.Columns["IsGazetted"].Visible = false;
				new GridEXColumn();
				GridEXFormatCondition fc = new GridEXFormatCondition(grdDutyDate.RootTable.Columns["HolidayDetail"], (ConditionOperator)0, (object)"Holiday");
				fc.FormatStyle.FontBold = (TriState)2;
				fc.FormatStyle.ForeColor = Color.Red;
				grdDutyDate.RootTable.FormatConditions.Add(fc);
				grdGroup.RootTable.Columns["EmployeeGroupName"].EditType = (EditType)0;
				grdGroup.RootTable.Columns["EmployeeGroupId"].Visible = false;
				grdGroup.RootTable.Columns["ShiftId"].ColumnType = (ColumnType)0;
				grdGroup.RootTable.Columns["ShiftId"].HasValueList = true;
				grdGroup.RootTable.Columns["ShiftId"].LimitToList = true;
				grdGroup.RootTable.Columns["ShiftId"].EditType = (EditType)4;
				grdGroup.RootTable.Columns["ShiftId"].Caption = "Shift";
				grdGroup.RootTable.Columns["ShiftId"].ValueList.PopulateValueList((IEnumerable)dtShift.DefaultView, "ShiftId", "ShiftName");
				grdGroup.RootTable.Columns["WeekDayName"].ColumnType = (ColumnType)0;
				grdGroup.RootTable.Columns["WeekDayName"].HasValueList = true;
				grdGroup.RootTable.Columns["WeekDayName"].LimitToList = true;
				grdGroup.RootTable.Columns["WeekDayName"].EditType = (EditType)4;
				grdGroup.RootTable.Columns["WeekDayName"].Caption = "RestDay";
				grdGroup.RootTable.Columns["WeekDayName"].ValueList.PopulateValueList((IEnumerable)dtWeekProfile.DefaultView, "ProfileId", "ProfileName");
				grdEmployee.DataSource = dtEmp;
				grdEmployee.RetrieveStructure();
				grdEmployee.RootTable.Columns["DefaultShift"].Visible = false;
				grdEmployee.RootTable.Columns["EmployeeId"].Visible = false;
				grdEmployee.RootTable.Columns["EmployeeGroupId"].Visible = false;
				grdEmployee.RootTable.Columns["EmployeeName"].Width = 250;
				grdEmployee.RootTable.Columns["DepartmentName"].Width = 120;
				grdEmployee.RootTable.Columns["DesignationName"].Width = 120;
				grdEmployee.AllowEdit = (InheritableBoolean)1;
				foreach (GridEXColumn item in (JanusCollectionBase)grdEmployee.RootTable.Columns)
				{
					GridEXColumn c = item;
					if (((JanusCollectionElement)c).Index >= 8)
					{
						c.ColumnType = (ColumnType)3;
					}
				}
				DataTable dtShiftcheckcombo = new DataTable();
				dtShiftcheckcombo.Columns.Add("Id", typeof(int));
				dtShiftcheckcombo.Columns.Add("ShiftShortName", typeof(string));
				if (dtAll.Rows.Count > 0)
				{
					int i;
					for (i = 0; i < dtAll.Rows.Count; i++)
					{
						if (!dtShiftcheckcombo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Convert.ToInt32(dtAll.Rows[i]["ShiftId"])))
						{
							dtShiftcheckcombo.Rows.Add(Convert.ToInt32(dtAll.Rows[i]["ShiftId"]), Convert.ToString(dtAll.Rows[i]["ShiftShortName"]));
						}
						genDutyRoasterDetail obj = new genDutyRoasterDetail();
						obj.EmployeeId = Convert.ToInt64(dtAll.Rows[i]["EmployeeId"]);
						obj.EmployeeHistoryId = Convert.ToInt64(dtAll.Rows[i]["EmployeeHistoryId"]);
						obj.ShiftId = Convert.ToInt32(dtAll.Rows[i]["ShiftId"]);
						obj.ShiftShortName = Convert.ToString(dtAll.Rows[i]["ShiftShortName"]);
						obj.DutyDate = Convert.ToDateTime(dtAll.Rows[i]["DutyDate"]);
						obj.WeekDayName = Convert.ToString(dtAll.Rows[i]["WeekDayName"]);
						obj.EmployeeGroupId = Convert.ToInt32(dtAll.Rows[i]["EmployeeGroupId"]);
						obj.IsOnDuty = Convert.ToBoolean(dtAll.Rows[i]["IsonDuty"]);
						obj.DutyRoasterDetailId = Conversion.ToInt64(dtAll.Rows[i]["DutyRoasterDetailId"]);
						obj.DutyRoasterId = Conversion.ToInt64(dtAll.Rows[i]["DutyRoasterId"]);
						obj.IsHoliday = Conversion.ToBool(dtAll.Rows[i]["IsHoliday"]);
						obj.DutyRoasterDetailLineId = i + 1;
						if (obj.DutyRoasterDetailId > 0)
						{
							obj.ActionTypeId = 2;
						}
						else
						{
							obj.ActionTypeId = 1;
						}
						obj.CreatedOn = DateTime.Now;
						obj.AlteredOn = DateTime.Now;
						obj.CreatedById = UserAccount.ID;
						obj.AlteredById = UserAccount.ID;
						lstDutyRoasterDetail.Add(obj);
					}
					if (dtShiftcheckcombo.Rows.Count > 0)
					{
						DDL.BindDDL(dtShiftcheckcombo, CmbShifts, "Id", "ShiftShortName", "Shift Name", ZeroIndex: true);
						((UltraGridBase)CmbShifts).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
						if (id > 0)
						{
							if (dtShiftcheckcombo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
							{
								CmbShifts.Value = id;
							}
							else
							{
								((Control)(object)CmbShifts).Text = string.Empty;
							}
						}
					}
				}
				DateTime objDutyDate = Conversion.ToDateTime(grdDutyDate.CurrentRow.Cells["DutyDate"].Value);
				lblDutyDate.Text = objDutyDate.ToString("dd-MMM-yyyy");
			}
			else
			{
				grdGroup.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDutyDateSetting()
	{
		try
		{
			grdDutyDate.RootTable.Columns["IsGazetted"].Visible = false;
			grdDutyDate.RootTable.Columns["HolidayDetail"].Visible = false;
			grdDutyDate.RootTable.Columns["RNo"].Width = 60;
			grdDutyDate.RootTable.Columns["WeekDay"].Width = 90;
			grdDutyDate.RootTable.Columns["OffDay"].Width = 70;
			grdDutyDate.RootTable.Columns["DutyDate"].Width = 80;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DutyRoasterDetailIdsUpdateForUpdate(genDutyRoaster obj)
	{
		try
		{
			obj.GenDutyRoasterDetailsList.Where((genDutyRoasterDetail x) => Conversion.ToInt((object)x.ActionTypeId) != 2).ToList().ForEach(delegate(genDutyRoasterDetail x)
			{
				x.ActionTypeId = 2;
			});
			DataTable dtDetail = genDutyRoaster.GetoffdaysfromDutyRoasterDetailByHeaderId(RecId);
			if (dtDetail.Rows.Count <= 0)
			{
				return;
			}
			int i;
			for (i = 0; i < dtDetail.Rows.Count; i++)
			{
				obj.GenDutyRoasterDetailsList.Where((genDutyRoasterDetail x) => Conversion.ToInt64((object)x.EmployeeId) == Conversion.ToInt64(dtDetail.Rows[i]["EmployeeId"]) && x.DutyDate == Conversion.ToDateTime(dtDetail.Rows[i]["DutyDate"])).ToList().ForEach(delegate(genDutyRoasterDetail x)
				{
					x.DutyRoasterDetailId = Conversion.ToInt64(dtDetail.Rows[i]["DutyRoasterDetailId"]);
				});
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
					DateTime getnextdate = obj.FromDate.AddDays(i);
					DateTime makedate = new DateTime(getnextdate.Year, getnextdate.Month, getnextdate.Day);
					lst.Add(makedate);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return lst;
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

	private void btnApply_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdGroup.GetRows().Length == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			int Countflag = 0;
			GridEXRow[] rows = grdGroup.GetRows();
			foreach (GridEXRow item2 in rows)
			{
				if (Conversion.ToInt(item2.Cells["EmployeeGroupId"].Value) > 0 && Conversion.ToInt(item2.Cells["ShiftId"].Value) > 0 && Conversion.ToInt(item2.Cells["WeekDayName"].Value) > 0)
				{
					lstDutyRoasterDetail.Where((genDutyRoasterDetail x) => !Conversion.ToBool((object)x.IsHoliday)).ToList().ForEach(delegate(genDutyRoasterDetail x)
					{
						x.IsOnDuty = false;
					});
					Countflag++;
					break;
				}
			}
			if (Countflag == 0)
			{
				throw new Exception("EmployeeGroup,ShiftName And WeekDayName must be select");
			}
			rows = grdGroup.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["EmployeeGroupId"].Value) > 0 && Conversion.ToInt(item.Cells["ShiftId"].Value) > 0 && Conversion.ToInt(item.Cells["WeekDayName"].Value) > 0)
				{
					int GroupId = Conversion.ToInt(item.Cells["EmployeeGroupId"].Value);
					int ShiftId = Conversion.ToInt(item.Cells["ShiftId"].Value);
					string WeekProfileName = Conversion.ToString((object)item.Cells["WeekDayName"].Text);
					lstDutyRoasterDetail.Where((genDutyRoasterDetail x) => Conversion.ToInt64((object)x.EmployeeGroupId) == GroupId && x.ShiftId == ShiftId && !x.IsHoliday).ToList().ForEach(delegate(genDutyRoasterDetail x)
					{
						x.IsOnDuty = true;
					});
					lstDutyRoasterDetail.Where((genDutyRoasterDetail x) => Conversion.ToInt64((object)x.EmployeeGroupId) == GroupId && x.WeekDayName == WeekProfileName && !x.IsHoliday).ToList().ForEach(delegate(genDutyRoasterDetail x)
					{
						x.IsOnDuty = false;
					});
				}
			}
			((Control)(object)grdDutyDate).Focus();
			grdDutyDate_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdDutyDate_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			DateTime objDutyDate = Conversion.ToDateTime(grdGroup.CurrentRow.Cells["DutyDate"].Value);
			lblDutyDate.Text = objDutyDate.ToString("dd-MMM-yyyy");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDutyDate_Click(object sender, EventArgs e)
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdDutyDate.CurrentRow;
			DateTime objDutyDate = Conversion.ToDateTime(item.Cells["DutyDate"].Value);
			lblDutyDate.Text = objDutyDate.ToString("dd-MMM-yyyy");
			GridEXRow[] rows = grdEmployee.GetRows();
			foreach (GridEXRow r in rows)
			{
				foreach (GridEXColumn item2 in (JanusCollectionBase)grdEmployee.RootTable.Columns)
				{
					GridEXColumn c = item2;
					if (((JanusCollectionElement)c).Index >= 8)
					{
						genDutyRoasterDetail obj = lstDutyRoasterDetail.Find((genDutyRoasterDetail x) => Conversion.ToInt64(r.Cells["EmployeeId"].Value) == x.EmployeeId && x.DutyDate == objDutyDate && x.ShiftShortName == c.Caption);
						if (obj != null)
						{
							r.BeginEdit();
							r.Cells[c.Caption].Value = obj.IsOnDuty;
							r.EndEdit();
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheckAgainstAllEmployee()
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DateTime objDutyDate2 = Conversion.ToDateTime((object)lblDutyDate.Text);
			bool startProcessing = false;
			GridEXRow[] rows = grdDutyDate.GetRows();
			foreach (GridEXRow item in rows)
			{
				DateTime objDutyDate = Conversion.ToDateTime(item.Cells["DutyDate"].Value);
				if (!startProcessing)
				{
					if (!(objDutyDate >= objDutyDate2))
					{
						continue;
					}
					startProcessing = true;
				}
				GridEXRow[] rows2 = grdEmployee.GetRows();
				foreach (GridEXRow r in rows2)
				{
					foreach (GridEXColumn item2 in (JanusCollectionBase)grdEmployee.RootTable.Columns)
					{
						GridEXColumn c = item2;
						if (((JanusCollectionElement)c).Index < 8)
						{
							continue;
						}
						genDutyRoasterDetail obj = lstDutyRoasterDetail.Find((genDutyRoasterDetail x) => Conversion.ToInt64(r.Cells["EmployeeId"].Value) == x.EmployeeId && x.DutyDate == objDutyDate2 && x.ShiftShortName == c.Caption);
						if (obj != null && r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
						{
							r.BeginEdit();
							r.Cells[c.Caption].Value = obj.IsOnDuty;
							r.EndEdit();
							_ = c.Caption;
							bool GetShiftValue = Conversion.ToBool((object)obj.IsOnDuty);
							lstDutyRoasterDetail.Where((genDutyRoasterDetail x) => Conversion.ToInt64((object)x.EmployeeId) == Conversion.ToInt64(r.Cells["EmployeeId"].Value) && x.DutyDate == objDutyDate && x.ShiftShortName == c.Caption).ToList().ForEach(delegate(genDutyRoasterDetail x)
							{
								x.IsOnDuty = GetShiftValue;
							});
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApplyAll_Click(object sender, EventArgs e)
	{
		try
		{
			CheckAgainstAllEmployee();
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
						objDutyRoasterDetail.IsOnDuty = false;
						if (RecId > 0)
						{
							objDutyRoasterDetail.ActionTypeId = 2;
						}
						else
						{
							objDutyRoasterDetail.ActionTypeId = 1;
						}
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

	private void grdEmployee_Click(object sender, EventArgs e)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		try
		{
			grdEmployee.UpdateData();
			GridEXRow item = grdEmployee.CurrentRow;
			int EmployeeId = Conversion.ToInt(item.Cells["EmployeeId"].Value);
			DateTime objDutyDate = Conversion.ToDateTime((object)lblDutyDate.Text);
			if (lblDutyDate.Text == string.Empty)
			{
				throw new Exception("First Select Date");
			}
			foreach (GridEXColumn item2 in (JanusCollectionBase)grdEmployee.RootTable.Columns)
			{
				GridEXColumn c = item2;
				if (((JanusCollectionElement)c).Index >= 8)
				{
					_ = c.Caption;
					bool GetShiftValue = Conversion.ToBool(item.Cells[c.Caption].Value);
					lstDutyRoasterDetail.Where((genDutyRoasterDetail x) => Conversion.ToInt64((object)x.EmployeeId) == EmployeeId && x.DutyDate == objDutyDate && x.ShiftShortName == c.Caption).ToList().ForEach(delegate(genDutyRoasterDetail x)
					{
						x.IsOnDuty = GetShiftValue;
					});
				}
			}
			grdEmployee.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("ShiftTimingName");
				dtHistory.Columns.Add("FromDate", typeof(DateTime));
				dtHistory.Columns.Add("ToDate", typeof(DateTime));
				dtHistory.Columns.Add("Remarks");
				dtHistory.Clear();
				for (int i = 0; i < dtgrd.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dtgrd.Rows[i]["Id"], dtgrd.Rows[i]["TimingDescription"], Conversion.ToDateTime(dtgrd.Rows[i]["DutyRoasterFromDate"]).ToShortDateString(), Conversion.ToDateTime(dtgrd.Rows[i]["DutyRoasterToDate"]).ToShortDateString(), dtgrd.Rows[i]["Remarks"]);
				}
				datagridHistory.DataSource = dtHistory;
				datagridHistory.RetrieveStructure();
				GridHistorySettings();
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

	private void GridHistorySettings()
	{
		try
		{
			datagridHistory.RootTable.Columns["FromDate"].FilterEditType = (FilterEditType)6;
			datagridHistory.RootTable.Columns["ToDate"].FilterEditType = (FilterEditType)6;
			datagridHistory.RootTable.Columns.Add("Edit");
			datagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			datagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			datagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			datagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			datagridHistory.RootTable.Columns["Edit"].Width = 50;
			datagridHistory.RootTable.Columns["Edit"].Position = 0;
			datagridHistory.RootTable.Columns.Add("SaveAs");
			datagridHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			datagridHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			datagridHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			datagridHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			datagridHistory.RootTable.Columns["SaveAs"].Width = 50;
			datagridHistory.RootTable.Columns["SaveAs"].Position = 1;
			datagridHistory.FrozenColumns = 2;
			CommonServices.GridAutoAdjustment(AryColWidth, datagridHistory);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			HistoryGridFill();
		}
	}

	private void datagridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			RecId = Conversion.ToInt64(datagridHistory.CurrentRow.Cells["Id"].Value);
			genDutyRoaster obj = genDutyRoaster.GetByID(RecId);
			if (obj == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			txtFromDate.Value = obj.FromDate;
			txtTodate.Value = obj.ToDate;
			txtDescription.Text = Conversion.ToString((object)obj.Description);
			cmbEmployeeCategory.Value = Conversion.ToInt((object)obj.EmployeeCategoryId);
			cmbDepartment.Value = Conversion.ToInt((object)obj.DepartmentId);
			cmbSection.Value = Conversion.ToInt((object)obj.SectionId);
			CmbLocation.Value = Conversion.ToInt((object)obj.LocationId);
			MannualDataSetInDutyRoaster();
			if (obj.DutyRoasterGroupsList.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < obj.DutyRoasterGroupsList.Count; i++)
			{
				GridEXRow[] rows = grdGroup.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["EmployeeGroupId"].Value = Conversion.ToInt((object)obj.DutyRoasterGroupsList[i].EmployeeGroupId);
					obj2.Cells["ShiftId"].Value = Conversion.ToInt((object)obj.DutyRoasterGroupsList[i].ShiftId);
					obj2.Cells["WeekDayName"].Value = Conversion.ToInt((object)obj.DutyRoasterGroupsList[i].WeekDayId);
					obj2.BeginEdit();
				}
			}
			grdGroup.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datagridHistory_Click(object sender, EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (datagridHistory.SelectedItems.Count <= 0)
			{
				return;
			}
			GridEXRow item = datagridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			DataTable dt = genDutyRoaster.GetHistoryDetailData(Conversion.ToInt(datagridHistory.CurrentRow.Cells["Id"].Value));
			if (dt.Rows.Count > 0)
			{
				DataTable dtHistoryDetail = new DataTable();
				dtHistoryDetail.Columns.Add("ShiftName");
				dtHistoryDetail.Columns.Add("ShiftTiming");
				dtHistoryDetail.Columns.Add("EmployeeType");
				dtHistoryDetail.Columns.Add("Designation");
				dtHistoryDetail.Columns.Add("Department");
				dtHistoryDetail.Columns.Add("EmployeeNo", typeof(int));
				dtHistoryDetail.Columns.Add("EmployeeName");
				dtHistoryDetail.Columns.Add("CNIC");
				dtHistoryDetail.Columns.Add("Mobile");
				dtHistoryDetail.Columns.Add("Location");
				dtHistoryDetail.Columns.Add("WeekDay");
				dtHistoryDetail.Columns.Add("DutyDate", typeof(DateTime));
				dtHistoryDetail.Columns.Add("IsOffDuty");
				dtHistoryDetail.Columns.Add("IsOnDuty");
				dtHistoryDetail.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistoryDetail.Rows.Add(dt.Rows[i]["ShiftName"], dt.Rows[i]["ShiftTiming"], dt.Rows[i]["EmployeeType"], dt.Rows[i]["DesignationName"], dt.Rows[i]["DepartmentName"], dt.Rows[i]["EmployeeNo"], dt.Rows[i]["EmployeeName"], dt.Rows[i]["CNIC"], dt.Rows[i]["Mobile1"], dt.Rows[i]["Locations"], dt.Rows[i]["WeekDayName"], Conversion.ToDateTime(dt.Rows[i]["DutyDate"]).ToShortDateString(), dt.Rows[i]["IsOffDuty"], dt.Rows[i]["IsOnDuty"]);
				}
				datagridhistoryDetail.DataSource = dtHistoryDetail;
				datagridhistoryDetail.RetrieveStructure();
				GridHistorydetailSettings();
			}
			else
			{
				datagridhistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistorydetailSettings()
	{
		try
		{
			CommonServices.GridAutoAdjustment(AryColWidth, datagridhistoryDetail);
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				datagridHistory_DoubleClick(null, null);
			}
			if (e.Column.Key == "SaveAs")
			{
				datagridHistory_DoubleClick(null, null);
				btnupdate.Visible = false;
				btnsave.Visible = false;
				btnSaveAs.Visible = true;
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDutyDate).Name);
			if (fs != null)
			{
				grdDutyDate.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdEmployee).Name);
			if (fs != null)
			{
				grdEmployee.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)datagridHistory).Name);
			if (fs != null)
			{
				datagridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)datagridhistoryDetail).Name);
			if (fs != null)
			{
				datagridhistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnEmployeeCategory_Click(object sender, EventArgs e)
	{
		try
		{
			new frmEmployeeCategory(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDepartment_Click(object sender, EventArgs e)
	{
		try
		{
			new genDepartment(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSection_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineSection(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLocation_Click(object sender, EventArgs e)
	{
		try
		{
			new frmgenLocation(UserAccount).Show();
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
				MessageBox.Show("You don't have the right to view this form...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShifttimingorm_Click(object sender, EventArgs e)
	{
		try
		{
			frmGenShiftTiming obj = new frmGenShiftTiming(UserAccount);
			obj.formright = CommonServices.SetRightsValueInRightsObject("ShiftTiming");
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

	private void frmDutyRoasterNew_KeyDown(object sender, KeyEventArgs e)
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
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDutyRoasterNew_FormClosed(object sender, FormClosedEventArgs e)
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

	private void grdEmployee_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdEmployee.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || e.KeyCode != Keys.Space)
			{
				return;
			}
			grdEmployee.UpdateData();
			int EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
			DateTime objDutyDate = Conversion.ToDateTime((object)lblDutyDate.Text);
			if (lblDutyDate.Text == string.Empty)
			{
				throw new Exception("First Select Date");
			}
			foreach (GridEXColumn item in (JanusCollectionBase)grdEmployee.RootTable.Columns)
			{
				GridEXColumn c = item;
				if (((JanusCollectionElement)c).Index >= 8)
				{
					_ = c.Caption;
					bool GetShiftValue = Conversion.ToBool(r.Cells[c.Caption].Value);
					lstDutyRoasterDetail.Where((genDutyRoasterDetail x) => Conversion.ToInt64((object)x.EmployeeId) == EmployeeId && x.DutyDate == objDutyDate && x.ShiftShortName == c.Caption).ToList().ForEach(delegate(genDutyRoasterDetail x)
					{
						x.IsOnDuty = GetShiftValue;
					});
				}
			}
			grdEmployee.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCheckAll_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbShifts.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Check All the Rows this Shift?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				DateTime objDutyDate = Conversion.ToDateTime((object)lblDutyDate.Text);
				string GetShiftShortName = Conversion.ToString((object)((Control)(object)CmbShifts).Text);
				bool GetShiftValue = true;
				GridEXRow[] rows = grdEmployee.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells[GetShiftShortName].Value = GetShiftValue;
					r.EndEdit();
					int EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
					lstDutyRoasterDetail.Where((genDutyRoasterDetail x) => Conversion.ToInt64((object)x.EmployeeId) == EmployeeId && x.DutyDate == objDutyDate && x.ShiftShortName == GetShiftShortName).ToList().ForEach(delegate(genDutyRoasterDetail x)
					{
						x.IsOnDuty = GetShiftValue;
					});
				}
				return;
			}
			CmbShifts.Focus();
			throw new Exception("Please Select Any Shift first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUncheckedAll_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbShifts.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to un-Check All the Rows this Shift?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				DateTime objDutyDate = Conversion.ToDateTime((object)lblDutyDate.Text);
				string GetShiftShortName = Conversion.ToString((object)((Control)(object)CmbShifts).Text);
				bool GetShiftValue = false;
				GridEXRow[] rows = grdEmployee.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells[GetShiftShortName].Value = GetShiftValue;
					r.EndEdit();
					int EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
					lstDutyRoasterDetail.Where((genDutyRoasterDetail x) => Conversion.ToInt64((object)x.EmployeeId) == EmployeeId && x.DutyDate == objDutyDate && x.ShiftShortName == GetShiftShortName).ToList().ForEach(delegate(genDutyRoasterDetail x)
					{
						x.IsOnDuty = GetShiftValue;
					});
				}
				return;
			}
			CmbShifts.Focus();
			throw new Exception("Please Select Any Shift first...");
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
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Expected O, but got Unknown
		//IL_0cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdd: Expected O, but got Unknown
		//IL_109f: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a9: Expected O, but got Unknown
		//IL_4d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d9e: Expected O, but got Unknown
		GridEXLayout grdDutyDate_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGroup_DesignTimeLayout = new GridEXLayout();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.AttendanceManagement.frmDutyRoasterNew));
		GridEXLayout datagridhistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout datagridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdEmployee = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.lblDutyDate = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.grdDutyDate = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label11 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.btnApplyAll = new System.Windows.Forms.Button();
		this.btnApply = new System.Windows.Forms.Button();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdGroup = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnEmployeeDefine = new System.Windows.Forms.Button();
		this.btnLocation = new System.Windows.Forms.Button();
		this.btnSection = new System.Windows.Forms.Button();
		this.btnDepartment = new System.Windows.Forms.Button();
		this.btnEmployeeCategory = new System.Windows.Forms.Button();
		this.btnLoad = new System.Windows.Forms.Button();
		this.CmbLocation = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbSection = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbDepartment = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbEmployeeName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbEmployeeCategory = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.BtnUncheckedAll = new System.Windows.Forms.Button();
		this.BtnCheckAll = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtTodate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.CmbShifts = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShifttimingorm = new System.Windows.Forms.ToolStripButton();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.datagridhistoryDetail = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.checkBox6 = new System.Windows.Forms.CheckBox();
		this.checkBox7 = new System.Windows.Forms.CheckBox();
		this.checkBox8 = new System.Windows.Forms.CheckBox();
		this.checkBox9 = new System.Windows.Forms.CheckBox();
		this.checkBox10 = new System.Windows.Forms.CheckBox();
		this.datagridHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.checkBox2 = new System.Windows.Forms.CheckBox();
		this.checkBox3 = new System.Windows.Forms.CheckBox();
		this.checkBox4 = new System.Windows.Forms.CheckBox();
		this.checkBox5 = new System.Windows.Forms.CheckBox();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployee).BeginInit();
		this.panel20.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDutyDate).BeginInit();
		this.panel19.SuspendLayout();
		this.panel14.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGroup).BeginInit();
		this.panel11.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEmployeeName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCategory).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShifts).BeginInit();
		this.panel13.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.datagridhistoryDetail).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.datagridHistory).BeginInit();
		this.panel10.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
		this.panel1.TabIndex = 1;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 661);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage3.Controls.Add(this.panel3);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(956, 635);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Form";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(956, 635);
		this.panel3.TabIndex = 1;
		this.panel5.Controls.Add(this.panel4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(956, 635);
		this.panel5.TabIndex = 1;
		this.panel4.Controls.Add(this.panel16);
		this.panel4.Controls.Add(this.panel14);
		this.panel4.Controls.Add(this.panel13);
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(956, 635);
		this.panel4.TabIndex = 0;
		this.panel16.Controls.Add(this.panel18);
		this.panel16.Controls.Add(this.panel17);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(0, 283);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(956, 352);
		this.panel16.TabIndex = 38;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployee);
		this.panel18.Controls.Add(this.panel20);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(264, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(692, 352);
		this.panel18.TabIndex = 1;
		this.grdEmployee.AllowEdit = (InheritableBoolean)2;
		this.grdEmployee.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployee.FilterMode = (FilterMode)2;
		this.grdEmployee.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployee.GroupByBoxVisible = false;
		this.grdEmployee.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdEmployee).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployee).Name = "grdEmployee";
		((ScrollableBase)this.grdEmployee).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployee).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployee).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Size = new System.Drawing.Size(692, 329);
		((System.Windows.Forms.Control)(object)this.grdEmployee).TabIndex = 0;
		this.grdEmployee.TotalRow = (InheritableBoolean)1;
		this.grdEmployee.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployee).VisualStyle = (VisualStyle)4;
		this.grdEmployee.CellUpdated += new ColumnActionEventHandler(grdEmployee_Click);
		((System.Windows.Forms.Control)(object)this.grdEmployee).Click += new System.EventHandler(grdEmployee_Click);
		((System.Windows.Forms.Control)(object)this.grdEmployee).KeyDown += new System.Windows.Forms.KeyEventHandler(grdEmployee_KeyDown);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar2);
		this.panel20.Controls.Add(this.lblDutyDate);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(692, 23);
		this.panel20.TabIndex = 38;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(649, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdEmployee;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 22);
		this.ctrlGrdBar2.TabIndex = 35;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.lblDutyDate.AutoSize = true;
		this.lblDutyDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDutyDate.ForeColor = System.Drawing.Color.White;
		this.lblDutyDate.Location = new System.Drawing.Point(8, 4);
		this.lblDutyDate.Name = "lblDutyDate";
		this.lblDutyDate.Size = new System.Drawing.Size(51, 16);
		this.lblDutyDate.TabIndex = 0;
		this.lblDutyDate.Text = "label5";
		this.panel17.Controls.Add(this.panel21);
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(264, 352);
		this.panel17.TabIndex = 0;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.grdDutyDate);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 23);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(264, 329);
		this.panel21.TabIndex = 39;
		this.grdDutyDate.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDutyDate).BackColor = System.Drawing.Color.White;
		this.grdDutyDate.ColumnAutoResize = true;
		grdDutyDate_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDutyDate.DesignTimeLayout = grdDutyDate_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDutyDate).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDutyDate).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDutyDate.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDutyDate).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDutyDate).Name = "grdDutyDate";
		((ScrollableBase)this.grdDutyDate).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDutyDate).Size = new System.Drawing.Size(264, 329);
		((System.Windows.Forms.Control)(object)this.grdDutyDate).TabIndex = 0;
		this.grdDutyDate.TotalRow = (InheritableBoolean)1;
		this.grdDutyDate.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDutyDate).VisualStyle = (VisualStyle)4;
		this.grdDutyDate.CellUpdated += new ColumnActionEventHandler(grdDutyDate_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grdDutyDate).Click += new System.EventHandler(grdDutyDate_Click);
		this.panel19.BackColor = System.Drawing.Color.Teal;
		this.panel19.Controls.Add(this.ctrlGrdBar1);
		this.panel19.Controls.Add(this.label11);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(264, 23);
		this.panel19.TabIndex = 38;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(220, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDutyDate;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 22);
		this.ctrlGrdBar1.TabIndex = 34;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 9.5f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(4, 3);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(104, 16);
		this.label11.TabIndex = 33;
		this.label11.Text = "Employee Days";
		this.panel14.Controls.Add(this.label13);
		this.panel14.Controls.Add(this.btnApplyAll);
		this.panel14.Controls.Add(this.btnApply);
		this.panel14.Controls.Add(this.groupBox3);
		this.panel14.Controls.Add(this.groupBox1);
		this.panel14.Controls.Add(this.BtnUncheckedAll);
		this.panel14.Controls.Add(this.BtnCheckAll);
		this.panel14.Controls.Add(this.groupBox2);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShifts);
		this.panel14.Controls.Add(this.label15);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 56);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(956, 227);
		this.panel14.TabIndex = 37;
		this.label13.BackColor = System.Drawing.SystemColors.Info;
		this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.Black;
		this.label13.Location = new System.Drawing.Point(2, 191);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(175, 32);
		this.label13.TabIndex = 35;
		this.label13.Text = "Click to apply Shift Select of of Current Date to all Date";
		this.btnApplyAll.BackColor = System.Drawing.Color.Teal;
		this.btnApplyAll.FlatAppearance.BorderSize = 0;
		this.btnApplyAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApplyAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApplyAll.ForeColor = System.Drawing.Color.White;
		this.btnApplyAll.Location = new System.Drawing.Point(180, 191);
		this.btnApplyAll.Name = "btnApplyAll";
		this.btnApplyAll.Size = new System.Drawing.Size(84, 32);
		this.btnApplyAll.TabIndex = 2;
		this.btnApplyAll.Text = "Apply all";
		this.btnApplyAll.UseVisualStyleBackColor = false;
		this.btnApplyAll.Click += new System.EventHandler(btnApplyAll_Click);
		this.btnApply.BackColor = System.Drawing.Color.Teal;
		this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApply.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApply.ForeColor = System.Drawing.Color.White;
		this.btnApply.Location = new System.Drawing.Point(875, 175);
		this.btnApply.Name = "btnApply";
		this.btnApply.Size = new System.Drawing.Size(69, 26);
		this.btnApply.TabIndex = 3;
		this.btnApply.Text = "Apply";
		this.btnApply.UseVisualStyleBackColor = false;
		this.btnApply.Click += new System.EventHandler(btnApply_Click);
		this.groupBox3.Controls.Add(this.panel12);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(531, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(422, 167);
		this.groupBox3.TabIndex = 36;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Apply Group";
		this.panel12.Controls.Add(this.panel15);
		this.panel12.Controls.Add(this.panel11);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(3, 19);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(416, 145);
		this.panel12.TabIndex = 35;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdGroup);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 23);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(416, 122);
		this.panel15.TabIndex = 38;
		((ScrollableBase)this.grdGroup).BackColor = System.Drawing.Color.White;
		this.grdGroup.ColumnAutoResize = true;
		grdGroup_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGroup.DesignTimeLayout = grdGroup_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGroup).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGroup).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGroup.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGroup).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGroup).Name = "grdGroup";
		((ScrollableBase)this.grdGroup).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGroup).Size = new System.Drawing.Size(416, 122);
		((System.Windows.Forms.Control)(object)this.grdGroup).TabIndex = 33;
		this.grdGroup.TotalRow = (InheritableBoolean)1;
		this.grdGroup.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGroup).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label10);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(416, 23);
		this.panel11.TabIndex = 37;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(3, 3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(403, 15);
		this.label10.TabIndex = 33;
		this.label10.Text = "Users can change the shift && rest day of an employee in the selected group.";
		this.groupBox1.Controls.Add(this.btnEmployeeDefine);
		this.groupBox1.Controls.Add(this.btnLocation);
		this.groupBox1.Controls.Add(this.btnSection);
		this.groupBox1.Controls.Add(this.btnDepartment);
		this.groupBox1.Controls.Add(this.btnEmployeeCategory);
		this.groupBox1.Controls.Add(this.btnLoad);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLocation);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSection);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEmployeeName);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(8, 74);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(517, 112);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Employees Filters For duty Roaster";
		this.btnEmployeeDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeDefine.Location = new System.Drawing.Point(483, 52);
		this.btnEmployeeDefine.Name = "btnEmployeeDefine";
		this.btnEmployeeDefine.Size = new System.Drawing.Size(28, 26);
		this.btnEmployeeDefine.TabIndex = 9;
		this.btnEmployeeDefine.TabStop = false;
		this.btnEmployeeDefine.UseVisualStyleBackColor = true;
		this.btnEmployeeDefine.Click += new System.EventHandler(btnEmployeeDefine_Click);
		this.btnLocation.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnLocation.Location = new System.Drawing.Point(483, 22);
		this.btnLocation.Name = "btnLocation";
		this.btnLocation.Size = new System.Drawing.Size(28, 26);
		this.btnLocation.TabIndex = 7;
		this.btnLocation.TabStop = false;
		this.btnLocation.UseVisualStyleBackColor = true;
		this.btnLocation.Click += new System.EventHandler(btnLocation_Click);
		this.btnSection.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnSection.Location = new System.Drawing.Point(226, 82);
		this.btnSection.Name = "btnSection";
		this.btnSection.Size = new System.Drawing.Size(28, 26);
		this.btnSection.TabIndex = 5;
		this.btnSection.TabStop = false;
		this.btnSection.UseVisualStyleBackColor = true;
		this.btnSection.Click += new System.EventHandler(btnSection_Click);
		this.btnDepartment.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDepartment.Location = new System.Drawing.Point(226, 52);
		this.btnDepartment.Name = "btnDepartment";
		this.btnDepartment.Size = new System.Drawing.Size(28, 26);
		this.btnDepartment.TabIndex = 3;
		this.btnDepartment.TabStop = false;
		this.btnDepartment.UseVisualStyleBackColor = true;
		this.btnDepartment.Click += new System.EventHandler(btnDepartment_Click);
		this.btnEmployeeCategory.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeCategory.Location = new System.Drawing.Point(226, 22);
		this.btnEmployeeCategory.Name = "btnEmployeeCategory";
		this.btnEmployeeCategory.Size = new System.Drawing.Size(28, 26);
		this.btnEmployeeCategory.TabIndex = 1;
		this.btnEmployeeCategory.TabStop = false;
		this.btnEmployeeCategory.UseVisualStyleBackColor = true;
		this.btnEmployeeCategory.Click += new System.EventHandler(btnEmployeeCategory_Click);
		this.btnLoad.BackColor = System.Drawing.Color.Teal;
		this.btnLoad.FlatAppearance.BorderSize = 0;
		this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoad.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoad.ForeColor = System.Drawing.Color.White;
		this.btnLoad.Location = new System.Drawing.Point(453, 82);
		this.btnLoad.Name = "btnLoad";
		this.btnLoad.Size = new System.Drawing.Size(58, 26);
		this.btnLoad.TabIndex = 10;
		this.btnLoad.Text = "Load";
		this.btnLoad.UseVisualStyleBackColor = false;
		this.btnLoad.Click += new System.EventHandler(btnLoad_Click);
		this.CmbLocation.AllowNull = (DefaultableBoolean)1;
		this.CmbLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance62;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLocation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLocation).Location = new System.Drawing.Point(326, 22);
		((System.Windows.Forms.Control)(object)this.CmbLocation).Name = "CmbLocation";
		((System.Windows.Forms.Control)(object)this.CmbLocation).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.CmbLocation).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbLocation).Leave += new System.EventHandler(CmbLocation_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(259, 28);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(53, 15);
		this.label2.TabIndex = 21;
		this.label2.Text = "Location";
		this.cmbSection.AllowNull = (DefaultableBoolean)1;
		this.cmbSection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbSection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSection).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Location = new System.Drawing.Point(76, 82);
		((System.Windows.Forms.Control)(object)this.cmbSection).Name = "cmbSection";
		((System.Windows.Forms.Control)(object)this.cmbSection).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.cmbSection).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbSection).Leave += new System.EventHandler(cmbSection_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 88);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(47, 15);
		this.label9.TabIndex = 21;
		this.label9.Text = "Section";
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(76, 52);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Leave += new System.EventHandler(cmbDepartment_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 58);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(71, 15);
		this.label8.TabIndex = 19;
		this.label8.Text = "Department";
		this.CmbEmployeeName.AllowNull = (DefaultableBoolean)1;
		this.CmbEmployeeName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEmployeeName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEmployeeName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmployeeName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmployeeName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbEmployeeName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).Location = new System.Drawing.Point(326, 52);
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).Name = "CmbEmployeeName";
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).TabIndex = 8;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(259, 58);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(66, 15);
		this.label1.TabIndex = 17;
		this.label1.Text = "Emp Name";
		this.cmbEmployeeCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Location = new System.Drawing.Point(76, 22);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Name = "cmbEmployeeCategory";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Leave += new System.EventHandler(cmbEmployeeCategory_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 28);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(54, 15);
		this.label3.TabIndex = 17;
		this.label3.Text = "Category";
		this.BtnUncheckedAll.BackColor = System.Drawing.Color.Teal;
		this.BtnUncheckedAll.FlatAppearance.BorderSize = 0;
		this.BtnUncheckedAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUncheckedAll.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUncheckedAll.ForeColor = System.Drawing.Color.White;
		this.BtnUncheckedAll.Location = new System.Drawing.Point(750, 194);
		this.BtnUncheckedAll.Name = "BtnUncheckedAll";
		this.BtnUncheckedAll.Size = new System.Drawing.Size(108, 26);
		this.BtnUncheckedAll.TabIndex = 10;
		this.BtnUncheckedAll.Text = "Un-Check All";
		this.BtnUncheckedAll.UseVisualStyleBackColor = false;
		this.BtnUncheckedAll.Click += new System.EventHandler(BtnUncheckedAll_Click);
		this.BtnCheckAll.BackColor = System.Drawing.Color.Teal;
		this.BtnCheckAll.FlatAppearance.BorderSize = 0;
		this.BtnCheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCheckAll.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnCheckAll.ForeColor = System.Drawing.Color.White;
		this.BtnCheckAll.Location = new System.Drawing.Point(668, 194);
		this.BtnCheckAll.Name = "BtnCheckAll";
		this.BtnCheckAll.Size = new System.Drawing.Size(79, 26);
		this.BtnCheckAll.TabIndex = 10;
		this.BtnCheckAll.Text = "Check All";
		this.BtnCheckAll.UseVisualStyleBackColor = false;
		this.BtnCheckAll.Click += new System.EventHandler(BtnCheckAll_Click);
		this.groupBox2.Controls.Add(this.txtTodate);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.txtFromDate);
		this.groupBox2.Controls.Add(this.txtDescription);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(8, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(517, 70);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.txtTodate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTodate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtTodate.Location = new System.Drawing.Point(72, 42);
		this.txtTodate.Name = "txtTodate";
		this.txtTodate.Size = new System.Drawing.Size(110, 23);
		this.txtTodate.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 20);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(63, 15);
		this.label7.TabIndex = 29;
		this.label7.Text = "From Date";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 44);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(48, 15);
		this.label6.TabIndex = 28;
		this.label6.Text = "To Date";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(188, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(68, 15);
		this.label12.TabIndex = 32;
		this.label12.Text = "Description";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDate.Location = new System.Drawing.Point(72, 16);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(110, 23);
		this.txtFromDate.TabIndex = 0;
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(256, 16);
		this.txtDescription.Multiline = true;
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(257, 49);
		this.txtDescription.TabIndex = 2;
		this.CmbShifts.AllowNull = (DefaultableBoolean)1;
		this.CmbShifts.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShifts.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShifts.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbShifts).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShifts).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShifts).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShifts).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbShifts).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShifts).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbShifts).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShifts).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShifts).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbShifts).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShifts).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShifts).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShifts).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShifts.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShifts).Location = new System.Drawing.Point(509, 194);
		((System.Windows.Forms.Control)(object)this.CmbShifts).Name = "CmbShifts";
		((System.Windows.Forms.Control)(object)this.CmbShifts).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.CmbShifts).TabIndex = 8;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(466, 200);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(37, 15);
		this.label15.TabIndex = 17;
		this.label15.Text = "Shifts";
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label5);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 27);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(956, 29);
		this.panel13.TabIndex = 36;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(4, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(116, 19);
		this.label5.TabIndex = 33;
		this.label5.Text = "Duty Roaster";
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnSaveAs, this.btnsave, this.btnupdate, this.btnPrint, this.btnRefresh, this.toolStripSeparator1, this.btnShifttimingorm });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(956, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = (System.Drawing.Image)resources.GetObject("btnSaveAs.Image");
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "&SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
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
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnShifttimingorm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShifttimingorm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShifttimingorm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShifttimingorm.Name = "btnShifttimingorm";
		this.btnShifttimingorm.Size = new System.Drawing.Size(89, 24);
		this.btnShifttimingorm.Text = "Shift Timing";
		this.btnShifttimingorm.Click += new System.EventHandler(btnShifttimingorm_Click);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Size = new System.Drawing.Size(956, 635);
		this.tabPage1.TabIndex = 1;
		this.tabPage1.Text = "History";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.datagridhistoryDetail);
		this.panel8.Controls.Add(this.panel2);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.datagridHistory);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(956, 635);
		this.panel8.TabIndex = 0;
		this.datagridhistoryDetail.AllowEdit = (InheritableBoolean)2;
		datagridhistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.datagridhistoryDetail.DesignTimeLayout = datagridhistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.datagridhistoryDetail.DynamicFiltering = true;
		this.datagridhistoryDetail.FilterMode = (FilterMode)2;
		this.datagridhistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.datagridhistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Location = new System.Drawing.Point(0, 322);
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Name = "datagridhistoryDetail";
		((ScrollableBase)this.datagridhistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Size = new System.Drawing.Size(956, 313);
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).TabIndex = 48;
		this.datagridhistoryDetail.TotalRow = (InheritableBoolean)1;
		this.datagridhistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagridhistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar4);
		this.panel2.Controls.Add(this.label14);
		this.panel2.Controls.Add(this.checkBox6);
		this.panel2.Controls.Add(this.checkBox7);
		this.panel2.Controls.Add(this.checkBox8);
		this.panel2.Controls.Add(this.checkBox9);
		this.panel2.Controls.Add(this.checkBox10);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 293);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(956, 29);
		this.panel2.TabIndex = 47;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(901, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.datagridhistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(53, 27);
		this.ctrlGrdBar4.TabIndex = 36;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.5f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(6, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(351, 16);
		this.label14.TabIndex = 5;
		this.label14.Text = "This Grid Show The Detail Of seleced Row of Main grid";
		this.checkBox6.AutoSize = true;
		this.checkBox6.Location = new System.Drawing.Point(115, 74);
		this.checkBox6.Name = "checkBox6";
		this.checkBox6.Size = new System.Drawing.Size(171, 20);
		this.checkBox6.TabIndex = 1;
		this.checkBox6.Text = "All Security Morning Shift";
		this.checkBox6.UseVisualStyleBackColor = true;
		this.checkBox6.Visible = false;
		this.checkBox7.AutoSize = true;
		this.checkBox7.Location = new System.Drawing.Point(16, 76);
		this.checkBox7.Name = "checkBox7";
		this.checkBox7.Size = new System.Drawing.Size(110, 20);
		this.checkBox7.TabIndex = 0;
		this.checkBox7.Text = "All Admin Shift";
		this.checkBox7.UseVisualStyleBackColor = true;
		this.checkBox7.Visible = false;
		this.checkBox8.AutoSize = true;
		this.checkBox8.Location = new System.Drawing.Point(264, 73);
		this.checkBox8.Name = "checkBox8";
		this.checkBox8.Size = new System.Drawing.Size(171, 20);
		this.checkBox8.TabIndex = 2;
		this.checkBox8.Text = "All Non Managment Shift";
		this.checkBox8.UseVisualStyleBackColor = true;
		this.checkBox8.Visible = false;
		this.checkBox9.AutoSize = true;
		this.checkBox9.Location = new System.Drawing.Point(264, 96);
		this.checkBox9.Name = "checkBox9";
		this.checkBox9.Size = new System.Drawing.Size(154, 20);
		this.checkBox9.TabIndex = 4;
		this.checkBox9.Text = "All Security Night Shift";
		this.checkBox9.UseVisualStyleBackColor = true;
		this.checkBox9.Visible = false;
		this.checkBox10.AutoSize = true;
		this.checkBox10.Location = new System.Drawing.Point(409, 73);
		this.checkBox10.Name = "checkBox10";
		this.checkBox10.Size = new System.Drawing.Size(143, 20);
		this.checkBox10.TabIndex = 3;
		this.checkBox10.Text = "All Managment Shift";
		this.checkBox10.UseVisualStyleBackColor = true;
		this.checkBox10.Visible = false;
		this.datagridHistory.AllowEdit = (InheritableBoolean)2;
		datagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.datagridHistory.DesignTimeLayout = datagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.datagridHistory.DynamicFiltering = true;
		this.datagridHistory.FilterMode = (FilterMode)2;
		this.datagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.datagridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.datagridHistory).Name = "datagridHistory";
		((ScrollableBase)this.datagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Size = new System.Drawing.Size(956, 264);
		((System.Windows.Forms.Control)(object)this.datagridHistory).TabIndex = 45;
		this.datagridHistory.TotalRow = (InheritableBoolean)1;
		this.datagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagridHistory).VisualStyle = (VisualStyle)4;
		this.datagridHistory.ColumnButtonClick += new ColumnActionEventHandler(datagridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.datagridHistory).Click += new System.EventHandler(datagridHistory_Click);
		((System.Windows.Forms.Control)(object)this.datagridHistory).DoubleClick += new System.EventHandler(datagridHistory_DoubleClick);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar3);
		this.panel10.Controls.Add(this.label4);
		this.panel10.Controls.Add(this.checkBox1);
		this.panel10.Controls.Add(this.checkBox2);
		this.panel10.Controls.Add(this.checkBox3);
		this.panel10.Controls.Add(this.checkBox4);
		this.panel10.Controls.Add(this.checkBox5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(956, 29);
		this.panel10.TabIndex = 12;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(901, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.datagridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(53, 27);
		this.ctrlGrdBar3.TabIndex = 36;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(6, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(179, 19);
		this.label4.TabIndex = 5;
		this.label4.Text = "Duty Roaster History";
		this.checkBox1.AutoSize = true;
		this.checkBox1.Location = new System.Drawing.Point(115, 74);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(171, 20);
		this.checkBox1.TabIndex = 1;
		this.checkBox1.Text = "All Security Morning Shift";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.checkBox1.Visible = false;
		this.checkBox2.AutoSize = true;
		this.checkBox2.Location = new System.Drawing.Point(16, 76);
		this.checkBox2.Name = "checkBox2";
		this.checkBox2.Size = new System.Drawing.Size(110, 20);
		this.checkBox2.TabIndex = 0;
		this.checkBox2.Text = "All Admin Shift";
		this.checkBox2.UseVisualStyleBackColor = true;
		this.checkBox2.Visible = false;
		this.checkBox3.AutoSize = true;
		this.checkBox3.Location = new System.Drawing.Point(264, 73);
		this.checkBox3.Name = "checkBox3";
		this.checkBox3.Size = new System.Drawing.Size(171, 20);
		this.checkBox3.TabIndex = 2;
		this.checkBox3.Text = "All Non Managment Shift";
		this.checkBox3.UseVisualStyleBackColor = true;
		this.checkBox3.Visible = false;
		this.checkBox4.AutoSize = true;
		this.checkBox4.Location = new System.Drawing.Point(264, 96);
		this.checkBox4.Name = "checkBox4";
		this.checkBox4.Size = new System.Drawing.Size(154, 20);
		this.checkBox4.TabIndex = 4;
		this.checkBox4.Text = "All Security Night Shift";
		this.checkBox4.UseVisualStyleBackColor = true;
		this.checkBox4.Visible = false;
		this.checkBox5.AutoSize = true;
		this.checkBox5.Location = new System.Drawing.Point(409, 73);
		this.checkBox5.Name = "checkBox5";
		this.checkBox5.Size = new System.Drawing.Size(143, 20);
		this.checkBox5.TabIndex = 3;
		this.checkBox5.Text = "All Managment Shift";
		this.checkBox5.UseVisualStyleBackColor = true;
		this.checkBox5.Visible = false;
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(0, 0);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(788, 27);
		this.miniToolStrip.TabIndex = 7;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmDutyRoasterNew";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmDutyRoasterNew_FormClosed);
		base.Load += new System.EventHandler(frmDutyRoaster_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDutyRoasterNew_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployee).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDutyDate).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGroup).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEmployeeName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCategory).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShifts).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.datagridhistoryDetail).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.datagridHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		base.ResumeLayout(false);
	}
}
