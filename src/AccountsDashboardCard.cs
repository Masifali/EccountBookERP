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

public class AccountsDashboardCard : UserControl
{
	private int _Id;

	private string _title;

	private string _OpeningBalance;

	private string _CurrentDrBalance;

	private string _CurrentCrBalance;

	private string _Status;

	private string _DiffValue;

	private string _ClosingBalance;

	private IContainer components;

	private Label CardTitle;

	private PictureBox pictureBox1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label lblOpeningBalance;

	private Label lblCurrentDrBalance;

	private Label lblCurrentCrBalance;

	private Label lblClosingBalance;

	private Label MoreInfo;

	private Panel panel1;

	private Label lblStatus;

	private Label lblDiffValue;

	public int Id
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

	public string OpeningBalance
	{
		get
		{
			return _OpeningBalance;
		}
		set
		{
			_OpeningBalance = value;
			lblOpeningBalance.Text = value;
		}
	}

	public string CurrentDrBalance
	{
		get
		{
			return _CurrentDrBalance;
		}
		set
		{
			_CurrentDrBalance = value;
			lblCurrentDrBalance.Text = value;
		}
	}

	public string CurrentCrBalance
	{
		get
		{
			return _CurrentCrBalance;
		}
		set
		{
			_CurrentCrBalance = value;
			lblCurrentCrBalance.Text = value;
		}
	}

	public string Status
	{
		get
		{
			return _Status;
		}
		set
		{
			_Status = value;
			lblStatus.Text = value;
		}
	}

	public string DiffValue
	{
		get
		{
			return _DiffValue;
		}
		set
		{
			_DiffValue = value;
			lblDiffValue.Text = value;
		}
	}

	public string ClosingBalance
	{
		get
		{
			return _ClosingBalance;
		}
		set
		{
			_ClosingBalance = value;
			lblClosingBalance.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public AccountsDashboardCard()
	{
		InitializeComponent();
		MoreInfo.Click += ControlClicked;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.DynamicCards.AccountsDashboardCard));
		this.CardTitle = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.lblOpeningBalance = new System.Windows.Forms.Label();
		this.lblCurrentDrBalance = new System.Windows.Forms.Label();
		this.lblCurrentCrBalance = new System.Windows.Forms.Label();
		this.lblClosingBalance = new System.Windows.Forms.Label();
		this.MoreInfo = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblDiffValue = new System.Windows.Forms.Label();
		this.lblStatus = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.CardTitle.BackColor = System.Drawing.Color.Teal;
		this.CardTitle.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CardTitle.ForeColor = System.Drawing.Color.White;
		this.CardTitle.Location = new System.Drawing.Point(0, 1);
		this.CardTitle.Name = "CardTitle";
		this.CardTitle.Size = new System.Drawing.Size(304, 36);
		this.CardTitle.TabIndex = 0;
		this.CardTitle.Text = "CAPTION";
		this.CardTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pictureBox1.BackColor = System.Drawing.Color.Teal;
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.ImageLocation = "";
		this.pictureBox1.Location = new System.Drawing.Point(5, 5);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(27, 28);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 1;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label2.Location = new System.Drawing.Point(10, 8);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(60, 16);
		this.label2.TabIndex = 2;
		this.label2.Text = "Opening";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label3.Location = new System.Drawing.Point(10, 87);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(53, 16);
		this.label3.TabIndex = 3;
		this.label3.Text = "Closing";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label4.Location = new System.Drawing.Point(10, 48);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(75, 16);
		this.label4.TabIndex = 4;
		this.label4.Text = "Current Cr";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label5.Location = new System.Drawing.Point(10, 28);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 16);
		this.label5.TabIndex = 5;
		this.label5.Text = "Current Dr";
		this.lblOpeningBalance.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblOpeningBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOpeningBalance.Location = new System.Drawing.Point(97, 9);
		this.lblOpeningBalance.Name = "lblOpeningBalance";
		this.lblOpeningBalance.Size = new System.Drawing.Size(201, 18);
		this.lblOpeningBalance.TabIndex = 6;
		this.lblOpeningBalance.Text = "Value";
		this.lblOpeningBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCurrentDrBalance.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblCurrentDrBalance.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblCurrentDrBalance.Location = new System.Drawing.Point(97, 29);
		this.lblCurrentDrBalance.Name = "lblCurrentDrBalance";
		this.lblCurrentDrBalance.Size = new System.Drawing.Size(201, 18);
		this.lblCurrentDrBalance.TabIndex = 7;
		this.lblCurrentDrBalance.Text = "Value";
		this.lblCurrentDrBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCurrentCrBalance.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblCurrentCrBalance.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblCurrentCrBalance.Location = new System.Drawing.Point(97, 49);
		this.lblCurrentCrBalance.Name = "lblCurrentCrBalance";
		this.lblCurrentCrBalance.Size = new System.Drawing.Size(201, 18);
		this.lblCurrentCrBalance.TabIndex = 8;
		this.lblCurrentCrBalance.Text = "Value";
		this.lblCurrentCrBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblClosingBalance.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblClosingBalance.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblClosingBalance.Location = new System.Drawing.Point(97, 88);
		this.lblClosingBalance.Name = "lblClosingBalance";
		this.lblClosingBalance.Size = new System.Drawing.Size(201, 18);
		this.lblClosingBalance.TabIndex = 9;
		this.lblClosingBalance.Text = "Value";
		this.lblClosingBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.MoreInfo.BackColor = System.Drawing.Color.Teal;
		this.MoreInfo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.MoreInfo.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.MoreInfo.ForeColor = System.Drawing.Color.White;
		this.MoreInfo.Location = new System.Drawing.Point(0, 152);
		this.MoreInfo.Name = "MoreInfo";
		this.MoreInfo.Size = new System.Drawing.Size(304, 23);
		this.MoreInfo.TabIndex = 10;
		this.MoreInfo.Text = "More Info  >>>>>";
		this.MoreInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.lblCurrentDrBalance);
		this.panel1.Controls.Add(this.lblOpeningBalance);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.lblStatus);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.lblDiffValue);
		this.panel1.Controls.Add(this.lblClosingBalance);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.lblCurrentCrBalance);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Location = new System.Drawing.Point(1, 37);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(302, 113);
		this.panel1.TabIndex = 11;
		this.lblDiffValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblDiffValue.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblDiffValue.Location = new System.Drawing.Point(97, 68);
		this.lblDiffValue.Name = "lblDiffValue";
		this.lblDiffValue.Size = new System.Drawing.Size(201, 18);
		this.lblDiffValue.TabIndex = 9;
		this.lblDiffValue.Text = "Value";
		this.lblDiffValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblStatus.AutoSize = true;
		this.lblStatus.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblStatus.Location = new System.Drawing.Point(10, 68);
		this.lblStatus.Name = "lblStatus";
		this.lblStatus.Size = new System.Drawing.Size(51, 16);
		this.lblStatus.TabIndex = 4;
		this.lblStatus.Text = "Status";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.MoreInfo);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.CardTitle);
		base.Name = "AccountsDashboardCard";
		base.Size = new System.Drawing.Size(304, 176);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
