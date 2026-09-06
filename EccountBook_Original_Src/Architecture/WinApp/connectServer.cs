using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Architecture.WinApp;

public class connectServer : Form
{
	public SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();

	private IContainer components;

	private Label label1;

	private Label label2;

	private TextBox txtusername;

	private TextBox txtpassword;

	private Label label3;

	private Label label4;

	private ComboBox cmbdatabase;

	private Button btnConnect;

	private TextBox txtservername;

	private ComboBox cmbauthentication;

	private Label label5;

	private Label label6;

	private TextBox txtstringName;

	private PictureBox pictureBox1;

	private Label label23;

	private Button btnLogin;

	public connectServer()
	{
		InitializeComponent();
		cmbauthentication.Items.Add("SQL Server Authentication");
		cmbauthentication.Items.Add("Windows Authentication");
		cmbauthentication.DropDownStyle = ComboBoxStyle.DropDownList;
	}

	private void connectServer_Load(object sender, EventArgs e)
	{
		txtusername.Enabled = false;
		txtpassword.Enabled = false;
		txtstringName.Focus();
	}

	private void cmbdatabase_DropDown(object sender, EventArgs e)
	{
	}

	private void btnConnect_Click(object sender, EventArgs e)
	{
		try
		{
			string name = "TPSSDb";
			if (cmbauthentication.Text == "SQL Server Authentication")
			{
				string server2 = txtservername.Text;
				string database2 = cmbdatabase.Text;
				string username = txtusername.Text;
				string password = txtpassword.Text;
				connectionString.DataSource = server2;
				connectionString.InitialCatalog = database2;
				connectionString.UserID = username;
				connectionString.Password = password;
				Modify(name, server2, database2, username, password);
				Application.Exit();
			}
			if (cmbauthentication.Text == "Windows Authentication")
			{
				string server = txtservername.Text;
				string database = cmbdatabase.Text;
				Modify(name, server, database);
				txtservername.Text = string.Empty;
				cmbdatabase.Text = string.Empty;
				Application.Exit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbauthentication_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (cmbauthentication.Text == "SQL Server Authentication")
			{
				txtusername.Enabled = true;
				txtpassword.Enabled = true;
			}
			if (cmbauthentication.Text == "Windows Authentication")
			{
				txtusername.Text = "";
				txtpassword.Text = "";
				txtusername.Enabled = false;
				txtpassword.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void updateAppConfigFile(string con)
	{
		try
		{
			XmlDocument objXmlfile = new XmlDocument();
			objXmlfile.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			foreach (XmlElement xElement in objXmlfile.DocumentElement)
			{
				if (xElement.Name == "connectionStrings")
				{
					xElement.FirstChild.Attributes[2].Value = con;
				}
			}
			objXmlfile.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Modify(string name, string server, string dataBase, string uId, string password)
	{
		try
		{
			string path = Path.GetDirectoryName(Path.GetDirectoryName(Application.StartupPath)) + "\\App.config";
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			XmlNodeList list = doc.DocumentElement.SelectNodes($"connectionStrings/add[@name='{name}']");
			bool num = list.Count == 0;
			XmlNode node;
			if (num)
			{
				node = doc.CreateNode(XmlNodeType.Element, "add", null);
				XmlAttribute attribute = doc.CreateAttribute("name");
				attribute.Value = name;
				node.Attributes.Append(attribute);
				attribute = doc.CreateAttribute("connectionString");
				attribute.Value = "";
				node.Attributes.Append(attribute);
				attribute = doc.CreateAttribute("providerName");
				attribute.Value = "System.Data.SqlClient";
				node.Attributes.Append(attribute);
			}
			else
			{
				node = list[0];
			}
			SqlConnectionStringBuilder conStringBuilder = new SqlConnectionStringBuilder(node.Attributes["connectionString"].Value)
			{
				InitialCatalog = dataBase,
				DataSource = server,
				IntegratedSecurity = false,
				UserID = uId,
				Password = password
			};
			node.Attributes["connectionString"].Value = conStringBuilder.ConnectionString;
			if (num)
			{
				doc.DocumentElement.SelectNodes("connectionStrings")[0].AppendChild(node);
				ConfigurationManager.RefreshSection("connectionStrings");
			}
			doc.Save(path);
			MessageBox.Show("Connection Created");
			if (MessageBox.Show("You want to Login?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				new LoginNew().Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Modify(string name, string server, string dataBase)
	{
		try
		{
			string path = Path.GetDirectoryName(Path.GetDirectoryName(Application.StartupPath)) + "\\App.config";
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			XmlNodeList list = doc.DocumentElement.SelectNodes($"connectionStrings/add[@name='{name}']");
			bool num = list.Count == 0;
			XmlNode node;
			if (num)
			{
				node = doc.CreateNode(XmlNodeType.Element, "add", null);
				XmlAttribute attribute = doc.CreateAttribute("name");
				attribute.Value = name;
				node.Attributes.Append(attribute);
				attribute = doc.CreateAttribute("connectionString");
				attribute.Value = "";
				node.Attributes.Append(attribute);
				attribute = doc.CreateAttribute("providerName");
				attribute.Value = "System.Data.SqlClient";
				node.Attributes.Append(attribute);
			}
			else
			{
				node = list[0];
			}
			SqlConnectionStringBuilder conStringBuilder = new SqlConnectionStringBuilder(node.Attributes["connectionString"].Value)
			{
				InitialCatalog = dataBase,
				DataSource = server,
				IntegratedSecurity = true
			};
			node.Attributes["connectionString"].Value = conStringBuilder.ConnectionString;
			if (num)
			{
				doc.DocumentElement.SelectNodes("connectionStrings")[0].AppendChild(node);
				ConfigurationManager.RefreshSection("connectionStrings");
			}
			doc.Save(path);
			MessageBox.Show("Connection Created");
			if (MessageBox.Show("You want to Login?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				new LoginNew().Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLogin_Click(object sender, EventArgs e)
	{
		try
		{
			ConfigurationManager.RefreshSection("connectionStrings");
			new LoginNew().Show();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.connectServer));
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtusername = new System.Windows.Forms.TextBox();
		this.txtpassword = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbdatabase = new System.Windows.Forms.ComboBox();
		this.btnConnect = new System.Windows.Forms.Button();
		this.txtservername = new System.Windows.Forms.TextBox();
		this.cmbauthentication = new System.Windows.Forms.ComboBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtstringName = new System.Windows.Forms.TextBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label23 = new System.Windows.Forms.Label();
		this.btnLogin = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(170, 222);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(38, 13);
		this.label1.TabIndex = 1;
		this.label1.Text = "Server";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(170, 248);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(53, 13);
		this.label2.TabIndex = 1;
		this.label2.Text = "Database";
		this.txtusername.Location = new System.Drawing.Point(257, 301);
		this.txtusername.Multiline = true;
		this.txtusername.Name = "txtusername";
		this.txtusername.Size = new System.Drawing.Size(220, 25);
		this.txtusername.TabIndex = 4;
		this.txtpassword.Location = new System.Drawing.Point(257, 330);
		this.txtpassword.Multiline = true;
		this.txtpassword.Name = "txtpassword";
		this.txtpassword.PasswordChar = '*';
		this.txtpassword.Size = new System.Drawing.Size(220, 25);
		this.txtpassword.TabIndex = 5;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(170, 306);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(55, 13);
		this.label3.TabIndex = 1;
		this.label3.Text = "Username";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(170, 330);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(53, 13);
		this.label4.TabIndex = 1;
		this.label4.Text = "Password";
		this.cmbdatabase.FormattingEnabled = true;
		this.cmbdatabase.Location = new System.Drawing.Point(257, 248);
		this.cmbdatabase.Name = "cmbdatabase";
		this.cmbdatabase.Size = new System.Drawing.Size(220, 21);
		this.cmbdatabase.TabIndex = 2;
		this.cmbdatabase.DropDown += new System.EventHandler(cmbdatabase_DropDown);
		this.btnConnect.Location = new System.Drawing.Point(330, 361);
		this.btnConnect.Name = "btnConnect";
		this.btnConnect.Size = new System.Drawing.Size(75, 23);
		this.btnConnect.TabIndex = 6;
		this.btnConnect.Text = "Connect";
		this.btnConnect.UseVisualStyleBackColor = true;
		this.btnConnect.Click += new System.EventHandler(btnConnect_Click);
		this.txtservername.Location = new System.Drawing.Point(257, 219);
		this.txtservername.Multiline = true;
		this.txtservername.Name = "txtservername";
		this.txtservername.Size = new System.Drawing.Size(220, 25);
		this.txtservername.TabIndex = 1;
		this.cmbauthentication.FormattingEnabled = true;
		this.cmbauthentication.Location = new System.Drawing.Point(257, 274);
		this.cmbauthentication.Name = "cmbauthentication";
		this.cmbauthentication.Size = new System.Drawing.Size(220, 21);
		this.cmbauthentication.TabIndex = 3;
		this.cmbauthentication.SelectedIndexChanged += new System.EventHandler(cmbauthentication_SelectedIndexChanged);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(170, 274);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 13);
		this.label5.TabIndex = 5;
		this.label5.Text = "Authentication";
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(132, 191);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(119, 13);
		this.label6.TabIndex = 8;
		this.label6.Text = "ConnectionString Name";
		this.label6.Visible = false;
		this.txtstringName.Location = new System.Drawing.Point(257, 188);
		this.txtstringName.Multiline = true;
		this.txtstringName.Name = "txtstringName";
		this.txtstringName.Size = new System.Drawing.Size(220, 25);
		this.txtstringName.TabIndex = 0;
		this.txtstringName.Visible = false;
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(496, 47);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(152, 110);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 9;
		this.pictureBox1.TabStop = false;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Transparent;
		this.label23.Font = new System.Drawing.Font("Felix Titling", 14.25f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.label23.Location = new System.Drawing.Point(204, 92);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(273, 23);
		this.label23.TabIndex = 10;
		this.label23.Text = "SqlServer Connection";
		this.btnLogin.Location = new System.Drawing.Point(411, 361);
		this.btnLogin.Name = "btnLogin";
		this.btnLogin.Size = new System.Drawing.Size(66, 23);
		this.btnLogin.TabIndex = 11;
		this.btnLogin.Text = "Login";
		this.btnLogin.UseVisualStyleBackColor = true;
		this.btnLogin.Click += new System.EventHandler(btnLogin_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(677, 450);
		base.Controls.Add(this.btnLogin);
		base.Controls.Add(this.label23);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.txtstringName);
		base.Controls.Add(this.cmbauthentication);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.btnConnect);
		base.Controls.Add(this.cmbdatabase);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.txtservername);
		base.Controls.Add(this.txtpassword);
		base.Controls.Add(this.txtusername);
		base.Name = "connectServer";
		this.Text = "connectServer";
		base.Load += new System.EventHandler(connectServer_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
