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

public class DiscountPolicyCard : UserControl
{
	private string _Count;

	private int _DiscountId;

	private string _DiscountType;

	private IContainer components;

	private PictureBox pictureBox1;

	private Label label1;

	private Label lblCount;

	private Button button1;

	private Label lblCustomerType;

	public string Count
	{
		get
		{
			return _Count;
		}
		set
		{
			_Count = value;
			lblCount.Text = value;
		}
	}

	public int DiscountId
	{
		get
		{
			return _DiscountId;
		}
		set
		{
			_DiscountId = value;
		}
	}

	public string DiscountType
	{
		get
		{
			return _DiscountType;
		}
		set
		{
			_DiscountType = value;
			lblCustomerType.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public DiscountPolicyCard()
	{
		InitializeComponent();
		Point pos = PointToScreen(lblCount.Location);
		pos = pictureBox1.PointToClient(pos);
		lblCount.Parent = pictureBox1;
		lblCount.Location = pos;
		lblCount.BackColor = Color.Transparent;
		button1.Click += ControlClicked;
	}

	private void button1_MouseHover(object sender, EventArgs e)
	{
		try
		{
			button1.BackColor = Color.FromArgb(249, 17, 57);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
		this.label1 = new System.Windows.Forms.Label();
		this.lblCount = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.button1 = new System.Windows.Forms.Button();
		this.lblCustomerType = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.label1.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(401, 3);
		this.label1.TabIndex = 1;
		this.lblCount.BackColor = System.Drawing.Color.Transparent;
		this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCount.ForeColor = System.Drawing.Color.FromArgb(52, 58, 64);
		this.lblCount.Location = new System.Drawing.Point(184, 29);
		this.lblCount.Name = "lblCount";
		this.lblCount.Size = new System.Drawing.Size(35, 18);
		this.lblCount.TabIndex = 2;
		this.lblCount.Text = "1";
		this.lblCount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.pictureBox1.Image = Architecture.WinApp.Properties.Resources.ribbon;
		this.pictureBox1.Location = new System.Drawing.Point(3, 13);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(395, 62);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.button1.BackColor = System.Drawing.Color.FromArgb(250, 62, 94);
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(157, 131);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(80, 30);
		this.button1.TabIndex = 3;
		this.button1.Text = "Select";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.MouseHover += new System.EventHandler(button1_MouseHover);
		this.lblCustomerType.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCustomerType.ForeColor = System.Drawing.Color.FromArgb(52, 58, 64);
		this.lblCustomerType.Location = new System.Drawing.Point(66, 82);
		this.lblCustomerType.Name = "lblCustomerType";
		this.lblCustomerType.Size = new System.Drawing.Size(271, 32);
		this.lblCustomerType.TabIndex = 4;
		this.lblCustomerType.Text = "Customer";
		this.lblCustomerType.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.lblCustomerType);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.lblCount);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.pictureBox1);
		base.Margin = new System.Windows.Forms.Padding(6);
		base.Name = "DiscountPolicyCard";
		base.Size = new System.Drawing.Size(401, 173);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}

}
