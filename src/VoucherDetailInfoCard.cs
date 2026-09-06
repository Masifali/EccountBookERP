using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.WinApp.Common;
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

public class VoucherDetailInfoCard : UserControl
{
	private int _AccountId;

	private int _OffsetAccountId;

	private string _AccountTitle;

	private string _OffsetAccountTitle;

	private string _JobLot;

	private string _DebitAmount;

	private string _CreditAmount;

	private string _Remarks;

	private DateTime _VoucherDate;

	private IContainer components;

	private Panel VoucherInfoPanel;

	private Button ShowHidButton;

	private Label lblOffsetAccount;

	private Label lblCreditAmount;

	private Label lblAccountTiltle;

	private Label lblDebitAmount;

	private Label lblJobLot;

	private Panel DetailPanel;

	private Label lblRemarks;

	private Label label26;

	public int AccountId
	{
		get
		{
			return _AccountId;
		}
		set
		{
			_AccountId = value;
		}
	}

	public int OffsetAccountId
	{
		get
		{
			return _OffsetAccountId;
		}
		set
		{
			_OffsetAccountId = value;
		}
	}

	public string AccountTitle
	{
		get
		{
			return _AccountTitle;
		}
		set
		{
			_AccountTitle = value;
			lblAccountTiltle.Text = value;
		}
	}

	public string OffsetAccountTitle
	{
		get
		{
			return _OffsetAccountTitle;
		}
		set
		{
			_OffsetAccountTitle = value;
			lblOffsetAccount.Text = value;
		}
	}

	public string JobLot
	{
		get
		{
			return _JobLot;
		}
		set
		{
			_JobLot = value;
			lblJobLot.Text = value;
		}
	}

	public string DebitAmount
	{
		get
		{
			return _DebitAmount;
		}
		set
		{
			_DebitAmount = value;
			lblDebitAmount.Text = value;
		}
	}

	public string CreditAmount
	{
		get
		{
			return _CreditAmount;
		}
		set
		{
			_CreditAmount = value;
			lblCreditAmount.Text = value;
		}
	}

	public string Remarks
	{
		get
		{
			return _Remarks;
		}
		set
		{
			_Remarks = value;
			lblRemarks.Text = value;
		}
	}

	public DateTime VoucherDate
	{
		get
		{
			return _VoucherDate;
		}
		set
		{
			_VoucherDate = value;
		}
	}

	public VoucherDetailInfoCard()
	{
		InitializeComponent();
	}

	private void lblAccountTiltle_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToGeneralLedgerFromLinkedEvent(AccountId, clsGlobalVariables.ActiveYr.Start_Period, VoucherDate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowHidButton_Click(object sender, EventArgs e)
	{
		try
		{
			if (ShowHidButton.Text == "+")
			{
				DetailPanel.Show();
				ShowHidButton.Text = "-";
				base.Height = 74;
			}
			else
			{
				DetailPanel.Hide();
				ShowHidButton.Text = "+";
				base.Height = 30;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherDetailInfoCard_Load(object sender, EventArgs e)
	{
		try
		{
			ShowHidButton.Text = "+";
			DetailPanel.Hide();
			base.Height = 30;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblOffsetAccount_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToGeneralLedgerFromLinkedEvent(OffsetAccountId, clsGlobalVariables.ActiveYr.Start_Period, VoucherDate);
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
		this.VoucherInfoPanel = new System.Windows.Forms.Panel();
		this.ShowHidButton = new System.Windows.Forms.Button();
		this.lblOffsetAccount = new System.Windows.Forms.Label();
		this.lblCreditAmount = new System.Windows.Forms.Label();
		this.lblAccountTiltle = new System.Windows.Forms.Label();
		this.lblDebitAmount = new System.Windows.Forms.Label();
		this.lblJobLot = new System.Windows.Forms.Label();
		this.DetailPanel = new System.Windows.Forms.Panel();
		this.lblRemarks = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.VoucherInfoPanel.SuspendLayout();
		this.DetailPanel.SuspendLayout();
		base.SuspendLayout();
		this.VoucherInfoPanel.BackColor = System.Drawing.Color.White;
		this.VoucherInfoPanel.Controls.Add(this.ShowHidButton);
		this.VoucherInfoPanel.Controls.Add(this.lblOffsetAccount);
		this.VoucherInfoPanel.Controls.Add(this.lblCreditAmount);
		this.VoucherInfoPanel.Controls.Add(this.lblAccountTiltle);
		this.VoucherInfoPanel.Controls.Add(this.lblDebitAmount);
		this.VoucherInfoPanel.Controls.Add(this.lblJobLot);
		this.VoucherInfoPanel.Location = new System.Drawing.Point(2, 3);
		this.VoucherInfoPanel.Margin = new System.Windows.Forms.Padding(0);
		this.VoucherInfoPanel.Name = "VoucherInfoPanel";
		this.VoucherInfoPanel.Size = new System.Drawing.Size(833, 30);
		this.VoucherInfoPanel.TabIndex = 2;
		this.ShowHidButton.BackColor = System.Drawing.Color.Teal;
		this.ShowHidButton.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ShowHidButton.ForeColor = System.Drawing.Color.White;
		this.ShowHidButton.Location = new System.Drawing.Point(3, 5);
		this.ShowHidButton.Name = "ShowHidButton";
		this.ShowHidButton.Size = new System.Drawing.Size(20, 20);
		this.ShowHidButton.TabIndex = 12;
		this.ShowHidButton.Text = "+";
		this.ShowHidButton.UseVisualStyleBackColor = false;
		this.ShowHidButton.Click += new System.EventHandler(ShowHidButton_Click);
		this.lblOffsetAccount.AutoEllipsis = true;
		this.lblOffsetAccount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOffsetAccount.ForeColor = System.Drawing.Color.RoyalBlue;
		this.lblOffsetAccount.Location = new System.Drawing.Point(308, 5);
		this.lblOffsetAccount.Name = "lblOffsetAccount";
		this.lblOffsetAccount.Size = new System.Drawing.Size(181, 20);
		this.lblOffsetAccount.TabIndex = 8;
		this.lblOffsetAccount.Text = "Offset Account";
		this.lblOffsetAccount.Click += new System.EventHandler(lblOffsetAccount_Click);
		this.lblCreditAmount.AutoEllipsis = true;
		this.lblCreditAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditAmount.ForeColor = System.Drawing.Color.Black;
		this.lblCreditAmount.Location = new System.Drawing.Point(726, 5);
		this.lblCreditAmount.Name = "lblCreditAmount";
		this.lblCreditAmount.Size = new System.Drawing.Size(102, 20);
		this.lblCreditAmount.TabIndex = 11;
		this.lblCreditAmount.Text = "121,121,121,12";
		this.lblAccountTiltle.AutoEllipsis = true;
		this.lblAccountTiltle.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccountTiltle.ForeColor = System.Drawing.Color.RoyalBlue;
		this.lblAccountTiltle.Location = new System.Drawing.Point(28, 5);
		this.lblAccountTiltle.Name = "lblAccountTiltle";
		this.lblAccountTiltle.Size = new System.Drawing.Size(276, 20);
		this.lblAccountTiltle.TabIndex = 7;
		this.lblAccountTiltle.Text = "Account Title";
		this.lblAccountTiltle.Click += new System.EventHandler(lblAccountTiltle_Click);
		this.lblDebitAmount.AutoEllipsis = true;
		this.lblDebitAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDebitAmount.ForeColor = System.Drawing.Color.Black;
		this.lblDebitAmount.Location = new System.Drawing.Point(619, 5);
		this.lblDebitAmount.Name = "lblDebitAmount";
		this.lblDebitAmount.Size = new System.Drawing.Size(102, 20);
		this.lblDebitAmount.TabIndex = 10;
		this.lblDebitAmount.Text = "121,121,121,12";
		this.lblJobLot.AutoEllipsis = true;
		this.lblJobLot.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJobLot.ForeColor = System.Drawing.Color.Black;
		this.lblJobLot.Location = new System.Drawing.Point(496, 5);
		this.lblJobLot.Name = "lblJobLot";
		this.lblJobLot.Size = new System.Drawing.Size(114, 20);
		this.lblJobLot.TabIndex = 9;
		this.lblJobLot.Text = "General";
		this.DetailPanel.Controls.Add(this.lblRemarks);
		this.DetailPanel.Controls.Add(this.label26);
		this.DetailPanel.Location = new System.Drawing.Point(25, 33);
		this.DetailPanel.Name = "DetailPanel";
		this.DetailPanel.Size = new System.Drawing.Size(793, 58);
		this.DetailPanel.TabIndex = 3;
		this.lblRemarks.AutoEllipsis = true;
		this.lblRemarks.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRemarks.ForeColor = System.Drawing.Color.Black;
		this.lblRemarks.Location = new System.Drawing.Point(63, 7);
		this.lblRemarks.Name = "lblRemarks";
		this.lblRemarks.Size = new System.Drawing.Size(727, 46);
		this.lblRemarks.TabIndex = 29;
		this.lblRemarks.Text = "Remarks";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Teal;
		this.label26.Location = new System.Drawing.Point(3, 7);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(59, 15);
		this.label26.TabIndex = 28;
		this.label26.Text = "Remarks:";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.DetailPanel);
		base.Controls.Add(this.VoucherInfoPanel);
		base.Name = "VoucherDetailInfoCard";
		base.Size = new System.Drawing.Size(834, 94);
		base.Load += new System.EventHandler(VoucherDetailInfoCard_Load);
		this.VoucherInfoPanel.ResumeLayout(false);
		this.DetailPanel.ResumeLayout(false);
		this.DetailPanel.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
