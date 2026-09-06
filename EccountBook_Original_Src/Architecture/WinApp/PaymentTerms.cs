using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;

namespace Architecture.WinApp;

public class PaymentTerms : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private MenuStrip menuStrip1;

	private ToolStripMenuItem dateLockToolStripMenuItem;

	private ToolStripMenuItem refreshToolStripMenuItem;

	private ToolStripMenuItem btnSave;

	private Panel panel3;

	private Label lll;

	private Panel panel4;

	internal GridEX grdfrm;

	private Label label1;

	private TextBox txtDescription;

	private TextBox txtCode;

	private Button btnAdd;

	public PaymentTerms(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DateLock_Load(object sender, EventArgs e)
	{
		btnSave.Text = "Save";
		BindGrid();
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImLcPaymentTerm obj = new ExImLcPaymentTerm();
			if (RecId > 0)
			{
				obj.Id = RecId;
			}
			if (txtCode.Text == string.Empty || txtCode.Text == null)
			{
				MessageBox.Show("Code Field is Required");
				return;
			}
			if (txtDescription.Text == string.Empty || txtDescription.Text == null)
			{
				MessageBox.Show("Description Field is Required");
				return;
			}
			obj.LcOrderTerm = txtCode.Text.Trim();
			obj.lcOrderTermDesc = txtDescription.Text.Trim();
			ExImLcPaymentTerm.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
			BindGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtPayment = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtPayment.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Code");
				dt.Columns.Add("lcOrderTermDesc");
				for (int i = 0; i < dtPayment.Rows.Count; i++)
				{
					dt.Rows.Add(dtPayment.Rows[i]["Id"], dtPayment.Rows[i]["LcOrderTerm"], dtPayment.Rows[i]["lcOrderTermDesc"]);
				}
				grdfrm.DataSource = dt;
				grdfrm.RetrieveStructure();
				grdfrm.RootTable.Columns["Id"].Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		RecId = Convert.ToInt32(grdfrm.CurrentRow.Cells[0].Value);
		ExImLcPaymentTerm lst = ExImLcPaymentTerm.GetByID(RecId);
		if (lst != null)
		{
			btnSave.Text = "Update";
			btnAdd.Text = "Update";
			txtCode.Text = lst.LcOrderTerm;
			txtDescription.Text = lst.lcOrderTermDesc;
		}
	}

	private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Reset();
	}

	public void Reset()
	{
		try
		{
			txtCode.Text = string.Empty;
			txtDescription.Text = string.Empty;
			btnSave.Text = "Save";
			btnAdd.Text = "Save";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		btnSave_Click(null, null);
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.lll = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.dateLockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.btnSave = new System.Windows.Forms.ToolStripMenuItem();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.menuStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(620, 369);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 95);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(620, 274);
		this.panel4.TabIndex = 6;
		this.grdfrm.AllowDelete = (InheritableBoolean)1;
		this.grdfrm.ColumnAutoResize = true;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(617, 271);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 77;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.btnAdd);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtDescription);
		this.panel3.Controls.Add(this.txtCode);
		this.panel3.Controls.Add(this.lll);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(620, 67);
		this.panel3.TabIndex = 5;
		this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.Location = new System.Drawing.Point(307, 34);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(59, 23);
		this.btnAdd.TabIndex = 2;
		this.btnAdd.Text = "Save";
		this.btnAdd.UseVisualStyleBackColor = true;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(124, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 9;
		this.label1.Text = "Description";
		this.txtDescription.Location = new System.Drawing.Point(122, 34);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(178, 22);
		this.txtDescription.TabIndex = 1;
		this.txtCode.Location = new System.Drawing.Point(14, 34);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(100, 22);
		this.txtCode.TabIndex = 0;
		this.lll.AutoSize = true;
		this.lll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lll.Location = new System.Drawing.Point(12, 12);
		this.lll.Name = "lll";
		this.lll.Size = new System.Drawing.Size(36, 15);
		this.lll.TabIndex = 6;
		this.lll.Text = "Code";
		this.panel2.Controls.Add(this.menuStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(620, 28);
		this.panel2.TabIndex = 4;
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.dateLockToolStripMenuItem, this.refreshToolStripMenuItem, this.btnSave });
		this.menuStrip1.Location = new System.Drawing.Point(0, 0);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(620, 24);
		this.menuStrip1.TabIndex = 0;
		this.menuStrip1.Text = "menuStrip1";
		this.dateLockToolStripMenuItem.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.dateLockToolStripMenuItem.Name = "dateLockToolStripMenuItem";
		this.dateLockToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
		this.dateLockToolStripMenuItem.Text = "PaymentTerms";
		this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
		this.refreshToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
		this.refreshToolStripMenuItem.Text = "Refresh";
		this.refreshToolStripMenuItem.Click += new System.EventHandler(refreshToolStripMenuItem_Click);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(43, 20);
		this.btnSave.Text = "Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(620, 369);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Name = "PaymentTerms";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(DateLock_Load);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
