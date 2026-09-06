using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.ModernHistories;

public class VoucherInfoHeaderCard : UserControl
{
	private bool _IsSelected;

	private string _RequestFrom;

	private int _VoucherId;

	private string _VoucherNo;

	private string _VoucherDate;

	private string _VoucherType;

	private string _PartyName;

	private string _ApprovedOrNot;

	private string _VoucherAmount;

	private IContainer components;

	private Label lblVoucherAmount;

	private Label lblVoucherType;

	private Label lblApprovedOrNot;

	private Label ValuePartyName;

	private Label ValueVoucherDate;

	private Label ValueVoucherNumber;

	private Label lableOrder;

	private Label label30;

	public CheckBox CbApprove;

	public bool IsSelected
	{
		get
		{
			return _IsSelected;
		}
		set
		{
			_IsSelected = value;
		}
	}

	public string RequestFrom
	{
		get
		{
			return _RequestFrom;
		}
		set
		{
			_RequestFrom = value;
		}
	}

	public int VoucherId
	{
		get
		{
			return _VoucherId;
		}
		set
		{
			_VoucherId = value;
		}
	}

	public string VoucherNumber
	{
		get
		{
			return _VoucherNo;
		}
		set
		{
			_VoucherNo = value;
			ValueVoucherNumber.Text = value;
		}
	}

	public string VoucherDate
	{
		get
		{
			return _VoucherDate;
		}
		set
		{
			_VoucherDate = value;
			ValueVoucherDate.Text = value;
		}
	}

	public string VoucherType
	{
		get
		{
			return _VoucherType;
		}
		set
		{
			_VoucherType = value;
			lblVoucherType.Text = value;
		}
	}

	public string PartyName
	{
		get
		{
			return _PartyName;
		}
		set
		{
			_PartyName = value;
			ValuePartyName.Text = value;
		}
	}

	public string ApprovedOrNot
	{
		get
		{
			return _ApprovedOrNot;
		}
		set
		{
			_ApprovedOrNot = value;
			lblApprovedOrNot.Text = value;
		}
	}

	public string VoucherAmount
	{
		get
		{
			return _VoucherAmount;
		}
		set
		{
			_VoucherAmount = value;
			lblVoucherAmount.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public VoucherInfoHeaderCard()
	{
		InitializeComponent();
		base.Click += ControlClicked;
		ValuePartyName.Click += ControlClicked;
		ValueVoucherNumber.Click += ControlClicked;
		ValueVoucherDate.Click += ControlClicked;
		lblApprovedOrNot.Click += ControlClicked;
		lblVoucherType.Click += ControlClicked;
		lblVoucherAmount.Click += ControlClicked;
		CbApprove.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void CbApprove_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (CbApprove.Checked)
			{
				IsSelected = true;
			}
			else
			{
				IsSelected = false;
			}
			base.Tag = "FromCheckBox";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherInfoHeaderCard_Click(object sender, EventArgs e)
	{
		try
		{
			base.Tag = "ControlClick";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherInfoHeaderCard_Load(object sender, EventArgs e)
	{
		try
		{
			if (RequestFrom == "ApproveOrUnApprove Voucher" || RequestFrom == "Pending For Review")
			{
				CbApprove.Show();
			}
			else
			{
				CbApprove.Hide();
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
		this.lblVoucherAmount = new System.Windows.Forms.Label();
		this.lblVoucherType = new System.Windows.Forms.Label();
		this.lblApprovedOrNot = new System.Windows.Forms.Label();
		this.ValuePartyName = new System.Windows.Forms.Label();
		this.ValueVoucherDate = new System.Windows.Forms.Label();
		this.ValueVoucherNumber = new System.Windows.Forms.Label();
		this.lableOrder = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.CbApprove = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		this.lblVoucherAmount.AutoSize = true;
		this.lblVoucherAmount.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblVoucherAmount.Location = new System.Drawing.Point(191, 89);
		this.lblVoucherAmount.Name = "lblVoucherAmount";
		this.lblVoucherAmount.Size = new System.Drawing.Size(19, 21);
		this.lblVoucherAmount.TabIndex = 29;
		this.lblVoucherAmount.Text = "0";
		this.lblVoucherType.AutoSize = true;
		this.lblVoucherType.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblVoucherType.ForeColor = System.Drawing.Color.Teal;
		this.lblVoucherType.Location = new System.Drawing.Point(3, 90);
		this.lblVoucherType.Name = "lblVoucherType";
		this.lblVoucherType.Size = new System.Drawing.Size(41, 21);
		this.lblVoucherType.TabIndex = 28;
		this.lblVoucherType.Text = "BPV";
		this.lblApprovedOrNot.AutoSize = true;
		this.lblApprovedOrNot.BackColor = System.Drawing.Color.Red;
		this.lblApprovedOrNot.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedOrNot.ForeColor = System.Drawing.Color.White;
		this.lblApprovedOrNot.Location = new System.Drawing.Point(96, 69);
		this.lblApprovedOrNot.Name = "lblApprovedOrNot";
		this.lblApprovedOrNot.Size = new System.Drawing.Size(92, 17);
		this.lblApprovedOrNot.TabIndex = 27;
		this.lblApprovedOrNot.Text = "Not Approved";
		this.ValuePartyName.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValuePartyName.ForeColor = System.Drawing.Color.FromArgb(52, 58, 64);
		this.ValuePartyName.Location = new System.Drawing.Point(3, 24);
		this.ValuePartyName.Name = "ValuePartyName";
		this.ValuePartyName.Size = new System.Drawing.Size(292, 43);
		this.ValuePartyName.TabIndex = 26;
		this.ValuePartyName.Text = "Party Name";
		this.ValueVoucherDate.AutoSize = true;
		this.ValueVoucherDate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueVoucherDate.ForeColor = System.Drawing.Color.Black;
		this.ValueVoucherDate.Location = new System.Drawing.Point(231, 3);
		this.ValueVoucherDate.Name = "ValueVoucherDate";
		this.ValueVoucherDate.Size = new System.Drawing.Size(15, 17);
		this.ValueVoucherDate.TabIndex = 25;
		this.ValueVoucherDate.Text = "0";
		this.ValueVoucherNumber.AutoSize = true;
		this.ValueVoucherNumber.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueVoucherNumber.ForeColor = System.Drawing.Color.Black;
		this.ValueVoucherNumber.Location = new System.Drawing.Point(73, 3);
		this.ValueVoucherNumber.Name = "ValueVoucherNumber";
		this.ValueVoucherNumber.Size = new System.Drawing.Size(15, 17);
		this.ValueVoucherNumber.TabIndex = 24;
		this.ValueVoucherNumber.Text = "0";
		this.lableOrder.AutoSize = true;
		this.lableOrder.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lableOrder.ForeColor = System.Drawing.Color.Teal;
		this.lableOrder.Location = new System.Drawing.Point(190, 3);
		this.lableOrder.Name = "lableOrder";
		this.lableOrder.Size = new System.Drawing.Size(41, 17);
		this.lableOrder.TabIndex = 23;
		this.lableOrder.Text = "Date:";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Teal;
		this.label30.Location = new System.Drawing.Point(3, 3);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(70, 17);
		this.label30.TabIndex = 22;
		this.label30.Text = "Voucher #";
		this.CbApprove.AutoSize = true;
		this.CbApprove.Location = new System.Drawing.Point(299, 42);
		this.CbApprove.Name = "CbApprove";
		this.CbApprove.Size = new System.Drawing.Size(15, 14);
		this.CbApprove.TabIndex = 30;
		this.CbApprove.UseVisualStyleBackColor = true;
		this.CbApprove.CheckedChanged += new System.EventHandler(CbApprove_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		base.Controls.Add(this.CbApprove);
		base.Controls.Add(this.lblVoucherAmount);
		base.Controls.Add(this.lblVoucherType);
		base.Controls.Add(this.lblApprovedOrNot);
		base.Controls.Add(this.ValuePartyName);
		base.Controls.Add(this.ValueVoucherDate);
		base.Controls.Add(this.ValueVoucherNumber);
		base.Controls.Add(this.lableOrder);
		base.Controls.Add(this.label30);
		base.Name = "VoucherInfoHeaderCard";
		base.Size = new System.Drawing.Size(320, 115);
		base.Load += new System.EventHandler(VoucherInfoHeaderCard_Load);
		base.Click += new System.EventHandler(VoucherInfoHeaderCard_Click);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
