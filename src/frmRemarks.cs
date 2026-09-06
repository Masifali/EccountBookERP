using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.WinApp.Helper;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{

public class frmRemarks : Form
{
	private int RemarksLength = 10;

	private IContainer components;

	private TextBox txtRemarks;

	private Button btnOk;

	private Button btnCancel;

	private Label lblHint;

	public string RemarksText { get; private set; }

	public frmRemarks(string message)
	{
		InitializeComponent();
		Text = message;
	}

	private void frmRemarks_Load(object sender, EventArgs e)
	{
		RemarksLength = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ValidationRemarksLength"));
		if (RemarksLength <= 0)
		{
			RemarksLength = 10;
		}
		lblHint.Text = $"Minimum {RemarksLength} characters required.";
	}

	private void txtRemarks_TextChanged(object sender, EventArgs e)
	{
		lblHint.ForeColor = ((txtRemarks.Text.Length >= RemarksLength) ? Color.Green : Color.DarkRed);
	}

	private void btnOk_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(txtRemarks.Text))
		{
			MessageBox.Show("Remarks are required.");
			return;
		}
		if (txtRemarks.Text.Trim().Length < RemarksLength)
		{
			MessageBox.Show($"At least {RemarksLength} characters are required.");
			return;
		}
		RemarksText = txtRemarks.Text.Trim();
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
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
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.btnOk = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.lblHint = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.txtRemarks.Location = new System.Drawing.Point(12, 12);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(360, 120);
		this.txtRemarks.TabIndex = 0;
		this.txtRemarks.TextChanged += new System.EventHandler(txtRemarks_TextChanged);
		this.lblHint.AutoSize = true;
		this.lblHint.ForeColor = System.Drawing.Color.DarkRed;
		this.lblHint.Location = new System.Drawing.Point(12, 135);
		this.lblHint.Name = "lblHint";
		this.lblHint.Size = new System.Drawing.Size(200, 13);
		this.lblHint.TabIndex = 3;
		this.lblHint.Text = "Minimum characters required: 10";
		this.btnOk.Location = new System.Drawing.Point(216, 155);
		this.btnOk.Name = "btnOk";
		this.btnOk.Size = new System.Drawing.Size(75, 30);
		this.btnOk.TabIndex = 1;
		this.btnOk.Text = "OK";
		this.btnOk.UseVisualStyleBackColor = true;
		this.btnOk.Click += new System.EventHandler(btnOk_Click);
		this.btnCancel.Location = new System.Drawing.Point(297, 155);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 30);
		this.btnCancel.TabIndex = 2;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(384, 200);
		base.Controls.Add(this.lblHint);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOk);
		base.Controls.Add(this.txtRemarks);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmRemarks";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Remarks";
		base.Load += new System.EventHandler(frmRemarks_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
