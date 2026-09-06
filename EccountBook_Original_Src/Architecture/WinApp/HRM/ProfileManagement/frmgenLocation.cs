using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.ProfileManagement;

public class frmgenLocation : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnnew;

	private ToolStrip tsItemType;

	private Panel panel1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel2;

	internal GridEX grd;

	private Panel panel11;

	internal UltraCombo cmbLocationType;

	private Label label7;

	private TextBox txtLocationName;

	private Label label49;

	private TextBox txtShortName;

	private Label label1;

	private TextBox txtSubLocationName;

	private Label label2;

	private TextBox txtEmail;

	private Label label5;

	private TextBox txtMobile2;

	private Label label10;

	private TextBox txtMobile1;

	private Label label11;

	private TextBox txtPortalURL;

	private Label label13;

	private TextBox txtLicenseNo;

	private Label label14;

	private TextBox txtAddressDetail;

	private Label label4;

	private Panel panel3;

	private Label label3;

	private Panel panel5;

	private Panel panel4;

	private Label label6;

	private ToolStripButton btnrefresh;

	private Button btnUpdateF;

	private Button BtnCancel;

	private Button BtnSavef;

	public frmgenLocation(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void Reset()
	{
		try
		{
			txtLocationName.Text = string.Empty;
			txtShortName.Text = string.Empty;
			txtSubLocationName.Text = string.Empty;
			txtEmail.Text = string.Empty;
			((Control)(object)cmbLocationType).Text = string.Empty;
			txtMobile1.Text = string.Empty;
			txtMobile2.Text = string.Empty;
			txtPortalURL.Text = string.Empty;
			txtLicenseNo.Text = string.Empty;
			txtAddressDetail.Text = string.Empty;
			txtLocationName.Focus();
			RecId = 0;
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			UpdateMode = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			DataTable dtLocation = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsActive = true,
				ProfileTypeId = 61
			});
			if (dtLocation.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtLocation, cmbLocationType, "ProfileId", "ProfileName", "Location Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formValidation()
	{
		if (txtLocationName.Text == string.Empty || txtLocationName.Text == null || txtLocationName.Text == "0")
		{
			MessageBox.Show("Location Name Required");
			txtLocationName.Focus();
			return false;
		}
		if (((Control)(object)cmbLocationType).Text.Trim() == string.Empty || ((Control)(object)cmbLocationType).Text == null || Conversion.ToInt(cmbLocationType.Value) == 0)
		{
			MessageBox.Show("Location Type");
			cmbLocationType.Focus();
			return false;
		}
		return true;
	}

	private void gridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtGetAll = genLocation.Getall(new genLocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("LocationType");
			dt.Columns.Add("LocationName");
			dt.Columns.Add("ShortName");
			dt.Columns.Add("SubName");
			dt.Columns.Add("Mobile1");
			dt.Columns.Add("Mobile2");
			dt.Columns.Add("Email");
			dt.Columns.Add("PortalURL");
			dt.Columns.Add("AddressDetail");
			dt.Columns.Add("LicenseNo");
			if (dtGetAll.Rows.Count > 0)
			{
				for (int i = 0; i < dtGetAll.Rows.Count; i++)
				{
					dt.Rows.Add(dtGetAll.Rows[i]["LocationId"], dtGetAll.Rows[i]["LocationType"], dtGetAll.Rows[i]["LocationName"], dtGetAll.Rows[i]["LocationShortName"], dtGetAll.Rows[i]["SubLocationName"], dtGetAll.Rows[i]["Mobile1"], dtGetAll.Rows[i]["Mobile2"], dtGetAll.Rows[i]["Email"], dtGetAll.Rows[i]["PortalURL"], dtGetAll.Rows[i]["HeaderDetail"], dtGetAll.Rows[i]["LicenseNo"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RetrivedData()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			UpdateMode = true;
			genLocation obj = new genLocation();
			RecId = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
			obj = genLocation.GetByID(RecId);
			txtLocationName.Text = Conversion.ToString((object)obj.LocationName);
			txtShortName.Text = Conversion.ToString((object)obj.LocationShortName);
			txtSubLocationName.Text = Conversion.ToString((object)obj.SubLocationName);
			txtEmail.Text = Conversion.ToString((object)obj.Email);
			cmbLocationType.Value = Conversion.ToInt((object)obj.LocationTypeProfileId);
			txtMobile1.Text = Conversion.ToString((object)obj.Mobile1);
			txtMobile2.Text = Conversion.ToString((object)obj.Mobile2);
			txtPortalURL.Text = Conversion.ToString((object)obj.PortalURL);
			txtLicenseNo.Text = Conversion.ToString((object)obj.LicenseNo);
			txtAddressDetail.Text = Conversion.ToString((object)obj.HeaderDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LeaveQuotaPolicy_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(980, 500);
		LocationTypeFill();
		gridFill();
		BtnSavef.Visible = true;
		btnUpdateF.Visible = false;
		btnsave.Visible = true;
		btnupdate.Visible = false;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		try
		{
			if (formValidation() && MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No && genLocation.Save(new genLocation
			{
				OrganizationId = Conversion.ToInt((object)UserAccount.OrganizationId),
				CompanyId = Conversion.ToInt((object)UserAccount.CompanyId),
				CreatedOn = Conversion.ToDateTime((object)DateTime.Now),
				CreatedById = Conversion.ToInt((object)UserAccount.ID),
				AlteredById = Conversion.ToInt((object)UserAccount.ID),
				AlteredOn = Conversion.ToDateTime((object)DateTime.Today),
				LocationName = Conversion.ToString((object)txtLocationName.Text.Trim()),
				LocationShortName = Conversion.ToString((object)txtShortName.Text.Trim()),
				SubLocationName = Conversion.ToString((object)txtSubLocationName.Text.Trim()),
				Email = Conversion.ToString((object)txtEmail.Text.Trim()),
				Mobile1 = Conversion.ToString((object)txtMobile1.Text.Trim()),
				Mobile2 = Conversion.ToString((object)txtMobile2.Text.Trim()),
				PortalURL = Conversion.ToString((object)txtPortalURL.Text.Trim()),
				LicenseNo = Conversion.ToString((object)txtLicenseNo.Text.Trim()),
				HeaderDetail = Conversion.ToString((object)txtAddressDetail.Text.Trim()),
				LocationTypeProfileId = Conversion.ToInt(cmbLocationType.Value)
			}) > 0)
			{
				MessageBox.Show("Save Successfully");
				Reset();
				gridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		try
		{
			if (formValidation() && MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No && genLocation.Save(new genLocation
			{
				LocationId = RecId,
				OrganizationId = Conversion.ToInt((object)UserAccount.OrganizationId),
				CompanyId = Conversion.ToInt((object)UserAccount.CompanyId),
				CreatedOn = Conversion.ToDateTime((object)DateTime.Now),
				CreatedById = Conversion.ToInt((object)UserAccount.ID),
				AlteredById = Conversion.ToInt((object)UserAccount.ID),
				AlteredOn = Conversion.ToDateTime((object)DateTime.Today),
				LocationName = Conversion.ToString((object)txtLocationName.Text.Trim()),
				LocationShortName = Conversion.ToString((object)txtShortName.Text.Trim()),
				SubLocationName = Conversion.ToString((object)txtSubLocationName.Text.Trim()),
				Email = Conversion.ToString((object)txtEmail.Text.Trim()),
				Mobile1 = Conversion.ToString((object)txtMobile1.Text.Trim()),
				Mobile2 = Conversion.ToString((object)txtMobile2.Text.Trim()),
				PortalURL = Conversion.ToString((object)txtPortalURL.Text.Trim()),
				LicenseNo = Conversion.ToString((object)txtLicenseNo.Text.Trim()),
				HeaderDetail = Conversion.ToString((object)txtAddressDetail.Text.Trim()),
				LocationTypeProfileId = Conversion.ToInt(cmbLocationType.Value)
			}) > 0)
			{
				MessageBox.Show("Update Successfully");
				Reset();
				gridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LeaveQuotaPolicy_KeyDown(object sender, KeyEventArgs e)
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

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RetrivedData();
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

	private void btnrefresh_Click(object sender, EventArgs e)
	{
		LocationTypeFill();
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		Close();
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
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.ProfileManagement.frmgenLocation));
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnrefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.txtAddressDetail = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtLicenseNo = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtPortalURL = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtMobile2 = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtMobile1 = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtSubLocationName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtShortName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtLocationName = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.tsItemType.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		base.SuspendLayout();
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnupdate, this.btnsave, this.btnrefresh });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(964, 27);
		this.tsItemType.TabIndex = 1;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnrefresh.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnrefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefresh.Name = "btnrefresh";
		this.btnrefresh.Size = new System.Drawing.Size(81, 24);
		this.btnrefresh.Text = "Refresh";
		this.btnrefresh.Click += new System.EventHandler(btnrefresh_Click);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel11);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 634);
		this.panel1.TabIndex = 7;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 181);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 453);
		this.panel2.TabIndex = 10;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 29);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(964, 424);
		this.panel5.TabIndex = 129;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(964, 424);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 29);
		this.panel4.TabIndex = 128;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(915, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(4, 6);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 19);
		this.label6.TabIndex = 128;
		this.label6.Text = "History";
		this.panel11.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.panel11.Controls.Add(this.btnUpdateF);
		this.panel11.Controls.Add(this.BtnCancel);
		this.panel11.Controls.Add(this.BtnSavef);
		this.panel11.Controls.Add(this.panel3);
		this.panel11.Controls.Add(this.txtAddressDetail);
		this.panel11.Controls.Add(this.label4);
		this.panel11.Controls.Add(this.txtLicenseNo);
		this.panel11.Controls.Add(this.label14);
		this.panel11.Controls.Add(this.txtPortalURL);
		this.panel11.Controls.Add(this.label13);
		this.panel11.Controls.Add(this.txtMobile2);
		this.panel11.Controls.Add(this.label10);
		this.panel11.Controls.Add(this.txtMobile1);
		this.panel11.Controls.Add(this.label11);
		this.panel11.Controls.Add(this.txtEmail);
		this.panel11.Controls.Add(this.label5);
		this.panel11.Controls.Add(this.txtSubLocationName);
		this.panel11.Controls.Add(this.label2);
		this.panel11.Controls.Add(this.txtShortName);
		this.panel11.Controls.Add(this.label1);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel11.Controls.Add(this.label7);
		this.panel11.Controls.Add(this.txtLocationName);
		this.panel11.Controls.Add(this.label49);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(4);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(964, 181);
		this.panel11.TabIndex = 0;
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(876, 143);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 10;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.btnUpdateF.Click += new System.EventHandler(btnupdate_Click);
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(797, 143);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 11;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(876, 143);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 10;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.BtnSavef.Click += new System.EventHandler(BtnCancel_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 29);
		this.panel3.TabIndex = 127;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 6);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(130, 19);
		this.label3.TabIndex = 128;
		this.label3.Text = "Location Name";
		this.txtAddressDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddressDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddressDetail.Location = new System.Drawing.Point(703, 61);
		this.txtAddressDetail.Margin = new System.Windows.Forms.Padding(4);
		this.txtAddressDetail.Multiline = true;
		this.txtAddressDetail.Name = "txtAddressDetail";
		this.txtAddressDetail.Size = new System.Drawing.Size(248, 75);
		this.txtAddressDetail.TabIndex = 9;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(620, 65);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(83, 15);
		this.label4.TabIndex = 126;
		this.label4.Text = "Address Detail";
		this.txtLicenseNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLicenseNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLicenseNo.Location = new System.Drawing.Point(703, 34);
		this.txtLicenseNo.Margin = new System.Windows.Forms.Padding(4);
		this.txtLicenseNo.Name = "txtLicenseNo";
		this.txtLicenseNo.Size = new System.Drawing.Size(248, 23);
		this.txtLicenseNo.TabIndex = 8;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(620, 38);
		this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(68, 15);
		this.label14.TabIndex = 120;
		this.label14.Text = "License No:";
		this.txtPortalURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPortalURL.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPortalURL.Location = new System.Drawing.Point(406, 113);
		this.txtPortalURL.Margin = new System.Windows.Forms.Padding(4);
		this.txtPortalURL.Name = "txtPortalURL";
		this.txtPortalURL.Size = new System.Drawing.Size(208, 23);
		this.txtPortalURL.TabIndex = 7;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(325, 117);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(65, 15);
		this.label13.TabIndex = 118;
		this.label13.Text = "Portal URL:";
		this.txtMobile2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMobile2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtMobile2.Location = new System.Drawing.Point(406, 87);
		this.txtMobile2.Margin = new System.Windows.Forms.Padding(4);
		this.txtMobile2.Name = "txtMobile2";
		this.txtMobile2.Size = new System.Drawing.Size(208, 23);
		this.txtMobile2.TabIndex = 6;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(325, 91);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(57, 15);
		this.label10.TabIndex = 114;
		this.label10.Text = "Mobile 2:";
		this.txtMobile1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMobile1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtMobile1.Location = new System.Drawing.Point(406, 61);
		this.txtMobile1.Margin = new System.Windows.Forms.Padding(4);
		this.txtMobile1.Name = "txtMobile1";
		this.txtMobile1.Size = new System.Drawing.Size(208, 23);
		this.txtMobile1.TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(325, 65);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(55, 15);
		this.label11.TabIndex = 112;
		this.label11.Text = "Mobile 1:";
		this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEmail.Location = new System.Drawing.Point(138, 113);
		this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(180, 23);
		this.txtEmail.TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(11, 117);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(85, 15);
		this.label5.TabIndex = 104;
		this.label5.Text = "Location Email";
		this.txtSubLocationName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSubLocationName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSubLocationName.Location = new System.Drawing.Point(138, 87);
		this.txtSubLocationName.Margin = new System.Windows.Forms.Padding(4);
		this.txtSubLocationName.Name = "txtSubLocationName";
		this.txtSubLocationName.Size = new System.Drawing.Size(180, 23);
		this.txtSubLocationName.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(11, 91);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(112, 15);
		this.label2.TabIndex = 102;
		this.label2.Text = "Sub Location Name";
		this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShortName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtShortName.Location = new System.Drawing.Point(138, 61);
		this.txtShortName.Margin = new System.Windows.Forms.Padding(4);
		this.txtShortName.Name = "txtShortName";
		this.txtShortName.Size = new System.Drawing.Size(180, 23);
		this.txtShortName.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(11, 65);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(120, 15);
		this.label1.TabIndex = 100;
		this.label1.Text = "Location Short Name";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(406, 32);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(325, 38);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(81, 15);
		this.label7.TabIndex = 97;
		this.label7.Text = "Location Type";
		this.txtLocationName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocationName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLocationName.Location = new System.Drawing.Point(138, 34);
		this.txtLocationName.Margin = new System.Windows.Forms.Padding(4);
		this.txtLocationName.Name = "txtLocationName";
		this.txtLocationName.Size = new System.Drawing.Size(180, 23);
		this.txtLocationName.TabIndex = 0;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(11, 38);
		this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(88, 15);
		this.label49.TabIndex = 1;
		this.label49.Text = "Location Name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tsItemType);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmgenLocation";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LeaveQuotaPolicy_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LeaveQuotaPolicy_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
