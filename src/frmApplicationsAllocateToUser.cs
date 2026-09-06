// using Architecture.WinApp.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Account_Definition;
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
namespace Architecture.WinApp.BookingOffice
{

public class frmApplicationsAllocateToUser : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDefineItemCustomGroup;

	private Panel panel2;

	private Panel PanelMainTemplateII;

	private Panel panel1;

	internal GridEX GridAllocated;

	private Button btnDelete;

	private Label labelAllocateGridTemplateII;

	private Panel panel5;

	internal GridEX GridUnAllocated;

	private Button BtnSave;

	private Label labelUnAllocateGridTemplateII;

	private Panel panel10;

	internal GridEX grdCompany;

	private Label label7;

	private ToolStripButton btnshortcutkeys;

	private Panel panel8;

	private Panel panel6;

	private Panel panel7;

	private Label lbltocompany;

	private Button BtnShow;

	internal UltraCombo CmbUserName;

	public frmApplicationsAllocateToUser(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void ApplicationsAllocateToUser_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1200, 500);
			UserNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UserNameFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbUserName.Value);
			DataTable dt = UserAccount.ReadAll(new UserAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbUserName, "Id", "UserName", "UserName", false);
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbUserName.Value = id;
					}
					else
					{
						((Control)(object)CmbUserName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbUserName).Text = string.Empty;
				((UltraGridBase)CmbUserName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			int UserId = Conversion.ToInt(CmbUserName.Value);
			if (((UltraGridBase)CmbUserName).ActiveRow != null && UserId > 0)
			{
				string UserName = Conversion.ToString((object)((Control)(object)CmbUserName).Text);
				CompanyGridFill(UserId, UserName);
				label7.Text = "Companies Of User : '" + UserName + "'";
			}
			else
			{
				grdCompany.DataSource = null;
				grdCompany.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyGridFill(int UserId, string UserName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			DataTable dtComp = UserAccountAllocation.GetAllCompaniesByUserId(new UserAccountAllocation
			{
				OrganizationId = UserAccount.OrganizationId,
				IsActive = 1,
				UserAccountId = Conversion.ToInt(CmbUserName.Value)
			});
			if (dtComp.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CompanyName", typeof(string));
				dt.Columns.Add("UserId", typeof(int));
				dt.Columns.Add("UserName", typeof(string));
				for (int i = 0; i < dtComp.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtComp.Rows[i]["CompanyId"]), Conversion.ToString(dtComp.Rows[i]["CompName"]), UserId, UserName);
				}
				grdCompany.DataSource = dt;
				grdCompany.RetrieveStructure();
				GridCompanySetting();
			}
			else
			{
				grdCompany.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridCompanySetting()
	{
		try
		{
			grdCompany.AllowEdit = (InheritableBoolean)2;
			grdCompany.RootTable.Columns["Id"].Visible = false;
			grdCompany.RootTable.Columns["UserId"].Visible = false;
			grdCompany.RootTable.HeaderLines = 1;
			grdCompany.RootTable.Columns["CompanyName"].HeaderAlignment = (TextAlignment)2;
			grdCompany.RootTable.Columns["CompanyName"].Width = 270;
			grdCompany.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdCompany_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int UserId = Conversion.ToInt(CmbUserName.Value);
			GridEXRow r = grdCompany.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && UserId > 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				string CompanyName = Conversion.ToString(r.Cells["CompanyName"].Value);
				string UserName = Conversion.ToString((object)((Control)(object)CmbUserName).Text);
				UnAllocatedGridFill(UserId, Id);
				AllocatedGridFill(UserId, Id);
				labelUnAllocateGridTemplateII.Text = "UnAllocated Apps Of User : '" + UserName + "'\n For Company : " + CompanyName;
				labelAllocateGridTemplateII.Text = "Allocated Apps Of User : '" + UserName + "'\n For Company : " + CompanyName;
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

	private void UnAllocatedGridFill(int UserId, int CompanyId)
	{
		try
		{
			DataTable dtUnAllocat = ApplicationsAllocateToUser.ApplicationsAllocateToUser_UnAllocatedData(CompanyId, UserId);
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("UserId");
				dt.Columns.Add("UserName");
				dt.Columns.Add("CompanyId");
				dt.Columns.Add("CompanyName");
				dt.Columns.Add("AppId");
				dt.Columns.Add("AppName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["ApplicationsAllocateToUserId"], UserId, dtUnAllocat.Rows[i]["UserName"], CompanyId, dtUnAllocat.Rows[i]["CompanyName"], dtUnAllocat.Rows[i]["AppId"], dtUnAllocat.Rows[i]["App"], dtUnAllocat.Rows[i]["IsActive"]);
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
			GridUnAllocated.AllowEdit = (InheritableBoolean)2;
			GridUnAllocated.RootTable.Columns["Id"].Visible = false;
			GridUnAllocated.RootTable.Columns["UserId"].Visible = false;
			GridUnAllocated.RootTable.Columns["UserName"].Visible = false;
			GridUnAllocated.RootTable.Columns["CompanyId"].Visible = false;
			GridUnAllocated.RootTable.Columns["CompanyName"].Visible = false;
			GridUnAllocated.RootTable.Columns["AppId"].Visible = false;
			GridUnAllocated.RootTable.Columns["IsActive"].Visible = false;
			GridUnAllocated.RootTable.HeaderLines = 1;
			GridUnAllocated.RootTable.Columns["AppName"].Width = 280;
			GridUnAllocated.RootTable.Columns["AppName"].HeaderAlignment = (TextAlignment)2;
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

	private void AllocatedGridFill(int UserId, int CompanyId)
	{
		try
		{
			DataTable dtAllocat = ApplicationsAllocateToUser.ApplicationsAllocateToUser_AllocatedData(CompanyId, UserId);
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("UserId");
				dt.Columns.Add("UserName");
				dt.Columns.Add("CompanyId");
				dt.Columns.Add("CompanyName");
				dt.Columns.Add("AppId");
				dt.Columns.Add("AppName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["ApplicationsAllocateToUserId"], UserId, dtAllocat.Rows[i]["UserName"], CompanyId, dtAllocat.Rows[i]["CompanyName"], dtAllocat.Rows[i]["AppId"], dtAllocat.Rows[i]["App"], dtAllocat.Rows[i]["IsActive"]);
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
			GridAllocated.AllowEdit = (InheritableBoolean)2;
			GridAllocated.RootTable.Columns["Id"].Visible = false;
			GridAllocated.RootTable.Columns["UserId"].Visible = false;
			GridAllocated.RootTable.Columns["UserName"].Visible = false;
			GridAllocated.RootTable.Columns["CompanyId"].Visible = false;
			GridAllocated.RootTable.Columns["CompanyName"].Visible = false;
			GridAllocated.RootTable.Columns["AppId"].Visible = false;
			GridAllocated.RootTable.Columns["IsActive"].Visible = false;
			GridAllocated.RootTable.HeaderLines = 1;
			GridAllocated.RootTable.Columns["AppName"].Width = 280;
			GridAllocated.RootTable.Columns["AppName"].HeaderAlignment = (TextAlignment)2;
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
			Insert(GridUnAllocated, "Allocate", Active: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert(GridEX grd, string Message, bool Active)
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
			if (MessageBox.Show("Are you sure to " + Message + " Users?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ApplicationsAllocateToUser obj = new ApplicationsAllocateToUser();
				obj.ApplicationsAllocateToUserList = new List<ApplicationsAllocateToUser>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApplicationsAllocateToUser User = new ApplicationsAllocateToUser();
					User.ApplicationsAllocateToUserId = Conversion.ToInt(r.Cells["Id"].Value);
					User.UserId = Conversion.ToInt(r.Cells["UserId"].Value);
					User.CompanyId = Conversion.ToInt(r.Cells["CompanyId"].Value);
					User.AppId = Conversion.ToInt(r.Cells["AppId"].Value);
					User.IsActive = Active;
					User.EntryUserId = UserAccount.ID;
					User.EntryDate = DateTime.Now;
					User.ModifyDate = DateTime.Now;
					User.ModifyUserId = UserAccount.ID;
					obj.ApplicationsAllocateToUserList.Add(User);
				}
				ApplicationsAllocateToUser.Save(obj);
				MessageBox.Show("Row's " + Message + " Successfully");
				grdCompany_SelectionChanged(null, null);
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
			Insert(GridAllocated, "Un-Allocate", Active: false);
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
			UserNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApplicationsAllocateToUser_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnDefineCostCenter_Click(object sender, EventArgs e)
	{
		try
		{
			new frmCostCenter(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUserDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new frmUserRights(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApplicationsAllocateToUser_KeyDown(object sender, KeyEventArgs e)
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
			((Control)(object)grdCompany).Focus();
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
			((Control)(object)grdCompany).Focus();
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
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.BookingOffice.frmApplicationsAllocateToUser));
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
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnDefineItemCustomGroup = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.PanelMainTemplateII = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.GridAllocated = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnDelete = new System.Windows.Forms.Button();
		this.labelAllocateGridTemplateII = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GridUnAllocated = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnSave = new System.Windows.Forms.Button();
		this.labelUnAllocateGridTemplateII = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdCompany = new GridEX();
		this.label7 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.BtnShow = new System.Windows.Forms.Button();
		this.CmbUserName = new UltraCombo();
		this.tsItemType.SuspendLayout();
		this.PanelMainTemplateII.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocated).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocated).BeginInit();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCompany).BeginInit();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).BeginInit();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnRefresh, this.btnDefineItemCustomGroup, this.btnshortcutkeys });
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
		this.btnDefineItemCustomGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDefineItemCustomGroup.Image = (System.Drawing.Image)resources.GetObject("btnDefineItemCustomGroup.Image");
		this.btnDefineItemCustomGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineItemCustomGroup.Name = "btnDefineItemCustomGroup";
		this.btnDefineItemCustomGroup.Size = new System.Drawing.Size(86, 24);
		this.btnDefineItemCustomGroup.Text = "Define User";
		this.btnDefineItemCustomGroup.Click += new System.EventHandler(BtnUserDefine_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.PanelMainTemplateII.Controls.Add(this.panel1);
		this.PanelMainTemplateII.Controls.Add(this.panel5);
		this.PanelMainTemplateII.Controls.Add(this.panel10);
		this.PanelMainTemplateII.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainTemplateII.Location = new System.Drawing.Point(0, 70);
		this.PanelMainTemplateII.Name = "PanelMainTemplateII";
		this.PanelMainTemplateII.Size = new System.Drawing.Size(1184, 391);
		this.PanelMainTemplateII.TabIndex = 1;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocated);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(800, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(365, 391);
		this.panel1.TabIndex = 80;
		this.GridAllocated.AllowEdit = (InheritableBoolean)2;
		this.GridAllocated.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocated.FilterMode = (FilterMode)2;
		this.GridAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocated.GroupByBoxVisible = false;
		this.GridAllocated.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Location = new System.Drawing.Point(0, 35);
		((System.Windows.Forms.Control)(object)this.GridAllocated).Name = "GridAllocated";
		((ScrollableBase)this.GridAllocated).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocated).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Size = new System.Drawing.Size(365, 356);
		((System.Windows.Forms.Control)(object)this.GridAllocated).TabIndex = 64;
		this.GridAllocated.TotalRow = (InheritableBoolean)1;
		this.GridAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocated).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.btnDelete);
		this.panel8.Controls.Add(this.labelAllocateGridTemplateII);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(365, 35);
		this.panel8.TabIndex = 69;
		this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDelete.BackColor = System.Drawing.Color.White;
		this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.Location = new System.Drawing.Point(328, 2);
		this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(33, 30);
		this.btnDelete.TabIndex = 5;
		this.btnDelete.UseVisualStyleBackColor = false;
		this.btnDelete.Click += new System.EventHandler(btnDeleteTemplateII_Click);
		this.labelAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.labelAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelAllocateGridTemplateII.Location = new System.Drawing.Point(2, 0);
		this.labelAllocateGridTemplateII.Name = "labelAllocateGridTemplateII";
		this.labelAllocateGridTemplateII.Size = new System.Drawing.Size(324, 30);
		this.labelAllocateGridTemplateII.TabIndex = 4;
		this.labelAllocateGridTemplateII.Text = "Allocated Apps Of User :";
		this.labelAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocated);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(430, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(370, 391);
		this.panel5.TabIndex = 79;
		this.GridUnAllocated.AllowEdit = (InheritableBoolean)2;
		this.GridUnAllocated.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocated.FilterMode = (FilterMode)2;
		this.GridUnAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocated.GroupByBoxVisible = false;
		this.GridUnAllocated.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Location = new System.Drawing.Point(0, 35);
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Name = "GridUnAllocated";
		((ScrollableBase)this.GridUnAllocated).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocated).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Size = new System.Drawing.Size(370, 356);
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Tag = "UnAllocatedGrid";
		this.GridUnAllocated.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocated).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.BtnSave);
		this.panel6.Controls.Add(this.labelUnAllocateGridTemplateII);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(370, 35);
		this.panel6.TabIndex = 68;
		this.BtnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSave.BackColor = System.Drawing.Color.White;
		this.BtnSave.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnSave.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSave.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnSave.Location = new System.Drawing.Point(333, 2);
		this.BtnSave.Margin = new System.Windows.Forms.Padding(0);
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(33, 31);
		this.BtnSave.TabIndex = 66;
		this.BtnSave.UseVisualStyleBackColor = false;
		this.BtnSave.Click += new System.EventHandler(BtnSaveTemplateII_Click);
		this.labelUnAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelUnAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.labelUnAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelUnAllocateGridTemplateII.Location = new System.Drawing.Point(6, 0);
		this.labelUnAllocateGridTemplateII.Name = "labelUnAllocateGridTemplateII";
		this.labelUnAllocateGridTemplateII.Size = new System.Drawing.Size(324, 30);
		this.labelUnAllocateGridTemplateII.TabIndex = 3;
		this.labelUnAllocateGridTemplateII.Text = "UnAllocated Apps Of User :";
		this.labelUnAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdCompany);
		this.panel10.Controls.Add(this.label7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(430, 391);
		this.panel10.TabIndex = 81;
		this.grdCompany.AllowEdit = (InheritableBoolean)2;
		this.grdCompany.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdCompany).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdCompany.FilterMode = (FilterMode)2;
		this.grdCompany.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCompany).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCompany.GroupByBoxVisible = false;
		this.grdCompany.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdCompany).Location = new System.Drawing.Point(0, 35);
		((System.Windows.Forms.Control)(object)this.grdCompany).Name = "grdCompany";
		((ScrollableBase)this.grdCompany).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdCompany).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdCompany).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCompany).Size = new System.Drawing.Size(430, 356);
		((System.Windows.Forms.Control)(object)this.grdCompany).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.grdCompany).Tag = "UnAllocatedGrid";
		this.grdCompany.TotalRow = (InheritableBoolean)1;
		this.grdCompany.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCompany).VisualStyle = (VisualStyle)4;
		this.grdCompany.SelectionChanged += new System.EventHandler(grdCompany_SelectionChanged);
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(430, 35);
		this.label7.TabIndex = 3;
		this.label7.Text = "Companies Of Selected User";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1184, 30);
		this.panel2.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Controls.Add(this.BtnShow);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUserName);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 30);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1184, 40);
		this.panel7.TabIndex = 60;
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(5, 13);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(62, 15);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "UserName";
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnShow.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnShow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(368, 8);
		this.BtnShow.Margin = new System.Windows.Forms.Padding(0);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(58, 25);
		this.BtnShow.TabIndex = 66;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.CmbUserName.AllowNull = (DefaultableBoolean)1;
		this.CmbUserName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUserName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUserName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbUserName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUserName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Location = new System.Drawing.Point(75, 7);
		((System.Windows.Forms.Control)(object)this.CmbUserName).Name = "CmbUserName";
		((System.Windows.Forms.Control)(object)this.CmbUserName).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.CmbUserName).TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 461);
		base.Controls.Add(this.PanelMainTemplateII);
		base.Controls.Add(this.panel7);
		base.Controls.Add(this.panel2);
		base.KeyPreview = true;
		base.Name = "frmApplicationsAllocateToUser";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ApplicationsAllocateToUser_FormClosing);
		base.Load += new System.EventHandler(ApplicationsAllocateToUser_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ApplicationsAllocateToUser_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.PanelMainTemplateII.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocated).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocated).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCompany).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).EndInit();
		base.ResumeLayout(false);
	}
}

}
