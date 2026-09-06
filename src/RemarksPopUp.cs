using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Inventory_Reports
{

public class RemarksPopUp : Form
{
	private IContainer components;

	internal Button btnOk;

	internal Label Label1;

	public RichTextBox txtRemarks;

	public RemarksPopUp()
	{
		InitializeComponent();
	}

	private void btnOk_Click(object sender, EventArgs e)
	{
		try
		{
			if (txtRemarks.Text == string.Empty)
			{
				MessageBox.Show("Remarks Required!");
				txtRemarks.Focus();
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

	private void SubFolderPopUp_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void RemarksPopUp_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.Return)
		{
			btnOk_Click(null, null);
		}
	}

	private void RemarksPopUp_Load(object sender, EventArgs e)
	{
		txtRemarks.Focus();
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
		this.btnOk = new System.Windows.Forms.Button();
		this.Label1 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		base.SuspendLayout();
		this.btnOk.BackColor = System.Drawing.Color.Teal;
		this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnOk.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnOk.ForeColor = System.Drawing.Color.White;
		this.btnOk.Location = new System.Drawing.Point(139, 83);
		this.btnOk.Name = "btnOk";
		this.btnOk.Size = new System.Drawing.Size(75, 25);
		this.btnOk.TabIndex = 5;
		this.btnOk.Text = "OK";
		this.btnOk.UseVisualStyleBackColor = false;
		this.btnOk.Click += new System.EventHandler(btnOk_Click);
		this.Label1.AutoSize = true;
		this.Label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Label1.Location = new System.Drawing.Point(4, 32);
		this.Label1.Name = "Label1";
		this.Label1.Size = new System.Drawing.Size(88, 15);
		this.Label1.TabIndex = 4;
		this.Label1.Text = "Enter Remarks: ";
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(95, 5);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(235, 76);
		this.txtRemarks.TabIndex = 6;
		this.txtRemarks.Text = "";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(334, 110);
		base.Controls.Add(this.txtRemarks);
		base.Controls.Add(this.btnOk);
		base.Controls.Add(this.Label1);
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "RemarksPopUp";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Remarks";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(SubFolderPopUp_FormClosed);
		base.Load += new System.EventHandler(RemarksPopUp_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(RemarksPopUp_KeyDown);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
