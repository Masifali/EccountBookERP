using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Templates;
using Architecture.Common;
using Architecture.Model;
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
namespace Architecture.WinApp.Templates
{

public class DefineOrgnization : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private int RecId;

	private DataTable dtHistoryGrid = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel9;

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

	internal GridEX grd;

	private Label label14;

	private Label label15;

	private Label label16;

	private TextBox txtEmail02;

	private Label label19;

	private Label label18;

	private Label label17;

	private TextBox txtMobile03;

	private TextBox txtMobile02;

	private TextBox txtMobile01;

	private Label label7;

	private TextBox txtReportingRemarks;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnShortCutKey;

	private TextBox txtCompanyCount;

	private Label label10;

	internal UltraCombo CmbTemplate;

	private Label label11;

	public DefineOrgnization(UserAccount userAccount)
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
			base.AutoScrollMinSize = new Size(740, 550);
			HistoryGridFill();
			TemplateNameFill();
			((Control)(object)CmbTemplate).Enabled = true;
			btnsave.Visible = true;
			btnupdate.Visible = false;
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
			DataTable dt = OrganizationTemplates.GetAllInTemplateRights();
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

	private void Reset()
	{
		try
		{
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
			txtReportingRemarks.Text = string.Empty;
			HistoryGridFill();
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
			TemplateNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtCode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Organization Code Field Required");
			txtCode.Focus();
			return false;
		}
		if (txtName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Organization Name Field Required");
			txtName.Focus();
			return false;
		}
		if (txtReportingTitle.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Reporting Title Field Required");
			txtReportingTitle.Focus();
			return false;
		}
		if (txtContactPerson.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Contact Person Field Required");
			txtContactPerson.Focus();
			return false;
		}
		if (txtEmail.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Email Field Required");
			txtEmail.Focus();
			return false;
		}
		if (txtEmail02.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Email II Field Required");
			txtEmail02.Focus();
			return false;
		}
		if (txtAdress.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Address Field Required");
			txtAdress.Focus();
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
		if (txtMobile02.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Mobile 02 Field Required");
			txtMobile02.Focus();
			return false;
		}
		if (txtMobile03.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Mobile 03 Field Required");
			txtMobile03.Focus();
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
		if (txtReportingRemarks.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Reporting Remarks Field Required");
			txtReportingRemarks.Focus();
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
			Organization obj = new Organization();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.OrgCode = txtCode.Text;
			obj.OrgName = txtName.Text;
			obj.OrgReportingTitle = txtReportingTitle.Text;
			obj.OrgContactPerson = txtContactPerson.Text;
			obj.OrgEmailA = txtEmail.Text;
			obj.OrgEmailB = txtEmail02.Text;
			obj.OrgAddress = txtAdress.Text;
			obj.ReportingRemarks = txtReportingRemarks.Text;
			obj.OrgTel = txtPhoneNumber.Text;
			obj.OrgMobileA = txtMobile01.Text;
			obj.OrgMobileB = txtMobile02.Text;
			obj.OrgMobileC = txtMobile03.Text;
			obj.OrgCountry = txtCountry.Text;
			obj.OrgState = txtState.Text;
			obj.AllowedLocationsCount = Conversion.ToInt((object)txtCompanyCount.Text);
			obj.OrganizaionTemplateId = Conversion.ToInt(CmbTemplate.Value);
			Organization.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			RecId = Id;
			Organization obj = Organization.GetByID(RecId);
			if (obj != null)
			{
				RecId = obj.Id;
				txtCode.Text = Conversion.ToString((object)obj.OrgCode);
				txtName.Text = Conversion.ToString((object)obj.OrgName);
				txtReportingTitle.Text = Conversion.ToString((object)obj.OrgReportingTitle);
				txtContactPerson.Text = Conversion.ToString((object)obj.OrgContactPerson);
				txtEmail.Text = Conversion.ToString((object)obj.OrgEmailA);
				txtEmail02.Text = Conversion.ToString((object)obj.OrgEmailB);
				txtAdress.Text = Conversion.ToString((object)obj.OrgAddress);
				txtReportingRemarks.Text = Conversion.ToString((object)obj.ReportingRemarks);
				txtPhoneNumber.Text = Conversion.ToString((object)obj.OrgTel);
				txtMobile01.Text = Conversion.ToString((object)obj.OrgMobileA);
				txtMobile02.Text = Conversion.ToString((object)obj.OrgMobileB);
				txtMobile03.Text = Conversion.ToString((object)obj.OrgMobileC);
				txtCountry.Text = Conversion.ToString((object)obj.OrgCountry);
				txtState.Text = Conversion.ToString((object)obj.OrgState);
				txtCompanyCount.Text = Conversion.ToString((object)obj.AllowedLocationsCount);
				CmbTemplate.Value = obj.OrganizaionTemplateId;
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

	private void HistoryGridFill()
	{
		try
		{
			dtHistoryGrid.Clear();
			dtHistoryGrid = Organization.GetAll();
			if (dtHistoryGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("OrgCode", typeof(string));
				dt.Columns.Add("OrgName", typeof(string));
				dt.Columns.Add("OrgContactPerson", typeof(string));
				dt.Columns.Add("OrgEmailA", typeof(string));
				dt.Columns.Add("OrgEmailB", typeof(string));
				dt.Columns.Add("OrgTel", typeof(string));
				dt.Columns.Add("OrgMobileA", typeof(string));
				dt.Columns.Add("OrgMobileB", typeof(string));
				dt.Columns.Add("OrgMobileC", typeof(string));
				dt.Columns.Add("OrgAddress", typeof(string));
				dt.Columns.Add("OrgReportingTitle", typeof(string));
				dt.Columns.Add("OrgCountry", typeof(string));
				dt.Columns.Add("OrgState", typeof(string));
				dt.Columns.Add("OrgBaseCurr", typeof(string));
				dt.Columns.Add("AllowedLocationsCount", typeof(int));
				dt.Columns.Add("ReportingRemarks", typeof(string));
				foreach (DataRow row in dtHistoryGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["OrgCode"], row["OrgName"], row["OrgContactPerson"], row["OrgEmailA"], row["OrgEmailB"], row["OrgTel"], row["OrgMobileA"], row["OrgMobileB"], row["OrgMobileC"], row["OrgAddress"], row["OrgReportingTitle"], row["OrgCountry"], row["OrgState"], row["OrgBaseCurr"], row["AllowedLocationsCount"], row["ReportingRemarks"]);
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
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Templates.DefineOrgnization));
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtName = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtReportingTitle = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtContactPerson = new System.Windows.Forms.TextBox();
		this.txtEmail02 = new System.Windows.Forms.TextBox();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtAdress = new System.Windows.Forms.TextBox();
		this.txtMobile03 = new System.Windows.Forms.TextBox();
		this.txtMobile02 = new System.Windows.Forms.TextBox();
		this.txtMobile01 = new System.Windows.Forms.TextBox();
		this.txtPhoneNumber = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtCompanyCount = new System.Windows.Forms.TextBox();
		this.txtState = new System.Windows.Forms.TextBox();
		this.txtReportingRemarks = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtCountry = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.CmbTemplate = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplate).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(724, 511);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(724, 511);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel9);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(716, 482);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Define Company";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 301);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(710, 178);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label14);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 274);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(710, 27);
		this.panel9.TabIndex = 2;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(5, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(63, 18);
		this.label14.TabIndex = 0;
		this.label14.Text = "History";
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTemplate);
		this.panel2.Controls.Add(this.label11);
		this.panel2.Controls.Add(this.txtCode);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.txtName);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.txtReportingTitle);
		this.panel2.Controls.Add(this.label16);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.txtContactPerson);
		this.panel2.Controls.Add(this.txtEmail02);
		this.panel2.Controls.Add(this.txtEmail);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.label19);
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.label17);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.txtAdress);
		this.panel2.Controls.Add(this.txtMobile03);
		this.panel2.Controls.Add(this.txtMobile02);
		this.panel2.Controls.Add(this.txtMobile01);
		this.panel2.Controls.Add(this.txtPhoneNumber);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.txtCompanyCount);
		this.panel2.Controls.Add(this.txtState);
		this.panel2.Controls.Add(this.txtReportingRemarks);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.txtCountry);
		this.panel2.Controls.Add(this.label12);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 57);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(710, 217);
		this.panel2.TabIndex = 0;
		this.txtCode.Location = new System.Drawing.Point(127, 6);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(279, 23);
		this.txtCode.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(106, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Organization Code";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 36);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(111, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Organization Name";
		this.txtName.Location = new System.Drawing.Point(127, 32);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(279, 23);
		this.txtName.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(8, 62);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "ReportingTitle";
		this.txtReportingTitle.Location = new System.Drawing.Point(127, 58);
		this.txtReportingTitle.Name = "txtReportingTitle";
		this.txtReportingTitle.Size = new System.Drawing.Size(279, 23);
		this.txtReportingTitle.TabIndex = 2;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(8, 140);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(119, 15);
		this.label16.TabIndex = 0;
		this.label16.Text = "Organization Email II";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 114);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(108, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Organization Email";
		this.txtContactPerson.Location = new System.Drawing.Point(127, 84);
		this.txtContactPerson.Name = "txtContactPerson";
		this.txtContactPerson.Size = new System.Drawing.Size(279, 23);
		this.txtContactPerson.TabIndex = 3;
		this.txtEmail02.Location = new System.Drawing.Point(127, 136);
		this.txtEmail02.Name = "txtEmail02";
		this.txtEmail02.Size = new System.Drawing.Size(279, 23);
		this.txtEmail02.TabIndex = 5;
		this.txtEmail.Location = new System.Drawing.Point(127, 110);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(279, 23);
		this.txtEmail.TabIndex = 4;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(8, 88);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(87, 15);
		this.label9.TabIndex = 0;
		this.label9.Text = "Contact Person";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(409, 88);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(61, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Mobile 03";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(409, 62);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(61, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Mobile 02";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(409, 36);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(59, 15);
		this.label17.TabIndex = 0;
		this.label17.Text = "Mobile 01";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(409, 10);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(85, 15);
		this.label5.TabIndex = 0;
		this.label5.Text = "PhoneNumber";
		this.txtAdress.Location = new System.Drawing.Point(127, 162);
		this.txtAdress.Name = "txtAdress";
		this.txtAdress.Size = new System.Drawing.Size(279, 23);
		this.txtAdress.TabIndex = 6;
		this.txtMobile03.Location = new System.Drawing.Point(499, 84);
		this.txtMobile03.Name = "txtMobile03";
		this.txtMobile03.Size = new System.Drawing.Size(208, 23);
		this.txtMobile03.TabIndex = 11;
		this.txtMobile02.Location = new System.Drawing.Point(499, 58);
		this.txtMobile02.Name = "txtMobile02";
		this.txtMobile02.Size = new System.Drawing.Size(208, 23);
		this.txtMobile02.TabIndex = 10;
		this.txtMobile01.Location = new System.Drawing.Point(499, 32);
		this.txtMobile01.Name = "txtMobile01";
		this.txtMobile01.Size = new System.Drawing.Size(208, 23);
		this.txtMobile01.TabIndex = 9;
		this.txtPhoneNumber.Location = new System.Drawing.Point(499, 6);
		this.txtPhoneNumber.Name = "txtPhoneNumber";
		this.txtPhoneNumber.Size = new System.Drawing.Size(208, 23);
		this.txtPhoneNumber.TabIndex = 8;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(8, 166);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(49, 15);
		this.label8.TabIndex = 0;
		this.label8.Text = "Address";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(8, 192);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(107, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Reporting Remarks";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(409, 114);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(49, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Country";
		this.txtCompanyCount.Location = new System.Drawing.Point(499, 162);
		this.txtCompanyCount.Name = "txtCompanyCount";
		this.txtCompanyCount.Size = new System.Drawing.Size(208, 23);
		this.txtCompanyCount.TabIndex = 14;
		this.txtCompanyCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCompanyCount_KeyPress);
		this.txtState.Location = new System.Drawing.Point(499, 136);
		this.txtState.Name = "txtState";
		this.txtState.Size = new System.Drawing.Size(208, 23);
		this.txtState.TabIndex = 13;
		this.txtReportingRemarks.Location = new System.Drawing.Point(127, 188);
		this.txtReportingRemarks.Name = "txtReportingRemarks";
		this.txtReportingRemarks.Size = new System.Drawing.Size(279, 23);
		this.txtReportingRemarks.TabIndex = 7;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(409, 166);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(93, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "Company Count";
		this.txtCountry.Location = new System.Drawing.Point(499, 110);
		this.txtCountry.Name = "txtCountry";
		this.txtCountry.Size = new System.Drawing.Size(208, 23);
		this.txtCountry.TabIndex = 12;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(409, 140);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(34, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "State";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(710, 54);
		this.panel3.TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 30);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(114, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Organization";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.toolStripSeparator3, this.btnnew, this.btnsave, this.btnRefresh, this.toolStripSeparator2, this.btnupdate, this.toolStripSeparator4, this.btnPrint, this.toolStripSeparator1, this.btnShortCutKey });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(710, 27);
		this.toolStrip1.TabIndex = 10;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(63, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(76, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(57, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
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
		this.CmbTemplate.AllowNull = (DefaultableBoolean)1;
		this.CmbTemplate.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTemplate.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTemplate.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Enabled = false;
		this.CmbTemplate.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Location = new System.Drawing.Point(499, 188);
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Name = "CmbTemplate";
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbTemplate).TabIndex = 15;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(409, 194);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(56, 15);
		this.label11.TabIndex = 206;
		this.label11.Text = "Template";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(724, 511);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineOrgnization";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmCompanyReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmCompanyReport_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplate).EndInit();
		base.ResumeLayout(false);
	}
}

}
