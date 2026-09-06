using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.DynamicCards;

public class FcyPayablesReceivablesHeaderCard : UserControl
{
	private int _CardTypeId;

	private string _CardDescription;

	private string _TotalFcyAmount;

	private string _TotalLcyAmount;

	private IContainer components;

	private Label lblDescription;

	private Panel panel1;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	private Label lblTotalFcyAmount;

	private Label lblTotalLcyAmount;

	private Label label6;

	private Label label7;

	private Panel panel2;

	public FlowLayoutPanel flowLayoutPanel1;

	public int CardTypeId
	{
		get
		{
			return _CardTypeId;
		}
		set
		{
			_CardTypeId = value;
		}
	}

	public string CardDescription
	{
		get
		{
			return _CardDescription;
		}
		set
		{
			_CardDescription = value;
			lblDescription.Text = value;
		}
	}

	public string TotalFcyAmount
	{
		get
		{
			return _TotalFcyAmount;
		}
		set
		{
			_TotalFcyAmount = value;
			lblTotalFcyAmount.Text = value;
		}
	}

	public string TotalLcyAmount
	{
		get
		{
			return _TotalLcyAmount;
		}
		set
		{
			_TotalLcyAmount = value;
			lblTotalLcyAmount.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public FcyPayablesReceivablesHeaderCard()
	{
		InitializeComponent();
		lblDescription.Click += ControlClicked;
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
		this.lblDescription = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.lblTotalFcyAmount = new System.Windows.Forms.Label();
		this.lblTotalLcyAmount = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.lblDescription.BackColor = System.Drawing.Color.Teal;
		this.lblDescription.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblDescription.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblDescription.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDescription.ForeColor = System.Drawing.Color.White;
		this.lblDescription.Location = new System.Drawing.Point(0, 0);
		this.lblDescription.Name = "lblDescription";
		this.lblDescription.Size = new System.Drawing.Size(460, 33);
		this.lblDescription.TabIndex = 0;
		this.lblDescription.Text = "Fcy Payables ";
		this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 33);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(460, 25);
		this.panel1.TabIndex = 1;
		this.label1.BackColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(0, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(460, 1);
		this.label1.TabIndex = 11;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(374, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 17);
		this.label5.TabIndex = 3;
		this.label5.Text = "Exch Rate";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(260, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(81, 17);
		this.label4.TabIndex = 2;
		this.label4.Text = "Lcy Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(126, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 17);
		this.label3.TabIndex = 1;
		this.label3.Text = "Fcy Amount";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 17);
		this.label2.TabIndex = 0;
		this.label2.Text = "Fcy Code";
		this.lblTotalFcyAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalFcyAmount.Location = new System.Drawing.Point(87, 3);
		this.lblTotalFcyAmount.Name = "lblTotalFcyAmount";
		this.lblTotalFcyAmount.Size = new System.Drawing.Size(138, 18);
		this.lblTotalFcyAmount.TabIndex = 0;
		this.lblTotalFcyAmount.Text = "0";
		this.lblTotalFcyAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTotalLcyAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalLcyAmount.Location = new System.Drawing.Point(233, 3);
		this.lblTotalLcyAmount.Name = "lblTotalLcyAmount";
		this.lblTotalLcyAmount.Size = new System.Drawing.Size(138, 18);
		this.lblTotalLcyAmount.TabIndex = 1;
		this.lblTotalLcyAmount.Text = "0";
		this.lblTotalLcyAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label6.BackColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(-1, 2);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(460, 1);
		this.label6.TabIndex = 12;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 4);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(43, 17);
		this.label7.TabIndex = 12;
		this.label7.Text = "Totals";
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.lblTotalLcyAmount);
		this.panel2.Controls.Add(this.lblTotalFcyAmount);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(0, 177);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(460, 25);
		this.panel2.TabIndex = 2;
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 58);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(460, 119);
		this.flowLayoutPanel1.TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.flowLayoutPanel1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.lblDescription);
		base.Name = "FcyPayablesReceivablesHeaderCard";
		base.Size = new System.Drawing.Size(460, 202);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
