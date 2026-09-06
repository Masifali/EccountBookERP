using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Inventory_Definition;

public class frmItemsReorderSchedule : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtUOM = new DataTable();

	private int RecId;

	private bool updateMod;

	private int success;

	private IContainer components;

	internal GridEX grdfrm;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Label label2;

	private TextBox MinQty;

	private Label label1;

	internal UltraCombo ItemId;

	private Label label3;

	internal UltraCombo BranchesId;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Label label7;

	private TextBox ReOrderPoint;

	private Label label6;

	private TextBox MaxQty;

	private Label label5;

	private TextBox OptimalQty;

	private Label label4;

	internal UltraCombo ItemUomId;

	private DateTimePicker EffectiveDate;

	private Label label8;

	public frmItemsReorderSchedule(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmItemsReorderSchedule_Load(object sender, EventArgs e)
	{
		GridBind();
		BranchesFill();
		ItemFill();
		UOMFIll();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				ItemsReorderSchedule itemsReorder = new ItemsReorderSchedule();
				itemsReorder.Id = Conversion.ToInt((object)RecId);
				itemsReorder.BranchesId = Conversion.ToInt(BranchesId.Value);
				itemsReorder.EffectiveDate = Conversion.ToDateTime((object)EffectiveDate.Value);
				itemsReorder.ItemId = Conversion.ToInt(ItemId.Value);
				itemsReorder.ItemUomId = Conversion.ToInt(ItemUomId.Value);
				itemsReorder.MinQty = Conversion.ToDouble((object)MinQty.Text);
				itemsReorder.OptimalQty = Conversion.ToDouble((object)OptimalQty.Text);
				itemsReorder.MaxQty = Conversion.ToDouble((object)MaxQty.Text);
				itemsReorder.ReOrderPoint = Conversion.ToDouble((object)ReOrderPoint.Text);
				success = ItemsReorderSchedule.Save(itemsReorder);
				if (success > 0)
				{
					MessageBox.Show("Update Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				ItemsReorderSchedule itemsReorder = new ItemsReorderSchedule();
				itemsReorder.BranchesId = Conversion.ToInt(BranchesId.Value);
				itemsReorder.EffectiveDate = Conversion.ToDateTime((object)EffectiveDate.Value);
				itemsReorder.ItemId = Conversion.ToInt(ItemId.Value);
				itemsReorder.ItemUomId = Conversion.ToInt(ItemUomId.Value);
				itemsReorder.MinQty = Conversion.ToDouble((object)MinQty.Text);
				itemsReorder.OptimalQty = Conversion.ToDouble((object)OptimalQty.Text);
				itemsReorder.MaxQty = Conversion.ToDouble((object)MaxQty.Text);
				itemsReorder.ReOrderPoint = Conversion.ToDouble((object)ReOrderPoint.Text);
				success = ItemsReorderSchedule.Save(itemsReorder);
				if (success > 0)
				{
					MessageBox.Show("Save Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		new ItemsReorderSchedule();
		DataTable dtgrid = ItemsReorderSchedule.Getall();
		if (dtgrid.Rows.Count > 0)
		{
			DataTable dtHistory = new DataTable();
			dtHistory.Columns.Add("Id");
			dtHistory.Columns.Add("BranchesId");
			dtHistory.Columns.Add("EffectiveDate");
			dtHistory.Columns.Add("ItemId");
			dtHistory.Columns.Add("ItemUomId");
			dtHistory.Columns.Add("MinQty");
			dtHistory.Columns.Add("MaxQty");
			dtHistory.Columns.Add("OptimalQty");
			dtHistory.Columns.Add("ReOrderPoint");
			for (int i = 0; i < dtgrid.Rows.Count; i++)
			{
				dtHistory.Rows.Add(dtgrid.Rows[i]["Id"], dtgrid.Rows[i]["BranchName"], Conversion.ToDateTime(dtgrid.Rows[i]["EffectiveDate"]).ToString("dd-MMM-yyyy"), dtgrid.Rows[i]["itemName"], dtgrid.Rows[i]["ItemUOM"], dtgrid.Rows[i]["MinQty"], dtgrid.Rows[i]["MaxQty"], dtgrid.Rows[i]["OptimalQty"], dtgrid.Rows[i]["ReOrderPoint"]);
			}
			grdfrm.DataSource = dtHistory;
			grdfrm.RetrieveStructure();
			grdSetting();
		}
	}

	public void RetrivedData(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ItemsReorderSchedule itemsReorder = new ItemsReorderSchedule();
			RecId = Id;
			itemsReorder = ItemsReorderSchedule.GetByID(RecId);
			BranchesId.Value = Conversion.ToInt((object)itemsReorder.BranchesId);
			EffectiveDate.Value = Conversion.ToDateTime((object)itemsReorder.EffectiveDate);
			ItemId.Value = Conversion.ToInt((object)itemsReorder.ItemId);
			ItemUomId.Value = Conversion.ToInt((object)itemsReorder.ItemUomId);
			MinQty.Text = Conversion.ToString((object)itemsReorder.MinQty);
			OptimalQty.Text = Conversion.ToString((object)itemsReorder.OptimalQty);
			MaxQty.Text = Conversion.ToString((object)itemsReorder.MaxQty);
			ReOrderPoint.Text = Conversion.ToString((object)itemsReorder.ReOrderPoint);
			btnsave.Visible = false;
			btnupdate.Visible = true;
			updateMod = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Reset()
	{
		try
		{
			((Control)(object)BranchesId).Text = string.Empty;
			EffectiveDate.Value = DateTime.Now;
			((Control)(object)ItemId).Text = string.Empty;
			((Control)(object)ItemUomId).Text = string.Empty;
			MinQty.Text = string.Empty;
			OptimalQty.Text = string.Empty;
			MaxQty.Text = string.Empty;
			ReOrderPoint.Text = string.Empty;
			BranchesId.Focus();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			updateMod = false;
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			DataTable dt = CommonServices.ItemGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, ItemId, "Id", "ItemName", "ItemName", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			DataTable dt = CommonServices.BrancheServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, BranchesId, "Id", "BranchName", "Branch Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMFIll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtUOM = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmItemsReorderSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && !updateMod)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && updateMod)
			{
				btnupdate_Click(null, null);
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

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RetrivedData(Conversion.ToInt((object)grdfrm.CurrentRow.Cells[0].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)BranchesId).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Branche Required");
			BranchesId.Focus();
			return false;
		}
		if (((Control)(object)ItemId).Text.Trim() == string.Empty)
		{
			MessageBox.Show("item Required");
			ItemId.Focus();
			return false;
		}
		if (((Control)(object)ItemUomId).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Item UOM Required");
			ItemUomId.Focus();
			return false;
		}
		return true;
	}

	private void ItemId_Leave(object sender, EventArgs e)
	{
		try
		{
			if (dtUOM.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			for (int i = 0; i < dtUOM.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtUOM.Rows[i]["ItemId"]) == Convert.ToInt32(ItemId.Value))
				{
					dt.Rows.Add(dtUOM.Rows[i]["Id"], dtUOM.Rows[i]["Equivalent"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, ItemUomId, "Id", "UOM", "UOM", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void grdSetting()
	{
		try
		{
			grdfrm.RootTable.Columns["BranchesId"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["EffectiveDate"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["MinQty"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["MaxQty"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["OptimalQty"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ReOrderPoint"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["BranchesId"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["EffectiveDate"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ItemUomId"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["MinQty"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["MaxQty"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["OptimalQty"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ReOrderPoint"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["BranchesId"].Width = 200;
			grdfrm.RootTable.Columns["EffectiveDate"].Width = 150;
			grdfrm.RootTable.Columns["ItemId"].Width = 200;
			grdfrm.RootTable.Columns["ItemUomId"].Width = 100;
			grdfrm.RootTable.Columns["MinQty"].Width = 100;
			grdfrm.RootTable.Columns["MaxQty"].Width = 100;
			grdfrm.RootTable.Columns["OptimalQty"].Width = 100;
			grdfrm.RootTable.Columns["ReOrderPoint"].Width = 100;
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.frmItemsReorderSchedule));
		this.grdfrm = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.ReOrderPoint = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.MaxQty = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.OptimalQty = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.ItemUomId = new UltraCombo();
		this.EffectiveDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.MinQty = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.ItemId = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.BranchesId = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ItemUomId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ItemId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.BranchesId).BeginInit();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.grdfrm.AllowDelete = (InheritableBoolean)1;
		this.grdfrm.ColumnAutoResize = true;
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1101, 292);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 78;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1101, 381);
		this.panel1.TabIndex = 1;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 89);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1101, 292);
		this.panel4.TabIndex = 2;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.ReOrderPoint);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.MaxQty);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.OptimalQty);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.ItemUomId);
		this.panel3.Controls.Add(this.EffectiveDate);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.MinQty);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.ItemId);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.BranchesId);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1101, 64);
		this.panel3.TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(210, 6);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 15);
		this.label8.TabIndex = 27;
		this.label8.Text = "Effective Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1000, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(56, 15);
		this.label7.TabIndex = 26;
		this.label7.Text = "Re-Order";
		this.ReOrderPoint.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ReOrderPoint.Location = new System.Drawing.Point(1000, 25);
		this.ReOrderPoint.Name = "ReOrderPoint";
		this.ReOrderPoint.Size = new System.Drawing.Size(95, 23);
		this.ReOrderPoint.TabIndex = 7;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(902, 6);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(53, 15);
		this.label6.TabIndex = 24;
		this.label6.Text = "Max Qty";
		this.MaxQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.MaxQty.Location = new System.Drawing.Point(902, 25);
		this.MaxQty.Name = "MaxQty";
		this.MaxQty.Size = new System.Drawing.Size(95, 23);
		this.MaxQty.TabIndex = 6;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(804, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(73, 15);
		this.label5.TabIndex = 22;
		this.label5.Text = "Optimal Qty";
		this.OptimalQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.OptimalQty.Location = new System.Drawing.Point(804, 25);
		this.OptimalQty.Name = "OptimalQty";
		this.OptimalQty.Size = new System.Drawing.Size(95, 23);
		this.OptimalQty.TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(604, 6);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(35, 15);
		this.label4.TabIndex = 20;
		this.label4.Text = "UOM";
		this.ItemUomId.AllowNull = (DefaultableBoolean)1;
		this.ItemUomId.AutoCompleteMode = (AutoCompleteMode)4;
		this.ItemUomId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ItemUomId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.ItemUomId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ItemUomId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ItemUomId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ItemUomId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.ItemUomId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ItemUomId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.ItemUomId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ItemUomId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ItemUomId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.ItemUomId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ItemUomId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ItemUomId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ItemUomId).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ItemUomId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ItemUomId).Location = new System.Drawing.Point(604, 23);
		((System.Windows.Forms.Control)(object)this.ItemUomId).Name = "ItemUomId";
		((System.Windows.Forms.Control)(object)this.ItemUomId).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.ItemUomId).TabIndex = 3;
		this.EffectiveDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.EffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.EffectiveDate.Location = new System.Drawing.Point(210, 25);
		this.EffectiveDate.Name = "EffectiveDate";
		this.EffectiveDate.Size = new System.Drawing.Size(168, 23);
		this.EffectiveDate.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(706, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(51, 15);
		this.label2.TabIndex = 17;
		this.label2.Text = "Min Qty";
		this.MinQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.MinQty.Location = new System.Drawing.Point(706, 25);
		this.MinQty.Name = "MinQty";
		this.MinQty.Size = new System.Drawing.Size(95, 23);
		this.MinQty.TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(381, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(31, 15);
		this.label1.TabIndex = 15;
		this.label1.Text = "Item";
		this.ItemId.AllowNull = (DefaultableBoolean)1;
		this.ItemId.AutoCompleteMode = (AutoCompleteMode)4;
		this.ItemId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ItemId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ItemId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.ItemId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ItemId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ItemId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ItemId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.ItemId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ItemId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.ItemId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ItemId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.ItemId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ItemId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.ItemId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ItemId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ItemId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ItemId).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ItemId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ItemId).Location = new System.Drawing.Point(381, 23);
		((System.Windows.Forms.Control)(object)this.ItemId).Name = "ItemId";
		((System.Windows.Forms.Control)(object)this.ItemId).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.ItemId).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.ItemId).Leave += new System.EventHandler(ItemId_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(9, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 15);
		this.label3.TabIndex = 13;
		this.label3.Text = "Branch";
		this.BranchesId.AllowNull = (DefaultableBoolean)1;
		this.BranchesId.AutoCompleteMode = (AutoCompleteMode)4;
		this.BranchesId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.BranchesId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.BranchesId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.BranchesId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.BranchesId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.BranchesId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.BranchesId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.BranchesId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.BranchesId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.BranchesId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.BranchesId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.BranchesId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.BranchesId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.BranchesId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.BranchesId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.BranchesId).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BranchesId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.BranchesId).Location = new System.Drawing.Point(9, 23);
		((System.Windows.Forms.Control)(object)this.BranchesId).Name = "BranchesId";
		((System.Windows.Forms.Control)(object)this.BranchesId).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.BranchesId).TabIndex = 0;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1101, 25);
		this.panel2.TabIndex = 0;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1101, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(75, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(59, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1101, 381);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmItemsReorderSchedule";
		this.Text = "frmItemsReorderSchedule";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmItemsReorderSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemsReorderSchedule_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ItemUomId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ItemId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.BranchesId).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}
