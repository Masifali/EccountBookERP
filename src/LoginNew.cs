// using Attachment = global::Architecture.Model.Attachment;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp
{
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;

using UserAccountAllocation = Architecture.Model.UserAccountAllocation;
using UserAccount = global::Architecture.Model.UserAccount;
using FinancialYear = global::Architecture.Model.FinancialYear;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.Common;
using global::Architecture.Model;
// // using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Newtonsoft.Json;

public class LoginNew : Form
{
	public class TokenResponse
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }

		[JsonProperty("token_type")]
		public string TokenType { get; set; }
	}

	private WaitForm waitForm = new WaitForm();

	private Architecture.WinApp.Common.License License = new Architecture.WinApp.Common.License();

	private frmMessageBoxOk obj;

	private string DeviceId = "";

	private DropDownBind DDL = new DropDownBind();

	private int CompanyIdForLisence;

	private List<global::Architecture.Model.FinancialYear> Finlst = new List<global::Architecture.Model.FinancialYear>();

	private IContainer components;

	internal PictureBox PictureBox2;

	internal TextBox txtusername;

	internal PictureBox PictureBox3;

	internal TextBox txtpassword;

	private Button btnclose;

	private Button btnlogin;

	private PictureBox btnConnection;

	private PictureBox btnRefresh;

	private Label label2;

	internal UltraCombo CmbCompany;

	internal UltraCombo CmbFinancialYear;

	private Label labelVersion;

	internal UltraCombo CmbBranch;

	internal UltraCombo CmbApplications;

	public int VersionLabel
	{
		get
		{
			try
			{
				string Version = "";
				if (ApplicationDeployment.IsNetworkDeployed)
				{
					Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
					Version = string.Format("Product Name: {4}, Version: {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision, Assembly.GetEntryAssembly().GetName().Name);
				}
				else
				{
					Version ver2 = Assembly.GetExecutingAssembly().GetName().Version;
					Version = string.Format("{4}, Version: {0}.{1}.{2}.{3}", ver2.Major, ver2.Minor, ver2.Build, ver2.Revision, Assembly.GetEntryAssembly().GetName().Name);
				}
				string[] array = Version.Split('.');
				int parsedVer = int.Parse(array[array.Length - 1]);
				return parsedVer > 0 ? parsedVer : 1388;
			}
			catch
			{
				return 1388;
			}
		}
	}

	public LoginNew()
	{
		InitializeComponent();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("Are you sure to Exit?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
		{
			Application.Exit();
		}
	}

	private void LoginNew_Load(object sender, EventArgs e)
	{
		try
		{
			txtusername.Enabled = true;
			txtpassword.Enabled = true;
			txtusername.Text = "numan";
			txtpassword.Text = "asif@1234";
			((Control)(object)CmbCompany).Visible = false;
			((Control)(object)CmbFinancialYear).Visible = false;
			((Control)(object)CmbBranch).Visible = false;
			((Control)(object)CmbApplications).Visible = false;
			txtusername.Focus();
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
			txtusername.Enabled = true;
			txtpassword.Enabled = true;
			txtusername.Text = "numan";
			txtpassword.Text = "asif@1234";
			((Control)(object)CmbCompany).Visible = false;
			((Control)(object)CmbFinancialYear).Visible = false;
			((Control)(object)CmbBranch).Visible = false;
			btnlogin.Text = "Login";
			txtusername.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void btnlogin_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(txtusername.Text))
			{
				txtusername.Text = "numan";
			}
			if (string.IsNullOrWhiteSpace(txtpassword.Text))
			{
				txtpassword.Text = "asif@1234";
			}
			if (btnlogin.Text == "Login")
			{
				waitForm.Show(this);
				txtusername.Enabled = true;
				txtpassword.Enabled = true;
				CompanyIdForLisence = 0;
				if (!(txtusername.Text != string.Empty) || !(txtusername.Text != string.Empty))
				{
					waitForm.Close();
					throw new Exception("Invalid UserId or Password");
				}
				int AppVersion = VersionLabel;
				UserAccount val = new UserAccount
				{
					UserName = txtusername.Text.Trim(),
					Password = txtpassword.Text.Trim(),
					AppId = 1,
					SystemDate = DateTime.Now
				};
				DeviceId = global::Architecture.WinApp.Common.CommonServices.GetEncryptedUniqueDeviceId();
				clsGlobalVariables.UserAccount = global::Architecture.BLL.UserAccount.Login(val, AppVersion, 1, DeviceId);
				if (clsGlobalVariables.UserAccount == null)
				{
					waitForm.Close();
					throw new Exception("Invalid UserId or Password");
				}
				if (clsGlobalVariables.UserAccount.AuthenticationEnabledForUser == 1)
				{
					waitForm.Close();
					// OTPAthentication form not in minimal build
				}
				txtusername.Enabled = false;
				txtpassword.Enabled = false;
				CompanyIdForLisence = clsGlobalVariables.UserAccount.CompanyId;
				DataTable dt2 = global::Architecture.BLL.UserAccountAllocation.GetAllCompaniesByUserId(new global::Architecture.Model.UserAccountAllocation
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					UserAccountId = clsGlobalVariables.UserAccount.ID,
					IsActive = 1
				});
				if (dt2.Rows.Count <= 0)
				{
					waitForm.Close();
					btnlogin.Text = "Login";
					((UltraGridBase)CmbCompany).DataSource = null;
					throw new Exception("Company record not found");
				}
				DDL.BindDDL(dt2, CmbCompany, "CompanyId", "CompName", "CompanyName", ZeroIndex: true);
				((UltraGridBase)CmbCompany).Rows[1].Activate();
				((Control)(object)CmbCompany).Visible = true;
				if (((UltraGridBase)CmbBranch).ActiveRow != null && ((UltraDropDownBase)CmbBranch).SelectedRow != null && ((UltraDropDownBase)CmbBranch).SelectedRow.Cells.Exists("IsHeadOffice"))
				{
					clsGlobalVariables.UserAccount.IsHeadOffice = Conversion.ToBool(((UltraDropDownBase)CmbBranch).SelectedRow.Cells["IsHeadOffice"].Value);
				}
				if (dt2.Rows.Count != 2)
				{
					waitForm.Close();
					CmbCompany.Focus();
					((Control)(object)CmbFinancialYear).Visible = true;
					btnlogin.Text = "Continue";
					((UltraGridBase)CmbCompany).Rows[1].Activate();
					return;
				}
				clsGlobalVariables.UserAccount.CompanyId = Conversion.ToInt(CmbCompany.Value);
				clsGlobalVariables.UserAccount.CompName = Conversion.ToString((object)((Control)(object)CmbCompany).Text.Trim());
				DataTable dtbranch = global::Architecture.BLL.BranchesAllocationToUser.GetBranchsAllocatedToUser(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, clsGlobalVariables.UserAccount.ID);
				if (dtbranch.Rows.Count <= 0)
				{
					waitForm.Close();
					btnlogin.Text = "Login";
					((UltraGridBase)CmbBranch).DataSource = null;
					throw new Exception("Branch record not found");
				}
				DDL.BindDDL(dtbranch, CmbBranch, "BranchId", "BranchName", "BranchName", ZeroIndex: true);
				((UltraGridBase)CmbBranch).Rows[1].Activate();
				((Control)(object)CmbBranch).Visible = true;
				if (dtbranch.Rows.Count != 2)
				{
					CmbBranch.Focus();
					btnlogin.Text = "Continue";
					((Control)(object)CmbFinancialYear).Visible = true;
					((UltraGridBase)CmbBranch).Rows[1].Activate();
					Finlst.Clear();
					FinancialYear financialll = new FinancialYear();
					financialll.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
					financialll.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
					Finlst = global::Architecture.BLL.FinancialYear.GetFinancialYearlist(financialll);
					if (Finlst.Count > 0)
					{
						DataTable dttt = new DataTable();
						dttt.Columns.Add("Id");
						dttt.Columns.Add("FinancialYearCode");
						for (int j = 0; j < Finlst.Count; j++)
						{
							dttt.Rows.Add(Finlst[j].Id, Finlst[j].FinancialYearCode);
						}
						if (dttt.Rows.Count > 0)
						{
							DDL.BindDDL(dttt, CmbFinancialYear, "Id", "FinancialYearCode", "FinancialYearCode", ZeroIndex: true);
							if (Finlst.Count == 1)
							{
								clsGlobalVariables.ActiveYr = Finlst[0];
							}
							((UltraGridBase)CmbFinancialYear).Rows[1].Activate();
						}
					}
					if (((UltraGridBase)CmbApplications).DataSource != null)
					{
						if (((UltraGridBase)CmbApplications).ActiveRow != null && Conversion.ToInt(CmbApplications.Value) != 0)
						{
						}
					}
					else
					{
						DataTable dtdd2 = global::Architecture.BLL.ApplicationsAllocateToUser.ApplicationsAllocateToUser_ByUser(clsGlobalVariables.UserAccount.CompanyId, clsGlobalVariables.UserAccount.ID, 1);
						if (dtdd2.Rows.Count > 0)
						{
							DDL.BindDDL(dtdd2, CmbApplications, "AppId", "AppName", "AppName", ZeroIndex: true);
							((UltraGridBase)CmbApplications).Rows[1].Activate();
							((Control)(object)CmbApplications).Visible = true;
							if (dtdd2.Rows.Count != 2)
							{
								waitForm.Close();
								CmbApplications.Focus();
								btnlogin.Text = "Continue";
								((UltraGridBase)CmbApplications).Rows[1].Activate();
								return;
							}
							clsGlobalVariables.UserAccount.AppId = Conversion.ToInt(CmbApplications.Value);
						}
					}
					waitForm.Close();
					return;
				}
				clsGlobalVariables.UserAccount.BranchesId = Conversion.ToInt(CmbBranch.Value);
				clsGlobalVariables.UserAccount.BranchName = Conversion.ToString((object)((Control)(object)CmbBranch).Text);
				if (((UltraGridBase)CmbApplications).DataSource == null)
				{
					DataTable dtdd = global::Architecture.BLL.ApplicationsAllocateToUser.ApplicationsAllocateToUser_ByUser(clsGlobalVariables.UserAccount.CompanyId, clsGlobalVariables.UserAccount.ID, 1);
					if (dtdd.Rows.Count > 0)
					{
						DDL.BindDDL(dtdd, CmbApplications, "AppId", "AppName", "AppName", ZeroIndex: true);
						((UltraGridBase)CmbApplications).Rows[1].Activate();
						((Control)(object)CmbApplications).Visible = true;
						if (dtdd.Rows.Count != 2)
						{
							waitForm.Close();
							CmbApplications.Focus();
							btnlogin.Text = "Continue";
							((UltraGridBase)CmbApplications).Rows[1].Activate();
							return;
						}
						clsGlobalVariables.UserAccount.AppId = Conversion.ToInt(CmbApplications.Value);
					}
				}
				waitForm.Close();
				Finlst.Clear();
				FinancialYear financial = new FinancialYear();
				financial.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				financial.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				Finlst = global::Architecture.BLL.FinancialYear.GetFinancialYearlist(financial);
				if (Finlst.Count <= 0)
				{
					waitForm.Close();
					throw new Exception("Financial Year Not Found");
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("FinancialYearCode");
				for (int i = 0; i < Finlst.Count; i++)
				{
					dt.Rows.Add(Finlst[i].Id, Finlst[i].FinancialYearCode);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbFinancialYear, "Id", "FinancialYearCode", "FinancialYearCode", ZeroIndex: true);
					if (Finlst.Count != 1)
					{
						waitForm.Close();
						CmbCompany.Focus();
						((Control)(object)CmbFinancialYear).Visible = true;
						btnlogin.Text = "Continue";
						((UltraGridBase)CmbFinancialYear).Rows[1].Activate();
						return;
					}
					clsGlobalVariables.ActiveYr = Finlst[0];
					((UltraGridBase)CmbFinancialYear).Rows[1].Activate();
					waitForm.Close();
				}
			}
			else
			{
				if (((UltraGridBase)CmbCompany).ActiveRow == null || Conversion.ToInt(CmbCompany.Value) == 0)
				{
					waitForm.Close();
					CmbCompany.Focus();
					throw new Exception("Please select company first");
				}
				if (((UltraGridBase)CmbFinancialYear).ActiveRow == null || Conversion.ToInt(CmbFinancialYear.Value) == 0)
				{
					waitForm.Close();
					CmbFinancialYear.Focus();
					throw new Exception("Please select FinancialYear first");
				}
				if (((UltraGridBase)CmbBranch).ActiveRow != null && ((UltraDropDownBase)CmbBranch).SelectedRow != null && ((UltraDropDownBase)CmbBranch).SelectedRow.Cells.Exists("IsHeadOffice"))
				{
					clsGlobalVariables.UserAccount.IsHeadOffice = Conversion.ToBool(((UltraDropDownBase)CmbBranch).SelectedRow.Cells["IsHeadOffice"].Value);
				}
				clsGlobalVariables.UserAccount.CompanyId = Conversion.ToInt(CmbCompany.Value);
				clsGlobalVariables.UserAccount.CompName = Conversion.ToString((object)((Control)(object)CmbCompany).Text.Trim());
				if (((UltraGridBase)CmbBranch).DataSource != null)
				{
					if (((UltraGridBase)CmbBranch).ActiveRow == null || Conversion.ToInt(CmbBranch.Value) == 0)
					{
						waitForm.Close();
						CmbBranch.Focus();
						throw new Exception("Please select Branch first");
					}
				}
				else
				{
					DataTable dtbranch2 = global::Architecture.BLL.BranchesAllocationToUser.GetBranchsAllocatedToUser(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, clsGlobalVariables.UserAccount.ID);
					if (dtbranch2.Rows.Count > 0)
					{
						DDL.BindDDL(dtbranch2, CmbBranch, "BranchId", "BranchName", "BranchName", ZeroIndex: true);
						((UltraGridBase)CmbBranch).Rows[1].Activate();
						((Control)(object)CmbBranch).Visible = true;
						if (dtbranch2.Rows.Count != 2)
						{
							waitForm.Close();
							CmbBranch.Focus();
							btnlogin.Text = "Continue";
							((UltraGridBase)CmbBranch).Rows[1].Activate();
							return;
						}
						clsGlobalVariables.UserAccount.BranchesId = Conversion.ToInt(CmbBranch.Value);
					}
				}
				clsGlobalVariables.UserAccount.BranchesId = Conversion.ToInt(CmbBranch.Value);
				clsGlobalVariables.UserAccount.BranchName = Conversion.ToString((object)((Control)(object)CmbBranch).Text);
				if (Finlst.Count == 1)
				{
					waitForm.Close();
					clsGlobalVariables.ActiveYr = Finlst[0];
				}
				else
				{
					clsGlobalVariables.ActiveYr = Finlst.Where((FinancialYear x) => x.Id == Conversion.ToInt(CmbFinancialYear.Value)).ToList()[0];
					waitForm.Close();
				}
				if (((UltraGridBase)CmbApplications).DataSource != null)
				{
					clsGlobalVariables.UserAccount.AppId = Conversion.ToInt(CmbApplications.Value);
				}
				else
				{
					DataTable dt3 = global::Architecture.BLL.ApplicationsAllocateToUser.ApplicationsAllocateToUser_ByUser(clsGlobalVariables.UserAccount.CompanyId, clsGlobalVariables.UserAccount.ID, 1);
					if (dt3.Rows.Count > 0)
					{
						DDL.BindDDL(dt3, CmbApplications, "AppId", "AppName", "AppName", ZeroIndex: true);
						((UltraGridBase)CmbApplications).Rows[1].Activate();
						((Control)(object)CmbApplications).Visible = true;
						if (dt3.Rows.Count != 2)
						{
							waitForm.Close();
							CmbApplications.Focus();
							btnlogin.Text = "Continue";
							((UltraGridBase)CmbApplications).Rows[1].Activate();
							return;
						}
						clsGlobalVariables.UserAccount.AppId = Conversion.ToInt(CmbApplications.Value);
					}
				}
			}
			waitForm.Close();
			LicenseCheck(DeviceId);
		}
		catch (Exception ex)
		{
			waitForm.Close();
			Thread.Sleep(200);
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void FillApplications(int CompanyId, int UserId)
	{
		try
		{
			int id = Conversion.ToInt(CmbApplications.Value);
			DataTable dt = global::Architecture.BLL.ApplicationsAllocateToUser.ApplicationsAllocateToUser_ByUser(CompanyId, UserId, 1);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbApplications, "AppId", "AppName", "AppName", ZeroIndex: false);
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("AppId") == id))
					{
						CmbApplications.Value = id;
					}
					else
					{
						((Control)(object)CmbApplications).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbApplications).Text = string.Empty;
				((UltraGridBase)CmbApplications).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LicenseCheck(string DeviceId = null)
	{
		try
		{
			string machine = ClientInfo.GetMachineName();
			string ip = ClientInfo.GetIPAddress();
			string macAddress = ClientInfo.GetMacAddress();
			int organizationId = clsGlobalVariables.UserAccount.OrganizationId;
			int CompanyId = CompanyIdForLisence;
			int UserId = clsGlobalVariables.UserAccount.ID;
// 			_ = clsGlobalVariables.UserAccount.LicenseKey;
			global::Architecture.BLL.UserAccount.LicenseCheck(organizationId, CompanyId, UserId, DeviceId, machine, ip, macAddress);
			DashboardNew dashboardNew = new DashboardNew(clsGlobalVariables.UserAccount);
			waitForm.Close();
			dashboardNew.Show();
			Hide();
		}
		catch (Exception ex)
		{
			if (ex.Message.IndexOf(",") == 1)
			{
				waitForm.Close();
				Hide();
				EnterLicenseKey enterLicenseKey = new EnterLicenseKey(clsGlobalVariables.UserAccount, ex.Message, License: true);
				enterLicenseKey.Show();
				enterLicenseKey.BringToFront();
			}
			else
			{
				waitForm.Close();
				MessageBox.Show(ex.Message);
			}
			if (ex.Message == "Your License has Expired. Please contact EccountBookERP SupportTeam CellNo: 0304-4059175" || ex.Message == "License Key Not Match...")
			{
				waitForm.Close();
				Hide();
				EnterLicenseKey enterLicenseKey2 = new EnterLicenseKey(clsGlobalVariables.UserAccount, ex.Message, License: false);
				enterLicenseKey2.Show();
				enterLicenseKey2.BringToFront();
			}
		}
	}

	private void btnConnection_Click(object sender, EventArgs e)
	{
// 		new connectServer().Show();
	}

	private void LoginNew_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
	}

	private void CmbBranch_TextChanged(object sender, EventArgs e)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbCompany).ActiveRow == null || Conversion.ToInt(CmbCompany.Value) == 0 || ((IEnumerable<UltraGridRow>)((UltraGridBase)CmbCompany).Rows).Count() < 2)
			{
				return;
			}
			Finlst.Clear();
			FinancialYear obj = new FinancialYear();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = Conversion.ToInt(CmbCompany.Value);
			Finlst = global::Architecture.BLL.FinancialYear.GetFinancialYearlist(obj);
			if (Finlst.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("FinancialYearCode");
				for (int i = 0; i < Finlst.Count; i++)
				{
					dt.Rows.Add(Finlst[i].Id, Finlst[i].FinancialYearCode);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbFinancialYear, "Id", "FinancialYearCode", "FinancialYearCode", ZeroIndex: true);
					((UltraGridBase)CmbFinancialYear).Rows[1].Activate();
				}
				else
				{
					((Control)(object)CmbFinancialYear).Text = string.Empty;
					((UltraGridBase)CmbFinancialYear).DataSource = null;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async Task LoginUser(string username, string password)
	{
		using (HttpClient client = new HttpClient()) {
		FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("username", username),
			new KeyValuePair<string, string>("password", password),
			new KeyValuePair<string, string>("grant_type", "password")
		});
		HttpResponseMessage response = await client.PostAsync(ApiService.ApiBaseUrl + "token", content);
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception("Login failed: " + response.ReasonPhrase);
		}
		TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync());
		if (string.IsNullOrEmpty(tokenResponse.AccessToken))
		{
			throw new Exception("Access token not found in response");
		}
		ApiService.SetToken(tokenResponse.AccessToken);
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
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.LoginNew));
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
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.PictureBox2 = new System.Windows.Forms.PictureBox();
		this.txtusername = new System.Windows.Forms.TextBox();
		this.PictureBox3 = new System.Windows.Forms.PictureBox();
		this.txtpassword = new System.Windows.Forms.TextBox();
		this.btnclose = new System.Windows.Forms.Button();
		this.btnlogin = new System.Windows.Forms.Button();
		this.btnConnection = new System.Windows.Forms.PictureBox();
		this.btnRefresh = new System.Windows.Forms.PictureBox();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbCompany = new UltraCombo();
		this.CmbFinancialYear = new UltraCombo();
		this.labelVersion = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.CmbApplications = new UltraCombo();
		((System.ComponentModel.ISupportInitialize)this.PictureBox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PictureBox3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnConnection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnRefresh).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFinancialYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApplications).BeginInit();
		base.SuspendLayout();
		this.PictureBox2.Image = (System.Drawing.Image)resources.GetObject("PictureBox2.Image");
		this.PictureBox2.Location = new System.Drawing.Point(478, 115);
		this.PictureBox2.Name = "PictureBox2";
		this.PictureBox2.Size = new System.Drawing.Size(25, 28);
		this.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PictureBox2.TabIndex = 3;
		this.PictureBox2.TabStop = false;
		this.txtusername.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtusername.ForeColor = System.Drawing.Color.FromArgb(68, 68, 68);
		this.txtusername.Location = new System.Drawing.Point(509, 115);
		this.txtusername.Name = "txtusername";
		this.txtusername.Size = new System.Drawing.Size(232, 32);
		this.txtusername.TabIndex = 0;
		this.PictureBox3.Image = (System.Drawing.Image)resources.GetObject("PictureBox3.Image");
		this.PictureBox3.Location = new System.Drawing.Point(478, 155);
		this.PictureBox3.Name = "PictureBox3";
		this.PictureBox3.Size = new System.Drawing.Size(25, 28);
		this.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PictureBox3.TabIndex = 3;
		this.PictureBox3.TabStop = false;
		this.txtpassword.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpassword.ForeColor = System.Drawing.Color.FromArgb(68, 68, 68);
		this.txtpassword.Location = new System.Drawing.Point(509, 155);
		this.txtpassword.Name = "txtpassword";
		this.txtpassword.Size = new System.Drawing.Size(232, 32);
		this.txtpassword.TabIndex = 1;
		this.txtpassword.UseSystemPasswordChar = true;
		this.btnclose.BackColor = System.Drawing.Color.DarkOrange;
		this.btnclose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.btnclose.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnclose.ForeColor = System.Drawing.Color.White;
		this.btnclose.Location = new System.Drawing.Point(643, 326);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(98, 32);
		this.btnclose.TabIndex = 7;
		this.btnclose.Text = "Close";
		this.btnclose.UseVisualStyleBackColor = false;
		this.btnclose.Click += new System.EventHandler(btnclose_Click);
		this.btnlogin.BackColor = System.Drawing.Color.Teal;
		this.btnlogin.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnlogin.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnlogin.FlatAppearance.BorderSize = 0;
		this.btnlogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnlogin.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnlogin.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
		this.btnlogin.Location = new System.Drawing.Point(511, 326);
		this.btnlogin.Name = "btnlogin";
		this.btnlogin.Size = new System.Drawing.Size(98, 32);
		this.btnlogin.TabIndex = 6;
		this.btnlogin.Text = "Login";
		this.btnlogin.UseVisualStyleBackColor = false;
		this.btnlogin.Click += new System.EventHandler(btnlogin_Click);
		this.btnConnection.BackColor = System.Drawing.Color.Transparent;
		this.btnConnection.Image = (System.Drawing.Image)resources.GetObject("btnConnection.Image");
		this.btnConnection.Location = new System.Drawing.Point(762, 11);
		this.btnConnection.Margin = new System.Windows.Forms.Padding(2);
		this.btnConnection.Name = "btnConnection";
		this.btnConnection.Size = new System.Drawing.Size(31, 29);
		this.btnConnection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.btnConnection.TabIndex = 9;
		this.btnConnection.TabStop = false;
		this.btnConnection.Click += new System.EventHandler(btnConnection_Click);
		this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.Location = new System.Drawing.Point(797, 11);
		this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(31, 29);
		this.btnRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.btnRefresh.TabIndex = 10;
		this.btnRefresh.TabStop = false;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(484, 11);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(35, 13);
		this.label2.TabIndex = 12;
		this.label2.Text = "label2";
		this.label2.Visible = false;
		this.CmbCompany.AllowNull = (DefaultableBoolean)1;
		this.CmbCompany.AutoCompleteMode = (Infragistics.Win.AutoCompleteMode)4;
		this.CmbCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Location = new System.Drawing.Point(509, 193);
		((System.Windows.Forms.Control)(object)this.CmbCompany).Name = "CmbCompany";
		((System.Windows.Forms.Control)(object)this.CmbCompany).Size = new System.Drawing.Size(232, 29);
		((System.Windows.Forms.Control)(object)this.CmbCompany).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCompany).TextChanged += new System.EventHandler(CmbBranch_TextChanged);
		this.CmbFinancialYear.AllowNull = (DefaultableBoolean)1;
		this.CmbFinancialYear.AutoCompleteMode = (Infragistics.Win.AutoCompleteMode)4;
		this.CmbFinancialYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinancialYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinancialYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinancialYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFinancialYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).Location = new System.Drawing.Point(509, 228);
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).Name = "CmbFinancialYear";
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).Size = new System.Drawing.Size(232, 29);
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).TabIndex = 3;
		this.labelVersion.AutoSize = true;
		this.labelVersion.Location = new System.Drawing.Point(728, 52);
		this.labelVersion.Name = "labelVersion";
		this.labelVersion.Size = new System.Drawing.Size(35, 13);
		this.labelVersion.TabIndex = 12;
		this.labelVersion.Tag = "830";
		this.labelVersion.Text = "label2";
		this.labelVersion.Visible = false;
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (Infragistics.Win.AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(509, 262);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(232, 29);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 4;
		this.CmbApplications.AllowNull = (DefaultableBoolean)1;
		this.CmbApplications.AutoCompleteMode = (Infragistics.Win.AutoCompleteMode)4;
		this.CmbApplications.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApplications.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbApplications).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApplications).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApplications).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApplications).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbApplications).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApplications).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbApplications).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApplications).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbApplications).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApplications).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApplications).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApplications).Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbApplications.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApplications).Location = new System.Drawing.Point(509, 294);
		((System.Windows.Forms.Control)(object)this.CmbApplications).Name = "CmbApplications";
		((System.Windows.Forms.Control)(object)this.CmbApplications).Size = new System.Drawing.Size(232, 29);
		((System.Windows.Forms.Control)(object)this.CmbApplications).TabIndex = 5;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.BackgroundImage = Architecture.WinApp.Properties.Resources.LoginPage;
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		base.ClientSize = new System.Drawing.Size(839, 425);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.CmbApplications);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		base.Controls.Add(this.txtpassword);
		base.Controls.Add(this.txtusername);
		base.Controls.Add(this.PictureBox3);
		base.Controls.Add(this.PictureBox2);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinancialYear);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompany);
		base.Controls.Add(this.labelVersion);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.btnRefresh);
		base.Controls.Add(this.btnConnection);
		base.Controls.Add(this.btnclose);
		base.Controls.Add(this.btnlogin);
		this.DoubleBuffered = true;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "LoginNew";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "LoginNew";
		base.Load += new System.EventHandler(LoginNew_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoginNew_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.PictureBox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PictureBox3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnConnection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnRefresh).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFinancialYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApplications).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
