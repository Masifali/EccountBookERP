using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PayrollManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PayrollManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.EmployeeManagement;
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
namespace Architecture.WinApp.HRM.PayRoll
{

public class EmployeeShortAdjustment : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private int RecId;

	private bool UpdateMode;

	private long Success;

	private int[] AryColWidth;

	private int LocationId;

	private DataTable dtemployee = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	internal UltraCombo cmbEmployeeName;

	private Label label1;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private GroupBox groupBox1;

	private TextBox txtSection;

	private Label label7;

	private TextBox txtDepartment;

	private Label label5;

	private TextBox txtDesgination;

	private Label label4;

	private TextBox txtEmployeeNo;

	private Label label3;

	private TextBox txtSalary;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label2;

	private TextBox txtDeductedShortHours;

	private TextBox txtDeductionRate;

	private Label label13;

	private CheckBox IsDeduction;

	private CheckBox IsLeaveAdjust;

	private SqlCeDataAdapter sqlCeDataAdapter1;

	private TextBox txtTotalMinutes;

	private TextBox txtTotalHours;

	private Label label6;

	private Label label11;

	internal UltraCombo cmbMonth;

	internal UltraCombo cmbYear;

	private Label label10;

	private Label label9;

	private Button btnEmployee;

	internal GridEX grd;

	private Panel panel5;

	private Label label14;

	private Panel panel6;

	private Label label12;

	private ToolStripButton btnRefresh;

	public EmployeeShortAdjustment(UserAccount pUserAccount)
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
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(960, 500);
			formright = CommonServices.SetRightsValueInRightsObject("EmployeeShortAdjustment");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			EmployeeName();
			YearFill();
			MonthFill();
			GridBind();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			cmbEmployeeName.Focus();
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "January");
			dt.Rows.Add(2, "February");
			dt.Rows.Add(3, "March");
			dt.Rows.Add(4, "April");
			dt.Rows.Add(5, "May");
			dt.Rows.Add(6, "June");
			dt.Rows.Add(7, "July");
			dt.Rows.Add(8, "August");
			dt.Rows.Add(9, "September");
			dt.Rows.Add(10, "October");
			dt.Rows.Add(11, "November");
			dt.Rows.Add(12, "December");
			DDL.BindDDL(dt, cmbMonth, "Id", "Name", "Month", ZeroIndex: false);
			((UltraGridBase)cmbMonth).DisplayLayout.Bands[0].Columns[1].Width = 140;
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Year");
			for (int i = 1900; i <= DateTime.Now.Year; i++)
			{
				dt.Rows.Add(i, i);
			}
			DataTable sortedDt = dt.Select("", "Id DESC").CopyToDataTable();
			DDL.BindDDL(sortedDt, cmbYear, "Id", "Year", "Year", ZeroIndex: false);
			((UltraGridBase)cmbYear).DisplayLayout.Bands[0].Columns[1].Width = 140;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			genEmployeeHistory obj = new genEmployeeHistory();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtemployee = genEmployeeHistory.GetAllActiveEmployee(obj);
			if (dtemployee.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtemployee, cmbEmployeeName, "EmployeeId", "EmployeeName", "Employee Name", false);
			}
			else
			{
				((UltraGridBase)cmbEmployeeName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbEmployeeName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbEmployeeName).ActiveRow != null)
			{
				if (dtemployee.Rows.Count > 0)
				{
					for (int i = 0; i < dtemployee.Rows.Count; i++)
					{
						if (Conversion.ToInt(cmbEmployeeName.Value) == Conversion.ToInt(dtemployee.Rows[i]["EmployeeId"]))
						{
							txtEmployeeNo.Text = Conversion.ToString(dtemployee.Rows[i]["EmployeeNo"]);
							txtDesgination.Text = Conversion.ToString(dtemployee.Rows[i]["DesignationName"]);
							txtDepartment.Text = Conversion.ToString(dtemployee.Rows[i]["DepartmentName"]);
							txtSalary.Text = Conversion.ToString(dtemployee.Rows[i]["TotalSalary"]);
							txtSection.Text = Conversion.ToString(dtemployee.Rows[i]["SectionName"]);
							LocationId = Conversion.ToInt(dtemployee.Rows[i]["LocationId"]);
						}
					}
				}
				else
				{
					txtEmployeeNo.Text = string.Empty;
					txtDesgination.Text = string.Empty;
					txtDepartment.Text = string.Empty;
					txtSalary.Text = string.Empty;
					txtSection.Text = string.Empty;
					LocationId = 0;
				}
			}
			else
			{
				txtEmployeeNo.Text = string.Empty;
				txtDesgination.Text = string.Empty;
				txtDepartment.Text = string.Empty;
				txtSalary.Text = string.Empty;
				txtSection.Text = string.Empty;
				LocationId = 0;
			}
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
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && !UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool Validation()
	{
		if (((UltraGridBase)cmbEmployeeName).ActiveRow == null)
		{
			MessageBox.Show("Employee Name Required!");
			cmbEmployeeName.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTotalHours.Text) == 0.0)
		{
			MessageBox.Show("Total Hours Field Required!");
			txtTotalHours.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtDeductedShortHours.Text) == 0.0)
		{
			MessageBox.Show("Total Deducted Hours Field Required!");
			txtDeductedShortHours.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtDeductionRate.Text) == 0.0)
		{
			MessageBox.Show("Total Deduction Rate Field Required!");
			txtDeductionRate.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		try
		{
			RecId = 0;
			((Control)(object)cmbEmployeeName).Text = string.Empty;
			cmbMonth.Value = string.Empty;
			cmbYear.Value = string.Empty;
			txtTotalHours.Text = string.Empty;
			txtTotalMinutes.Text = string.Empty;
			txtDeductionRate.Text = string.Empty;
			txtDeductedShortHours.Text = string.Empty;
			IsDeduction.Checked = false;
			IsLeaveAdjust.Checked = false;
			txtSalary.Text = string.Empty;
			txtEmployeeNo.Text = string.Empty;
			txtDepartment.Text = string.Empty;
			txtDesgination.Text = string.Empty;
			txtSection.Text = string.Empty;
			cmbEmployeeName.Focus();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			GridBind();
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

	private void btnRefresh_Click(object sender, EventArgs e)
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

	private void GridBind()
	{
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("EmployeeShortAdjustmentId");
			dt.Columns.Add("EmployeeId");
			dt.Columns.Add("EmployeeName");
			dt.Columns.Add("LocationId");
			dt.Columns.Add("SAMonth");
			dt.Columns.Add("SAYear");
			dt.Columns.Add("TotalShortHours", typeof(double));
			dt.Columns.Add("TotalShortMinutes", typeof(double));
			dt.Columns.Add("DeductedShortHours", typeof(double));
			dt.Columns.Add("DeductionRate", typeof(double));
			dt.Columns.Add("IsDeduction", typeof(bool));
			dt.Columns.Add("IsLeaveAdjust", typeof(bool));
			DataTable dtGrid = EmployeeShortAdjustment.FormHistory(new EmployeeShortAdjustment
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGrid.Rows.Count > 0)
			{
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrid.Rows[i]["EmployeeShortAdjustmentId"], dtGrid.Rows[i]["EmployeeId"], dtGrid.Rows[i]["EmployeeName"], dtGrid.Rows[i]["LocationId"], dtGrid.Rows[i]["SAMonth"], dtGrid.Rows[i]["SAYear"], dtGrid.Rows[i]["TotalShortHours"], dtGrid.Rows[i]["TotalShortMinutes"], dtGrid.Rows[i]["DeductedShortHours"], dtGrid.Rows[i]["DeductionRate"], dtGrid.Rows[i]["IsDeduction"], dtGrid.Rows[i]["IsLeaveAdjust"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSetting();
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

	private void grdSetting()
	{
		try
		{
			grd.RootTable.Columns["EmployeeName"].Width = 250;
			grd.RootTable.Columns["EmployeeShortAdjustmentId"].Visible = false;
			grd.RootTable.Columns["EmployeeId"].Visible = false;
			grd.RootTable.Columns["LocationId"].Visible = false;
			grd.RootTable.Columns["TotalShortHours"].FormatString = "#,#";
			grd.RootTable.Columns["TotalShortHours"].TotalFormatString = "#,#";
			grd.RootTable.Columns["TotalShortHours"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DeductedShortHours"].FormatString = "#,#";
			grd.RootTable.Columns["DeductedShortHours"].TotalFormatString = "#,#";
			grd.RootTable.Columns["DeductedShortHours"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DeductionRate"].FormatString = "#,#";
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeFamilyInfo_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RetrivedData(Conversion.ToInt((object)grd.CurrentRow.Cells[0].Value.ToString()));
			UpdateMode = true;
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
			EmployeeShortAdjustment obj = new EmployeeShortAdjustment();
			RecId = Id;
			obj = EmployeeShortAdjustment.GetByID(RecId);
			if (obj != null)
			{
				cmbEmployeeName.Value = Conversion.ToInt((object)obj.EmployeeId);
				cmbMonth.Value = obj.SAMonth;
				cmbYear.Value = obj.SAYear;
				txtTotalHours.Text = Conversion.ToInt((object)obj.TotalShortHours).ToString();
				txtTotalMinutes.Text = Conversion.ToInt((object)obj.TotalShortMinutes).ToString();
				txtDeductedShortHours.Text = Conversion.ToInt((object)obj.DeductedShortHours).ToString();
				txtDeductionRate.Text = Conversion.ToInt((object)obj.DeductionRate).ToString();
				IsDeduction.Checked = Conversion.ToBool((object)obj.IsDeduction);
				IsLeaveAdjust.Checked = Conversion.ToBool((object)obj.IsLeaveAdjust);
				cmbEmployeeName_Leave(null, null);
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (Validation())
			{
				EmployeeShortAdjustment obj = new EmployeeShortAdjustment();
				if (RecId > 0)
				{
					obj.EmployeeShortAdjustmentId = RecId;
				}
				obj.EmployeeId = Conversion.ToInt(cmbEmployeeName.Value);
				obj.LocationId = LocationId;
				obj.SAMonth = Conversion.ToInt(cmbMonth.Value);
				obj.SAYear = Conversion.ToInt(cmbYear.Value);
				obj.TotalShortHours = Conversion.ToInt((object)txtTotalHours.Text);
				obj.TotalShortMinutes = Conversion.ToInt((object)txtTotalMinutes.Text);
				obj.DeductedShortHours = Conversion.ToInt((object)txtDeductedShortHours.Text);
				obj.DeductionRate = Conversion.ToInt((object)txtDeductionRate.Text);
				obj.IsDeduction = Conversion.ToBool((object)IsDeduction.Checked);
				obj.IsLeaveAdjust = Conversion.ToBool((object)IsLeaveAdjust.Checked);
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.CreatedById = UserAccount.ID;
				obj.CreatedOn = DateTime.Now;
				obj.AlteredById = UserAccount.ID;
				obj.AlteredOn = DateTime.Now;
				obj.ApprovedOn = DateTime.Now;
				obj.ApprovedById = UserAccount.ID;
				Success = EmployeeShortAdjustment.Save(obj);
				if (RecId > 0)
				{
					MessageBox.Show("Update Successfully");
				}
				else
				{
					MessageBox.Show("Saved Successfully");
				}
				Reset();
			}
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

	private void btnEmployee_Click(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtTotalHours_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtTotalMinutes_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtDeductedShortHours_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtDeductionRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void EmployeeShortAdjustment_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PayRoll.EmployeeShortAdjustment));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.btnEmployee = new System.Windows.Forms.Button();
		this.cmbMonth = new UltraCombo();
		this.cmbYear = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtTotalMinutes = new System.Windows.Forms.TextBox();
		this.txtTotalHours = new System.Windows.Forms.TextBox();
		this.IsLeaveAdjust = new System.Windows.Forms.CheckBox();
		this.IsDeduction = new System.Windows.Forms.CheckBox();
		this.txtDeductionRate = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtDeductedShortHours = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtSalary = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtSection = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDepartment = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtDesgination = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtEmployeeNo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbEmployeeName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.sqlCeDataAdapter1 = new SqlCeDataAdapter();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(943, 383);
		this.panel1.TabIndex = 2;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 200);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(943, 183);
		this.panel4.TabIndex = 2;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(943, 152);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grdEmployeeFamilyInfo_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label14);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(943, 31);
		this.panel5.TabIndex = 130;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(893, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(9, 6);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(58, 17);
		this.label14.TabIndex = 20;
		this.label14.Text = "History";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.btnEmployee);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMonth);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbYear);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.txtTotalMinutes);
		this.panel3.Controls.Add(this.txtTotalHours);
		this.panel3.Controls.Add(this.IsLeaveAdjust);
		this.panel3.Controls.Add(this.IsDeduction);
		this.panel3.Controls.Add(this.txtDeductionRate);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.txtDeductedShortHours);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeName);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(943, 172);
		this.panel3.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label12);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(943, 31);
		this.panel6.TabIndex = 129;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(9, 6);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(237, 19);
		this.label12.TabIndex = 20;
		this.label12.Text = "Employee Short Adjustment";
		this.btnEmployee.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnEmployee.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployee.Location = new System.Drawing.Point(193, 60);
		this.btnEmployee.Name = "btnEmployee";
		this.btnEmployee.Size = new System.Drawing.Size(31, 26);
		this.btnEmployee.TabIndex = 1;
		this.btnEmployee.TabStop = false;
		this.btnEmployee.UseVisualStyleBackColor = true;
		this.btnEmployee.Click += new System.EventHandler(btnEmployee_Click);
		this.cmbMonth.AllowNull = (DefaultableBoolean)1;
		this.cmbMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMonth).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMonth).Location = new System.Drawing.Point(227, 60);
		((System.Windows.Forms.Control)(object)this.cmbMonth).Name = "cmbMonth";
		((System.Windows.Forms.Control)(object)this.cmbMonth).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.cmbMonth).TabIndex = 2;
		this.cmbYear.AllowNull = (DefaultableBoolean)1;
		this.cmbYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbYear).Location = new System.Drawing.Point(328, 60);
		((System.Windows.Forms.Control)(object)this.cmbYear).Name = "cmbYear";
		((System.Windows.Forms.Control)(object)this.cmbYear).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.cmbYear).TabIndex = 3;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(328, 42);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(29, 15);
		this.label10.TabIndex = 125;
		this.label10.Text = "Year";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(227, 42);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(43, 15);
		this.label9.TabIndex = 124;
		this.label9.Text = "Month";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(13, 90);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(79, 15);
		this.label11.TabIndex = 121;
		this.label11.Text = "Total Minutes";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(413, 42);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(68, 15);
		this.label6.TabIndex = 120;
		this.label6.Text = "Total Hours";
		this.txtTotalMinutes.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalMinutes.Location = new System.Drawing.Point(13, 109);
		this.txtTotalMinutes.Name = "txtTotalMinutes";
		this.txtTotalMinutes.Size = new System.Drawing.Size(100, 23);
		this.txtTotalMinutes.TabIndex = 5;
		this.txtTotalMinutes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTotalMinutes_KeyPress);
		this.txtTotalHours.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalHours.Location = new System.Drawing.Point(416, 62);
		this.txtTotalHours.Name = "txtTotalHours";
		this.txtTotalHours.Size = new System.Drawing.Size(100, 23);
		this.txtTotalHours.TabIndex = 4;
		this.txtTotalHours.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTotalHours_KeyPress);
		this.IsLeaveAdjust.AutoSize = true;
		this.IsLeaveAdjust.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.IsLeaveAdjust.Location = new System.Drawing.Point(328, 131);
		this.IsLeaveAdjust.Name = "IsLeaveAdjust";
		this.IsLeaveAdjust.Size = new System.Drawing.Size(105, 19);
		this.IsLeaveAdjust.TabIndex = 9;
		this.IsLeaveAdjust.Text = "Is Leave Adjust";
		this.IsLeaveAdjust.UseVisualStyleBackColor = true;
		this.IsDeduction.AutoSize = true;
		this.IsDeduction.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.IsDeduction.Location = new System.Drawing.Point(328, 108);
		this.IsDeduction.Name = "IsDeduction";
		this.IsDeduction.Size = new System.Drawing.Size(94, 19);
		this.IsDeduction.TabIndex = 8;
		this.IsDeduction.Text = "Is Deduction";
		this.IsDeduction.UseVisualStyleBackColor = true;
		this.txtDeductionRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeductionRate.Location = new System.Drawing.Point(227, 109);
		this.txtDeductionRate.Name = "txtDeductionRate";
		this.txtDeductionRate.Size = new System.Drawing.Size(97, 23);
		this.txtDeductionRate.TabIndex = 7;
		this.txtDeductionRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeductionRate_KeyPress);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(227, 90);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(89, 15);
		this.label13.TabIndex = 117;
		this.label13.Text = "Deduction Rate";
		this.txtDeductedShortHours.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeductedShortHours.Location = new System.Drawing.Point(114, 109);
		this.txtDeductedShortHours.Name = "txtDeductedShortHours";
		this.txtDeductedShortHours.Size = new System.Drawing.Size(110, 23);
		this.txtDeductedShortHours.TabIndex = 6;
		this.txtDeductedShortHours.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeductedShortHours_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(111, 90);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(113, 15);
		this.label2.TabIndex = 107;
		this.label2.Text = "Deduct Short Hours";
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.txtSalary);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtSection);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDepartment);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtDesgination);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtEmployeeNo);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(525, 40);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(413, 127);
		this.groupBox1.TabIndex = 10;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Employee Information";
		this.txtSalary.BackColor = System.Drawing.Color.White;
		this.txtSalary.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSalary.Location = new System.Drawing.Point(298, 19);
		this.txtSalary.Name = "txtSalary";
		this.txtSalary.ReadOnly = true;
		this.txtSalary.Size = new System.Drawing.Size(108, 23);
		this.txtSalary.TabIndex = 1;
		this.txtSalary.TabStop = false;
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(204, 23);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(94, 15);
		this.label8.TabIndex = 29;
		this.label8.Text = "Employee Salary";
		this.txtSection.BackColor = System.Drawing.Color.White;
		this.txtSection.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSection.Location = new System.Drawing.Point(92, 97);
		this.txtSection.Name = "txtSection";
		this.txtSection.ReadOnly = true;
		this.txtSection.Size = new System.Drawing.Size(314, 23);
		this.txtSection.TabIndex = 4;
		this.txtSection.TabStop = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(10, 101);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(47, 15);
		this.label7.TabIndex = 29;
		this.label7.Text = "Section";
		this.txtDepartment.BackColor = System.Drawing.Color.White;
		this.txtDepartment.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDepartment.Location = new System.Drawing.Point(92, 71);
		this.txtDepartment.Name = "txtDepartment";
		this.txtDepartment.ReadOnly = true;
		this.txtDepartment.Size = new System.Drawing.Size(314, 23);
		this.txtDepartment.TabIndex = 3;
		this.txtDepartment.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(10, 75);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 15);
		this.label5.TabIndex = 29;
		this.label5.Text = "Department";
		this.txtDesgination.BackColor = System.Drawing.Color.White;
		this.txtDesgination.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDesgination.Location = new System.Drawing.Point(92, 45);
		this.txtDesgination.Name = "txtDesgination";
		this.txtDesgination.ReadOnly = true;
		this.txtDesgination.Size = new System.Drawing.Size(314, 23);
		this.txtDesgination.TabIndex = 2;
		this.txtDesgination.TabStop = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(10, 49);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 29;
		this.label4.Text = "Desgination";
		this.txtEmployeeNo.BackColor = System.Drawing.Color.White;
		this.txtEmployeeNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEmployeeNo.Location = new System.Drawing.Point(92, 19);
		this.txtEmployeeNo.Name = "txtEmployeeNo";
		this.txtEmployeeNo.ReadOnly = true;
		this.txtEmployeeNo.Size = new System.Drawing.Size(108, 23);
		this.txtEmployeeNo.TabIndex = 0;
		this.txtEmployeeNo.TabStop = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(10, 23);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 29;
		this.label3.Text = "Employee No";
		this.cmbEmployeeName.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbEmployeeName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Location = new System.Drawing.Point(13, 60);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Name = "cmbEmployeeName";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Leave += new System.EventHandler(cmbEmployeeName_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(13, 42);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(94, 15);
		this.label1.TabIndex = 19;
		this.label1.Text = "Employee Name";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(943, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnupdate, this.btnsave, this.btnPrint, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(943, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(78, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.sqlCeDataAdapter1.DeleteCommand = null;
		this.sqlCeDataAdapter1.InsertCommand = null;
		this.sqlCeDataAdapter1.SelectCommand = null;
		this.sqlCeDataAdapter1.UpdateCommand = null;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(943, 383);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EmployeeShortAdjustment";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(EmployeeShortAdjustment_FormClosing);
		base.Load += new System.EventHandler(EmployeeFamilyInfo_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EmployeeFamilyInfo_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
