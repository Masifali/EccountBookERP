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

public class DefineCountry : Form
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

	private Label label1;

	private TextBox txtcountrycode;

	private Panel panel6;

	private Label label23;

	internal GridEX grdcountrydefine;

	private Label label2;

	private TextBox txtcountryName;

	private ToolStripButton toolStripButton1;

	public DefineCountry(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		saveCountry();
	}

	public void saveCountry()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = country.save(new Country
				{
					Code = Conversion.ToString((object)txtcountrycode.Text),
					Description = Conversion.ToString((object)txtcountryName.Text),
					EntryDate = Conversion.ToDateTime((object)DateTime.Now),
					EntryUser = Conversion.ToInt((object)UserAccount.ID),
					ModifyDate = Conversion.ToDateTime((object)DateTime.Now),
					PostDate = Conversion.ToDateTime((object)DateTime.Now),
					PostUser = Conversion.ToInt((object)0),
					PostState = false,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Save Successfully...[" + success + "]");
				}
				CounytryDefineGridFill();
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
		txtcountrycode.Text = string.Empty;
		txtcountryName.Text = string.Empty;
	}

	private void CounytryDefineGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		DataTable lst = country.GetAll(new Country());
		DataTable dt = new DataTable();
		dt.Columns.Add("Id");
		dt.Columns.Add("CountryCode");
		dt.Columns.Add("Description");
		grdcountrydefine.DataSource = dt;
		grdcountrydefine.RetrieveStructure();
		for (int i = 0; i < lst.Rows.Count; i++)
		{
			dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["Code"].ToString(), lst.Rows[i]["Description"]);
		}
		grdcountrydefine.DataSource = dt;
		grdcountrydefine.RootTable.Columns["Id"].Visible = false;
	}

	private bool FormValidation()
	{
		if (txtcountrycode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Country Code Field Required");
			txtcountrycode.Focus();
			return false;
		}
		if (txtcountryName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Country Name Field Required");
			txtcountryName.Focus();
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
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			Country bnk = new Country();
			RecId = Conversion.ToInt((object)grdcountrydefine.CurrentRow.Cells[0].Value.ToString());
			bnk = country.GetById(RecId);
			txtcountrycode.Text = bnk.Code;
			txtcountryName.Text = bnk.Description;
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
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = country.save(new Country
				{
					Id = RecId,
					Code = Conversion.ToString((object)txtcountrycode.Text),
					Description = Conversion.ToString((object)txtcountryName.Text),
					EntryDate = Conversion.ToDateTime((object)DateTime.Now),
					ModifyUser = Conversion.ToInt((object)UserAccount.ID),
					ModifyDate = Conversion.ToDateTime((object)DateTime.Now),
					PostDate = Conversion.ToDateTime((object)DateTime.Now),
					PostUser = Conversion.ToInt((object)0),
					PostState = false,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Update Successfully...[" + success + "]");
				}
				CounytryDefineGridFill();
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
		CounytryDefineGridFill();
		txtcountrycode.Focus();
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
		txtcountrycode.Text = string.Empty;
		txtcountryName.Text = string.Empty;
		btnsave.Visible = true;
		btnUpdate.Visible = false;
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		new DefineCity(UserAccount).Show();
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
		GridEXLayout grdcountrydefine_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdcountrydefine = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtcountryName = new System.Windows.Forms.TextBox();
		this.txtcountrycode = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcountrydefine).BeginInit();
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
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdcountrydefine);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 110);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(605, 340);
		this.panel4.TabIndex = 3;
		this.grdcountrydefine.AllowDelete = (InheritableBoolean)1;
		grdcountrydefine_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdcountrydefine.DesignTimeLayout = grdcountrydefine_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdcountrydefine).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdcountrydefine.FilterMode = (FilterMode)2;
		this.grdcountrydefine.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdcountrydefine).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdcountrydefine.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdcountrydefine).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdcountrydefine).Name = "grdcountrydefine";
		((ScrollableBase)this.grdcountrydefine).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdcountrydefine).Size = new System.Drawing.Size(605, 340);
		((System.Windows.Forms.Control)(object)this.grdcountrydefine).TabIndex = 2;
		this.grdcountrydefine.TotalRow = (InheritableBoolean)1;
		this.grdcountrydefine.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdcountrydefine).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdcountrydefine).DoubleClick += new System.EventHandler(grdcountrydefine_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.txtcountryName);
		this.panel5.Controls.Add(this.txtcountrycode);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(605, 55);
		this.panel5.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(194, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(49, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Country";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(79, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "Country Code";
		this.txtcountryName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcountryName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcountryName.Location = new System.Drawing.Point(194, 26);
		this.txtcountryName.Name = "txtcountryName";
		this.txtcountryName.Size = new System.Drawing.Size(207, 23);
		this.txtcountryName.TabIndex = 1;
		this.txtcountrycode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcountrycode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcountrycode.Location = new System.Drawing.Point(10, 26);
		this.txtcountrycode.Name = "txtcountrycode";
		this.txtcountrycode.Size = new System.Drawing.Size(182, 23);
		this.txtcountrycode.TabIndex = 0;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(605, 28);
		this.panel3.TabIndex = 2;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(605, 31);
		this.panel6.TabIndex = 6;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(131, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Country Define";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(605, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripButton1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(605, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
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
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(78, 24);
		this.toolStripButton1.Text = "DefineCity";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(605, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineCountry";
		this.Text = "DefineCountry";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(DefineCountry_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCountry_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdcountrydefine).EndInit();
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
