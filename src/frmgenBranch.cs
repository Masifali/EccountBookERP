using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.PolicyManagment;
using Architecture.WinApp.HRM.ProfileManagement;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.HRM.EmployeeManagement
{

public class frmgenBranch : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private IContainer components;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnnew;

	private ToolStrip tsItemType;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdfrm;

	internal UltraCombo cmbLocation;

	private Label label48;

	private TextBox txtBranchName;

	private Label label50;

	internal UltraCombo cmbBranchType;

	private Label label3;

	private Panel panel3;

	private Panel panel1;

	private Label label2;

	private Panel panel5;

	private Panel panel4;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateF;

	private Button BtnCancel;

	private Button BtnSavef;

	private Button btnLocation;

	private Button btnbranchType;

	private ToolStripButton btnRefresh;

	public frmgenBranch(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void LocationFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genLocation.Getall(new genLocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("LocationName");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["LocationId"], lst.Rows[i]["LocationName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbLocation, "Id", "LocationName", "Location", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			DataTable lst = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ProfilePrefix = "All",
				ProfileTypeId = 62
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("ProfileId");
			dt.Columns.Add("ProfileName");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Clear();
				dt.Rows.Add(lst.Rows[i]["ProfileId"], lst.Rows[i]["ProfileName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbBranchType, "ProfileId", "ProfileName", "Branch Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)cmbLocation).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Location");
			cmbLocation.Focus();
			return false;
		}
		if (((Control)(object)cmbBranchType).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Branch Type");
			cmbBranchType.Focus();
			return false;
		}
		if (txtBranchName.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Branch Name");
			txtBranchName.Focus();
			return false;
		}
		return true;
	}

	private void BenifitDefine_Load(object sender, EventArgs e)
	{
		btnsave.Visible = true;
		btnupdate.Visible = false;
		BtnSavef.Visible = true;
		btnUpdateF.Visible = false;
		gridfill();
		LocationFill();
		BranchTypeFill();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		if (!formvalidation())
		{
			return;
		}
		try
		{
			if (genBranch.Save(new genBranch
			{
				LocationId = Conversion.ToInt(cmbLocation.Value),
				BranchTypeProfileId = Conversion.ToInt(cmbBranchType.Value),
				BranchName = Conversion.ToString((object)txtBranchName.Text.Trim()),
				AlteredById = Conversion.ToInt((object)UserAccount.ID),
				CreatedById = Conversion.ToInt((object)UserAccount.ID),
				UserLogId = Conversion.ToInt((object)UserAccount.ID)
			}) > 0)
			{
				MessageBox.Show("Save Successfully");
				clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			BtnSavef.Visible = false;
			btnUpdateF.Visible = true;
			UpdateMode = true;
			genBranch cat = new genBranch();
			RecId = Conversion.ToInt((object)grdfrm.CurrentRow.Cells["Id"].Value.ToString());
			cat = genBranch.GetByID(RecId);
			cmbLocation.Value = Conversion.ToString((object)cat.LocationId);
			cmbBranchType.Value = Conversion.ToString((object)cat.BranchTypeProfileId);
			txtBranchName.Text = Conversion.ToString((object)cat.BranchName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		if (!formvalidation())
		{
			return;
		}
		try
		{
			if (genBranch.Save(new genBranch
			{
				BranchId = Conversion.ToInt((object)RecId),
				LocationId = Conversion.ToInt(cmbLocation.Value),
				BranchTypeProfileId = Conversion.ToInt(cmbBranchType.Value),
				BranchName = Conversion.ToString((object)txtBranchName.Text.Trim()),
				AlteredById = Conversion.ToInt((object)UserAccount.ID),
				CreatedById = Conversion.ToInt((object)UserAccount.ID),
				UserLogId = Conversion.ToInt((object)UserAccount.ID)
			}) > 0)
			{
				MessageBox.Show("Update Successfully");
				clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			DataTable getList = genBranch.Getall(new genBranch());
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("Location");
			table.Columns.Add("BranchType");
			table.Columns.Add("Branch");
			table.Rows.Clear();
			for (int i = 0; i < getList.Rows.Count; i++)
			{
				table.Rows.Add(getList.Rows[i]["BranchId"], getList.Rows[i]["LocationName"], getList.Rows[i]["ProfileName"], getList.Rows[i]["BranchName"]);
			}
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			GridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdfrm.RootTable.Columns["Id"].Visible = false;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void clear()
	{
		btnsave.Visible = true;
		btnupdate.Visible = false;
		BtnSavef.Visible = true;
		btnUpdateF.Visible = false;
		UpdateMode = false;
		((Control)(object)cmbBranchType).Text = string.Empty;
		((Control)(object)cmbLocation).Text = string.Empty;
		txtBranchName.Text = string.Empty;
		cmbLocation.Focus();
		gridfill();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		clear();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		LocationFill();
		BranchTypeFill();
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void BenifitDefine_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnLocation_Click(object sender, EventArgs e)
	{
		try
		{
			new frmgenLocation(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnbranchType_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 62;
			profileDefine.Show();
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
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.EmployeeManagement.frmgenBranch));
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
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdfrm = new GridEX();
		this.label48 = new System.Windows.Forms.Label();
		this.txtBranchName = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.cmbBranchType = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbLocation = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.btnbranchType = new System.Windows.Forms.Button();
		this.btnLocation = new System.Windows.Forms.Button();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tsItemType.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLocation).BeginInit();
		this.panel3.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 10.25f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(61, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 10.25f);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 10.25f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnupdate, this.btnsave, this.btnRefresh });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(460, 27);
		this.tsItemType.TabIndex = 1;
		this.tsItemType.Text = "toolStrip4";
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(150, 150);
		this.ctrlGrdBar1.TabIndex = 23;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 256);
		((System.Windows.Forms.Control)(object)this.grdfrm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(460, 305);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(64, 77);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(79, 15);
		this.label48.TabIndex = 0;
		this.label48.Text = "Branch Name";
		this.txtBranchName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchName.Location = new System.Drawing.Point(151, 73);
		this.txtBranchName.Margin = new System.Windows.Forms.Padding(4);
		this.txtBranchName.Name = "txtBranchName";
		this.txtBranchName.Size = new System.Drawing.Size(293, 23);
		this.txtBranchName.TabIndex = 4;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(64, 20);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(53, 15);
		this.label50.TabIndex = 2;
		this.label50.Text = "Location";
		this.cmbBranchType.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBranchType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBranchType).Location = new System.Drawing.Point(151, 43);
		((System.Windows.Forms.Control)(object)this.cmbBranchType).Name = "cmbBranchType";
		((System.Windows.Forms.Control)(object)this.cmbBranchType).Size = new System.Drawing.Size(260, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchType).TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(64, 49);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 15);
		this.label3.TabIndex = 7;
		this.label3.Text = "Branch Type";
		this.cmbLocation.AllowNull = (DefaultableBoolean)1;
		this.cmbLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocation).Location = new System.Drawing.Point(151, 14);
		((System.Windows.Forms.Control)(object)this.cmbLocation).Name = "cmbLocation";
		((System.Windows.Forms.Control)(object)this.cmbLocation).Size = new System.Drawing.Size(260, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocation).TabIndex = 0;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel3.Controls.Add(this.panel1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(460, 561);
		this.panel3.TabIndex = 22;
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar2);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 224);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(460, 32);
		this.panel1.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(411, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdfrm;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 28);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(4, 5);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 19);
		this.label2.TabIndex = 9;
		this.label2.Text = "History";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.btnLocation);
		this.panel5.Controls.Add(this.btnbranchType);
		this.panel5.Controls.Add(this.btnUpdateF);
		this.panel5.Controls.Add(this.BtnCancel);
		this.panel5.Controls.Add(this.BtnSavef);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchType);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocation);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label50);
		this.panel5.Controls.Add(this.txtBranchName);
		this.panel5.Controls.Add(this.label48);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 59);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(460, 165);
		this.panel5.TabIndex = 0;
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(369, 103);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 5;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.btnUpdateF.Click += new System.EventHandler(btnupdate_Click);
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(290, 103);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 6;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(369, 103);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 5;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.BtnSavef.Click += new System.EventHandler(btnsave_Click);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.tsItemType);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(460, 59);
		this.panel4.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 33);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(123, 19);
		this.label1.TabIndex = 8;
		this.label1.Text = "Define Branch";
		this.btnbranchType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnbranchType.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnbranchType.Location = new System.Drawing.Point(414, 43);
		this.btnbranchType.Name = "btnbranchType";
		this.btnbranchType.Size = new System.Drawing.Size(30, 26);
		this.btnbranchType.TabIndex = 3;
		this.btnbranchType.TabStop = false;
		this.btnbranchType.UseVisualStyleBackColor = true;
		this.btnbranchType.Click += new System.EventHandler(btnbranchType_Click);
		this.btnLocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLocation.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnLocation.Location = new System.Drawing.Point(414, 14);
		this.btnLocation.Name = "btnLocation";
		this.btnLocation.Size = new System.Drawing.Size(30, 26);
		this.btnLocation.TabIndex = 1;
		this.btnLocation.TabStop = false;
		this.btnLocation.UseVisualStyleBackColor = true;
		this.btnLocation.Click += new System.EventHandler(btnLocation_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(460, 561);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.ctrlGrdBar1);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmgenBranch";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBook";
		base.Load += new System.EventHandler(BenifitDefine_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(BenifitDefine_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLocation).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
