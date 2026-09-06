// using Architecture.WinApp.Configurations;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Templates;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Templates;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.Templates
{

public class DefineCompany : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private int RecId;

	private DataTable dtHistoryGrid = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabDefineCompany;

	private Panel panel9;

	internal GridEX grd;

	private Panel panel4;

	private Button browse;

	private PictureBox LogoPic;

	private TextBox txtLastName;

	private Label label11;

	private TextBox txtFirstName;

	private Label label10;

	private TextBox txtContactPerson;

	private Label label9;

	private TextBox txtAdress;

	private Label label8;

	private TextBox txtState;

	private Label label12;

	private TextBox txtCountry;

	private Label label6;

	private TextBox txtPhoneNumber;

	private Label label5;

	private TextBox txtEmail;

	private Label label4;

	private TextBox txtReportingTitle;

	private Label label3;

	private TextBox txtName;

	private Label label2;

	private TextBox txtCode;

	private Label label1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnPrint;

	private ToolStripButton btnRefresh;

	private ToolStrip miniToolStrip;

	internal UltraCombo CmbOrganizationName;

	private Label label14;

	private TextBox txtEmail02;

	private Label label15;

	private Label label19;

	private Label label18;

	private Label label17;

	private TextBox txtMobile03;

	private TextBox txtMobile02;

	private TextBox txtMobile01;

	private TextBox txtAddressOtherLang;

	private Label label20;

	private TextBox txtcompanyNameOtherLAng;

	private Label label21;

	private TextBox txtcompanyWebsite;

	private Label label23;

	private TextBox txtCompanyFaxNo;

	private Label label22;

	private TextBox txtBaseCurrency;

	private Label label24;

	private TextBox txtUserAllowedCount;

	private Label label25;

	private Label label26;

	private Label label27;

	internal UltraCombo CmbTemplate;

	private Label label7;

	private TextBox txtCity;

	private Label label13;

	private Label label33;

	private Label label32;

	private Label label31;

	private Label label30;

	private Label label29;

	private Label label28;

	private Label label16;

	private Label label37;

	private Label label38;

	private Label label36;

	private Label label35;

	private Label label43;

	private Label label42;

	private Label label41;

	private Label label40;

	private Label label39;

	private Label label34;

	private TextBox txtCompanyId;

	private Label label44;

	private Panel PanelOtherForms;

	private TabPage tabScript;

	private Panel panel2;

	private Button BtnGenerateScript;

	internal UltraCombo CmbCompanyNameForScript;

	private Label label45;

	private Label label46;

	private ToolStripButton BtnTemplateDefine;

	public DefineCompany(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmCompanyReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1310, 550);
			OrganizationFill();
			TemplateNameFill();
			HistoryGridFill();
			tabControl1.TabPages.Remove(tabScript);
			FormHelper.AddTabPageToTabControl(tabControl1, "tabBranches");
			FormHelper.AddTabPageToTabControl(tabControl1, "tabFinancialYear");
			FormHelper.AddTabPageToTabControl(tabControl1, "tabModuleAllocateToOrganizationTemplates");
			FormHelper.AddTabPageToTabControl(tabControl1, "tabScreenAllocateToCompany");
			FormHelper.AddTabPageToTabControl(tabControl1, "tabUserRightsByCompany");
			FormHelper.AddTabPageToTabControl(tabControl1, "tabDefineUser");
			FormHelper.AddTabPageToTabControl(tabControl1, "tabScreenAllocateToUser");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrganizationFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbOrganizationName.Value);
			DataTable dt = Organization.GetAll();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbOrganizationName, "Id", "OrgName", "Organization Name", true);
			((UltraGridBase)CmbOrganizationName).Rows[0].Activate();
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbOrganizationName.Value = Id;
				}
				else
				{
					((Control)(object)CmbOrganizationName).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TemplateNameFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTemplate.Value);
			DataTable dt = OrganizationTemplates.FormHistory();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbTemplate, "Id", "TemplateName", "Template Name", true);
			((UltraGridBase)CmbTemplate).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbTemplate.Value = Id;
				}
				else
				{
					((Control)(object)CmbTemplate).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbOrganizationName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbOrganizationName).ActiveRow != null && Conversion.ToInt(CmbOrganizationName.Value) > 0)
			{
				Organization obj = Organization.GetByID(Conversion.ToInt(CmbOrganizationName.Value));
				if (obj != null)
				{
					txtReportingTitle.Text = obj.OrgReportingTitle;
					txtState.Text = obj.OrgState;
					txtCountry.Text = obj.OrgCountry;
					txtContactPerson.Text = obj.OrgContactPerson;
					txtEmail.Text = obj.OrgEmailA;
					txtPhoneNumber.Text = obj.OrgTel;
					CmbTemplate.Value = obj.OrganizaionTemplateId;
				}
			}
			else
			{
				txtReportingTitle.Text = "";
				txtState.Text = "";
				txtCountry.Text = "";
				txtContactPerson.Text = "";
				txtEmail.Text = "";
				txtPhoneNumber.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			LogoPic.Image = null;
			LogoPic.Invalidate();
			txtCode.Text = string.Empty;
			txtName.Text = string.Empty;
			txtReportingTitle.Text = string.Empty;
			txtContactPerson.Text = string.Empty;
			txtEmail.Text = string.Empty;
			txtEmail02.Text = string.Empty;
			txtAdress.Text = string.Empty;
			txtPhoneNumber.Text = string.Empty;
			txtMobile01.Text = string.Empty;
			txtMobile02.Text = string.Empty;
			txtMobile03.Text = string.Empty;
			txtCountry.Text = string.Empty;
			txtState.Text = string.Empty;
			txtFirstName.Text = string.Empty;
			txtcompanyWebsite.Text = string.Empty;
			txtCity.Text = string.Empty;
			txtcompanyNameOtherLAng.Text = string.Empty;
			txtCompanyFaxNo.Text = string.Empty;
			txtBaseCurrency.Text = string.Empty;
			txtUserAllowedCount.Text = string.Empty;
			((Control)(object)CmbTemplate).Text = string.Empty;
			txtAddressOtherLang.Text = string.Empty;
			HistoryGridFill();
			txtFirstName.Enabled = true;
			txtLastName.Enabled = true;
			btnsave.Visible = true;
			btnupdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
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
			OrganizationFill();
			TemplateNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbOrganizationName).ActiveRow == null || Conversion.ToInt(CmbOrganizationName.Value) == 0)
		{
			MessageBox.Show("Organization Field Required");
			CmbOrganizationName.Focus();
			return false;
		}
		if (txtCode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Company Code Field Required");
			txtCode.Focus();
			return false;
		}
		if (txtName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Company Name Field Required");
			txtName.Focus();
			return false;
		}
		if (txtReportingTitle.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Reporting Title Field Required");
			txtReportingTitle.Focus();
			return false;
		}
		if (txtEmail.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Email Field Required");
			txtEmail.Focus();
			return false;
		}
		if (txtPhoneNumber.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Phone Number Field Required");
			txtPhoneNumber.Focus();
			return false;
		}
		if (txtMobile01.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Mobile 01 Field Required");
			txtMobile01.Focus();
			return false;
		}
		if (txtAdress.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Address Field Required");
			txtAdress.Focus();
			return false;
		}
		if (txtCountry.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Country Field Required");
			txtCountry.Focus();
			return false;
		}
		if (txtState.Text.Trim() == string.Empty)
		{
			MessageBox.Show("State Field Required");
			txtState.Focus();
			return false;
		}
		if (txtCity.Text.Trim() == string.Empty)
		{
			MessageBox.Show("City Field Required");
			txtCity.Focus();
			return false;
		}
		if (txtBaseCurrency.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Base Currency Field Required");
			txtBaseCurrency.Focus();
			return false;
		}
		if (txtContactPerson.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Contact Person Field Required");
			txtContactPerson.Focus();
			return false;
		}
		if (txtUserAllowedCount.Text.Trim() == string.Empty || Conversion.ToInt((object)txtUserAllowedCount.Text) == 0)
		{
			MessageBox.Show("Allowed User Count Field Required");
			txtUserAllowedCount.Focus();
			return false;
		}
		return true;
	}

	private void InsertOnlyForSave()
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (txtFirstName.Text.Trim() == string.Empty)
			{
				MessageBox.Show("First Name Field Required");
				txtFirstName.Focus();
				return;
			}
			if (txtLastName.Text.Trim() == string.Empty)
			{
				MessageBox.Show("Last Name Field Required");
				txtLastName.Focus();
				return;
			}
			CompanyRegistration obj = new CompanyRegistration();
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.OrgCompanyTypeId = Conversion.ToInt(CmbOrganizationName.Value);
			obj.CompCode = txtCode.Text;
			obj.CompName = txtName.Text;
			obj.CompanyNameOtherLanguage = txtcompanyNameOtherLAng.Text;
			obj.CompanyNameOtherLing = txtcompanyNameOtherLAng.Text;
			obj.CompReportingTitle = txtReportingTitle.Text;
			obj.CompContactPerson = txtContactPerson.Text;
			obj.FirstName = txtFirstName.Text;
			obj.LastName = txtLastName.Text;
			obj.CompanyWebsite = txtcompanyWebsite.Text;
			obj.CompAddress = txtAdress.Text;
			obj.CompanyAddressOtherLanguage = txtAddressOtherLang.Text;
			obj.CompEmailA = txtEmail.Text;
			obj.CompEmailB = txtEmail02.Text;
			obj.CompTel = txtPhoneNumber.Text;
			obj.CompMobileA = txtMobile01.Text;
			obj.CompMobileB = txtMobile02.Text;
			obj.CompMobileC = txtMobile03.Text;
			obj.CompanyFaxNo = txtCompanyFaxNo.Text;
			obj.CompBaseCurr = txtBaseCurrency.Text;
			obj.AllowedUserCount = Conversion.ToInt((object)txtUserAllowedCount.Text);
			obj.CompCountry = txtCountry.Text;
			obj.CompState = txtState.Text;
			obj.CityName = txtCity.Text;
			obj.CompanyTemplateId = Conversion.ToInt(CmbTemplate.Value);
			obj.CompanyId = Conversion.ToInt((object)txtCompanyId.Text);
			using (MemoryStream stream = new MemoryStream())
			{
				if (LogoPic.Image != null)
				{
					Image img = LogoPic.Image;
					new Bitmap(img).Save(stream, img.RawFormat);
					byte[] pic = stream.ToArray();
					obj.CompLogoImage = pic;
					stream.Position = 0L;
					img.Dispose();
					stream.Dispose();
				}
			}
			Company.SaveAll(obj);
			MessageBox.Show("Save Successfully");
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			InsertOnlyForSave();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				if (!FormValidation())
				{
					return;
				}
				Company obj = new Company();
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
				obj.EntryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.EntryUser = UserAccount.ID;
				obj.ModifyUser = UserAccount.ID;
				obj.OrgCompanyTypeId = Conversion.ToInt(CmbOrganizationName.Value);
				obj.CompCode = txtCode.Text;
				obj.CompName = txtName.Text;
				obj.CompanyNameOtherLanguage = txtcompanyNameOtherLAng.Text;
				obj.CompanyNameOtherLing = txtcompanyNameOtherLAng.Text;
				obj.CompReportingTitle = txtReportingTitle.Text;
				obj.CompContactPerson = txtContactPerson.Text;
				obj.CompanyWebsite = txtcompanyWebsite.Text;
				obj.CompAddress = txtAdress.Text;
				obj.CompanyAddressOtherLanguage = txtAddressOtherLang.Text;
				obj.CompanyAddressOtherLanguage = txtAddressOtherLang.Text;
				obj.CompEmailA = txtEmail.Text;
				obj.CompEmailB = txtEmail02.Text;
				obj.CompTel = txtPhoneNumber.Text;
				obj.CompMobileA = txtMobile01.Text;
				obj.CompMobileB = txtMobile02.Text;
				obj.CompMobileC = txtMobile03.Text;
				obj.CompanyFaxNo = txtCompanyFaxNo.Text;
				obj.CompBaseCurr = txtBaseCurrency.Text;
				obj.AllowedUserCount = Conversion.ToInt((object)txtUserAllowedCount.Text);
				obj.CompCountry = txtCountry.Text;
				obj.CompState = txtState.Text;
				obj.CityName = txtCity.Text;
				obj.CompanyTemplateId = Conversion.ToInt(CmbTemplate.Value);
				obj.CompanyId = Conversion.ToInt((object)txtCompanyId.Text);
				using (MemoryStream stream = new MemoryStream())
				{
					if (LogoPic.Image != null)
					{
						Image img = LogoPic.Image;
						new Bitmap(img).Save(stream, img.RawFormat);
						byte[] pic = stream.ToArray();
						obj.CompLogoImage = pic;
						stream.Position = 0L;
						img.Dispose();
						stream.Dispose();
					}
				}
				Company.Save(obj);
				MessageBox.Show("Update Successfully");
				Reset();
				return;
			}
			throw new Exception("RecId Not Found...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId Not Found...");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			RecId = Id;
			Company obj = Company.GetByID(RecId);
			if (obj == null)
			{
				return;
			}
			RecId = obj.Id;
			CmbOrganizationName.Value = Conversion.ToInt((object)obj.OrgCompanyTypeId);
			txtCode.Text = Conversion.ToString((object)obj.CompCode);
			txtName.Text = Conversion.ToString((object)obj.CompName);
			txtcompanyNameOtherLAng.Text = Conversion.ToString((object)obj.CompanyNameOtherLanguage);
			txtReportingTitle.Text = Conversion.ToString((object)obj.CompReportingTitle);
			txtContactPerson.Text = Conversion.ToString((object)obj.CompContactPerson);
			txtFirstName.Text = Conversion.ToString((object)obj.FirstName);
			txtLastName.Text = Conversion.ToString((object)obj.LastName);
			txtcompanyWebsite.Text = Conversion.ToString((object)obj.CompanyWebsite);
			txtAdress.Text = Conversion.ToString((object)obj.CompAddress);
			txtAddressOtherLang.Text = Conversion.ToString((object)obj.CompanyAddressOtherLanguage);
			txtEmail.Text = Conversion.ToString((object)obj.CompEmailA);
			txtEmail02.Text = Conversion.ToString((object)obj.CompEmailB);
			txtPhoneNumber.Text = Conversion.ToString((object)obj.CompTel);
			txtMobile01.Text = Conversion.ToString((object)obj.CompMobileA);
			txtMobile02.Text = Conversion.ToString((object)obj.CompMobileB);
			txtMobile03.Text = Conversion.ToString((object)obj.CompMobileC);
			txtCompanyFaxNo.Text = Conversion.ToString((object)obj.CompanyFaxNo);
			txtBaseCurrency.Text = Conversion.ToString((object)obj.CompBaseCurr);
			txtUserAllowedCount.Text = Conversion.ToString((object)obj.AllowedUserCount);
			txtCompanyId.Text = Conversion.ToString((object)obj.CompanyId);
			txtCountry.Text = Conversion.ToString((object)obj.CompCountry);
			txtState.Text = Conversion.ToString((object)obj.CompState);
			txtCity.Text = Conversion.ToString((object)obj.CityName);
			CmbTemplate.Value = Conversion.ToInt((object)obj.CompanyTemplateId);
			if (obj.CompLogoImage != null)
			{
				using MemoryStream stream = new MemoryStream(obj.CompLogoImage);
				Bitmap bmp = new Bitmap(stream);
				LogoPic.Image = bmp;
			}
			tabControl1.SelectedIndex = 0;
			btnsave.Visible = false;
			btnupdate.Visible = true;
			UpdateMode = true;
			txtFirstName.Enabled = false;
			txtLastName.Enabled = false;
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
			Stream myStream = null;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png;*.gif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
			if (openFileDialog.ShowDialog(this) == DialogResult.OK && (myStream = openFileDialog.OpenFile()) != null)
			{
				string fileName = openFileDialog.FileName;
				if (myStream.Length > 5000000)
				{
					MessageBox.Show("File Size Limit Exceeded");
				}
				else
				{
					LogoPic.Image = Image.FromFile(fileName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedTab.Name == "tabDefineCompany")
			{
				PanelOtherForms.Visible = false;
				tabControl1.Dock = DockStyle.Fill;
			}
			else if (tabControl1.SelectedTab.Name == "tabScript")
			{
				PanelOtherForms.Visible = false;
				tabControl1.Dock = DockStyle.Fill;
			}
			else if (tabControl1.SelectedTab.Name == "tabBranches")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				frmBranches form = new frmBranches(UserAccount);
				FormHelper.OpenFormInPanel("frmBranches", form, PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabFinancialYear")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				frmFinancialYear form2 = new frmFinancialYear(UserAccount);
				FormHelper.OpenFormInPanel("frmFinancialYear", form2, PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabScreenAllocateToCompany")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				ScreensAllocateToCompany form3 = new ScreensAllocateToCompany(UserAccount);
				FormHelper.OpenFormInPanel("ScreensAllocateToCompany", form3, PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabDefineUser")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				frmUserRights form4 = new frmUserRights(UserAccount);
				FormHelper.OpenFormInPanel("frmUserRights", form4, PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabScreenAllocateToUser")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				UserRightsByCompany form5 = new UserRightsByCompany(UserAccount);
				FormHelper.OpenFormInPanel("UserRightsByCompany", form5, PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabModuleAllocateToOrganizationTemplates")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				frmModuleAllocateToOrganizationTemplates form6 = new frmModuleAllocateToOrganizationTemplates(UserAccount);
				FormHelper.OpenFormInPanel("frmModuleAllocateToOrganizationTemplates", form6, PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabUserRightsByCompany")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				UserRightsByCompany form7 = new UserRightsByCompany(UserAccount);
				FormHelper.OpenFormInPanel("UserRightsByCompany", form7, PanelOtherForms);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		try
		{
			dtHistoryGrid.Clear();
			dtHistoryGrid = Company.FormHistory(0);
			if (dtHistoryGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("OrgCompanyTypeId", typeof(int));
				dt.Columns.Add("OrganizationName", typeof(string));
				dt.Columns.Add("CompCode", typeof(string));
				dt.Columns.Add("CompName", typeof(string));
				dt.Columns.Add("CompContactPerson", typeof(string));
				dt.Columns.Add("FirstName", typeof(string));
				dt.Columns.Add("LastName", typeof(string));
				dt.Columns.Add("CompEmailA", typeof(string));
				dt.Columns.Add("CompEmailB", typeof(string));
				dt.Columns.Add("CompTel", typeof(string));
				dt.Columns.Add("CompMobileA", typeof(string));
				dt.Columns.Add("CompMobileB", typeof(string));
				dt.Columns.Add("CompMobileC", typeof(string));
				dt.Columns.Add("CompAddress", typeof(string));
				dt.Columns.Add("CompReportingTitle", typeof(string));
				dt.Columns.Add("CompLogo", typeof(string));
				dt.Columns.Add("CompCountry", typeof(string));
				dt.Columns.Add("CompState", typeof(string));
				dt.Columns.Add("CompBaseCurr", typeof(string));
				dt.Columns.Add("CompType", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("AllowedUserCount", typeof(int));
				dt.Columns.Add("CompanyTypeId", typeof(int));
				dt.Columns.Add("IsHeadOffice", typeof(bool));
				dt.Columns.Add("CompanyNameOtherLanguage", typeof(string));
				dt.Columns.Add("CompanyAddressOtherLanguage", typeof(string));
				dt.Columns.Add("CompanyWebsite", typeof(string));
				dt.Columns.Add("CompanyFaxNo", typeof(string));
				dt.Columns.Add("CompanyTemplateId", typeof(int));
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("CompanyId", typeof(int));
				foreach (DataRow row in dtHistoryGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["OrgCompanyTypeId"], row["OrgName"], row["CompCode"], row["CompName"], row["CompContactPerson"], row["FirstName"], row["LastName"], row["CompEmailA"], row["CompEmailB"], row["CompTel"], row["CompMobileA"], row["CompMobileB"], row["CompMobileC"], row["CompAddress"], row["CompReportingTitle"], row["CompLogo"], row["CompCountry"], row["CompState"], row["CompBaseCurr"], row["CompType"], row["EntryUser"], row["EntryDate"], row["ModifyUser"], row["ModifyDate"], row["AllowedUserCount"], row["CompanyTypeId"], row["IsHeadOffice"], row["CompanyNameOtherLanguage"], row["CompanyAddressOtherLanguage"], row["CompanyWebsite"], row["CompanyFaxNo"], row["CompanyTemplateId"], row["CityName"], row["CompanyId"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grd.ClearStructure();
			}
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["OrgCompanyTypeId"].Visible = false;
			grd.RootTable.Columns["CompanyTypeId"].Visible = false;
			grd.RootTable.Columns["CompanyTemplateId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].Width = 40;
			grd.RootTable.Columns["Edit"].Position = 0;
			grd.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					ReadById(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmCompanyReport_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled && UpdateMode)
				{
					btnupdate_Click(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
	{
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Organization Code");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Entry First Colum");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
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

	private void txtCompanyCount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCompanyId_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnTemplateDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineOrgnization(UserAccount).Show();
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
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Expected O, but got Unknown
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Expected O, but got Unknown
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Templates.DefineCompany));
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabDefineCompany = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtAdress = new System.Windows.Forms.TextBox();
		this.CmbTemplate = new UltraCombo();
		this.txtLastName = new System.Windows.Forms.TextBox();
		this.txtFirstName = new System.Windows.Forms.TextBox();
		this.txtContactPerson = new System.Windows.Forms.TextBox();
		this.txtUserAllowedCount = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtMobile03 = new System.Windows.Forms.TextBox();
		this.txtMobile02 = new System.Windows.Forms.TextBox();
		this.txtMobile01 = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbOrganizationName = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.browse = new System.Windows.Forms.Button();
		this.LogoPic = new System.Windows.Forms.PictureBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtAddressOtherLang = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtCity = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtState = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtCompanyId = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.txtBaseCurrency = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtCountry = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtPhoneNumber = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtEmail02 = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtcompanyWebsite = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.txtCompanyFaxNo = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtReportingTitle = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtcompanyNameOtherLAng = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.txtName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.tabScript = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.BtnGenerateScript = new System.Windows.Forms.Button();
		this.CmbCompanyNameForScript = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.PanelOtherForms = new System.Windows.Forms.Panel();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.BtnTemplateDefine = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabDefineCompany.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel9.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplate).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrganizationName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.LogoPic).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabScript.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyNameForScript).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Controls.Add(this.PanelOtherForms);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1294, 720);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabDefineCompany);
		this.tabControl1.Controls.Add(this.tabScript);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(10, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1284, 720);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabDefineCompany.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDefineCompany.Controls.Add(this.panel9);
		this.tabDefineCompany.Controls.Add(this.panel4);
		this.tabDefineCompany.Controls.Add(this.panel3);
		this.tabDefineCompany.Location = new System.Drawing.Point(4, 25);
		this.tabDefineCompany.Name = "tabDefineCompany";
		this.tabDefineCompany.Padding = new System.Windows.Forms.Padding(3);
		this.tabDefineCompany.Size = new System.Drawing.Size(1276, 691);
		this.tabDefineCompany.TabIndex = 0;
		this.tabDefineCompany.Text = "Define Company";
		this.tabDefineCompany.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 266);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1270, 422);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label26);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 244);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1270, 22);
		this.panel9.TabIndex = 2;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(3, 3);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(63, 18);
		this.label26.TabIndex = 1;
		this.label26.Text = "History";
		this.panel4.Controls.Add(this.txtAdress);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTemplate);
		this.panel4.Controls.Add(this.txtLastName);
		this.panel4.Controls.Add(this.txtFirstName);
		this.panel4.Controls.Add(this.txtContactPerson);
		this.panel4.Controls.Add(this.txtUserAllowedCount);
		this.panel4.Controls.Add(this.label37);
		this.panel4.Controls.Add(this.label38);
		this.panel4.Controls.Add(this.label36);
		this.panel4.Controls.Add(this.label35);
		this.panel4.Controls.Add(this.label43);
		this.panel4.Controls.Add(this.label42);
		this.panel4.Controls.Add(this.label41);
		this.panel4.Controls.Add(this.label40);
		this.panel4.Controls.Add(this.label39);
		this.panel4.Controls.Add(this.label34);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.label32);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.label30);
		this.panel4.Controls.Add(this.label29);
		this.panel4.Controls.Add(this.label28);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.txtMobile03);
		this.panel4.Controls.Add(this.txtMobile02);
		this.panel4.Controls.Add(this.txtMobile01);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrganizationName);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.browse);
		this.panel4.Controls.Add(this.LogoPic);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.txtAddressOtherLang);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtCity);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.txtState);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtCompanyId);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Controls.Add(this.txtBaseCurrency);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.txtCountry);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtPhoneNumber);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtEmail02);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.txtEmail);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.txtcompanyWebsite);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Controls.Add(this.txtCompanyFaxNo);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.txtReportingTitle);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtcompanyNameOtherLAng);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.txtName);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtCode);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1270, 189);
		this.panel4.TabIndex = 0;
		this.txtAdress.Location = new System.Drawing.Point(390, 134);
		this.txtAdress.Name = "txtAdress";
		this.txtAdress.Size = new System.Drawing.Size(364, 23);
		this.txtAdress.TabIndex = 12;
		this.CmbTemplate.AllowNull = (DefaultableBoolean)1;
		this.CmbTemplate.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTemplate.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTemplate.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Enabled = false;
		this.CmbTemplate.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Location = new System.Drawing.Point(861, 107);
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Name = "CmbTemplate";
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbTemplate).TabIndex = 23;
		this.txtLastName.Location = new System.Drawing.Point(861, 57);
		this.txtLastName.Name = "txtLastName";
		this.txtLastName.Size = new System.Drawing.Size(191, 23);
		this.txtLastName.TabIndex = 21;
		this.txtFirstName.Location = new System.Drawing.Point(861, 32);
		this.txtFirstName.Name = "txtFirstName";
		this.txtFirstName.Size = new System.Drawing.Size(191, 23);
		this.txtFirstName.TabIndex = 20;
		this.txtContactPerson.Location = new System.Drawing.Point(861, 6);
		this.txtContactPerson.Name = "txtContactPerson";
		this.txtContactPerson.Size = new System.Drawing.Size(191, 23);
		this.txtContactPerson.TabIndex = 19;
		this.txtUserAllowedCount.Location = new System.Drawing.Point(861, 82);
		this.txtUserAllowedCount.Name = "txtUserAllowedCount";
		this.txtUserAllowedCount.Size = new System.Drawing.Size(191, 23);
		this.txtUserAllowedCount.TabIndex = 22;
		this.txtUserAllowedCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCompanyCount_KeyPress);
		this.label37.AutoSize = true;
		this.label37.BackColor = System.Drawing.Color.White;
		this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(614, 33);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(15, 20);
		this.label37.TabIndex = 208;
		this.label37.Text = "*";
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.White;
		this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(614, 83);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(15, 20);
		this.label38.TabIndex = 208;
		this.label38.Text = "*";
		this.label36.AutoSize = true;
		this.label36.BackColor = System.Drawing.Color.White;
		this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(614, 58);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(15, 20);
		this.label36.TabIndex = 208;
		this.label36.Text = "*";
		this.label35.AutoSize = true;
		this.label35.BackColor = System.Drawing.Color.White;
		this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(844, 110);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(15, 20);
		this.label35.TabIndex = 208;
		this.label35.Text = "*";
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.White;
		this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(844, 58);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(15, 20);
		this.label43.TabIndex = 208;
		this.label43.Text = "*";
		this.label42.AutoSize = true;
		this.label42.BackColor = System.Drawing.Color.White;
		this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(844, 33);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(15, 20);
		this.label42.TabIndex = 208;
		this.label42.Text = "*";
		this.label41.AutoSize = true;
		this.label41.BackColor = System.Drawing.Color.White;
		this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(844, 7);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(15, 20);
		this.label41.TabIndex = 208;
		this.label41.Text = "*";
		this.label40.AutoSize = true;
		this.label40.BackColor = System.Drawing.Color.White;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(374, 135);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(15, 20);
		this.label40.TabIndex = 208;
		this.label40.Text = "*";
		this.label39.AutoSize = true;
		this.label39.BackColor = System.Drawing.Color.White;
		this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(614, 110);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(15, 20);
		this.label39.TabIndex = 208;
		this.label39.Text = "*";
		this.label34.AutoSize = true;
		this.label34.BackColor = System.Drawing.Color.White;
		this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(844, 83);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(15, 20);
		this.label34.TabIndex = 208;
		this.label34.Text = "*";
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.White;
		this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(374, 33);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(15, 20);
		this.label33.TabIndex = 208;
		this.label33.Text = "*";
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.White;
		this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(374, 7);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(15, 20);
		this.label32.TabIndex = 208;
		this.label32.Text = "*";
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(84, 135);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(15, 20);
		this.label31.TabIndex = 208;
		this.label31.Text = "*";
		this.label30.AutoSize = true;
		this.label30.BackColor = System.Drawing.Color.White;
		this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(84, 110);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(15, 20);
		this.label30.TabIndex = 208;
		this.label30.Text = "*";
		this.label29.AutoSize = true;
		this.label29.BackColor = System.Drawing.Color.White;
		this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(84, 58);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(15, 20);
		this.label29.TabIndex = 208;
		this.label29.Text = "*";
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.White;
		this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(84, 33);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(15, 20);
		this.label28.TabIndex = 208;
		this.label28.Text = "*";
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(84, 7);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(15, 20);
		this.label16.TabIndex = 208;
		this.label16.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(293, 86);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(61, 15);
		this.label19.TabIndex = 205;
		this.label19.Text = "Mobile 03";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(293, 61);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(61, 15);
		this.label18.TabIndex = 206;
		this.label18.Text = "Mobile 02";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(293, 36);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(59, 15);
		this.label17.TabIndex = 207;
		this.label17.Text = "Mobile 01";
		this.txtMobile03.Location = new System.Drawing.Point(390, 82);
		this.txtMobile03.Name = "txtMobile03";
		this.txtMobile03.Size = new System.Drawing.Size(143, 23);
		this.txtMobile03.TabIndex = 10;
		this.txtMobile02.Location = new System.Drawing.Point(390, 57);
		this.txtMobile02.Name = "txtMobile02";
		this.txtMobile02.Size = new System.Drawing.Size(143, 23);
		this.txtMobile02.TabIndex = 9;
		this.txtMobile01.Location = new System.Drawing.Point(390, 32);
		this.txtMobile01.Name = "txtMobile01";
		this.txtMobile01.Size = new System.Drawing.Size(143, 23);
		this.txtMobile01.TabIndex = 8;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(759, 113);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(56, 15);
		this.label7.TabIndex = 204;
		this.label7.Text = "Template";
		this.CmbOrganizationName.AllowNull = (DefaultableBoolean)1;
		this.CmbOrganizationName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrganizationName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrganizationName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrganizationName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrganizationName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbOrganizationName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).Location = new System.Drawing.Point(101, 4);
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).Name = "CmbOrganizationName";
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).Leave += new System.EventHandler(CmbOrganizationName_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(4, 10);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(76, 15);
		this.label14.TabIndex = 204;
		this.label14.Text = "Organization";
		this.browse.BackColor = System.Drawing.Color.Teal;
		this.browse.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.browse.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.browse.ForeColor = System.Drawing.Color.White;
		this.browse.Location = new System.Drawing.Point(1055, 153);
		this.browse.Name = "browse";
		this.browse.Size = new System.Drawing.Size(210, 29);
		this.browse.TabIndex = 25;
		this.browse.Text = "Browse";
		this.browse.UseVisualStyleBackColor = false;
		this.browse.Click += new System.EventHandler(browse_Click);
		this.LogoPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.LogoPic.Location = new System.Drawing.Point(1055, 6);
		this.LogoPic.Name = "LogoPic";
		this.LogoPic.Size = new System.Drawing.Size(210, 144);
		this.LogoPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.LogoPic.TabIndex = 2;
		this.LogoPic.TabStop = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(759, 61);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(63, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Last Name";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(759, 36);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(64, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "First Name";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(759, 10);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(87, 15);
		this.label9.TabIndex = 0;
		this.label9.Text = "Contact Person";
		this.txtAddressOtherLang.Location = new System.Drawing.Point(390, 159);
		this.txtAddressOtherLang.Name = "txtAddressOtherLang";
		this.txtAddressOtherLang.Size = new System.Drawing.Size(662, 23);
		this.txtAddressOtherLang.TabIndex = 13;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(293, 163);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(82, 15);
		this.label20.TabIndex = 0;
		this.label20.Text = "Other Address";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(293, 138);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(49, 15);
		this.label8.TabIndex = 0;
		this.label8.Text = "Address";
		this.txtCity.Location = new System.Drawing.Point(629, 82);
		this.txtCity.Name = "txtCity";
		this.txtCity.Size = new System.Drawing.Size(125, 23);
		this.txtCity.TabIndex = 17;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(537, 86);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(27, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "City";
		this.txtState.Location = new System.Drawing.Point(629, 57);
		this.txtState.Name = "txtState";
		this.txtState.Size = new System.Drawing.Size(125, 23);
		this.txtState.TabIndex = 16;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(537, 61);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(34, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "State";
		this.txtCompanyId.Location = new System.Drawing.Point(861, 134);
		this.txtCompanyId.Name = "txtCompanyId";
		this.txtCompanyId.Size = new System.Drawing.Size(191, 23);
		this.txtCompanyId.TabIndex = 24;
		this.txtCompanyId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCompanyId_KeyPress);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(759, 138);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(72, 15);
		this.label44.TabIndex = 0;
		this.label44.Text = "Company Id";
		this.txtBaseCurrency.Location = new System.Drawing.Point(629, 109);
		this.txtBaseCurrency.Name = "txtBaseCurrency";
		this.txtBaseCurrency.Size = new System.Drawing.Size(124, 23);
		this.txtBaseCurrency.TabIndex = 18;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(537, 113);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(81, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Base Currency";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(759, 86);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(81, 15);
		this.label25.TabIndex = 0;
		this.label25.Text = "Allowed Users";
		this.txtCountry.Location = new System.Drawing.Point(629, 32);
		this.txtCountry.Name = "txtCountry";
		this.txtCountry.Size = new System.Drawing.Size(125, 23);
		this.txtCountry.TabIndex = 15;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(537, 36);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(49, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Country";
		this.txtPhoneNumber.Location = new System.Drawing.Point(390, 6);
		this.txtPhoneNumber.Name = "txtPhoneNumber";
		this.txtPhoneNumber.Size = new System.Drawing.Size(143, 23);
		this.txtPhoneNumber.TabIndex = 7;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(293, 10);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(85, 15);
		this.label5.TabIndex = 0;
		this.label5.Text = "PhoneNumber";
		this.txtEmail02.Location = new System.Drawing.Point(101, 159);
		this.txtEmail02.Name = "txtEmail02";
		this.txtEmail02.Size = new System.Drawing.Size(189, 23);
		this.txtEmail02.TabIndex = 6;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(4, 163);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(47, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Email II";
		this.txtEmail.Location = new System.Drawing.Point(101, 134);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(189, 23);
		this.txtEmail.TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(4, 138);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(43, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Email I";
		this.txtcompanyWebsite.Location = new System.Drawing.Point(629, 6);
		this.txtcompanyWebsite.Name = "txtcompanyWebsite";
		this.txtcompanyWebsite.Size = new System.Drawing.Size(124, 23);
		this.txtcompanyWebsite.TabIndex = 14;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(537, 10);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(50, 15);
		this.label23.TabIndex = 0;
		this.label23.Text = "Website";
		this.txtCompanyFaxNo.Location = new System.Drawing.Point(390, 109);
		this.txtCompanyFaxNo.Name = "txtCompanyFaxNo";
		this.txtCompanyFaxNo.Size = new System.Drawing.Size(143, 23);
		this.txtCompanyFaxNo.TabIndex = 11;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(293, 113);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(42, 15);
		this.label22.TabIndex = 0;
		this.label22.Text = "Fax no";
		this.txtReportingTitle.Location = new System.Drawing.Point(101, 109);
		this.txtReportingTitle.Name = "txtReportingTitle";
		this.txtReportingTitle.Size = new System.Drawing.Size(189, 23);
		this.txtReportingTitle.TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(4, 113);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "ReportingTitle";
		this.txtcompanyNameOtherLAng.Location = new System.Drawing.Point(101, 82);
		this.txtcompanyNameOtherLAng.Name = "txtcompanyNameOtherLAng";
		this.txtcompanyNameOtherLAng.Size = new System.Drawing.Size(189, 23);
		this.txtcompanyNameOtherLAng.TabIndex = 3;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(4, 86);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(72, 15);
		this.label21.TabIndex = 0;
		this.label21.Text = "Other Name";
		this.txtName.Location = new System.Drawing.Point(101, 57);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(189, 23);
		this.txtName.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(4, 61);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(39, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Name";
		this.txtCode.Location = new System.Drawing.Point(101, 32);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(189, 23);
		this.txtCode.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(4, 36);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(34, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Code";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label27);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1270, 52);
		this.panel3.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(4, 30);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(84, 19);
		this.label27.TabIndex = 11;
		this.label27.Text = "Company";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.btnRefresh, this.btnsave, this.btnupdate, this.btnPrint, this.BtnTemplateDefine });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1270, 27);
		this.toolStrip1.TabIndex = 10;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.tabScript.Controls.Add(this.panel2);
		this.tabScript.Location = new System.Drawing.Point(4, 25);
		this.tabScript.Name = "tabScript";
		this.tabScript.Size = new System.Drawing.Size(1276, 691);
		this.tabScript.TabIndex = 1;
		this.tabScript.Text = "Generate Script";
		this.tabScript.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.BtnGenerateScript);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompanyNameForScript);
		this.panel2.Controls.Add(this.label45);
		this.panel2.Controls.Add(this.label46);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1276, 38);
		this.panel2.TabIndex = 0;
		this.BtnGenerateScript.BackColor = System.Drawing.Color.Teal;
		this.BtnGenerateScript.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.BtnGenerateScript.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnGenerateScript.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.BtnGenerateScript.ForeColor = System.Drawing.Color.White;
		this.BtnGenerateScript.Location = new System.Drawing.Point(298, 6);
		this.BtnGenerateScript.Name = "BtnGenerateScript";
		this.BtnGenerateScript.Size = new System.Drawing.Size(90, 26);
		this.BtnGenerateScript.TabIndex = 216;
		this.BtnGenerateScript.Text = "Generate";
		this.BtnGenerateScript.UseVisualStyleBackColor = false;
		this.CmbCompanyNameForScript.AllowNull = (DefaultableBoolean)1;
		this.CmbCompanyNameForScript.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompanyNameForScript.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompanyNameForScript.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompanyNameForScript).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameForScript).Enabled = false;
		this.CmbCompanyNameForScript.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameForScript).Location = new System.Drawing.Point(77, 6);
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameForScript).Name = "CmbCompanyNameForScript";
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameForScript).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameForScript).TabIndex = 213;
		this.label45.AutoSize = true;
		this.label45.BackColor = System.Drawing.Color.White;
		this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(60, 9);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(15, 20);
		this.label45.TabIndex = 215;
		this.label45.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(6, 12);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(58, 15);
		this.label46.TabIndex = 214;
		this.label46.Text = "Company";
		this.PanelOtherForms.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelOtherForms.Location = new System.Drawing.Point(0, 0);
		this.PanelOtherForms.Name = "PanelOtherForms";
		this.PanelOtherForms.Size = new System.Drawing.Size(10, 720);
		this.PanelOtherForms.TabIndex = 172;
		this.PanelOtherForms.Visible = false;
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(126, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(885, 27);
		this.miniToolStrip.TabIndex = 11;
		this.BtnTemplateDefine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnTemplateDefine.Image = (System.Drawing.Image)resources.GetObject("BtnTemplateDefine.Image");
		this.BtnTemplateDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnTemplateDefine.Name = "BtnTemplateDefine";
		this.BtnTemplateDefine.Size = new System.Drawing.Size(139, 24);
		this.BtnTemplateDefine.Text = "Define Organization";
		this.BtnTemplateDefine.Click += new System.EventHandler(BtnTemplateDefine_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1294, 720);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineCompany";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmCompanyReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmCompanyReport_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabDefineCompany.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplate).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrganizationName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.LogoPic).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabScript.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyNameForScript).EndInit();
		base.ResumeLayout(false);
	}
}

}
