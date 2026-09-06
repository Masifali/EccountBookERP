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

public class ApprovalTotalsCard : UserControl
{
	private IContainer components;

	public Label labelCaptionTotal;

	public Label labelValue;

	private TableLayoutPanel FlpMain;

	private Panel panelFooter;

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public ApprovalTotalsCard()
	{
		InitializeComponent();
		base.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void ApprovalTotalsCard_Load(object sender, EventArgs e)
	{
	}

	private void ApprovalTotalsCard_Click(object sender, EventArgs e)
	{
		try
		{
			base.Tag = "UserControlClick";
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
		this.labelCaptionTotal = new System.Windows.Forms.Label();
		this.labelValue = new System.Windows.Forms.Label();
		this.FlpMain = new System.Windows.Forms.TableLayoutPanel();
		this.panelFooter = new System.Windows.Forms.Panel();
		this.panelFooter.SuspendLayout();
		base.SuspendLayout();
		this.labelCaptionTotal.AutoSize = true;
		this.labelCaptionTotal.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
		this.labelCaptionTotal.ForeColor = System.Drawing.Color.FromArgb(64, 0, 0);
		this.labelCaptionTotal.Location = new System.Drawing.Point(3, 2);
		this.labelCaptionTotal.Name = "labelCaptionTotal";
		this.labelCaptionTotal.Size = new System.Drawing.Size(82, 25);
		this.labelCaptionTotal.TabIndex = 33;
		this.labelCaptionTotal.Text = "Caption";
		this.labelValue.Font = new System.Drawing.Font("Verdana", 12.75f, System.Drawing.FontStyle.Bold);
		this.labelValue.Location = new System.Drawing.Point(104, 3);
		this.labelValue.Name = "labelValue";
		this.labelValue.Size = new System.Drawing.Size(162, 22);
		this.labelValue.TabIndex = 33;
		this.labelValue.Text = "Value";
		this.labelValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.FlpMain.ColumnCount = 2;
		this.FlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40f));
		this.FlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60f));
		this.FlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.FlpMain.Location = new System.Drawing.Point(0, 0);
		this.FlpMain.Name = "FlpMain";
		this.FlpMain.RowCount = 2;
		this.FlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.FlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.FlpMain.Size = new System.Drawing.Size(270, 86);
		this.FlpMain.TabIndex = 35;
		this.panelFooter.Controls.Add(this.labelValue);
		this.panelFooter.Controls.Add(this.labelCaptionTotal);
		this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelFooter.Location = new System.Drawing.Point(0, 86);
		this.panelFooter.Name = "panelFooter";
		this.panelFooter.Size = new System.Drawing.Size(270, 29);
		this.panelFooter.TabIndex = 36;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.FlpMain);
		base.Controls.Add(this.panelFooter);
		this.Cursor = System.Windows.Forms.Cursors.Hand;
		base.Margin = new System.Windows.Forms.Padding(1);
		base.Name = "ApprovalTotalsCard";
		base.Size = new System.Drawing.Size(270, 115);
		base.Load += new System.EventHandler(ApprovalTotalsCard_Load);
		base.Click += new System.EventHandler(ApprovalTotalsCard_Click);
		this.panelFooter.ResumeLayout(false);
		this.panelFooter.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
