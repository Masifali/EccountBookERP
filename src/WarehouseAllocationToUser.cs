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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.Common;
using global::Architecture.Model;
// // using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

public class WarehouseAllocationToUser : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ContextMenuStrip contextMenuStrip1;

	private PrintDialog printDialog1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private Panel panel1;

	internal GridEX GridUnAllocatedWarehouses;

	private Panel panel7;

	private Label lbltocompany;

	private Panel panel6;

	private Label label15;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	internal GridEX GridAllocatedWarehouses;

	internal UltraCombo CmbUser;

	private Panel panel4;

	private Panel panel3;

	private Label label4;

	private Label label1;

	private Button btnDeAllocate;

	private Panel panel8;

	private Panel panel5;

	private Button BtnAllocateItems;

	private Button btnShow;

	public WarehouseAllocationToUser(UserAccount puserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void AcfrmAcAllocation_Load(object sender, EventArgs e)
	{
		try
		{
			UsersFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UsersFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = UserAccount.ReadAll(new UserAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbUser, "ID", "UserName", "User Name", false);
				((UltraGridBase)CmbUser).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbUser.Value) > 0)
			{
				UnAllocatedWarehouseGridFill();
				AllocatedWarehouseGridFill();
				return;
			}
			throw new Exception("Select Company First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UnAllocatedWarehouseGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			DataTable dtUnAllocat = UserWarehouse.GetAllocatedAndUnAllocatedItems(new UserWarehouse
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserAccountId = Conversion.ToInt(CmbUser.Value),
				UserWarehuseId = 1
			});
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("WarehouseId");
				dt.Columns.Add("Warehosue");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], dtUnAllocat.Rows[i]["WareHouseName"]);
				}
				GridUnAllocatedWarehouses.DataSource = dt;
				GridUnAllocatedWarehouses.RetrieveStructure();
				GridUnAllocatedItemsSetting();
			}
			else
			{
				GridUnAllocatedWarehouses.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridUnAllocatedItemsSetting()
	{
		try
		{
			GridUnAllocatedWarehouses.RootTable.Columns["WarehouseId"].Visible = false;
			GridUnAllocatedWarehouses.RootTable.Columns["Warehosue"].Width = 360;
			GridUnAllocatedWarehouses.RootTable.Columns.Add("Select");
			GridUnAllocatedWarehouses.RootTable.Columns["Select"].ActAsSelector = true;
			GridUnAllocatedWarehouses.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridUnAllocatedWarehouses.RootTable.Columns["Select"].Width = 60;
			GridUnAllocatedWarehouses.RootTable.Columns["Select"].Position = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllocatedWarehouseGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			DataTable dtAllocat = UserWarehouse.GetAllocatedAndUnAllocatedItems(new UserWarehouse
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserAccountId = Conversion.ToInt(CmbUser.Value),
				UserWarehuseId = 2
			});
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("WarehouseId");
				dt.Columns.Add("Warehouse");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], dtAllocat.Rows[i]["WareHouseName"]);
				}
				GridAllocatedWarehouses.DataSource = dt;
				GridAllocatedWarehouses.RetrieveStructure();
				GridAllocatedItemsSetting();
			}
			else
			{
				GridAllocatedWarehouses.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridAllocatedItemsSetting()
	{
		try
		{
			GridAllocatedWarehouses.RootTable.Columns["WarehouseId"].Visible = false;
			GridAllocatedWarehouses.RootTable.Columns["Warehouse"].Width = 350;
			GridAllocatedWarehouses.RootTable.Columns.Add("Select");
			GridAllocatedWarehouses.RootTable.Columns["Select"].ActAsSelector = true;
			GridAllocatedWarehouses.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridAllocatedWarehouses.RootTable.Columns["Select"].Width = 60;
			GridAllocatedWarehouses.RootTable.Columns["Select"].Position = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAllocateItems_Click(object sender, EventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbUser.Value) == 0)
			{
				MessageBox.Show("Select User First");
				CmbUser.Focus();
				return;
			}
			if (GridUnAllocatedWarehouses.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Allocate Warehouse's");
			}
			UserWarehouse obj = new UserWarehouse();
			obj.UserWarehousesList = new List<UserWarehouse>();
			GridEXRow[] checkedRows = GridUnAllocatedWarehouses.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				UserWarehouse item = new UserWarehouse();
				item.BranchId = UserAccount.BranchesId;
				item.CompanyId = UserAccount.CompanyId;
				item.OrganizationId = UserAccount.OrganizationId;
				item.EntryUserId = UserAccount.ID;
				item.ModifyUserId = UserAccount.ID;
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				item.UserAccountId = Conversion.ToInt(CmbUser.Value);
				item.InvWareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				item.IsActive = true;
				obj.UserWarehousesList.Add(item);
			}
			UserWarehouse.Save(obj);
			MessageBox.Show("Warehouses's Allocated Successfully");
			UnAllocatedWarehouseGridFill();
			AllocatedWarehouseGridFill();
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
			if (GridAllocatedWarehouses.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Un-Allocate Warehouse's");
			}
			if (Conversion.ToInt(CmbUser.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to UnAllocate Warehouses's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = GridAllocatedWarehouses.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["WarehouseId"].Value) + ",";
					}
					UserWarehouse.DeleteById(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbUser.Value), ids);
					MessageBox.Show("Item's UnAllocated Successfully");
					UnAllocatedWarehouseGridFill();
					AllocatedWarehouseGridFill();
				}
				return;
			}
			throw new Exception("Select User First To Un-Allocate Warehouses's");
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
			UsersFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcfrmAcAllocation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnRefresh_Click(null, null);
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

	private void btnclose_Click(object sender, EventArgs e)
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
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
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.printDialog1 = new System.Windows.Forms.PrintDialog();
		this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.GridAllocatedWarehouses = new GridEX();
		this.btnDeAllocate = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GridUnAllocatedWarehouses = new GridEX();
		this.BtnAllocateItems = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbUser = new UltraCombo();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedWarehouses).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedWarehouses).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUser).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.printDialog1.UseEXDialog = true;
		this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.dataGridViewTextBoxColumn8.HeaderText = "✓";
		this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(844, 594);
		this.panel1.TabIndex = 37;
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Controls.Add(this.btnDeAllocate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(425, 109);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(414, 485);
		this.panel4.TabIndex = 65;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocatedWarehouses);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 31);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(414, 454);
		this.panel8.TabIndex = 67;
		this.GridAllocatedWarehouses.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridAllocatedWarehouses).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocatedWarehouses.FilterMode = (FilterMode)2;
		this.GridAllocatedWarehouses.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocatedWarehouses).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocatedWarehouses.GroupByBoxVisible = false;
		this.GridAllocatedWarehouses.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedWarehouses).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridAllocatedWarehouses).Name = "GridAllocatedWarehouses";
		((ScrollableBase)this.GridAllocatedWarehouses).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocatedWarehouses).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocatedWarehouses).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedWarehouses).Size = new System.Drawing.Size(414, 454);
		((System.Windows.Forms.Control)(object)this.GridAllocatedWarehouses).TabIndex = 63;
		this.GridAllocatedWarehouses.TotalRow = (InheritableBoolean)1;
		this.GridAllocatedWarehouses.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocatedWarehouses).VisualStyle = (VisualStyle)4;
		this.btnDeAllocate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeAllocate.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeAllocate.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeAllocate.Location = new System.Drawing.Point(384, 3);
		this.btnDeAllocate.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeAllocate.Name = "btnDeAllocate";
		this.btnDeAllocate.Size = new System.Drawing.Size(25, 25);
		this.btnDeAllocate.TabIndex = 4;
		this.btnDeAllocate.UseVisualStyleBackColor = true;
		this.btnDeAllocate.Click += new System.EventHandler(btnDeAllocate_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(414, 31);
		this.label1.TabIndex = 3;
		this.label1.Text = "Allocated Warehouses";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.BtnAllocateItems);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 109);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(425, 485);
		this.panel3.TabIndex = 64;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedWarehouses);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 31);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(425, 454);
		this.panel5.TabIndex = 66;
		this.GridUnAllocatedWarehouses.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedWarehouses).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocatedWarehouses.FilterMode = (FilterMode)2;
		this.GridUnAllocatedWarehouses.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedWarehouses).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocatedWarehouses.GroupByBoxVisible = false;
		this.GridUnAllocatedWarehouses.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedWarehouses).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedWarehouses).Name = "GridUnAllocatedWarehouses";
		((ScrollableBase)this.GridUnAllocatedWarehouses).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocatedWarehouses).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocatedWarehouses).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedWarehouses).Size = new System.Drawing.Size(425, 454);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedWarehouses).TabIndex = 62;
		this.GridUnAllocatedWarehouses.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocatedWarehouses.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocatedWarehouses).VisualStyle = (VisualStyle)4;
		this.BtnAllocateItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAllocateItems.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnAllocateItems.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAllocateItems.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnAllocateItems.Location = new System.Drawing.Point(397, 3);
		this.BtnAllocateItems.Margin = new System.Windows.Forms.Padding(0);
		this.BtnAllocateItems.Name = "BtnAllocateItems";
		this.BtnAllocateItems.Size = new System.Drawing.Size(25, 25);
		this.BtnAllocateItems.TabIndex = 65;
		this.BtnAllocateItems.UseVisualStyleBackColor = true;
		this.BtnAllocateItems.Click += new System.EventHandler(BtnAllocateItems_Click);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(425, 31);
		this.label4.TabIndex = 2;
		this.label4.Text = "Pending Warehouses For Allocation";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.btnShow);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUser);
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 64);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(844, 45);
		this.panel7.TabIndex = 58;
		this.btnShow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btnShow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(339, 8);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(74, 26);
		this.btnShow.TabIndex = 34;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.CmbUser.AllowNull = (DefaultableBoolean)1;
		this.CmbUser.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUser.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUser.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUser).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbUser).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUser).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUser).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUser).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbUser).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUser).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbUser).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUser).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUser).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbUser).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUser).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUser).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUser).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUser.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUser).Location = new System.Drawing.Point(84, 8);
		((System.Windows.Forms.Control)(object)this.CmbUser).Name = "CmbUser";
		((System.Windows.Forms.Control)(object)this.CmbUser).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.CmbUser).TabIndex = 33;
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(15, 14);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(65, 15);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "User Name";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 29);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(844, 35);
		this.panel6.TabIndex = 44;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(13, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(239, 24);
		this.label15.TabIndex = 0;
		this.label15.Text = "User Warehouse Allocation";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(844, 29);
		this.panel2.TabIndex = 45;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(844, 27);
		this.toolStrip1.TabIndex = 55;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(844, 594);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "WarehouseAllocationToUser";
		this.Text = "Warehouse Allocation";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(AcfrmAcAllocation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmAcAllocation_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedWarehouses).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedWarehouses).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUser).EndInit();
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
