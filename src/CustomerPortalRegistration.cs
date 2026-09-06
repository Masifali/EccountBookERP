using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.DMS_FolderHierarchy
{

public class CustomerPortalRegistration : Form
{
	private int RecId;

	private DataTable dtrights = new DataTable();

	private DataTable dtAllUser = new DataTable();

	private DataTable dtCompanies = new DataTable();

	private string uId = "";

	private string ScreenName = "";

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton toolStripButton4;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton5;

	private ToolStripButton toolStripButton6;

	private ToolStripButton toolStripButton13;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton toolStripButton14;

	private ToolStripButton toolStripButton15;

	private ToolStripButton toolStripButton16;

	private ToolStrip miniToolStrip;

	private TabControl tabControl1;

	private TabPage tabPage3;

	private Panel panel10;

	private Panel panel8;

	internal GridEX grduserdefine;

	private Panel panel9;

	private Panel panel17;

	internal GridEX grdBranches;

	private Panel panel16;

	private GroupBox groupBox1;

	internal UltraCombo CmbPartyName;

	private Label label5;

	private CheckBox chkboxIsActive;

	private TextBox txtLastName;

	private Label label4;

	private TextBox txtFirstName;

	private Label label3;

	private TextBox txtPassword;

	private Label label2;

	private TextBox txtUsername;

	private Label label1;

	private Panel panel11;

	private Label label15;

	private Panel panel7;

	private Panel panel12;

	private Panel panel13;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew1;

	private ToolStripButton butnUpdate;

	private ToolStripButton butnSave;

	private RadioButton SubUserRadioButton;

	private RadioButton MainUserRadioButton;

	public CustomerPortalRegistration(UserAccount user)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmUserRights_Load(object sender, EventArgs e)
	{
		try
		{
			dtCompanies.Columns.Add("Id");
			dtCompanies.Columns.Add("CompanyId");
			dtCompanies.Columns.Add("Location");
			dtCompanies.Columns.Add("Active", typeof(bool));
			dtCompanies.Columns.Add("UserStatus", typeof(bool));
			bindUser();
			PartyNameFill();
			LocationsBind();
			dtrights.Columns.Add("ID");
			dtrights.Columns.Add("ScreenName");
			dtrights.Columns.Add("Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grduserdefine.RootTable.Columns["ID"].Visible = false;
			grduserdefine.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grduserdefine.RootTable.Columns["PartyName"].Width = 300;
			grduserdefine.RootTable.Columns["Password"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsUserBind()
	{
	}

	private void bindUser()
	{
		try
		{
			dtAllUser = UserAccount.ReadUserDataByUserTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, 2);
			if (dtAllUser.Rows.Count > 0)
			{
				grduserdefine.DataSource = dtAllUser;
				grduserdefine.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserFromRefresh()
	{
		try
		{
			((Control)(object)CmbPartyName).Text = string.Empty;
			txtUsername.Clear();
			txtFirstName.Clear();
			txtLastName.Clear();
			txtPassword.Clear();
			butnSave.Visible = true;
			butnUpdate.Visible = false;
			chkboxIsActive.Checked = false;
			LocationsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		UserFromRefresh();
	}

	private void PartyNameFill()
	{
		try
		{
			DataTable dt = CommonServices.GetGlAccountIdandCompanyNameBySupplierCustomerId();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPartyName, "Id", "CompanyName", "Part Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationsBind()
	{
		try
		{
			DataTable dt = CommonServices.CompanyServiceBind();
			if (dt.Rows.Count > 0)
			{
				dtCompanies.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtCompanies.Rows.Add(0, dt.Rows[i]["Id"], dt.Rows[i]["CompName"], true, true);
				}
				if (dtCompanies.Rows.Count > 0)
				{
					grdBranches.DataSource = dtCompanies;
					grdBranches.RetrieveStructure();
					GridSettingLocations();
				}
				else
				{
					grdBranches.ClearStructure();
				}
			}
			else
			{
				grdBranches.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingLocations()
	{
		try
		{
			grdBranches.RootTable.Columns["CompanyId"].Visible = false;
			grdBranches.RootTable.Columns["Id"].Visible = false;
			grdBranches.RootTable.Columns["Location"].Width = 250;
			grdBranches.RootTable.Columns["CompanyId"].EditType = (EditType)0;
			grdBranches.RootTable.Columns["Id"].EditType = (EditType)0;
			grdBranches.RootTable.Columns["Location"].EditType = (EditType)0;
			grdBranches.RootTable.Columns["Active"].EditType = (EditType)8;
			grdBranches.RootTable.Columns["UserStatus"].EditType = (EditType)8;
			grdBranches.RootTable.Columns["Active"].UseHeaderSelector = true;
			grdBranches.RootTable.Columns["UserStatus"].UseHeaderSelector = true;
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (SubUserRadioButton.Checked && ((UltraGridBase)CmbPartyName).ActiveRow == null)
		{
			MessageBox.Show("Party Name Field is Required");
			CmbPartyName.Focus();
			return false;
		}
		if (txtFirstName.Text.Trim() == string.Empty || txtFirstName.Text.Trim() == "0")
		{
			MessageBox.Show("First Name Field is Required");
			txtFirstName.Focus();
			return false;
		}
		if (txtLastName.Text.Trim() == string.Empty || txtLastName.Text.Trim() == "0")
		{
			MessageBox.Show("Last Name Field is Required");
			txtLastName.Focus();
			return false;
		}
		if (txtUsername.Text.Trim() == string.Empty || txtUsername.Text.Trim() == "0")
		{
			MessageBox.Show("UserName Field Required");
			txtUsername.Focus();
			return false;
		}
		if (txtPassword.Text.Trim() == string.Empty || txtPassword.Text.Trim() == "0")
		{
			MessageBox.Show("Password Field Required");
			txtPassword.Focus();
			return false;
		}
		return true;
	}

	private string RemoveSpecialCharacters(string str)
	{
		StringBuilder sb = new StringBuilder();
		foreach (char c in str)
		{
			if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
			{
				sb.Append(c);
			}
		}
		return sb.ToString();
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			UserAccount UA = new UserAccount();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				UA.ID = RecId;
			}
			else
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(CmbPartyName.Value) > 0)
				{
					string configurationByOrgCompandConfigDescription = CommonServices.GetConfigurationByOrgCompandConfigDescription("RootPathForShareFolderStructure");
					string FolderName = RemoveSpecialCharacters(((Control)(object)CmbPartyName).Text);
					string path = configurationByOrgCompandConfigDescription + "\\" + FolderName;
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
				}
			}
			int count = 0;
			UA.UserAccountAllocationsList = new List<UserAccountAllocation>();
			GridEXRow[] rows = grdBranches.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToBool(item.Cells["Active"].Value))
				{
					UserAccountAllocation allocation = new UserAccountAllocation();
					allocation.BranchId = UserAccount.BranchesId;
					allocation.Id = Conversion.ToInt(item.Cells["Id"].Value);
					allocation.CompanyId = Conversion.ToInt(item.Cells["CompanyId"].Value);
					allocation.OrganizationId = UserAccount.OrganizationId;
					if (Conversion.ToBool(item.Cells["UserStatus"].Value))
					{
						allocation.IsActive = 1;
					}
					else
					{
						allocation.IsActive = 0;
					}
					allocation.EntryDate = DateTime.Now;
					UA.UserAccountAllocationsList.Add(allocation);
					count++;
				}
			}
			if (count == 0)
			{
				throw new Exception("Please select Location first");
			}
			UA.OrganizationId = UserAccount.OrganizationId;
			UA.CompanyId = UserAccount.CompanyId;
			UA.EntryUserId = UserAccount.ID;
			UA.EntryDate = DateTime.Now;
			UA.ModifyDate = DateTime.Now;
			UA.ModifyUserId = UserAccount.ID;
			UA.BranchesId = Conversion.ToInt(grdBranches.CurrentRow.Cells["Id"].Value);
			if (MainUserRadioButton.Checked)
			{
				UA.SupplierCustomerId = 0;
			}
			else
			{
				UA.SupplierCustomerId = Conversion.ToInt(CmbPartyName.Value);
			}
			UA.UserName = Conversion.ToString((object)txtUsername.Text);
			UA.FirstName = Conversion.ToString((object)txtFirstName.Text);
			UA.LastName = Conversion.ToString((object)txtLastName.Text);
			UA.Password = Conversion.ToString((object)txtPassword.Text);
			UA.UserTypeId = 2;
			if (chkboxIsActive.Checked)
			{
				UA.IsActive = true;
			}
			else
			{
				UA.IsActive = false;
			}
			UserAccount.Save(UA);
			if (RecId == 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
			}
			UserFromRefresh();
			bindUser();
		}
		catch (Exception)
		{
			throw;
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

	private void grduserdefine_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			butnSave.Visible = false;
			butnUpdate.Visible = true;
			UserAccount ua = new UserAccount();
			RecId = Conversion.ToInt(grduserdefine.CurrentRow.Cells["Id"].Value);
			ua = UserAccount.GetByID(RecId);
			ua.ID = RecId;
			CmbPartyName.Value = ua.SupplierCustomerId;
			txtUsername.Text = ua.UserName;
			txtFirstName.Text = ua.FirstName;
			txtLastName.Text = ua.LastName;
			txtPassword.Text = Encryption.UserPasswordDecryptString(ua.Password);
			chkboxIsActive.Checked = Conversion.ToBool((object)ua.IsActive);
			for (int i = 0; i < ua.UserAccountAllocationsList.Count; i++)
			{
				GridEXRow[] rows = grdBranches.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (ua.UserAccountAllocationsList[i].CompanyId == Conversion.ToInt(r.Cells["CompanyId"].Value))
					{
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdBranches.UpdateData();
						dtCompanies.Rows.Add(ua.UserAccountAllocationsList[i].Id, ua.UserAccountAllocationsList[i].CompanyId, ua.UserAccountAllocationsList[i].CompName, 1, ua.UserAccountAllocationsList[i].IsActive);
					}
				}
			}
			grdBranches.DataSource = dtCompanies;
			grdBranches.RetrieveStructure();
			GridSettingLocations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtUsername_Leave(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grduserdefine).Name);
			if (fs != null)
			{
				grduserdefine.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdBranches_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grdBranches.UpdateData();
	}

	private void MainUserRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (MainUserRadioButton.Checked)
			{
				((Control)(object)CmbPartyName).Enabled = false;
			}
			else
			{
				((Control)(object)CmbPartyName).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SubUserRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (SubUserRadioButton.Checked)
			{
				((Control)(object)CmbPartyName).Enabled = true;
			}
			else
			{
				((Control)(object)CmbPartyName).Enabled = false;
			}
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
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_0d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da4: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.DMS_FolderHierarchy.CustomerPortalRegistration));
		GridEXLayout grduserdefine_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdBranches_DesignTimeLayout = new GridEXLayout();
		Appearance appearance20 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton14 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton15 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton16 = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grduserdefine = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdBranches = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.SubUserRadioButton = new System.Windows.Forms.RadioButton();
		this.CmbPartyName = new UltraCombo();
		this.MainUserRadioButton = new System.Windows.Forms.RadioButton();
		this.label5 = new System.Windows.Forms.Label();
		this.chkboxIsActive = new System.Windows.Forms.CheckBox();
		this.txtLastName = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtFirstName = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtPassword = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtUsername = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew1 = new System.Windows.Forms.ToolStripButton();
		this.butnUpdate = new System.Windows.Forms.ToolStripButton();
		this.butnSave = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grduserdefine).BeginInit();
		this.panel9.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdBranches).BeginInit();
		this.panel16.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		this.panel11.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabControl1.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnNew, this.btnUpdate, this.btnSave, this.toolStripButton4, this.toolStripSeparator1, this.toolStripButton5, this.toolStripButton6, this.toolStripButton13, this.toolStripSeparator4, this.toolStripButton14,
			this.toolStripButton15, this.toolStripButton16
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(576, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnsave_Click);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton4.Text = "&Print";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(64, 24);
		this.toolStripButton5.Text = "&Delete";
		this.toolStripButton6.Image = Architecture.WinApp.Properties.Resources.icons8_copy_16;
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton6.Text = "&Copy";
		this.toolStripButton13.Image = (System.Drawing.Image)resources.GetObject("toolStripButton13.Image");
		this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton13.Name = "toolStripButton13";
		this.toolStripButton13.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton13.Text = "&Paste";
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton14.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton14.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton14.Name = "toolStripButton14";
		this.toolStripButton14.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton14.Text = "&Attachment";
		this.toolStripButton15.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton15.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton15.Name = "toolStripButton15";
		this.toolStripButton15.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton15.Text = "&SMS";
		this.toolStripButton16.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.toolStripButton16.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton16.Name = "toolStripButton16";
		this.toolStripButton16.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton16.Text = "He&lp";
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(70, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(73, 27);
		this.miniToolStrip.TabIndex = 20;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1314, 6);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 19;
		this.tabPage3.Controls.Add(this.panel10);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1010, 560);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "User Define";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel8);
		this.panel10.Controls.Add(this.panel9);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Controls.Add(this.panel7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1010, 560);
		this.panel10.TabIndex = 3;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grduserdefine);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 196);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1010, 364);
		this.panel8.TabIndex = 62;
		this.grduserdefine.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grduserdefine).BackColor = System.Drawing.Color.White;
		grduserdefine_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grduserdefine.DesignTimeLayout = grduserdefine_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grduserdefine).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grduserdefine).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grduserdefine.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grduserdefine).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grduserdefine).Name = "grduserdefine";
		((ScrollableBase)this.grduserdefine).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grduserdefine).Size = new System.Drawing.Size(1010, 364);
		((System.Windows.Forms.Control)(object)this.grduserdefine).TabIndex = 0;
		this.grduserdefine.TotalRow = (InheritableBoolean)1;
		this.grduserdefine.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grduserdefine).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grduserdefine).DoubleClick += new System.EventHandler(grduserdefine_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add(this.panel17);
		this.panel9.Controls.Add(this.panel16);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 72);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1010, 124);
		this.panel9.TabIndex = 61;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdBranches);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(528, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(478, 124);
		this.panel17.TabIndex = 48;
		((ScrollableBase)this.grdBranches).BackColor = System.Drawing.Color.White;
		grdBranches_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdBranches.DesignTimeLayout = grdBranches_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdBranches).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdBranches.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdBranches).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdBranches).Name = "grdBranches";
		((ScrollableBase)this.grdBranches).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdBranches).Size = new System.Drawing.Size(478, 122);
		((System.Windows.Forms.Control)(object)this.grdBranches).TabIndex = 0;
		this.grdBranches.TotalRow = (InheritableBoolean)1;
		this.grdBranches.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdBranches).VisualStyle = (VisualStyle)4;
		this.grdBranches.CellValueChanged += new ColumnActionEventHandler(grdBranches_CellUpdated);
		this.panel16.Controls.Add(this.groupBox1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(528, 124);
		this.panel16.TabIndex = 47;
		this.groupBox1.Controls.Add(this.SubUserRadioButton);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
		this.groupBox1.Controls.Add(this.MainUserRadioButton);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.chkboxIsActive);
		this.groupBox1.Controls.Add(this.txtLastName);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtFirstName);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtPassword);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtUsername);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Location = new System.Drawing.Point(3, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(525, 116);
		this.groupBox1.TabIndex = 46;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "User Detail";
		this.SubUserRadioButton.AutoSize = true;
		this.SubUserRadioButton.Checked = true;
		this.SubUserRadioButton.Location = new System.Drawing.Point(189, 9);
		this.SubUserRadioButton.Name = "SubUserRadioButton";
		this.SubUserRadioButton.Size = new System.Drawing.Size(69, 17);
		this.SubUserRadioButton.TabIndex = 1;
		this.SubUserRadioButton.TabStop = true;
		this.SubUserRadioButton.Text = "Sub User";
		this.SubUserRadioButton.UseVisualStyleBackColor = true;
		this.SubUserRadioButton.CheckedChanged += new System.EventHandler(SubUserRadioButton_CheckedChanged);
		this.CmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(6, 51);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(214, 22);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 2;
		this.MainUserRadioButton.AutoSize = true;
		this.MainUserRadioButton.Location = new System.Drawing.Point(96, 9);
		this.MainUserRadioButton.Name = "MainUserRadioButton";
		this.MainUserRadioButton.Size = new System.Drawing.Size(73, 17);
		this.MainUserRadioButton.TabIndex = 0;
		this.MainUserRadioButton.Text = "Main User";
		this.MainUserRadioButton.UseVisualStyleBackColor = true;
		this.MainUserRadioButton.CheckedChanged += new System.EventHandler(MainUserRadioButton_CheckedChanged);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(6, 32);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(62, 13);
		this.label5.TabIndex = 12;
		this.label5.Text = "Party Name";
		this.chkboxIsActive.AutoSize = true;
		this.chkboxIsActive.Checked = true;
		this.chkboxIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkboxIsActive.Location = new System.Drawing.Point(374, 96);
		this.chkboxIsActive.Name = "chkboxIsActive";
		this.chkboxIsActive.Size = new System.Drawing.Size(64, 17);
		this.chkboxIsActive.TabIndex = 7;
		this.chkboxIsActive.Text = "IsActive";
		this.chkboxIsActive.UseVisualStyleBackColor = true;
		this.txtLastName.Location = new System.Drawing.Point(118, 94);
		this.txtLastName.Name = "txtLastName";
		this.txtLastName.Size = new System.Drawing.Size(102, 20);
		this.txtLastName.TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(115, 75);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(58, 13);
		this.label4.TabIndex = 6;
		this.label4.Text = "Last Name";
		this.txtFirstName.Location = new System.Drawing.Point(6, 94);
		this.txtFirstName.Name = "txtFirstName";
		this.txtFirstName.Size = new System.Drawing.Size(106, 20);
		this.txtFirstName.TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(6, 75);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 13);
		this.label3.TabIndex = 4;
		this.label3.Text = "First Name";
		this.txtPassword.Location = new System.Drawing.Point(226, 94);
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '*';
		this.txtPassword.Size = new System.Drawing.Size(138, 20);
		this.txtPassword.TabIndex = 6;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(227, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(53, 13);
		this.label2.TabIndex = 2;
		this.label2.Text = "Password";
		this.txtUsername.Location = new System.Drawing.Point(226, 52);
		this.txtUsername.Name = "txtUsername";
		this.txtUsername.Size = new System.Drawing.Size(138, 20);
		this.txtUsername.TabIndex = 3;
		this.txtUsername.Leave += new System.EventHandler(txtUsername_Leave);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(228, 32);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(60, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "User Name";
		this.panel11.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel11.Controls.Add(this.label15);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 32);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1010, 40);
		this.panel11.TabIndex = 60;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Segoe UI", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(10, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(297, 30);
		this.label15.TabIndex = 0;
		this.label15.Text = "Customer Portal Registration";
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1010, 32);
		this.panel7.TabIndex = 0;
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1010, 31);
		this.panel12.TabIndex = 2;
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.ctrlGrdBar3);
		this.panel13.Controls.Add(this.toolStrip3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Margin = new System.Windows.Forms.Padding(2);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1010, 30);
		this.panel13.TabIndex = 1;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(956, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grduserdefine;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 30);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew1, this.butnUpdate, this.butnSave });
		this.toolStrip3.Location = new System.Drawing.Point(0, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(122, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew1.Name = "btnNew1";
		this.btnNew1.Size = new System.Drawing.Size(55, 24);
		this.btnNew1.Text = "&New";
		this.btnNew1.Click += new System.EventHandler(btnnew_Click);
		this.butnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.butnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.butnUpdate.Name = "butnUpdate";
		this.butnUpdate.Size = new System.Drawing.Size(69, 24);
		this.butnUpdate.Text = "&Update";
		this.butnUpdate.Visible = false;
		this.butnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.butnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.butnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.butnSave.Name = "butnSave";
		this.butnSave.Size = new System.Drawing.Size(55, 24);
		this.butnSave.Text = "&Save";
		this.butnSave.Click += new System.EventHandler(btnsave_Click);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1018, 586);
		this.tabControl1.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1018, 586);
		base.Controls.Add(this.tabControl1);
		base.Name = "CustomerPortalRegistration";
		this.Text = "  EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmUserRights_Load);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grduserdefine).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdBranches).EndInit();
		this.panel16.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
