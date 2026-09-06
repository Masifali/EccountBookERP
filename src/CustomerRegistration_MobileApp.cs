using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.VPS;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Configurations
{

public class CustomerRegistration_MobileApp : Form
{
	private int RecId;

	private DataTable dtrights = new DataTable();

	private DataTable dtAllUser = new DataTable();

	private DataTable dtCompanies = new DataTable();

	private string uId = "";

	private string ScreenName = "";

	private string ImageNameOrPath = "";

	private int ProfileId;

	private ServerCredentials Sc = new ServerCredentials();

	private bool IsVpsAttachmentsServiceOn;

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

	private Label label6;

	private MaskedTextBox txtCnicNo;

	private Label label7;

	private MaskedTextBox txtWhatsAppNo;

	private MaskedTextBox txtEmail;

	private Label label9;

	private TextBox txtFatherName;

	private Label label8;

	private MaskedTextBox txtCellNo;

	private Label label10;

	private DateTimePicker datJoiningDate;

	private Label label11;

	private Label label22;

	private Label label24;

	private Label label23;

	private Label label21;

	private Label label19;

	private Label label18;

	private Label label17;

	private Label label16;

	private Label label14;

	private Label label13;

	private Label label12;

	private Button browse;

	private PictureBox ProfileImagePictureBox;

	private Button btnClearImage;

	private Panel panel1;

	internal GridEX grdApps;

	public CustomerRegistration_MobileApp(UserAccount user)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
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
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn"));
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			datJoiningDate.ShowCheckBox = true;
			datJoiningDate.Checked = false;
			bindUser();
			PartyNameFill();
			txtEmail.Mask = "";
			LocationsBind();
			FillApps();
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
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grduserdefine.RootTable.Columns["ID"].Visible = false;
			grduserdefine.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grduserdefine.RootTable.Columns["Password"].Visible = false;
			grduserdefine.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grduserdefine.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grduserdefine.RootTable.Columns["PartyName"].Width = 220;
			grduserdefine.RootTable.Columns["UserName"].Width = 100;
			grduserdefine.RootTable.Columns["Password"].Width = 100;
			grduserdefine.RootTable.Columns["FirstName"].Width = 100;
			grduserdefine.RootTable.Columns["LastName"].Width = 100;
			grduserdefine.RootTable.Columns["FatherName"].Width = 150;
			grduserdefine.RootTable.Columns["JoiningDate"].Width = 90;
			grduserdefine.RootTable.Columns["WhatsApp"].Width = 110;
			grduserdefine.RootTable.Columns["CellNo"].Width = 110;
			grduserdefine.RootTable.Columns["CNICNumber"].Width = 120;
			grduserdefine.RootTable.Columns["Email"].Width = 160;
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
			dtAllUser.Rows.Clear();
			dtAllUser = UserAccount.ReadUserDataByUserTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, 3);
			if (dtAllUser.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ID", typeof(int));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("PartyName");
				dt.Columns.Add("UserName");
				dt.Columns.Add("Password");
				dt.Columns.Add("FirstName");
				dt.Columns.Add("LastName");
				dt.Columns.Add("FatherName");
				dt.Columns.Add("JoiningDate", typeof(DateTime));
				dt.Columns.Add("WhatsApp");
				dt.Columns.Add("CellNo");
				dt.Columns.Add("CNICNumber");
				dt.Columns.Add("Email");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtAllUser.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllUser.Rows[i]["ID"], dtAllUser.Rows[i]["SupplierCustomerId"], dtAllUser.Rows[i]["PartyName"], dtAllUser.Rows[i]["UserName"], dtAllUser.Rows[i]["Password"], dtAllUser.Rows[i]["FirstName"], dtAllUser.Rows[i]["LastName"], dtAllUser.Rows[i]["FatherName"], dtAllUser.Rows[i]["JoiningDate"], dtAllUser.Rows[i]["WhatsApp"], dtAllUser.Rows[i]["CellNo"], dtAllUser.Rows[i]["CNICNumber"], dtAllUser.Rows[i]["Email"], dtAllUser.Rows[i]["IsActive"]);
				}
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
			ProfileId = 0;
			RecId = 0;
			ImageNameOrPath = "";
			((Control)(object)CmbPartyName).Text = string.Empty;
			txtUsername.Clear();
			txtFirstName.Clear();
			txtLastName.Clear();
			txtPassword.Clear();
			txtFatherName.Clear();
			txtWhatsAppNo.Text = string.Empty;
			txtCellNo.Text = string.Empty;
			txtEmail.Text = string.Empty;
			txtCnicNo.Text = string.Empty;
			datJoiningDate.Checked = false;
			ProfileImagePictureBox.Image = null;
			ProfileImagePictureBox.Tag = null;
			ProfileImagePictureBox.Invalidate();
			butnSave.Visible = true;
			butnUpdate.Visible = false;
			chkboxIsActive.Checked = false;
			LocationsBind();
			FillApps();
			bindUser();
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
				DDL.BindDDLNew(dt, CmbPartyName, "Id", "CompanyName", "Party Name", false);
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
			grdBranches.RootTable.Columns["UserStatus"].Visible = false;
			grdBranches.RootTable.Columns["Id"].Visible = false;
			grdBranches.RootTable.Columns["Location"].Width = 235;
			grdBranches.RootTable.Columns["Active"].Width = 55;
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

	private void FillApps()
	{
		try
		{
			List<ApplicationsAllocateToCompany> objList = clsGlobalVariables.GetApplicationsAllocateToCompanyList.Where((ApplicationsAllocateToCompany row) => (row.AppId == 4 || row.AppId == 6) && row.CompanyId == UserAccount.CompanyId).ToList();
			if (objList == null || objList.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AppName", typeof(string));
			foreach (ApplicationsAllocateToCompany obj in objList)
			{
				dt.Rows.Add(obj.AppId, obj.AppName);
			}
			grdApps.DataSource = dt;
			grdApps.RetrieveStructure();
			grdApps.AllowEdit = (InheritableBoolean)2;
			grdApps.RootTable.Columns["Id"].Visible = false;
			grdApps.RootTable.Columns["AppName"].Width = 200;
			grdApps.RootTable.Columns.Add("Select");
			grdApps.RootTable.Columns["Select"].ActAsSelector = true;
			grdApps.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdApps.RootTable.Columns["Select"].Width = 20;
			grdApps.RootTable.Columns["Select"].Position = 0;
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
		if (((UltraGridBase)CmbPartyName).ActiveRow == null)
		{
			MessageBox.Show("Party Name Field is Required");
			CmbPartyName.Focus();
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
		if (txtFatherName.Text.Trim() == string.Empty || txtFatherName.Text.Trim() == "0")
		{
			MessageBox.Show("Father Name Field Required");
			txtFatherName.Focus();
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
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Expected O, but got Unknown
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
				UserAccountAllocation allocation2 = new UserAccountAllocation();
				allocation2.BranchId = UserAccount.BranchesId;
				allocation2.Id = Conversion.ToInt(item.Cells["Id"].Value);
				allocation2.CompanyId = Conversion.ToInt(item.Cells["CompanyId"].Value);
				allocation2.OrganizationId = UserAccount.OrganizationId;
				if (Conversion.ToBool(item.Cells["Active"].Value))
				{
					allocation2.IsActive = 1;
				}
				else
				{
					allocation2.IsActive = 0;
				}
				allocation2.EntryDate = DateTime.Now;
				UA.UserAccountAllocationsList.Add(allocation2);
				count++;
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
			UA.SupplierCustomerId = Conversion.ToInt(CmbPartyName.Value);
			UA.UserName = Conversion.ToString((object)txtUsername.Text);
			UA.FirstName = Conversion.ToString((object)txtFirstName.Text);
			UA.LastName = Conversion.ToString((object)txtLastName.Text);
			UA.Password = Conversion.ToString((object)txtPassword.Text);
			UA.UserTypeId = 3;
			UA.AppId = 3;
			if (chkboxIsActive.Checked)
			{
				UA.IsActive = true;
			}
			else
			{
				UA.IsActive = false;
			}
			UA.UserProfileList = new UserProfile();
			UA.UserProfileList.Id = ProfileId;
			UA.UserProfileList.FatherName = txtFatherName.Text;
			if (!datJoiningDate.Checked)
			{
				datJoiningDate.Focus();
				throw new Exception("Joining Date is Required");
			}
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
			UA.UserProfileList.JoiningDate = datJoiningDate.Value;
			UA.UserProfileList.ProfileImageFileName = ImageNameOrPath;
			if (Conversion.ToString(ProfileImagePictureBox.Tag) != null && Conversion.ToString(ProfileImagePictureBox.Tag) != string.Empty && ImageNameOrPath != Conversion.ToString(ProfileImagePictureBox.Tag))
			{
				UA.UserProfileList.PictureToDeleteInUpdateIfAny = Conversion.ToString(ProfileImagePictureBox.Tag);
			}
			UA.ApplicationsAllocateToUserList = new List<ApplicationsAllocateToUser>();
			if (grdApps.RowCount == 0)
			{
				throw new Exception("Please select An App first");
			}
			Dictionary<int, UserAccountAllocation> companyAllocations = UA.UserAccountAllocationsList.Where((UserAccountAllocation app) => app.CompanyId > 0 && app.IsActive == 1).ToDictionary((UserAccountAllocation app) => app.CompanyId, (UserAccountAllocation app) => app);
			rows = grdApps.GetCheckedRows();
			for (int i = 0; i < rows.Length; i++)
			{
				int appId = Conversion.ToInt(rows[i].Cells["Id"].Value);
				foreach (UserAccountAllocation app2 in companyAllocations.Values)
				{
					ApplicationsAllocateToUser allocation = new ApplicationsAllocateToUser
					{
						ApplicationsAllocateToUserId = 0L,
						CompanyId = app2.CompanyId,
						AppId = appId,
						IsActive = true
					};
					UA.ApplicationsAllocateToUserList.Add(allocation);
				}
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
			MessageBox.Show(ex.Message);
		}
	}

	private void grduserdefine_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
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
			if (ua.UserProfileList != null)
			{
				ProfileId = ua.UserProfileList.Id;
				txtFatherName.Text = ua.UserProfileList.FatherName;
				datJoiningDate.Checked = true;
				datJoiningDate.Value = ua.UserProfileList.JoiningDate;
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
			for (int j = 0; j < ua.UserAccountAllocationsList.Count; j++)
			{
				GridEXRow[] rows = grdBranches.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (ua.UserAccountAllocationsList[j].CompanyId == Conversion.ToInt(r2.Cells["CompanyId"].Value))
					{
						if (r2 != null && (int)r2.RowType == 0)
						{
							r2.Delete();
						}
						grdBranches.UpdateData();
						dtCompanies.Rows.Add(ua.UserAccountAllocationsList[j].Id, ua.UserAccountAllocationsList[j].CompanyId, ua.UserAccountAllocationsList[j].CompName, ua.UserAccountAllocationsList[j].IsActive, ua.UserAccountAllocationsList[j].IsActive);
					}
				}
			}
			grdBranches.DataSource = dtCompanies;
			grdBranches.RetrieveStructure();
			GridSettingLocations();
			for (int i = 0; i < ua.ApplicationsAllocateToUserList.Count; i++)
			{
				GridEXRow[] rows = grdApps.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (ua.ApplicationsAllocateToUserList[i].AppId == Conversion.ToInt(r.Cells["Id"].Value))
					{
						if (r != null && (int)r.RowType == 0)
						{
							r.IsChecked = true;
						}
						grdApps.UpdateData();
					}
				}
			}
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

	private void CustomerRegistration_MobileApp_KeyDown(object sender, KeyEventArgs e)
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

	private void CustomerRegistration_MobileApp_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Expected O, but got Unknown
		//IL_0ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eae: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Configurations.CustomerRegistration_MobileApp));
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
		GridEXLayout grdApps_DesignTimeLayout = new GridEXLayout();
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
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grduserdefine = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdBranches = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnClearImage = new System.Windows.Forms.Button();
		this.browse = new System.Windows.Forms.Button();
		this.ProfileImagePictureBox = new System.Windows.Forms.PictureBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.datJoiningDate = new System.Windows.Forms.DateTimePicker();
		this.txtCellNo = new System.Windows.Forms.MaskedTextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.MaskedTextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtFatherName = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtCnicNo = new System.Windows.Forms.MaskedTextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtWhatsAppNo = new System.Windows.Forms.MaskedTextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbPartyName = new UltraCombo();
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
		this.SubUserRadioButton = new System.Windows.Forms.RadioButton();
		this.MainUserRadioButton = new System.Windows.Forms.RadioButton();
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
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdApps = new GridEX();
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
		((System.ComponentModel.ISupportInitialize)this.ProfileImagePictureBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		this.panel11.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdApps).BeginInit();
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
		this.tabPage3.Controls.Add(this.panel10);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1308, 560);
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
		this.panel10.Size = new System.Drawing.Size(1308, 560);
		this.panel10.TabIndex = 3;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grduserdefine);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 254);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1308, 306);
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
		((System.Windows.Forms.Control)(object)this.grduserdefine).Size = new System.Drawing.Size(1308, 306);
		((System.Windows.Forms.Control)(object)this.grduserdefine).TabIndex = 0;
		this.grduserdefine.TotalRow = (InheritableBoolean)1;
		this.grduserdefine.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grduserdefine).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grduserdefine).DoubleClick += new System.EventHandler(grduserdefine_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add(this.panel1);
		this.panel9.Controls.Add(this.panel17);
		this.panel9.Controls.Add(this.panel16);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 72);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1308, 182);
		this.panel9.TabIndex = 61;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdBranches);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(707, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(299, 182);
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
		((System.Windows.Forms.Control)(object)this.grdBranches).Size = new System.Drawing.Size(299, 182);
		((System.Windows.Forms.Control)(object)this.grdBranches).TabIndex = 0;
		this.grdBranches.TotalRow = (InheritableBoolean)1;
		this.grdBranches.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdBranches).VisualStyle = (VisualStyle)4;
		this.grdBranches.CellValueChanged += new ColumnActionEventHandler(grdBranches_CellUpdated);
		this.panel16.Controls.Add(this.groupBox1);
		this.panel16.Controls.Add(this.SubUserRadioButton);
		this.panel16.Controls.Add(this.MainUserRadioButton);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(707, 182);
		this.panel16.TabIndex = 47;
		this.groupBox1.Controls.Add(this.btnClearImage);
		this.groupBox1.Controls.Add(this.browse);
		this.groupBox1.Controls.Add(this.ProfileImagePictureBox);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.datJoiningDate);
		this.groupBox1.Controls.Add(this.txtCellNo);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtEmail);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtFatherName);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtCnicNo);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtWhatsAppNo);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
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
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(697, 174);
		this.groupBox1.TabIndex = 46;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "User Detail";
		this.btnClearImage.BackColor = System.Drawing.Color.Teal;
		this.btnClearImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClearImage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnClearImage.ForeColor = System.Drawing.Color.White;
		this.btnClearImage.Location = new System.Drawing.Point(606, 147);
		this.btnClearImage.Name = "btnClearImage";
		this.btnClearImage.Size = new System.Drawing.Size(75, 23);
		this.btnClearImage.TabIndex = 13;
		this.btnClearImage.Text = "Clear";
		this.btnClearImage.UseVisualStyleBackColor = false;
		this.btnClearImage.Click += new System.EventHandler(btnClearImage_Click);
		this.browse.BackColor = System.Drawing.Color.Teal;
		this.browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.browse.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.browse.ForeColor = System.Drawing.Color.White;
		this.browse.Location = new System.Drawing.Point(530, 147);
		this.browse.Name = "browse";
		this.browse.Size = new System.Drawing.Size(75, 23);
		this.browse.TabIndex = 12;
		this.browse.Text = "Browse";
		this.browse.UseVisualStyleBackColor = false;
		this.browse.Click += new System.EventHandler(browse_Click);
		this.ProfileImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ProfileImagePictureBox.Location = new System.Drawing.Point(512, 20);
		this.ProfileImagePictureBox.Name = "ProfileImagePictureBox";
		this.ProfileImagePictureBox.Size = new System.Drawing.Size(179, 125);
		this.ProfileImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.ProfileImagePictureBox.TabIndex = 217;
		this.ProfileImagePictureBox.TabStop = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(278, 25);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(74, 15);
		this.label22.TabIndex = 129;
		this.label22.Text = "Joining Date";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(73, 50);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 17);
		this.label24.TabIndex = 128;
		this.label24.Text = "*";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(352, 50);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(14, 17);
		this.label23.TabIndex = 127;
		this.label23.Text = "*";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Red;
		this.label21.Location = new System.Drawing.Point(353, 24);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(14, 17);
		this.label21.TabIndex = 125;
		this.label21.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(352, 75);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 123;
		this.label19.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(352, 125);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 122;
		this.label18.Text = "*";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(352, 100);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 17);
		this.label17.TabIndex = 121;
		this.label17.Text = "*";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(73, 150);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(14, 17);
		this.label16.TabIndex = 120;
		this.label16.Text = "*";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Red;
		this.label14.Location = new System.Drawing.Point(73, 75);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(14, 17);
		this.label14.TabIndex = 119;
		this.label14.Text = "*";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(73, 100);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 17);
		this.label13.TabIndex = 118;
		this.label13.Text = "*";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Red;
		this.label12.Location = new System.Drawing.Point(73, 125);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(14, 17);
		this.label12.TabIndex = 117;
		this.label12.Text = "*";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Red;
		this.label11.Location = new System.Drawing.Point(73, 25);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(14, 17);
		this.label11.TabIndex = 116;
		this.label11.Text = "*";
		this.datJoiningDate.CustomFormat = "dd-MMM-yy";
		this.datJoiningDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datJoiningDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datJoiningDate.Location = new System.Drawing.Point(370, 21);
		this.datJoiningDate.Name = "datJoiningDate";
		this.datJoiningDate.Size = new System.Drawing.Size(139, 23);
		this.datJoiningDate.TabIndex = 6;
		this.txtCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCellNo.Location = new System.Drawing.Point(370, 72);
		this.txtCellNo.Mask = "####-###-####";
		this.txtCellNo.Name = "txtCellNo";
		this.txtCellNo.Size = new System.Drawing.Size(139, 23);
		this.txtCellNo.TabIndex = 8;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(278, 76);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(45, 15);
		this.label10.TabIndex = 23;
		this.label10.Text = "Cell No";
		this.txtEmail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmail.Location = new System.Drawing.Point(370, 122);
		this.txtEmail.Mask = "LLLLLLLL@LLLLLLLL.LLL";
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(139, 23);
		this.txtEmail.TabIndex = 10;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(278, 126);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(39, 15);
		this.label9.TabIndex = 21;
		this.label9.Text = "Email ";
		this.txtFatherName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFatherName.Location = new System.Drawing.Point(88, 147);
		this.txtFatherName.Name = "txtFatherName";
		this.txtFatherName.Size = new System.Drawing.Size(187, 23);
		this.txtFatherName.TabIndex = 5;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(2, 151);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(75, 15);
		this.label8.TabIndex = 19;
		this.label8.Text = "Father Name";
		this.txtCnicNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCnicNo.Location = new System.Drawing.Point(370, 97);
		this.txtCnicNo.Mask = "00000-0000000-0";
		this.txtCnicNo.Name = "txtCnicNo";
		this.txtCnicNo.Size = new System.Drawing.Size(139, 23);
		this.txtCnicNo.TabIndex = 9;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(278, 101);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(56, 15);
		this.label7.TabIndex = 16;
		this.label7.Text = "CNIC No.";
		this.txtWhatsAppNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWhatsAppNo.Location = new System.Drawing.Point(370, 47);
		this.txtWhatsAppNo.Mask = "####-###-####";
		this.txtWhatsAppNo.Name = "txtWhatsAppNo";
		this.txtWhatsAppNo.Size = new System.Drawing.Size(139, 23);
		this.txtWhatsAppNo.TabIndex = 7;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(278, 51);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(70, 15);
		this.label6.TabIndex = 14;
		this.label6.Text = "WhatsApp#";
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
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(88, 20);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(187, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(2, 26);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 15);
		this.label5.TabIndex = 12;
		this.label5.Text = "Party Name";
		this.chkboxIsActive.AutoSize = true;
		this.chkboxIsActive.Checked = true;
		this.chkboxIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkboxIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkboxIsActive.Location = new System.Drawing.Point(437, 148);
		this.chkboxIsActive.Name = "chkboxIsActive";
		this.chkboxIsActive.Size = new System.Drawing.Size(68, 19);
		this.chkboxIsActive.TabIndex = 11;
		this.chkboxIsActive.Text = "IsActive";
		this.chkboxIsActive.UseVisualStyleBackColor = true;
		this.txtLastName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLastName.Location = new System.Drawing.Point(88, 122);
		this.txtLastName.Name = "txtLastName";
		this.txtLastName.Size = new System.Drawing.Size(187, 23);
		this.txtLastName.TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(2, 126);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 6;
		this.label4.Text = "Last Name";
		this.txtFirstName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFirstName.Location = new System.Drawing.Point(88, 97);
		this.txtFirstName.Name = "txtFirstName";
		this.txtFirstName.Size = new System.Drawing.Size(187, 23);
		this.txtFirstName.TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(2, 101);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 4;
		this.label3.Text = "First Name";
		this.txtPassword.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPassword.Location = new System.Drawing.Point(88, 72);
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '*';
		this.txtPassword.Size = new System.Drawing.Size(187, 23);
		this.txtPassword.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(2, 76);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 2;
		this.label2.Text = "Password";
		this.txtUsername.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUsername.Location = new System.Drawing.Point(88, 47);
		this.txtUsername.Name = "txtUsername";
		this.txtUsername.Size = new System.Drawing.Size(187, 23);
		this.txtUsername.TabIndex = 1;
		this.txtUsername.Leave += new System.EventHandler(txtUsername_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(2, 51);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(65, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "User Name";
		this.SubUserRadioButton.AutoSize = true;
		this.SubUserRadioButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SubUserRadioButton.Location = new System.Drawing.Point(297, 206);
		this.SubUserRadioButton.Name = "SubUserRadioButton";
		this.SubUserRadioButton.Size = new System.Drawing.Size(72, 19);
		this.SubUserRadioButton.TabIndex = 1;
		this.SubUserRadioButton.Text = "Sub User";
		this.SubUserRadioButton.UseVisualStyleBackColor = true;
		this.SubUserRadioButton.Visible = false;
		this.SubUserRadioButton.CheckedChanged += new System.EventHandler(SubUserRadioButton_CheckedChanged);
		this.MainUserRadioButton.AutoSize = true;
		this.MainUserRadioButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.MainUserRadioButton.Location = new System.Drawing.Point(297, 187);
		this.MainUserRadioButton.Name = "MainUserRadioButton";
		this.MainUserRadioButton.Size = new System.Drawing.Size(78, 19);
		this.MainUserRadioButton.TabIndex = 0;
		this.MainUserRadioButton.Text = "Main User";
		this.MainUserRadioButton.UseVisualStyleBackColor = true;
		this.MainUserRadioButton.Visible = false;
		this.MainUserRadioButton.CheckedChanged += new System.EventHandler(MainUserRadioButton_CheckedChanged);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label15);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 32);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1308, 40);
		this.panel11.TabIndex = 60;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(5, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(297, 30);
		this.label15.TabIndex = 0;
		this.label15.Text = "Customer Portal Registration";
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1308, 32);
		this.panel7.TabIndex = 0;
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1308, 31);
		this.panel12.TabIndex = 2;
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.ctrlGrdBar3);
		this.panel13.Controls.Add(this.toolStrip3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Margin = new System.Windows.Forms.Padding(2);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1308, 30);
		this.panel13.TabIndex = 1;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1267, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grduserdefine;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew1, this.butnUpdate, this.butnSave });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1308, 30);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew1.Name = "btnNew1";
		this.btnNew1.Size = new System.Drawing.Size(59, 27);
		this.btnNew1.Text = "&New";
		this.btnNew1.Click += new System.EventHandler(btnnew_Click);
		this.butnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.butnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.butnUpdate.Name = "butnUpdate";
		this.butnUpdate.Size = new System.Drawing.Size(78, 27);
		this.butnUpdate.Text = "&Update";
		this.butnUpdate.Visible = false;
		this.butnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.butnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.butnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.butnSave.Name = "butnSave";
		this.butnSave.Size = new System.Drawing.Size(64, 27);
		this.butnSave.Text = "&Save";
		this.butnSave.Click += new System.EventHandler(btnsave_Click);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1316, 586);
		this.tabControl1.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1314, 6);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 19;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdApps);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(1006, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(299, 182);
		this.panel1.TabIndex = 49;
		((ScrollableBase)this.grdApps).BackColor = System.Drawing.Color.White;
		grdApps_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdApps.DesignTimeLayout = grdApps_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdApps).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdApps).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdApps.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdApps).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdApps).Name = "grdApps";
		((ScrollableBase)this.grdApps).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdApps).Size = new System.Drawing.Size(299, 182);
		((System.Windows.Forms.Control)(object)this.grdApps).TabIndex = 0;
		this.grdApps.TotalRow = (InheritableBoolean)1;
		this.grdApps.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdApps).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1316, 586);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "CustomerRegistration_MobileApp";
		this.Text = "  EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(CustomerRegistration_MobileApp_FormClosed);
		base.Load += new System.EventHandler(frmUserRights_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CustomerRegistration_MobileApp_KeyDown);
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
		this.panel16.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ProfileImagePictureBox).EndInit();
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
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdApps).EndInit();
		base.ResumeLayout(false);
	}
}

}
