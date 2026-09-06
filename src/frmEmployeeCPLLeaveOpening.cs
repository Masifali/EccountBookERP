using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.AttendanceManagement;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.AttendanceManagement;
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
namespace Architecture.WinApp.HRM.LeaveManagment
{

public class frmEmployeeCPLLeaveOpening : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private int updateDetailIndex;

	public List<CPLAttendance> Deletelst = new List<CPLAttendance>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripSeparator toolStripSeparator;

	private Panel panel6;

	private Label label15;

	private Panel panel7;

	internal UltraCombo cmbEmployee;

	internal GridEX grd;

	private Panel panel9;

	internal GridEX DataGridHistory;

	private Panel panel8;

	private Button btnAdd;

	private Label label2;

	private Label label4;

	private Label label3;

	private Label label1;

	private DateTimePicker datOutTime;

	private DateTimePicker datInTime;

	private DateTimePicker datDate;

	private Label label5;

	private TextBox txtCplQuota;

	private Button btnUpdate;

	private Button btnCancel;

	private Label label6;

	private Button BtnEmploye;

	private Panel panel11;

	private Panel panel10;

	private Label label7;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnRefresh;

	public frmEmployeeCPLLeaveOpening(UserAccount userAccount)
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
			cmbEmployee.Focus();
			dtGrid.Columns.Add("Id");
			dtGrid.Columns.Add("EmployeeId");
			dtGrid.Columns.Add("EmployeeName");
			dtGrid.Columns.Add("CPLDate");
			dtGrid.Columns.Add("InTime");
			dtGrid.Columns.Add("OutTime");
			dtGrid.Columns.Add("CPLQuota", typeof(decimal));
			EmployeeNameFill();
			btnAdd.Visible = true;
			btnUpdate.Visible = false;
			btnCancel.Visible = false;
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
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtEmployeeName = genEmployeeHistory.GetAllEmployeesActive(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtEmployeeName.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtEmployeeName, cmbEmployee, "EmployeeId", "EmployeeName", "EmployeeName", false);
				((UltraGridBase)cmbEmployee).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSetting()
	{
		try
		{
			grd.RootTable.Columns["EmployeeId"].Visible = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["CPLQuota"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CPLQuota"].FormatString = "#,#.##";
			grd.RootTable.Columns["CPLQuota"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["CPLQuota"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Caption = "Delete";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].Image = Resources.icons8_delete_bin_16;
			grd.RootTable.Columns["Delete"].Width = 35;
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
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			CPLAttendance obj = new CPLAttendance();
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.CPLAttendancesList = new List<CPLAttendance>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				CPLAttendance ObjDt = new CPLAttendance();
				ObjDt.CPLAttendanceId = Conversion.ToInt64(item.Cells["Id"].Value);
				ObjDt.EmployeeId = Conversion.ToInt64(item.Cells["EmployeeId"].Value);
				ObjDt.CPLDate = Conversion.ToDateTime(item.Cells["CPLDate"].Value);
				ObjDt.CPLQuota = Conversion.ToDecimal(item.Cells["CPLQuota"].Value);
				string DutyDate = ObjDt.CPLDate.ToShortDateString();
				ObjDt.InTime = Conversion.ToDateTime(item.Cells["InTime"].Value);
				ObjDt.OutTime = Conversion.ToDateTime(item.Cells["OutTime"].Value);
				string DutyInTimeOnly = ObjDt.InTime.ToString("hh:mm tt");
				string DutyoutTimeOnly = ObjDt.OutTime.ToString("hh:mm tt");
				ObjDt.InTime = Convert.ToDateTime(DutyDate + " " + DutyInTimeOnly);
				ObjDt.OutTime = Convert.ToDateTime(DutyDate + " " + DutyoutTimeOnly);
				ObjDt.CreatedById = UserAccount.ID;
				ObjDt.AlteredById = UserAccount.ID;
				ObjDt.CreatedOn = DateTime.Now;
				ObjDt.AlteredOn = DateTime.Now;
				ObjDt.EmployeeAttendanceId = 0L;
				if (ObjDt.CPLAttendanceId == 0L)
				{
					ObjDt.ActionTypeId = 1;
				}
				else
				{
					ObjDt.ActionTypeId = 2;
				}
				ObjDt.OrganizationId = UserAccount.OrganizationId;
				ObjDt.CompanyId = UserAccount.CompanyId;
				ObjDt.ReqTypeId = 1;
				obj.CPLAttendancesList.Add(ObjDt);
			}
			if (Deletelst.Count > 0)
			{
				for (int i = 0; i < Deletelst.Count; i++)
				{
					obj.CPLAttendancesList.Add(Deletelst[i]);
				}
			}
			CPLAttendance.Save(obj);
			MessageBox.Show("Saved Successfully");
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DataBaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void ReadById(long Id)
	{
		try
		{
			dtGrid.Rows.Clear();
			CPLAttendance obj = CPLAttendance.ReadById(Id);
			if (obj != null)
			{
				tabControl1.SelectedIndex = 0;
				dtGrid.Rows.Add(obj.CPLAttendanceId, obj.EmployeeId, obj.EmployeeName, obj.CPLDate.ToString("dd-MMM.yy"), obj.InTime.ToString("HH:mm tt"), obj.OutTime.ToString("HH:mm tt"), Conversion.ToDecimal((object)obj.CPLQuota));
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				GrdSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
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
			cmbEmployee.Focus();
			((Control)(object)cmbEmployee).Text = string.Empty;
			txtCplQuota.Text = string.Empty;
			datDate.Value = DateTime.Now;
			dtGrid.Rows.Clear();
			grd.DataSource = dtGrid;
			btnAdd.Visible = true;
			btnUpdate.Visible = false;
			btnCancel.Visible = false;
			Deletelst.Clear();
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

	private void btnLoad_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValiadation())
			{
				dtGrid.Rows.Add(0, cmbEmployee.Value, ((Control)(object)cmbEmployee).Text.Trim(), datDate.Value.ToString("dd-MMM-yy"), datInTime.Value.ToString("HH:mm tt"), datOutTime.Value.ToString("HH:mm tt"), txtCplQuota.Text.Trim());
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				GrdSetting();
				((Control)(object)cmbEmployee).Text = string.Empty;
				cmbEmployee.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbEmployee.Value = item.Cells["EmployeeId"].Value.ToString();
				((Control)(object)cmbEmployee).Text = item.Cells["EmployeeName"].Value.ToString();
				datDate.Text = item.Cells["CPLDate"].Value.ToString();
				datInTime.Text = item.Cells["InTime"].Value.ToString();
				datOutTime.Text = item.Cells["OutTime"].Value.ToString();
				txtCplQuota.Text = Conversion.ToDecimal(item.Cells["CPLQuota"].Value).ToString();
				btnAdd.Visible = false;
				btnUpdate.Visible = true;
				btnCancel.Visible = true;
				cmbEmployee.Focus();
			}
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
			dtGrid.Rows[updateDetailIndex]["EmployeeId"] = cmbEmployee.Value;
			dtGrid.Rows[updateDetailIndex]["EmployeeName"] = ((Control)(object)cmbEmployee).Text;
			dtGrid.Rows[updateDetailIndex]["CPLDate"] = datDate.Text;
			dtGrid.Rows[updateDetailIndex]["InTime"] = datInTime.Text;
			dtGrid.Rows[updateDetailIndex]["OutTime"] = datOutTime.Text;
			dtGrid.Rows[updateDetailIndex]["CPLQuota"] = txtCplQuota.Text;
			grd.DataSource = dtGrid;
			btnAdd.Visible = true;
			btnUpdate.Visible = false;
			btnCancel.Visible = false;
			cmbEmployee.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbEmployee).Text = "";
			txtCplQuota.Text = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		try
		{
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt64(item.Cells["Id"].Value) > 0)
				{
					CPLAttendance ObjDt = new CPLAttendance();
					ObjDt.CPLAttendanceId = Conversion.ToInt64(item.Cells["Id"].Value);
					ObjDt.EmployeeId = Conversion.ToInt64(item.Cells["EmployeeId"].Value);
					ObjDt.CPLDate = Conversion.ToDateTime(item.Cells["CPLDate"].Value);
					ObjDt.CPLQuota = Conversion.ToDecimal(item.Cells["CPLQuota"].Value);
					string DutyDate = ObjDt.CPLDate.ToShortDateString();
					ObjDt.InTime = Conversion.ToDateTime(item.Cells["InTime"].Value);
					ObjDt.OutTime = Conversion.ToDateTime(item.Cells["OutTime"].Value);
					string DutyInTimeOnly = ObjDt.InTime.ToString("hh:mm tt");
					string DutyoutTimeOnly = ObjDt.OutTime.ToString("hh:mm tt");
					ObjDt.InTime = Convert.ToDateTime(DutyDate + " " + DutyInTimeOnly);
					ObjDt.OutTime = Convert.ToDateTime(DutyDate + " " + DutyoutTimeOnly);
					ObjDt.CreatedById = UserAccount.ID;
					ObjDt.AlteredById = UserAccount.ID;
					ObjDt.CreatedOn = DateTime.Now;
					ObjDt.AlteredOn = DateTime.Now;
					ObjDt.ActionTypeId = 3;
					ObjDt.OrganizationId = UserAccount.OrganizationId;
					ObjDt.CompanyId = UserAccount.CompanyId;
					Deletelst.Add(ObjDt);
				}
			}
			dtGrid.Rows[item.RowIndex].Delete();
			if (item != null && (int)item.RowType == 0)
			{
				item.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DataBaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = CPLAttendance.ReadAll(new CPLAttendance
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReqTypeId = 1
			});
			if (dt.Rows.Count > 0)
			{
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				DataGridHistory.RootTable.Columns["CPLAttendanceId"].Visible = false;
				DataGridHistory.RootTable.Columns["EmployeeId"].Visible = false;
				DataGridHistory.RootTable.Columns["CPLQuota"].FormatString = "#,#.##";
				DataGridHistory.RootTable.Columns["CPLQuota"].TotalFormatString = "#,#.##";
				DataGridHistory.RootTable.Columns["CPLQuota"].AggregateFunction = (AggregateFunction)2;
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValiadation()
	{
		if (((UltraGridBase)cmbEmployee).ActiveRow == null)
		{
			cmbEmployee.Focus();
			MessageBox.Show("EmployeeName Field Required");
			return false;
		}
		if (Conversion.ToDecimal((object)txtCplQuota.Text) == 0m)
		{
			txtCplQuota.Focus();
			MessageBox.Show("CPL Quota Field Required");
			return false;
		}
		return true;
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt64(DataGridHistory.CurrentRow.Cells["CPLAttendanceId"].Value));
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
			GetHistory();
		}
	}

	private void txtCplQuota_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnEmploye_Click(object sender, EventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			EmployeeNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmEmployeeCPLLeaveOpening_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance18 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.LeaveManagment.frmEmployeeCPLLeaveOpening));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.BtnEmploye = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.txtCplQuota = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.datOutTime = new System.Windows.Forms.DateTimePicker();
		this.datInTime = new System.Windows.Forms.DateTimePicker();
		this.datDate = new System.Windows.Forms.DateTimePicker();
		this.btnUpdate = new System.Windows.Forms.Button();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbEmployee = new UltraCombo();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1047, 719);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1047, 719);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1039, 693);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1033, 687);
		this.panel2.TabIndex = 0;
		this.panel7.Controls.Add(this.panel11);
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 129);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1033, 558);
		this.panel7.TabIndex = 3;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 27);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1033, 531);
		this.panel11.TabIndex = 2;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.ColumnAutoResize = true;
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1033, 531);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1033, 27);
		this.panel10.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(986, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 28;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(7, 5);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(46, 17);
		this.label7.TabIndex = 27;
		this.label7.Text = "Detail";
		this.panel5.Controls.Add(this.BtnEmploye);
		this.panel5.Controls.Add(this.btnCancel);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.txtCplQuota);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.datOutTime);
		this.panel5.Controls.Add(this.datInTime);
		this.panel5.Controls.Add(this.datDate);
		this.panel5.Controls.Add(this.btnUpdate);
		this.panel5.Controls.Add(this.btnAdd);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployee);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 58);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1033, 71);
		this.panel5.TabIndex = 0;
		this.BtnEmploye.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnEmploye.Location = new System.Drawing.Point(239, 31);
		this.BtnEmploye.Name = "BtnEmploye";
		this.BtnEmploye.Size = new System.Drawing.Size(28, 26);
		this.BtnEmploye.TabIndex = 1;
		this.BtnEmploye.TabStop = false;
		this.BtnEmploye.UseVisualStyleBackColor = true;
		this.BtnEmploye.Click += new System.EventHandler(BtnEmploye_Click);
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(880, 32);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(70, 25);
		this.btnCancel.TabIndex = 8;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(640, 12);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(63, 15);
		this.label5.TabIndex = 577;
		this.label5.Text = "CPL Quota";
		this.txtCplQuota.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCplQuota.Location = new System.Drawing.Point(636, 33);
		this.txtCplQuota.Name = "txtCplQuota";
		this.txtCplQuota.Size = new System.Drawing.Size(119, 23);
		this.txtCplQuota.TabIndex = 5;
		this.txtCplQuota.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCplQuota_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(518, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(54, 15);
		this.label4.TabIndex = 575;
		this.label4.Text = "OutTime";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(393, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(45, 15);
		this.label3.TabIndex = 575;
		this.label3.Text = "InTime";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(277, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(32, 15);
		this.label1.TabIndex = 575;
		this.label1.Text = "Date";
		this.datOutTime.CustomFormat = "HH:mm tt";
		this.datOutTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datOutTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datOutTime.Location = new System.Drawing.Point(514, 33);
		this.datOutTime.Name = "datOutTime";
		this.datOutTime.Size = new System.Drawing.Size(119, 23);
		this.datOutTime.TabIndex = 4;
		this.datInTime.CustomFormat = "HH:mm tt";
		this.datInTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datInTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datInTime.Location = new System.Drawing.Point(389, 33);
		this.datInTime.Name = "datInTime";
		this.datInTime.Size = new System.Drawing.Size(119, 23);
		this.datInTime.TabIndex = 3;
		this.datDate.CustomFormat = "dd-MMM-yyyy";
		this.datDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDate.Location = new System.Drawing.Point(273, 33);
		this.datDate.Name = "datDate";
		this.datDate.Size = new System.Drawing.Size(119, 23);
		this.datDate.TabIndex = 2;
		this.btnUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.ForeColor = System.Drawing.Color.White;
		this.btnUpdate.Location = new System.Drawing.Point(810, 32);
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(70, 25);
		this.btnUpdate.TabIndex = 7;
		this.btnUpdate.Text = "Update";
		this.btnUpdate.UseVisualStyleBackColor = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(758, 32);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(52, 25);
		this.btnAdd.TabIndex = 6;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnLoad_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(94, 15);
		this.label2.TabIndex = 573;
		this.label2.Text = "Employee Name";
		this.cmbEmployee.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Location = new System.Drawing.Point(9, 31);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Name = "cmbEmployee";
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1033, 30);
		this.panel4.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(7, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(112, 19);
		this.label15.TabIndex = 26;
		this.label15.Text = "CPL Opening";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1033, 28);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.toolStripSeparator, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1033, 27);
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
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1039, 693);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1033, 687);
		this.panel6.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 27);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1033, 660);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1033, 660);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1033, 27);
		this.panel8.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = null;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(989, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar2.TabIndex = 28;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(5, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(175, 19);
		this.label6.TabIndex = 27;
		this.label6.Text = "CPL Opening History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1047, 719);
		base.Controls.Add(this.panel1);
		base.Name = "frmEmployeeCPLLeaveOpening";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEmployeeCPLLeaveOpening_FormClosing);
		base.Load += new System.EventHandler(frmItemPricingSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemPricingSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).EndInit();
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

}
