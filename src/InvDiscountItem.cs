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
namespace Architecture.WinApp.Inventory_Definition
{

public class InvDiscountItem : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMood;

	private Rightsobjects formrights = new Rightsobjects();

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

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkIsActive;

	private Button btnUpdateF;

	private Button BtnCancel;

	private Button BtnSavef;

	internal GridEX grdfrm;

	private Panel panel2;

	private Label label1;

	public InvDiscountItem(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private bool FormValiadation()
	{
		if (txtcode.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Discount Item Code");
			txtcode.Focus();
			return false;
		}
		if (txtDescription.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Discount %");
			txtDescription.Focus();
			return false;
		}
		return true;
	}

	private void BindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable getList = InvDiscountItem.FormHistory(new InvDiscountItem
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (getList.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("Id");
				table.Columns.Add("DiscountItemName");
				table.Columns.Add("Percentage", typeof(double));
				table.Columns.Add("EntryUser");
				table.Columns.Add("EntryDate");
				table.Columns.Add("IsActive");
				grdfrm.DataSource = table;
				for (int i = 0; i < getList.Rows.Count; i++)
				{
					table.Rows.Add(getList.Rows[i]["Id"], getList.Rows[i]["DiscountItemName"], Conversion.ToDouble(getList.Rows[i]["DiscPerncentage"]), getList.Rows[i]["EntryUser"], Conversion.ToDateTime(getList.Rows[i]["EntryDate"]).ToShortDateString(), getList.Rows[i]["IsActive"]);
				}
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
				grdSettings();
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
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["EntryUser"].Width = 100;
			grdfrm.RootTable.Columns["EntryDate"].Width = 80;
			grdfrm.RootTable.Columns["DiscountItemName"].Width = 100;
			grdfrm.RootTable.Columns["Percentage"].Width = 80;
			grdfrm.RootTable.Columns["DiscountItemName"].Caption = "Discount Name";
			grdfrm.RootTable.Columns.Add("Edit");
			grdfrm.RootTable.Columns["Edit"].Key = "Edit";
			grdfrm.RootTable.Columns["Edit"].Caption = "Edit";
			grdfrm.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfrm.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdfrm.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdfrm.RootTable.Columns["Edit"].Width = 40;
			grdfrm.RootTable.Columns["Edit"].Position = 0;
			grdfrm.FrozenColumns = 1;
			grdfrm.AllowEdit = (InheritableBoolean)2;
			grdfrm.AllowDelete = (InheritableBoolean)2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
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

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		if (e.Column.Key == "Edit")
		{
			ReadById();
		}
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

	private void btnupdate_Click(object sender, EventArgs e)
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
			InvDiscountItem obj = new InvDiscountItem();
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
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.DiscountItemName = txtcode.Text;
			obj.DiscPerncentage = Conversion.ToDouble((object)txtDescription.Text);
			if (Conversion.ToDouble((object)txtDescription.Text) > 100.0)
			{
				throw new Exception("Disc Perncentage % Should Not Be Greater Than 100");
			}
			if (ChkIsActive.Checked)
			{
				obj.IsActive = true;
			}
			else
			{
				obj.IsActive = false;
			}
			int Id = InvDiscountItem.Save(obj);
			if (RecId == 0 && Id > 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			clear();
			BindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvDiscountItem cat = new InvDiscountItem();
			RecId = Conversion.ToInt((object)grdfrm.CurrentRow.Cells["Id"].Value.ToString());
			cat = InvDiscountItem.GetById(RecId);
			txtcode.Text = Conversion.ToString((object)cat.DiscountItemName);
			txtDescription.Text = Conversion.ToString((object)cat.DiscPerncentage);
			ChkIsActive.Checked = Conversion.ToBool((object)cat.IsActive);
			BindHistory();
			grdSettings();
			btnupdate.Visible = true;
			btnsave.Visible = false;
			btnUpdateF.Visible = true;
			BtnSavef.Visible = false;
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
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			BtnSavef.Enabled = formrights.DoHaveSaveRight;
			btnUpdateF.Enabled = formrights.DoHaveUpdateRights;
			btnUpdateF.Visible = false;
			BtnSavef.Visible = true;
			btnsave.Visible = true;
			BindHistory();
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

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave.Visible = true;
			clear();
			BindHistory();
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

	private void clear()
	{
		try
		{
			btnsave.Visible = true;
			txtcode.Clear();
			txtDescription.Clear();
			btnupdate.Visible = false;
			UpdateMood = false;
			txtcode.Focus();
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
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

	private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void InvDiscountItem_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c56: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.InvDiscountItem));
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.ChkIsActive = new System.Windows.Forms.CheckBox();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.txtcode = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.tsItemTaxSchedule.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(503, 27);
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
		this.panel7.Size = new System.Drawing.Size(503, 28);
		this.panel7.TabIndex = 81;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label43.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(4, 5);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(87, 19);
		this.label43.TabIndex = 0;
		this.label43.Text = "Discounts";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.btnUpdateF);
		this.panel4.Controls.Add(this.BtnCancel);
		this.panel4.Controls.Add(this.BtnSavef);
		this.panel4.Controls.Add(this.ChkIsActive);
		this.panel4.Controls.Add(this.txtDescription);
		this.panel4.Controls.Add(this.txtcode);
		this.panel4.Controls.Add(this.label49);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(503, 123);
		this.panel4.TabIndex = 80;
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(343, 76);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 605;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.btnUpdateF.Click += new System.EventHandler(btnupdate_Click);
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(264, 76);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 604;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(343, 76);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 603;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.BtnSavef.Click += new System.EventHandler(btnsave_Click);
		this.ChkIsActive.AutoSize = true;
		this.ChkIsActive.Checked = true;
		this.ChkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkIsActive.Location = new System.Drawing.Point(167, 76);
		this.ChkIsActive.Name = "ChkIsActive";
		this.ChkIsActive.Size = new System.Drawing.Size(71, 19);
		this.ChkIsActive.TabIndex = 2;
		this.ChkIsActive.Text = "Is Active";
		this.ChkIsActive.UseVisualStyleBackColor = true;
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(167, 47);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(251, 23);
		this.txtDescription.TabIndex = 1;
		this.txtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDescription_KeyPress);
		this.txtcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcode.Location = new System.Drawing.Point(167, 21);
		this.txtcode.Name = "txtcode";
		this.txtcode.Size = new System.Drawing.Size(251, 23);
		this.txtcode.TabIndex = 0;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(77, 51);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(79, 15);
		this.label49.TabIndex = 15;
		this.label49.Text = "Percentage %";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(77, 25);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(90, 15);
		this.label48.TabIndex = 13;
		this.label48.Text = "Discount Name";
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 178);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(503, 446);
		this.panel1.TabIndex = 82;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(503, 418);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 83;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(503, 28);
		this.panel2.TabIndex = 82;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(456, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 27);
		this.ctrlGrdBar1.TabIndex = 83;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(503, 624);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel7);
		base.Controls.Add(this.tsItemTaxSchedule);
		base.KeyPreview = true;
		base.Name = "InvDiscountItem";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Discounts";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvDiscountItem_FormClosing);
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
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
