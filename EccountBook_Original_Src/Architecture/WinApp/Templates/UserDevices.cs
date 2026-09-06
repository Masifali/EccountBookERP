using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Main;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Templates;

public class UserDevices : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	private TabControl tabControl1;

	private TabPage tabPartyToItem;

	private Panel panel2;

	private Panel PanelMainTemplateII;

	private Panel panel1;

	internal GridEX GridAllocated;

	private Button btnDeleteTemplateII;

	private Label labelAllocateGridTemplateII;

	private Panel panel5;

	internal GridEX GridUnAllocated;

	private Button BtnSaveTemplateII;

	private Label labelUnAllocateGridTemplateII;

	private Panel panel10;

	internal GridEX grdUsers;

	private Label label7;

	private ToolStripButton btnshortcutkeys;

	private Panel panel8;

	private Panel panel6;

	public UserDevices(UserAccount pUserAccount)
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
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1200, 500);
			formright = CommonServices.SetRightsValueInRightsObject(Conversion.ToString((object)base.Name));
			BtnSaveTemplateII.Enabled = formright.DoHaveSaveRight;
			btnDeleteTemplateII.Enabled = formright.DoHaveCanDelete;
			UserGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UserGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtAllUser = UserAccount.ReadAll(new UserAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtAllUser != null && dtAllUser.Rows.Count == 0)
			{
				grdUsers.DataSource = null;
				return;
			}
			DataTable activeUsers = (from row in dtAllUser.AsEnumerable()
				where row.Field<bool>("IsActive")
				select row).CopyToDataTable();
			if (dtAllUser.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("UserName", typeof(string));
			dt.Columns.Add("IsActive", typeof(bool));
			dt.Columns.Add("Title", typeof(string));
			dt.Columns.Add("Email", typeof(string));
			dt.Columns.Add("PhoneNumber", typeof(string));
			foreach (DataRow dr in activeUsers.Rows)
			{
				dt.Rows.Add(dr["ID"], dr["UserName"], dr["IsActive"], dr["Title"], dr["Email"], dr["PhoneNumber"]);
			}
			grdUsers.DataSource = dt;
			grdUsers.RetrieveStructure();
			UserGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UserGridSetting()
	{
		try
		{
			grdUsers.RootTable.Columns["Id"].Visible = false;
			grdUsers.RootTable.Columns["IsActive"].Visible = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdItemName_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdUsers.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int UserId = Conversion.ToInt(r.Cells["Id"].Value);
				string UserName = Conversion.ToString(r.Cells["UserName"].Value);
				UnAllocatedDevicesGridFill(UserId);
				AllocatedDevicesGridFill(UserId);
				labelUnAllocateGridTemplateII.Text = "UnAllocated Devices Of User : '" + UserName + "'";
				labelAllocateGridTemplateII.Text = "Allocated Devices Of User : '" + UserName + "'";
			}
			else
			{
				GridUnAllocated.DataSource = null;
				GridAllocated.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UnAllocatedDevicesGridFill(int UserId)
	{
		try
		{
			DataTable dtUnAllocat = UserDevices.UserDevices_UnAllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, UserId);
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("UserId");
				dt.Columns.Add("UserName");
				dt.Columns.Add("DeviceId");
				dt.Columns.Add("DeviceName");
				dt.Columns.Add("DeviceUserName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], UserId, dtUnAllocat.Rows[i]["UserName"], dtUnAllocat.Rows[i]["DeviceId"], dtUnAllocat.Rows[i]["DeviceName"], dtUnAllocat.Rows[i]["DeviceUserName"], dtUnAllocat.Rows[i]["IsActive"]);
				}
				GridUnAllocated.DataSource = dt;
				GridUnAllocated.RetrieveStructure();
				GridUnAllocatedDevicesSetting();
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

	private void GridUnAllocatedDevicesSetting()
	{
		try
		{
			GridUnAllocated.AllowEdit = (InheritableBoolean)2;
			GridUnAllocated.RootTable.Columns["Id"].Visible = false;
			GridUnAllocated.RootTable.Columns["UserId"].Visible = false;
			GridUnAllocated.RootTable.Columns["UserName"].Visible = false;
			GridUnAllocated.RootTable.Columns["DeviceId"].Visible = false;
			GridUnAllocated.RootTable.Columns["IsActive"].Visible = false;
			GridUnAllocated.RootTable.HeaderLines = 2;
			GridUnAllocated.RootTable.Columns["DeviceName"].Width = 150;
			GridUnAllocated.RootTable.Columns["DeviceUserName"].Width = 150;
			GridUnAllocated.RootTable.Columns.Add("Select");
			GridUnAllocated.RootTable.Columns["Select"].ActAsSelector = true;
			GridUnAllocated.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridUnAllocated.RootTable.Columns["Select"].Width = 30;
			GridUnAllocated.RootTable.Columns["Select"].Position = 0;
			GridUnAllocated.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllocatedDevicesGridFill(int UserId)
	{
		try
		{
			DataTable dtAllocat = UserDevices.UserDevices_AllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, UserId);
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("UserId");
				dt.Columns.Add("UserName");
				dt.Columns.Add("DeviceId");
				dt.Columns.Add("DeviceName");
				dt.Columns.Add("DeviceUserName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], UserId, dtAllocat.Rows[i]["UserName"], dtAllocat.Rows[i]["DeviceId"], dtAllocat.Rows[i]["DeviceName"], dtAllocat.Rows[i]["DeviceUserName"], dtAllocat.Rows[i]["IsActive"]);
				}
				GridAllocated.DataSource = dt;
				GridAllocated.RetrieveStructure();
				GridAllocatedDevicesSetting();
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

	private void GridAllocatedDevicesSetting()
	{
		try
		{
			GridAllocated.AllowEdit = (InheritableBoolean)1;
			GridAllocated.RootTable.Columns["Id"].Visible = false;
			GridAllocated.RootTable.Columns["UserId"].Visible = false;
			GridAllocated.RootTable.Columns["UserName"].Visible = false;
			GridAllocated.RootTable.Columns["DeviceId"].Visible = false;
			GridAllocated.RootTable.Columns["IsActive"].Visible = false;
			GridAllocated.RootTable.HeaderLines = 2;
			GridAllocated.RootTable.Columns["DeviceName"].Width = 150;
			GridAllocated.RootTable.Columns["DeviceUserName"].Width = 150;
			GridAllocated.RootTable.Columns.Add("Select");
			GridAllocated.RootTable.Columns["Select"].ActAsSelector = true;
			GridAllocated.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridAllocated.RootTable.Columns["Select"].Width = 30;
			GridAllocated.RootTable.Columns["Select"].Position = 0;
			GridAllocated.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnSaveTemplateII_Click(object sender, EventArgs e)
	{
		try
		{
			Insert(GridUnAllocated);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert(GridEX grd, string Message = "Allocate")
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Length == 0)
			{
				throw new Exception("Check Row's Which You want To " + Message);
			}
			if (MessageBox.Show("Are you sure to " + Message + " Devices?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				UserDevices obj = new UserDevices();
				obj.UserDevicesList = new List<UserDevices>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					UserDevices item = new UserDevices();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.UserId = Conversion.ToInt(r.Cells["UserId"].Value);
					item.DeviceId = Conversion.ToString(r.Cells["DeviceId"].Value);
					item.IsActive = Message == "Allocate";
					item.EntryUserId = UserAccount.ID;
					item.ModifyUserId = UserAccount.ID;
					obj.UserDevicesList.Add(item);
				}
				UserDevices.Save(obj);
				MessageBox.Show("Row's " + Message + " Successfully");
				grdItemName_SelectionChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDeleteTemplateII_Click(object sender, EventArgs e)
	{
		try
		{
			Insert(GridAllocated, "UnAllocated");
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
			UserGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserDevices_FormClosing(object sender, FormClosingEventArgs e)
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
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			((Control)(object)grdUsers).Focus();
		}
		if (e.Control)
		{
			_ = e.KeyCode;
			_ = 85;
		}
		if (e.Control && e.KeyCode == Keys.A)
		{
			BtnSaveTemplateII_Click(null, null);
		}
		if (e.Control && (e.KeyCode == Keys.D || e.KeyCode == Keys.Delete))
		{
			btnDeleteTemplateII_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			((Control)(object)grdUsers).Focus();
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
		if (e.Control && e.KeyCode == Keys.Left)
		{
			((Control)(object)GridUnAllocated).Focus();
		}
		if (e.Control && e.KeyCode == Keys.Right)
		{
			((Control)(object)GridAllocated).Focus();
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
			dt.Rows.Add("Ctrl+A/Ctrl+S", "For Press Allocate Button");
			dt.Rows.Add("Ctrl+U", "For Press Update Button");
			dt.Rows.Add("Ctrl+D/Ctrl+Delete", "For Press UnAllocate Button");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on User Grid");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPartyToItem = new System.Windows.Forms.TabPage();
		this.PanelMainTemplateII = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.GridAllocated = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnDeleteTemplateII = new System.Windows.Forms.Button();
		this.labelAllocateGridTemplateII = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GridUnAllocated = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnSaveTemplateII = new System.Windows.Forms.Button();
		this.labelUnAllocateGridTemplateII = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdUsers = new GridEX();
		this.label7 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPartyToItem.SuspendLayout();
		this.PanelMainTemplateII.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocated).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocated).BeginInit();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUsers).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnshortcutkeys });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1184, 27);
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
		this.tabControl1.Controls.Add(this.tabPartyToItem);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 30);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 431);
		this.tabControl1.TabIndex = 1;
		this.tabPartyToItem.Controls.Add(this.PanelMainTemplateII);
		this.tabPartyToItem.Location = new System.Drawing.Point(4, 25);
		this.tabPartyToItem.Name = "tabPartyToItem";
		this.tabPartyToItem.Padding = new System.Windows.Forms.Padding(3);
		this.tabPartyToItem.Size = new System.Drawing.Size(1176, 402);
		this.tabPartyToItem.TabIndex = 1;
		this.tabPartyToItem.Text = "UserDevices Allocation";
		this.tabPartyToItem.UseVisualStyleBackColor = true;
		this.PanelMainTemplateII.Controls.Add(this.panel1);
		this.PanelMainTemplateII.Controls.Add(this.panel5);
		this.PanelMainTemplateII.Controls.Add(this.panel10);
		this.PanelMainTemplateII.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainTemplateII.Location = new System.Drawing.Point(3, 3);
		this.PanelMainTemplateII.Name = "PanelMainTemplateII";
		this.PanelMainTemplateII.Size = new System.Drawing.Size(1170, 396);
		this.PanelMainTemplateII.TabIndex = 1;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocated);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(800, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(370, 396);
		this.panel1.TabIndex = 80;
		this.GridAllocated.AllowEdit = (InheritableBoolean)2;
		this.GridAllocated.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocated.FilterMode = (FilterMode)2;
		this.GridAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocated.GroupByBoxVisible = false;
		this.GridAllocated.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.GridAllocated).Name = "GridAllocated";
		((ScrollableBase)this.GridAllocated).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocated).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Size = new System.Drawing.Size(370, 368);
		((System.Windows.Forms.Control)(object)this.GridAllocated).TabIndex = 64;
		this.GridAllocated.TotalRow = (InheritableBoolean)1;
		this.GridAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocated).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.btnDeleteTemplateII);
		this.panel8.Controls.Add(this.labelAllocateGridTemplateII);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(370, 28);
		this.panel8.TabIndex = 69;
		this.btnDeleteTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeleteTemplateII.BackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeleteTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeleteTemplateII.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeleteTemplateII.Location = new System.Drawing.Point(337, 2);
		this.btnDeleteTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeleteTemplateII.Name = "btnDeleteTemplateII";
		this.btnDeleteTemplateII.Size = new System.Drawing.Size(28, 25);
		this.btnDeleteTemplateII.TabIndex = 5;
		this.btnDeleteTemplateII.UseVisualStyleBackColor = false;
		this.btnDeleteTemplateII.Click += new System.EventHandler(btnDeleteTemplateII_Click);
		this.labelAllocateGridTemplateII.AutoSize = true;
		this.labelAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.labelAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelAllocateGridTemplateII.Location = new System.Drawing.Point(2, 5);
		this.labelAllocateGridTemplateII.Name = "labelAllocateGridTemplateII";
		this.labelAllocateGridTemplateII.Size = new System.Drawing.Size(271, 17);
		this.labelAllocateGridTemplateII.TabIndex = 4;
		this.labelAllocateGridTemplateII.Text = "Allocated Devices Of User : Ali Rehman";
		this.labelAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocated);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(430, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(370, 396);
		this.panel5.TabIndex = 79;
		this.GridUnAllocated.AllowEdit = (InheritableBoolean)2;
		this.GridUnAllocated.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocated.FilterMode = (FilterMode)2;
		this.GridUnAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocated.GroupByBoxVisible = false;
		this.GridUnAllocated.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Name = "GridUnAllocated";
		((ScrollableBase)this.GridUnAllocated).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocated).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Size = new System.Drawing.Size(370, 368);
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Tag = "UnAllocatedGrid";
		this.GridUnAllocated.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocated).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.BtnSaveTemplateII);
		this.panel6.Controls.Add(this.labelUnAllocateGridTemplateII);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(370, 28);
		this.panel6.TabIndex = 68;
		this.BtnSaveTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSaveTemplateII.BackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnSaveTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSaveTemplateII.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnSaveTemplateII.Location = new System.Drawing.Point(338, 2);
		this.BtnSaveTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.BtnSaveTemplateII.Name = "BtnSaveTemplateII";
		this.BtnSaveTemplateII.Size = new System.Drawing.Size(28, 25);
		this.BtnSaveTemplateII.TabIndex = 66;
		this.BtnSaveTemplateII.UseVisualStyleBackColor = false;
		this.BtnSaveTemplateII.Click += new System.EventHandler(BtnSaveTemplateII_Click);
		this.labelUnAllocateGridTemplateII.AutoSize = true;
		this.labelUnAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelUnAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.labelUnAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelUnAllocateGridTemplateII.Location = new System.Drawing.Point(6, 5);
		this.labelUnAllocateGridTemplateII.Name = "labelUnAllocateGridTemplateII";
		this.labelUnAllocateGridTemplateII.Size = new System.Drawing.Size(290, 17);
		this.labelUnAllocateGridTemplateII.TabIndex = 3;
		this.labelUnAllocateGridTemplateII.Text = "UnAllocated Devices Of User : Ali Rehman";
		this.labelUnAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdUsers);
		this.panel10.Controls.Add(this.label7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(430, 396);
		this.panel10.TabIndex = 81;
		this.grdUsers.AllowEdit = (InheritableBoolean)2;
		this.grdUsers.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdUsers).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdUsers.FilterMode = (FilterMode)2;
		this.grdUsers.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdUsers).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdUsers.GroupByBoxVisible = false;
		this.grdUsers.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdUsers).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdUsers).Name = "grdUsers";
		((ScrollableBase)this.grdUsers).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdUsers).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdUsers).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdUsers).Size = new System.Drawing.Size(430, 368);
		((System.Windows.Forms.Control)(object)this.grdUsers).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.grdUsers).Tag = "UnAllocatedGrid";
		this.grdUsers.TotalRow = (InheritableBoolean)1;
		this.grdUsers.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdUsers).VisualStyle = (VisualStyle)4;
		this.grdUsers.SelectionChanged += new System.EventHandler(grdItemName_SelectionChanged);
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(430, 28);
		this.label7.TabIndex = 3;
		this.label7.Text = "Users";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1184, 30);
		this.panel2.TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 461);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel2);
		base.KeyPreview = true;
		base.Name = "UserDevices";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(UserDevices_FormClosing);
		base.Load += new System.EventHandler(frmItemsCustomizedGroups_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemsCustomizedGroups_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPartyToItem.ResumeLayout(false);
		this.PanelMainTemplateII.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocated).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocated).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdUsers).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
