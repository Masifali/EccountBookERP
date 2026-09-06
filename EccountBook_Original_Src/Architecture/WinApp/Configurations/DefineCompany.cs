using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.OrganizationRegistration;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.OrganizationRegistration;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Configurations;

public class DefineCompany : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private int RecId;

	private DataTable dtgrd = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel9;

	internal GridEX grd;

	private Panel panel4;

	private Label label13;

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

	private TextBox txtCity;

	private Label label7;

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
			DataTable dt = Company.GetAllCompanies();
			if (dt.Rows.Count > 0)
			{
				grd.DataSource = dt;
				grd.RetrieveStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
	}

	private void Insert()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			OrganizationRegistration obj = new OrganizationRegistration();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.Code = txtCode.Text;
			obj.Name = txtName.Text;
			obj.ReportingTitle = txtReportingTitle.Text;
			obj.Email = txtEmail.Text;
			obj.PhoneNumber = txtPhoneNumber.Text;
			obj.Country = txtCountry.Text;
			obj.City = txtCity.Text;
			obj.Address = txtAdress.Text;
			obj.ContactPerson = txtContactPerson.Text;
			obj.FirstName = txtFirstName.Text;
			obj.LastName = txtLastName.Text;
			obj.NoOfLocations = 1;
			obj.State = txtState.Text;
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
			OrganizationRegistration.save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
				Reset();
			}
			else
			{
				MessageBox.Show("Save Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void RetrivedData(int Id)
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

	private void frmCompanyReport_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnupdate.Visible && btnupdate.Enabled && tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void label8_Click(object sender, EventArgs e)
	{
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
					LogoPic.Load(fileName);
				}
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
				txtCode.Text = item.Cells["CompanyCode"].Value.ToString();
				txtName.Text = item.Cells["CompanyName"].Value.ToString();
				txtReportingTitle.Text = item.Cells["ReportingTitle"].Value.ToString();
				txtEmail.Text = item.Cells["Email"].Value.ToString();
				txtPhoneNumber.Text = item.Cells["Phone"].Value.ToString();
				txtCountry.Text = item.Cells["Country"].Value.ToString();
				txtState.Text = item.Cells["State"].Value.ToString();
				txtCity.Text = item.Cells["City"].Value.ToString();
				txtAdress.Text = item.Cells["Address"].Value.ToString();
				txtContactPerson.Text = item.Cells["ContactPerson"].Value.ToString();
				txtFirstName.Text = item.Cells["FirstName"].Value.ToString();
				txtLastName.Text = item.Cells["LastName"].Value.ToString();
				LogoPic.Image = (Bitmap)item.Cells["LogoImage"].Value;
				btnsave.Visible = false;
				btnupdate.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefineCompany_FormClosing(object sender, FormClosingEventArgs e)
	{
		HistoryStack.Remove(this);
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Configurations.DefineCompany));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.browse = new System.Windows.Forms.Button();
		this.LogoPic = new System.Windows.Forms.PictureBox();
		this.txtLastName = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtFirstName = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtContactPerson = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtAdress = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtState = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtCity = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtCountry = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtPhoneNumber = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtReportingTitle = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.LogoPic).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(899, 568);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(899, 568);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(891, 542);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Define Company";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(885, 536);
		this.panel2.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 260);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(885, 276);
		this.panel9.TabIndex = 2;
		this.grd.AllowDelete = (InheritableBoolean)1;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(885, 276);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 48;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.browse);
		this.panel4.Controls.Add(this.LogoPic);
		this.panel4.Controls.Add(this.txtLastName);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtFirstName);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.txtContactPerson);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.txtAdress);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtState);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtCity);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtCountry);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtPhoneNumber);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtEmail);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.txtReportingTitle);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtName);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtCode);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(885, 230);
		this.panel4.TabIndex = 1;
		this.label13.AutoSize = true;
		this.label13.Location = new System.Drawing.Point(305, 133);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(78, 13);
		this.label13.TabIndex = 200;
		this.label13.Text = "Company Logo";
		this.browse.Location = new System.Drawing.Point(464, 198);
		this.browse.Name = "browse";
		this.browse.Size = new System.Drawing.Size(56, 23);
		this.browse.TabIndex = 199;
		this.browse.Text = "Browse";
		this.browse.UseVisualStyleBackColor = true;
		this.browse.Click += new System.EventHandler(browse_Click);
		this.LogoPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.LogoPic.Location = new System.Drawing.Point(393, 84);
		this.LogoPic.Name = "LogoPic";
		this.LogoPic.Size = new System.Drawing.Size(200, 112);
		this.LogoPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.LogoPic.TabIndex = 2;
		this.LogoPic.TabStop = false;
		this.txtLastName.Location = new System.Drawing.Point(393, 61);
		this.txtLastName.Name = "txtLastName";
		this.txtLastName.Size = new System.Drawing.Size(200, 20);
		this.txtLastName.TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(308, 65);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(55, 13);
		this.label11.TabIndex = 0;
		this.label11.Text = "LastName";
		this.label11.Click += new System.EventHandler(label8_Click);
		this.txtFirstName.Location = new System.Drawing.Point(393, 38);
		this.txtFirstName.Name = "txtFirstName";
		this.txtFirstName.Size = new System.Drawing.Size(200, 20);
		this.txtFirstName.TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(308, 42);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(54, 13);
		this.label10.TabIndex = 0;
		this.label10.Text = "FirstName";
		this.label10.Click += new System.EventHandler(label8_Click);
		this.txtContactPerson.Location = new System.Drawing.Point(393, 15);
		this.txtContactPerson.Name = "txtContactPerson";
		this.txtContactPerson.Size = new System.Drawing.Size(200, 20);
		this.txtContactPerson.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(308, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(77, 13);
		this.label9.TabIndex = 0;
		this.label9.Text = "ContactPerson";
		this.label9.Click += new System.EventHandler(label8_Click);
		this.txtAdress.Location = new System.Drawing.Point(94, 199);
		this.txtAdress.Name = "txtAdress";
		this.txtAdress.Size = new System.Drawing.Size(200, 20);
		this.txtAdress.TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(9, 203);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(45, 13);
		this.label8.TabIndex = 0;
		this.label8.Text = "Address";
		this.label8.Click += new System.EventHandler(label8_Click);
		this.txtState.Location = new System.Drawing.Point(94, 153);
		this.txtState.Name = "txtState";
		this.txtState.Size = new System.Drawing.Size(200, 20);
		this.txtState.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(9, 157);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(32, 13);
		this.label12.TabIndex = 0;
		this.label12.Text = "State";
		this.txtCity.Location = new System.Drawing.Point(94, 176);
		this.txtCity.Name = "txtCity";
		this.txtCity.Size = new System.Drawing.Size(200, 20);
		this.txtCity.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(9, 180);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(24, 13);
		this.label7.TabIndex = 0;
		this.label7.Text = "City";
		this.txtCountry.Location = new System.Drawing.Point(94, 130);
		this.txtCountry.Name = "txtCountry";
		this.txtCountry.Size = new System.Drawing.Size(200, 20);
		this.txtCountry.TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(9, 134);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(43, 13);
		this.label6.TabIndex = 0;
		this.label6.Text = "Country";
		this.txtPhoneNumber.Location = new System.Drawing.Point(94, 107);
		this.txtPhoneNumber.Name = "txtPhoneNumber";
		this.txtPhoneNumber.Size = new System.Drawing.Size(200, 20);
		this.txtPhoneNumber.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(9, 111);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 13);
		this.label5.TabIndex = 0;
		this.label5.Text = "PhoneNumber";
		this.txtEmail.Location = new System.Drawing.Point(94, 84);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(200, 20);
		this.txtEmail.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(9, 88);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 13);
		this.label4.TabIndex = 0;
		this.label4.Text = "Company Email";
		this.txtReportingTitle.Location = new System.Drawing.Point(94, 61);
		this.txtReportingTitle.Name = "txtReportingTitle";
		this.txtReportingTitle.Size = new System.Drawing.Size(200, 20);
		this.txtReportingTitle.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(9, 65);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(73, 13);
		this.label3.TabIndex = 0;
		this.label3.Text = "ReportingTitle";
		this.txtName.Location = new System.Drawing.Point(94, 38);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(200, 20);
		this.txtName.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(9, 42);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(82, 13);
		this.label2.TabIndex = 0;
		this.label2.Text = "Company Name";
		this.txtCode.Location = new System.Drawing.Point(94, 15);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(200, 20);
		this.txtCode.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(9, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(79, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "Company Code";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(885, 30);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnsave, this.btnupdate, this.btnPrint, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(885, 27);
		this.toolStrip1.TabIndex = 10;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
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
		this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(57, 24);
		this.btnPrint.Text = "&Print";
		this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(58, 24);
		this.btnRefresh.Text = "Refresh";
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(899, 568);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineCompany";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DefineCompany_FormClosing);
		base.Load += new System.EventHandler(frmCompanyReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmCompanyReport_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.LogoPic).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
