// using Attachment = global::Architecture.Model.Attachment;
using UserAccount = global::Architecture.Model.UserAccount;
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
namespace Architecture.WinApp
{
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;


using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
// using Architecture.WinApp.Properties;

public class frmWaitForm : Form
{
	private IContainer components;

	private Panel panel1;

	private Label label3;

	private Label label2;

	private Label label1;

	public frmWaitForm()
	{
		InitializeComponent();
	}

	public frmWaitForm(Form parent)
	{
		InitializeComponent();
		if (parent != null)
		{
			base.StartPosition = FormStartPosition.Manual;
			base.Location = new Point(parent.Location.X + parent.Width / 2 - base.Width / 2, parent.Location.Y + parent.Height / 2 - base.Height / 2);
		}
		else
		{
			base.StartPosition = FormStartPosition.CenterParent;
		}
	}

	public void CloseWaitForm()
	{
		base.DialogResult = DialogResult.OK;
		Close();
		if (label1.Image != null)
		{
			label1.Image.Dispose();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(276, 92);
		this.panel1.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
		this.label2.Location = new System.Drawing.Point(112, 49);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(87, 17);
		this.label2.TabIndex = 3;
		this.label2.Text = "Please Wait";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Verdana", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(111, 26);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(107, 23);
		this.label1.TabIndex = 2;
		this.label1.Text = "Loading...";
		this.label3.Dock = System.Windows.Forms.DockStyle.Left;
		this.label3.Image = Architecture.WinApp.Properties.Resources._1231;
		this.label3.Location = new System.Drawing.Point(0, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(105, 90);
		this.label3.TabIndex = 4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(276, 92);
		base.ControlBox = false;
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmWaitForm";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "frmWaitForm";
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
