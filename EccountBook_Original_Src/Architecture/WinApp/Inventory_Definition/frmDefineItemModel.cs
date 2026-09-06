using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Definition;

public class frmDefineItemModel : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private long RecId;

	private bool UpdateMood;

	private IContainer components;

	private ToolStrip tsItemTaxSchedule;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel7;

	private Label label43;

	private Panel panel4;

	private TextBox txtSeqNo;

	private TextBox txtItemModel;

	private Label label49;

	private Label label48;

	private Panel panel1;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkIsActive;

	private Panel panel2;

	private Label label3;

	private Panel panel3;

	private Button btnClear;

	private Button BtnSave2;

	private Button BtnUpdate2;

	private TextBox txtSampleNo;

	private Label label1;

	public frmDefineItemModel(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private bool formValidation()
	{
		if (txtItemModel.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Item Model Name");
			txtItemModel.Focus();
			return false;
		}
		if (txtSeqNo.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert SeqNo");
			txtSeqNo.Focus();
			return false;
		}
		return true;
	}

	private void clear()
	{
		try
		{
			txtItemModel.Clear();
			txtSampleNo.Clear();
			txtSeqNo.Clear();
			UpdateMood = false;
			btnsave.Visible = true;
			BtnSave2.Visible = true;
			btnupdate.Visible = false;
			BtnUpdate2.Visible = false;
			txtItemModel.Focus();
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable getList = ItemModel.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			DataTable table = new DataTable();
			table.Columns.Add("Id", typeof(long));
			table.Columns.Add("ItemModel");
			table.Columns.Add("SeqNo");
			table.Columns.Add("IsActive");
			table.Columns.Add("SampleNo");
			grdfrm.DataSource = table;
			for (int i = 0; i < getList.Rows.Count; i++)
			{
				table.Rows.Add(getList.Rows[i]["itemModelId"], getList.Rows[i]["modalName"], getList.Rows[i]["SeqNo"], getList.Rows[i]["isActive"], getList.Rows[i]["SampleNo"]);
			}
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.AllowEdit = (InheritableBoolean)2;
			grdfrm.AllowDelete = (InheritableBoolean)2;
			grdfrm.RootTable.Columns["ItemModel"].Width = 250;
			grdfrm.RootTable.Columns["SeqNo"].Width = 60;
			grdfrm.RootTable.Columns["IsActive"].Width = 70;
			grdfrm.RootTable.Columns["SampleNo"].Width = 100;
			ctrlGrdBar1_Load(null, null);
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
			RecId = 0L;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		try
		{
			if (!formValidation())
			{
				return;
			}
			if (grdfrm.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdfrm.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (RecId > 0)
					{
						if (Conversion.ToInt(item.Cells["SeqNo"].Value) == Conversion.ToInt((object)txtSeqNo.Text) && Conversion.ToInt(item.Cells["Id"].Value) != RecId)
						{
							throw new Exception("Seq No:" + txtSeqNo.Text + " Already Allotted to another item model please check!");
						}
					}
					else if (Conversion.ToInt(item.Cells["SeqNo"].Value) == Conversion.ToInt((object)txtSeqNo.Text))
					{
						throw new Exception("Seq No:" + txtSeqNo.Text + " Already Allotted to another item model please check!");
					}
				}
			}
			ItemModel Item = new ItemModel();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				Item.itemModelId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			Item.modalName = Conversion.ToString((object)txtItemModel.Text.Trim());
			Item.SampleNo = Conversion.ToString((object)txtSampleNo.Text.Trim());
			Item.SeqNo = Conversion.ToInt((object)txtSeqNo.Text.Trim());
			Item.CreatedUserId = UserAccount.ID;
			Item.LastModifiedUserId = UserAccount.ID;
			Item.OrganizationId = UserAccount.OrganizationId;
			Item.CompanyId = UserAccount.CompanyId;
			Item.BranchesId = UserAccount.BranchesId;
			if (ChkIsActive.Checked)
			{
				Item.isActive = true;
			}
			else
			{
				Item.isActive = false;
			}
			ItemModel.Save(Item);
			if (RecId == 0L)
			{
				MessageBox.Show("Save Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
			}
			gridfill();
			clear();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			BtnSave2.Visible = false;
			ItemModel cat = new ItemModel();
			RecId = Conversion.ToInt64(grdfrm.CurrentRow.Cells["Id"].Value);
			cat = ItemModel.GetByID(RecId);
			txtItemModel.Text = Conversion.ToString((object)cat.modalName);
			txtSampleNo.Text = Conversion.ToString((object)cat.SampleNo);
			txtSeqNo.Text = Conversion.ToString((object)cat.SeqNo);
			ChkIsActive.Checked = Conversion.ToBool((object)cat.isActive);
			btnupdate.Visible = true;
			BtnUpdate2.Visible = true;
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
			btnupdate.Visible = false;
			BtnSave2.Visible = true;
			BtnUpdate2.Visible = false;
			base.WindowState = FormWindowState.Normal;
			base.Size = new Size(500, 680);
			if (base.Parent != null)
			{
				int centerX = base.Parent.Left + (base.Parent.Width - base.Width) / 2;
				int centerY = base.Parent.Top + (base.Parent.Height - base.Height) / 2;
				base.Location = new Point(centerX, centerY);
			}
			else
			{
				base.StartPosition = FormStartPosition.CenterParent;
			}
			gridfill();
			txtItemModel.Focus();
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
			MessageBox.Show(ex.Message);
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

	private void BtnSave2_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0L;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnUpdate2_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnClear_Click(object sender, EventArgs e)
	{
		try
		{
			clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InvDeffrmWarehouse_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtSeqNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
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
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.frmDefineItemModel));
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnClear = new System.Windows.Forms.Button();
		this.BtnSave2 = new System.Windows.Forms.Button();
		this.BtnUpdate2 = new System.Windows.Forms.Button();
		this.ChkIsActive = new System.Windows.Forms.CheckBox();
		this.txtSeqNo = new System.Windows.Forms.TextBox();
		this.txtItemModel = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtSampleNo = new System.Windows.Forms.TextBox();
		this.tsItemTaxSchedule.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.tsItemTaxSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(359, 27);
		this.tsItemTaxSchedule.TabIndex = 80;
		this.tsItemTaxSchedule.Text = "toolStrip3";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label43);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 27);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(359, 26);
		this.panel7.TabIndex = 81;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.Teal;
		this.label43.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(4, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(102, 19);
		this.label43.TabIndex = 0;
		this.label43.Text = "Item Model";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.btnClear);
		this.panel4.Controls.Add(this.BtnSave2);
		this.panel4.Controls.Add(this.BtnUpdate2);
		this.panel4.Controls.Add(this.ChkIsActive);
		this.panel4.Controls.Add(this.txtSampleNo);
		this.panel4.Controls.Add(this.txtSeqNo);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtItemModel);
		this.panel4.Controls.Add(this.label49);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 53);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(359, 140);
		this.panel4.TabIndex = 0;
		this.btnClear.BackColor = System.Drawing.Color.Teal;
		this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClear.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnClear.ForeColor = System.Drawing.Color.White;
		this.btnClear.Location = new System.Drawing.Point(206, 84);
		this.btnClear.Name = "btnClear";
		this.btnClear.Size = new System.Drawing.Size(61, 27);
		this.btnClear.TabIndex = 5;
		this.btnClear.Text = "Cancel";
		this.btnClear.UseVisualStyleBackColor = false;
		this.btnClear.Click += new System.EventHandler(btnClear_Click);
		this.BtnSave2.BackColor = System.Drawing.Color.Teal;
		this.BtnSave2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSave2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSave2.ForeColor = System.Drawing.Color.White;
		this.BtnSave2.Location = new System.Drawing.Point(267, 84);
		this.BtnSave2.Name = "BtnSave2";
		this.BtnSave2.Size = new System.Drawing.Size(80, 27);
		this.BtnSave2.TabIndex = 4;
		this.BtnSave2.Text = "Save";
		this.BtnSave2.UseVisualStyleBackColor = false;
		this.BtnSave2.Click += new System.EventHandler(BtnSave2_Click);
		this.BtnUpdate2.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdate2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdate2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdate2.ForeColor = System.Drawing.Color.White;
		this.BtnUpdate2.Location = new System.Drawing.Point(266, 84);
		this.BtnUpdate2.Name = "BtnUpdate2";
		this.BtnUpdate2.Size = new System.Drawing.Size(80, 27);
		this.BtnUpdate2.TabIndex = 5;
		this.BtnUpdate2.Text = "Update";
		this.BtnUpdate2.UseVisualStyleBackColor = false;
		this.BtnUpdate2.Click += new System.EventHandler(BtnUpdate2_Click);
		this.ChkIsActive.AutoSize = true;
		this.ChkIsActive.Checked = true;
		this.ChkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsActive.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkIsActive.Location = new System.Drawing.Point(271, 59);
		this.ChkIsActive.Name = "ChkIsActive";
		this.ChkIsActive.Size = new System.Drawing.Size(85, 20);
		this.ChkIsActive.TabIndex = 3;
		this.ChkIsActive.Text = "Is Active";
		this.ChkIsActive.UseVisualStyleBackColor = true;
		this.txtSeqNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSeqNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSeqNo.Location = new System.Drawing.Point(90, 58);
		this.txtSeqNo.Name = "txtSeqNo";
		this.txtSeqNo.Size = new System.Drawing.Size(177, 23);
		this.txtSeqNo.TabIndex = 2;
		this.txtSeqNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSeqNo_KeyPress);
		this.txtItemModel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemModel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemModel.Location = new System.Drawing.Point(90, 8);
		this.txtItemModel.Name = "txtItemModel";
		this.txtItemModel.Size = new System.Drawing.Size(257, 23);
		this.txtItemModel.TabIndex = 0;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(10, 62);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(46, 15);
		this.label49.TabIndex = 15;
		this.label49.Text = "Seq No";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(10, 12);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(69, 15);
		this.label48.TabIndex = 13;
		this.label48.Text = "Item Model";
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 193);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(359, 312);
		this.panel1.TabIndex = 82;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 26);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(359, 286);
		this.panel3.TabIndex = 10;
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(359, 286);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(359, 26);
		this.panel2.TabIndex = 9;
		this.panel2.Visible = false;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(317, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 83;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 19);
		this.label3.TabIndex = 25;
		this.label3.Text = "History";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 37);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(66, 15);
		this.label1.TabIndex = 15;
		this.label1.Text = "Sample No";
		this.txtSampleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSampleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSampleNo.Location = new System.Drawing.Point(90, 33);
		this.txtSampleNo.Name = "txtSampleNo";
		this.txtSampleNo.Size = new System.Drawing.Size(257, 23);
		this.txtSampleNo.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(359, 505);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel7);
		base.Controls.Add(this.tsItemTaxSchedule);
		base.KeyPreview = true;
		base.Name = "frmDefineItemModel";
		this.Text = "Define Item Model";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvDeffrmWarehouse_FormClosing);
		base.Load += new System.EventHandler(InvDeffrmWarehouse_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDeffrmWarehouse_KeyDown);
		this.tsItemTaxSchedule.ResumeLayout(false);
		this.tsItemTaxSchedule.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
