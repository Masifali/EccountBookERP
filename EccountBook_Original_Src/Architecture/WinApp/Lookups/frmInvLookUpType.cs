using System;
using System.Collections.Generic;
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

namespace Architecture.WinApp.Lookups;

public class frmInvLookUpType : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton toolStripButton6;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private Label label2;

	private TextBox txtprefix;

	private Label label1;

	private TextBox txtDescription;

	internal GridEX grdlookuptype;

	private Label label4;

	private TextBox txtSerialTo;

	private Label label3;

	private TextBox txtSerialFrom;

	public frmInvLookUpType(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmEduLookUpType_Load(object sender, EventArgs e)
	{
		GridBind();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				InvLookupType lookupType = new InvLookupType();
				if (RecId > 0 && UpdateMode)
				{
					lookupType.Id = RecId;
				}
				lookupType.OrganizationId = UserAccount.OrganizationId;
				lookupType.CompanyId = UserAccount.CompanyId;
				lookupType.Prefix = txtprefix.Text;
				lookupType.SerialFrom = Conversion.ToInt((object)txtSerialFrom.Text);
				lookupType.SerialTo = Conversion.ToInt((object)txtSerialTo.Text);
				lookupType.ProfileTypeName = txtDescription.Text;
				lookupType.EntryDate = DateTime.Now;
				lookupType.EntryUser = UserAccount.ID;
				lookupType.ModifyDate = DateTime.Now;
				lookupType.ModifyUser = UserAccount.ID;
				lookupType.PostDate = DateTime.Now;
				lookupType.PostUser = UserAccount.ID;
				lookupType.PostState = false;
				int success = InvLookupType.Save(lookupType);
				if (RecId == 0 && !UpdateMode && success > 0)
				{
					MessageBox.Show("Save Successfully");
					FormReset();
				}
				if (RecId > 0 && UpdateMode && success > 0)
				{
					MessageBox.Show("Update Successfully");
					FormReset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnsave_Click(null, null);
	}

	public void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			InvLookupType val = new InvLookupType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			List<InvLookupType> lst = new List<InvLookupType>();
			lst = InvLookupType.GetAll(val);
			if (lst.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ProfileName");
				dt.Columns.Add("Prefix");
				dt.Columns.Add("SFrom");
				dt.Columns.Add("STo");
				for (int i = 0; i < lst.Count; i++)
				{
					dt.Rows.Add(lst[i].Id, lst[i].ProfileTypeName, lst[i].Prefix, lst[i].SerialFrom, lst[i].SerialTo);
				}
				grdlookuptype.DataSource = dt;
				grdlookuptype.RetrieveStructure();
				GridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridSetting()
	{
		grdlookuptype.RootTable.Columns["Id"].Visible = false;
		grdlookuptype.RootTable.Columns["ProfileName"].Width = 100;
		grdlookuptype.RootTable.Columns["Prefix"].Width = 80;
		grdlookuptype.RootTable.Columns["SFrom"].Width = 60;
		grdlookuptype.RootTable.Columns["STo"].Width = 60;
	}

	private bool FormValidation()
	{
		if (txtDescription.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Profile Name Field Required");
			txtDescription.Focus();
			return false;
		}
		if (txtprefix.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Prefix Field Required");
			txtprefix.Focus();
			return false;
		}
		return true;
	}

	public void FormReset()
	{
		txtDescription.Text = string.Empty;
		txtprefix.Text = string.Empty;
		txtSerialFrom.Text = string.Empty;
		txtSerialTo.Text = string.Empty;
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		UpdateMode = false;
		RecId = 0;
		GridBind();
	}

	private void grdlookuptype_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvLookupType eduLookups = new InvLookupType();
			RecId = Conversion.ToInt((object)grdlookuptype.CurrentRow.Cells[0].Value.ToString());
			eduLookups = InvLookupType.GetByID(RecId);
			if (eduLookups != null)
			{
				txtDescription.Text = eduLookups.ProfileTypeName;
				txtprefix.Text = eduLookups.Prefix;
				txtSerialFrom.Text = eduLookups.SerialFrom.ToString();
				txtSerialTo.Text = eduLookups.SerialTo.ToString();
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				UpdateMode = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FormReset();
	}

	private void frmEduLookUpType_KeyDown(object sender, KeyEventArgs e)
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
				btnUpdate_Click(null, null);
			}
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		GridEXLayout grdlookuptype_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lookups.frmInvLookUpType));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdlookuptype = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.txtSerialTo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtSerialFrom = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtprefix = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlookuptype).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(578, 454);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(578, 454);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(570, 428);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(564, 422);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdlookuptype);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 92);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(564, 330);
		this.panel5.TabIndex = 2;
		this.grdlookuptype.AllowDelete = (InheritableBoolean)1;
		grdlookuptype_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdlookuptype.DesignTimeLayout = grdlookuptype_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdlookuptype).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdlookuptype.FilterMode = (FilterMode)2;
		this.grdlookuptype.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdlookuptype).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdlookuptype.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdlookuptype).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdlookuptype).Name = "grdlookuptype";
		((ScrollableBase)this.grdlookuptype).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdlookuptype).Size = new System.Drawing.Size(564, 330);
		((System.Windows.Forms.Control)(object)this.grdlookuptype).TabIndex = 4;
		this.grdlookuptype.TotalRow = (InheritableBoolean)1;
		this.grdlookuptype.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdlookuptype).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdlookuptype).DoubleClick += new System.EventHandler(grdlookuptype_DoubleClick);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.txtSerialTo);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtSerialFrom);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtprefix);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtDescription);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(564, 63);
		this.panel4.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(431, 6);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 16);
		this.label4.TabIndex = 13;
		this.label4.Text = "Serial To";
		this.txtSerialTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSerialTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSerialTo.Location = new System.Drawing.Point(434, 26);
		this.txtSerialTo.Name = "txtSerialTo";
		this.txtSerialTo.Size = new System.Drawing.Size(103, 22);
		this.txtSerialTo.TabIndex = 12;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(322, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(77, 16);
		this.label3.TabIndex = 11;
		this.label3.Text = "Serial From";
		this.txtSerialFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSerialFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSerialFrom.Location = new System.Drawing.Point(325, 26);
		this.txtSerialFrom.Name = "txtSerialFrom";
		this.txtSerialFrom.Size = new System.Drawing.Size(103, 22);
		this.txtSerialFrom.TabIndex = 10;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(213, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(41, 16);
		this.label2.TabIndex = 9;
		this.label2.Text = "Prefix";
		this.txtprefix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtprefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtprefix.Location = new System.Drawing.Point(216, 26);
		this.txtprefix.Name = "txtprefix";
		this.txtprefix.Size = new System.Drawing.Size(103, 22);
		this.txtprefix.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 16);
		this.label1.TabIndex = 8;
		this.label1.Text = "Description";
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(10, 26);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(200, 22);
		this.txtDescription.TabIndex = 0;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(564, 29);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripButton6, this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(564, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton6.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(117, 24);
		this.toolStripButton6.Text = "Lookup Type    ";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
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
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(578, 454);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmInvLookUpType";
		this.Text = "LookUp Type";
		base.Load += new System.EventHandler(frmEduLookUpType_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmEduLookUpType_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdlookuptype).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
