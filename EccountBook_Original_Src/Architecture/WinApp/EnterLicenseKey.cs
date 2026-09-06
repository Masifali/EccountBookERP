using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;

namespace Architecture.WinApp;

public class EnterLicenseKey : Form
{
	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Button btnSubmit;

	private Button btnContinue;

	private Panel panel1;

	private Panel panel2;

	private Label label2;

	private Label label1;

	private Label lblOr;

	private Label LblDayLeftMsg;

	private PictureBox PicLicanceKey;

	private Label label3;

	private Label label5;

	private Panel panel4;

	private RichTextBox txtLicense;

	private Label lblMessage;

	public EnterLicenseKey(UserAccount pUserAccount, string Message, bool License)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		lblMessage.Text = Message;
		if (!License)
		{
			lblOr.Visible = false;
			btnContinue.Visible = false;
		}
		else
		{
			lblOr.Visible = true;
			btnContinue.Visible = true;
		}
	}

	private void btnSubmit_Click(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		try
		{
			if (txtLicense.Text == string.Empty)
			{
				MessageBox.Show("Enter License Please");
				return;
			}
			CLV.SaveLicense(new CLV
			{
				LicenseKey = txtLicense.Text.Trim(),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			Hide();
			LoginNew loginNew = new LoginNew();
			loginNew.Show();
			loginNew.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			Hide();
			new DashboardNew(clsGlobalVariables.UserAccount).Show();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.EnterLicenseKey));
		this.btnContinue = new System.Windows.Forms.Button();
		this.btnSubmit = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.LblDayLeftMsg = new System.Windows.Forms.Label();
		this.PicLicanceKey = new System.Windows.Forms.PictureBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.lblOr = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.lblMessage = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtLicense = new System.Windows.Forms.RichTextBox();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PicLicanceKey).BeginInit();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.btnContinue.BackColor = System.Drawing.Color.FromArgb(22, 150, 112);
		this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnContinue.Font = new System.Drawing.Font("Segoe UI", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnContinue.ForeColor = System.Drawing.Color.White;
		this.btnContinue.Location = new System.Drawing.Point(100, 553);
		this.btnContinue.Name = "btnContinue";
		this.btnContinue.Size = new System.Drawing.Size(99, 32);
		this.btnContinue.TabIndex = 9;
		this.btnContinue.Text = "Continue";
		this.btnContinue.UseVisualStyleBackColor = false;
		this.btnContinue.Click += new System.EventHandler(button1_Click);
		this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(38, 49, 142);
		this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSubmit.ForeColor = System.Drawing.Color.White;
		this.btnSubmit.Location = new System.Drawing.Point(100, 475);
		this.btnSubmit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.btnSubmit.Name = "btnSubmit";
		this.btnSubmit.Size = new System.Drawing.Size(99, 32);
		this.btnSubmit.TabIndex = 7;
		this.btnSubmit.Text = "Sumbit";
		this.btnSubmit.UseVisualStyleBackColor = false;
		this.btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(309, 597);
		this.panel1.TabIndex = 1;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.lblMessage);
		this.panel2.Controls.Add(this.LblDayLeftMsg);
		this.panel2.Controls.Add(this.PicLicanceKey);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.lblOr);
		this.panel2.Controls.Add(this.btnContinue);
		this.panel2.Controls.Add(this.btnSubmit);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(304, 597);
		this.panel2.TabIndex = 1;
		this.LblDayLeftMsg.Dock = System.Windows.Forms.DockStyle.Top;
		this.LblDayLeftMsg.Font = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold);
		this.LblDayLeftMsg.ForeColor = System.Drawing.Color.FromArgb(245, 65, 65);
		this.LblDayLeftMsg.Location = new System.Drawing.Point(0, 284);
		this.LblDayLeftMsg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.LblDayLeftMsg.Name = "LblDayLeftMsg";
		this.LblDayLeftMsg.Size = new System.Drawing.Size(304, 67);
		this.LblDayLeftMsg.TabIndex = 18;
		this.LblDayLeftMsg.Text = "Paste Your Activation Key Here";
		this.LblDayLeftMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.PicLicanceKey.BackColor = System.Drawing.Color.White;
		this.PicLicanceKey.Dock = System.Windows.Forms.DockStyle.Top;
		this.PicLicanceKey.Image = (System.Drawing.Image)resources.GetObject("PicLicanceKey.Image");
		this.PicLicanceKey.Location = new System.Drawing.Point(0, 200);
		this.PicLicanceKey.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.PicLicanceKey.Name = "PicLicanceKey";
		this.PicLicanceKey.Size = new System.Drawing.Size(304, 84);
		this.PicLicanceKey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.PicLicanceKey.TabIndex = 17;
		this.PicLicanceKey.TabStop = false;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Dock = System.Windows.Forms.DockStyle.Top;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 13.8f);
		this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
		this.label3.Location = new System.Drawing.Point(0, 120);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(304, 80);
		this.label3.TabIndex = 16;
		this.label3.Text = "Please Paste The License Key To Get Rid of This Message.";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
		this.label5.Location = new System.Drawing.Point(0, 65);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(304, 55);
		this.label5.TabIndex = 15;
		this.label5.Text = "Welcome Back !";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblOr.BackColor = System.Drawing.Color.White;
		this.lblOr.Font = new System.Drawing.Font("Segoe UI", 13.8f);
		this.lblOr.ForeColor = System.Drawing.Color.Black;
		this.lblOr.Location = new System.Drawing.Point(100, 509);
		this.lblOr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblOr.Name = "lblOr";
		this.lblOr.Size = new System.Drawing.Size(87, 35);
		this.lblOr.TabIndex = 11;
		this.lblOr.Text = "OR";
		this.lblOr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(238, 7);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 25);
		this.label2.TabIndex = 2;
		this.label2.Text = "ERP";
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 24f);
		this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(304, 65);
		this.label1.TabIndex = 1;
		this.label1.Text = "∈ccountΒook ";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblMessage.BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
		this.lblMessage.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMessage.ForeColor = System.Drawing.Color.DarkRed;
		this.lblMessage.Location = new System.Drawing.Point(0, 351);
		this.lblMessage.Name = "lblMessage";
		this.lblMessage.Size = new System.Drawing.Size(304, 47);
		this.lblMessage.TabIndex = 19;
		this.lblMessage.Text = "label6";
		this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel4.Controls.Add(this.txtLicense);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 398);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(304, 67);
		this.panel4.TabIndex = 20;
		this.txtLicense.BackColor = System.Drawing.Color.White;
		this.txtLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLicense.Location = new System.Drawing.Point(2, 4);
		this.txtLicense.Margin = new System.Windows.Forms.Padding(2);
		this.txtLicense.Name = "txtLicense";
		this.txtLicense.Size = new System.Drawing.Size(300, 58);
		this.txtLicense.TabIndex = 8;
		this.txtLicense.Text = "";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(308, 597);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		base.Name = "EnterLicenseKey";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EnterLicenseKey";
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.PicLicanceKey).EndInit();
		this.panel4.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
