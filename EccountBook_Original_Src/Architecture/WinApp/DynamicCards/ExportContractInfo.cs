using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.DynamicCards;

public class ExportContractInfo : UserControl
{
	private int _Id;

	private string _Description;

	private string _lable1;

	private string _lable2;

	private string _lable3;

	private string _lable4;

	private IContainer components;

	private Panel panel1;

	private Label lblDescription;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label lbl4;

	private Label lbl3;

	private Label lbl2;

	private Label lbl1;

	private Label label6;

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

	public string Description
	{
		get
		{
			return _Description;
		}
		set
		{
			_Description = value;
			lblDescription.Text = value;
		}
	}

	public string lable1
	{
		get
		{
			return _lable1;
		}
		set
		{
			_lable1 = value;
			lbl1.Text = value;
		}
	}

	public string lable2
	{
		get
		{
			return _lable2;
		}
		set
		{
			_lable2 = value;
			lbl2.Text = value;
		}
	}

	public string lable3
	{
		get
		{
			return _lable3;
		}
		set
		{
			_lable3 = value;
			lbl3.Text = value;
		}
	}

	public string lable4
	{
		get
		{
			return _lable4;
		}
		set
		{
			_lable4 = value;
			lbl4.Text = value;
		}
	}

	public ExportContractInfo()
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblDescription = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.lbl4 = new System.Windows.Forms.Label();
		this.lbl3 = new System.Windows.Forms.Label();
		this.lbl2 = new System.Windows.Forms.Label();
		this.lbl1 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.lblDescription);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(172, 36);
		this.panel1.TabIndex = 7;
		this.lblDescription.BackColor = System.Drawing.Color.White;
		this.lblDescription.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblDescription.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDescription.ForeColor = System.Drawing.Color.Black;
		this.lblDescription.Location = new System.Drawing.Point(0, 1);
		this.lblDescription.Name = "lblDescription";
		this.lblDescription.Size = new System.Drawing.Size(173, 35);
		this.lblDescription.TabIndex = 0;
		this.lblDescription.Text = "Expiry After 15 Days";
		this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 39);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(49, 17);
		this.label1.TabIndex = 8;
		this.label1.Text = "MTons";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 57);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(25, 17);
		this.label2.TabIndex = 9;
		this.label2.Text = "Fcl";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 76);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(28, 17);
		this.label3.TabIndex = 10;
		this.label3.Text = "Fcy";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(6, 95);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(73, 17);
		this.label4.TabIndex = 11;
		this.label4.Text = "% Of Total";
		this.lbl4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl4.ForeColor = System.Drawing.Color.Teal;
		this.lbl4.Location = new System.Drawing.Point(85, 95);
		this.lbl4.Name = "lbl4";
		this.lbl4.Size = new System.Drawing.Size(86, 17);
		this.lbl4.TabIndex = 15;
		this.lbl4.Text = "0";
		this.lbl4.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lbl3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl3.ForeColor = System.Drawing.Color.Teal;
		this.lbl3.Location = new System.Drawing.Point(56, 76);
		this.lbl3.Name = "lbl3";
		this.lbl3.Size = new System.Drawing.Size(115, 17);
		this.lbl3.TabIndex = 14;
		this.lbl3.Text = "0";
		this.lbl3.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lbl2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl2.ForeColor = System.Drawing.Color.Teal;
		this.lbl2.Location = new System.Drawing.Point(56, 57);
		this.lbl2.Name = "lbl2";
		this.lbl2.Size = new System.Drawing.Size(115, 17);
		this.lbl2.TabIndex = 13;
		this.lbl2.Text = "0";
		this.lbl2.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lbl1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl1.ForeColor = System.Drawing.Color.Teal;
		this.lbl1.Location = new System.Drawing.Point(56, 39);
		this.lbl1.Name = "lbl1";
		this.lbl1.Size = new System.Drawing.Size(115, 17);
		this.lbl1.TabIndex = 12;
		this.lbl1.Text = "0";
		this.lbl1.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label6.BackColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(0, 35);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(172, 1);
		this.label6.TabIndex = 16;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.label6);
		base.Controls.Add(this.lbl4);
		base.Controls.Add(this.lbl3);
		base.Controls.Add(this.lbl2);
		base.Controls.Add(this.lbl1);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.panel1);
		this.Cursor = System.Windows.Forms.Cursors.Hand;
		base.Margin = new System.Windows.Forms.Padding(1);
		base.Name = "ExportContractInfo";
		base.Size = new System.Drawing.Size(172, 117);
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
