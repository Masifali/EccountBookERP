using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.PartyProcessing;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.PartyProcessing;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.PartyProcessing
{

public class DefItemCatagoryPartyProcessing : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private List<COAAllocation> Accountslst = new List<COAAllocation>();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Label label35;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel2;

	private Panel panel3;

	private Panel panel6;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label1;

	private CheckBox chkstatus;

	private TextBox txtItemCategorySerialTo;

	private TextBox txtItemCategorySerialFrom;

	private TextBox txtItemCategoryDescription;

	private TextBox txtItemCategoryCode;

	private Label label39;

	private Label label38;

	private Label label37;

	private Label label36;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label8;

	internal UltraCombo CmbItemParentCategory;

	private Panel panel4;

	public DefItemCatagoryPartyProcessing(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void ItemParentCategoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dt = Item.InventoryParentCategories(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "InventoryParentCategories"
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbItemParentCategory, "Id", "InvParentCateDescription", "InvParentCateDescription", ZeroIndex: false);
				((UltraGridBase)CmbItemParentCategory).Rows[0].Activate();
			}
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
			txtItemCategoryCode.Clear();
			txtItemCategoryDescription.Clear();
			txtItemCategorySerialFrom.Clear();
			txtItemCategorySerialTo.Clear();
			((Control)(object)CmbItemParentCategory).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			txtItemCategoryCode.Focus();
			gridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (txtItemCategoryCode.Text.Trim() == "" || txtItemCategoryCode.Text == "0")
		{
			MessageBox.Show("Please Insert Code");
			txtItemCategoryCode.Focus();
			return false;
		}
		if (txtItemCategoryDescription.Text.Trim() == "" || txtItemCategoryDescription.Text.Trim() == "0")
		{
			MessageBox.Show("Please Insert Description");
			txtItemCategoryDescription.Focus();
			return false;
		}
		if (txtItemCategorySerialFrom.Text.Trim() == "" || txtItemCategorySerialFrom.Text.Trim() == "0")
		{
			MessageBox.Show("Please Insert Serial From");
			txtItemCategorySerialFrom.Focus();
			return false;
		}
		if (txtItemCategorySerialTo.Text.Trim() == "" || txtItemCategorySerialTo.Text.Trim() == "0")
		{
			MessageBox.Show("Please Insert Serial To");
			txtItemCategorySerialTo.Focus();
			return false;
		}
		if (((Control)(object)CmbItemParentCategory).Text.Trim() == "" || Conversion.ToInt(CmbItemParentCategory.Value) == 0)
		{
			MessageBox.Show("Please Select Parent Category");
			CmbItemParentCategory.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ItemCategoryPartyProcessing.Save(new ItemCategoryPartyProcessing
			{
				Id = Conversion.ToInt((object)RecId),
				CategoryCode = Conversion.ToString((object)txtItemCategoryCode.Text.Trim()),
				CategoryDescription = Conversion.ToString((object)txtItemCategoryDescription.Text.Trim()),
				SerialFrom = Conversion.ToInt((object)txtItemCategorySerialFrom.Text.Trim()),
				SerialTo = Conversion.ToInt((object)txtItemCategorySerialTo.Text.Trim()),
				ParentCategoriesId = Conversion.ToInt(CmbItemParentCategory.Value),
				CategoryStatus = Conversion.ToBool((object)chkstatus.Checked),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EntryUser = UserAccount.ID,
				ModifyUser = UserAccount.ID
			});
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			Reset();
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

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ItemCategoryPartyProcessing cat = new ItemCategoryPartyProcessing();
			RecId = Conversion.ToInt((object)grdfrm.CurrentRow.Cells[0].Value.ToString());
			cat = ItemCategoryPartyProcessing.GetByID(RecId);
			txtItemCategoryCode.Text = Conversion.ToString((object)cat.CategoryCode);
			txtItemCategoryDescription.Text = Conversion.ToString((object)cat.CategoryDescription);
			txtItemCategorySerialFrom.Text = Conversion.ToString((object)cat.SerialFrom);
			txtItemCategorySerialTo.Text = Conversion.ToString((object)cat.SerialTo);
			CmbItemParentCategory.Value = Conversion.ToInt((object)cat.ParentCategoriesId);
			chkstatus.Checked = Conversion.ToBool((object)cat.CategoryStatus);
			btnsave.Visible = false;
			btnupdate.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CategoryCode");
			dt.Columns.Add("CategoryDescription");
			dt.Columns.Add("SerialFrom");
			dt.Columns.Add("SerialTo");
			dt.Columns.Add("CategoryStatus");
			DataTable dtGrid = ItemCategoryPartyProcessing.Getall(new ItemCategoryPartyProcessing
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGrid.Rows.Count > 0)
			{
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["CategoryCode"], dtGrid.Rows[i]["CategoryDescription"], dtGrid.Rows[i]["SerialFrom"], dtGrid.Rows[i]["SerialTo"], dtGrid.Rows[i]["CategoryStatus"]);
				}
				grdfrm.DataSource = dt;
				grdfrm.RetrieveStructure();
				gridsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grdfrm.RootTable.Columns["Id"].Visible = false;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvDeffrmItemCatagory_Load(object sender, EventArgs e)
	{
		try
		{
			gridfill();
			ItemParentCategoryFill();
			btnupdate.Visible = false;
			txtItemCategoryCode.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
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

	private void InvDeffrmItemCatagory_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (btnsave.Visible && e.Control && e.KeyCode == Keys.S)
		{
			btnsave_Click(null, null);
		}
		if (btnupdate.Visible && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void txtItemCategorySerialFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtItemCategorySerialTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void DefItemCatagoryPartyProcessing_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.DefItemCatagoryPartyProcessing));
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.label35 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdfrm = new GridEX();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbItemParentCategory = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.txtItemCategorySerialTo = new System.Windows.Forms.TextBox();
		this.txtItemCategorySerialFrom = new System.Windows.Forms.TextBox();
		this.txtItemCategoryDescription = new System.Windows.Forms.TextBox();
		this.txtItemCategoryCode = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemParentCategory).BeginInit();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(10, 5);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(122, 23);
		this.label35.TabIndex = 5;
		this.label35.Text = "Item Category";
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1036, 29);
		this.panel1.TabIndex = 66;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(980, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 68;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1036, 568);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1036, 27);
		this.toolStrip1.TabIndex = 67;
		this.toolStrip1.Text = "toolStrip1";
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
		this.btnupdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.label35);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 29);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1036, 40);
		this.panel2.TabIndex = 67;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 69);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1036, 67);
		this.panel3.TabIndex = 68;
		this.panel6.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.panel6.Controls.Add(this.label8);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemParentCategory);
		this.panel6.Controls.Add(this.label7);
		this.panel6.Controls.Add(this.label6);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Controls.Add(this.chkstatus);
		this.panel6.Controls.Add(this.txtItemCategorySerialTo);
		this.panel6.Controls.Add(this.txtItemCategorySerialFrom);
		this.panel6.Controls.Add(this.txtItemCategoryDescription);
		this.panel6.Controls.Add(this.txtItemCategoryCode);
		this.panel6.Controls.Add(this.label39);
		this.panel6.Controls.Add(this.label38);
		this.panel6.Controls.Add(this.label37);
		this.panel6.Controls.Add(this.label36);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1036, 67);
		this.panel6.TabIndex = 10;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(321, 36);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(121, 15);
		this.label8.TabIndex = 40;
		this.label8.Text = "Item Parent Category";
		this.CmbItemParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Location = new System.Drawing.Point(451, 33);
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Name = "CmbItemParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Size = new System.Drawing.Size(220, 24);
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(57, 11);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(13, 17);
		this.label7.TabIndex = 21;
		this.label7.Text = "*";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(76, 35);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(13, 17);
		this.label6.TabIndex = 21;
		this.label6.Text = "*";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.ForeColor = System.Drawing.Color.Red;
		this.label5.Location = new System.Drawing.Point(755, 13);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(13, 17);
		this.label5.TabIndex = 20;
		this.label5.Text = "*";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(407, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(13, 17);
		this.label1.TabIndex = 16;
		this.label1.Text = "*";
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.chkstatus.Location = new System.Drawing.Point(678, 36);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(74, 21);
		this.chkstatus.TabIndex = 5;
		this.chkstatus.Text = "Is Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.txtItemCategorySerialTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategorySerialTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtItemCategorySerialTo.Location = new System.Drawing.Point(95, 33);
		this.txtItemCategorySerialTo.Name = "txtItemCategorySerialTo";
		this.txtItemCategorySerialTo.Size = new System.Drawing.Size(220, 25);
		this.txtItemCategorySerialTo.TabIndex = 3;
		this.txtItemCategorySerialTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemCategorySerialTo_KeyPress);
		this.txtItemCategorySerialFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategorySerialFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtItemCategorySerialFrom.Location = new System.Drawing.Point(774, 9);
		this.txtItemCategorySerialFrom.Name = "txtItemCategorySerialFrom";
		this.txtItemCategorySerialFrom.Size = new System.Drawing.Size(220, 25);
		this.txtItemCategorySerialFrom.TabIndex = 2;
		this.txtItemCategorySerialFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemCategorySerialFrom_KeyPress);
		this.txtItemCategoryDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategoryDescription.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtItemCategoryDescription.Location = new System.Drawing.Point(451, 7);
		this.txtItemCategoryDescription.Name = "txtItemCategoryDescription";
		this.txtItemCategoryDescription.Size = new System.Drawing.Size(220, 25);
		this.txtItemCategoryDescription.TabIndex = 1;
		this.txtItemCategoryCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategoryCode.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtItemCategoryCode.Location = new System.Drawing.Point(95, 7);
		this.txtItemCategoryCode.Name = "txtItemCategoryCode";
		this.txtItemCategoryCode.Size = new System.Drawing.Size(220, 25);
		this.txtItemCategoryCode.TabIndex = 0;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label39.Location = new System.Drawing.Point(12, 37);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(58, 17);
		this.label39.TabIndex = 3;
		this.label39.Text = "Serial To";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label38.Location = new System.Drawing.Point(675, 13);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(74, 17);
		this.label38.TabIndex = 2;
		this.label38.Text = "Serial From";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label37.Location = new System.Drawing.Point(324, 11);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(74, 17);
		this.label37.TabIndex = 1;
		this.label37.Text = "Description";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label36.Location = new System.Drawing.Point(12, 11);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(39, 17);
		this.label36.TabIndex = 0;
		this.label36.Text = "Code";
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 136);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1036, 568);
		this.panel4.TabIndex = 69;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1036, 704);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefItemCatagoryPartyProcessing";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DefItemCatagoryPartyProcessing_FormClosing);
		base.Load += new System.EventHandler(InvDeffrmItemCatagory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDeffrmItemCatagory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemParentCategory).EndInit();
		this.panel4.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
