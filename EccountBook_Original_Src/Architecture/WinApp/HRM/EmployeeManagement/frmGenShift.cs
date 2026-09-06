using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.AttendanceManagement;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.EmployeeManagement;

public class frmGenShift : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private int RecId;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TextBox txtShiftUrduName;

	private Label label3;

	private TextBox txtShiftName;

	private Label label1;

	private TextBox txtShortName;

	private Label label2;

	internal UltraCombo comproject;

	private Label label41;

	internal UltraCombo combranches;

	private Label label42;

	private ToolStripButton btnShiftTiming;

	private Button btnUpdateF;

	private Button BtnCancel;

	private Button BtnSavef;

	private Label label4;

	internal GridEX grdShift;

	private Panel panel5;

	private Label label5;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripSeparator toolStripSeparator1;

	public frmGenShift(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGenShift_Load(object sender, EventArgs e)
	{
		Project();
		braches();
		GridBind();
		txtShortName.Focus();
		btnsave.Visible = true;
		btnupdate.Visible = false;
		BtnSavef.Visible = true;
		btnUpdateF.Visible = false;
	}

	private void Project()
	{
		try
		{
			DataTable dtProject = CommonServices.ProjectServiceBind();
			if (dtProject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProject, comproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)comproject).DisplayLayout.Bands[0].Columns[1].Width = 168;
				((UltraGridBase)comproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void braches()
	{
		try
		{
			DataTable dtBranch = CommonServices.BrancheServiceBind();
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtBranch, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).DisplayLayout.Bands[0].Columns[1].Width = 180;
				((UltraGridBase)combranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public bool Validation()
	{
		if (txtShiftName.Text == string.Empty)
		{
			MessageBox.Show("Shift Name Filed Required!");
			txtShiftName.Focus();
			return false;
		}
		return true;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		try
		{
			if (Validation() && genShift.Save(new genShift
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				CreatedById = UserAccount.ID,
				BranchId = Conversion.ToInt(combranches.Value),
				ProjectId = Conversion.ToInt(comproject.Value),
				ShiftName = txtShiftName.Text,
				ShiftShortName = txtShortName.Text,
				ShiftUrduName = txtShiftUrduName.Text
			}) > 0)
			{
				MessageBox.Show("Save Successfully");
				ResetForm();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		try
		{
			if (Validation() && genShift.Save(new genShift
			{
				ShiftId = RecId,
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				AlteredById = UserAccount.ID,
				BranchId = Conversion.ToInt(combranches.Value),
				ProjectId = Conversion.ToInt(comproject.Value),
				ShiftName = txtShiftName.Text,
				ShiftShortName = txtShortName.Text,
				ShiftUrduName = txtShiftUrduName.Text
			}) > 0)
			{
				MessageBox.Show("Update Successfully");
				ResetForm();
			}
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
			genShift shift = new genShift();
			RecId = Id;
			shift = genShift.GetByID(RecId);
			if (shift != null)
			{
				combranches.Value = shift.BranchId;
				comproject.Value = shift.ProjectId;
				txtShiftName.Text = shift.ShiftName;
				txtShortName.Text = shift.ShiftShortName;
				txtShiftUrduName.Text = shift.ShiftUrduName;
				UpdateMode = true;
				btnsave.Visible = false;
				btnupdate.Visible = true;
				BtnSavef.Visible = false;
				btnUpdateF.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		ResetForm();
	}

	public void ResetForm()
	{
		try
		{
			((Control)(object)combranches).Text = string.Empty;
			((Control)(object)comproject).Text = string.Empty;
			txtShortName.Text = string.Empty;
			txtShiftUrduName.Text = string.Empty;
			txtShiftName.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
			RecId = 0;
			UpdateMode = false;
			Project();
			braches();
			GridBind();
			txtShortName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genShift.Getall(new genShift
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dthiostory = new DataTable();
				dthiostory.Columns.Add("Id");
				dthiostory.Columns.Add("Shift");
				dthiostory.Columns.Add("ShortName");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dthiostory.Rows.Add(lst.Rows[i]["ShiftId"], lst.Rows[i]["ShiftName"], lst.Rows[i]["ShiftShortName"]);
				}
				grdShift.DataSource = dthiostory;
				grdShift.RetrieveStructure();
				GRidSetting();
			}
			else
			{
				grdShift.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GRidSetting()
	{
		try
		{
			grdShift.RootTable.Columns["Shift"].EditType = (EditType)0;
			grdShift.RootTable.Columns["ShortName"].EditType = (EditType)0;
			grdShift.RootTable.Columns["Shift"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["ShortName"].FilterEditType = (FilterEditType)1;
			grdShift.RootTable.Columns["Id"].Visible = false;
			grdShift.RootTable.Columns["Shift"].Width = 150;
			grdShift.RootTable.Columns["ShortName"].Width = 100;
			ctrlGrdBar2_Load(null, null);
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
			ReadById(Conversion.ToInt(grdShift.CurrentRow.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGenShift_KeyDown(object sender, KeyEventArgs e)
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

	private void btnShiftTiming_Click(object sender, EventArgs e)
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void frmGenShift_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.EmployeeManagement.frmGenShift));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdShift = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.comproject = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.txtShiftUrduName = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtShiftName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtShortName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShiftTiming = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdShift).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(434, 561);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Multiline = true;
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(434, 561);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(426, 535);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdShift);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(420, 529);
		this.panel2.TabIndex = 0;
		this.grdShift.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdShift).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdShift.FilterMode = (FilterMode)2;
		this.grdShift.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdShift).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdShift.GroupByBoxVisible = false;
		this.grdShift.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdShift).Location = new System.Drawing.Point(0, 229);
		((System.Windows.Forms.Control)(object)this.grdShift).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdShift).Name = "grdShift";
		((ScrollableBase)this.grdShift).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdShift).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdShift).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdShift).Size = new System.Drawing.Size(420, 300);
		((System.Windows.Forms.Control)(object)this.grdShift).TabIndex = 1;
		this.grdShift.TotalRow = (InheritableBoolean)1;
		this.grdShift.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdShift).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdShift).DoubleClick += new System.EventHandler(grdShift_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar2);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 198);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(420, 31);
		this.panel5.TabIndex = 11;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(372, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdShift;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 28);
		this.ctrlGrdBar2.TabIndex = 24;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 6);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 19);
		this.label5.TabIndex = 10;
		this.label5.Text = "History";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.btnUpdateF);
		this.panel4.Controls.Add(this.BtnCancel);
		this.panel4.Controls.Add(this.BtnSavef);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel4.Controls.Add(this.label41);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel4.Controls.Add(this.label42);
		this.panel4.Controls.Add(this.txtShiftUrduName);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtShiftName);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtShortName);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(420, 141);
		this.panel4.TabIndex = 0;
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(308, 93);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 3;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.btnUpdateF.Click += new System.EventHandler(btnupdate_Click);
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(229, 93);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 4;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(308, 93);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 117;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.BtnSavef.Click += new System.EventHandler(btnsave_Click);
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(251, 222);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(179, 228);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(46, 15);
		this.label41.TabIndex = 115;
		this.label41.Text = "Project";
		this.label41.Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(251, 190);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(179, 190);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(46, 15);
		this.label42.TabIndex = 114;
		this.label42.Text = "Branch";
		this.label42.Visible = false;
		this.txtShiftUrduName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShiftUrduName.Location = new System.Drawing.Point(157, 64);
		this.txtShiftUrduName.Name = "txtShiftUrduName";
		this.txtShiftUrduName.Size = new System.Drawing.Size(226, 23);
		this.txtShiftUrduName.TabIndex = 2;
		this.txtShiftUrduName.Visible = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(83, 68);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(68, 15);
		this.label3.TabIndex = 110;
		this.label3.Text = "Urdu Name";
		this.label3.Visible = false;
		this.txtShiftName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShiftName.Location = new System.Drawing.Point(157, 6);
		this.txtShiftName.Name = "txtShiftName";
		this.txtShiftName.Size = new System.Drawing.Size(226, 23);
		this.txtShiftName.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(83, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 15);
		this.label1.TabIndex = 108;
		this.label1.Text = "Shift Name";
		this.txtShortName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShortName.Location = new System.Drawing.Point(157, 35);
		this.txtShortName.Name = "txtShortName";
		this.txtShortName.Size = new System.Drawing.Size(226, 23);
		this.txtShortName.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(83, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 15);
		this.label2.TabIndex = 107;
		this.label2.Text = "Short Name";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(420, 57);
		this.panel3.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(6, 33);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(105, 19);
		this.label4.TabIndex = 9;
		this.label4.Text = "Define Shift";
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnupdate, this.btnsave, this.toolStripSeparator1, this.btnShiftTiming });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(420, 27);
		this.toolStrip1.TabIndex = 8;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 10.25f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 10.25f);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 10.25f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(61, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnShiftTiming.Font = new System.Drawing.Font("Segoe UI", 10.25f);
		this.btnShiftTiming.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShiftTiming.Name = "btnShiftTiming";
		this.btnShiftTiming.Size = new System.Drawing.Size(81, 24);
		this.btnShiftTiming.Text = "ShiftTiming";
		this.btnShiftTiming.Click += new System.EventHandler(btnShiftTiming_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(434, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmGenShift";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "frmGenShift";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmGenShift_FormClosing);
		base.Load += new System.EventHandler(frmGenShift_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGenShift_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdShift).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
