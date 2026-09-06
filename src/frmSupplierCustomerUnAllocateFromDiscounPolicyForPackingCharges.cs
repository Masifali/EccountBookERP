using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
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
namespace Architecture.WinApp.Inventory_Definition
{

public class frmSupplierCustomerUnAllocateFromDiscounPolicyForPackingCharges : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbDiscountType;

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

	internal GridEX GridUnAllocatedItems;

	internal GridEX GridAllocatedItems;

	private ToolStripButton btnshortcutkeys;

	private Button btnshow;

	private Label label36;

	public frmSupplierCustomerUnAllocateFromDiscounPolicyForPackingCharges(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmSupplierCustomerAllocateToDiscounTypeForPackingCharges_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(820, 450);
			DiscountPolicyFill();
			CmbDiscountType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DiscountPolicyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCG = DiscountPolicy.GetDiscountPolicyForCustomers(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCG.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("PolicyName");
				dt.Columns.Add("DiscountType");
				dt.Columns.Add("DiscountRate");
				dt.Columns.Add("EffectiveDate");
				for (int i = 0; i < dtCG.Rows.Count; i++)
				{
					dt.Rows.Add(dtCG.Rows[i]["Id"], dtCG.Rows[i]["PolicyName"], dtCG.Rows[i]["DiscountType"], dtCG.Rows[i]["DiscountRate"], dtCG.Rows[i]["EffectiveDate"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbDiscountType, "Id", "PolicyName", "Discount Policy", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemCutomGroup_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ShowData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			ShowData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ShowData()
	{
		try
		{
			if (Conversion.ToInt(CmbDiscountType.Value) > 0)
			{
				UnAllocatedItemsGridFill();
				AllocatedItemsGridFill();
			}
			else
			{
				MessageBox.Show("Select Discount Type First");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UnAllocatedItemsGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			DataTable dtUnAllocat = SupplierCustomerUnAllocateFromDiscounPolicy.GetAllocatedAndUnAllocatedSupplierCustomerIds(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DiscountPolicyId = Conversion.ToInt(CmbDiscountType.Value),
				ActionId = 1
			});
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierCustomer");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["SupplierCustomerId"], dtUnAllocat.Rows[i]["SupplierCustomer"]);
				}
				GridUnAllocatedItems.DataSource = dt;
				GridUnAllocatedItems.RetrieveStructure();
				GridUnAllocatedItemsSetting();
			}
			else
			{
				GridUnAllocatedItems.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridUnAllocatedItemsSetting()
	{
		try
		{
			GridUnAllocatedItems.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridUnAllocatedItems.RootTable.Columns["SupplierCustomer"].Width = 340;
			GridUnAllocatedItems.RootTable.Columns.Add("Select");
			GridUnAllocatedItems.RootTable.Columns["Select"].ActAsSelector = true;
			GridUnAllocatedItems.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridUnAllocatedItems.RootTable.Columns["Select"].Width = 60;
			GridUnAllocatedItems.RootTable.Columns["Select"].Position = 0;
			GridUnAllocatedItems.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllocatedItemsGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			DataTable dtAllocat = SupplierCustomerUnAllocateFromDiscounPolicy.GetAllocatedAndUnAllocatedSupplierCustomerIds(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DiscountPolicyId = Conversion.ToInt(CmbDiscountType.Value),
				ActionId = 2
			});
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("SupplierCustomer");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["SupplierCustomerId"], dtAllocat.Rows[i]["SupplierCustomer"]);
				}
				GridAllocatedItems.DataSource = dt;
				GridAllocatedItems.RetrieveStructure();
				GridAllocatedItemsSetting();
			}
			else
			{
				GridAllocatedItems.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridAllocatedItemsSetting()
	{
		try
		{
			GridAllocatedItems.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridAllocatedItems.RootTable.Columns["SupplierCustomer"].Width = 340;
			GridAllocatedItems.RootTable.Columns.Add("Select");
			GridAllocatedItems.RootTable.Columns["Select"].ActAsSelector = true;
			GridAllocatedItems.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridAllocatedItems.RootTable.Columns["Select"].Width = 60;
			GridAllocatedItems.RootTable.Columns["Select"].Position = 0;
			GridAllocatedItems.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			if (Conversion.ToInt(CmbDiscountType.Value) == 0)
			{
				MessageBox.Show("Select Discount Policy First");
				CmbDiscountType.Focus();
				return;
			}
			if (GridUnAllocatedItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To UnAllocate SupplierCustomer");
			}
			SupplierCustomerUnAllocateFromDiscounPolicy obj = new SupplierCustomerUnAllocateFromDiscounPolicy();
			obj.SupplierCustomerUnAllocateFromDiscounPolicyList = new List<SupplierCustomerUnAllocateFromDiscounPolicy>();
			GridEXRow[] checkedRows = GridUnAllocatedItems.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				SupplierCustomerUnAllocateFromDiscounPolicy item = new SupplierCustomerUnAllocateFromDiscounPolicy();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.EntryUserId = UserAccount.ID;
				item.ModifyUserId = UserAccount.ID;
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				item.DiscountPolicyId = Conversion.ToInt(CmbDiscountType.Value);
				item.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				obj.SupplierCustomerUnAllocateFromDiscounPolicyList.Add(item);
			}
			SupplierCustomerUnAllocateFromDiscounPolicy.Save(obj);
			MessageBox.Show("SupplierCustomer UnAllocated Successfully");
			UnAllocatedItemsGridFill();
			AllocatedItemsGridFill();
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
			if (Conversion.ToInt(CmbDiscountType.Value) == 0)
			{
				MessageBox.Show("Select Discount Policy First");
				CmbDiscountType.Focus();
				return;
			}
			if (GridAllocatedItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Allocate SupplierCustomers");
			}
			if (Conversion.ToInt(CmbDiscountType.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Allocate SupplierCustomer's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = GridAllocatedItems.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["SupplierCustomerId"].Value) + ",";
					}
					SupplierCustomerUnAllocateFromDiscounPolicy.DeleteById(Conversion.ToInt(CmbDiscountType.Value), ids);
					MessageBox.Show("SupplierCustomer Allocated Successfully");
					UnAllocatedItemsGridFill();
					AllocatedItemsGridFill();
				}
				return;
			}
			throw new Exception("Select Discount Policy First To Allocate SupplierCustomer's");
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
			DiscountPolicyFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSupplierCustomerAllocateToDiscounTypeForPackingCharges_FormClosing(object sender, FormClosingEventArgs e)
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
			CmbDiscountType.Focus();
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
				((Control)(object)GridAllocatedItems).Focus();
			}
			else
			{
				((Control)(object)GridUnAllocatedItems).Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Right)
		{
			if (((Control)(object)GridAllocatedItems).Focused)
			{
				((Control)(object)GridUnAllocatedItems).Focus();
			}
			else
			{
				((Control)(object)GridAllocatedItems).Focus();
			}
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+A", "For Press Allocate Button to Allocate Items");
			dt.Rows.Add("Ctrl+D", "For Press UnAllocate Button to UnAllocate Items");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on Discount Policy");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
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
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GridAllocatedItems = new GridEX();
		this.btnDeAllocate = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.GridUnAllocatedItems = new GridEX();
		this.BtnAllocateItems = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label36 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.CmbDiscountType = new UltraCombo();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedItems).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedItems).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(802, 586);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocatedItems);
		this.panel4.Controls.Add(this.btnDeAllocate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(400, 91);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(400, 495);
		this.panel4.TabIndex = 80;
		this.GridAllocatedItems.AllowEdit = (InheritableBoolean)2;
		this.GridAllocatedItems.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocatedItems.FilterMode = (FilterMode)2;
		this.GridAllocatedItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocatedItems.GroupByBoxVisible = false;
		this.GridAllocatedItems.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Name = "GridAllocatedItems";
		((ScrollableBase)this.GridAllocatedItems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocatedItems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocatedItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Size = new System.Drawing.Size(400, 467);
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).TabIndex = 0;
		this.GridAllocatedItems.TotalRow = (InheritableBoolean)1;
		this.GridAllocatedItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocatedItems).VisualStyle = (VisualStyle)4;
		this.btnDeAllocate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeAllocate.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeAllocate.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeAllocate.Location = new System.Drawing.Point(371, 1);
		this.btnDeAllocate.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeAllocate.Name = "btnDeAllocate";
		this.btnDeAllocate.Size = new System.Drawing.Size(25, 25);
		this.btnDeAllocate.TabIndex = 1;
		this.btnDeAllocate.UseVisualStyleBackColor = true;
		this.btnDeAllocate.Click += new System.EventHandler(btnDeAllocate_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(400, 28);
		this.label1.TabIndex = 4;
		this.label1.Text = "Pending Supplier/Customer For Allocation";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems);
		this.panel3.Controls.Add(this.BtnAllocateItems);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 91);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(400, 495);
		this.panel3.TabIndex = 79;
		this.GridUnAllocatedItems.AllowEdit = (InheritableBoolean)2;
		this.GridUnAllocatedItems.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocatedItems.FilterMode = (FilterMode)2;
		this.GridUnAllocatedItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocatedItems.GroupByBoxVisible = false;
		this.GridUnAllocatedItems.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Name = "GridUnAllocatedItems";
		((ScrollableBase)this.GridUnAllocatedItems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocatedItems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocatedItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Size = new System.Drawing.Size(400, 467);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Tag = "UnAllocatedGrid";
		this.GridUnAllocatedItems.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocatedItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocatedItems).VisualStyle = (VisualStyle)4;
		this.BtnAllocateItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAllocateItems.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnAllocateItems.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAllocateItems.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnAllocateItems.Location = new System.Drawing.Point(371, 2);
		this.BtnAllocateItems.Margin = new System.Windows.Forms.Padding(0);
		this.BtnAllocateItems.Name = "BtnAllocateItems";
		this.BtnAllocateItems.Size = new System.Drawing.Size(25, 25);
		this.BtnAllocateItems.TabIndex = 1;
		this.BtnAllocateItems.UseVisualStyleBackColor = true;
		this.BtnAllocateItems.Click += new System.EventHandler(BtnAllocateItems_Click);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(400, 28);
		this.label4.TabIndex = 3;
		this.label4.Text = "Allocated Supplier/Customer";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.label36);
		this.panel7.Controls.Add(this.btnshow);
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountType);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 57);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(802, 34);
		this.panel7.TabIndex = 0;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(92, 9);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(14, 17);
		this.label36.TabIndex = 573;
		this.label36.Text = "*";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(346, 4);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(54, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(7, 10);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(90, 15);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "Discount Policy";
		this.CmbDiscountType.AllowNull = (DefaultableBoolean)1;
		this.CmbDiscountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDiscountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Location = new System.Drawing.Point(107, 4);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Name = "CmbDiscountType";
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Leave += new System.EventHandler(CmbItemCutomGroup_TextChanged);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 25);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(802, 32);
		this.panel6.TabIndex = 45;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(9, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(433, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Supplier/Customer Un_Allocate From Discount Type";
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(802, 25);
		this.panel2.TabIndex = 0;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnshortcutkeys });
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
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(802, 586);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmSupplierCustomerAllocateToDiscounTypeForPackingCharges";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSupplierCustomerAllocateToDiscounTypeForPackingCharges_FormClosing);
		base.Load += new System.EventHandler(frmSupplierCustomerAllocateToDiscounTypeForPackingCharges_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemsCustomizedGroups_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedItems).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedItems).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).EndInit();
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
