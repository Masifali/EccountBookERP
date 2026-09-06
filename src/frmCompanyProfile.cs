using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
namespace Architecture.WinApp.Configurations
{

public class frmCompanyProfile : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private int RecId;

	private int CompanyId;

	private DataTable dtgrd = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabCompany;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel9;

	internal GridEX grd;

	private TextBox txtEmail;

	private Label label4;

	private TextBox txtReportingTitle;

	private Label label3;

	private TextBox txtName;

	private Label label2;

	private TextBox txtCode;

	private Label label1;

	private TextBox txtAdress;

	private Label label8;

	private TextBox txtCountry;

	private Label label6;

	private TextBox txtPhoneNumber;

	private Label label5;

	private TextBox txtContactPerson;

	private Label label9;

	private TextBox txtState;

	private Label label12;

	private PictureBox LogoPic;

	private Button browse;

	internal UltraCombo CmbOrganizationName;

	private Label label14;

	private TextBox txtAddressOtherLingo;

	private TextBox txtCompanynameOtherLingo;

	private Label label10;

	private Label label7;

	private ToolStripButton BtnShortCutkeys;

	public frmCompanyProfile(UserAccount userAccount)
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
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 650);
			OrganizationFill();
			CmbOrganizationName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddTabPage(string tabName, Form form)
	{
		TabPage existingTab = tabControl1.TabPages.Cast<TabPage>().FirstOrDefault((TabPage tp) => tp.Name == tabName);
		if (existingTab == null)
		{
			existingTab = new TabPage
			{
				Name = tabName,
				Text = tabName.Substring(3)
			};
			tabControl1.TabPages.Add(existingTab);
		}
		form.TopLevel = false;
		form.Visible = true;
		form.FormBorderStyle = FormBorderStyle.None;
		form.Dock = DockStyle.Fill;
		existingTab.Controls.Add(form);
	}

	private void OrganizationFill()
	{
		try
		{
			Organization obj = Organization.GetByID(UserAccount.OrganizationId);
			if (obj != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("OrgName");
				dt.Rows.Add(obj.Id, obj.OrgName);
				DDL.BindDDL(dt, CmbOrganizationName, "Id", "OrgName", "Organization Name", ZeroIndex: false);
				((UltraGridBase)CmbOrganizationName).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrganizationName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbOrganizationName).ActiveRow != null)
			{
				LocationsBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationsBind()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		try
		{
			Reset();
			if (((UltraGridBase)CmbOrganizationName).ActiveRow == null)
			{
				return;
			}
			DataTable dt = Company.GetAlldt(new Company
			{
				OrgCompanyTypeId = Conversion.ToInt(CmbOrganizationName.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("CompanyId", typeof(int));
				dtGrid.Columns.Add("OrgId");
				dtGrid.Columns.Add("CompanyCode");
				dtGrid.Columns.Add("CompanyName");
				dtGrid.Columns.Add("OtherName");
				dtGrid.Columns.Add("ReportingTitle");
				dtGrid.Columns.Add("Email");
				dtGrid.Columns.Add("Phone");
				dtGrid.Columns.Add("Country");
				dtGrid.Columns.Add("State");
				dtGrid.Columns.Add("Address");
				dtGrid.Columns.Add("OtherAddress");
				dtGrid.Columns.Add("ContactPerson");
				dtGrid.Columns.Add("LogoImage", typeof(byte[]));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CompanyId"], dt.Rows[i]["OrgCompanyTypeId"], dt.Rows[i]["CompCode"], dt.Rows[i]["CompName"], dt.Rows[i]["CompanyNameOtherLanguage"], dt.Rows[i]["CompReportingTitle"], dt.Rows[i]["CompEmailA"], dt.Rows[i]["CompMobileA"], dt.Rows[i]["CompCountry"], dt.Rows[i]["CompState"], dt.Rows[i]["CompAddress"], dt.Rows[i]["CompanyAddressOtherLanguage"], dt.Rows[i]["CompContactPerson"], Conversion.ToByteArray(dt.Rows[i]["CompLogoImage"]));
				}
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grd.RootTable.Columns["Id"].Visible = false;
				grd.RootTable.Columns["CompanyId"].Visible = false;
				grd.RootTable.Columns["OrgId"].Visible = false;
				grd.RootTable.Columns["LogoImage"].Visible = false;
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

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				RecId = Conversion.ToInt(item.Cells["Id"].Value);
				CmbOrganizationName.Value = item.Cells["OrgId"].Value.ToString();
				txtCode.Text = item.Cells["CompanyCode"].Value.ToString();
				txtName.Text = item.Cells["CompanyName"].Value.ToString();
				txtCompanynameOtherLingo.Text = item.Cells["OtherName"].Value.ToString();
				txtReportingTitle.Text = item.Cells["ReportingTitle"].Value.ToString();
				txtEmail.Text = item.Cells["Email"].Value.ToString();
				txtPhoneNumber.Text = item.Cells["Phone"].Value.ToString();
				txtCountry.Text = item.Cells["Country"].Value.ToString();
				txtState.Text = item.Cells["State"].Value.ToString();
				txtAdress.Text = item.Cells["Address"].Value.ToString();
				txtAddressOtherLingo.Text = item.Cells["OtherAddress"].Value.ToString();
				txtContactPerson.Text = item.Cells["ContactPerson"].Value.ToString();
				LogoPic.Image = (Bitmap)item.Cells["LogoImage"].Value;
				CompanyId = Conversion.ToInt(item.Cells["CompanyId"].Value);
				btnsave.Visible = false;
				btnupdate.Visible = true;
			}
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
			MessageBox.Show("CompanyName Field is Required");
			CmbOrganizationName.Focus();
			return false;
		}
		if (txtCode.Text.Trim() == string.Empty || txtCode.Text.Trim() == "0")
		{
			MessageBox.Show("Code Field is Required");
			txtCode.Focus();
			return false;
		}
		if (txtName.Text.Trim() == string.Empty || txtName.Text.Trim() == "0")
		{
			MessageBox.Show("Name Field is Required");
			txtName.Focus();
			return false;
		}
		if (txtReportingTitle.Text.Trim() == string.Empty || txtReportingTitle.Text.Trim() == "0")
		{
			MessageBox.Show("Reporting Title Field is Required");
			txtReportingTitle.Focus();
			return false;
		}
		if (txtContactPerson.Text.Trim() == string.Empty || txtContactPerson.Text.Trim() == "0")
		{
			MessageBox.Show("Contact Person Field is Required");
			txtContactPerson.Focus();
			return false;
		}
		if (txtAdress.Text.Trim() == string.Empty || txtAdress.Text.Trim() == "0")
		{
			MessageBox.Show("Address Field is Required");
			txtAdress.Focus();
			return false;
		}
		if (txtEmail.Text.Trim() == string.Empty || txtEmail.Text.Trim() == "0")
		{
			MessageBox.Show("Email Field is Required");
			txtEmail.Focus();
			return false;
		}
		if (txtPhoneNumber.Text.Trim() == string.Empty || txtPhoneNumber.Text.Trim() == "0")
		{
			MessageBox.Show("Phone Number Field is Required");
			txtPhoneNumber.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			Company obj = new Company();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
				obj.CompanyId = CompanyId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.OrgCompanyTypeId = Conversion.ToInt(CmbOrganizationName.Value);
			obj.CompCode = txtCode.Text;
			obj.CompName = txtName.Text;
			obj.CompanyNameOtherLanguage = txtCompanynameOtherLingo.Text;
			obj.CompanyNameOtherLing = txtCompanynameOtherLingo.Text;
			obj.CompReportingTitle = txtReportingTitle.Text;
			obj.CompEmailA = txtEmail.Text;
			obj.CompMobileA = txtPhoneNumber.Text;
			obj.CompCountry = txtCountry.Text;
			obj.CompState = txtState.Text;
			obj.CompAddress = txtAdress.Text;
			obj.CompanyAddressOtherLanguage = txtAddressOtherLingo.Text;
			obj.CompContactPerson = txtContactPerson.Text;
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
				else
				{
					obj.CompLogoImage = Conversion.ToByteArray((object)DBNull.Value);
				}
			}
			Company.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			LocationsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadById(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			CompanyReport obj = new CompanyReport();
			obj = CompanyReport.GetByID(Id);
			if (obj != null)
			{
				RecId = obj.CompanyReportId;
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnupdate.Visible = true;
				UpdateMode = true;
			}
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
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
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

	private void Reset()
	{
		try
		{
			txtCode.Clear();
			txtName.Clear();
			txtReportingTitle.Clear();
			txtEmail.Clear();
			txtPhoneNumber.Clear();
			txtCountry.Clear();
			txtState.Clear();
			txtAdress.Clear();
			txtContactPerson.Clear();
			txtAddressOtherLingo.Clear();
			txtCompanynameOtherLingo.Clear();
			CompanyId = 0;
			LogoPic.Image = null;
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

	private void frmCompanyProfile_FormClosing(object sender, FormClosingEventArgs e)
	{
		HistoryStack.Remove(this);
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			OrganizationFill();
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
			_ = tabControl1.SelectedIndex;
			_ = 1;
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					tabControl1.SelectedIndex = 1;
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 2;
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					tabControl1.SelectedIndex = 3;
				}
				else if (tabControl1.SelectedIndex == 3)
				{
					tabControl1.SelectedIndex = 4;
				}
				else if (tabControl1.SelectedIndex == 4)
				{
					tabControl1.SelectedIndex = 5;
				}
				else if (tabControl1.SelectedIndex == 5)
				{
					tabControl1.SelectedIndex = 0;
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					CmbOrganizationName.Focus();
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 121;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbOrganizationName.Focus();
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 80;
				}
				if (e.Alt && e.KeyCode != Keys.D1)
				{
					_ = e.KeyCode;
					_ = 97;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Organization combo");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+F12", "For SaveAs");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on  Organization combo");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (!(grd.CurrentColumn.Key == "Delete"))
				{
				}
			}
			else if (e.Control && e.Shift)
			{
				_ = e.KeyCode;
				_ = 46;
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
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Configurations.frmCompanyProfile));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabCompany = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbOrganizationName = new UltraCombo();
		this.txtAddressOtherLingo = new System.Windows.Forms.TextBox();
		this.txtAdress = new System.Windows.Forms.TextBox();
		this.txtCompanynameOtherLingo = new System.Windows.Forms.TextBox();
		this.txtReportingTitle = new System.Windows.Forms.TextBox();
		this.txtName = new System.Windows.Forms.TextBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.browse = new System.Windows.Forms.Button();
		this.LogoPic = new System.Windows.Forms.PictureBox();
		this.txtContactPerson = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtState = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtCountry = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtPhoneNumber = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabCompany.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel9.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrganizationName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.LogoPic).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(965, 630);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabCompany);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(965, 630);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabCompany.Controls.Add(this.panel2);
		this.tabCompany.Location = new System.Drawing.Point(4, 25);
		this.tabCompany.Name = "tabCompany";
		this.tabCompany.Padding = new System.Windows.Forms.Padding(3);
		this.tabCompany.Size = new System.Drawing.Size(957, 601);
		this.tabCompany.TabIndex = 0;
		this.tabCompany.Text = "Form";
		this.tabCompany.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(951, 595);
		this.panel2.TabIndex = 0;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 258);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(951, 337);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 235);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(951, 23);
		this.panel9.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(901, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(46, 20);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrganizationName);
		this.panel4.Controls.Add(this.txtAddressOtherLingo);
		this.panel4.Controls.Add(this.txtAdress);
		this.panel4.Controls.Add(this.txtCompanynameOtherLingo);
		this.panel4.Controls.Add(this.txtReportingTitle);
		this.panel4.Controls.Add(this.txtName);
		this.panel4.Controls.Add(this.txtCode);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.browse);
		this.panel4.Controls.Add(this.LogoPic);
		this.panel4.Controls.Add(this.txtContactPerson);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtState);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtCountry);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtPhoneNumber);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtEmail);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 48);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(951, 187);
		this.panel4.TabIndex = 1;
		this.CmbOrganizationName.AllowNull = (DefaultableBoolean)1;
		this.CmbOrganizationName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrganizationName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrganizationName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrganizationName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrganizationName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrganizationName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrganizationName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).Location = new System.Drawing.Point(114, 5);
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).Name = "CmbOrganizationName";
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbOrganizationName).TextChanged += new System.EventHandler(CmbOrganizationName_TextChanged);
		this.txtAddressOtherLingo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddressOtherLingo.Location = new System.Drawing.Point(114, 159);
		this.txtAddressOtherLingo.Name = "txtAddressOtherLingo";
		this.txtAddressOtherLingo.Size = new System.Drawing.Size(524, 23);
		this.txtAddressOtherLingo.TabIndex = 6;
		this.txtAdress.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAdress.Location = new System.Drawing.Point(114, 133);
		this.txtAdress.Name = "txtAdress";
		this.txtAdress.Size = new System.Drawing.Size(524, 23);
		this.txtAdress.TabIndex = 5;
		this.txtCompanynameOtherLingo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCompanynameOtherLingo.Location = new System.Drawing.Point(114, 83);
		this.txtCompanynameOtherLingo.Name = "txtCompanynameOtherLingo";
		this.txtCompanynameOtherLingo.Size = new System.Drawing.Size(226, 23);
		this.txtCompanynameOtherLingo.TabIndex = 3;
		this.txtReportingTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReportingTitle.Location = new System.Drawing.Point(114, 108);
		this.txtReportingTitle.Name = "txtReportingTitle";
		this.txtReportingTitle.Size = new System.Drawing.Size(226, 23);
		this.txtReportingTitle.TabIndex = 4;
		this.txtName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtName.Location = new System.Drawing.Point(114, 58);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(226, 23);
		this.txtName.TabIndex = 2;
		this.txtCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCode.Location = new System.Drawing.Point(114, 33);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(226, 23);
		this.txtCode.TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(4, 11);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(111, 15);
		this.label14.TabIndex = 202;
		this.label14.Text = "Organization Name";
		this.browse.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.browse.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.browse.ForeColor = System.Drawing.Color.White;
		this.browse.Location = new System.Drawing.Point(644, 158);
		this.browse.Name = "browse";
		this.browse.Size = new System.Drawing.Size(303, 24);
		this.browse.TabIndex = 12;
		this.browse.Text = "Browse";
		this.browse.UseVisualStyleBackColor = false;
		this.browse.Click += new System.EventHandler(browse_Click);
		this.LogoPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.LogoPic.Location = new System.Drawing.Point(644, 7);
		this.LogoPic.Name = "LogoPic";
		this.LogoPic.Size = new System.Drawing.Size(303, 149);
		this.LogoPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.LogoPic.TabIndex = 2;
		this.LogoPic.TabStop = false;
		this.txtContactPerson.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContactPerson.Location = new System.Drawing.Point(438, 7);
		this.txtContactPerson.Name = "txtContactPerson";
		this.txtContactPerson.Size = new System.Drawing.Size(200, 23);
		this.txtContactPerson.TabIndex = 7;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(347, 11);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(84, 15);
		this.label9.TabIndex = 0;
		this.label9.Text = "ContactPerson";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(4, 163);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(82, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "Other Address";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(4, 137);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(49, 15);
		this.label8.TabIndex = 0;
		this.label8.Text = "Address";
		this.txtState.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtState.Location = new System.Drawing.Point(438, 108);
		this.txtState.Name = "txtState";
		this.txtState.Size = new System.Drawing.Size(200, 23);
		this.txtState.TabIndex = 11;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(347, 112);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(34, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "State";
		this.txtCountry.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCountry.Location = new System.Drawing.Point(438, 83);
		this.txtCountry.Name = "txtCountry";
		this.txtCountry.Size = new System.Drawing.Size(200, 23);
		this.txtCountry.TabIndex = 10;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(347, 87);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(49, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Country";
		this.txtPhoneNumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPhoneNumber.Location = new System.Drawing.Point(438, 58);
		this.txtPhoneNumber.Name = "txtPhoneNumber";
		this.txtPhoneNumber.Size = new System.Drawing.Size(200, 23);
		this.txtPhoneNumber.TabIndex = 9;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(347, 62);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(85, 15);
		this.label5.TabIndex = 0;
		this.label5.Text = "PhoneNumber";
		this.txtEmail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmail.Location = new System.Drawing.Point(438, 33);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(200, 23);
		this.txtEmail.TabIndex = 8;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(347, 37);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(90, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Company Email";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 87);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(72, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Other Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 112);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "ReportingTitle";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 62);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Company Name";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 37);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Company Code";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(951, 48);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnRefresh, this.btnsave, this.btnupdate, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(951, 27);
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
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(965, 630);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmCompanyProfile";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmCompanyProfile_FormClosing);
		base.Load += new System.EventHandler(frmCompanyReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmCompanyReport_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabCompany.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrganizationName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.LogoPic).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
