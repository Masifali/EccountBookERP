using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

namespace Architecture.WinApp;

public class DefineExportPackingType : Form
{
	private int RecId;

	private UserAccount UserAccount = new UserAccount();

	private bool UpdateMod;

	private IContainer components;

	private Panel panel4;

	internal GridEX grdPackingType;

	private Panel panel5;

	private Label label2;

	private TextBox txtpacktypecode;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnsave;

	private Label label23;

	private Panel panel6;

	private Panel panel3;

	private Panel panel2;

	private Panel panel1;

	private Label label1;

	private TextBox txtpacktypeDesc;

	public DefineExportPackingType(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DefinePackingType_Load(object sender, EventArgs e)
	{
		btnUpdate.Visible = false;
		BindGridPackingType();
		txtpacktypecode.Focus();
	}

	private bool FormValidation()
	{
		if (txtpacktypecode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Pack Type Code Field Required");
			txtpacktypecode.Focus();
			return false;
		}
		if (txtpacktypeDesc.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Pack Type Desc Field Required");
			txtpacktypeDesc.Focus();
			return false;
		}
		return true;
	}

	public void formReFresh()
	{
		txtpacktypecode.Text = string.Empty;
		txtpacktypeDesc.Text = string.Empty;
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		UpdateMod = false;
		txtpacktypecode.Focus();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				InvPackingTypeExport val = new InvPackingTypeExport
				{
					Code = Conversion.ToString((object)txtpacktypecode.Text),
					Description = Conversion.ToString((object)txtpacktypeDesc.Text),
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EntryDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ModifyDate = DateTime.Now,
					ModifyUser = UserAccount.ID,
					PostDate = DateTime.Now,
					PostUser = UserAccount.ID,
					PostState = true,
					JobLotId = 6
				};
				DataTable dt = CommonServices.ProjectServiceBind();
				val.ProjectId = Conversion.ToInt(dt.Rows[0]["Id"]);
				int success = InvExportPackingType.Save(val);
				if (success >= 1)
				{
					MessageBox.Show("Record Save Successfully...[" + success + "]");
				}
				BindGridPackingType();
				formReFresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				InvPackingTypeExport val = new InvPackingTypeExport
				{
					Id = RecId,
					Code = Conversion.ToString((object)txtpacktypecode.Text),
					Description = Conversion.ToString((object)txtpacktypeDesc.Text),
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EntryDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ModifyDate = DateTime.Now,
					ModifyUser = UserAccount.ID,
					PostDate = DateTime.Now,
					PostUser = UserAccount.ID,
					PostState = true,
					JobLotId = 6
				};
				DataTable dt = CommonServices.ProjectServiceBind();
				val.ProjectId = Conversion.ToInt(dt.Rows[0]["Id"]);
				if (InvExportPackingType.Save(val) == 0)
				{
					MessageBox.Show("Record Update Successfully...[" + RecId + "]");
				}
				btnsave.Visible = true;
				btnUpdate.Visible = false;
				BindGridPackingType();
				formReFresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		txtpacktypecode.Text = string.Empty;
		txtpacktypeDesc.Text = string.Empty;
		btnsave.Visible = true;
		btnUpdate.Visible = false;
	}

	private void grdPackingType_DoubleClick(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMod = true;
			InvPackingTypeExport pack = new InvPackingTypeExport();
			RecId = Conversion.ToInt((object)grdPackingType.CurrentRow.Cells[0].Value.ToString());
			pack = InvExportPackingType.GetByID(RecId);
			txtpacktypecode.Text = pack.Code;
			txtpacktypeDesc.Text = pack.Description;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridPackingType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		DataTable lst = InvExportPackingType.Getall(new InvPackingTypeExport
		{
			CompanyId = UserAccount.CompanyId,
			OrganizationId = UserAccount.OrganizationId
		});
		DataTable dt = new DataTable();
		dt.Columns.Add("Id");
		dt.Columns.Add("Code");
		dt.Columns.Add("Description");
		grdPackingType.DataSource = dt;
		grdPackingType.RetrieveStructure();
		for (int i = 0; i < lst.Rows.Count; i++)
		{
			dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["Code"].ToString(), lst.Rows[i]["Description"].ToString());
		}
		grdPackingType.DataSource = dt;
		grdPackingType.RootTable.Columns["Id"].Visible = false;
		grdSetting();
	}

	private void grdSetting()
	{
		grdPackingType.RootTable.Columns["Code"].Width = 150;
		grdPackingType.RootTable.Columns["Description"].Width = 150;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void DefinePackingType_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.S && !UpdateMod)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.U && UpdateMod)
		{
			btnUpdate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		GridEXLayout grdPackingType_DesignTimeLayout = new GridEXLayout();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdPackingType = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtpacktypeDesc = new System.Windows.Forms.TextBox();
		this.txtpacktypecode = new System.Windows.Forms.TextBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.label23 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingType).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingType);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 134);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(624, 316);
		this.panel4.TabIndex = 3;
		this.grdPackingType.AllowDelete = (InheritableBoolean)1;
		this.grdPackingType.AllowEdit = (InheritableBoolean)2;
		grdPackingType_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingType.DesignTimeLayout = grdPackingType_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingType).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingType.FilterMode = (FilterMode)2;
		this.grdPackingType.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackingType).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingType.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingType).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPackingType).Name = "grdPackingType";
		((ScrollableBase)this.grdPackingType).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingType).Size = new System.Drawing.Size(624, 316);
		((System.Windows.Forms.Control)(object)this.grdPackingType).TabIndex = 2;
		this.grdPackingType.TotalRow = (InheritableBoolean)1;
		this.grdPackingType.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingType).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdPackingType).DoubleClick += new System.EventHandler(grdPackingType_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtpacktypeDesc);
		this.panel5.Controls.Add(this.txtpacktypecode);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 67);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(624, 67);
		this.panel5.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(242, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(47, 16);
		this.label1.TabIndex = 1;
		this.label1.Text = "             ";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(12, 11);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(110, 16);
		this.label2.TabIndex = 1;
		this.label2.Text = "Pack Type Code";
		this.txtpacktypeDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpacktypeDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpacktypeDesc.Location = new System.Drawing.Point(245, 36);
		this.txtpacktypeDesc.Multiline = true;
		this.txtpacktypeDesc.Name = "txtpacktypeDesc";
		this.txtpacktypeDesc.Size = new System.Drawing.Size(367, 22);
		this.txtpacktypeDesc.TabIndex = 1;
		this.txtpacktypecode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpacktypecode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpacktypecode.Location = new System.Drawing.Point(15, 36);
		this.txtpacktypecode.Multiline = true;
		this.txtpacktypecode.Name = "txtpacktypecode";
		this.txtpacktypecode.Size = new System.Drawing.Size(224, 22);
		this.txtpacktypecode.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnUpdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(624, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(17, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(190, 24);
		this.label23.TabIndex = 25;
		this.label23.Text = "Export Packing Type ";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(624, 40);
		this.panel6.TabIndex = 6;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(624, 40);
		this.panel3.TabIndex = 2;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(624, 27);
		this.panel2.TabIndex = 0;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(624, 450);
		this.panel1.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(624, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineExportPackingType";
		this.Text = "DefinePackingType";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(DefinePackingType_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefinePackingType_KeyDown);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingType).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
