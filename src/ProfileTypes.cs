using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.HRM.PolicyManagment
{

public class ProfileTypes : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	private IContainer components;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnnew;

	private ToolStrip tsItemType;

	private Panel panel1;

	private Panel panel11;

	private TextBox txtprefix;

	private TextBox txtprofilename;

	private Label label49;

	private Label label48;

	private Panel panel2;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label1;

	private Panel panel4;

	private Panel panel3;

	private Label label2;

	private Button btnUpdateF;

	private Button BtnCancel;

	private Button BtnSavef;

	public ProfileTypes(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private bool formvalidation()
	{
		if (txtprofilename.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert ProfileName");
			txtprofilename.Focus();
			return false;
		}
		if (txtprefix.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Prefix");
			txtprefix.Focus();
			return false;
		}
		return true;
	}

	private void clear()
	{
		BtnSavef.Visible = true;
		btnUpdateF.Visible = false;
		btnsave.Visible = true;
		btnupdate.Visible = false;
		UpdateMode = false;
		txtprefix.Clear();
		txtprofilename.Clear();
		txtprofilename.Focus();
		gridfill();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		if (!formvalidation())
		{
			return;
		}
		try
		{
			if (genProfileType.Save(new genProfileType
			{
				Prefix = Conversion.ToString((object)txtprefix.Text.Trim()),
				ProfileTypeName = Conversion.ToString((object)txtprofilename.Text.Trim()),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			}) > 0)
			{
				MessageBox.Show("Save Successfully");
				gridfill();
				clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		if (!formvalidation())
		{
			return;
		}
		try
		{
			if (genProfileType.Save(new genProfileType
			{
				ProfileTypeId = Conversion.ToInt((object)RecId),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ProfileTypeName = Conversion.ToString((object)txtprofilename.Text.Trim()),
				Prefix = Conversion.ToString((object)txtprefix.Text.Trim())
			}) > 0)
			{
				MessageBox.Show("Update Successfully");
				gridfill();
				clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable getList = genProfileType.Getall(new genProfileType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("ProfileType");
			table.Columns.Add("Prefix");
			grdfrm.DataSource = null;
			table.Clear();
			grdfrm.DataSource = table;
			for (int i = 0; i < getList.Rows.Count; i++)
			{
				table.Rows.Add(getList.Rows[i]["ProfileTypeId"], getList.Rows[i]["ProfileTypeName"], getList.Rows[i]["Prefix"]);
			}
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			grdfrm.RootTable.Columns["Id"].Visible = false;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			UpdateMode = true;
			genProfileType cat = new genProfileType();
			RecId = Conversion.ToInt((object)grdfrm.CurrentRow.Cells[0].Value.ToString());
			cat = genProfileType.GetByID(RecId);
			txtprefix.Text = Conversion.ToString((object)cat.Prefix);
			txtprofilename.Text = Conversion.ToString((object)cat.ProfileTypeName);
			gridfill();
			btnsave.Visible = false;
			btnupdate.Visible = true;
			BtnSavef.Visible = false;
			btnUpdateF.Visible = true;
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ProfileTypes_Load(object sender, EventArgs e)
	{
		gridfill();
		BtnSavef.Visible = true;
		btnUpdateF.Visible = false;
		btnsave.Visible = true;
		btnupdate.Visible = false;
		txtprofilename.Focus();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		clear();
	}

	private void ProfileTypes_KeyDown(object sender, KeyEventArgs e)
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

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		Close();
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
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PolicyManagment.ProfileTypes));
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.txtprofilename = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtprefix = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.tsItemType.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(434, 27);
		this.tsItemType.TabIndex = 1;
		this.tsItemType.Text = "toolStrip4";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.btnUpdateF);
		this.panel1.Controls.Add(this.BtnCancel);
		this.panel1.Controls.Add(this.BtnSavef);
		this.panel1.Controls.Add(this.txtprofilename);
		this.panel1.Controls.Add(this.label48);
		this.panel1.Controls.Add(this.txtprefix);
		this.panel1.Controls.Add(this.panel11);
		this.panel1.Controls.Add(this.label49);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(434, 135);
		this.panel1.TabIndex = 0;
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(303, 90);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 2;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.btnUpdateF.Click += new System.EventHandler(btnupdate_Click);
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(224, 90);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 3;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(303, 90);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 2;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.BtnSavef.Click += new System.EventHandler(btnsave_Click);
		this.txtprofilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtprofilename.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtprofilename.Location = new System.Drawing.Point(157, 36);
		this.txtprofilename.Margin = new System.Windows.Forms.Padding(4);
		this.txtprofilename.Name = "txtprofilename";
		this.txtprofilename.Size = new System.Drawing.Size(221, 23);
		this.txtprofilename.TabIndex = 0;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(73, 40);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(76, 15);
		this.label48.TabIndex = 0;
		this.label48.Text = "Profile Name";
		this.txtprefix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtprefix.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtprefix.Location = new System.Drawing.Point(157, 64);
		this.txtprefix.Margin = new System.Windows.Forms.Padding(4);
		this.txtprefix.Name = "txtprefix";
		this.txtprefix.Size = new System.Drawing.Size(221, 23);
		this.txtprefix.TabIndex = 1;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(4);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(434, 28);
		this.panel11.TabIndex = 8;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(13, 5);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(116, 19);
		this.label1.TabIndex = 1;
		this.label1.Text = "Profile Types";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(73, 68);
		this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(37, 15);
		this.label49.TabIndex = 1;
		this.label49.Text = "Prefix";
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 162);
		this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(434, 399);
		this.panel2.TabIndex = 8;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 27);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(434, 372);
		this.panel4.TabIndex = 3;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(434, 372);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(434, 27);
		this.panel3.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(368, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(60, 27);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(4, 3);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 17);
		this.label2.TabIndex = 2;
		this.label2.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(434, 561);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tsItemType);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "ProfileTypes";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Profile Type";
		base.Load += new System.EventHandler(ProfileTypes_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ProfileTypes_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
