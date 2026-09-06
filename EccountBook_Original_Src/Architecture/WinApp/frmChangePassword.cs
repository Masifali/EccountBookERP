using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Model;

namespace Architecture.WinApp;

public class frmChangePassword : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Button btnsave;

	private Label label3;

	private TextBox txtNewPassward;

	private Label label2;

	private TextBox txtConfirmNewPassword;

	private Label label1;

	private TextBox txtOldPassword;

	private Label label5;

	private Label label4;

	private Panel panel3;

	private Button btnclose;

	private Label label15;

	private Label label6;

	private Label label7;

	private Label label8;

	public frmChangePassword(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmChangePassword_Load(object sender, EventArgs e)
	{
		txtOldPassword.Focus();
	}

	public void FormReset()
	{
		txtOldPassword.Text = string.Empty;
		txtConfirmNewPassword.Text = string.Empty;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = UserAccount.ID;
			obj.OldPassword = txtOldPassword.Text.Trim();
			if (txtOldPassword.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter Old Passward !");
				txtOldPassword.Focus();
			}
			else if (txtNewPassward.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter New Passward !");
				txtNewPassward.Focus();
			}
			else if (txtConfirmNewPassword.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter Confirm New Passward !");
				txtConfirmNewPassword.Focus();
			}
			else if (txtConfirmNewPassword.Text == txtNewPassward.Text)
			{
				obj.Password = txtConfirmNewPassword.Text;
				string password = txtNewPassward.Text.Trim();
				bool num = password.Length > 7;
				bool hasSpecialCharacter = !string.IsNullOrEmpty(password) && password.Any((char c) => !char.IsLetterOrDigit(c));
				if (num)
				{
					if (!hasSpecialCharacter)
					{
						MessageBox.Show("New Passward Should Have a Special Character!", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					UserAccount.UpdateUserNewPassword(obj);
					MessageBox.Show("Update Successfully");
					Application.Exit();
				}
				else
				{
					MessageBox.Show("New Passward Minimum Lenght Should be of 8 Characters!", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else
			{
				MessageBox.Show("New Passward Should Match With Confirm New Passward!");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmChangePassword_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			btnsave_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			FormReset();
		}
	}

	private void btnclose_Click_1(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.frmChangePassword));
		this.panel1 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnsave = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.txtNewPassward = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtConfirmNewPassword = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtOldPassword = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnclose = new System.Windows.Forms.Button();
		this.label15 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.label8);
		this.panel1.Controls.Add(this.label7);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(383, 588);
		this.panel1.TabIndex = 0;
		this.label8.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(0, 544);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(381, 42);
		this.label8.TabIndex = 23;
		this.label8.Text = "Powered By Synergic Corporate Solution";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.DarkGreen;
		this.label7.Location = new System.Drawing.Point(0, 364);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(381, 180);
		this.label7.TabIndex = 7;
		this.label7.Text = "Password Must be at least \r\n8 characters \r\nand contain at least \r\none special letter";
		this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(288, 50);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(46, 25);
		this.label6.TabIndex = 7;
		this.label6.Text = "ERP";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.btnsave);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtNewPassward);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtConfirmNewPassword);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtOldPassword);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 144);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(381, 220);
		this.panel4.TabIndex = 22;
		this.btnsave.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnsave.Font = new System.Drawing.Font("Calibri", 12.75f, System.Drawing.FontStyle.Bold);
		this.btnsave.ForeColor = System.Drawing.Color.White;
		this.btnsave.Location = new System.Drawing.Point(302, 104);
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(57, 27);
		this.btnsave.TabIndex = 3;
		this.btnsave.Text = "Save";
		this.btnsave.UseVisualStyleBackColor = false;
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(16, 56);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(97, 17);
		this.label3.TabIndex = 5;
		this.label3.Text = "New Password";
		this.txtNewPassward.Location = new System.Drawing.Point(174, 54);
		this.txtNewPassward.Name = "txtNewPassward";
		this.txtNewPassward.PasswordChar = '*';
		this.txtNewPassward.Size = new System.Drawing.Size(185, 20);
		this.txtNewPassward.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(16, 81);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(151, 17);
		this.label2.TabIndex = 3;
		this.label2.Text = "Confirm New Password";
		this.txtConfirmNewPassword.Location = new System.Drawing.Point(174, 79);
		this.txtConfirmNewPassword.Name = "txtConfirmNewPassword";
		this.txtConfirmNewPassword.PasswordChar = '*';
		this.txtConfirmNewPassword.Size = new System.Drawing.Size(185, 20);
		this.txtConfirmNewPassword.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(16, 31);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(92, 17);
		this.label1.TabIndex = 1;
		this.label1.Text = "Old Password";
		this.txtOldPassword.Location = new System.Drawing.Point(174, 30);
		this.txtOldPassword.Name = "txtOldPassword";
		this.txtOldPassword.PasswordChar = '*';
		this.txtOldPassword.Size = new System.Drawing.Size(185, 20);
		this.txtOldPassword.TabIndex = 0;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(0, 96);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(381, 48);
		this.label5.TabIndex = 21;
		this.label5.Text = "Welcome !";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(0, 37);
		this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(381, 59);
		this.label4.TabIndex = 20;
		this.label4.Text = "∈ccountΒook ";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.panel3.Controls.Add(this.btnclose);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(381, 37);
		this.panel3.TabIndex = 18;
		this.btnclose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnclose.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnclose.FlatAppearance.BorderSize = 0;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Image = (System.Drawing.Image)resources.GetObject("btnclose.Image");
		this.btnclose.Location = new System.Drawing.Point(331, 4);
		this.btnclose.Margin = new System.Windows.Forms.Padding(2);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(44, 28);
		this.btnclose.TabIndex = 6;
		this.btnclose.UseVisualStyleBackColor = false;
		this.btnclose.Click += new System.EventHandler(btnclose_Click_1);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(8, 7);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(164, 24);
		this.label15.TabIndex = 2;
		this.label15.Text = "Change Password";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(383, 588);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.KeyPreview = true;
		base.Name = "frmChangePassword";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBookERP";
		base.Load += new System.EventHandler(frmChangePassword_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmChangePassword_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
	}
}
