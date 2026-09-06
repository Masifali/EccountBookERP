using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Model;
using Architecture.Model.Main;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.SQL
{

public class SqlLogin : Form
{
	private WaitForm waitForm = new WaitForm();

	private IContainer components;

	internal TextBox txtpassword;

	internal TextBox txtusername;

	internal PictureBox PictureBox3;

	internal PictureBox PictureBox2;

	private Button btnclose;

	private Button btnlogin;

	private Label label1;

	public SqlLogin()
	{
		InitializeComponent();
	}

	private void btnlogin_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			if (txtusername.Text != string.Empty && txtpassword.Text != string.Empty)
			{
				AdminUser obj = UserAccount.AdminLogin(new UserAccount
				{
					UserName = txtusername.Text.Trim(),
					Password = txtpassword.Text.Trim(),
					SystemDate = DateTime.Now
				});
				if (obj.UserName != string.Empty)
				{
					SqlQueryExecuter sqlQueryExecuter = new SqlQueryExecuter();
					SqlQueryExecuter._Role = obj.Role;
					sqlQueryExecuter.ShowDialog();
				}
				else
				{
					MessageBox.Show("Invalid UserId or Password");
				}
			}
			else
			{
				MessageBox.Show("Invalid UserId or Password");
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Invalid UserId or Password");
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void SqlLogin_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			btnlogin.PerformClick();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SQL.SqlLogin));
		this.txtpassword = new System.Windows.Forms.TextBox();
		this.txtusername = new System.Windows.Forms.TextBox();
		this.PictureBox3 = new System.Windows.Forms.PictureBox();
		this.PictureBox2 = new System.Windows.Forms.PictureBox();
		this.btnclose = new System.Windows.Forms.Button();
		this.btnlogin = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.PictureBox3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PictureBox2).BeginInit();
		base.SuspendLayout();
		this.txtpassword.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpassword.ForeColor = System.Drawing.Color.FromArgb(68, 68, 68);
		this.txtpassword.Location = new System.Drawing.Point(172, 124);
		this.txtpassword.Name = "txtpassword";
		this.txtpassword.Size = new System.Drawing.Size(232, 32);
		this.txtpassword.TabIndex = 7;
		this.txtpassword.UseSystemPasswordChar = true;
		this.txtusername.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtusername.ForeColor = System.Drawing.Color.FromArgb(68, 68, 68);
		this.txtusername.Location = new System.Drawing.Point(172, 84);
		this.txtusername.Name = "txtusername";
		this.txtusername.Size = new System.Drawing.Size(232, 32);
		this.txtusername.TabIndex = 6;
		this.PictureBox3.Image = (System.Drawing.Image)resources.GetObject("PictureBox3.Image");
		this.PictureBox3.Location = new System.Drawing.Point(141, 128);
		this.PictureBox3.Name = "PictureBox3";
		this.PictureBox3.Size = new System.Drawing.Size(25, 28);
		this.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PictureBox3.TabIndex = 8;
		this.PictureBox3.TabStop = false;
		this.PictureBox2.Image = (System.Drawing.Image)resources.GetObject("PictureBox2.Image");
		this.PictureBox2.Location = new System.Drawing.Point(141, 88);
		this.PictureBox2.Name = "PictureBox2";
		this.PictureBox2.Size = new System.Drawing.Size(25, 28);
		this.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PictureBox2.TabIndex = 9;
		this.PictureBox2.TabStop = false;
		this.btnclose.BackColor = System.Drawing.Color.DarkOrange;
		this.btnclose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.btnclose.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnclose.ForeColor = System.Drawing.Color.White;
		this.btnclose.Location = new System.Drawing.Point(304, 166);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(98, 32);
		this.btnclose.TabIndex = 11;
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
		this.btnlogin.Location = new System.Drawing.Point(172, 166);
		this.btnlogin.Name = "btnlogin";
		this.btnlogin.Size = new System.Drawing.Size(98, 32);
		this.btnlogin.TabIndex = 10;
		this.btnlogin.Text = "Login";
		this.btnlogin.UseVisualStyleBackColor = false;
		this.btnlogin.Click += new System.EventHandler(btnlogin_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(201, 25);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(163, 31);
		this.label1.TabIndex = 12;
		this.label1.Text = "Admin Login";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(560, 262);
		base.ControlBox = false;
		base.Controls.Add(this.label1);
		base.Controls.Add(this.txtpassword);
		base.Controls.Add(this.txtusername);
		base.Controls.Add(this.PictureBox3);
		base.Controls.Add(this.PictureBox2);
		base.Controls.Add(this.btnclose);
		base.Controls.Add(this.btnlogin);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "SqlLogin";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Admin Login";
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SqlLogin_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.PictureBox3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PictureBox2).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
