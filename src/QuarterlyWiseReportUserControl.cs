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

public class QuarterlyWiseReportUserControl : UserControl
{
	private string _QuarterStartDate;

	private string _QuarterEndDate;

	private string _Year;

	private string _Amount;

	private IContainer components;

	private Label label6;

	private Label lblAmount;

	private Label lblQuarterStartDate;

	private Label lblYear;

	private Label lblQuarterEndDate;

	public string QuarterStartDate
	{
		get
		{
			return _QuarterStartDate;
		}
		set
		{
			_QuarterStartDate = value;
			lblQuarterStartDate.Text = value;
		}
	}

	public string QuarterEndDate
	{
		get
		{
			return _QuarterEndDate;
		}
		set
		{
			_QuarterEndDate = value;
			lblQuarterEndDate.Text = value;
		}
	}

	public string Year
	{
		get
		{
			return _Year;
		}
		set
		{
			_Year = value;
			lblYear.Text = value;
		}
	}

	public string Amount
	{
		get
		{
			return _Amount;
		}
		set
		{
			_Amount = value;
			lblAmount.Text = value;
		}
	}

	public QuarterlyWiseReportUserControl()
	{
		InitializeComponent();
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
		this.label6 = new System.Windows.Forms.Label();
		this.lblAmount = new System.Windows.Forms.Label();
		this.lblQuarterStartDate = new System.Windows.Forms.Label();
		this.lblYear = new System.Windows.Forms.Label();
		this.lblQuarterEndDate = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(225, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(418, 17);
		this.label6.TabIndex = 10;
		this.label6.Text = "----------------------------------------------------------------------------------";
		this.lblAmount.AutoSize = true;
		this.lblAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmount.Location = new System.Drawing.Point(649, 4);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(15, 17);
		this.lblAmount.TabIndex = 9;
		this.lblAmount.Text = "0";
		this.lblQuarterStartDate.AutoSize = true;
		this.lblQuarterStartDate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblQuarterStartDate.Location = new System.Drawing.Point(5, 4);
		this.lblQuarterStartDate.Name = "lblQuarterStartDate";
		this.lblQuarterStartDate.Size = new System.Drawing.Size(66, 17);
		this.lblQuarterStartDate.TabIndex = 8;
		this.lblQuarterStartDate.Text = "StartDate";
		this.lblYear.AutoSize = true;
		this.lblYear.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblYear.Location = new System.Drawing.Point(179, 4);
		this.lblYear.Name = "lblYear";
		this.lblYear.Size = new System.Drawing.Size(34, 17);
		this.lblYear.TabIndex = 11;
		this.lblYear.Text = "year";
		this.lblQuarterEndDate.AutoSize = true;
		this.lblQuarterEndDate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblQuarterEndDate.Location = new System.Drawing.Point(89, 5);
		this.lblQuarterEndDate.Name = "lblQuarterEndDate";
		this.lblQuarterEndDate.Size = new System.Drawing.Size(60, 17);
		this.lblQuarterEndDate.TabIndex = 12;
		this.lblQuarterEndDate.Text = "EndDate";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblQuarterEndDate);
		base.Controls.Add(this.lblYear);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.lblAmount);
		base.Controls.Add(this.lblQuarterStartDate);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "QuarterlyWiseReportUserControl";
		base.Size = new System.Drawing.Size(763, 26);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
