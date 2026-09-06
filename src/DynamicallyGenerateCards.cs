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

public class DynamicallyGenerateCards : UserControl
{
	private string _Reason;

	private string _title;

	private string _Count;

	private string _Id;

	private int _ReasonId;

	private string _TargetUrl;

	private IContainer components;

	private Panel pnlPendingInwardGatePass;

	private Panel panel6;

	private Label CardTitle;

	public Label lblReason;

	public Label CardCount;

	public string Reason
	{
		get
		{
			return _Reason;
		}
		set
		{
			_Reason = value;
			lblReason.Text = value;
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
			CardTitle.Text = value;
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

	public int ReasonId
	{
		get
		{
			return _ReasonId;
		}
		set
		{
			_ReasonId = value;
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

	public DynamicallyGenerateCards()
	{
		InitializeComponent();
		base.Click += ControlClicked;
		CardTitle.Click += ControlClicked;
		CardCount.Click += ControlClicked;
		lblReason.Click += ControlClicked;
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
		this.pnlPendingInwardGatePass = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.CardTitle = new System.Windows.Forms.Label();
		this.lblReason = new System.Windows.Forms.Label();
		this.CardCount = new System.Windows.Forms.Label();
		this.pnlPendingInwardGatePass.SuspendLayout();
		this.panel6.SuspendLayout();
		base.SuspendLayout();
		this.pnlPendingInwardGatePass.BackColor = System.Drawing.Color.White;
		this.pnlPendingInwardGatePass.Controls.Add(this.panel6);
		this.pnlPendingInwardGatePass.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pnlPendingInwardGatePass.Location = new System.Drawing.Point(1, 1);
		this.pnlPendingInwardGatePass.Name = "pnlPendingInwardGatePass";
		this.pnlPendingInwardGatePass.Size = new System.Drawing.Size(183, 52);
		this.pnlPendingInwardGatePass.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.CardTitle);
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(166, 134);
		this.panel6.TabIndex = 4;
		this.CardTitle.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CardTitle.ForeColor = System.Drawing.Color.White;
		this.CardTitle.Location = new System.Drawing.Point(0, 0);
		this.CardTitle.Name = "CardTitle";
		this.CardTitle.Size = new System.Drawing.Size(166, 52);
		this.CardTitle.TabIndex = 2;
		this.CardTitle.Text = "Title";
		this.CardTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblReason.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblReason.Location = new System.Drawing.Point(0, 53);
		this.lblReason.Name = "lblReason";
		this.lblReason.Size = new System.Drawing.Size(167, 45);
		this.lblReason.TabIndex = 5;
		this.lblReason.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblReason.Visible = false;
		this.CardCount.BackColor = System.Drawing.Color.White;
		this.CardCount.Cursor = System.Windows.Forms.Cursors.Hand;
		this.CardCount.Font = new System.Drawing.Font("Calibri", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CardCount.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.CardCount.Location = new System.Drawing.Point(4, 81);
		this.CardCount.Name = "CardCount";
		this.CardCount.Size = new System.Drawing.Size(160, 39);
		this.CardCount.TabIndex = 3;
		this.CardCount.Text = "0";
		this.CardCount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.CardCount);
		base.Controls.Add(this.lblReason);
		base.Controls.Add(this.pnlPendingInwardGatePass);
		base.Name = "DynamicallyGenerateCards";
		base.Size = new System.Drawing.Size(167, 135);
		this.pnlPendingInwardGatePass.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
