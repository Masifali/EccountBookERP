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
namespace Architecture.WinApp.SQL
{

public class ResultForm : Form
{
	private IContainer components;

	private RichTextBox txtResult;

	public ResultForm()
	{
		InitializeComponent();
	}

	public void AddMessage(string message, Color color)
	{
		txtResult.SelectionStart = txtResult.TextLength;
		txtResult.SelectionLength = 0;
		txtResult.SelectionColor = color;
		txtResult.AppendText(message + Environment.NewLine);
		txtResult.SelectionColor = txtResult.ForeColor;
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
		this.txtResult = new System.Windows.Forms.RichTextBox();
		base.SuspendLayout();
		this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtResult.Location = new System.Drawing.Point(0, 0);
		this.txtResult.Name = "txtResult";
		this.txtResult.Size = new System.Drawing.Size(800, 450);
		this.txtResult.TabIndex = 0;
		this.txtResult.Text = "";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Controls.Add(this.txtResult);
		base.Name = "ResultForm";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "ResultForm";
		base.ResumeLayout(false);
	}
}

}
