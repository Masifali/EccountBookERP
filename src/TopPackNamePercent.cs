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
namespace Architecture.WinApp.DynamicCards
{

public class TopPackNamePercent : UserControl
{
	private string _PackName;

	private string _Percent;

	private Color _StripColor;

	private IContainer components;

	private Label Percent;

	public Label StripColor;

	private Label lblPack;

	public string lblPackName
	{
		get
		{
			return _PackName;
		}
		set
		{
			_PackName = value;
			lblPack.Text = value;
		}
	}

	public string lblPercent
	{
		get
		{
			return _Percent;
		}
		set
		{
			_Percent = value;
			Percent.Text = value;
		}
	}

	public Color lblStripColor
	{
		get
		{
			return _StripColor;
		}
		set
		{
			_StripColor = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public TopPackNamePercent()
	{
		InitializeComponent();
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void TopImageNamePercent_Load(object sender, EventArgs e)
	{
		try
		{
			StripColor.BackColor = lblStripColor;
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
		this.Percent = new System.Windows.Forms.Label();
		this.StripColor = new System.Windows.Forms.Label();
		this.lblPack = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.Percent.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Percent.Location = new System.Drawing.Point(3, 83);
		this.Percent.Name = "Percent";
		this.Percent.Size = new System.Drawing.Size(77, 24);
		this.Percent.TabIndex = 21;
		this.Percent.Text = "0";
		this.Percent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.StripColor.BackColor = System.Drawing.Color.FromArgb(34, 131, 19);
		this.StripColor.Location = new System.Drawing.Point(7, 20);
		this.StripColor.Name = "StripColor";
		this.StripColor.Size = new System.Drawing.Size(6, 50);
		this.StripColor.TabIndex = 24;
		this.lblPack.AutoSize = true;
		this.lblPack.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPack.Location = new System.Drawing.Point(19, 35);
		this.lblPack.Name = "lblPack";
		this.lblPack.Size = new System.Drawing.Size(52, 21);
		this.lblPack.TabIndex = 25;
		this.lblPack.Text = "50 Kg";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblPack);
		base.Controls.Add(this.StripColor);
		base.Controls.Add(this.Percent);
		base.Name = "TopPackNamePercent";
		base.Size = new System.Drawing.Size(83, 110);
		base.Load += new System.EventHandler(TopImageNamePercent_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
