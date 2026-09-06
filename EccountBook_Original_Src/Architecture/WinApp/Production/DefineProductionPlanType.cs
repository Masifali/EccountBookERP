using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Production;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class DefineProductionPlanType : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnsave;

	private Panel panel5;

	private Panel panel6;

	private Label label23;

	internal GridEX grddefine;

	private Label label2;

	private TextBox txtDescription;

	private CtrlGrdBar ctrlGrdBar1;

	public DefineProductionPlanType(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		save();
	}

	public void save()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = ProductionPlanType.Save(new ProductionPlanType
				{
					PlanTypeCode = Conversion.ToString((object)txtDescription.Text.Trim()),
					PlanTypeDescription = Conversion.ToString((object)txtDescription.Text.Trim())
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Save Successfully...[" + success + "]");
				}
				DefineGridFill();
				countryformReFresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void countryformReFresh()
	{
		txtDescription.Text = string.Empty;
	}

	private void DefineGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		DataTable lst = ProductionPlanType.GetAll(new ProductionPlanType());
		DataTable dt = new DataTable();
		dt.Columns.Add("Id");
		dt.Columns.Add("Description");
		grddefine.DataSource = dt;
		grddefine.RetrieveStructure();
		for (int i = 0; i < lst.Rows.Count; i++)
		{
			dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["PlanTypeDescription"]);
		}
		grddefine.DataSource = dt;
		grddefine.RootTable.Columns["Id"].Visible = false;
	}

	private bool FormValidation()
	{
		if (txtDescription.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Description Field Required");
			txtDescription.Focus();
			return false;
		}
		return true;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void grdcountrydefine_DoubleClick(object sender, EventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			RecId = Conversion.ToInt((object)grddefine.CurrentRow.Cells[0].Value.ToString());
			DataTable obj = ProductionPlanType.GetAll(new ProductionPlanType
			{
				Id = RecId
			});
			txtDescription.Text = Conversion.ToString(obj.Rows[0]["PlanTypeDescription"]);
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
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = ProductionPlanType.Save(new ProductionPlanType
				{
					Id = RecId,
					PlanTypeCode = Conversion.ToString((object)txtDescription.Text.Trim()),
					PlanTypeDescription = Conversion.ToString((object)txtDescription.Text.Trim())
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Update Successfully...[" + success + "]");
				}
				DefineGridFill();
				countryformReFresh();
				btnUpdate.Visible = false;
				btnnew.Visible = true;
				btnsave.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefineCountry_Load(object sender, EventArgs e)
	{
		DefineGridFill();
		txtDescription.Focus();
	}

	private void DefineCountry_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		txtDescription.Text = string.Empty;
		btnsave.Visible = true;
		btnUpdate.Visible = false;
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grddefine).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grddefine).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grddefine.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DefineProductionPlanType_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		GridEXLayout grddefine_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grddefine = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddefine).BeginInit();
		this.panel5.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(605, 450);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grddefine);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 128);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(605, 322);
		this.panel4.TabIndex = 3;
		this.grddefine.AllowDelete = (InheritableBoolean)1;
		grddefine_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddefine.DesignTimeLayout = grddefine_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddefine).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddefine.FilterMode = (FilterMode)2;
		this.grddefine.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddefine).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddefine.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddefine).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grddefine).Name = "grddefine";
		((ScrollableBase)this.grddefine).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddefine).Size = new System.Drawing.Size(605, 322);
		((System.Windows.Forms.Control)(object)this.grddefine).TabIndex = 2;
		this.grddefine.TotalRow = (InheritableBoolean)1;
		this.grddefine.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddefine).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grddefine).DoubleClick += new System.EventHandler(grdcountrydefine_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtDescription);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 73);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(605, 55);
		this.panel5.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(70, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Description";
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(13, 24);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(207, 23);
		this.txtDescription.TabIndex = 1;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(605, 46);
		this.panel3.TabIndex = 2;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(605, 48);
		this.panel6.TabIndex = 6;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 12);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(154, 24);
		this.label23.TabIndex = 25;
		this.label23.Text = "Define Plan Type";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(605, 27);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(558, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grddefine;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 11;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnsave, this.btnUpdate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(605, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(605, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineProductionPlanType";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DefineProductionPlanType_FormClosing);
		base.Load += new System.EventHandler(DefineCountry_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCountry_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddefine).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
