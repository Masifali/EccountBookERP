using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.PayRoll;

public class frmEmployeeSalary : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private long Success;

	private int EmployeeHistoryId;

	private DataTable dtemployee = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	internal GridEX grd;

	private Panel panel3;

	internal UltraCombo cmbEmployeeName;

	private Label label1;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripLabel toolStripLabel1;

	private ToolStripSeparator toolStripSeparator1;

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

	private Label label15;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbSalaryType;

	private Label label11;

	private Label label2;

	private TextBox txtAmount;

	private TextBox txtTypePercent;

	public frmEmployeeSalary(UserAccount pUserAccount)
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
			EmployeeName();
			SalaryTypeFill();
			GridBind();
			btnsave.Visible = true;
			btnupdate.Visible = false;
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

	private void SalaryTypeFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			DataTable dtSalaryType = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ProfileTypeId = 37
			});
			if (dtSalaryType.Rows.Count > 0)
			{
				for (int i = 0; i < dtSalaryType.Rows.Count; i++)
				{
					dt.Rows.Add(dtSalaryType.Rows[i]["ProfileId"], dtSalaryType.Rows[i]["ProfileName"]);
				}
				DDL.BindDDL(dt, cmbSalaryType, "Id", "Name", "Salary Type", ZeroIndex: false);
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
		if (((UltraGridBase)cmbSalaryType).ActiveRow == null)
		{
			MessageBox.Show("Salary Type Required!");
			cmbSalaryType.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmount.Text) == 0.0)
		{
			MessageBox.Show("Amount Field Required!");
			txtAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTypePercent.Text) == 0.0)
		{
			MessageBox.Show("Type Percent Field Required!");
			txtTypePercent.Focus();
			return false;
		}
		if (Conversion.ToInt((object)EmployeeHistoryId) == 0)
		{
			MessageBox.Show("Employee History Required");
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
			((Control)(object)cmbSalaryType).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtTypePercent.Text = string.Empty;
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

	private void GridBind()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("EmployeeSalaryId");
			dt.Columns.Add("EmployeeId");
			dt.Columns.Add("EmployeeName");
			dt.Columns.Add("EmployeeHistoryId");
			dt.Columns.Add("SalaryType");
			dt.Columns.Add("SalaryTypePercent", typeof(double));
			dt.Columns.Add("Amount", typeof(double));
			DataTable dtGrid = EmployeeSalary.FormHistory(new EmployeeSalary
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGrid.Rows.Count > 0)
			{
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrid.Rows[i]["EmployeeSalaryId"], dtGrid.Rows[i]["EmployeeId"], dtGrid.Rows[i]["EmployeeName"], dtGrid.Rows[i]["EmployeeHistoryId"], dtGrid.Rows[i]["SalaryType"], dtGrid.Rows[i]["SalaryTypePercent"], dtGrid.Rows[i]["Amount"]);
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
			grd.RootTable.Columns["EmployeeSalaryId"].Visible = false;
			grd.RootTable.Columns["EmployeeId"].Visible = false;
			grd.RootTable.Columns["EmployeeHistoryId"].Visible = false;
			grd.RootTable.Columns["Amount"].Visible = false;
			grd.RootTable.Columns["Amount"].FormatString = "#,#";
			grd.RootTable.Columns["Amount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RetrivedData(int Id)
	{
		try
		{
			new List<EmployeeSalary>();
			RecId = Id;
			if (EmployeeSalary.GetByID((long)RecId, 0L) == null)
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
				EmployeeSalary obj = new EmployeeSalary();
				if (RecId > 0)
				{
					obj.EmployeeSalaryId = RecId;
				}
				obj.EmployeeId = Conversion.ToInt(cmbEmployeeName.Value);
				obj.SalaryTypeProfileId = Conversion.ToInt(cmbSalaryType.Value);
				obj.SalaryTypePercent = Conversion.ToDecimal((object)txtTypePercent.Text);
				obj.Amount = Conversion.ToDecimal((object)txtAmount.Text);
				obj.EmployeeHistoryId = EmployeeHistoryId;
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.CreatedById = UserAccount.ID;
				obj.CreatedOn = DateTime.Now;
				obj.AlteredById = UserAccount.ID;
				obj.AlteredOn = DateTime.Now;
				Success = EmployeeSalary.Save(obj);
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
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
							EmployeeHistoryId = Conversion.ToInt(dtemployee.Rows[i]["EmployeeHistoryId"]);
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
					EmployeeHistoryId = 0;
				}
			}
			else
			{
				txtEmployeeNo.Text = string.Empty;
				txtDesgination.Text = string.Empty;
				txtDepartment.Text = string.Empty;
				txtSalary.Text = string.Empty;
				txtSection.Text = string.Empty;
				EmployeeHistoryId = 0;
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void frmEmployeeSalary_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance1 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PayRoll.frmEmployeeSalary));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtTypePercent = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbSalaryType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
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
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSalaryType).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1100, 562);
		this.panel1.TabIndex = 2;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 232);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1100, 330);
		this.panel4.TabIndex = 2;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1100, 330);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grdEmployeeFamilyInfo_DoubleClick);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.txtAmount);
		this.panel3.Controls.Add(this.txtTypePercent);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSalaryType);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeName);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1100, 204);
		this.panel3.TabIndex = 0;
		this.txtAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(499, 29);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(100, 23);
		this.txtAmount.TabIndex = 3;
		this.txtTypePercent.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTypePercent.Location = new System.Drawing.Point(396, 29);
		this.txtTypePercent.Name = "txtTypePercent";
		this.txtTypePercent.Size = new System.Drawing.Size(100, 23);
		this.txtTypePercent.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(499, 10);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(49, 15);
		this.label2.TabIndex = 107;
		this.label2.Text = "Amount";
		this.cmbSalaryType.AllowNull = (DefaultableBoolean)1;
		this.cmbSalaryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSalaryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSalaryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSalaryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.cmbSalaryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSalaryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSalaryType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSalaryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSalaryType).Location = new System.Drawing.Point(249, 29);
		((System.Windows.Forms.Control)(object)this.cmbSalaryType).Name = "cmbSalaryType";
		((System.Windows.Forms.Control)(object)this.cmbSalaryType).Size = new System.Drawing.Size(144, 26);
		((System.Windows.Forms.Control)(object)this.cmbSalaryType).TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(249, 10);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 105;
		this.label11.Text = "Salary Type";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(396, 10);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(75, 15);
		this.label15.TabIndex = 29;
		this.label15.Text = "Type Percent";
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
		this.groupBox1.Location = new System.Drawing.Point(724, 8);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(364, 189);
		this.groupBox1.TabIndex = 102;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Employee Information";
		this.txtSalary.BackColor = System.Drawing.Color.White;
		this.txtSalary.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSalary.Location = new System.Drawing.Point(121, 138);
		this.txtSalary.Name = "txtSalary";
		this.txtSalary.ReadOnly = true;
		this.txtSalary.Size = new System.Drawing.Size(234, 25);
		this.txtSalary.TabIndex = 100;
		this.txtSalary.TabStop = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(12, 145);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(97, 15);
		this.label8.TabIndex = 29;
		this.label8.Text = "Employee Salary";
		this.txtSection.BackColor = System.Drawing.Color.White;
		this.txtSection.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSection.Location = new System.Drawing.Point(121, 108);
		this.txtSection.Name = "txtSection";
		this.txtSection.ReadOnly = true;
		this.txtSection.Size = new System.Drawing.Size(234, 25);
		this.txtSection.TabIndex = 100;
		this.txtSection.TabStop = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(12, 115);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(46, 15);
		this.label7.TabIndex = 29;
		this.label7.Text = "Section";
		this.txtDepartment.BackColor = System.Drawing.Color.White;
		this.txtDepartment.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDepartment.Location = new System.Drawing.Point(121, 79);
		this.txtDepartment.Name = "txtDepartment";
		this.txtDepartment.ReadOnly = true;
		this.txtDepartment.Size = new System.Drawing.Size(234, 25);
		this.txtDepartment.TabIndex = 100;
		this.txtDepartment.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(12, 85);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 15);
		this.label5.TabIndex = 29;
		this.label5.Text = "Department";
		this.txtDesgination.BackColor = System.Drawing.Color.White;
		this.txtDesgination.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDesgination.Location = new System.Drawing.Point(121, 51);
		this.txtDesgination.Name = "txtDesgination";
		this.txtDesgination.ReadOnly = true;
		this.txtDesgination.Size = new System.Drawing.Size(234, 25);
		this.txtDesgination.TabIndex = 100;
		this.txtDesgination.TabStop = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(12, 55);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 15);
		this.label4.TabIndex = 29;
		this.label4.Text = "Desgination";
		this.txtEmployeeNo.BackColor = System.Drawing.Color.White;
		this.txtEmployeeNo.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmployeeNo.Location = new System.Drawing.Point(121, 22);
		this.txtEmployeeNo.Name = "txtEmployeeNo";
		this.txtEmployeeNo.ReadOnly = true;
		this.txtEmployeeNo.Size = new System.Drawing.Size(234, 25);
		this.txtEmployeeNo.TabIndex = 100;
		this.txtEmployeeNo.TabStop = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(12, 28);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(77, 15);
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
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
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
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Location = new System.Drawing.Point(18, 29);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Name = "cmbEmployeeName";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Leave += new System.EventHandler(cmbEmployeeName_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(18, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 19;
		this.label1.Text = "Employee Name";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1100, 28);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1049, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripLabel1, this.toolStripSeparator1, this.btnnew, this.btnupdate, this.btnsave, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1100, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(96, 24);
		this.toolStripLabel1.Text = "Employee Salary";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1100, 562);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmEmployeeSalary";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEmployeeSalary_FormClosing);
		base.Load += new System.EventHandler(EmployeeFamilyInfo_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EmployeeFamilyInfo_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSalaryType).EndInit();
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
