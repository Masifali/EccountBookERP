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
using Architecture.WinApp.Lookups;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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

public class InvDeffrmWarehouse : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMood;

	private DropDownBind DDL = new DropDownBind();

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

	private CheckBox ChkIsActive;

	private Panel panel2;

	private Label label3;

	private Panel panel3;

	private Button btnClear;

	private Button BtnSave2;

	private Button BtnUpdate2;

	internal UltraCombo CmbWarehouseType;

	private Label label1;

	private ToolStripButton btnWarehouseAllocateToBranch;

	private ToolStripButton btnDefineRack;

	public InvDeffrmWarehouse(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)CmbWarehouseType).ActiveRow == null)
		{
			MessageBox.Show("Department Type Field Is Required");
			txtcode.Focus();
			return false;
		}
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

	private void GetWarehouseTypes()
	{
		try
		{
			DataTable dt = InvWareHouse.GetWareHouseType();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbWarehouseType, "Id", "WareHouseType", "Department Type", ZeroIndex: false);
				((UltraGridBase)CmbWarehouseType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void clear()
	{
		try
		{
			txtcode.Clear();
			txtDescription.Clear();
			UpdateMood = false;
			btnsave.Visible = true;
			BtnSave2.Visible = true;
			btnupdate.Visible = false;
			BtnUpdate2.Visible = false;
			CmbWarehouseType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill()
	{
		try
		{
			DataTable getList = CommonServices.WareHouseGetAllService();
			DataTable table = new DataTable();
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("DepartmentType");
			table.Columns.Add("WarehouseCode");
			table.Columns.Add("Description");
			table.Columns.Add("IsActive");
			grdfrm.DataSource = table;
			for (int i = 0; i < getList.Rows.Count; i++)
			{
				table.Rows.Add(getList.Rows[i]["Id"], getList.Rows[i]["WareHouseType"], getList.Rows[i]["WareHouseCode"], getList.Rows[i]["WareHouseName"], getList.Rows[i]["IsActive"]);
			}
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.AllowEdit = (InheritableBoolean)2;
			grdfrm.AllowDelete = (InheritableBoolean)2;
			grdfrm.RootTable.Columns["DepartmentType"].Width = 250;
			grdfrm.RootTable.Columns["WarehouseCode"].Width = 250;
			grdfrm.RootTable.Columns["Description"].Width = 250;
			grdfrm.RootTable.Columns["IsActive"].Width = 150;
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
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			InvWareHouse cat = new InvWareHouse();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				cat.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			cat.WareHouseCode = Conversion.ToString((object)txtcode.Text.Trim());
			cat.WareHouseName = Conversion.ToString((object)txtDescription.Text.Trim());
			cat.WarehouseType = Conversion.ToString(CmbWarehouseType.Value);
			cat.EntryDate = DateTime.Now;
			cat.EntryUser = UserAccount.ID;
			cat.ModifyDate = DateTime.Now;
			cat.ModifyUser = UserAccount.ID;
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			if (ChkIsActive.Checked)
			{
				cat.IsActive = true;
			}
			else
			{
				cat.IsActive = false;
			}
			InvWareHouse.Save(cat);
			if (RecId == 0)
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
			InvWareHouse cat = new InvWareHouse();
			RecId = Conversion.ToInt((object)grdfrm.CurrentRow.Cells[0].Value.ToString());
			cat = InvWareHouse.GetByID(RecId);
			txtcode.Text = Conversion.ToString((object)cat.WareHouseCode);
			txtDescription.Text = Conversion.ToString((object)cat.WareHouseName);
			CmbWarehouseType.Value = Conversion.ToInt((object)cat.WarehouseType);
			ChkIsActive.Checked = Conversion.ToBool((object)cat.IsActive);
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
			gridfill();
			GetWarehouseTypes();
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
			RecId = 0;
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

	private void btnWarehouseAllocateToBranch_Click(object sender, EventArgs e)
	{
		try
		{
			new frmWarehousesAllocationToBranch(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineRack_Click(object sender, EventArgs e)
	{
		try
		{
			frmRackDefine obj = new frmRackDefine(UserAccount);
			obj.StartPosition = FormStartPosition.CenterScreen;
			obj.Show();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.InvDeffrmWarehouse));
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnWarehouseAllocateToBranch = new System.Windows.Forms.ToolStripButton();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbWarehouseType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.btnClear = new System.Windows.Forms.Button();
		this.BtnSave2 = new System.Windows.Forms.Button();
		this.BtnUpdate2 = new System.Windows.Forms.Button();
		this.ChkIsActive = new System.Windows.Forms.CheckBox();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.txtcode = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.btnDefineRack = new System.Windows.Forms.ToolStripButton();
		this.tsItemTaxSchedule.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseType).BeginInit();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.tsItemTaxSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnupdate, this.btnsave, this.btnWarehouseAllocateToBranch, this.btnDefineRack });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(529, 27);
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
		this.btnWarehouseAllocateToBranch.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnWarehouseAllocateToBranch.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWarehouseAllocateToBranch.Name = "btnWarehouseAllocateToBranch";
		this.btnWarehouseAllocateToBranch.Size = new System.Drawing.Size(212, 24);
		this.btnWarehouseAllocateToBranch.Text = "Warehouse Allocate To Branch";
		this.btnWarehouseAllocateToBranch.Click += new System.EventHandler(btnWarehouseAllocateToBranch_Click);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label43);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 27);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(529, 26);
		this.panel7.TabIndex = 81;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.Teal;
		this.label43.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(8, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(158, 19);
		this.label43.TabIndex = 0;
		this.label43.Text = "Define Warehouse";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseType);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.btnClear);
		this.panel4.Controls.Add(this.BtnSave2);
		this.panel4.Controls.Add(this.BtnUpdate2);
		this.panel4.Controls.Add(this.ChkIsActive);
		this.panel4.Controls.Add(this.txtDescription);
		this.panel4.Controls.Add(this.txtcode);
		this.panel4.Controls.Add(this.label49);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 53);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(529, 126);
		this.panel4.TabIndex = 80;
		this.CmbWarehouseType.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseType).Location = new System.Drawing.Point(129, 6);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseType).Name = "CmbWarehouseType";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseType).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseType).TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(23, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(99, 15);
		this.label1.TabIndex = 20;
		this.label1.Text = "Department Type";
		this.btnClear.BackColor = System.Drawing.Color.Teal;
		this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClear.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnClear.ForeColor = System.Drawing.Color.White;
		this.btnClear.Location = new System.Drawing.Point(181, 93);
		this.btnClear.Name = "btnClear";
		this.btnClear.Size = new System.Drawing.Size(80, 27);
		this.btnClear.TabIndex = 6;
		this.btnClear.Text = "Cancel";
		this.btnClear.UseVisualStyleBackColor = false;
		this.btnClear.Click += new System.EventHandler(btnClear_Click);
		this.BtnSave2.BackColor = System.Drawing.Color.Teal;
		this.BtnSave2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSave2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSave2.ForeColor = System.Drawing.Color.White;
		this.BtnSave2.Location = new System.Drawing.Point(268, 93);
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
		this.BtnUpdate2.Location = new System.Drawing.Point(267, 93);
		this.BtnUpdate2.Name = "BtnUpdate2";
		this.BtnUpdate2.Size = new System.Drawing.Size(80, 27);
		this.BtnUpdate2.TabIndex = 5;
		this.BtnUpdate2.Text = "Update";
		this.BtnUpdate2.UseVisualStyleBackColor = false;
		this.BtnUpdate2.Click += new System.EventHandler(BtnUpdate2_Click);
		this.ChkIsActive.AutoSize = true;
		this.ChkIsActive.Checked = true;
		this.ChkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsActive.Location = new System.Drawing.Point(352, 69);
		this.ChkIsActive.Name = "ChkIsActive";
		this.ChkIsActive.Size = new System.Drawing.Size(67, 17);
		this.ChkIsActive.TabIndex = 3;
		this.ChkIsActive.Text = "Is Active";
		this.ChkIsActive.UseVisualStyleBackColor = true;
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(129, 63);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(219, 25);
		this.txtDescription.TabIndex = 2;
		this.txtcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcode.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcode.Location = new System.Drawing.Point(129, 35);
		this.txtcode.Name = "txtcode";
		this.txtcode.Size = new System.Drawing.Size(219, 25);
		this.txtcode.TabIndex = 1;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(23, 68);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(68, 15);
		this.label49.TabIndex = 15;
		this.label49.Text = "Description";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(23, 40);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(97, 15);
		this.label48.TabIndex = 13;
		this.label48.Text = "Warehouse Code";
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 179);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(529, 326);
		this.panel1.TabIndex = 82;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 26);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(529, 300);
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(529, 300);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 7;
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
		this.panel2.Size = new System.Drawing.Size(529, 26);
		this.panel2.TabIndex = 9;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(487, 2);
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
		this.label3.Location = new System.Drawing.Point(8, 2);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 19);
		this.label3.TabIndex = 25;
		this.label3.Text = "History";
		this.btnDefineRack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDefineRack.Image = (System.Drawing.Image)resources.GetObject("btnDefineRack.Image");
		this.btnDefineRack.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineRack.Name = "btnDefineRack";
		this.btnDefineRack.Size = new System.Drawing.Size(88, 24);
		this.btnDefineRack.Text = "Define Rack";
		this.btnDefineRack.Click += new System.EventHandler(btnDefineRack_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(529, 505);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel7);
		base.Controls.Add(this.tsItemTaxSchedule);
		base.KeyPreview = true;
		base.Name = "InvDeffrmWarehouse";
		this.Text = "InvDeffrmWarehouse";
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
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseType).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
