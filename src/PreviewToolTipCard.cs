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
namespace Architecture.WinApp.ModernHistories
{

public class PreviewToolTipCard : UserControl
{
	private IContainer components;

	public FlowLayoutPanel FlpMain;

	public Label labelCaption;

	public int DocumentId { get; set; }

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public PreviewToolTipCard()
	{
		InitializeComponent();
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void PreviewToolTipCard_Load(object sender, EventArgs e)
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
		this.FlpMain = new System.Windows.Forms.FlowLayoutPanel();
		this.labelCaption = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.FlpMain.BackColor = System.Drawing.Color.WhiteSmoke;
		this.FlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.FlpMain.Location = new System.Drawing.Point(0, 23);
		this.FlpMain.Name = "FlpMain";
		this.FlpMain.Size = new System.Drawing.Size(323, 121);
		this.FlpMain.TabIndex = 0;
		this.labelCaption.Dock = System.Windows.Forms.DockStyle.Top;
		this.labelCaption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelCaption.ForeColor = System.Drawing.Color.Maroon;
		this.labelCaption.Location = new System.Drawing.Point(0, 0);
		this.labelCaption.Name = "labelCaption";
		this.labelCaption.Size = new System.Drawing.Size(323, 23);
		this.labelCaption.TabIndex = 1;
		this.labelCaption.Text = "Preview";
		this.labelCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.FlpMain);
		base.Controls.Add(this.labelCaption);
		this.Cursor = System.Windows.Forms.Cursors.Hand;
		base.Name = "PreviewToolTipCard";
		base.Size = new System.Drawing.Size(323, 144);
		base.Load += new System.EventHandler(PreviewToolTipCard_Load);
		base.ResumeLayout(false);
	}
}

}
