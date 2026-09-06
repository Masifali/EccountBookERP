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
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.HRM.AttendanceManagement
{

public class frmEmployeeRoaster : Form
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

	private ToolStripButton btnupdate;

	private ToolStripButton btnRefresh;

	private TextBox txtDescription;

	private Button btnLoad;

	private Label label12;

	private DateTimePicker txtFromDate;

	private Label label7;

	private Label label6;

	private DateTimePicker txtTodate;

	private ToolStrip miniToolStrip;

	internal GridEX grdEmployee;

	private Label lblDutyDate;

	private Panel panel11;

	internal GridEX grdDutyDate;

	private Button btnApply;

	internal UltraCombo CmbEmployeeName;

	private Label label1;

	private ToolStripButton btnsave;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private Button btnEmployeeDefine;

	private GroupBox groupBox3;

	private Panel panel13;

	private Panel panel15;

	internal GridEX grdGroup;

	private Panel panel14;

	private Label label10;

	private Panel panel12;

	private Panel panel16;

	private Panel panel17;

	private Panel panel19;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label11;

	private Panel panel18;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label2;

	internal GridEX datagridhistoryDetail;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar4;

	private Label label3;

	private CheckBox checkBox6;

	private CheckBox checkBox7;

	private CheckBox checkBox8;

	private CheckBox checkBox9;

	private CheckBox checkBox10;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel6;

	private Label label5;

	private Label label13;

	private Button btnApplyAll;

	private ToolStripButton btnSaveAs;

	public frmEmployeeRoaster(UserAccount userAccount)
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
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(980, 500);
			tabPage3.AutoScroll = true;
			tabPage3.AutoScrollMargin = new Size(20, 20);
			tabPage3.AutoScrollMinSize = new Size(980, 500);
			formright = CommonServices.SetRightsValueInRightsObject("EmployeeRoaster");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			txtFromDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			dtShift.Columns.Add("ShiftId");
			dtShift.Columns.Add("ShiftName");
			ShiftFill();
			WeekDaysFill();
			EmployeeName();
			lblDutyDate.Text = string.Empty;
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
			ShiftFill();
			EmployeeName();
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
			txtDescription.Text = string.Empty;
			txtFromDate.Focus();
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
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtemployee = genEmployeeHistory.GetAllActiveEmployee(new genEmployeeHistory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
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

	private void Insert()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
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
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.GenDutyRoasterDetailsList = new List<genDutyRoasterDetail>();
				obj.DutyRoasterGroupsList = new List<DutyRoasterGroups>();
				obj.GenDutyRoasterDetailsList = lstDutyRoasterDetail;
				MakeOffDaysRecords(obj);
				DutyRoasterDetailIdsUpdateForUpdate(obj);
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
				Reset();
				return;
			}
			MannualDataSetInDutyRoaster();
			throw new Exception("RecId Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
	}

	private void btnLoad_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (((UltraGridBase)CmbEmployeeName).ActiveRow == null)
				{
					MessageBox.Show("Please Select Employee...");
					CmbEmployeeName.Focus();
				}
				else
				{
					btnupdate.Visible = true;
					MannualDataSetInDutyRoaster();
				}
				return;
			}
			throw new Exception("RecId Not Found Please GetMaster Record");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MannualDataSetInDutyRoaster()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = txtFromDate.Value,
				ToDate = txtTodate.Value,
				EmployeeId = Conversion.ToInt(CmbEmployeeName.Value),
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
				for (int i = 0; i < dtAll.Rows.Count; i++)
				{
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
				GridhistorySettings();
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

	private void GridhistorySettings()
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
			datagridHistory.FrozenColumns = 2;
			CommonServices.GridAutoAdjustment(AryColWidth, datagridHistory);
			ctrlGrdBar3_Load(null, null);
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
			DataTable dt = genDutyRoaster.GetHistoryDetailData(Conversion.ToInt(datagridHistory.CurrentRow.Cells[0].Value));
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

	private void datagridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
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
			if (obj.DutyRoasterGroupsList.Count > 0)
			{
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
			DataTable dtEmployeeFill = genDutyRoaster.GetEmployeesByDutyRosterId(Conversion.ToInt((object)RecId));
			if (dtEmployeeFill.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtEmployeeFill, CmbEmployeeName, "EmployeeId", "EmployeeName", "Employee Name", false);
			}
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

	private void frmEmployeeRoaster_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Expected O, but got Unknown
		//IL_0de2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dec: Expected O, but got Unknown
		//IL_2d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d6c: Expected O, but got Unknown
		GridEXLayout grdDutyDate_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGroup_DesignTimeLayout = new GridEXLayout();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.AttendanceManagement.frmEmployeeRoaster));
		GridEXLayout datagridhistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout datagridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdEmployee = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.lblDutyDate = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdDutyDate = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label11 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.btnApplyAll = new System.Windows.Forms.Button();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.btnApply = new System.Windows.Forms.Button();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdGroup = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnEmployeeDefine = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbEmployeeName = new UltraCombo();
		this.btnLoad = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.txtTodate = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.datagridhistoryDetail = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
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
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployee).BeginInit();
		this.panel18.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDutyDate).BeginInit();
		this.panel19.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGroup).BeginInit();
		this.panel14.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEmployeeName).BeginInit();
		this.groupBox1.SuspendLayout();
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
		this.panel1.Size = new System.Drawing.Size(1052, 597);
		this.panel1.TabIndex = 1;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1052, 597);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage3.Controls.Add(this.panel3);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1044, 571);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Form";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1044, 571);
		this.panel3.TabIndex = 1;
		this.panel5.Controls.Add(this.panel4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1044, 571);
		this.panel5.TabIndex = 1;
		this.panel4.Controls.Add(this.panel12);
		this.panel4.Controls.Add(this.panel11);
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1044, 571);
		this.panel4.TabIndex = 0;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployee);
		this.panel12.Controls.Add(this.panel18);
		this.panel12.Controls.Add(this.panel16);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 260);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1044, 311);
		this.panel12.TabIndex = 35;
		this.grdEmployee.AllowEdit = (InheritableBoolean)2;
		this.grdEmployee.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployee.FilterMode = (FilterMode)2;
		this.grdEmployee.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployee.GroupByBoxVisible = false;
		this.grdEmployee.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Location = new System.Drawing.Point(264, 23);
		((System.Windows.Forms.Control)(object)this.grdEmployee).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployee).Name = "grdEmployee";
		((ScrollableBase)this.grdEmployee).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployee).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployee).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Size = new System.Drawing.Size(780, 288);
		((System.Windows.Forms.Control)(object)this.grdEmployee).TabIndex = 0;
		this.grdEmployee.TotalRow = (InheritableBoolean)1;
		this.grdEmployee.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployee).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdEmployee).Click += new System.EventHandler(grdEmployee_Click);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.lblDutyDate);
		this.panel18.Controls.Add(this.ctrlGrdBar2);
		this.panel18.Controls.Add(this.label2);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(264, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(780, 23);
		this.panel18.TabIndex = 40;
		this.lblDutyDate.AutoSize = true;
		this.lblDutyDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDutyDate.ForeColor = System.Drawing.Color.White;
		this.lblDutyDate.Location = new System.Drawing.Point(238, 4);
		this.lblDutyDate.Name = "lblDutyDate";
		this.lblDutyDate.Size = new System.Drawing.Size(51, 16);
		this.lblDutyDate.TabIndex = 0;
		this.lblDutyDate.Text = "label5";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(736, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdEmployee;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 22);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.5f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(4, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(124, 16);
		this.label2.TabIndex = 33;
		this.label2.Text = "Employee Roaster";
		this.panel16.Controls.Add(this.panel17);
		this.panel16.Controls.Add(this.panel19);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(264, 311);
		this.panel16.TabIndex = 0;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdDutyDate);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 23);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(264, 288);
		this.panel17.TabIndex = 40;
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
		((System.Windows.Forms.Control)(object)this.grdDutyDate).Size = new System.Drawing.Size(264, 288);
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
		this.panel19.TabIndex = 39;
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
		this.panel11.Controls.Add(this.label13);
		this.panel11.Controls.Add(this.btnApplyAll);
		this.panel11.Controls.Add(this.panel6);
		this.panel11.Controls.Add(this.btnApply);
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Controls.Add(this.groupBox2);
		this.panel11.Controls.Add(this.groupBox1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 27);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1044, 233);
		this.panel11.TabIndex = 34;
		this.label13.BackColor = System.Drawing.SystemColors.Info;
		this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.Black;
		this.label13.Location = new System.Drawing.Point(8, 204);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(298, 24);
		this.label13.TabIndex = 42;
		this.label13.Text = "Click to apply Shift Select of of Current Date to all Date";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnApplyAll.BackColor = System.Drawing.Color.Teal;
		this.btnApplyAll.FlatAppearance.BorderSize = 0;
		this.btnApplyAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApplyAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApplyAll.ForeColor = System.Drawing.Color.White;
		this.btnApplyAll.Location = new System.Drawing.Point(312, 204);
		this.btnApplyAll.Name = "btnApplyAll";
		this.btnApplyAll.Size = new System.Drawing.Size(90, 24);
		this.btnApplyAll.TabIndex = 41;
		this.btnApplyAll.Text = "Apply all";
		this.btnApplyAll.UseVisualStyleBackColor = false;
		this.btnApplyAll.Click += new System.EventHandler(btnApplyAll_Click);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1044, 29);
		this.panel6.TabIndex = 40;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(4, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(157, 19);
		this.label5.TabIndex = 33;
		this.label5.Text = "Employee Roaster";
		this.btnApply.BackColor = System.Drawing.Color.Teal;
		this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApply.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApply.ForeColor = System.Drawing.Color.White;
		this.btnApply.Location = new System.Drawing.Point(972, 203);
		this.btnApply.Name = "btnApply";
		this.btnApply.Size = new System.Drawing.Size(64, 27);
		this.btnApply.TabIndex = 2;
		this.btnApply.Text = "Apply";
		this.btnApply.UseVisualStyleBackColor = false;
		this.btnApply.Click += new System.EventHandler(btnApply_Click);
		this.groupBox3.Controls.Add(this.panel13);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(616, 32);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(422, 167);
		this.groupBox3.TabIndex = 37;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Apply Group";
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 19);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(416, 145);
		this.panel13.TabIndex = 35;
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
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label10);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(416, 23);
		this.panel14.TabIndex = 37;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(3, 3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(403, 15);
		this.label10.TabIndex = 33;
		this.label10.Text = "Users can change the shift && rest day of an employee in the selected group.";
		this.groupBox2.Controls.Add(this.btnEmployeeDefine);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEmployeeName);
		this.groupBox2.Controls.Add(this.btnLoad);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(8, 132);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(604, 66);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Employee Filter";
		this.btnEmployeeDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeDefine.Location = new System.Drawing.Point(444, 22);
		this.btnEmployeeDefine.Name = "btnEmployeeDefine";
		this.btnEmployeeDefine.Size = new System.Drawing.Size(28, 26);
		this.btnEmployeeDefine.TabIndex = 2;
		this.btnEmployeeDefine.TabStop = false;
		this.btnEmployeeDefine.UseVisualStyleBackColor = true;
		this.btnEmployeeDefine.Click += new System.EventHandler(btnEmployeeDefine_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 28);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(94, 15);
		this.label1.TabIndex = 17;
		this.label1.Text = "Employee Name";
		this.CmbEmployeeName.AllowNull = (DefaultableBoolean)1;
		this.CmbEmployeeName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEmployeeName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEmployeeName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmployeeName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmployeeName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEmployeeName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbEmployeeName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).Location = new System.Drawing.Point(105, 22);
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).Name = "CmbEmployeeName";
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).Size = new System.Drawing.Size(333, 26);
		((System.Windows.Forms.Control)(object)this.CmbEmployeeName).TabIndex = 0;
		this.btnLoad.BackColor = System.Drawing.Color.Teal;
		this.btnLoad.FlatAppearance.BorderSize = 0;
		this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoad.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoad.ForeColor = System.Drawing.Color.White;
		this.btnLoad.Location = new System.Drawing.Point(478, 22);
		this.btnLoad.Name = "btnLoad";
		this.btnLoad.Size = new System.Drawing.Size(69, 26);
		this.btnLoad.TabIndex = 3;
		this.btnLoad.Text = "Load";
		this.btnLoad.UseVisualStyleBackColor = false;
		this.btnLoad.Click += new System.EventHandler(btnLoad_Click);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtFromDate);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtTodate);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtDescription);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(8, 32);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(604, 99);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 27);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(63, 15);
		this.label7.TabIndex = 29;
		this.label7.Text = "From Date";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDate.Location = new System.Drawing.Point(73, 23);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(118, 23);
		this.txtFromDate.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 54);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(48, 15);
		this.label6.TabIndex = 28;
		this.label6.Text = "To Date";
		this.txtTodate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTodate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtTodate.Location = new System.Drawing.Point(73, 50);
		this.txtTodate.Name = "txtTodate";
		this.txtTodate.Size = new System.Drawing.Size(118, 23);
		this.txtTodate.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(195, 27);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(68, 15);
		this.label12.TabIndex = 32;
		this.label12.Text = "Description";
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(263, 24);
		this.txtDescription.Multiline = true;
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(330, 49);
		this.txtDescription.TabIndex = 2;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnSaveAs, this.btnsave, this.btnupdate, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1044, 27);
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
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
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
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Size = new System.Drawing.Size(1044, 571);
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
		this.panel8.Size = new System.Drawing.Size(1044, 571);
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
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Location = new System.Drawing.Point(0, 272);
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Name = "datagridhistoryDetail";
		((ScrollableBase)this.datagridhistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).Size = new System.Drawing.Size(1044, 299);
		((System.Windows.Forms.Control)(object)this.datagridhistoryDetail).TabIndex = 48;
		this.datagridhistoryDetail.TotalRow = (InheritableBoolean)1;
		this.datagridhistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagridhistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar4);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.checkBox6);
		this.panel2.Controls.Add(this.checkBox7);
		this.panel2.Controls.Add(this.checkBox8);
		this.panel2.Controls.Add(this.checkBox9);
		this.panel2.Controls.Add(this.checkBox10);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 244);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1044, 28);
		this.panel2.TabIndex = 47;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(992, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar4.MyGrid = this.datagridhistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(44, 24);
		this.ctrlGrdBar4.TabIndex = 37;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(10, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(263, 19);
		this.label3.TabIndex = 5;
		this.label3.Text = "Employee Duty Roaster History";
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
		((System.Windows.Forms.Control)(object)this.datagridHistory).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.datagridHistory).Name = "datagridHistory";
		((ScrollableBase)this.datagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagridHistory).Size = new System.Drawing.Size(1044, 216);
		((System.Windows.Forms.Control)(object)this.datagridHistory).TabIndex = 45;
		this.datagridHistory.TotalRow = (InheritableBoolean)1;
		this.datagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagridHistory).VisualStyle = (VisualStyle)4;
		this.datagridHistory.ColumnButtonClick += new ColumnActionEventHandler(datagridHistory_ColumnButtonClick);
		this.datagridHistory.SelectionChanged += new System.EventHandler(datagridHistory_Click);
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
		this.panel10.Size = new System.Drawing.Size(1044, 28);
		this.panel10.TabIndex = 12;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(992, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar3.MyGrid = this.datagridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(44, 24);
		this.ctrlGrdBar3.TabIndex = 37;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(10, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(263, 19);
		this.label4.TabIndex = 5;
		this.label4.Text = "Employee Duty Roaster History";
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
		base.ClientSize = new System.Drawing.Size(1052, 597);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmEmployeeRoaster";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEmployeeRoaster_FormClosing);
		base.Load += new System.EventHandler(frmDutyRoaster_Load);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployee).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDutyDate).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGroup).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEmployeeName).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
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

}
