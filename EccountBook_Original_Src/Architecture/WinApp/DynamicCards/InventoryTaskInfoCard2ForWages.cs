using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.DynamicCards;

public class InventoryTaskInfoCard2ForWages : UserControl
{
	private int _TransId;

	private int _DocumentTypeId;

	private string _title;

	private string _QtyTotal;

	private string _WeightTotal;

	private string _AvgRate;

	private string _AmountTotal;

	private IContainer components;

	private Label CardTitle;

	private Label label2;

	private Panel panel1;

	private Label label4;

	private Label label3;

	private Label label6;

	private Label label5;

	private Label lblQtyTotal;

	private Label label7;

	private Label lblWeightTotal;

	private Label lblAmountTotal;

	private Panel panel2;

	public FlowLayoutPanel ValuesFlowLayoutPanel;

	private Label label1;

	private Label label8;

	private Label label10;

	private Label lblAvgRate;

	public int TransId
	{
		get
		{
			return _TransId;
		}
		set
		{
			_TransId = value;
		}
	}

	public int DocumentTypeId
	{
		get
		{
			return _DocumentTypeId;
		}
		set
		{
			_DocumentTypeId = value;
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

	public string QtyTotal
	{
		get
		{
			return _QtyTotal;
		}
		set
		{
			_QtyTotal = value;
			lblQtyTotal.Text = value;
		}
	}

	public string WeightTotal
	{
		get
		{
			return _WeightTotal;
		}
		set
		{
			_WeightTotal = value;
			lblWeightTotal.Text = value;
		}
	}

	public string AvgRate
	{
		get
		{
			return _AvgRate;
		}
		set
		{
			_AvgRate = value;
			lblAvgRate.Text = value;
		}
	}

	public string AmountTotal
	{
		get
		{
			return _AmountTotal;
		}
		set
		{
			_AmountTotal = value;
			lblAmountTotal.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public InventoryTaskInfoCard2ForWages()
	{
		InitializeComponent();
		CardTitle.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void CompletePurchaseSaleDocs_Load(object sender, EventArgs e)
	{
		try
		{
			if (TransId == 1)
			{
				CardTitle.BackColor = Color.Gold;
				panel1.BackColor = Color.FromArgb(255, 221, 149);
				BackColor = Color.FromArgb(255, 221, 149);
			}
			else if (TransId == 2)
			{
				CardTitle.BackColor = Color.FromArgb(0, 169, 255);
				panel1.BackColor = Color.FromArgb(137, 207, 243);
				BackColor = Color.FromArgb(137, 207, 243);
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
		this.CardTitle = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.lblQtyTotal = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.lblWeightTotal = new System.Windows.Forms.Label();
		this.lblAmountTotal = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lblAvgRate = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.ValuesFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.CardTitle.BackColor = System.Drawing.Color.Gold;
		this.CardTitle.Cursor = System.Windows.Forms.Cursors.Hand;
		this.CardTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.CardTitle.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CardTitle.ForeColor = System.Drawing.Color.Black;
		this.CardTitle.Location = new System.Drawing.Point(0, 0);
		this.CardTitle.Name = "CardTitle";
		this.CardTitle.Size = new System.Drawing.Size(577, 24);
		this.CardTitle.TabIndex = 0;
		this.CardTitle.Text = "Purchase Invoice";
		this.CardTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label2.Location = new System.Drawing.Point(9, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 16);
		this.label2.TabIndex = 2;
		this.label2.Text = "Activity";
		this.panel1.BackColor = System.Drawing.Color.FromArgb(255, 221, 149);
		this.panel1.Controls.Add(this.label8);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 24);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(577, 26);
		this.panel1.TabIndex = 11;
		this.label8.BackColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(2, 25);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(690, 1);
		this.label8.TabIndex = 22;
		this.label8.Text = "label8";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label6.Location = new System.Drawing.Point(467, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(57, 16);
		this.label6.TabIndex = 13;
		this.label6.Text = "Amount";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label5.Location = new System.Drawing.Point(365, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 16);
		this.label5.TabIndex = 12;
		this.label5.Text = "Avg Rate";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label4.Location = new System.Drawing.Point(268, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 16);
		this.label4.TabIndex = 11;
		this.label4.Text = "Weight";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label3.Location = new System.Drawing.Point(175, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(31, 16);
		this.label3.TabIndex = 10;
		this.label3.Text = "Qty";
		this.lblQtyTotal.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblQtyTotal.Location = new System.Drawing.Point(144, 5);
		this.lblQtyTotal.Name = "lblQtyTotal";
		this.lblQtyTotal.Size = new System.Drawing.Size(94, 16);
		this.lblQtyTotal.TabIndex = 14;
		this.lblQtyTotal.Text = "0";
		this.lblQtyTotal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.label7.Location = new System.Drawing.Point(8, 5);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(40, 16);
		this.label7.TabIndex = 15;
		this.label7.Text = "Total";
		this.lblWeightTotal.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWeightTotal.Location = new System.Drawing.Point(248, 5);
		this.lblWeightTotal.Name = "lblWeightTotal";
		this.lblWeightTotal.Size = new System.Drawing.Size(100, 16);
		this.lblWeightTotal.TabIndex = 16;
		this.lblWeightTotal.Text = "0";
		this.lblWeightTotal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.lblAmountTotal.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmountTotal.Location = new System.Drawing.Point(435, 5);
		this.lblAmountTotal.Name = "lblAmountTotal";
		this.lblAmountTotal.Size = new System.Drawing.Size(124, 16);
		this.lblAmountTotal.TabIndex = 18;
		this.lblAmountTotal.Text = "0";
		this.lblAmountTotal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.panel2.Controls.Add(this.lblAvgRate);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.lblQtyTotal);
		this.panel2.Controls.Add(this.lblAmountTotal);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.lblWeightTotal);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(0, 77);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(577, 27);
		this.panel2.TabIndex = 19;
		this.lblAvgRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAvgRate.Location = new System.Drawing.Point(355, 6);
		this.lblAvgRate.Name = "lblAvgRate";
		this.lblAvgRate.Size = new System.Drawing.Size(74, 16);
		this.lblAvgRate.TabIndex = 24;
		this.lblAvgRate.Text = "0";
		this.lblAvgRate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label10.BackColor = System.Drawing.Color.Black;
		this.label10.Location = new System.Drawing.Point(2, 1);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(690, 1);
		this.label10.TabIndex = 22;
		this.label10.Text = "label10";
		this.ValuesFlowLayoutPanel.AutoScroll = true;
		this.ValuesFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ValuesFlowLayoutPanel.Location = new System.Drawing.Point(0, 50);
		this.ValuesFlowLayoutPanel.Name = "ValuesFlowLayoutPanel";
		this.ValuesFlowLayoutPanel.Size = new System.Drawing.Size(577, 27);
		this.ValuesFlowLayoutPanel.TabIndex = 20;
		this.label1.BackColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(2, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(690, 1);
		this.label1.TabIndex = 21;
		this.label1.Text = "label1";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(255, 221, 149);
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.label1);
		base.Controls.Add(this.ValuesFlowLayoutPanel);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.CardTitle);
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InventoryTaskInfoCard2ForWages";
		base.Size = new System.Drawing.Size(577, 104);
		base.Load += new System.EventHandler(CompletePurchaseSaleDocs_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
