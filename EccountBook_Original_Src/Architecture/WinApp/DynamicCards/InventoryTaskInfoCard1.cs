using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.DynamicCards;

public class InventoryTaskInfoCard1 : UserControl
{
	private int _ParentId;

	private int _TransId;

	private string _Key;

	private string _title;

	private string _VehicleNos;

	private string _Qty;

	private string _Weight;

	private IContainer components;

	private Label CardTitle;

	private Label label2;

	private Label label4;

	private Label label5;

	private Label lblVehicleNos;

	private Label lblQuantity;

	private Label lblWeight;

	private Panel panel1;

	private Label label1;

	private Label label3;

	private Label label6;

	public int ParentId
	{
		get
		{
			return _ParentId;
		}
		set
		{
			_ParentId = value;
		}
	}

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

	public string key
	{
		get
		{
			return _Key;
		}
		set
		{
			_Key = value;
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

	public string VehicleNos
	{
		get
		{
			return _VehicleNos;
		}
		set
		{
			_VehicleNos = value;
			lblVehicleNos.Text = value;
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
			lblQuantity.Text = value;
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

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public InventoryTaskInfoCard1()
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
			}
			else if (TransId == 2)
			{
				CardTitle.BackColor = Color.FromArgb(0, 169, 255);
				panel1.BackColor = Color.FromArgb(137, 207, 243);
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
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.lblVehicleNos = new System.Windows.Forms.Label();
		this.lblQuantity = new System.Windows.Forms.Label();
		this.lblWeight = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.CardTitle.BackColor = System.Drawing.Color.Gold;
		this.CardTitle.Cursor = System.Windows.Forms.Cursors.Hand;
		this.CardTitle.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CardTitle.ForeColor = System.Drawing.Color.Black;
		this.CardTitle.Location = new System.Drawing.Point(0, 0);
		this.CardTitle.Name = "CardTitle";
		this.CardTitle.Size = new System.Drawing.Size(192, 33);
		this.CardTitle.TabIndex = 0;
		this.CardTitle.Text = "Purchase Invoice";
		this.CardTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(55, 17);
		this.label2.TabIndex = 2;
		this.label2.Text = "Vehicles";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 54);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 17);
		this.label4.TabIndex = 4;
		this.label4.Text = "Weight";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 29);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(61, 17);
		this.label5.TabIndex = 5;
		this.label5.Text = "Quantity";
		this.lblVehicleNos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblVehicleNos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblVehicleNos.Location = new System.Drawing.Point(66, 3);
		this.lblVehicleNos.Name = "lblVehicleNos";
		this.lblVehicleNos.Size = new System.Drawing.Size(124, 18);
		this.lblVehicleNos.TabIndex = 6;
		this.lblVehicleNos.Text = "Value";
		this.lblVehicleNos.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblQuantity.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblQuantity.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblQuantity.Location = new System.Drawing.Point(66, 28);
		this.lblQuantity.Name = "lblQuantity";
		this.lblQuantity.Size = new System.Drawing.Size(124, 18);
		this.lblQuantity.TabIndex = 7;
		this.lblQuantity.Text = "Value";
		this.lblQuantity.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblWeight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblWeight.Location = new System.Drawing.Point(66, 53);
		this.lblWeight.Name = "lblWeight";
		this.lblWeight.Size = new System.Drawing.Size(124, 18);
		this.lblWeight.TabIndex = 8;
		this.lblWeight.Text = "Value";
		this.lblWeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.panel1.BackColor = System.Drawing.Color.FromArgb(255, 221, 149);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.lblQuantity);
		this.panel1.Controls.Add(this.lblVehicleNos);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.lblWeight);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Location = new System.Drawing.Point(1, 33);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(229, 73);
		this.panel1.TabIndex = 11;
		this.label6.BackColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(1, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(226, 1);
		this.label6.TabIndex = 11;
		this.label3.BackColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(3, 49);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(220, 1);
		this.label3.TabIndex = 10;
		this.label1.BackColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(3, 24);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(220, 1);
		this.label1.TabIndex = 9;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.CardTitle);
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InventoryTaskInfoCard1";
		base.Size = new System.Drawing.Size(193, 107);
		base.Load += new System.EventHandler(CompletePurchaseSaleDocs_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
