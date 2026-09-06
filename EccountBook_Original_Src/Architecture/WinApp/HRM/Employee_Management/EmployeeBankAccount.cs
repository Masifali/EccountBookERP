using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.HRM.Employee_Management;

public class EmployeeBankAccount : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private int Success;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	internal GridEX grdEmployeeBankAcount;

	private Panel panel3;

	private TextBox txtAccountTitle;

	private Label label2;

	private Label label40;

	internal UltraCombo cmbEmployeeName;

	internal UltraCombo cmbBank;

	private Label label42;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripLabel toolStripLabel1;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private TextBox txtAccountNo;

	private Label label1;

	private CheckBox checkBoxPayRoll;

	public EmployeeBankAccount(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void EmployeeBankAccount_Load(object sender, EventArgs e)
	{
		EmployeeName();
		BankName();
		GridBind();
	}

	public bool Validation()
	{
		if (((Control)(object)cmbEmployeeName).Text == string.Empty)
		{
			MessageBox.Show("Employee Name Required!");
			cmbEmployeeName.Focus();
			return false;
		}
		if (((Control)(object)cmbBank).Text == string.Empty)
		{
			MessageBox.Show("Bank Required!");
			cmbBank.Focus();
			return false;
		}
		if (txtAccountNo.Text == string.Empty)
		{
			MessageBox.Show("AccountNo Required!");
			txtAccountNo.Focus();
			return false;
		}
		if (txtAccountTitle.Text == string.Empty)
		{
			MessageBox.Show("Account Title Required!");
			txtAccountTitle.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		((Control)(object)cmbEmployeeName).Text = "";
		((Control)(object)cmbBank).Text = "";
		txtAccountNo.Text = "";
		checkBoxPayRoll.Checked = false;
		txtAccountTitle.Clear();
		cmbEmployeeName.Focus();
		UpdateMode = false;
		btnsave.Visible = true;
		btnupdate.Visible = false;
		GridBind();
	}

	public void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = hrmEmployeeBankAccount.Getall(new hrmEmployeeBankAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("EmployeeBankAccountId");
			dt.Columns.Add("Employee");
			dt.Columns.Add("Bank");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("AccountNo");
			dt.Columns.Add("PayRoll");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["EmployeeBankAccountId"], lst.Rows[i]["EmployeeName"], lst.Rows[i]["BankName"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["AccountNo"], lst.Rows[i]["IsPayrollAccount"]);
			}
			grdEmployeeBankAcount.DataSource = dt;
			grdEmployeeBankAcount.RetrieveStructure();
			grdEmployeeBankAcountSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdEmployeeBankAcountSetting()
	{
		try
		{
			grdEmployeeBankAcount.RootTable.Columns["Employee"].EditType = (EditType)0;
			grdEmployeeBankAcount.RootTable.Columns["Bank"].EditType = (EditType)0;
			grdEmployeeBankAcount.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdEmployeeBankAcount.RootTable.Columns["AccountNo"].EditType = (EditType)0;
			grdEmployeeBankAcount.RootTable.Columns["PayRoll"].EditType = (EditType)0;
			grdEmployeeBankAcount.RootTable.Columns["Employee"].FilterEditType = (FilterEditType)1;
			grdEmployeeBankAcount.RootTable.Columns["Bank"].FilterEditType = (FilterEditType)1;
			grdEmployeeBankAcount.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdEmployeeBankAcount.RootTable.Columns["AccountNo"].FilterEditType = (FilterEditType)1;
			grdEmployeeBankAcount.RootTable.Columns["PayRoll"].FilterEditType = (FilterEditType)1;
			grdEmployeeBankAcount.RootTable.Columns["EmployeeBankAccountId"].Visible = false;
			grdEmployeeBankAcount.RootTable.Columns["Employee"].Width = 250;
			grdEmployeeBankAcount.RootTable.Columns["Bank"].Width = 150;
			grdEmployeeBankAcount.RootTable.Columns["AccountTitle"].Width = 100;
			grdEmployeeBankAcount.RootTable.Columns["AccountNo"].Width = 100;
			grdEmployeeBankAcount.RootTable.Columns["PayRoll"].Width = 50;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void RetrivedData(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			hrmEmployeeBankAccount refr = new hrmEmployeeBankAccount();
			RecId = Id;
			refr = hrmEmployeeBankAccount.GetByID(RecId);
			if (refr != null)
			{
				cmbEmployeeName.Value = Conversion.ToInt((object)refr.EmployeeId);
				cmbBank.Value = Conversion.ToInt((object)refr.BankAccountId);
				txtAccountTitle.Text = Conversion.ToString((object)refr.AccountTitle);
				txtAccountNo.Text = Conversion.ToString((object)refr.AccountNo);
				checkBoxPayRoll.Checked = Conversion.ToBool((object)refr.IsPayrollAccount);
				UpdateMode = true;
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

	private void EmployeeName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genEmployee.Getall(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EmployeeName");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["EmployeeId"], lst.Rows[i]["FistName"]?.ToString() + " " + lst.Rows[i]["MiddleName"]?.ToString() + " " + lst.Rows[i]["LastName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbEmployeeName, "Id", "EmployeeName", "Employee Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = Bank.GetAllDt(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbBank, "Id", "BranchName", "Bank Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeBankAccount_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
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
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
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

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (Validation())
			{
				hrmEmployeeBankAccount refr = new hrmEmployeeBankAccount();
				refr.EmployeeBankAccountLineId = 0;
				refr.EmployeeBankAccountId = Conversion.ToInt((object)RecId);
				refr.EmployeeId = Conversion.ToInt(cmbEmployeeName.Value);
				refr.BankAccountId = Conversion.ToInt(cmbBank.Value);
				refr.AccountTitle = Conversion.ToString((object)txtAccountTitle.Text);
				refr.AccountNo = Conversion.ToString((object)txtAccountNo.Text);
				refr.IsPayrollAccount = Conversion.ToBool((object)checkBoxPayRoll.Checked);
				refr.OrganizationId = UserAccount.OrganizationId;
				refr.CompanyId = UserAccount.CompanyId;
				refr.CreatedById = UserAccount.ID;
				refr.AlteredById = UserAccount.ID;
				refr.ActionTypeId = UserAccount.ID;
				Success = hrmEmployeeBankAccount.Save(refr);
				if (Success > 0)
				{
					MessageBox.Show("Update Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (Validation())
			{
				hrmEmployeeBankAccount refr = new hrmEmployeeBankAccount();
				refr.EmployeeBankAccountLineId = 0;
				refr.EmployeeId = Conversion.ToInt(cmbEmployeeName.Value);
				refr.BankAccountId = Conversion.ToInt(cmbBank.Value);
				refr.AccountTitle = Conversion.ToString((object)txtAccountTitle.Text);
				refr.AccountNo = Conversion.ToString((object)txtAccountNo.Text);
				refr.IsPayrollAccount = Conversion.ToBool((object)checkBoxPayRoll.Checked);
				refr.OrganizationId = UserAccount.OrganizationId;
				refr.CompanyId = UserAccount.CompanyId;
				refr.CreatedById = UserAccount.ID;
				refr.AlteredById = UserAccount.ID;
				refr.ActionTypeId = UserAccount.ID;
				Success = hrmEmployeeBankAccount.Save(refr);
				if (Success > 0)
				{
					MessageBox.Show("Save Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeBankAcount_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RetrivedData(Conversion.ToInt((object)grdEmployeeBankAcount.CurrentRow.Cells[0].Value.ToString()));
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
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
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
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.Employee_Management.EmployeeBankAccount));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdEmployeeBankAcount = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.checkBoxPayRoll = new System.Windows.Forms.CheckBox();
		this.txtAccountNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAccountTitle = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.cmbEmployeeName = new UltraCombo();
		this.cmbBank = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeBankAcount).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBank).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1137, 534);
		this.panel1.TabIndex = 4;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeBankAcount);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 76);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1137, 458);
		this.panel4.TabIndex = 2;
		this.grdEmployeeBankAcount.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdEmployeeBankAcount).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeBankAcount.FilterMode = (FilterMode)2;
		this.grdEmployeeBankAcount.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeBankAcount).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeBankAcount.GroupByBoxVisible = false;
		this.grdEmployeeBankAcount.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeBankAcount).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployeeBankAcount).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeBankAcount).Name = "grdEmployeeBankAcount";
		((ScrollableBase)this.grdEmployeeBankAcount).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeBankAcount).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeBankAcount).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeBankAcount).Size = new System.Drawing.Size(1137, 458);
		((System.Windows.Forms.Control)(object)this.grdEmployeeBankAcount).TabIndex = 4;
		this.grdEmployeeBankAcount.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeBankAcount.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeBankAcount).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdEmployeeBankAcount).DoubleClick += new System.EventHandler(grdEmployeeBankAcount_DoubleClick);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.checkBoxPayRoll);
		this.panel3.Controls.Add(this.txtAccountNo);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtAccountTitle);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.label40);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeName);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBank);
		this.panel3.Controls.Add(this.label42);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1137, 48);
		this.panel3.TabIndex = 1;
		this.checkBoxPayRoll.AutoSize = true;
		this.checkBoxPayRoll.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.checkBoxPayRoll.Location = new System.Drawing.Point(1035, 13);
		this.checkBoxPayRoll.Name = "checkBoxPayRoll";
		this.checkBoxPayRoll.Size = new System.Drawing.Size(79, 19);
		this.checkBoxPayRoll.TabIndex = 105;
		this.checkBoxPayRoll.Text = "Is Payroll";
		this.checkBoxPayRoll.UseVisualStyleBackColor = true;
		this.txtAccountNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccountNo.Location = new System.Drawing.Point(903, 11);
		this.txtAccountNo.Name = "txtAccountNo";
		this.txtAccountNo.Size = new System.Drawing.Size(125, 23);
		this.txtAccountNo.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(828, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 104;
		this.label1.Text = "Account No";
		this.txtAccountTitle.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccountTitle.Location = new System.Drawing.Point(697, 11);
		this.txtAccountTitle.Name = "txtAccountTitle";
		this.txtAccountTitle.Size = new System.Drawing.Size(125, 23);
		this.txtAccountTitle.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(613, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(78, 15);
		this.label2.TabIndex = 102;
		this.label2.Text = "Account Title";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(6, 15);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(93, 15);
		this.label40.TabIndex = 100;
		this.label40.Text = "Employee Name";
		this.cmbEmployeeName.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Location = new System.Drawing.Point(105, 9);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Name = "cmbEmployeeName";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).TabIndex = 0;
		this.cmbBank.AllowNull = (DefaultableBoolean)1;
		this.cmbBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBank).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBank).Location = new System.Drawing.Point(379, 9);
		((System.Windows.Forms.Control)(object)this.cmbBank).Name = "cmbBank";
		((System.Windows.Forms.Control)(object)this.cmbBank).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbBank).TabIndex = 1;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(339, 15);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(34, 15);
		this.label42.TabIndex = 98;
		this.label42.Text = "Bank";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1137, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripLabel1, this.toolStripSeparator1, this.btnnew, this.btnupdate, this.btnsave, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1137, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(137, 24);
		this.toolStripLabel1.Text = "Employee Bank Account";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(77, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(58, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1137, 534);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EmployeeBankAccount";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(EmployeeBankAccount_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EmployeeBankAccount_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeBankAcount).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBank).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
