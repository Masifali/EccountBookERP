using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.WinApp.Properties;
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

public class MyOrderStatusCard : UserControl
{
	private string _Caption;

	private string _Value;

	private IContainer components;

	private Label lblCaption;

	private PictureBox pictureBox1;

	private Label lblValue;

	public string Caption
	{
		get
		{
			return _Caption;
		}
		set
		{
			_Caption = value;
			lblCaption.Text = value;
		}
	}

	public string Value
	{
		get
		{
			return _Value;
		}
		set
		{
			_Value = value;
			lblValue.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public MyOrderStatusCard()
	{
		InitializeComponent();
		lblCaption.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void MyOrderStatusCard_Load(object sender, EventArgs e)
	{
		try
		{
			if (Caption == "Booking / Demand")
			{
				pictureBox1.Image = Resources.stack_fill;
			}
			else if (Caption == "Pending Orders")
			{
				pictureBox1.Image = Resources.icons8_pending_32;
			}
			else if (Caption == "Delivery In Transit")
			{
				pictureBox1.Image = Resources.icons8_in_transit_32;
			}
			else if (Caption == "Sales Bill")
			{
				pictureBox1.Image = Resources.icons8_bill_32;
			}
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
		this.lblCaption = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.lblValue = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.Color.White;
		this.lblCaption.Location = new System.Drawing.Point(0, 0);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(178, 43);
		this.lblCaption.TabIndex = 128;
		this.lblCaption.Text = "Booking / Demand";
		this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pictureBox1.Image = Architecture.WinApp.Properties.Resources.stack_fill;
		this.pictureBox1.Location = new System.Drawing.Point(71, 59);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(34, 34);
		this.pictureBox1.TabIndex = 129;
		this.pictureBox1.TabStop = false;
		this.lblValue.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblValue.Location = new System.Drawing.Point(42, 106);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(88, 22);
		this.lblValue.TabIndex = 133;
		this.lblValue.Text = "0";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.lblValue);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.lblCaption);
		base.Name = "MyOrderStatusCard";
		base.Size = new System.Drawing.Size(178, 143);
		base.Load += new System.EventHandler(MyOrderStatusCard_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}

}
