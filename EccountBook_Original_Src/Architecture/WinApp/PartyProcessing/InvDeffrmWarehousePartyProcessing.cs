using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.PartyProcessing;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.PartyProcessing;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PartyProcessing;

public class InvDeffrmWarehousePartyProcessing : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMood;

	private IContainer components;

	private ToolStrip tsItemTaxSchedule;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel7;

	private Label label43;

	private Panel panel4;

	private TextBox txtDescription;

	private TextBox txtcode;

	private Label label49;

	private Label label48;

	private Panel panel1;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	public InvDeffrmWarehousePartyProcessing(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private bool formvalidation()
	{
		if (txtcode.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Warehouse Code");
			txtcode.Focus();
			return false;
		}
		if (txtDescription.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Warehouse Name");
			txtDescription.Focus();
			return false;
		}
		return true;
	}

	private void clear()
	{
		btnsave.Visible = true;
		txtcode.Clear();
		txtDescription.Clear();
		btnupdate.Visible = false;
		UpdateMood = false;
		txtcode.Focus();
	}

	private void gridfill()
	{
		DataTable getList = CommonServices.WareHousePartyProcessingGetAllService();
		DataTable table = new DataTable();
		table.Columns.Add("Id");
		table.Columns.Add("Code");
		table.Columns.Add("Description");
		grdfrm.DataSource = null;
		table.Clear();
		grdfrm.DataSource = table;
		for (int i = 0; i < getList.Rows.Count; i++)
		{
			table.Rows.Add(getList.Rows[i]["Id"], getList.Rows[i]["WareHouseCode"], getList.Rows[i]["WareHouseName"]);
		}
		grdfrm.DataSource = table;
		grdfrm.RetrieveStructure();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		if (!formvalidation())
		{
			return;
		}
		try
		{
			InvWareHousePartyProcessing.Save(new InvWareHousePartyProcessing
			{
				WareHouseCode = Conversion.ToString((object)txtcode.Text.Trim()),
				WareHouseName = Conversion.ToString((object)txtDescription.Text.Trim()),
				EntryDate = DateTime.Now,
				EntryUser = UserAccount.ID,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			MessageBox.Show("Save Successfully");
			gridfill();
			clear();
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
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		if (!formvalidation())
		{
			return;
		}
		try
		{
			InvWareHousePartyProcessing.Save(new InvWareHousePartyProcessing
			{
				Id = Conversion.ToInt((object)RecId),
				ModifyDate = DateTime.Now,
				ModifyUser = UserAccount.ID,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WareHouseCode = Conversion.ToString((object)txtcode.Text.Trim()),
				WareHouseName = Conversion.ToString((object)txtDescription.Text.Trim())
			});
			MessageBox.Show("Update Successfully");
			gridfill();
			clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave.Visible = true;
			clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			InvWareHousePartyProcessing cat = new InvWareHousePartyProcessing();
			RecId = Conversion.ToInt((object)grdfrm.CurrentRow.Cells[0].Value.ToString());
			cat = InvWareHousePartyProcessing.GetByID(RecId);
			txtcode.Text = Conversion.ToString((object)cat.WareHouseCode);
			txtDescription.Text = Conversion.ToString((object)cat.WareHouseName);
			gridfill();
			btnupdate.Visible = true;
			UpdateMood = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvDeffrmWarehouse_Load(object sender, EventArgs e)
	{
		try
		{
			btnsave.Visible = true;
			gridfill();
			btnupdate.Visible = false;
			txtcode.Focus();
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdfrm).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdfrm).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void InvDeffrmWarehouse_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.S && !UpdateMood)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMood)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
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

	private void InvDeffrmWarehousePartyProcessing_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.InvDeffrmWarehousePartyProcessing));
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.txtcode = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tsItemTaxSchedule.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		base.SuspendLayout();
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(859, 27);
		this.tsItemTaxSchedule.TabIndex = 80;
		this.tsItemTaxSchedule.Text = "toolStrip3";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(69, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.label43);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 27);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(859, 42);
		this.panel7.TabIndex = 81;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(4, 8);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(183, 24);
		this.label43.TabIndex = 0;
		this.label43.Text = "Define Warehouse";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.txtDescription);
		this.panel4.Controls.Add(this.txtcode);
		this.panel4.Controls.Add(this.label49);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 69);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(859, 64);
		this.panel4.TabIndex = 80;
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(410, 16);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(220, 25);
		this.txtDescription.TabIndex = 1;
		this.txtcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcode.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcode.Location = new System.Drawing.Point(65, 16);
		this.txtcode.Name = "txtcode";
		this.txtcode.Size = new System.Drawing.Size(220, 25);
		this.txtcode.TabIndex = 0;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(314, 20);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(74, 17);
		this.label49.TabIndex = 15;
		this.label49.Text = "Description";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(16, 20);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(39, 17);
		this.label48.TabIndex = 13;
		this.label48.Text = "Code";
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 133);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(859, 223);
		this.panel1.TabIndex = 82;
		this.grdfrm.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(859, 223);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 77;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(812, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 27);
		this.ctrlGrdBar1.TabIndex = 83;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(859, 356);
		base.Controls.Add(this.ctrlGrdBar1);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel7);
		base.Controls.Add(this.tsItemTaxSchedule);
		base.KeyPreview = true;
		base.Name = "InvDeffrmWarehousePartyProcessing";
		this.Text = "InvDeffrmWarehouse";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvDeffrmWarehousePartyProcessing_FormClosing);
		base.Load += new System.EventHandler(InvDeffrmWarehouse_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDeffrmWarehouse_KeyDown);
		this.tsItemTaxSchedule.ResumeLayout(false);
		this.tsItemTaxSchedule.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
