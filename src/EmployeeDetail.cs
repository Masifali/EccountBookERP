using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Employee_Management
{

public class EmployeeDetail : Form
{
	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private TabPage tabPage5;

	private TabPage tabPage6;

	private TabPage tabPage7;

	private TabPage tabPage8;

	private ToolStrip toolStrip1;

	private ToolStripLabel toolStripLabel1;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private Panel panel3;

	private Label label5;

	private TextBox cmbAddressDetail;

	internal UltraCombo cmbProvince;

	private Label label4;

	internal UltraCombo cmbCity;

	private Label label2;

	internal UltraCombo cmbAddressType;

	private Label label1;

	internal UltraCombo cmbCountry;

	private Label label3;

	private Panel panel4;

	internal GridEX grdEmployeeRegistration;

	private Panel panel5;

	private ToolStrip toolStrip2;

	private ToolStripLabel toolStripLabel2;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripButton toolStripButton4;

	private Panel panel6;

	private Label label6;

	private TextBox txtOrganization;

	internal UltraCombo cmbDesignation;

	private Label label7;

	internal UltraCombo cmbCIty2;

	private Label label8;

	private DateTimePicker dtpTo;

	private DateTimePicker dtpFrom;

	private Label label10;

	private Label label9;

	private Label label11;

	private TextBox txtDescription;

	private Panel panel7;

	internal GridEX gridEX1;

	private Panel panel8;

	private Panel panel10;

	private Label label12;

	private TextBox textBox1;

	internal UltraCombo ultraCombo1;

	private Label label13;

	private Panel panel9;

	private ToolStrip toolStrip3;

	private ToolStripLabel toolStripLabel3;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton toolStripButton5;

	private ToolStripButton toolStripButton6;

	private ToolStripButton toolStripButton7;

	private ToolStripButton toolStripButton8;

	private Label label14;

	private TextBox textBox2;

	private Panel panel11;

	internal GridEX gridEX2;

	public EmployeeDetail()
	{
		InitializeComponent();
	}

	private void EmployeeDetail_Load(object sender, EventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Expected O, but got Unknown
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Expected O, but got Unknown
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Employee_Management.EmployeeDetail));
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdEmployeeRegistration = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbAddressDetail = new System.Windows.Forms.TextBox();
		this.cmbProvince = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbCity = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbAddressType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbCountry = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.gridEX1 = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.dtpTo = new System.Windows.Forms.DateTimePicker();
		this.dtpFrom = new System.Windows.Forms.DateTimePicker();
		this.cmbCIty2 = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtOrganization = new System.Windows.Forms.TextBox();
		this.cmbDesignation = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.gridEX2 = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.ultraCombo1 = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.tabPage7 = new System.Windows.Forms.TabPage();
		this.tabPage8 = new System.Windows.Forms.TabPage();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeRegistration).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbProvince).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAddressType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCountry).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridEX1).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCIty2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridEX2).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		this.panel9.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1064, 423);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Controls.Add(this.tabPage5);
		this.tabControl1.Controls.Add(this.tabPage6);
		this.tabControl1.Controls.Add(this.tabPage7);
		this.tabControl1.Controls.Add(this.tabPage8);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1064, 423);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1056, 397);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Address Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 103);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1046, 288);
		this.panel4.TabIndex = 2;
		this.grdEmployeeRegistration.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeRegistration.FilterMode = (FilterMode)2;
		this.grdEmployeeRegistration.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeRegistration.GroupByBoxVisible = false;
		this.grdEmployeeRegistration.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Name = "grdEmployeeRegistration";
		((ScrollableBase)this.grdEmployeeRegistration).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeRegistration).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeRegistration).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Size = new System.Drawing.Size(1046, 288);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).TabIndex = 3;
		this.grdEmployeeRegistration.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeRegistration.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeRegistration).VisualStyle = (VisualStyle)4;
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.cmbAddressDetail);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProvince);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCity);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAddressType);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCountry);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 31);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1046, 72);
		this.panel3.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(360, 44);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(97, 16);
		this.label5.TabIndex = 15;
		this.label5.Text = "Address Detail";
		this.cmbAddressDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAddressDetail.Location = new System.Drawing.Point(472, 40);
		this.cmbAddressDetail.Name = "cmbAddressDetail";
		this.cmbAddressDetail.Size = new System.Drawing.Size(568, 22);
		this.cmbAddressDetail.TabIndex = 14;
		this.cmbProvince.AllowNull = (DefaultableBoolean)1;
		this.cmbProvince.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProvince.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProvince.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbProvince).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProvince).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProvince).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProvince).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbProvince).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProvince).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbProvince).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProvince).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbProvince).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProvince).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProvince).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProvince).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProvince.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProvince).Location = new System.Drawing.Point(812, 6);
		((System.Windows.Forms.Control)(object)this.cmbProvince).Name = "cmbProvince";
		((System.Windows.Forms.Control)(object)this.cmbProvince).Size = new System.Drawing.Size(228, 25);
		((System.Windows.Forms.Control)(object)this.cmbProvince).TabIndex = 12;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(726, 10);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(61, 16);
		this.label4.TabIndex = 13;
		this.label4.Text = "Province";
		this.cmbCity.AllowNull = (DefaultableBoolean)1;
		this.cmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCity).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCity).Location = new System.Drawing.Point(118, 35);
		((System.Windows.Forms.Control)(object)this.cmbCity).Name = "cmbCity";
		((System.Windows.Forms.Control)(object)this.cmbCity).Size = new System.Drawing.Size(228, 25);
		((System.Windows.Forms.Control)(object)this.cmbCity).TabIndex = 10;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(11, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(30, 16);
		this.label2.TabIndex = 11;
		this.label2.Text = "City";
		this.cmbAddressType.AllowNull = (DefaultableBoolean)1;
		this.cmbAddressType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAddressType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAddressType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAddressType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbAddressType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAddressType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAddressType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAddressType).Location = new System.Drawing.Point(118, 6);
		((System.Windows.Forms.Control)(object)this.cmbAddressType).Name = "cmbAddressType";
		((System.Windows.Forms.Control)(object)this.cmbAddressType).Size = new System.Drawing.Size(228, 25);
		((System.Windows.Forms.Control)(object)this.cmbAddressType).TabIndex = 8;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(11, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(94, 16);
		this.label1.TabIndex = 9;
		this.label1.Text = "Address Type";
		this.cmbCountry.AllowNull = (DefaultableBoolean)1;
		this.cmbCountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbCountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbCountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCountry).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCountry).Location = new System.Drawing.Point(472, 6);
		((System.Windows.Forms.Control)(object)this.cmbCountry).Name = "cmbCountry";
		((System.Windows.Forms.Control)(object)this.cmbCountry).Size = new System.Drawing.Size(240, 25);
		((System.Windows.Forms.Control)(object)this.cmbCountry).TabIndex = 6;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(360, 10);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(53, 16);
		this.label3.TabIndex = 7;
		this.label3.Text = "Country";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1046, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripLabel1, this.toolStripSeparator1, this.btnnew, this.btnupdate, this.btnsave, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1046, 27);
		this.toolStrip1.TabIndex = 6;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(86, 24);
		this.toolStripLabel1.Text = "Address Detail";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(77, 24);
		this.btnupdate.Text = "&Update";
		this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(58, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1056, 397);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Experience";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.gridEX1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 135);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1050, 259);
		this.panel7.TabIndex = 2;
		this.gridEX1.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.gridEX1).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridEX1.FilterMode = (FilterMode)2;
		this.gridEX1.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridEX1).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridEX1.GroupByBoxVisible = false;
		this.gridEX1.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.gridEX1).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.gridEX1).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.gridEX1).Name = "gridEX1";
		((ScrollableBase)this.gridEX1).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridEX1).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.gridEX1).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridEX1).Size = new System.Drawing.Size(1050, 259);
		((System.Windows.Forms.Control)(object)this.gridEX1).TabIndex = 3;
		this.gridEX1.TotalRow = (InheritableBoolean)1;
		this.gridEX1.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridEX1).VisualStyle = (VisualStyle)4;
		this.panel6.Controls.Add(this.label11);
		this.panel6.Controls.Add(this.txtDescription);
		this.panel6.Controls.Add(this.label10);
		this.panel6.Controls.Add(this.label9);
		this.panel6.Controls.Add(this.dtpTo);
		this.panel6.Controls.Add(this.dtpFrom);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCIty2);
		this.panel6.Controls.Add(this.label8);
		this.panel6.Controls.Add(this.label6);
		this.panel6.Controls.Add(this.txtOrganization);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDesignation);
		this.panel6.Controls.Add(this.label7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 33);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1050, 102);
		this.panel6.TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(9, 72);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(76, 16);
		this.label11.TabIndex = 27;
		this.label11.Text = "Description";
		this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(99, 69);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(854, 22);
		this.txtDescription.TabIndex = 26;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(13, 47);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(39, 16);
		this.label10.TabIndex = 25;
		this.label10.Text = "From";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(345, 47);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(25, 16);
		this.label9.TabIndex = 24;
		this.label9.Text = "To";
		this.dtpTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpTo.Location = new System.Drawing.Point(431, 41);
		this.dtpTo.Name = "dtpTo";
		this.dtpTo.Size = new System.Drawing.Size(228, 22);
		this.dtpTo.TabIndex = 23;
		this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpFrom.Location = new System.Drawing.Point(99, 41);
		this.dtpFrom.Name = "dtpFrom";
		this.dtpFrom.Size = new System.Drawing.Size(228, 22);
		this.dtpFrom.TabIndex = 22;
		this.cmbCIty2.AllowNull = (DefaultableBoolean)1;
		this.cmbCIty2.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCIty2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCIty2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCIty2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbCIty2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCIty2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCIty2).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCIty2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCIty2).Location = new System.Drawing.Point(725, 10);
		((System.Windows.Forms.Control)(object)this.cmbCIty2).Name = "cmbCIty2";
		((System.Windows.Forms.Control)(object)this.cmbCIty2).Size = new System.Drawing.Size(228, 25);
		((System.Windows.Forms.Control)(object)this.cmbCIty2).TabIndex = 20;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(678, 14);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(30, 16);
		this.label8.TabIndex = 21;
		this.label8.Text = "City";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(341, 14);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(83, 16);
		this.label6.TabIndex = 19;
		this.label6.Text = "Organization";
		this.txtOrganization.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrganization.Location = new System.Drawing.Point(431, 11);
		this.txtOrganization.Name = "txtOrganization";
		this.txtOrganization.Size = new System.Drawing.Size(228, 22);
		this.txtOrganization.TabIndex = 18;
		this.cmbDesignation.AllowNull = (DefaultableBoolean)1;
		this.cmbDesignation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDesignation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDesignation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDesignation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Location = new System.Drawing.Point(99, 10);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Name = "cmbDesignation";
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Size = new System.Drawing.Size(228, 25);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).TabIndex = 16;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(10, 14);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(80, 16);
		this.label7.TabIndex = 17;
		this.label7.Text = "Designation";
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1050, 30);
		this.panel5.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripLabel2, this.toolStripSeparator2, this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.toolStripButton4 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1050, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripLabel2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel2.Name = "toolStripLabel2";
		this.toolStripLabel2.Size = new System.Drawing.Size(86, 24);
		this.toolStripLabel2.Text = "Address Detail";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(77, 24);
		this.toolStripButton2.Text = "&Update";
		this.toolStripButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(64, 24);
		this.toolStripButton3.Text = "&Save";
		this.toolStripButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(58, 24);
		this.toolStripButton4.Text = "&Print";
		this.toolStripButton4.Visible = false;
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1056, 397);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Education";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.tabPage4.Controls.Add(this.panel8);
		this.tabPage4.Location = new System.Drawing.Point(4, 22);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(1056, 397);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "Family";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1056, 397);
		this.panel8.TabIndex = 0;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.gridEX2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 102);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1056, 292);
		this.panel11.TabIndex = 2;
		this.gridEX2.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.gridEX2).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridEX2.FilterMode = (FilterMode)2;
		this.gridEX2.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridEX2).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridEX2.GroupByBoxVisible = false;
		this.gridEX2.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.gridEX2).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.gridEX2).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.gridEX2).Name = "gridEX2";
		((ScrollableBase)this.gridEX2).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridEX2).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.gridEX2).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridEX2).Size = new System.Drawing.Size(1056, 292);
		((System.Windows.Forms.Control)(object)this.gridEX2).TabIndex = 3;
		this.gridEX2.TotalRow = (InheritableBoolean)1;
		this.gridEX2.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridEX2).VisualStyle = (VisualStyle)4;
		this.panel10.Controls.Add(this.label14);
		this.panel10.Controls.Add(this.textBox2);
		this.panel10.Controls.Add(this.label12);
		this.panel10.Controls.Add(this.textBox1);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.ultraCombo1);
		this.panel10.Controls.Add(this.label13);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1056, 72);
		this.panel10.TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(6, 43);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(76, 16);
		this.label14.TabIndex = 33;
		this.label14.Text = "Description";
		this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox2.Location = new System.Drawing.Point(96, 40);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(531, 22);
		this.textBox2.TabIndex = 32;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(346, 16);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(45, 16);
		this.label12.TabIndex = 31;
		this.label12.Text = "Name";
		this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.Location = new System.Drawing.Point(399, 13);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(228, 22);
		this.textBox1.TabIndex = 30;
		this.ultraCombo1.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(96, 9);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(228, 25);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 28;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(7, 13);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(83, 16);
		this.label13.TabIndex = 29;
		this.label13.Text = "Relationship";
		this.panel9.Controls.Add(this.toolStrip3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1056, 30);
		this.panel9.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripLabel3, this.toolStripSeparator3, this.toolStripButton5, this.toolStripButton6, this.toolStripButton7, this.toolStripButton8 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1056, 27);
		this.toolStrip3.TabIndex = 8;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripLabel3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel3.Name = "toolStripLabel3";
		this.toolStripLabel3.Size = new System.Drawing.Size(86, 24);
		this.toolStripLabel3.Text = "Address Detail";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton5.Image = (System.Drawing.Image)resources.GetObject("toolStripButton5.Image");
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton5.Text = "&New";
		this.toolStripButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.Image = (System.Drawing.Image)resources.GetObject("toolStripButton6.Image");
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(77, 24);
		this.toolStripButton6.Text = "&Update";
		this.toolStripButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton7.Image = (System.Drawing.Image)resources.GetObject("toolStripButton7.Image");
		this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton7.Name = "toolStripButton7";
		this.toolStripButton7.Size = new System.Drawing.Size(64, 24);
		this.toolStripButton7.Text = "&Save";
		this.toolStripButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton8.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton8.Name = "toolStripButton8";
		this.toolStripButton8.Size = new System.Drawing.Size(58, 24);
		this.toolStripButton8.Text = "&Print";
		this.toolStripButton8.Visible = false;
		this.tabPage5.Location = new System.Drawing.Point(4, 22);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(1056, 397);
		this.tabPage5.TabIndex = 4;
		this.tabPage5.Text = "Reference";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.tabPage6.Location = new System.Drawing.Point(4, 22);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(1056, 397);
		this.tabPage6.TabIndex = 5;
		this.tabPage6.Text = "Bank Account";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.tabPage7.Location = new System.Drawing.Point(4, 22);
		this.tabPage7.Name = "tabPage7";
		this.tabPage7.Size = new System.Drawing.Size(1056, 397);
		this.tabPage7.TabIndex = 6;
		this.tabPage7.Text = "Speciality";
		this.tabPage7.UseVisualStyleBackColor = true;
		this.tabPage8.Location = new System.Drawing.Point(4, 22);
		this.tabPage8.Name = "tabPage8";
		this.tabPage8.Size = new System.Drawing.Size(1056, 397);
		this.tabPage8.TabIndex = 7;
		this.tabPage8.Text = "Signature";
		this.tabPage8.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1064, 423);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EmployeeDetail";
		this.Text = "Employee Detail";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(EmployeeDetail_Load);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeRegistration).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbProvince).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAddressType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCountry).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridEX1).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCIty2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridEX2).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
