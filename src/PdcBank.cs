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

public class PdcBank : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMood;

	private IContainer components;

	private ToolStripButton btnsave;

	private ToolStripButton btnnew;

	private Panel panel4;

	internal GridEX grdcropyear;

	private Panel panel5;

	private Label label2;

	private TextBox txtBankName;

	private ToolStrip toolStrip1;

	private ToolStripButton btnUpdate;

	private Panel panel2;

	private Panel panel1;

	public PdcBank(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DefineCropYear_Load(object sender, EventArgs e)
	{
		BindBanks();
		btnUpdate.Visible = false;
		txtBankName.Focus();
	}

	private void Inset()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				PdcBankName obj = new global::Architecture.Model.PdcBankName();
				if (RecId > 0)
				{
					obj.Id = RecId;
				}
				obj.BankName = Conversion.ToString((object)txtBankName.Text);
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				int success = global::Architecture.BLL.PdcBankName.Save(obj);
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully...[" + success + "]");
				}
				else
				{
					MessageBox.Show("Record Save Successfully...[" + success + "]");
				}
				BindBanks();
				formReFresh();
			}
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
			Inset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindBanks()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = global::Architecture.BLL.PdcBankName.Getall(new global::Architecture.Model.PdcBankName
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			grdcropyear.DataSource = lst;
			grdcropyear.RetrieveStructure();
			grdcropyear.RootTable.Columns["Id"].Visible = false;
			grdcropyear.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcropyear_DoubleClick(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMood = true;
			PdcBankName crop = new global::Architecture.Model.PdcBankName();
			RecId = Conversion.ToInt((object)grdcropyear.CurrentRow.Cells[0].Value.ToString());
			crop = global::Architecture.BLL.PdcBankName.GetByID(RecId);
			txtBankName.Text = crop.BankName;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtBankName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Bank Name Field Required");
			txtBankName.Focus();
			return false;
		}
		return true;
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Inset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void formReFresh()
	{
		RecId = 0;
		txtBankName.Text = string.Empty;
		txtBankName.Focus();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		formReFresh();
		btnUpdate.Visible = false;
		btnsave.Visible = true;
	}

	private void DefineCropYear_KeyDown(object sender, KeyEventArgs e)
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
		GridEXLayout grdcropyear_DesignTimeLayout = new GridEXLayout();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdcropyear = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.txtBankName = new System.Windows.Forms.TextBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcropyear).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdcropyear);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 79);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(479, 371);
		this.panel4.TabIndex = 3;
		this.grdcropyear.AllowDelete = (InheritableBoolean)1;
		grdcropyear_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdcropyear.DesignTimeLayout = grdcropyear_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdcropyear).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdcropyear.FilterMode = (FilterMode)2;
		this.grdcropyear.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdcropyear).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdcropyear.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdcropyear).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdcropyear).Name = "grdcropyear";
		((ScrollableBase)this.grdcropyear).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdcropyear).Size = new System.Drawing.Size(479, 371);
		((System.Windows.Forms.Control)(object)this.grdcropyear).TabIndex = 2;
		this.grdcropyear.TotalRow = (InheritableBoolean)1;
		this.grdcropyear.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdcropyear).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdcropyear).DoubleClick += new System.EventHandler(grdcropyear_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtBankName);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 27);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(479, 52);
		this.panel5.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(7, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(79, 16);
		this.label2.TabIndex = 1;
		this.label2.Text = "Bank Name";
		this.txtBankName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBankName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBankName.Location = new System.Drawing.Point(93, 13);
		this.txtBankName.Multiline = true;
		this.txtBankName.Name = "txtBankName";
		this.txtBankName.Size = new System.Drawing.Size(241, 22);
		this.txtBankName.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnUpdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(479, 27);
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
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(479, 27);
		this.panel2.TabIndex = 0;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(479, 450);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(479, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PdcBank";
		this.Text = "Bank";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(DefineCropYear_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCropYear_KeyDown);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdcropyear).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
