using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Lookups
{

public class Define_Department : Form
{
	private UserAccount UserAccount = new UserAccount();

	private bool UpdateMood;

	private int RecId;

	private IContainer components;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnnew;

	private ToolStrip toolStrip1;

	private ToolStripButton btnsave;

	private Label txtProvienceName;

	private Panel panel2;

	private Label label23;

	private TextBox txtDepartmentName;

	private Panel panel3;

	private Panel panel6;

	private Panel panel4;

	internal GridEX grdDefineDepartment;

	private Panel panel5;

	private Panel panel1;

	public Define_Department(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void Define_Department_Load(object sender, EventArgs e)
	{
		DepartmentDefineGridFill();
	}

	public void Reset()
	{
		try
		{
			DepartmentDefineGridFill();
			txtDepartmentName.Text = string.Empty;
			UpdateMood = false;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDepartmentName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Department Name Field Required");
			txtDepartmentName.Focus();
			return false;
		}
		return true;
	}

	private void DepartmentDefineGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		DataTable lst = Department.GetAll(new Department
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId
		});
		DataTable dt = new DataTable();
		dt.Columns.Add("Id");
		dt.Columns.Add("DepartmentName");
		for (int i = 0; i < lst.Rows.Count; i++)
		{
			dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DepartmentName"]);
		}
		grdDefineDepartment.DataSource = dt;
		grdDefineDepartment.RetrieveStructure();
		grdDefineDepartment.RootTable.Columns["Id"].Visible = false;
		grdDefineDepartment.RootTable.Columns["DepartmentName"].Width = 200;
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				MessageBox.Show("Record Update Successfully...[" + Department.Save(new Department
				{
					Id = RecId,
					DepartmentName = Conversion.ToString((object)txtDepartmentName.Text),
					EntryDate = Conversion.ToDateTime((object)DateTime.Now),
					ModifyDate = Conversion.ToDateTime((object)DateTime.Now),
					EntryUser = UserAccount.ID,
					PostDate = Conversion.ToDateTime((object)DateTime.Now),
					PostUser = 0,
					PostState = false,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				}) + "]");
				Reset();
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				MessageBox.Show("Record Save Successfully...[" + Department.Save(new Department
				{
					DepartmentName = Conversion.ToString((object)txtDepartmentName.Text),
					EntryDate = Conversion.ToDateTime((object)DateTime.Now),
					ModifyDate = Conversion.ToDateTime((object)DateTime.Now),
					EntryUser = UserAccount.ID,
					PostDate = Conversion.ToDateTime((object)DateTime.Now),
					PostUser = 0,
					PostState = false,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				}) + "]");
				Reset();
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

	private void Define_Department_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMood)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMood)
			{
				btnUpdate_Click(null, null);
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

	private void grdDefineCity_DoubleClick(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMood = true;
			Department deprt = new Department();
			RecId = Conversion.ToInt((object)grdDefineDepartment.CurrentRow.Cells[0].Value.ToString());
			deprt = Department.GetByID(RecId);
			txtDepartmentName.Text = deprt.DepartmentName;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		GridEXLayout grdDefineDepartment_DesignTimeLayout = new GridEXLayout();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.txtProvienceName = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.txtDepartmentName = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdDefineDepartment = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDefineDepartment).BeginInit();
		this.panel5.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnUpdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(391, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.txtProvienceName.AutoSize = true;
		this.txtProvienceName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProvienceName.Location = new System.Drawing.Point(5, 13);
		this.txtProvienceName.Name = "txtProvienceName";
		this.txtProvienceName.Size = new System.Drawing.Size(106, 15);
		this.txtProvienceName.TabIndex = 1;
		this.txtProvienceName.Text = "Department Name";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(391, 27);
		this.panel2.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(165, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Department Define";
		this.txtDepartmentName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDepartmentName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDepartmentName.Location = new System.Drawing.Point(116, 7);
		this.txtDepartmentName.Multiline = true;
		this.txtDepartmentName.Name = "txtDepartmentName";
		this.txtDepartmentName.Size = new System.Drawing.Size(263, 26);
		this.txtDepartmentName.TabIndex = 3;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(391, 28);
		this.panel3.TabIndex = 2;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(391, 28);
		this.panel6.TabIndex = 6;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdDefineDepartment);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 94);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(391, 356);
		this.panel4.TabIndex = 3;
		this.grdDefineDepartment.AllowEdit = (InheritableBoolean)2;
		grdDefineDepartment_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDefineDepartment.DesignTimeLayout = grdDefineDepartment_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDefineDepartment.FilterMode = (FilterMode)2;
		this.grdDefineDepartment.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDefineDepartment.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Name = "grdDefineDepartment";
		((ScrollableBase)this.grdDefineDepartment).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Size = new System.Drawing.Size(391, 356);
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).TabIndex = 2;
		this.grdDefineDepartment.TotalRow = (InheritableBoolean)1;
		this.grdDefineDepartment.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDefineDepartment).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).DoubleClick += new System.EventHandler(grdDefineCity_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.txtProvienceName);
		this.panel5.Controls.Add(this.txtDepartmentName);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(391, 39);
		this.panel5.TabIndex = 2;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(391, 450);
		this.panel1.TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(391, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "Define_Department";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(Define_Department_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Define_Department_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDefineDepartment).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
