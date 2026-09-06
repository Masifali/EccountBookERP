using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.SDT;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SDT;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.SDT
{

public class frmClientCustomGroup : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbCutomGroup;

	private Panel panel6;

	private Label label15;

	private Panel panel7;

	private Label lbltocompany;

	private Panel panel3;

	private Label label4;

	private Button BtnAllocateItems;

	private Panel panel4;

	private Label label1;

	private Button btnDeAllocate;

	internal GridEX GridUnAllocated;

	internal GridEX GridAllocated;

	private Button BtnShow;

	private ToolStripButton BtnNew;

	public frmClientCustomGroup(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmItemsCustomizedGroups_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(700, 400);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			BtnAllocateItems.Enabled = formright.DoHaveSaveRight;
			btnDeAllocate.Enabled = formright.DoHaveUpdateRights;
			CustomGroupBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CustomGroupBind()
	{
		try
		{
			int ID = Conversion.ToInt(CmbCutomGroup.Value);
			DataTable dt = CustomGroup.GetCustomGroupForCombo(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCutomGroup, "customGroupId", "customGroupName", "CustomGroup Name", ZeroIndex: false);
				((UltraGridBase)CmbCutomGroup).DisplayLayout.Bands[0].Columns[1].Width = 250;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("customGroupId") == ID))
					{
						CmbCutomGroup.Value = ID;
					}
					else
					{
						((Control)(object)CmbCutomGroup).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCutomGroup).Text = string.Empty;
				((UltraGridBase)CmbCutomGroup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemCutomGroup_TextChanged(object sender, EventArgs e)
	{
	}

	private void frmItemsCustomizedGroups_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			btnRefresh_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			CmbCutomGroup.Focus();
		}
		if (e.Control && e.KeyCode == Keys.A)
		{
			BtnAllocateItems_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.D)
		{
			btnDeAllocate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			if (Conversion.ToString(base.ActiveControl.Tag) == "UnAllocatedGrid")
			{
				((Control)(object)GridAllocated).Focus();
			}
			else
			{
				((Control)(object)GridUnAllocated).Focus();
			}
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		if (Conversion.ToInt(CmbCutomGroup.Value) > 0)
		{
			UnAllocatedGridFill();
			AllocatedGridFill();
		}
		else
		{
			MessageBox.Show("Select Custom Group First");
		}
	}

	private void UnAllocatedGridFill()
	{
		try
		{
			DataTable dtUnAllocat = ClientCustomGroup.ClientCustomGroupUnAllocatedToCustomGroup(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbCutomGroup.Value));
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("Suppliercustomer");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], dtUnAllocat.Rows[i]["SuppliercustomerId"], dtUnAllocat.Rows[i]["Suppliercustomer"]);
				}
				GridUnAllocated.DataSource = dt;
				GridUnAllocated.RetrieveStructure();
				GridUnAllocatedSetting();
			}
			else
			{
				GridUnAllocated.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridUnAllocatedSetting()
	{
		try
		{
			GridUnAllocated.RootTable.Columns["Id"].Visible = false;
			GridUnAllocated.RootTable.Columns["SuppliercustomerId"].Visible = false;
			GridUnAllocated.RootTable.Columns["Suppliercustomer"].Width = 340;
			GridUnAllocated.RootTable.Columns.Add("Select");
			GridUnAllocated.RootTable.Columns["Select"].ActAsSelector = true;
			GridUnAllocated.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridUnAllocated.RootTable.Columns["Select"].Width = 60;
			GridUnAllocated.RootTable.Columns["Select"].Position = 0;
			GridUnAllocated.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllocatedGridFill()
	{
		try
		{
			DataTable dtAllocat = ClientCustomGroup.ClientCustomGroupAllocatedToCustomGroup(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbCutomGroup.Value));
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("SuppliercustomerId");
				dt.Columns.Add("Suppliercustomer");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], dtAllocat.Rows[i]["SuppliercustomerId"], dtAllocat.Rows[i]["Suppliercustomer"]);
				}
				GridAllocated.DataSource = dt;
				GridAllocated.RetrieveStructure();
				GridAllocatedSetting();
			}
			else
			{
				GridAllocated.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridAllocatedSetting()
	{
		try
		{
			GridAllocated.RootTable.Columns["Id"].Visible = false;
			GridAllocated.RootTable.Columns["SuppliercustomerId"].Visible = false;
			GridAllocated.RootTable.Columns["Suppliercustomer"].Width = 340;
			GridAllocated.RootTable.Columns.Add("Select");
			GridAllocated.RootTable.Columns["Select"].ActAsSelector = true;
			GridAllocated.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridAllocated.RootTable.Columns["Select"].Width = 60;
			GridAllocated.RootTable.Columns["Select"].Position = 0;
			GridAllocated.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnAllocateItems_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbCutomGroup.Value) == 0)
			{
				MessageBox.Show("Select Custom Group First");
				CmbCutomGroup.Focus();
				return;
			}
			if (GridUnAllocated.GetCheckedRows().Length == 0)
			{
				throw new Exception("Check Row's first To Allocate");
			}
			Insert(GridUnAllocated, Active: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert(GridEX grd, bool Active)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			ClientCustomGroup obj = new ClientCustomGroup();
			obj.ClientCustomGroupList = new List<ClientCustomGroup>();
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				ClientCustomGroup item = new ClientCustomGroup();
				item.organizationId = UserAccount.OrganizationId;
				item.companyId = UserAccount.CompanyId;
				item.EntryUserId = UserAccount.ID;
				item.ModifyUserId = UserAccount.ID;
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				item.isActive = Active;
				item.CustomGroupId = Conversion.ToInt(CmbCutomGroup.Value);
				item.Id = Conversion.ToInt(r.Cells["Id"].Value);
				item.supplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				obj.ClientCustomGroupList.Add(item);
			}
			ClientCustomGroup.SaveAndUpdate(obj);
			if (Active)
			{
				MessageBox.Show("Selected Record Allocated Successfully");
			}
			else
			{
				MessageBox.Show("Selected Record UnAllocated Successfully");
			}
			UnAllocatedGridFill();
			AllocatedGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDeAllocate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbCutomGroup.Value) == 0)
			{
				MessageBox.Show("Select Custom Group First");
				CmbCutomGroup.Focus();
				return;
			}
			if (GridAllocated.GetCheckedRows().Length == 0)
			{
				throw new Exception("Check Row's first To UnAllocate");
			}
			Insert(GridAllocated, Active: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			CustomGroupBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemAllocationToCustomGroup_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnNew_Click(object sender, EventArgs e)
	{
		try
		{
			GridAllocated.ClearStructure();
			GridUnAllocated.ClearStructure();
			((Control)(object)CmbCutomGroup).Text = "";
			CmbCutomGroup.Focus();
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SDT.frmClientCustomGroup));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GridAllocated = new GridEX();
		this.btnDeAllocate = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.GridUnAllocated = new GridEX();
		this.BtnAllocateItems = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.CmbCutomGroup = new UltraCombo();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnShow = new System.Windows.Forms.Button();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocated).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocated).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCutomGroup).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(802, 619);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocated);
		this.panel4.Controls.Add(this.btnDeAllocate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(400, 97);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(400, 522);
		this.panel4.TabIndex = 80;
		this.GridAllocated.AllowEdit = (InheritableBoolean)2;
		this.GridAllocated.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocated.FilterMode = (FilterMode)2;
		this.GridAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocated.GroupByBoxVisible = false;
		this.GridAllocated.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.GridAllocated).Name = "GridAllocated";
		((ScrollableBase)this.GridAllocated).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocated).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Size = new System.Drawing.Size(400, 498);
		((System.Windows.Forms.Control)(object)this.GridAllocated).TabIndex = 64;
		this.GridAllocated.TotalRow = (InheritableBoolean)1;
		this.GridAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocated).VisualStyle = (VisualStyle)4;
		this.btnDeAllocate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeAllocate.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeAllocate.Image = Architecture.WinApp.Properties.Resources.close_line;
		this.btnDeAllocate.Location = new System.Drawing.Point(371, 0);
		this.btnDeAllocate.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeAllocate.Name = "btnDeAllocate";
		this.btnDeAllocate.Size = new System.Drawing.Size(25, 24);
		this.btnDeAllocate.TabIndex = 5;
		this.btnDeAllocate.UseVisualStyleBackColor = true;
		this.btnDeAllocate.Click += new System.EventHandler(btnDeAllocate_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(400, 24);
		this.label1.TabIndex = 4;
		this.label1.Text = "Allocated (Active)";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocated);
		this.panel3.Controls.Add(this.BtnAllocateItems);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 97);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(400, 522);
		this.panel3.TabIndex = 79;
		this.GridUnAllocated.AllowEdit = (InheritableBoolean)2;
		this.GridUnAllocated.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocated.FilterMode = (FilterMode)2;
		this.GridUnAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocated.GroupByBoxVisible = false;
		this.GridUnAllocated.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Name = "GridUnAllocated";
		((ScrollableBase)this.GridUnAllocated).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocated).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Size = new System.Drawing.Size(400, 498);
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Tag = "UnAllocatedGrid";
		this.GridUnAllocated.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocated).VisualStyle = (VisualStyle)4;
		this.BtnAllocateItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAllocateItems.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnAllocateItems.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAllocateItems.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnAllocateItems.Location = new System.Drawing.Point(371, 0);
		this.BtnAllocateItems.Margin = new System.Windows.Forms.Padding(0);
		this.BtnAllocateItems.Name = "BtnAllocateItems";
		this.BtnAllocateItems.Size = new System.Drawing.Size(25, 24);
		this.BtnAllocateItems.TabIndex = 66;
		this.BtnAllocateItems.UseVisualStyleBackColor = true;
		this.BtnAllocateItems.Click += new System.EventHandler(BtnAllocateItems_Click);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(400, 24);
		this.label4.TabIndex = 3;
		this.label4.Text = "Pending  For Allocation (InActive)";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.BtnShow);
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCutomGroup);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 57);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(802, 40);
		this.panel7.TabIndex = 59;
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(6, 12);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(84, 15);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "Custom Group";
		this.CmbCutomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCutomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCutomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCutomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCutomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbCutomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCutomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCutomGroup).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCutomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCutomGroup).Location = new System.Drawing.Point(91, 6);
		((System.Windows.Forms.Control)(object)this.CmbCutomGroup).Name = "CmbCutomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCutomGroup).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.CmbCutomGroup).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCutomGroup).TextChanged += new System.EventHandler(CmbItemCutomGroup_TextChanged);
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 25);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(802, 32);
		this.panel6.TabIndex = 45;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(2, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(384, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Supplier Customer Allocation To Cutom Group";
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(802, 25);
		this.panel2.TabIndex = 0;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNew, this.btnRefresh });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(802, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(338, 6);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(62, 26);
		this.BtnShow.TabIndex = 17;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.BtnNew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(58, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(802, 619);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmClientCustomGroup";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ItemAllocationToCustomGroup_FormClosing);
		base.Load += new System.EventHandler(frmItemsCustomizedGroups_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemsCustomizedGroups_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocated).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocated).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCutomGroup).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
