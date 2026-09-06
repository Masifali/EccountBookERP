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

public class PendingWorkCards : UserControl
{
	private string _Caption;

	private string _title;

	private string _Count;

	private string _Id;

	private string _TargetUrl;

	private IContainer components;

	private Label CardCount;

	public Label CardTitle;

	public string Caption
	{
		get
		{
			return _Caption;
		}
		set
		{
			_Caption = value;
			CardTitle.Text = value;
		}
	}

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
		}
	}

	public string Count
	{
		get
		{
			return _Count;
		}
		set
		{
			_Count = value;
			CardCount.Text = value;
		}
	}

	public string Id
	{
		get
		{
			return _Id;
		}
		set
		{
			_Id = value;
		}
	}

	public string TargetUrl
	{
		get
		{
			return _TargetUrl;
		}
		set
		{
			_TargetUrl = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public PendingWorkCards()
	{
		InitializeComponent();
		base.Click += ControlClicked;
		CardTitle.Click += ControlClicked;
		CardCount.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
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
		this.CardTitle = new System.Windows.Forms.Label();
		this.CardCount = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.CardTitle.BackColor = System.Drawing.Color.Teal;
		this.CardTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.CardTitle.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CardTitle.ForeColor = System.Drawing.Color.White;
		this.CardTitle.Location = new System.Drawing.Point(0, 0);
		this.CardTitle.Margin = new System.Windows.Forms.Padding(0);
		this.CardTitle.Name = "CardTitle";
		this.CardTitle.Size = new System.Drawing.Size(136, 58);
		this.CardTitle.TabIndex = 2;
		this.CardTitle.Text = "Title";
		this.CardTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CardCount.BackColor = System.Drawing.Color.White;
		this.CardCount.Cursor = System.Windows.Forms.Cursors.Hand;
		this.CardCount.Font = new System.Drawing.Font("Calibri", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CardCount.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.CardCount.Location = new System.Drawing.Point(3, 61);
		this.CardCount.Name = "CardCount";
		this.CardCount.Size = new System.Drawing.Size(131, 39);
		this.CardCount.TabIndex = 3;
		this.CardCount.Text = "0";
		this.CardCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.CardTitle);
		base.Controls.Add(this.CardCount);
		base.Name = "PendingWorkCards";
		base.Size = new System.Drawing.Size(136, 107);
		base.ResumeLayout(false);
	}
}

}
