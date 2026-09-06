using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.SpecialApprovalDashboard.UserControlCards;

public class SpecialApprovalGrn_Reason2 : UserControl
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

	public Label txtQuantity;

	public Label txtFactoryWeight;

	public Label txtSupplierWeight;

	public Label txtExcessWeight;

	public Label txtReason;

	public Label txtWtCutTotal;

	public Label txtEbTotal;

	public Label txtGrossWeight;

	private Label label8;

	private Label label9;

	private Label label10;

	public Label txtStockWeight;

	public Label txtBillWeight;

	public Label txtAddLessWeight;

	private Label label17;

	private Label label18;

	private Label label19;

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

	public SpecialApprovalGrn_Reason2()
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
		this.txtQuantity = new System.Windows.Forms.Label();
		this.txtFactoryWeight = new System.Windows.Forms.Label();
		this.txtSupplierWeight = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.btnApprove = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.txtExcessWeight = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.CbApprove = new System.Windows.Forms.CheckBox();
		this.txtWtCutTotal = new System.Windows.Forms.Label();
		this.txtEbTotal = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.Label();
		this.txtBillWeight = new System.Windows.Forms.Label();
		this.txtAddLessWeight = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
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
		this.txtReason.Text = "Bill Weight Exceeds From sTock Weight";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(37, 60);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 17);
		this.label3.TabIndex = 2;
		this.label3.Text = "GRN No";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(178, 60);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(68, 17);
		this.label4.TabIndex = 3;
		this.label4.Text = "GRN Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Teal;
		this.label5.Location = new System.Drawing.Point(323, 60);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(74, 17);
		this.label5.TabIndex = 4;
		this.label5.Text = "Vehicle No";
		this.txtVehicleNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(314, 79);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(93, 13);
		this.txtVehicleNo.TabIndex = 7;
		this.txtVehicleNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.datGrnDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datGrnDate.Location = new System.Drawing.Point(156, 79);
		this.datGrnDate.Name = "datGrnDate";
		this.datGrnDate.Size = new System.Drawing.Size(112, 13);
		this.datGrnDate.TabIndex = 6;
		this.datGrnDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtGrnNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrnNo.ForeColor = System.Drawing.Color.Blue;
		this.txtGrnNo.Location = new System.Drawing.Point(15, 79);
		this.txtGrnNo.Name = "txtGrnNo";
		this.txtGrnNo.Size = new System.Drawing.Size(101, 17);
		this.txtGrnNo.TabIndex = 5;
		this.txtGrnNo.Text = "0";
		this.txtGrnNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtGrnNo.Click += new System.EventHandler(txtGrnNo_Click);
		this.txtSuppCellNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSuppCellNo.Location = new System.Drawing.Point(313, 123);
		this.txtSuppCellNo.Name = "txtSuppCellNo";
		this.txtSuppCellNo.Size = new System.Drawing.Size(95, 13);
		this.txtSuppCellNo.TabIndex = 12;
		this.txtSuppCellNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtSupplierName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierName.Location = new System.Drawing.Point(139, 126);
		this.txtSupplierName.Name = "txtSupplierName";
		this.txtSupplierName.Size = new System.Drawing.Size(147, 42);
		this.txtSupplierName.TabIndex = 11;
		this.txtSupplierName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Teal;
		this.label11.Location = new System.Drawing.Point(294, 100);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(132, 17);
		this.label11.TabIndex = 10;
		this.label11.Text = "Cell && WhatsAppNo";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Teal;
		this.label12.Location = new System.Drawing.Point(163, 100);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(99, 17);
		this.label12.TabIndex = 9;
		this.label12.Text = "Supplier Name";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Teal;
		this.label13.Location = new System.Drawing.Point(20, 100);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(91, 17);
		this.label13.TabIndex = 8;
		this.label13.Text = "Gp No&& Date";
		this.txtGpNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNo.ForeColor = System.Drawing.Color.Blue;
		this.txtGpNo.Location = new System.Drawing.Point(15, 123);
		this.txtGpNo.Name = "txtGpNo";
		this.txtGpNo.Size = new System.Drawing.Size(101, 17);
		this.txtGpNo.TabIndex = 13;
		this.txtGpNo.Text = "0";
		this.txtGpNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtGpNo.Click += new System.EventHandler(txtGpNo_Click);
		this.datGpDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datGpDate.Location = new System.Drawing.Point(15, 143);
		this.datGpDate.Name = "datGpDate";
		this.datGpDate.Size = new System.Drawing.Size(101, 17);
		this.datGpDate.TabIndex = 16;
		this.datGpDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtSuppWhatsAppNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSuppWhatsAppNo.Location = new System.Drawing.Point(313, 143);
		this.txtSuppWhatsAppNo.Name = "txtSuppWhatsAppNo";
		this.txtSuppWhatsAppNo.Size = new System.Drawing.Size(95, 13);
		this.txtSuppWhatsAppNo.TabIndex = 15;
		this.txtSuppWhatsAppNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.datPoDat.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datPoDat.Location = new System.Drawing.Point(15, 212);
		this.datPoDat.Name = "datPoDat";
		this.datPoDat.Size = new System.Drawing.Size(101, 17);
		this.datPoDat.TabIndex = 24;
		this.datPoDat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtPoNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPoNo.ForeColor = System.Drawing.Color.Blue;
		this.txtPoNo.Location = new System.Drawing.Point(15, 191);
		this.txtPoNo.Name = "txtPoNo";
		this.txtPoNo.Size = new System.Drawing.Size(101, 17);
		this.txtPoNo.TabIndex = 22;
		this.txtPoNo.Text = "0";
		this.txtPoNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtPoNo.Click += new System.EventHandler(txtPoNo_Click);
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(285, 190);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.Size = new System.Drawing.Size(150, 50);
		this.txtDeliveryTerm.TabIndex = 21;
		this.txtDeliveryTerm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemName.Location = new System.Drawing.Point(137, 190);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(150, 50);
		this.txtItemName.TabIndex = 20;
		this.txtItemName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Teal;
		this.label22.Location = new System.Drawing.Point(313, 167);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(95, 17);
		this.label22.TabIndex = 19;
		this.label22.Text = "Delivery Term";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Teal;
		this.label23.Location = new System.Drawing.Point(174, 167);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(76, 17);
		this.label23.TabIndex = 18;
		this.label23.Text = "Item Name";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Teal;
		this.label24.Location = new System.Drawing.Point(18, 167);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(94, 17);
		this.label24.TabIndex = 17;
		this.label24.Text = "Po No && Date";
		this.txtQuantity.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQuantity.Location = new System.Drawing.Point(315, 264);
		this.txtQuantity.Name = "txtQuantity";
		this.txtQuantity.Size = new System.Drawing.Size(91, 13);
		this.txtQuantity.TabIndex = 30;
		this.txtQuantity.Text = "0";
		this.txtQuantity.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryWeight.Location = new System.Drawing.Point(173, 264);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.Size = new System.Drawing.Size(79, 13);
		this.txtFactoryWeight.TabIndex = 29;
		this.txtFactoryWeight.Text = "0";
		this.txtFactoryWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtSupplierWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierWeight.Location = new System.Drawing.Point(10, 262);
		this.txtSupplierWeight.Name = "txtSupplierWeight";
		this.txtSupplierWeight.Size = new System.Drawing.Size(111, 16);
		this.txtSupplierWeight.TabIndex = 28;
		this.txtSupplierWeight.Text = "0";
		this.txtSupplierWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Teal;
		this.label28.Location = new System.Drawing.Point(329, 241);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(62, 17);
		this.label28.TabIndex = 27;
		this.label28.Text = "Quantity";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Teal;
		this.label29.Location = new System.Drawing.Point(161, 241);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(103, 17);
		this.label29.TabIndex = 26;
		this.label29.Text = "Factory Weight";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Teal;
		this.label30.Location = new System.Drawing.Point(11, 241);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(108, 17);
		this.label30.TabIndex = 25;
		this.label30.Text = "Supplier Weight";
		this.btnApprove.BackColor = System.Drawing.Color.Teal;
		this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApprove.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprove.ForeColor = System.Drawing.Color.White;
		this.btnApprove.Location = new System.Drawing.Point(170, 359);
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
		this.panel1.Location = new System.Drawing.Point(296, 355);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(123, 42);
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
		this.label31.Size = new System.Drawing.Size(96, 17);
		this.label31.TabIndex = 33;
		this.label31.Text = "Excess Weight";
		this.CbApprove.AutoSize = true;
		this.CbApprove.Location = new System.Drawing.Point(11, 9);
		this.CbApprove.Name = "CbApprove";
		this.CbApprove.Size = new System.Drawing.Size(15, 14);
		this.CbApprove.TabIndex = 33;
		this.CbApprove.UseVisualStyleBackColor = true;
		this.CbApprove.CheckedChanged += new System.EventHandler(CbApprove_CheckedChanged);
		this.txtWtCutTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWtCutTotal.Location = new System.Drawing.Point(315, 300);
		this.txtWtCutTotal.Name = "txtWtCutTotal";
		this.txtWtCutTotal.Size = new System.Drawing.Size(91, 13);
		this.txtWtCutTotal.TabIndex = 39;
		this.txtWtCutTotal.Text = "0";
		this.txtWtCutTotal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtEbTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEbTotal.Location = new System.Drawing.Point(173, 300);
		this.txtEbTotal.Name = "txtEbTotal";
		this.txtEbTotal.Size = new System.Drawing.Size(79, 13);
		this.txtEbTotal.TabIndex = 38;
		this.txtEbTotal.Text = "0";
		this.txtEbTotal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtGrossWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(10, 298);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(111, 16);
		this.txtGrossWeight.TabIndex = 37;
		this.txtGrossWeight.Text = "0";
		this.txtGrossWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Teal;
		this.label8.Location = new System.Drawing.Point(319, 279);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 17);
		this.label8.TabIndex = 36;
		this.label8.Text = "WtCut Total";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Teal;
		this.label9.Location = new System.Drawing.Point(183, 279);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(58, 17);
		this.label9.TabIndex = 35;
		this.label9.Text = "Eb Total";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Teal;
		this.label10.Location = new System.Drawing.Point(20, 279);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(91, 17);
		this.label10.TabIndex = 34;
		this.label10.Text = "Gross Weight";
		this.txtStockWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockWeight.Location = new System.Drawing.Point(315, 338);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.Size = new System.Drawing.Size(91, 13);
		this.txtStockWeight.TabIndex = 45;
		this.txtStockWeight.Text = "0";
		this.txtStockWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtBillWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeight.Location = new System.Drawing.Point(173, 338);
		this.txtBillWeight.Name = "txtBillWeight";
		this.txtBillWeight.Size = new System.Drawing.Size(79, 13);
		this.txtBillWeight.TabIndex = 44;
		this.txtBillWeight.Text = "0";
		this.txtBillWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtAddLessWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLessWeight.Location = new System.Drawing.Point(10, 336);
		this.txtAddLessWeight.Name = "txtAddLessWeight";
		this.txtAddLessWeight.Size = new System.Drawing.Size(111, 16);
		this.txtAddLessWeight.TabIndex = 43;
		this.txtAddLessWeight.Text = "0";
		this.txtAddLessWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Teal;
		this.label17.Location = new System.Drawing.Point(315, 315);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(90, 17);
		this.label17.TabIndex = 42;
		this.label17.Text = "Stock Weight";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Teal;
		this.label18.Location = new System.Drawing.Point(174, 315);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(77, 17);
		this.label18.TabIndex = 41;
		this.label18.Text = "Bill Weight";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Teal;
		this.label19.Location = new System.Drawing.Point(11, 315);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(108, 17);
		this.label19.TabIndex = 40;
		this.label19.Text = "AddLess Weight";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.txtStockWeight);
		base.Controls.Add(this.txtBillWeight);
		base.Controls.Add(this.txtAddLessWeight);
		base.Controls.Add(this.label17);
		base.Controls.Add(this.label18);
		base.Controls.Add(this.label19);
		base.Controls.Add(this.txtWtCutTotal);
		base.Controls.Add(this.txtEbTotal);
		base.Controls.Add(this.txtGrossWeight);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.label10);
		base.Controls.Add(this.CbApprove);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.btnApprove);
		base.Controls.Add(this.txtQuantity);
		base.Controls.Add(this.txtFactoryWeight);
		base.Controls.Add(this.txtSupplierWeight);
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
		base.Name = "SpecialApprovalGrn_Reason2";
		base.Size = new System.Drawing.Size(439, 406);
		base.Load += new System.EventHandler(SpecialApprovalGrn_Reason1_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
