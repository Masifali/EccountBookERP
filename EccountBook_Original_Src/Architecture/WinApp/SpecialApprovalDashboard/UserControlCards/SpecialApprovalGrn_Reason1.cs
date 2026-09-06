using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.SpecialApprovalDashboard.UserControlCards;

public class SpecialApprovalGrn_Reason1 : UserControl
{
	private bool _IsSelected;

	private IContainer components;

	private Label label1;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label11;

	private Label label12;

	private Label label13;

	private Label label22;

	private Label label23;

	private Label label24;

	private Label label28;

	private Label label29;

	private Label label30;

	private Button btnApprove;

	private Panel panel1;

	private Label label31;

	private PictureBox pictureBox1;

	public CheckBox CbApprove;

	public Label txtVehicleNo;

	public Label datGrnDate;

	public Label txtGrnNo;

	public Label txtSuppCellNo;

	public Label txtSupplierName;

	public Label txtGpNo;

	public Label datGpDate;

	public Label txtSuppWhatsAppNo;

	public Label datPoDat;

	public Label txtPoNo;

	public Label txtDeliveryTerm;

	public Label txtItemName;

	public Label txtItemGrnWeight;

	public Label txtOrderBal;

	public Label txtCityName;

	public Label txtExcessWeight;

	public Label txtReason;

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

	public SpecialApprovalGrn_Reason1()
	{
		InitializeComponent();
		DoubleBuffered = true;
		ApplyRoundedCorners(this, 12);
		CbApprove.Click += ControlClicked;
		btnApprove.Click += ControlClicked;
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

	private void ApplyRoundedCorners(UserControl ctrl, int radius = 10)
	{
		int diameter = radius * 2;
		GraphicsPath path = new GraphicsPath();
		path.StartFigure();
		path.AddArc(0, 0, diameter, diameter, 180f, 90f);
		path.AddArc(ctrl.Width - diameter, 0, diameter, diameter, 270f, 90f);
		path.AddArc(ctrl.Width - diameter, ctrl.Height - diameter, diameter, diameter, 0f, 90f);
		path.AddArc(0, ctrl.Height - diameter, diameter, diameter, 90f, 90f);
		path.CloseFigure();
		ctrl.Region = new Region(path);
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		ApplyRoundedCorners(this, 12);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		int diameter = 12 * 2;
		GraphicsPath path = new GraphicsPath();
		path.AddArc(0, 0, diameter, diameter, 180f, 90f);
		path.AddArc(base.Width - diameter - 1, 0, diameter, diameter, 270f, 90f);
		path.AddArc(base.Width - diameter - 1, base.Height - diameter - 1, diameter, diameter, 0f, 90f);
		path.AddArc(0, base.Height - diameter - 1, diameter, diameter, 90f, 90f);
		path.CloseFigure();
		using Pen pen = new Pen(Color.Gray, 1f);
		e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
		e.Graphics.DrawPath(pen, path);
	}

	private void txtGrnNo_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GrnSlipWithSubReports(Conversion.ToInt(txtGrnNo.Tag));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGpNo_Click(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(txtGpNo.Tag);
			if (Id > 0)
			{
				CommonServices.InwardGatePassWithWbAndLabSlip(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPoNo_Click(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(txtPoNo.Tag);
			if (Id > 0)
			{
				CommonServices.PurchaseOrderSlipReport203(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SpecialApprovalGrn_Reason1_Load(object sender, EventArgs e)
	{
	}

	private void btnApprove_Click(object sender, EventArgs e)
	{
		try
		{
			base.Tag = "ApproveBtnClicked";
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
		this.label1 = new System.Windows.Forms.Label();
		this.txtReason = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.Label();
		this.datGrnDate = new System.Windows.Forms.Label();
		this.txtGrnNo = new System.Windows.Forms.Label();
		this.txtSuppCellNo = new System.Windows.Forms.Label();
		this.txtSupplierName = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtGpNo = new System.Windows.Forms.Label();
		this.datGpDate = new System.Windows.Forms.Label();
		this.txtSuppWhatsAppNo = new System.Windows.Forms.Label();
		this.datPoDat = new System.Windows.Forms.Label();
		this.txtPoNo = new System.Windows.Forms.Label();
		this.txtDeliveryTerm = new System.Windows.Forms.Label();
		this.txtItemName = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtItemGrnWeight = new System.Windows.Forms.Label();
		this.txtOrderBal = new System.Windows.Forms.Label();
		this.txtCityName = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.btnApprove = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.txtExcessWeight = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.CbApprove = new System.Windows.Forms.CheckBox();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(32, 14);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(56, 17);
		this.label1.TabIndex = 0;
		this.label1.Text = "Reason:";
		this.txtReason.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReason.Location = new System.Drawing.Point(94, 14);
		this.txtReason.Name = "txtReason";
		this.txtReason.Size = new System.Drawing.Size(331, 38);
		this.txtReason.TabIndex = 1;
		this.txtReason.Text = "Weight Difference Between Supplier and Factory Exceeds Allowed Tolerance";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(31, 60);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 17);
		this.label3.TabIndex = 2;
		this.label3.Text = "GRN No";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(176, 60);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(68, 17);
		this.label4.TabIndex = 3;
		this.label4.Text = "GRN Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Teal;
		this.label5.Location = new System.Drawing.Point(325, 60);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(74, 17);
		this.label5.TabIndex = 4;
		this.label5.Text = "Vehicle No";
		this.txtVehicleNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(316, 79);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(93, 13);
		this.txtVehicleNo.TabIndex = 7;
		this.txtVehicleNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.datGrnDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datGrnDate.Location = new System.Drawing.Point(154, 79);
		this.datGrnDate.Name = "datGrnDate";
		this.datGrnDate.Size = new System.Drawing.Size(112, 13);
		this.datGrnDate.TabIndex = 6;
		this.datGrnDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtGrnNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrnNo.ForeColor = System.Drawing.Color.Blue;
		this.txtGrnNo.Location = new System.Drawing.Point(9, 79);
		this.txtGrnNo.Name = "txtGrnNo";
		this.txtGrnNo.Size = new System.Drawing.Size(101, 17);
		this.txtGrnNo.TabIndex = 5;
		this.txtGrnNo.Text = "0";
		this.txtGrnNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtGrnNo.Click += new System.EventHandler(txtGrnNo_Click);
		this.txtSuppCellNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSuppCellNo.Location = new System.Drawing.Point(315, 123);
		this.txtSuppCellNo.Name = "txtSuppCellNo";
		this.txtSuppCellNo.Size = new System.Drawing.Size(95, 13);
		this.txtSuppCellNo.TabIndex = 12;
		this.txtSuppCellNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtSupplierName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierName.Location = new System.Drawing.Point(137, 126);
		this.txtSupplierName.Name = "txtSupplierName";
		this.txtSupplierName.Size = new System.Drawing.Size(147, 42);
		this.txtSupplierName.TabIndex = 11;
		this.txtSupplierName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Teal;
		this.label11.Location = new System.Drawing.Point(296, 100);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(132, 17);
		this.label11.TabIndex = 10;
		this.label11.Text = "Cell && WhatsAppNo";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Teal;
		this.label12.Location = new System.Drawing.Point(161, 100);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(99, 17);
		this.label12.TabIndex = 9;
		this.label12.Text = "Supplier Name";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Teal;
		this.label13.Location = new System.Drawing.Point(14, 100);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(91, 17);
		this.label13.TabIndex = 8;
		this.label13.Text = "Gp No&& Date";
		this.txtGpNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNo.ForeColor = System.Drawing.Color.Blue;
		this.txtGpNo.Location = new System.Drawing.Point(9, 123);
		this.txtGpNo.Name = "txtGpNo";
		this.txtGpNo.Size = new System.Drawing.Size(101, 17);
		this.txtGpNo.TabIndex = 13;
		this.txtGpNo.Text = "0";
		this.txtGpNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtGpNo.Click += new System.EventHandler(txtGpNo_Click);
		this.datGpDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datGpDate.Location = new System.Drawing.Point(9, 143);
		this.datGpDate.Name = "datGpDate";
		this.datGpDate.Size = new System.Drawing.Size(101, 17);
		this.datGpDate.TabIndex = 16;
		this.datGpDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtSuppWhatsAppNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSuppWhatsAppNo.Location = new System.Drawing.Point(315, 143);
		this.txtSuppWhatsAppNo.Name = "txtSuppWhatsAppNo";
		this.txtSuppWhatsAppNo.Size = new System.Drawing.Size(95, 13);
		this.txtSuppWhatsAppNo.TabIndex = 15;
		this.txtSuppWhatsAppNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.datPoDat.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datPoDat.Location = new System.Drawing.Point(9, 212);
		this.datPoDat.Name = "datPoDat";
		this.datPoDat.Size = new System.Drawing.Size(101, 17);
		this.datPoDat.TabIndex = 24;
		this.datPoDat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtPoNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPoNo.ForeColor = System.Drawing.Color.Blue;
		this.txtPoNo.Location = new System.Drawing.Point(9, 191);
		this.txtPoNo.Name = "txtPoNo";
		this.txtPoNo.Size = new System.Drawing.Size(101, 17);
		this.txtPoNo.TabIndex = 22;
		this.txtPoNo.Text = "0";
		this.txtPoNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtPoNo.Click += new System.EventHandler(txtPoNo_Click);
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(287, 190);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.Size = new System.Drawing.Size(150, 50);
		this.txtDeliveryTerm.TabIndex = 21;
		this.txtDeliveryTerm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemName.Location = new System.Drawing.Point(135, 190);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(150, 50);
		this.txtItemName.TabIndex = 20;
		this.txtItemName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Teal;
		this.label22.Location = new System.Drawing.Point(315, 167);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(95, 17);
		this.label22.TabIndex = 19;
		this.label22.Text = "Delivery Term";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Teal;
		this.label23.Location = new System.Drawing.Point(172, 167);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(76, 17);
		this.label23.TabIndex = 18;
		this.label23.Text = "Item Name";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Teal;
		this.label24.Location = new System.Drawing.Point(12, 167);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(94, 17);
		this.label24.TabIndex = 17;
		this.label24.Text = "Po No && Date";
		this.txtItemGrnWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemGrnWeight.Location = new System.Drawing.Point(317, 264);
		this.txtItemGrnWeight.Name = "txtItemGrnWeight";
		this.txtItemGrnWeight.Size = new System.Drawing.Size(91, 13);
		this.txtItemGrnWeight.TabIndex = 30;
		this.txtItemGrnWeight.Text = "0";
		this.txtItemGrnWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtOrderBal.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderBal.Location = new System.Drawing.Point(171, 264);
		this.txtOrderBal.Name = "txtOrderBal";
		this.txtOrderBal.Size = new System.Drawing.Size(79, 13);
		this.txtOrderBal.TabIndex = 29;
		this.txtOrderBal.Text = "0";
		this.txtOrderBal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtCityName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCityName.Location = new System.Drawing.Point(4, 261);
		this.txtCityName.Name = "txtCityName";
		this.txtCityName.Size = new System.Drawing.Size(111, 35);
		this.txtCityName.TabIndex = 28;
		this.txtCityName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Teal;
		this.label28.Location = new System.Drawing.Point(304, 241);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(116, 17);
		this.label28.TabIndex = 27;
		this.label28.Text = "Item GRN Weight";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Teal;
		this.label29.Location = new System.Drawing.Point(153, 241);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(115, 17);
		this.label29.TabIndex = 26;
		this.label29.Text = "Order Bal Weight";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Teal;
		this.label30.Location = new System.Drawing.Point(23, 241);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(72, 17);
		this.label30.TabIndex = 25;
		this.label30.Text = "City Name";
		this.btnApprove.BackColor = System.Drawing.Color.Teal;
		this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApprove.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprove.ForeColor = System.Drawing.Color.White;
		this.btnApprove.Location = new System.Drawing.Point(170, 289);
		this.btnApprove.Name = "btnApprove";
		this.btnApprove.Size = new System.Drawing.Size(79, 32);
		this.btnApprove.TabIndex = 31;
		this.btnApprove.Text = "Approve";
		this.btnApprove.UseVisualStyleBackColor = false;
		this.btnApprove.Click += new System.EventHandler(btnApprove_Click);
		this.panel1.BackColor = System.Drawing.Color.FromArgb(255, 138, 138);
		this.panel1.Controls.Add(this.pictureBox1);
		this.panel1.Controls.Add(this.txtExcessWeight);
		this.panel1.Controls.Add(this.label31);
		this.panel1.Location = new System.Drawing.Point(293, 285);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(138, 42);
		this.panel1.TabIndex = 32;
		this.pictureBox1.Image = Architecture.WinApp.Properties.Resources.Warning;
		this.pictureBox1.Location = new System.Drawing.Point(2, 2);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(25, 25);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 33;
		this.pictureBox1.TabStop = false;
		this.txtExcessWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExcessWeight.Location = new System.Drawing.Point(31, 23);
		this.txtExcessWeight.Name = "txtExcessWeight";
		this.txtExcessWeight.Size = new System.Drawing.Size(73, 13);
		this.txtExcessWeight.TabIndex = 33;
		this.txtExcessWeight.Text = "0";
		this.txtExcessWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(25, 4);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(116, 17);
		this.label31.TabIndex = 33;
		this.label31.Text = "Po Excess Weight";
		this.CbApprove.AutoSize = true;
		this.CbApprove.Location = new System.Drawing.Point(11, 9);
		this.CbApprove.Name = "CbApprove";
		this.CbApprove.Size = new System.Drawing.Size(15, 14);
		this.CbApprove.TabIndex = 33;
		this.CbApprove.UseVisualStyleBackColor = true;
		this.CbApprove.CheckedChanged += new System.EventHandler(CbApprove_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.CbApprove);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.btnApprove);
		base.Controls.Add(this.txtItemGrnWeight);
		base.Controls.Add(this.txtOrderBal);
		base.Controls.Add(this.txtCityName);
		base.Controls.Add(this.label28);
		base.Controls.Add(this.label29);
		base.Controls.Add(this.label30);
		base.Controls.Add(this.datPoDat);
		base.Controls.Add(this.txtPoNo);
		base.Controls.Add(this.txtDeliveryTerm);
		base.Controls.Add(this.txtItemName);
		base.Controls.Add(this.label22);
		base.Controls.Add(this.label23);
		base.Controls.Add(this.label24);
		base.Controls.Add(this.datGpDate);
		base.Controls.Add(this.txtSuppWhatsAppNo);
		base.Controls.Add(this.txtGpNo);
		base.Controls.Add(this.txtSuppCellNo);
		base.Controls.Add(this.txtSupplierName);
		base.Controls.Add(this.label11);
		base.Controls.Add(this.label12);
		base.Controls.Add(this.label13);
		base.Controls.Add(this.txtVehicleNo);
		base.Controls.Add(this.datGrnDate);
		base.Controls.Add(this.txtGrnNo);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.txtReason);
		base.Controls.Add(this.label1);
		base.Name = "SpecialApprovalGrn_Reason1";
		base.Size = new System.Drawing.Size(439, 339);
		base.Load += new System.EventHandler(SpecialApprovalGrn_Reason1_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
