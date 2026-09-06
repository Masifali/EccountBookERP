using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.VPS;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.BookingOffice;
using Architecture.WinApp.Common;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Templates;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Configurations;

public class frmUserRights : Form
{
	private int RecId;

	private DataTable dtrights = new DataTable();

	private DataTable dtAllUser = new DataTable();

	private DataTable dtCompanies = new DataTable();

	private string uId = "";

	private string ScreenName = "";

	private bool DoesHasSupplierPortalApp;

	private bool HasCustomerPortalRight;

	private bool DeviceDependencyFeature;

	private DropDownBind DDL = new DropDownBind();

	private string ImageNameOrPath = "";

	private int ProfileId;

	private ServerCredentials Sc = new ServerCredentials();

	private bool IsVpsAttachmentsServiceOn;

	private bool TwoWayAuthentication;

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

	private TabControl tabControl1;

	private TabPage tabPage3;

	private Panel PanelUserDefine;

	internal GridEX grduserdefine;

	private Panel panel9;

	private Panel panel17;

	internal GridEX grdBranches;

	private Panel panel16;

	private GroupBox groupBox1;

	internal UltraCombo CmbRoleName;

	private Label label6;

	internal UltraCombo CmbUserName;

	private CheckBox chkboxIsActive;

	private Label label5;

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

	private Panel panel13;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew1;

	private ToolStripButton butnUpdate;

	private ToolStripButton butnSave;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel4;

	internal GridEX grdUser;

	private Panel panel3;

	private Panel panel6;

	private Label label23;

	private Panel panel2;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private ToolStrip miniToolStrip;

	private ToolStripButton BtnUser;

	private Label label16;

	private TextBox txtFatherName;

	private Label label8;

	private Label label24;

	private Label label20;

	private Label label14;

	private Label label13;

	private PictureBox ProfileImagePictureBox;

	private Label label7;

	private Label label19;

	private Label label18;

	private Label label17;

	private MaskedTextBox txtCellNo;

	private Label label10;

	private MaskedTextBox txtEmail;

	private Label label9;

	private MaskedTextBox txtCnicNo;

	private Label label11;

	private MaskedTextBox txtWhatsAppNo;

	private Label label12;

	private Label label25;

	private Button btnClearImage;

	private Button browse;

	private TabControl tabControlForForms;

	private TabPage tabDefineUser;

	private Panel PanelForViewForm;

	private CheckBox chkEnableAuthentication;

	private FlowLayoutPanel flowLayoutPanel1;

	private CheckBox ChkDeviceDepenedency;

	private ToolStripButton BtnDeviceAllocation;

	public frmUserRights(UserAccount user)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmUserRights_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1000, 600);
			DeviceDependencyFeature = CommonServices.GetERPFeatureById(16);
			BtnDeviceAllocation.Visible = DeviceDependencyFeature;
			ChkDeviceDepenedency.Visible = DeviceDependencyFeature;
			dtCompanies.Columns.Add("Id");
			dtCompanies.Columns.Add("CompanyId");
			dtCompanies.Columns.Add("Location");
			dtCompanies.Columns.Add("Active", typeof(bool));
			dtCompanies.Columns.Add("UserStatus", typeof(bool));
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn"));
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			txtEmail.Mask = "";
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "TwoWayAuthentication");
			TwoWayAuthentication = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			if (TwoWayAuthentication)
			{
				chkEnableAuthentication.Visible = true;
			}
			else
			{
				chkEnableAuthentication.Visible = false;
			}
			bindUser();
			UserNameFill();
			UserRole();
			LocationsBind();
			dtrights.Columns.Add("ID");
			dtrights.Columns.Add("ScreenName");
			dtrights.Columns.Add("Rights");
			txtUsername.Focus();
			AddTabPage("tabApplicationsAllocateToUser");
			ApplicationsAllocateToCompany obj2 = clsGlobalVariables.GetApplicationsAllocateToCompanyList.Find((ApplicationsAllocateToCompany row) => row.AppId == 5 && row.CompanyId == UserAccount.CompanyId);
			DoesHasSupplierPortalApp = obj2 != null && obj2.IsActive;
			if (DoesHasSupplierPortalApp)
			{
				AddTabPage("tabUsersWithCostCenter");
				AddTabPage("tabChartOfAccountAllocateCostCenter");
				AddTabPage("tabSupplierCustomerAllocateCostCenter");
			}
			ApplicationsAllocateToCompany obj3 = clsGlobalVariables.GetApplicationsAllocateToCompanyList.Find((ApplicationsAllocateToCompany row) => (row.AppId == 4 || row.AppId == 6) && row.CompanyId == UserAccount.CompanyId);
			HasCustomerPortalRight = obj3 != null && obj3.IsActive;
			if (HasCustomerPortalRight)
			{
				AddTabPage("tabCustomerPortalRegistration");
			}
			PanelForViewForm.Visible = false;
			PanelUserDefine.Visible = true;
			PanelUserDefine.Dock = DockStyle.Fill;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdUser.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdUser.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdUser.RootTable.Columns["ID"].Visible = false;
			grdUser.RootTable.Columns["AppId"].Visible = false;
			grdUser.RootTable.Columns["UserGroupName"].Visible = false;
			grdUser.RootTable.Columns["Title"].Caption = "Role Name";
			grdUser.RootTable.Columns["AuthenticationEnabled"].Visible = TwoWayAuthentication;
			grdUser.RootTable.Columns["DeviceDependency"].Visible = DeviceDependencyFeature;
			grdUser.RootTable.Columns["UserName"].Width = 120;
			grdUser.RootTable.Columns["Password"].Width = 100;
			grdUser.RootTable.Columns["UserGroupName"].Width = 100;
			grdUser.RootTable.Columns["FirstName"].Width = 120;
			grdUser.RootTable.Columns["LastName"].Width = 120;
			grdUser.RootTable.Columns["FatherName"].Width = 120;
			grdUser.RootTable.Columns["Title"].Width = 120;
			grdUser.RootTable.Columns["WhatsApp"].Width = 120;
			grdUser.RootTable.Columns["PhoneNumber"].Width = 120;
			grdUser.RootTable.Columns["CNIC"].Width = 120;
			grdUser.RootTable.Columns["Email"].Width = 120;
			grdUser.RootTable.Columns["IsActive"].Width = 60;
			grdUser.RootTable.Columns["AuthenticationEnabled"].Width = 180;
			grduserdefine.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item2 in (JanusCollectionBase)grduserdefine.RootTable.Columns)
			{
				item2.HeaderAlignment = (TextAlignment)2;
			}
			grduserdefine.RootTable.Columns["UserGroupName"].Visible = false;
			grduserdefine.RootTable.Columns["ID"].Visible = false;
			grduserdefine.RootTable.Columns["AppId"].Visible = false;
			grduserdefine.RootTable.Columns["Title"].Caption = "Role Name";
			grduserdefine.RootTable.Columns["AuthenticationEnabled"].Visible = TwoWayAuthentication;
			grduserdefine.RootTable.Columns["UserName"].Width = 120;
			grduserdefine.RootTable.Columns["Password"].Width = 100;
			grduserdefine.RootTable.Columns["UserGroupName"].Width = 100;
			grduserdefine.RootTable.Columns["FirstName"].Width = 120;
			grduserdefine.RootTable.Columns["LastName"].Width = 120;
			grduserdefine.RootTable.Columns["FatherName"].Width = 120;
			grduserdefine.RootTable.Columns["Title"].Width = 120;
			grduserdefine.RootTable.Columns["WhatsApp"].Width = 120;
			grduserdefine.RootTable.Columns["PhoneNumber"].Width = 120;
			grduserdefine.RootTable.Columns["CNIC"].Width = 120;
			grduserdefine.RootTable.Columns["Email"].Width = 120;
			grduserdefine.RootTable.Columns["IsActive"].Width = 60;
			grduserdefine.RootTable.Columns["AuthenticationEnabled"].Width = 180;
			ctrlGrdBar3_Load(null, null);
			ctrlGrdBar1_Load(null, null);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UserAccount obj = new UserAccount();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtAllUser = UserAccount.ReadAll(obj);
			if (dtAllUser.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ID", typeof(int));
				dt.Columns.Add("UserName", typeof(string));
				dt.Columns.Add("Password", typeof(string));
				dt.Columns.Add("UserGroupName", typeof(string));
				dt.Columns.Add("FirstName", typeof(string));
				dt.Columns.Add("LastName", typeof(string));
				dt.Columns.Add("FatherName", typeof(string));
				dt.Columns.Add("Title", typeof(string));
				dt.Columns.Add("WhatsApp", typeof(string));
				dt.Columns.Add("PhoneNumber", typeof(string));
				dt.Columns.Add("CNIC", typeof(string));
				dt.Columns.Add("Email", typeof(string));
				dt.Columns.Add("IsActive", typeof(bool));
				dt.Columns.Add("AppId", typeof(int));
				dt.Columns.Add("AuthenticationEnabled", typeof(string));
				dt.Columns.Add("DeviceDependency", typeof(string));
				for (int i = 0; i < dtAllUser.Rows.Count; i++)
				{
					dtAllUser.Rows[i]["Password"] = Encryption.UserPasswordDecryptString(Conversion.ToString(dtAllUser.Rows[i]["Password"]));
					dt.Rows.Add(dtAllUser.Rows[i]["ID"], dtAllUser.Rows[i]["UserName"], dtAllUser.Rows[i]["Password"], dtAllUser.Rows[i]["UserGroupName"], dtAllUser.Rows[i]["FirstName"], dtAllUser.Rows[i]["LastName"], dtAllUser.Rows[i]["FatherName"], dtAllUser.Rows[i]["Title"], dtAllUser.Rows[i]["WhatsApp"], dtAllUser.Rows[i]["PhoneNumber"], dtAllUser.Rows[i]["CNICNumber"], dtAllUser.Rows[i]["Email"], dtAllUser.Rows[i]["IsActive"], dtAllUser.Rows[i]["AppId"], (Conversion.ToInt(dtAllUser.Rows[i]["AuthenticationEnabled"]) == 1) ? " Two Way Authentication Enabled" : "Authentication not Enabled", dtAllUser.Rows[i]["DeviceDependency"]);
				}
				grdUser.DataSource = dt;
				grdUser.RetrieveStructure();
				grduserdefine.DataSource = dt;
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
			txtUsername.Clear();
			txtPassword.Clear();
			txtFirstName.Clear();
			txtLastName.Clear();
			CmbUserName.Value = 0;
			ProfileId = 0;
			ImageNameOrPath = "";
			txtFatherName.Clear();
			txtWhatsAppNo.Text = string.Empty;
			txtCellNo.Text = string.Empty;
			txtEmail.Text = string.Empty;
			txtCnicNo.Text = string.Empty;
			ProfileImagePictureBox.Image = null;
			ProfileImagePictureBox.Tag = null;
			ProfileImagePictureBox.Invalidate();
			chkEnableAuthentication.Checked = false;
			butnSave.Visible = true;
			butnUpdate.Visible = false;
			chkboxIsActive.Checked = false;
			ChkDeviceDepenedency.Checked = false;
			UserNameFill();
			LocationsBind();
			txtFirstName.Focus();
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

	private void UserNameFill()
	{
		try
		{
			if (dtAllUser.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UserName");
			for (int i = 0; i < dtAllUser.Rows.Count; i++)
			{
				if (dtAllUser.Rows[i]["Title"].ToString() != "Admin")
				{
					dt.Rows.Add(dtAllUser.Rows[i]["Id"], dtAllUser.Rows[i]["UserName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbUserName, "Id", "UserName", "User Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserRole()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = UserAccount.ReadAllRole(new UserAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbRoleName, "Id", "RoleDescription", "RoleDescription", ZeroIndex: false);
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
					dtCompanies.Rows.Add(0, dt.Rows[i]["Id"], dt.Rows[i]["CompName"], 0, 0);
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
			grdBranches.RootTable.Columns["UserStatus"].Visible = false;
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

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
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
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			int count = 0;
			UA.UserAccountAllocationsList = new List<UserAccountAllocation>();
			GridEXRow[] rows = grdBranches.GetRows();
			foreach (GridEXRow item in rows)
			{
				UserAccountAllocation allocation = new UserAccountAllocation();
				allocation.BranchId = UserAccount.BranchesId;
				allocation.Id = Conversion.ToInt(item.Cells["Id"].Value);
				allocation.CompanyId = Conversion.ToInt(item.Cells["CompanyId"].Value);
				allocation.OrganizationId = UserAccount.OrganizationId;
				if (Conversion.ToBool(item.Cells["Active"].Value))
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
			if (count == 0)
			{
				throw new Exception("Please select Location first");
			}
			UA.CompanyId = UserAccount.CompanyId;
			UA.OrganizationId = UserAccount.OrganizationId;
			UA.EntryUserId = UserAccount.ID;
			UA.EntryDate = DateTime.Now;
			UA.ModifyDate = DateTime.Now;
			UA.BranchesId = Conversion.ToInt(grdBranches.CurrentRow.Cells["Id"].Value);
			if (chkboxIsActive.Checked)
			{
				UA.IsActive = true;
			}
			else
			{
				UA.IsActive = false;
			}
			UA.AuthenticationEnabledForUser = Conversion.ToInt((object)chkEnableAuthentication.Checked);
			UA.UserName = Conversion.ToString((object)txtUsername.Text);
			UA.Password = Conversion.ToString((object)txtPassword.Text);
			UA.FirstName = Conversion.ToString((object)txtFirstName.Text);
			UA.LastName = Conversion.ToString((object)txtLastName.Text);
			UA.UserRoleId = Conversion.ToInt(CmbRoleName.Value);
			UA.UserGroupId = Conversion.ToInt(CmbUserName.Value);
			UA.DeviceDependency = DeviceDependencyFeature && ChkDeviceDepenedency.Checked;
			UA.UserProfileList = new UserProfile();
			UA.UserProfileList.Id = ProfileId;
			UA.UserProfileList.FatherName = txtFatherName.Text;
			if (!ValidatePhoneNumber(txtWhatsAppNo.Text))
			{
				txtWhatsAppNo.Focus();
				throw new Exception("WhatsApp No is not Valid");
			}
			if (!ValidatePhoneNumber(txtCellNo.Text))
			{
				txtCellNo.Focus();
				throw new Exception("Cell No is not Valid");
			}
			if (!ValidateCNIC(txtCnicNo.Text))
			{
				txtCnicNo.Focus();
				throw new Exception("CNIC is not Valid");
			}
			if (!ValidateEmail(txtEmail.Text))
			{
				txtEmail.Focus();
				throw new Exception("Email is not Valid");
			}
			UA.UserProfileList.WhatsApp = txtWhatsAppNo.Text;
			UA.UserProfileList.CellNo = txtCellNo.Text;
			UA.UserProfileList.CNICNumber = txtCnicNo.Text;
			UA.UserProfileList.Email = txtEmail.Text;
			UA.UserProfileList.JoiningDate = DateTime.Now;
			UA.UserProfileList.ProfileImageFileName = ImageNameOrPath;
			if (Conversion.ToString(ProfileImagePictureBox.Tag) != null && Conversion.ToString(ProfileImagePictureBox.Tag) != string.Empty && ImageNameOrPath != Conversion.ToString(ProfileImagePictureBox.Tag))
			{
				UA.UserProfileList.PictureToDeleteInUpdateIfAny = Conversion.ToString(ProfileImagePictureBox.Tag);
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
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private static bool ValidatePhoneNumber(string phoneNumber)
	{
		return new Regex("^\\d{4}-\\d{3}-\\d{4}$").IsMatch(phoneNumber);
	}

	private bool ValidateEmail(string Mail)
	{
		return new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").IsMatch(Mail);
	}

	private static bool ValidateCNIC(string cnic)
	{
		return new Regex("^\\d{5}-\\d{7}-\\d{1}$").IsMatch(cnic);
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
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool FormValidation()
	{
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
		if (txtFirstName.Text.Trim() == string.Empty || txtFirstName.Text.Trim() == "0")
		{
			MessageBox.Show("First Name Field Required");
			txtFirstName.Focus();
			return false;
		}
		if (txtLastName.Text.Trim() == string.Empty || txtLastName.Text.Trim() == "0")
		{
			MessageBox.Show("Last Name Field Required");
			txtLastName.Focus();
			return false;
		}
		if (txtFatherName.Text.Trim() == string.Empty || txtFatherName.Text.Trim() == "0")
		{
			MessageBox.Show("Father Name Field Required");
			txtFatherName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRoleName).ActiveRow == null)
		{
			MessageBox.Show("Role Name Field Required");
			CmbRoleName.Focus();
			return false;
		}
		return true;
	}

	private void grduserdefine_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			butnSave.Visible = false;
			butnUpdate.Visible = true;
			UserAccount ua = new UserAccount();
			RecId = Conversion.ToInt(grduserdefine.CurrentRow.Cells["ID"].Value);
			ua = UserAccount.GetByID(RecId);
			if (ua == null)
			{
				return;
			}
			ua.ID = RecId;
			txtFirstName.Text = ua.FirstName;
			txtLastName.Text = ua.LastName;
			txtUsername.Text = ua.UserName;
			txtPassword.Text = Encryption.UserPasswordDecryptString(ua.Password);
			CmbRoleName.Value = ua.UserRoleId;
			chkboxIsActive.Checked = Conversion.ToBool((object)ua.IsActive);
			chkEnableAuthentication.Checked = Conversion.ToBool((object)ua.AuthenticationEnabledForUser);
			ChkDeviceDepenedency.Checked = ua.DeviceDependency;
			if (ua.UserProfileList != null)
			{
				ProfileId = ua.UserProfileList.Id;
				txtFatherName.Text = ua.UserProfileList.FatherName;
				txtWhatsAppNo.Text = ua.UserProfileList.WhatsApp;
				txtCellNo.Text = ua.UserProfileList.CellNo;
				txtCnicNo.Text = ua.UserProfileList.CNICNumber;
				txtEmail.Text = ua.UserProfileList.Email;
				ImageNameOrPath = ua.UserProfileList.ProfileImageFileName;
				if (ImageNameOrPath != null && ImageNameOrPath != string.Empty)
				{
					if (!IsVpsAttachmentsServiceOn)
					{
						if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
						{
							throw new Exception("Attachment Folder Path Not Configure Properly");
						}
						ProfileImagePictureBox.Image = Image.FromFile(Path.Combine(clsGlobalVariables.AttachmentFolderPath, ImageNameOrPath));
						ProfileImagePictureBox.Tag = ua.UserProfileList.ProfileImageFileName;
					}
					else
					{
						string uri = "ftp://" + Sc.ServerIp + "/" + ImageNameOrPath;
						try
						{
							FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(uri);
							obj.Method = "RETR";
							obj.UsePassive = true;
							obj.UseBinary = true;
							obj.KeepAlive = false;
							obj.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							using FtpWebResponse ftpWebResponse = (FtpWebResponse)obj.GetResponse();
							using Stream responseStream = ftpWebResponse.GetResponseStream();
							using MemoryStream memoryStream = new MemoryStream();
							responseStream.CopyTo(memoryStream);
							_ = (double)memoryStream.Length / 1048576.0;
							memoryStream.Position = 0L;
							using MemoryStream stream = new MemoryStream();
							Image image = Image.FromStream(memoryStream);
							Bitmap bmp = new Bitmap(image);
							bmp.Save(stream, image.RawFormat);
							ProfileImagePictureBox.Image = bmp;
							ProfileImagePictureBox.Tag = ImageNameOrPath;
							image.Dispose();
							stream.Position = 0L;
							stream.Dispose();
						}
						catch (WebException webEx)
						{
							if (webEx.Response is FtpWebResponse response)
							{
								throw new Exception("FTP Error: " + response.StatusDescription + " - " + webEx.Message);
							}
							throw new Exception("Error: " + webEx.Message);
						}
						catch (Exception ex)
						{
							throw new Exception("Error: " + ex.Message);
						}
					}
				}
				else
				{
					ImageNameOrPath = "";
					ProfileImagePictureBox.Image = null;
					ProfileImagePictureBox.Tag = "";
					ProfileImagePictureBox.Invalidate();
				}
			}
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
			txtUsername.Focus();
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdUser).Name);
			if (fs != null)
			{
				grdUser.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdBranches_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grdBranches.UpdateData();
	}

	private void frmUserRights_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnUser_Click(object sender, EventArgs e)
	{
		try
		{
			new frmBranchesAllocationToUser(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void browse_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Path.GetFileName(openFileDialog.FileName);
				ProfileImagePictureBox.Image = new Bitmap(openFileDialog.FileName);
				ImageNameOrPath = openFileDialog.FileName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnClearImage_Click(object sender, EventArgs e)
	{
		try
		{
			ImageNameOrPath = null;
			ProfileImagePictureBox.Image.Dispose();
			ProfileImagePictureBox.Image = null;
			ProfileImagePictureBox.Invalidate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmUserRights_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (btnSave.Visible && e.Control && e.KeyCode == Keys.S)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (btnUpdate.Visible && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	private void AddTabPage(string tabName)
	{
		TabPage existingTab = tabControl1.TabPages.Cast<TabPage>().FirstOrDefault((TabPage tp) => tp.Name == tabName);
		if (existingTab == null)
		{
			existingTab = new TabPage
			{
				Name = tabName,
				Text = tabName.Substring(3)
			};
			tabControlForForms.TabPages.Add(existingTab);
		}
	}

	private void tabControlForForms_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			PanelForViewForm.Visible = false;
			PanelUserDefine.Visible = true;
			switch (tabControlForForms.SelectedTab.Name)
			{
			case "tabDefineUser":
				PanelUserDefine.Dock = DockStyle.Fill;
				break;
			case "tabApplicationsAllocateToUser":
			{
				string screenName5 = "ApplicationsAllocateToUser";
				frmApplicationsAllocateToUser form5 = new frmApplicationsAllocateToUser(UserAccount);
				OpenReportForm(screenName5, form5);
				PanelForViewForm.Visible = true;
				PanelUserDefine.Visible = false;
				PanelForViewForm.Dock = DockStyle.Fill;
				break;
			}
			case "tabSupplierCustomerAllocateCostCenter":
			{
				string screenName4 = "frmSupplierCustomerAllocateCostCenter";
				frmSupplierCustomerAllocateCostCenter form4 = new frmSupplierCustomerAllocateCostCenter(UserAccount);
				OpenReportForm(screenName4, form4);
				PanelForViewForm.Visible = true;
				PanelUserDefine.Visible = false;
				PanelForViewForm.Dock = DockStyle.Fill;
				break;
			}
			case "tabChartOfAccountAllocateCostCenter":
			{
				string screenName3 = "frmChartOfAccountAllocateCostCenter";
				frmChartOfAccountAllocateCostCenter form3 = new frmChartOfAccountAllocateCostCenter(UserAccount);
				OpenReportForm(screenName3, form3);
				PanelForViewForm.Visible = true;
				PanelUserDefine.Visible = false;
				PanelForViewForm.Dock = DockStyle.Fill;
				break;
			}
			case "tabUsersWithCostCenter":
			{
				string screenName2 = "frmUsersWithCostCenter";
				frmUsersWithCostCenter form2 = new frmUsersWithCostCenter(UserAccount);
				OpenReportForm(screenName2, form2);
				PanelForViewForm.Visible = true;
				PanelUserDefine.Visible = false;
				PanelForViewForm.Dock = DockStyle.Fill;
				break;
			}
			case "tabCustomerPortalRegistration":
			{
				string screenName = "CustomerRegistration_MobileApp";
				CustomerRegistration_MobileApp form = new CustomerRegistration_MobileApp(UserAccount);
				OpenReportForm(screenName, form);
				PanelForViewForm.Visible = true;
				PanelUserDefine.Visible = false;
				PanelForViewForm.Dock = DockStyle.Fill;
				break;
			}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenReportForm(string screenName, Form formInstance)
	{
		try
		{
			if (PanelForViewForm.Controls == null || PanelForViewForm.Controls.Count == 0)
			{
				ScreenName = screenName;
			}
			foreach (Form form in PanelForViewForm.Controls.OfType<Form>())
			{
				if (form.Tag == null || form.Tag.ToString() != screenName)
				{
					form.Hide();
				}
				if (form.Tag.ToString() == ScreenName)
				{
					PanelForViewForm.Controls.Remove(form);
					ScreenName = "";
				}
			}
			Form existingForm = PanelForViewForm.Controls.OfType<Form>().FirstOrDefault((Form f) => f.Tag != null && f.Tag.ToString() == screenName);
			if (existingForm != null)
			{
				existingForm.WindowState = FormWindowState.Maximized;
				existingForm.Dock = DockStyle.Fill;
				existingForm.Show();
				return;
			}
			formInstance.Tag = screenName;
			formInstance.TopLevel = false;
			formInstance.AutoScroll = true;
			formInstance.FormBorderStyle = FormBorderStyle.None;
			formInstance.WindowState = FormWindowState.Maximized;
			PanelForViewForm.Controls.Add(formInstance);
			formInstance.Show();
			PanelForViewForm.Refresh();
			formInstance.WindowState = FormWindowState.Maximized;
			formInstance.Dock = DockStyle.Fill;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdBranches_Click(object sender, EventArgs e)
	{
		grdBranches.UpdateData();
	}

	private void BtnDeviceAllocation_Click(object sender, EventArgs e)
	{
		try
		{
			new UserDevices(UserAccount).Show();
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
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_1662: Unknown result type (might be due to invalid IL or missing references)
		//IL_166c: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Configurations.frmUserRights));
		GridEXLayout grdUser_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grduserdefine_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdBranches_DesignTimeLayout = new GridEXLayout();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
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
		this.grdUser = new GridEX();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.PanelForViewForm = new System.Windows.Forms.Panel();
		this.PanelUserDefine = new System.Windows.Forms.Panel();
		this.grduserdefine = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdBranches = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.chkEnableAuthentication = new System.Windows.Forms.CheckBox();
		this.ChkDeviceDepenedency = new System.Windows.Forms.CheckBox();
		this.chkboxIsActive = new System.Windows.Forms.CheckBox();
		this.label25 = new System.Windows.Forms.Label();
		this.btnClearImage = new System.Windows.Forms.Button();
		this.browse = new System.Windows.Forms.Button();
		this.label24 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.ProfileImagePictureBox = new System.Windows.Forms.PictureBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtCellNo = new System.Windows.Forms.MaskedTextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.MaskedTextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtCnicNo = new System.Windows.Forms.MaskedTextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtWhatsAppNo = new System.Windows.Forms.MaskedTextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtFatherName = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbRoleName = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbUserName = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtLastName = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtFirstName = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtPassword = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtUsername = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew1 = new System.Windows.Forms.ToolStripButton();
		this.butnUpdate = new System.Windows.Forms.ToolStripButton();
		this.butnSave = new System.Windows.Forms.ToolStripButton();
		this.BtnUser = new System.Windows.Forms.ToolStripButton();
		this.BtnDeviceAllocation = new System.Windows.Forms.ToolStripButton();
		this.tabControlForForms = new System.Windows.Forms.TabControl();
		this.tabDefineUser = new System.Windows.Forms.TabPage();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUser).BeginInit();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.PanelUserDefine.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grduserdefine).BeginInit();
		this.panel9.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdBranches).BeginInit();
		this.panel16.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ProfileImagePictureBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRoleName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).BeginInit();
		this.panel11.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel13.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabControlForForms.SuspendLayout();
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
		this.grdUser.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdUser).BackColor = System.Drawing.Color.White;
		grdUser_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdUser.DesignTimeLayout = grdUser_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdUser).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdUser.DynamicFiltering = true;
		this.grdUser.FilterMode = (FilterMode)2;
		this.grdUser.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdUser).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdUser.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdUser).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdUser).Name = "grdUser";
		((ScrollableBase)this.grdUser).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdUser).Size = new System.Drawing.Size(970, 465);
		((System.Windows.Forms.Control)(object)this.grdUser).TabIndex = 44;
		this.grdUser.TotalRow = (InheritableBoolean)1;
		this.grdUser.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdUser).VisualStyle = (VisualStyle)4;
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
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(976, 535);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "User History";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(970, 529);
		this.panel1.TabIndex = 1;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdUser);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 64);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(970, 465);
		this.panel4.TabIndex = 2;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(970, 34);
		this.panel3.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(970, 34);
		this.panel6.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(111, 24);
		this.label23.TabIndex = 0;
		this.label23.Text = "User History";
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(970, 30);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(970, 31);
		this.panel5.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(922, 6);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdUser;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 19;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.New });
		this.toolStrip2.Location = new System.Drawing.Point(0, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(67, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(55, 24);
		this.New.Text = "&New";
		this.tabPage3.Controls.Add(this.PanelForViewForm);
		this.tabPage3.Controls.Add(this.PanelUserDefine);
		this.tabPage3.Controls.Add(this.tabControlForForms);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(976, 535);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "User Define";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.PanelForViewForm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelForViewForm.Location = new System.Drawing.Point(972, 27);
		this.PanelForViewForm.Name = "PanelForViewForm";
		this.PanelForViewForm.Size = new System.Drawing.Size(4, 508);
		this.PanelForViewForm.TabIndex = 4;
		this.PanelUserDefine.Controls.Add((System.Windows.Forms.Control)(object)this.grduserdefine);
		this.PanelUserDefine.Controls.Add(this.panel9);
		this.PanelUserDefine.Controls.Add(this.panel11);
		this.PanelUserDefine.Controls.Add(this.panel7);
		this.PanelUserDefine.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelUserDefine.Location = new System.Drawing.Point(0, 27);
		this.PanelUserDefine.Name = "PanelUserDefine";
		this.PanelUserDefine.Size = new System.Drawing.Size(972, 508);
		this.PanelUserDefine.TabIndex = 3;
		this.grduserdefine.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grduserdefine).BackColor = System.Drawing.Color.White;
		grduserdefine_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grduserdefine.DesignTimeLayout = grduserdefine_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grduserdefine).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grduserdefine).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grduserdefine.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grduserdefine).Location = new System.Drawing.Point(0, 240);
		((System.Windows.Forms.Control)(object)this.grduserdefine).Name = "grduserdefine";
		((ScrollableBase)this.grduserdefine).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grduserdefine).Size = new System.Drawing.Size(972, 268);
		((System.Windows.Forms.Control)(object)this.grduserdefine).TabIndex = 0;
		this.grduserdefine.TotalRow = (InheritableBoolean)1;
		this.grduserdefine.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grduserdefine).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grduserdefine).DoubleClick += new System.EventHandler(grduserdefine_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add(this.panel17);
		this.panel9.Controls.Add(this.panel16);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 59);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(972, 181);
		this.panel9.TabIndex = 61;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdBranches);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(705, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(481, 181);
		this.panel17.TabIndex = 48;
		((ScrollableBase)this.grdBranches).BackColor = System.Drawing.Color.White;
		grdBranches_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdBranches.DesignTimeLayout = grdBranches_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdBranches).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdBranches).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdBranches.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdBranches).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdBranches).Name = "grdBranches";
		((ScrollableBase)this.grdBranches).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdBranches).Size = new System.Drawing.Size(481, 181);
		((System.Windows.Forms.Control)(object)this.grdBranches).TabIndex = 0;
		this.grdBranches.TotalRow = (InheritableBoolean)1;
		this.grdBranches.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdBranches).VisualStyle = (VisualStyle)4;
		this.grdBranches.CellValueChanged += new ColumnActionEventHandler(grdBranches_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grdBranches).Click += new System.EventHandler(grdBranches_Click);
		this.panel16.Controls.Add(this.groupBox1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(705, 181);
		this.panel16.TabIndex = 47;
		this.groupBox1.Controls.Add(this.flowLayoutPanel1);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.btnClearImage);
		this.groupBox1.Controls.Add(this.browse);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.ProfileImagePictureBox);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtCellNo);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtEmail);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtCnicNo);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtWhatsAppNo);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtFatherName);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRoleName);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUserName);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtLastName);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtFirstName);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtPassword);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtUsername);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(696, 172);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "User Detail";
		this.flowLayoutPanel1.Controls.Add(this.chkEnableAuthentication);
		this.flowLayoutPanel1.Controls.Add(this.ChkDeviceDepenedency);
		this.flowLayoutPanel1.Controls.Add(this.chkboxIsActive);
		this.flowLayoutPanel1.Location = new System.Drawing.Point(280, 117);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(229, 53);
		this.flowLayoutPanel1.TabIndex = 10;
		this.chkEnableAuthentication.AutoSize = true;
		this.chkEnableAuthentication.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkEnableAuthentication.Location = new System.Drawing.Point(3, 3);
		this.chkEnableAuthentication.Name = "chkEnableAuthentication";
		this.chkEnableAuthentication.Size = new System.Drawing.Size(143, 19);
		this.chkEnableAuthentication.TabIndex = 0;
		this.chkEnableAuthentication.Text = "Enable Authentication";
		this.chkEnableAuthentication.UseVisualStyleBackColor = true;
		this.ChkDeviceDepenedency.AutoSize = true;
		this.ChkDeviceDepenedency.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkDeviceDepenedency.Location = new System.Drawing.Point(3, 28);
		this.ChkDeviceDepenedency.Name = "ChkDeviceDepenedency";
		this.ChkDeviceDepenedency.Size = new System.Drawing.Size(130, 19);
		this.ChkDeviceDepenedency.TabIndex = 1;
		this.ChkDeviceDepenedency.Text = "Device Depenedent";
		this.ChkDeviceDepenedency.UseVisualStyleBackColor = true;
		this.chkboxIsActive.AutoSize = true;
		this.chkboxIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkboxIsActive.Location = new System.Drawing.Point(139, 28);
		this.chkboxIsActive.Name = "chkboxIsActive";
		this.chkboxIsActive.Size = new System.Drawing.Size(68, 19);
		this.chkboxIsActive.TabIndex = 2;
		this.chkboxIsActive.Text = "IsActive";
		this.chkboxIsActive.UseVisualStyleBackColor = true;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(69, 148);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(14, 17);
		this.label25.TabIndex = 240;
		this.label25.Text = "*";
		this.btnClearImage.BackColor = System.Drawing.Color.Teal;
		this.btnClearImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClearImage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnClearImage.ForeColor = System.Drawing.Color.White;
		this.btnClearImage.Location = new System.Drawing.Point(603, 141);
		this.btnClearImage.Name = "btnClearImage";
		this.btnClearImage.Size = new System.Drawing.Size(88, 25);
		this.btnClearImage.TabIndex = 12;
		this.btnClearImage.Text = "Clear";
		this.btnClearImage.UseVisualStyleBackColor = false;
		this.btnClearImage.Click += new System.EventHandler(btnClearImage_Click);
		this.browse.BackColor = System.Drawing.Color.Teal;
		this.browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.browse.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.browse.ForeColor = System.Drawing.Color.White;
		this.browse.Location = new System.Drawing.Point(512, 141);
		this.browse.Name = "browse";
		this.browse.Size = new System.Drawing.Size(91, 25);
		this.browse.TabIndex = 11;
		this.browse.Text = "Browse";
		this.browse.UseVisualStyleBackColor = false;
		this.browse.Click += new System.EventHandler(browse_Click);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(72, 95);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 17);
		this.label24.TabIndex = 237;
		this.label24.Text = "*";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(72, 70);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 17);
		this.label20.TabIndex = 236;
		this.label20.Text = "*";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Red;
		this.label14.Location = new System.Drawing.Point(72, 45);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(14, 17);
		this.label14.TabIndex = 235;
		this.label14.Text = "*";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(72, 20);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 17);
		this.label13.TabIndex = 234;
		this.label13.Text = "*";
		this.ProfileImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ProfileImagePictureBox.Location = new System.Drawing.Point(512, 15);
		this.ProfileImagePictureBox.Name = "ProfileImagePictureBox";
		this.ProfileImagePictureBox.Size = new System.Drawing.Size(179, 125);
		this.ProfileImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.ProfileImagePictureBox.TabIndex = 233;
		this.ProfileImagePictureBox.TabStop = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(352, 19);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(14, 17);
		this.label7.TabIndex = 231;
		this.label7.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(352, 44);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 229;
		this.label19.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(352, 94);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 228;
		this.label18.Text = "*";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(352, 69);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 17);
		this.label17.TabIndex = 227;
		this.label17.Text = "*";
		this.txtCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCellNo.Location = new System.Drawing.Point(370, 41);
		this.txtCellNo.Mask = "####-###-####";
		this.txtCellNo.Name = "txtCellNo";
		this.txtCellNo.Size = new System.Drawing.Size(139, 23);
		this.txtCellNo.TabIndex = 7;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(277, 45);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(45, 15);
		this.label10.TabIndex = 226;
		this.label10.Text = "Cell No";
		this.txtEmail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmail.Location = new System.Drawing.Point(370, 91);
		this.txtEmail.Mask = "##@##.###";
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(139, 23);
		this.txtEmail.TabIndex = 9;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(277, 95);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(39, 15);
		this.label9.TabIndex = 225;
		this.label9.Text = "Email ";
		this.txtCnicNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCnicNo.Location = new System.Drawing.Point(370, 66);
		this.txtCnicNo.Mask = "00000-0000000-0";
		this.txtCnicNo.Name = "txtCnicNo";
		this.txtCnicNo.Size = new System.Drawing.Size(139, 23);
		this.txtCnicNo.TabIndex = 8;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(277, 70);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(56, 15);
		this.label11.TabIndex = 224;
		this.label11.Text = "CNIC No.";
		this.txtWhatsAppNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWhatsAppNo.Location = new System.Drawing.Point(370, 16);
		this.txtWhatsAppNo.Mask = "####-###-####";
		this.txtWhatsAppNo.Name = "txtWhatsAppNo";
		this.txtWhatsAppNo.Size = new System.Drawing.Size(139, 23);
		this.txtWhatsAppNo.TabIndex = 6;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(277, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(70, 15);
		this.label12.TabIndex = 223;
		this.label12.Text = "WhatsApp#";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(72, 120);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(14, 17);
		this.label16.TabIndex = 123;
		this.label16.Text = "*";
		this.txtFatherName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFatherName.Location = new System.Drawing.Point(87, 117);
		this.txtFatherName.Name = "txtFatherName";
		this.txtFatherName.Size = new System.Drawing.Size(187, 23);
		this.txtFatherName.TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1, 121);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(75, 15);
		this.label8.TabIndex = 122;
		this.label8.Text = "Father Name";
		this.CmbRoleName.AllowNull = (DefaultableBoolean)1;
		this.CmbRoleName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRoleName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRoleName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRoleName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbRoleName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRoleName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRoleName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRoleName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRoleName).Location = new System.Drawing.Point(87, 144);
		((System.Windows.Forms.Control)(object)this.CmbRoleName).Name = "CmbRoleName";
		((System.Windows.Forms.Control)(object)this.CmbRoleName).Size = new System.Drawing.Size(187, 26);
		((System.Windows.Forms.Control)(object)this.CmbRoleName).TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(1, 149);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(65, 15);
		this.label6.TabIndex = 10;
		this.label6.Text = "Role Name";
		this.CmbUserName.AllowNull = (DefaultableBoolean)1;
		this.CmbUserName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUserName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUserName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbUserName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Location = new System.Drawing.Point(512, 175);
		((System.Windows.Forms.Control)(object)this.CmbUserName).Name = "CmbUserName";
		((System.Windows.Forms.Control)(object)this.CmbUserName).Size = new System.Drawing.Size(175, 26);
		((System.Windows.Forms.Control)(object)this.CmbUserName).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbUserName).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(443, 181);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(65, 15);
		this.label5.TabIndex = 8;
		this.label5.Text = "User Name";
		this.label5.Visible = false;
		this.txtLastName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLastName.Location = new System.Drawing.Point(87, 92);
		this.txtLastName.Name = "txtLastName";
		this.txtLastName.Size = new System.Drawing.Size(187, 23);
		this.txtLastName.TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1, 96);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 6;
		this.label4.Text = "Last Name";
		this.txtFirstName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFirstName.Location = new System.Drawing.Point(87, 67);
		this.txtFirstName.Name = "txtFirstName";
		this.txtFirstName.Size = new System.Drawing.Size(187, 23);
		this.txtFirstName.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1, 71);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 4;
		this.label3.Text = "First Name";
		this.txtPassword.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPassword.Location = new System.Drawing.Point(87, 42);
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '*';
		this.txtPassword.Size = new System.Drawing.Size(187, 23);
		this.txtPassword.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1, 46);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 2;
		this.label2.Text = "Password";
		this.txtUsername.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUsername.Location = new System.Drawing.Point(87, 17);
		this.txtUsername.Name = "txtUsername";
		this.txtUsername.Size = new System.Drawing.Size(187, 23);
		this.txtUsername.TabIndex = 0;
		this.txtUsername.Leave += new System.EventHandler(txtUsername_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(1, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(65, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "User Name";
		this.panel11.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label15);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 34);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(972, 25);
		this.panel11.TabIndex = 60;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(930, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grduserdefine;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(1, 1);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(145, 23);
		this.label15.TabIndex = 0;
		this.label15.Text = "Add New User";
		this.panel7.Controls.Add(this.panel13);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(972, 34);
		this.panel7.TabIndex = 0;
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.toolStrip3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Margin = new System.Windows.Forms.Padding(2);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(972, 32);
		this.panel13.TabIndex = 1;
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew1, this.butnUpdate, this.butnSave, this.BtnUser, this.BtnDeviceAllocation });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(972, 32);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew1.Name = "btnNew1";
		this.btnNew1.Size = new System.Drawing.Size(59, 29);
		this.btnNew1.Text = "&New";
		this.btnNew1.Click += new System.EventHandler(btnnew_Click);
		this.butnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.butnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.butnUpdate.Name = "butnUpdate";
		this.butnUpdate.Size = new System.Drawing.Size(78, 29);
		this.butnUpdate.Text = "&Update";
		this.butnUpdate.Visible = false;
		this.butnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.butnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.butnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.butnSave.Name = "butnSave";
		this.butnSave.Size = new System.Drawing.Size(64, 29);
		this.butnSave.Text = "&Save";
		this.butnSave.Click += new System.EventHandler(btnsave_Click);
		this.BtnUser.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUser.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnUser.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUser.Name = "BtnUser";
		this.BtnUser.Size = new System.Drawing.Size(215, 29);
		this.BtnUser.Text = "Branches Allocation To User";
		this.BtnUser.Click += new System.EventHandler(BtnUser_Click);
		this.BtnDeviceAllocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDeviceAllocation.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnDeviceAllocation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDeviceAllocation.Name = "BtnDeviceAllocation";
		this.BtnDeviceAllocation.Size = new System.Drawing.Size(199, 29);
		this.BtnDeviceAllocation.Text = "Device Allocation To User";
		this.BtnDeviceAllocation.Click += new System.EventHandler(BtnDeviceAllocation_Click);
		this.tabControlForForms.Controls.Add(this.tabDefineUser);
		this.tabControlForForms.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControlForForms.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlForForms.Location = new System.Drawing.Point(0, 0);
		this.tabControlForForms.Name = "tabControlForForms";
		this.tabControlForForms.SelectedIndex = 0;
		this.tabControlForForms.Size = new System.Drawing.Size(976, 27);
		this.tabControlForForms.TabIndex = 0;
		this.tabControlForForms.SelectedIndexChanged += new System.EventHandler(tabControlForForms_SelectedIndexChanged);
		this.tabDefineUser.Location = new System.Drawing.Point(4, 25);
		this.tabDefineUser.Name = "tabDefineUser";
		this.tabDefineUser.Padding = new System.Windows.Forms.Padding(3);
		this.tabDefineUser.Size = new System.Drawing.Size(968, 0);
		this.tabDefineUser.TabIndex = 0;
		this.tabDefineUser.Text = "Define User";
		this.tabDefineUser.UseVisualStyleBackColor = true;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(984, 561);
		this.tabControl1.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(100, 6);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdUser;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 19;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 561);
		base.Controls.Add(this.tabControl1);
		base.Name = "frmUserRights";
		this.Text = "  EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmUserRights_FormClosing);
		base.Load += new System.EventHandler(frmUserRights_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmUserRights_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUser).EndInit();
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.PanelUserDefine.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grduserdefine).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdBranches).EndInit();
		this.panel16.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.flowLayoutPanel1.ResumeLayout(false);
		this.flowLayoutPanel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ProfileImagePictureBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRoleName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabControlForForms.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
