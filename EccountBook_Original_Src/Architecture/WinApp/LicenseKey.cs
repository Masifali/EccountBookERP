using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;

namespace Architecture.WinApp;

public class LicenseKey : Form
{
	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Label label1;

	private Label label3;

	private Button btnSubmit;

	private Label label2;

	private DateTimePicker txtLicenseExpiryDate;

	private Panel panel1;

	private Panel panel4;

	private TextBox txtLicense;

	private Label LblDayLeftMsg;

	private PictureBox PicLicanceKey;

	private Label label5;

	public LicenseKey(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void LicenseKey_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(420, 420);
			FillDate();
			txtLicense.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDate()
	{
		try
		{
			DataTable dt = UserAccount.GetLicensCompanyAndExpiryDate(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				txtLicenseExpiryDate.Value = Conversion.ToDateTime(dt.Rows[0]["LicenseDate"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show("License Updated Successfully...");
			FillDate();
			txtLicense.Text = string.Empty;
			txtLicense.Focus();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.LicenseKey));
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btnSubmit = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.txtLicenseExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.PicLicanceKey = new System.Windows.Forms.PictureBox();
		this.LblDayLeftMsg = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtLicense = new System.Windows.Forms.TextBox();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PicLicanceKey).BeginInit();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(411, 65);
		this.label1.TabIndex = 18;
		this.label1.Text = "∈ccountΒook ";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(295, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 25);
		this.label3.TabIndex = 25;
		this.label3.Text = "ERP";
		this.btnSubmit.BackColor = System.Drawing.Color.Teal;
		this.btnSubmit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSubmit.Font = new System.Drawing.Font("Verdana", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSubmit.ForeColor = System.Drawing.Color.White;
		this.btnSubmit.Location = new System.Drawing.Point(296, 324);
		this.btnSubmit.Margin = new System.Windows.Forms.Padding(2);
		this.btnSubmit.Name = "btnSubmit";
		this.btnSubmit.Size = new System.Drawing.Size(108, 32);
		this.btnSubmit.TabIndex = 26;
		this.btnSubmit.Text = "Sumbit";
		this.btnSubmit.UseVisualStyleBackColor = false;
		this.btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Red;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 13f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(10, 73);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(223, 25);
		this.label2.TabIndex = 25;
		this.label2.Text = "License Will be Expire on";
		this.txtLicenseExpiryDate.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLicenseExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtLicenseExpiryDate.Location = new System.Drawing.Point(238, 72);
		this.txtLicenseExpiryDate.Name = "txtLicenseExpiryDate";
		this.txtLicenseExpiryDate.Size = new System.Drawing.Size(168, 26);
		this.txtLicenseExpiryDate.TabIndex = 27;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.LblDayLeftMsg);
		this.panel1.Controls.Add(this.PicLicanceKey);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.txtLicenseExpiryDate);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.btnSubmit);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(411, 361);
		this.panel1.TabIndex = 28;
		this.label5.BackColor = System.Drawing.Color.Red;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
		this.label5.Location = new System.Drawing.Point(0, 65);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(411, 40);
		this.label5.TabIndex = 28;
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.PicLicanceKey.BackColor = System.Drawing.Color.White;
		this.PicLicanceKey.Dock = System.Windows.Forms.DockStyle.Top;
		this.PicLicanceKey.Image = (System.Drawing.Image)resources.GetObject("PicLicanceKey.Image");
		this.PicLicanceKey.Location = new System.Drawing.Point(0, 105);
		this.PicLicanceKey.Margin = new System.Windows.Forms.Padding(2);
		this.PicLicanceKey.Name = "PicLicanceKey";
		this.PicLicanceKey.Size = new System.Drawing.Size(411, 116);
		this.PicLicanceKey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.PicLicanceKey.TabIndex = 29;
		this.PicLicanceKey.TabStop = false;
		this.LblDayLeftMsg.Dock = System.Windows.Forms.DockStyle.Top;
		this.LblDayLeftMsg.Font = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold);
		this.LblDayLeftMsg.ForeColor = System.Drawing.Color.FromArgb(245, 65, 65);
		this.LblDayLeftMsg.Location = new System.Drawing.Point(0, 221);
		this.LblDayLeftMsg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.LblDayLeftMsg.Name = "LblDayLeftMsg";
		this.LblDayLeftMsg.Size = new System.Drawing.Size(411, 37);
		this.LblDayLeftMsg.TabIndex = 30;
		this.LblDayLeftMsg.Text = "Paste Your Activation Key Here";
		this.LblDayLeftMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel4.Controls.Add(this.txtLicense);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 258);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(411, 62);
		this.panel4.TabIndex = 31;
		this.txtLicense.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtLicense.Location = new System.Drawing.Point(0, 0);
		this.txtLicense.Multiline = true;
		this.txtLicense.Name = "txtLicense";
		this.txtLicense.Size = new System.Drawing.Size(411, 62);
		this.txtLicense.TabIndex = 9;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(415, 361);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "LicenseKey";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "LicenseKey";
		base.Load += new System.EventHandler(LicenseKey_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.PicLicanceKey).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		base.ResumeLayout(false);
	}
}
