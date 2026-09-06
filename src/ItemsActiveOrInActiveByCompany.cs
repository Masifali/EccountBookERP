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
using System.Linq;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.BLL.Inventory;
using global::Architecture.Common;
using global::Architecture.Model;
// // using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

public class ItemsActiveOrInActiveByCompany : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ContextMenuStrip contextMenuStrip1;

	private PrintDialog printDialog1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private Panel panel1;

	internal GridEX GridInActiveItems;

	private Panel panel7;

	private Label lbltocompany;

	private Panel panel6;

	private Label label15;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	internal GridEX GridActiveItems;

	internal UltraCombo cmbToCompany;

	private Panel panel4;

	private Panel panel3;

	private Label label4;

	private Label label1;

	private Button btnInActive;

	private Panel panel8;

	private Panel panel5;

	private Button BtnActive;

	private Button btnShow;

	private ToolStripButton BtnNew;

	private ToolStripButton btnShortCutKey;

	public ItemsActiveOrInActiveByCompany(UserAccount puserAccount)
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
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(860, 400);
			companytofill();
			cmbToCompany.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void companytofill()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbToCompany).Enabled = true;
			List<Company> lst = Conversion.ConvertDataTableToList<Company>(Company.GetAlldt(new Company
			{
				OrgCompanyTypeId = UserAccount.OrganizationId
			}));
			if (lst.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("CompanyName");
				for (int i = 0; i < lst.Count; i++)
				{
					dt.Rows.Add(lst[i].Id, lst[i].CompName);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbToCompany, "Id", "CompanyName", "Company Name", ZeroIndex: false);
				}
				List<Company> lstComp = lst.Where((Company x) => x.Id == UserAccount.CompanyId).ToList();
				if (lstComp.Count > 0 && lstComp[0].CompType == "Branch")
				{
					((Control)(object)cmbToCompany).Enabled = false;
					cmbToCompany.Value = UserAccount.CompanyId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			companytofill();
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
			GridActiveItems.DataSource = null;
			GridInActiveItems.DataSource = null;
			cmbToCompany.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemsActiveOrInActiveByCompany_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbToCompany.Value) > 0)
			{
				InActiveItemsGridFill();
				ActiveItemsGridFill();
				return;
			}
			throw new Exception("Select Company First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InActiveItemsGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			DataTable dtUnAllocat = ItemAllocation.ReadAllItemsFromAllocationWithStatusByCompanyId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = Conversion.ToInt(cmbToCompany.Value),
				ItemStatus = false
			});
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], dtUnAllocat.Rows[i]["ItemName"]);
				}
				GridInActiveItems.DataSource = dt;
				GridInActiveItems.RetrieveStructure();
				GridInActiveItemsSetting();
			}
			else
			{
				GridInActiveItems.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridInActiveItemsSetting()
	{
		try
		{
			GridInActiveItems.RootTable.Columns["ItemId"].Visible = false;
			GridInActiveItems.RootTable.Columns["ItemName"].Width = 360;
			GridInActiveItems.RootTable.Columns.Add("Select");
			GridInActiveItems.RootTable.Columns["Select"].ActAsSelector = true;
			GridInActiveItems.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridInActiveItems.RootTable.Columns["Select"].Width = 60;
			GridInActiveItems.RootTable.Columns["Select"].Position = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ActiveItemsGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			DataTable dtAllocat = ItemAllocation.ReadAllItemsFromAllocationWithStatusByCompanyId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = Conversion.ToInt(cmbToCompany.Value),
				ItemStatus = true
			});
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], dtAllocat.Rows[i]["ItemName"]);
				}
				GridActiveItems.DataSource = dt;
				GridActiveItems.RetrieveStructure();
				GridActiveItemsSetting();
			}
			else
			{
				GridActiveItems.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridActiveItemsSetting()
	{
		try
		{
			GridActiveItems.RootTable.Columns["ItemId"].Visible = false;
			GridActiveItems.RootTable.Columns["ItemName"].Width = 350;
			GridActiveItems.RootTable.Columns.Add("Select");
			GridActiveItems.RootTable.Columns["Select"].ActAsSelector = true;
			GridActiveItems.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridActiveItems.RootTable.Columns["Select"].Width = 60;
			GridActiveItems.RootTable.Columns["Select"].Position = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnActiveItems_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbToCompany.Value) == 0)
			{
				MessageBox.Show("Select Company First");
				cmbToCompany.Focus();
				return;
			}
			if (GridInActiveItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Active Items");
			}
			string ItemIds = "";
			GridEXRow[] checkedRows = GridInActiveItems.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				ItemIds = ItemIds + Conversion.ToString(r.Cells["ItemId"].Value) + ",";
			}
			ItemAllocation.UpdateItemsStatusInAllocation(UserAccount.OrganizationId, Conversion.ToInt(cmbToCompany.Value), ItemIds, true);
			MessageBox.Show("Items Active Successfully");
			InActiveItemsGridFill();
			ActiveItemsGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnInActive_Click(object sender, EventArgs e)
	{
		try
		{
			if (GridActiveItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To InActive Items");
			}
			if (Conversion.ToInt(cmbToCompany.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to InActive Items?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ItemIds = "";
					GridEXRow[] checkedRows = GridActiveItems.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ItemIds = ItemIds + Conversion.ToString(r.Cells["ItemId"].Value) + ",";
					}
					ItemAllocation.UpdateItemsStatusInAllocation(UserAccount.OrganizationId, Conversion.ToInt(cmbToCompany.Value), ItemIds, false);
					MessageBox.Show("Items InActive Successfully");
					InActiveItemsGridFill();
					ActiveItemsGridFill();
				}
				return;
			}
			throw new Exception("Select Company First To InActive Items");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AcfrmAcAllocation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNew_Click(null, null);
			}
			if (e.Control && (e.KeyCode == Keys.A || e.KeyCode == Keys.S))
			{
				btnInActive_Click(null, null);
			}
			if (e.Control && (e.KeyCode == Keys.D || e.KeyCode == Keys.U))
			{
				btnInActive_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbToCompany.Focus();
			}
			if (e.Control && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Down))
			{
				((Control)(object)GridActiveItems).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				((Control)(object)GridInActiveItems).Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+A", "For Active");
			dt.Rows.Add("Ctrl+D", "For InActive");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Company Combo");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On InActive Grid");
			dt.Rows.Add("Ctrl+ArrowRight", "For Focus On Active Grid");
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
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ItemsActiveOrInActiveByCompany));
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.printDialog1 = new System.Windows.Forms.PrintDialog();
		this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.GridActiveItems = new GridEX();
		this.btnInActive = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GridInActiveItems = new GridEX();
		this.BtnActive = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbToCompany = new UltraCombo();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridActiveItems).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridInActiveItems).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbToCompany).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(844, 361);
		this.panel1.TabIndex = 37;
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Controls.Add(this.btnInActive);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(425, 92);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(414, 269);
		this.panel4.TabIndex = 65;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.GridActiveItems);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 31);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(414, 238);
		this.panel8.TabIndex = 67;
		this.GridActiveItems.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridActiveItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridActiveItems.FilterMode = (FilterMode)2;
		this.GridActiveItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridActiveItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridActiveItems.GroupByBoxVisible = false;
		this.GridActiveItems.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridActiveItems).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridActiveItems).Name = "GridActiveItems";
		((ScrollableBase)this.GridActiveItems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridActiveItems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridActiveItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridActiveItems).Size = new System.Drawing.Size(414, 238);
		((System.Windows.Forms.Control)(object)this.GridActiveItems).TabIndex = 63;
		this.GridActiveItems.TotalRow = (InheritableBoolean)1;
		this.GridActiveItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridActiveItems).VisualStyle = (VisualStyle)4;
		this.btnInActive.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnInActive.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnInActive.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnInActive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnInActive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnInActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnInActive.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnInActive.Location = new System.Drawing.Point(384, 3);
		this.btnInActive.Margin = new System.Windows.Forms.Padding(0);
		this.btnInActive.Name = "btnInActive";
		this.btnInActive.Size = new System.Drawing.Size(25, 25);
		this.btnInActive.TabIndex = 4;
		this.btnInActive.UseVisualStyleBackColor = true;
		this.btnInActive.Click += new System.EventHandler(btnInActive_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(414, 31);
		this.label1.TabIndex = 3;
		this.label1.Text = "Active Items";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.BtnActive);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 92);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(425, 269);
		this.panel3.TabIndex = 64;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridInActiveItems);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 31);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(425, 238);
		this.panel5.TabIndex = 66;
		this.GridInActiveItems.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridInActiveItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridInActiveItems.FilterMode = (FilterMode)2;
		this.GridInActiveItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridInActiveItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridInActiveItems.GroupByBoxVisible = false;
		this.GridInActiveItems.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridInActiveItems).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridInActiveItems).Name = "GridInActiveItems";
		((ScrollableBase)this.GridInActiveItems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridInActiveItems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridInActiveItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridInActiveItems).Size = new System.Drawing.Size(425, 238);
		((System.Windows.Forms.Control)(object)this.GridInActiveItems).TabIndex = 62;
		this.GridInActiveItems.TotalRow = (InheritableBoolean)1;
		this.GridInActiveItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridInActiveItems).VisualStyle = (VisualStyle)4;
		this.BtnActive.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnActive.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnActive.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnActive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnActive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnActive.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnActive.Location = new System.Drawing.Point(397, 3);
		this.BtnActive.Margin = new System.Windows.Forms.Padding(0);
		this.BtnActive.Name = "BtnActive";
		this.BtnActive.Size = new System.Drawing.Size(25, 25);
		this.BtnActive.TabIndex = 65;
		this.BtnActive.UseVisualStyleBackColor = true;
		this.BtnActive.Click += new System.EventHandler(BtnActiveItems_Click);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(425, 31);
		this.label4.TabIndex = 2;
		this.label4.Text = "InActive Items";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.btnShow);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbToCompany);
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 54);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(844, 38);
		this.panel7.TabIndex = 58;
		this.btnShow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btnShow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(366, 6);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(56, 26);
		this.btnShow.TabIndex = 34;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbToCompany.AllowNull = (DefaultableBoolean)1;
		this.cmbToCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbToCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbToCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbToCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbToCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbToCompany).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbToCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbToCompany).Location = new System.Drawing.Point(99, 6);
		((System.Windows.Forms.Control)(object)this.cmbToCompany).Name = "cmbToCompany";
		((System.Windows.Forms.Control)(object)this.cmbToCompany).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.cmbToCompany).TabIndex = 33;
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(6, 12);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(93, 15);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "Company Name";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 29);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(844, 25);
		this.panel6.TabIndex = 44;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(209, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Items Active Or InActive";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(844, 29);
		this.panel2.TabIndex = 45;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNew, this.btnRefresh, this.btnShortCutKey });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(844, 27);
		this.toolStrip1.TabIndex = 55;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(844, 361);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "ItemsActiveOrInActiveByCompany";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ItemsActiveOrInActiveByCompany_FormClosed);
		base.Load += new System.EventHandler(AcfrmAcAllocation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmAcAllocation_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridActiveItems).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridInActiveItems).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbToCompany).EndInit();
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
