using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp
{
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;


using UserAccount = global::Architecture.Model.UserAccount;
using City = global::Architecture.Model.City;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.Common;
using global::Architecture.Model;
// using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

public class DefineExportPrefixType : Form
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

	internal GridEX grd;

	private Label label2;

	private TextBox txtPrefixDescription;

	public DefineExportPrefixType(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
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

	public void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			ExportPrefixType obj = new ExportPrefixType();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.PrefixDescription = txtPrefixDescription.Text;
			ExportPrefixType.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Reset()
	{
		txtPrefixDescription.Text = string.Empty;
	}

	private void GridFill()
	{
		DataTable lst = ExportPrefixType.FormHistory();
		DataTable dt = new DataTable();
		dt.Columns.Add("Id");
		dt.Columns.Add("Description");
		grd.DataSource = dt;
		grd.RetrieveStructure();
		for (int i = 0; i < lst.Rows.Count; i++)
		{
			dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["PrefixDescription"]);
		}
		grd.DataSource = dt;
		grd.RootTable.Columns["Id"].Visible = false;
	}

	private bool FormValidation()
	{
		if (txtPrefixDescription.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Description Field Required");
			txtPrefixDescription.Focus();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				txtPrefixDescription.Text = Conversion.ToString((object)r.Cells["Description"].Text);
				btnsave.Visible = false;
				btnUpdate.Visible = true;
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
			if (RecId == 0)
			{
				throw new Exception("RecId not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefineCountry_Load(object sender, EventArgs e)
	{
		txtPrefixDescription.Focus();
		GridFill();
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
		RecId = 0;
		txtPrefixDescription.Text = string.Empty;
		btnsave.Visible = true;
		btnUpdate.Visible = false;
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		new DefineCity(UserAccount).Show();
	}

	private void panel6_Paint(object sender, PaintEventArgs e)
	{
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
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.txtPrefixDescription = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
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
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 100);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(605, 350);
		this.panel4.TabIndex = 3;
		this.grd.AllowDelete = (InheritableBoolean)1;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(605, 350);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grdcountrydefine_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtPrefixDescription);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 56);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(605, 44);
		this.panel5.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Description";
		this.txtPrefixDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPrefixDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPrefixDescription.Location = new System.Drawing.Point(84, 11);
		this.txtPrefixDescription.Name = "txtPrefixDescription";
		this.txtPrefixDescription.Size = new System.Drawing.Size(261, 23);
		this.txtPrefixDescription.TabIndex = 1;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(605, 29);
		this.panel3.TabIndex = 2;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(605, 31);
		this.panel6.TabIndex = 6;
		this.panel6.Paint += new System.Windows.Forms.PaintEventHandler(panel6_Paint);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(103, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Prefix Type";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(605, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnsave, this.btnUpdate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(605, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(605, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineExportPrefixType";
		this.Text = "DefineCountry";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(DefineCountry_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCountry_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
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

}
