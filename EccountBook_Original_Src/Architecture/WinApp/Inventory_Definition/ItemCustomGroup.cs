using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Definition;

public class ItemCustomGroup : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtHistory = new DataTable();

	private int RecId;

	private int UpdateIndex;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private Panel panel2;

	internal GridEX Grd;

	private Button BtnCancel;

	private Button BtnSavef;

	private Button btnUpdateF;

	private Panel panel3;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label23;

	private Panel panel4;

	private Label label1;

	private TextBox txtgroupName;

	private TextBox txtGroupcode;

	private Label label2;

	private Label Effective;

	public ItemCustomGroup(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void ItemCustomGroup_Load(object sender, EventArgs e)
	{
		try
		{
			BackColor = Color.Teal;
			BindHistory();
			btnUpdate.Visible = false;
			btnsave.Visible = true;
			btnUpdateF.Visible = false;
			BtnSavef.Visible = true;
			txtGroupcode.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ItemCustomGroup obj = new ItemCustomGroup();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			dtHistory = ItemCustomGroup.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("GroupCode");
				dt.Columns.Add("GroupName");
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["GroupCode"], dtHistory.Rows[i]["GroupName"]);
				}
				Grd.DataSource = dt;
				Grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				Grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			Grd.RootTable.Columns["Id"].Visible = false;
			Grd.RootTable.Columns.Add("Edit");
			Grd.RootTable.Columns["Edit"].Key = "Edit";
			Grd.RootTable.Columns["Edit"].Caption = "Edit";
			Grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			Grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			Grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			Grd.RootTable.Columns["Edit"].Width = 40;
			Grd.RootTable.Columns["Edit"].Position = 0;
			Grd.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, Grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = Grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				RecId = Conversion.ToInt(item.Cells["Id"].Value);
				txtGroupcode.Text = Conversion.ToString((object)item.Cells["GroupCode"].Text);
				txtgroupName.Text = Conversion.ToString((object)item.Cells["GroupName"].Text);
				btnUpdate.Visible = true;
				btnsave.Visible = false;
				btnUpdateF.Visible = true;
				BtnSavef.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		if (e.Column.Key == "Edit")
		{
			ReadById();
		}
	}

	private bool FormValiadation()
	{
		if (txtGroupcode.Text == "")
		{
			MessageBox.Show("Group Code Field Is Required");
			txtGroupcode.Focus();
			return false;
		}
		if (txtgroupName.Text == "")
		{
			MessageBox.Show("Group Name Field Is Required");
			txtgroupName.Focus();
			return false;
		}
		return true;
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

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValiadation())
			{
				return;
			}
			ItemCustomGroup obj = new ItemCustomGroup();
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
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.GroupCode = Conversion.ToString((object)txtGroupcode.Text);
			obj.GroupName = Conversion.ToString((object)txtgroupName.Text);
			int Id = ItemCustomGroup.save(obj);
			if (RecId == 0 && Id > 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			Reset();
			BindHistory();
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
			Insert();
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
			Reset();
			BindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			txtGroupcode.Focus();
			txtGroupcode.Text = string.Empty;
			txtgroupName.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)Grd).Name);
			if (fs != null)
			{
				Grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ItemCustomGroup_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			btnsave_Click(null, null);
		}
	}

	private void ItemCustomGroup_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Expected O, but got Unknown
		GridEXLayout Grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.ItemCustomGroup));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.Grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.txtgroupName = new System.Windows.Forms.TextBox();
		this.txtGroupcode = new System.Windows.Forms.TextBox();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.Effective = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(479, 594);
		this.panel1.TabIndex = 0;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.Grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 207);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(479, 387);
		this.panel3.TabIndex = 10;
		this.Grd.AllowEdit = (InheritableBoolean)2;
		Grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.Grd.DesignTimeLayout = Grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.Grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.Grd.DynamicFiltering = true;
		this.Grd.FilterMode = (FilterMode)2;
		this.Grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.Grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.Grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.Grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.Grd).Name = "Grd";
		((ScrollableBase)this.Grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.Grd).Size = new System.Drawing.Size(479, 387);
		((System.Windows.Forms.Control)(object)this.Grd).TabIndex = 0;
		this.Grd.TotalRow = (InheritableBoolean)1;
		this.Grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.Grd).VisualStyle = (VisualStyle)4;
		this.Grd.ColumnButtonClick += new ColumnActionEventHandler(Grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.Grd).DoubleClick += new System.EventHandler(Grd_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label23);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 176);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(479, 31);
		this.panel6.TabIndex = 9;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(67, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(434, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.Grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 26;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.txtgroupName);
		this.panel2.Controls.Add(this.txtGroupcode);
		this.panel2.Controls.Add(this.btnUpdateF);
		this.panel2.Controls.Add(this.BtnCancel);
		this.panel2.Controls.Add(this.BtnSavef);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.Effective);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(479, 149);
		this.panel2.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(479, 31);
		this.panel4.TabIndex = 598;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(8, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(168, 19);
		this.label1.TabIndex = 25;
		this.label1.Text = "Item Custom Group";
		this.txtgroupName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgroupName.Location = new System.Drawing.Point(133, 72);
		this.txtgroupName.Name = "txtgroupName";
		this.txtgroupName.Size = new System.Drawing.Size(272, 23);
		this.txtgroupName.TabIndex = 1;
		this.txtGroupcode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGroupcode.Location = new System.Drawing.Point(133, 44);
		this.txtGroupcode.Name = "txtGroupcode";
		this.txtGroupcode.Size = new System.Drawing.Size(272, 23);
		this.txtGroupcode.TabIndex = 0;
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(330, 101);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 2;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.btnUpdateF.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(251, 101);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 3;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(330, 101);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 2;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.BtnSavef.Click += new System.EventHandler(btnsave_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(46, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 17);
		this.label2.TabIndex = 595;
		this.label2.Text = "Group Name";
		this.Effective.AutoSize = true;
		this.Effective.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Effective.Location = new System.Drawing.Point(46, 47);
		this.Effective.Name = "Effective";
		this.Effective.Size = new System.Drawing.Size(81, 17);
		this.Effective.TabIndex = 594;
		this.Effective.Text = "Group Code";
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnsave, this.btnUpdate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(479, 27);
		this.toolStrip1.TabIndex = 0;
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
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(479, 594);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "ItemCustomGroup";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "ItemCustomGroup";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ItemCustomGroup_FormClosing);
		base.Load += new System.EventHandler(ItemCustomGroup_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ItemCustomGroup_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.Grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
