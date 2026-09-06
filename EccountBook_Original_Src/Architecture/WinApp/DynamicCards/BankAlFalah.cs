using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.DynamicCards;

public class BankAlFalah : UserControl
{
	private int _Id;

	private int _ChequeTypeId;

	private string _CheqType;

	private string _ChequeNumber;

	private string _PayeeTitle;

	private string _AmountInWords;

	private string _Amount;

	private string _D1;

	private string _D2;

	private string _M1;

	private string _M2;

	private string _Y1;

	private string _Y2;

	private string _Y3;

	private string _Y4;

	private bool _IsSelected;

	public bool flagButtonClick;

	private IContainer components;

	private PictureBox pictureBox1;

	private Label lblChequeAmount;

	private Label lblPayeeTitle;

	private Label lblD1;

	private Label lblD2;

	private Label lblM1;

	private Label lblM2;

	private Label lblY3;

	private Label lblY2;

	private Label lblY1;

	private Label lblY4;

	public CheckBox cbSelect;

	public Button button1;

	private TextBox lblAmountinWords;

	private Label lblChequeNumber;

	private TextBox txtChequeType;

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

	public int ChequeTypeId
	{
		get
		{
			return _ChequeTypeId;
		}
		set
		{
			_ChequeTypeId = value;
		}
	}

	public string CheqType
	{
		get
		{
			return _CheqType;
		}
		set
		{
			_CheqType = value;
			txtChequeType.Text = value;
		}
	}

	public string ChequeNumber
	{
		get
		{
			return _ChequeNumber;
		}
		set
		{
			_ChequeNumber = value;
			lblChequeNumber.Text = value;
		}
	}

	public string PayeeTitle
	{
		get
		{
			return _PayeeTitle;
		}
		set
		{
			_PayeeTitle = value;
			lblPayeeTitle.Text = value;
		}
	}

	public string AmountInWords
	{
		get
		{
			return _AmountInWords;
		}
		set
		{
			_AmountInWords = value;
			lblAmountinWords.Text = value;
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
			lblChequeAmount.Text = value;
		}
	}

	public string D1
	{
		get
		{
			return _D1;
		}
		set
		{
			_D1 = value;
			lblD1.Text = value;
		}
	}

	public string D2
	{
		get
		{
			return _D2;
		}
		set
		{
			_D2 = value;
			lblD2.Text = value;
		}
	}

	public string M1
	{
		get
		{
			return _M1;
		}
		set
		{
			_M1 = value;
			lblM1.Text = value;
		}
	}

	public string M2
	{
		get
		{
			return _M2;
		}
		set
		{
			_M2 = value;
			lblM2.Text = value;
		}
	}

	public string Y1
	{
		get
		{
			return _Y1;
		}
		set
		{
			_Y1 = value;
			lblY1.Text = value;
		}
	}

	public string Y2
	{
		get
		{
			return _Y2;
		}
		set
		{
			_Y2 = value;
			lblY2.Text = value;
		}
	}

	public string Y3
	{
		get
		{
			return _Y3;
		}
		set
		{
			_Y3 = value;
			lblY3.Text = value;
		}
	}

	public string Y4
	{
		get
		{
			return _Y4;
		}
		set
		{
			_Y4 = value;
			lblY4.Text = value;
		}
	}

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

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public BankAlFalah()
	{
		InitializeComponent();
		cbSelect.Click += ControlClicked;
		button1.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void BankAlFalah_Load(object sender, EventArgs e)
	{
	}

	private void MoreInfo_Click(object sender, EventArgs e)
	{
	}

	private void label1_Click(object sender, EventArgs e)
	{
	}

	private void label1_Click_1(object sender, EventArgs e)
	{
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
	}

	private void cbSelect_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (cbSelect.Checked)
			{
				IsSelected = true;
			}
			else
			{
				IsSelected = false;
			}
			base.Tag = "FormCheckBox";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		base.Tag = "ButtonClicked";
		flagButtonClick = true;
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
		this.lblChequeAmount = new System.Windows.Forms.Label();
		this.lblPayeeTitle = new System.Windows.Forms.Label();
		this.lblD1 = new System.Windows.Forms.Label();
		this.lblD2 = new System.Windows.Forms.Label();
		this.lblM1 = new System.Windows.Forms.Label();
		this.lblM2 = new System.Windows.Forms.Label();
		this.lblY3 = new System.Windows.Forms.Label();
		this.lblY2 = new System.Windows.Forms.Label();
		this.lblY1 = new System.Windows.Forms.Label();
		this.lblY4 = new System.Windows.Forms.Label();
		this.cbSelect = new System.Windows.Forms.CheckBox();
		this.button1 = new System.Windows.Forms.Button();
		this.lblAmountinWords = new System.Windows.Forms.TextBox();
		this.lblChequeNumber = new System.Windows.Forms.Label();
		this.txtChequeType = new System.Windows.Forms.TextBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.lblChequeAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblChequeAmount.BackColor = System.Drawing.Color.FromArgb(247, 238, 229);
		this.lblChequeAmount.Font = new System.Drawing.Font("Verdana", 7f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblChequeAmount.Location = new System.Drawing.Point(302, 109);
		this.lblChequeAmount.Name = "lblChequeAmount";
		this.lblChequeAmount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		this.lblChequeAmount.Size = new System.Drawing.Size(82, 20);
		this.lblChequeAmount.TabIndex = 6;
		this.lblChequeAmount.Text = "00";
		this.lblChequeAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPayeeTitle.AutoSize = true;
		this.lblPayeeTitle.BackColor = System.Drawing.Color.FromArgb(247, 238, 229);
		this.lblPayeeTitle.Location = new System.Drawing.Point(36, 93);
		this.lblPayeeTitle.Name = "lblPayeeTitle";
		this.lblPayeeTitle.Size = new System.Drawing.Size(16, 13);
		this.lblPayeeTitle.TabIndex = 7;
		this.lblPayeeTitle.Text = "---";
		this.lblD1.AutoSize = true;
		this.lblD1.Location = new System.Drawing.Point(284, 67);
		this.lblD1.Name = "lblD1";
		this.lblD1.Size = new System.Drawing.Size(16, 13);
		this.lblD1.TabIndex = 8;
		this.lblD1.Text = "---";
		this.lblD1.Click += new System.EventHandler(label1_Click);
		this.lblD2.AutoSize = true;
		this.lblD2.Location = new System.Drawing.Point(301, 67);
		this.lblD2.Name = "lblD2";
		this.lblD2.Size = new System.Drawing.Size(16, 13);
		this.lblD2.TabIndex = 10;
		this.lblD2.Text = "---";
		this.lblD2.Click += new System.EventHandler(label1_Click_1);
		this.lblM1.AutoSize = true;
		this.lblM1.Location = new System.Drawing.Point(318, 67);
		this.lblM1.Name = "lblM1";
		this.lblM1.Size = new System.Drawing.Size(16, 13);
		this.lblM1.TabIndex = 11;
		this.lblM1.Text = "---";
		this.lblM2.AutoSize = true;
		this.lblM2.Location = new System.Drawing.Point(335, 67);
		this.lblM2.Name = "lblM2";
		this.lblM2.Size = new System.Drawing.Size(16, 13);
		this.lblM2.TabIndex = 12;
		this.lblM2.Text = "---";
		this.lblY3.AutoSize = true;
		this.lblY3.Location = new System.Drawing.Point(386, 67);
		this.lblY3.Name = "lblY3";
		this.lblY3.Size = new System.Drawing.Size(16, 13);
		this.lblY3.TabIndex = 15;
		this.lblY3.Text = "---";
		this.lblY2.AutoSize = true;
		this.lblY2.Location = new System.Drawing.Point(369, 67);
		this.lblY2.Name = "lblY2";
		this.lblY2.Size = new System.Drawing.Size(16, 13);
		this.lblY2.TabIndex = 14;
		this.lblY2.Text = "---";
		this.lblY1.AutoSize = true;
		this.lblY1.Location = new System.Drawing.Point(352, 67);
		this.lblY1.Name = "lblY1";
		this.lblY1.Size = new System.Drawing.Size(16, 13);
		this.lblY1.TabIndex = 13;
		this.lblY1.Text = "---";
		this.lblY4.AutoSize = true;
		this.lblY4.Location = new System.Drawing.Point(403, 68);
		this.lblY4.Name = "lblY4";
		this.lblY4.Size = new System.Drawing.Size(16, 13);
		this.lblY4.TabIndex = 17;
		this.lblY4.Text = "---";
		this.cbSelect.AutoSize = true;
		this.cbSelect.Location = new System.Drawing.Point(3, 236);
		this.cbSelect.Name = "cbSelect";
		this.cbSelect.Size = new System.Drawing.Size(56, 17);
		this.cbSelect.TabIndex = 18;
		this.cbSelect.Text = "Select";
		this.cbSelect.UseVisualStyleBackColor = true;
		this.cbSelect.CheckedChanged += new System.EventHandler(cbSelect_CheckedChanged);
		this.button1.Location = new System.Drawing.Point(347, 232);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 19;
		this.button1.Tag = "Print";
		this.button1.Text = "Print";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.lblAmountinWords.BackColor = System.Drawing.Color.FromArgb(247, 238, 229);
		this.lblAmountinWords.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lblAmountinWords.Location = new System.Drawing.Point(48, 113);
		this.lblAmountinWords.Multiline = true;
		this.lblAmountinWords.Name = "lblAmountinWords";
		this.lblAmountinWords.Size = new System.Drawing.Size(244, 40);
		this.lblAmountinWords.TabIndex = 20;
		this.lblChequeNumber.AutoSize = true;
		this.lblChequeNumber.BackColor = System.Drawing.Color.FromArgb(247, 238, 229);
		this.lblChequeNumber.Location = new System.Drawing.Point(323, 40);
		this.lblChequeNumber.Name = "lblChequeNumber";
		this.lblChequeNumber.Size = new System.Drawing.Size(16, 13);
		this.lblChequeNumber.TabIndex = 21;
		this.lblChequeNumber.Text = "---";
		this.txtChequeType.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.txtChequeType.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtChequeType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChequeType.ForeColor = System.Drawing.Color.White;
		this.txtChequeType.Location = new System.Drawing.Point(-1, 0);
		this.txtChequeType.Multiline = true;
		this.txtChequeType.Name = "txtChequeType";
		this.txtChequeType.Size = new System.Drawing.Size(431, 21);
		this.txtChequeType.TabIndex = 22;
		this.txtChequeType.Text = "Open";
		this.txtChequeType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.pictureBox1.BackColor = System.Drawing.Color.Teal;
		this.pictureBox1.Image = Architecture.WinApp.Properties.Resources.BankAlfalah_PK;
		this.pictureBox1.ImageLocation = "";
		this.pictureBox1.Location = new System.Drawing.Point(1, 21);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(429, 209);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 1;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.WhiteSmoke;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.txtChequeType);
		base.Controls.Add(this.lblChequeNumber);
		base.Controls.Add(this.lblAmountinWords);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.cbSelect);
		base.Controls.Add(this.lblY4);
		base.Controls.Add(this.lblY3);
		base.Controls.Add(this.lblY2);
		base.Controls.Add(this.lblY1);
		base.Controls.Add(this.lblM2);
		base.Controls.Add(this.lblM1);
		base.Controls.Add(this.lblD2);
		base.Controls.Add(this.lblD1);
		base.Controls.Add(this.lblPayeeTitle);
		base.Controls.Add(this.lblChequeAmount);
		base.Controls.Add(this.pictureBox1);
		base.Name = "BankAlFalah";
		base.Size = new System.Drawing.Size(428, 259);
		base.Load += new System.EventHandler(BankAlFalah_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
