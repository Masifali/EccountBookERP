// using Attachment = global::Architecture.Model.Attachment;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.Common;
using global::Architecture.Model;
using UserAccount = global::Architecture.Model.UserAccount;

public class OTPAthentication : Form
{
	private UserAccount UserAccount = new UserAccount();

	private bool Submit;

	private bool Resend;

	public bool LoadOTPForm;

	private int timerValue = 60;

	private IContainer components;

	private TextBox txtOTP;

	private Button btnSend;

	private Button btnReset;

	private Timer timer1;

	private Panel panel1;

	private Label labelTimerShow;

	private Label label3;

	private Label label1;

	public OTPAthentication(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void OTPAthentication_Load(object sender, EventArgs e)
	{
		try
		{
			if (LoadOTPForm)
			{
				txtOTP.Enabled = true;
				btnReset.Enabled = false;
				btnReset.Visible = false;
				btnSend.Visible = true;
				btnSend.Enabled = true;
				timer1.Start();
				timer1.Interval = 60;
				labelTimerShow.Text = timer1.ToString();
				timer1.Interval = 1000;
				timer1.Tick += timer_Tick;
				timer1.Start();
			}
			else
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void timer_Tick(object sender, EventArgs e)
	{
		try
		{
			timerValue--;
			if (timerValue <= 0)
			{
				timer1.Stop();
				txtOTP.Enabled = false;
				btnReset.Enabled = true;
				btnReset.Visible = true;
				btnSend.Enabled = false;
			}
			labelTimerShow.Text = timerValue.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			int AuthenticationCode = global::Architecture.BLL.UserAccount.setAuthenticationCode(clsGlobalVariables.UserAccount.ID, (string)null);
			clsGlobalVariables.UserAccount.AuthenticationCode = AuthenticationCode;
			if (AuthenticationCode > 0)
			{
				timerValue = 60;
				timer1.Start();
				txtOTP.Enabled = true;
				btnReset.Enabled = false;
				btnReset.Visible = false;
				btnSend.Visible = true;
				btnSend.Enabled = true;
			}
			else
			{
				MessageBox.Show("Resend Failed!");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSend_Click(object sender, EventArgs e)
	{
		try
		{
			if (txtOTP.Text != string.Empty && txtOTP.Text.Length <= 6)
			{
				bool otpverified = false;
				if (Conversion.ToInt((object)txtOTP.Text) == clsGlobalVariables.UserAccount.AuthenticationCode)
				{
					otpverified = true;
				}
				if (otpverified)
				{
					Close();
				}
				else
				{
					MessageBox.Show("OTP Verification  Failed!");
				}
			}
			else
			{
				MessageBox.Show("OTP should be 6 digit!");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOTP_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if ((!char.IsDigit(e.KeyChar) || txtOTP.Text.Length >= 6) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == (Keys.F4 | Keys.Alt))
		{
			return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void OTPAthentication_FormClosing(object sender, FormClosingEventArgs e)
	{
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
		this.components = new System.ComponentModel.Container();
		this.txtOTP = new System.Windows.Forms.TextBox();
		this.btnSend = new System.Windows.Forms.Button();
		this.btnReset = new System.Windows.Forms.Button();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.panel1 = new System.Windows.Forms.Panel();
		this.labelTimerShow = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.txtOTP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOTP.Font = new System.Drawing.Font("Verdana", 20f);
		this.txtOTP.Location = new System.Drawing.Point(100, 117);
		this.txtOTP.Margin = new System.Windows.Forms.Padding(4);
		this.txtOTP.Name = "txtOTP";
		this.txtOTP.Size = new System.Drawing.Size(233, 40);
		this.txtOTP.TabIndex = 9;
		this.txtOTP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOTP_KeyPress);
		this.btnSend.BackColor = System.Drawing.Color.Teal;
		this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSend.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.btnSend.ForeColor = System.Drawing.Color.White;
		this.btnSend.Location = new System.Drawing.Point(99, 158);
		this.btnSend.Name = "btnSend";
		this.btnSend.Size = new System.Drawing.Size(235, 39);
		this.btnSend.TabIndex = 10;
		this.btnSend.Text = "Submit";
		this.btnSend.UseVisualStyleBackColor = false;
		this.btnSend.Click += new System.EventHandler(btnSend_Click);
		this.btnReset.BackColor = System.Drawing.Color.White;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.DodgerBlue;
		this.btnReset.Location = new System.Drawing.Point(224, 203);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(109, 28);
		this.btnReset.TabIndex = 11;
		this.btnReset.Text = "Resend OTP";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.labelTimerShow);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.btnReset);
		this.panel1.Controls.Add(this.txtOTP);
		this.panel1.Controls.Add(this.btnSend);
		this.panel1.Location = new System.Drawing.Point(68, 53);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(429, 287);
		this.panel1.TabIndex = 12;
		this.labelTimerShow.AutoSize = true;
		this.labelTimerShow.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelTimerShow.Location = new System.Drawing.Point(100, 204);
		this.labelTimerShow.Name = "labelTimerShow";
		this.labelTimerShow.Size = new System.Drawing.Size(48, 18);
		this.labelTimerShow.TabIndex = 13;
		this.labelTimerShow.Text = "Time";
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Dock = System.Windows.Forms.DockStyle.Top;
		this.label3.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(0, 44);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(427, 69);
		this.label3.TabIndex = 12;
		this.label3.Text = "Please check your email for the OTP verification code we've just sent.";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Verdana", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(427, 44);
		this.label1.TabIndex = 8;
		this.label1.Text = " OTP Verification";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.RoyalBlue;
		base.ClientSize = new System.Drawing.Size(569, 394);
		base.ControlBox = false;
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OTPAthentication";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "OTPAthentication";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OTPAthentication_FormClosing);
		base.Load += new System.EventHandler(OTPAthentication_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
