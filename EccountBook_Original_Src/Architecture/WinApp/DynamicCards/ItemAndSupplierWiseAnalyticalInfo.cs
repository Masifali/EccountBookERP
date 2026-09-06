using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.DynamicCards;

public class ItemAndSupplierWiseAnalyticalInfo : UserControl
{
	private string _ItemLable;

	private string _Qty;

	private string _Weight;

	private string _AvgRate;

	private string _Exp;

	private string _eavgRate;

	private string _branchName;

	private IContainer components;

	private Label lblitem;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label lblQty;

	private Label lblWeight;

	private Label lblavgrate;

	private Label lblexp40;

	private Label lbelavgrate;

	private Label txtBranch;

	public string Item
	{
		get
		{
			return _ItemLable;
		}
		set
		{
			_ItemLable = value;
			lblitem.Text = value;
		}
	}

	public string Qty
	{
		get
		{
			return _Qty;
		}
		set
		{
			_Qty = value;
			lblQty.Text = value;
		}
	}

	public string Weight
	{
		get
		{
			return _Weight;
		}
		set
		{
			_Weight = value;
			lblWeight.Text = value;
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
			lblavgrate.Text = value;
		}
	}

	public string Exp
	{
		get
		{
			return _Exp;
		}
		set
		{
			_Exp = value;
			lblexp40.Text = value;
		}
	}

	public string EAvgRate
	{
		get
		{
			return _eavgRate;
		}
		set
		{
			_eavgRate = value;
			lbelavgrate.Text = value;
		}
	}

	public string branchName
	{
		get
		{
			return _branchName;
		}
		set
		{
			_branchName = value;
			txtBranch.Text = value;
		}
	}

	public ItemAndSupplierWiseAnalyticalInfo()
	{
		InitializeComponent();
	}

	private void ItemAndSupplierWiseAnalyticalInfo_Load(object sender, EventArgs e)
	{
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
		this.lblitem = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.lblQty = new System.Windows.Forms.Label();
		this.lblWeight = new System.Windows.Forms.Label();
		this.lblavgrate = new System.Windows.Forms.Label();
		this.lblexp40 = new System.Windows.Forms.Label();
		this.lbelavgrate = new System.Windows.Forms.Label();
		this.txtBranch = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblitem.BackColor = System.Drawing.Color.Teal;
		this.lblitem.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblitem.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblitem.ForeColor = System.Drawing.Color.White;
		this.lblitem.Location = new System.Drawing.Point(0, 0);
		this.lblitem.Name = "lblitem";
		this.lblitem.Size = new System.Drawing.Size(203, 34);
		this.lblitem.TabIndex = 0;
		this.lblitem.Text = "Item";
		this.lblitem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(9, 38);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(26, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Qty";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(9, 57);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "Weight";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(9, 75);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(54, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "Avg Rate";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(9, 93);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(59, 15);
		this.label5.TabIndex = 4;
		this.label5.Text = "Exp/40Kg";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(9, 111);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(54, 15);
		this.label6.TabIndex = 5;
		this.label6.Text = "Avg Rate";
		this.lblQty.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblQty.Location = new System.Drawing.Point(82, 37);
		this.lblQty.Name = "lblQty";
		this.lblQty.Size = new System.Drawing.Size(113, 16);
		this.lblQty.TabIndex = 6;
		this.lblQty.Text = "0";
		this.lblQty.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblWeight.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWeight.Location = new System.Drawing.Point(82, 58);
		this.lblWeight.Name = "lblWeight";
		this.lblWeight.Size = new System.Drawing.Size(113, 13);
		this.lblWeight.TabIndex = 7;
		this.lblWeight.Text = "0";
		this.lblWeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblavgrate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblavgrate.Location = new System.Drawing.Point(82, 76);
		this.lblavgrate.Name = "lblavgrate";
		this.lblavgrate.Size = new System.Drawing.Size(113, 13);
		this.lblavgrate.TabIndex = 8;
		this.lblavgrate.Text = "0";
		this.lblavgrate.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblexp40.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblexp40.Location = new System.Drawing.Point(82, 94);
		this.lblexp40.Name = "lblexp40";
		this.lblexp40.Size = new System.Drawing.Size(113, 13);
		this.lblexp40.TabIndex = 9;
		this.lblexp40.Text = "0";
		this.lblexp40.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lbelavgrate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbelavgrate.Location = new System.Drawing.Point(82, 112);
		this.lbelavgrate.Name = "lbelavgrate";
		this.lbelavgrate.Size = new System.Drawing.Size(113, 13);
		this.lbelavgrate.TabIndex = 10;
		this.lbelavgrate.Text = "0";
		this.lbelavgrate.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtBranch.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranch.Location = new System.Drawing.Point(12, 125);
		this.txtBranch.Name = "txtBranch";
		this.txtBranch.Size = new System.Drawing.Size(183, 18);
		this.txtBranch.TabIndex = 11;
		this.txtBranch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.txtBranch);
		base.Controls.Add(this.lbelavgrate);
		base.Controls.Add(this.lblexp40);
		base.Controls.Add(this.lblavgrate);
		base.Controls.Add(this.lblWeight);
		base.Controls.Add(this.lblQty);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.lblitem);
		base.Name = "ItemAndSupplierWiseAnalyticalInfo";
		base.Size = new System.Drawing.Size(203, 145);
		base.Load += new System.EventHandler(ItemAndSupplierWiseAnalyticalInfo_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
