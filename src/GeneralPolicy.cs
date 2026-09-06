using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.HRM.PolicyManagment
{

public class GeneralPolicy : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private DropDownBind DDL = new DropDownBind();

	private bool updatemode;

	private IContainer components;

	private ToolStripButton btnnew;

	private ToolStrip tsItemType;

	private Panel panel1;

	private Panel panel11;

	private TextBox txtpayrollcutoffday;

	private Label label49;

	private Label label48;

	private GroupBox groupBox1;

	private CheckBox chkismonthendpayroll;

	private Panel panel3;

	private Label label1;

	private GroupBox groupBox2;

	private TextBox txtadvancelimitpercent;

	private Panel panel2;

	private Label label2;

	private Label label3;

	private TextBox txtadvanceafterday;

	private Label label4;

	private GroupBox groupBox5;

	private TextBox txtdeductionleavenoofhalfday;

	private Label label16;

	private TextBox txtdeductionleavenoofshortleave;

	private Panel panel6;

	private Label label13;

	private Label label14;

	private TextBox txtdeductionleavenofoflate;

	private Label label15;

	private GroupBox groupBox3;

	private TextBox txtmarkabsentaftermin;

	private Label label11;

	private TextBox txtnoofshorthoursinday;

	private Label label12;

	private TextBox txtnoofshortleaveinmonth;

	private Panel panel4;

	private Label label5;

	private Label label6;

	private TextBox txtmonthlyshorthourslimit;

	private Label label7;

	private GroupBox groupBox4;

	private Panel panel5;

	private Label label8;

	private CheckBox chkisseprateotpayroll;

	private Label label9;

	private TextBox txtotfactorrate;

	private Label label10;

	private GroupBox groupBox6;

	private CheckBox chkshowlatearrivalwithgracetime;

	private CheckBox chkisallowmultishiftinoneday;

	private Label label17;

	private Panel panel7;

	private Label label18;

	private Label label19;

	private TextBox txtovertimeapplyaftermin;

	private Label label20;

	private GroupBox groupBox7;

	private Panel panel8;

	private Label label21;

	private Label label22;

	private TextBox txtcplapplyminuteshours;

	private Label label24;

	private Label label23;

	private DateTimePicker txtdateto;

	private DateTimePicker txtdatefrom;

	internal UltraCombo cmbid;

	private Label label25;

	private ToolStripButton btnSave;

	private Label label26;

	private TextBox txtCplCalculateHours;

	private Panel panel9;

	private Label label27;

	private ToolStripButton btnRefresh;

	public GeneralPolicy(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void GetHeaderId()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtGetHeader = hrmPolicyHeader.Getall(new hrmPolicyHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGetHeader.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtGetHeader, cmbid, "PolicyHeaderId", "DateFrom", "DateFrom", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbbeniftprofile_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new hrmPolicyDetail();
			GetHeaderId();
			RecId = Conversion.ToInt(cmbid.Value);
			DataTable dt = hrmPolicyDetail.GetByID(RecId);
			if (dt.Rows.Count > 0)
			{
				txtpayrollcutoffday.Text = dt.Rows[0]["PayrollCutOffDay"].ToString();
				chkismonthendpayroll.Checked = Conversion.ToBool(dt.Rows[0]["IsMonthEndPayroll"]);
				txtadvanceafterday.Text = dt.Rows[0]["AdvanceAfterDay"].ToString();
				txtadvancelimitpercent.Text = dt.Rows[0]["AdvanceLimitPercent"].ToString();
				txtotfactorrate.Text = dt.Rows[0]["OTFactorRate"].ToString();
				chkisseprateotpayroll.Checked = Conversion.ToBool(dt.Rows[0]["IsSeparteOTPayroll"]);
				txtmonthlyshorthourslimit.Text = dt.Rows[0]["ShortNoOfHours"].ToString();
				txtnoofshortleaveinmonth.Text = dt.Rows[0]["NoOfShortLeaveInMonth"].ToString();
				txtnoofshorthoursinday.Text = dt.Rows[0]["NoOfShortHoursInDay"].ToString();
				txtdeductionleavenofoflate.Text = dt.Rows[0]["LateNoOfDay"].ToString();
				txtdeductionleavenoofshortleave.Text = dt.Rows[0]["NoOfShortLeave"].ToString();
				txtdeductionleavenoofhalfday.Text = dt.Rows[0]["NoOfHalfDay"].ToString();
				txtcplapplyminuteshours.Text = dt.Rows[0]["CPLApplyMinHours"].ToString();
				txtovertimeapplyaftermin.Text = dt.Rows[0]["OTApplyAfterMin"].ToString();
				chkisallowmultishiftinoneday.Checked = Conversion.ToBool(dt.Rows[0]["AllowMultiShiftToEmployeeOnDay"]);
				chkshowlatearrivalwithgracetime.Checked = Conversion.ToBool(dt.Rows[0]["LateArrivalWithGraceTime"]);
				btnSave.Text = "Update";
				updatemode = true;
			}
			else
			{
				MessageBox.Show("Record No Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneralPolicy_Load(object sender, EventArgs e)
	{
		GetHeaderId();
		cmbid.Focus();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		try
		{
			hrmPolicyHeader header = new hrmPolicyHeader();
			GetHeaderId();
			if (RecId > 0 && updatemode)
			{
				header.PolicyHeaderId = RecId;
			}
			header.OrganizationId = UserAccount.OrganizationId;
			header.CompanyId = UserAccount.CompanyId;
			header.DateFrom = Conversion.ToDateTime((object)txtdatefrom.Value);
			header.DateTo = Conversion.ToDateTime((object)txtdateto.Value);
			header.AlteredOn = Conversion.ToDateTime((object)DateTime.Now);
			header.AlteredBy = Conversion.ToInt((object)UserAccount.ID);
			header.CreatedOn = Conversion.ToDateTime((object)DateTime.Now);
			header.CreatedBy = Conversion.ToInt((object)UserAccount.ID);
			header.ActionType = 1;
			header.HrmPolicyDetailList = new List<hrmPolicyDetail>();
			hrmPolicyDetail cat = new hrmPolicyDetail();
			cat.PayrollCutOffDay = Conversion.ToInt((object)txtpayrollcutoffday.Text);
			cat.IsMonthEndPayroll = Conversion.ToBool((object)chkismonthendpayroll.Checked);
			cat.AdvanceAfterDay = Conversion.ToInt((object)txtadvanceafterday.Text);
			cat.AdvanceLimitPercent = Conversion.ToInt((object)txtadvancelimitpercent.Text);
			cat.OTFactorRate = Conversion.ToInt((object)txtotfactorrate.Text);
			cat.IsSeparteOTPayroll = Conversion.ToBool((object)chkisseprateotpayroll.Checked);
			cat.ShortNoOfHours = Conversion.ToInt((object)txtmonthlyshorthourslimit.Text);
			cat.NoOfShortLeaveInMonth = Conversion.ToInt((object)txtnoofshortleaveinmonth.Text);
			cat.NoOfShortHoursInDay = Conversion.ToInt((object)txtnoofshorthoursinday.Text);
			cat.LateNoOfDay = Conversion.ToInt((object)txtdeductionleavenofoflate.Text);
			cat.NoOfShortLeave = Conversion.ToInt((object)txtdeductionleavenoofshortleave.Text);
			cat.NoOfHalfDay = Conversion.ToInt((object)txtdeductionleavenoofhalfday.Text);
			cat.CPLApplyMinHours = Conversion.ToInt((object)txtcplapplyminuteshours.Text);
			cat.EmployeeDutyHour = Conversion.ToInt((object)txtCplCalculateHours.Text);
			cat.OTApplyAfterMin = Conversion.ToInt((object)txtovertimeapplyaftermin.Text);
			cat.AllowMultiShiftToEmployeeOnDay = Conversion.ToBool((object)chkisallowmultishiftinoneday.Checked);
			cat.LateArrivalWithGraceTime = Conversion.ToBool((object)chkshowlatearrivalwithgracetime.Checked);
			header.HrmPolicyDetailList.Add(cat);
			int success = hrmPolicyHeader.Save(header);
			if (RecId == 0 && !updatemode && success > 0)
			{
				MessageBox.Show("Save Successfully");
			}
			if (RecId > 0 && updatemode && success > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById()
	{
	}

	private void Reset()
	{
		try
		{
			RecId = 0;
			((Control)(object)cmbid).Text = string.Empty;
			txtdatefrom.Value = DateTime.Now;
			txtdateto.Value = DateTime.Now;
			txtpayrollcutoffday.Text = string.Empty;
			chkismonthendpayroll.Checked = false;
			chkisallowmultishiftinoneday.Checked = false;
			chkisseprateotpayroll.Checked = false;
			chkshowlatearrivalwithgracetime.Checked = false;
			txtadvanceafterday.Text = string.Empty;
			txtcplapplyminuteshours.Text = string.Empty;
			txtdeductionleavenofoflate.Text = string.Empty;
			txtdeductionleavenoofhalfday.Text = string.Empty;
			txtdeductionleavenoofshortleave.Text = string.Empty;
			txtmarkabsentaftermin.Text = string.Empty;
			txtmonthlyshorthourslimit.Text = string.Empty;
			txtnoofshorthoursinday.Text = string.Empty;
			txtnoofshortleaveinmonth.Text = string.Empty;
			txtotfactorrate.Text = string.Empty;
			txtovertimeapplyaftermin.Text = string.Empty;
			txtCplCalculateHours.Text = string.Empty;
			updatemode = false;
			btnSave.Text = "Save";
			txtdatefrom.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void GeneralPolicy_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && !updatemode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && updatemode)
			{
				btnsave_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			GetHeaderId();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PolicyManagment.GeneralPolicy));
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbid = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtCplCalculateHours = new System.Windows.Forms.TextBox();
		this.txtcplapplyminuteshours = new System.Windows.Forms.TextBox();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.chkshowlatearrivalwithgracetime = new System.Windows.Forms.CheckBox();
		this.chkisallowmultishiftinoneday = new System.Windows.Forms.CheckBox();
		this.label17 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtovertimeapplyaftermin = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.txtdeductionleavenoofhalfday = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtdeductionleavenoofshortleave = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtdeductionleavenofoflate = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtmarkabsentaftermin = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtnoofshorthoursinday = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtnoofshortleaveinmonth = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtmonthlyshorthourslimit = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtadvancelimitpercent = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtadvanceafterday = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.chkisseprateotpayroll = new System.Windows.Forms.CheckBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtotfactorrate = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.chkismonthendpayroll = new System.Windows.Forms.CheckBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtpayrollcutoffday = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tsItemType.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbid).BeginInit();
		this.groupBox7.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox6.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox5.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnSave, this.btnRefresh });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(871, 27);
		this.tsItemType.TabIndex = 1;
		this.tsItemType.Text = "toolStrip4";
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(65, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnsave_Click);
		this.panel1.Controls.Add(this.panel11);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(871, 634);
		this.panel1.TabIndex = 7;
		this.panel11.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.panel11.Controls.Add(this.panel9);
		this.panel11.Controls.Add(this.label25);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.cmbid);
		this.panel11.Controls.Add(this.label24);
		this.panel11.Controls.Add(this.label23);
		this.panel11.Controls.Add(this.txtdateto);
		this.panel11.Controls.Add(this.txtdatefrom);
		this.panel11.Controls.Add(this.groupBox7);
		this.panel11.Controls.Add(this.groupBox6);
		this.panel11.Controls.Add(this.groupBox5);
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Controls.Add(this.groupBox2);
		this.panel11.Controls.Add(this.groupBox4);
		this.panel11.Controls.Add(this.groupBox1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(4);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(871, 634);
		this.panel11.TabIndex = 8;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label27);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(871, 28);
		this.panel9.TabIndex = 15;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(5, 3);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(126, 19);
		this.label27.TabIndex = 6;
		this.label27.Text = "General Policy";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(11, 33);
		this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(73, 15);
		this.label25.TabIndex = 14;
		this.label25.Text = "Load Record";
		this.cmbid.AllowNull = (DefaultableBoolean)1;
		this.cmbid.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbid).Location = new System.Drawing.Point(14, 53);
		((System.Windows.Forms.Control)(object)this.cmbid).Name = "cmbid";
		((System.Windows.Forms.Control)(object)this.cmbid).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.cmbid).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.cmbid).Leave += new System.EventHandler(cmbbeniftprofile_Leave);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(324, 33);
		this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(48, 15);
		this.label24.TabIndex = 12;
		this.label24.Text = "Date To";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(217, 33);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(63, 15);
		this.label23.TabIndex = 6;
		this.label23.Text = "Date From";
		this.txtdateto.CustomFormat = "mm-dd-yyyy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtdateto.Location = new System.Drawing.Point(327, 53);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(106, 23);
		this.txtdateto.TabIndex = 1;
		this.txtdatefrom.CustomFormat = "mm-dd-yyyy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtdatefrom.Location = new System.Drawing.Point(220, 53);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(104, 23);
		this.txtdatefrom.TabIndex = 0;
		this.groupBox7.Controls.Add(this.panel8);
		this.groupBox7.Controls.Add(this.label26);
		this.groupBox7.Controls.Add(this.label22);
		this.groupBox7.Controls.Add(this.txtCplCalculateHours);
		this.groupBox7.Controls.Add(this.txtcplapplyminuteshours);
		this.groupBox7.Location = new System.Drawing.Point(12, 436);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(405, 102);
		this.groupBox7.TabIndex = 8;
		this.groupBox7.TabStop = false;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label21);
		this.panel8.Location = new System.Drawing.Point(0, 9);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(405, 28);
		this.panel8.TabIndex = 5;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(5, 3);
		this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(35, 17);
		this.label21.TabIndex = 6;
		this.label21.Text = "CPL";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(7, 73);
		this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(113, 15);
		this.label26.TabIndex = 0;
		this.label26.Text = "CPL Calculate Hours";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(7, 45);
		this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(140, 15);
		this.label22.TabIndex = 0;
		this.label22.Text = "CPL Apply minutes hours";
		this.txtCplCalculateHours.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCplCalculateHours.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCplCalculateHours.Location = new System.Drawing.Point(220, 71);
		this.txtCplCalculateHours.Margin = new System.Windows.Forms.Padding(4);
		this.txtCplCalculateHours.Name = "txtCplCalculateHours";
		this.txtCplCalculateHours.Size = new System.Drawing.Size(177, 23);
		this.txtCplCalculateHours.TabIndex = 0;
		this.txtcplapplyminuteshours.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcplapplyminuteshours.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcplapplyminuteshours.Location = new System.Drawing.Point(220, 43);
		this.txtcplapplyminuteshours.Margin = new System.Windows.Forms.Padding(4);
		this.txtcplapplyminuteshours.Name = "txtcplapplyminuteshours";
		this.txtcplapplyminuteshours.Size = new System.Drawing.Size(177, 23);
		this.txtcplapplyminuteshours.TabIndex = 0;
		this.groupBox6.Controls.Add(this.chkshowlatearrivalwithgracetime);
		this.groupBox6.Controls.Add(this.chkisallowmultishiftinoneday);
		this.groupBox6.Controls.Add(this.label17);
		this.groupBox6.Controls.Add(this.panel7);
		this.groupBox6.Controls.Add(this.label19);
		this.groupBox6.Controls.Add(this.txtovertimeapplyaftermin);
		this.groupBox6.Controls.Add(this.label20);
		this.groupBox6.Location = new System.Drawing.Point(447, 365);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(405, 135);
		this.groupBox6.TabIndex = 9;
		this.groupBox6.TabStop = false;
		this.chkshowlatearrivalwithgracetime.AutoSize = true;
		this.chkshowlatearrivalwithgracetime.Location = new System.Drawing.Point(222, 98);
		this.chkshowlatearrivalwithgracetime.Name = "chkshowlatearrivalwithgracetime";
		this.chkshowlatearrivalwithgracetime.Size = new System.Drawing.Size(15, 14);
		this.chkshowlatearrivalwithgracetime.TabIndex = 2;
		this.chkshowlatearrivalwithgracetime.UseVisualStyleBackColor = true;
		this.chkisallowmultishiftinoneday.AutoSize = true;
		this.chkisallowmultishiftinoneday.Location = new System.Drawing.Point(222, 74);
		this.chkisallowmultishiftinoneday.Name = "chkisallowmultishiftinoneday";
		this.chkisallowmultishiftinoneday.Size = new System.Drawing.Size(15, 14);
		this.chkisallowmultishiftinoneday.TabIndex = 1;
		this.chkisallowmultishiftinoneday.UseVisualStyleBackColor = true;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(7, 98);
		this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(191, 15);
		this.label17.TabIndex = 7;
		this.label17.Text = "Show Late Arrival With Grace Time";
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label18);
		this.panel7.Location = new System.Drawing.Point(0, 9);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(405, 28);
		this.panel7.TabIndex = 5;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(5, 3);
		this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(187, 17);
		this.label18.TabIndex = 6;
		this.label18.Text = "Attendance Configuration";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(7, 45);
		this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(139, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Overtime apply after min";
		this.txtovertimeapplyaftermin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtovertimeapplyaftermin.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtovertimeapplyaftermin.Location = new System.Drawing.Point(221, 43);
		this.txtovertimeapplyaftermin.Margin = new System.Windows.Forms.Padding(4);
		this.txtovertimeapplyaftermin.Name = "txtovertimeapplyaftermin";
		this.txtovertimeapplyaftermin.Size = new System.Drawing.Size(177, 23);
		this.txtovertimeapplyaftermin.TabIndex = 0;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(7, 70);
		this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(164, 15);
		this.label20.TabIndex = 1;
		this.label20.Text = "Is Allow multi shift in one day";
		this.groupBox5.Controls.Add(this.txtdeductionleavenoofhalfday);
		this.groupBox5.Controls.Add(this.label16);
		this.groupBox5.Controls.Add(this.txtdeductionleavenoofshortleave);
		this.groupBox5.Controls.Add(this.panel6);
		this.groupBox5.Controls.Add(this.label14);
		this.groupBox5.Controls.Add(this.txtdeductionleavenofoflate);
		this.groupBox5.Controls.Add(this.label15);
		this.groupBox5.Location = new System.Drawing.Point(13, 298);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(405, 135);
		this.groupBox5.TabIndex = 8;
		this.groupBox5.TabStop = false;
		this.txtdeductionleavenoofhalfday.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeductionleavenoofhalfday.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeductionleavenoofhalfday.Location = new System.Drawing.Point(221, 99);
		this.txtdeductionleavenoofhalfday.Margin = new System.Windows.Forms.Padding(4);
		this.txtdeductionleavenoofhalfday.Name = "txtdeductionleavenoofhalfday";
		this.txtdeductionleavenoofhalfday.Size = new System.Drawing.Size(177, 23);
		this.txtdeductionleavenoofhalfday.TabIndex = 2;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(7, 98);
		this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(178, 15);
		this.label16.TabIndex = 7;
		this.label16.Text = "Deduction Leave No of Half Day";
		this.txtdeductionleavenoofshortleave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeductionleavenoofshortleave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeductionleavenoofshortleave.Location = new System.Drawing.Point(221, 71);
		this.txtdeductionleavenoofshortleave.Margin = new System.Windows.Forms.Padding(4);
		this.txtdeductionleavenoofshortleave.Name = "txtdeductionleavenoofshortleave";
		this.txtdeductionleavenoofshortleave.Size = new System.Drawing.Size(177, 23);
		this.txtdeductionleavenoofshortleave.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label13);
		this.panel6.Location = new System.Drawing.Point(0, 9);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(405, 28);
		this.panel6.TabIndex = 5;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(5, 3);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(129, 17);
		this.label13.TabIndex = 6;
		this.label13.Text = "Deduction Option";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(7, 45);
		this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(154, 15);
		this.label14.TabIndex = 0;
		this.label14.Text = "Deduction Leave No of Late";
		this.txtdeductionleavenofoflate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeductionleavenofoflate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeductionleavenofoflate.Location = new System.Drawing.Point(221, 43);
		this.txtdeductionleavenofoflate.Margin = new System.Windows.Forms.Padding(4);
		this.txtdeductionleavenofoflate.Name = "txtdeductionleavenofoflate";
		this.txtdeductionleavenofoflate.Size = new System.Drawing.Size(177, 23);
		this.txtdeductionleavenofoflate.TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(7, 70);
		this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(189, 15);
		this.label15.TabIndex = 1;
		this.label15.Text = "Deduction Leave No of short leave";
		this.groupBox3.Controls.Add(this.txtmarkabsentaftermin);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.txtnoofshorthoursinday);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.txtnoofshortleaveinmonth);
		this.groupBox3.Controls.Add(this.panel4);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.txtmonthlyshorthourslimit);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Location = new System.Drawing.Point(447, 196);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(405, 158);
		this.groupBox3.TabIndex = 8;
		this.groupBox3.TabStop = false;
		this.txtmarkabsentaftermin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtmarkabsentaftermin.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtmarkabsentaftermin.Location = new System.Drawing.Point(221, 126);
		this.txtmarkabsentaftermin.Margin = new System.Windows.Forms.Padding(4);
		this.txtmarkabsentaftermin.Name = "txtmarkabsentaftermin";
		this.txtmarkabsentaftermin.Size = new System.Drawing.Size(177, 23);
		this.txtmarkabsentaftermin.TabIndex = 3;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(7, 100);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(137, 15);
		this.label11.TabIndex = 7;
		this.label11.Text = "No of short Hours in day";
		this.txtnoofshorthoursinday.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtnoofshorthoursinday.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtnoofshorthoursinday.Location = new System.Drawing.Point(221, 98);
		this.txtnoofshorthoursinday.Margin = new System.Windows.Forms.Padding(4);
		this.txtnoofshorthoursinday.Name = "txtnoofshorthoursinday";
		this.txtnoofshorthoursinday.Size = new System.Drawing.Size(177, 23);
		this.txtnoofshorthoursinday.TabIndex = 2;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 128);
		this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(127, 15);
		this.label12.TabIndex = 9;
		this.label12.Text = "Mark Absent After Min";
		this.txtnoofshortleaveinmonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtnoofshortleaveinmonth.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtnoofshortleaveinmonth.Location = new System.Drawing.Point(221, 70);
		this.txtnoofshortleaveinmonth.Margin = new System.Windows.Forms.Padding(4);
		this.txtnoofshortleaveinmonth.Name = "txtnoofshortleaveinmonth";
		this.txtnoofshortleaveinmonth.Size = new System.Drawing.Size(177, 23);
		this.txtnoofshortleaveinmonth.TabIndex = 1;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label5);
		this.panel4.Location = new System.Drawing.Point(0, 9);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(405, 28);
		this.panel4.TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 3);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(85, 17);
		this.label5.TabIndex = 6;
		this.label5.Text = "Late/Short";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 44);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(144, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Monthly Short Hour Limit";
		this.txtmonthlyshorthourslimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtmonthlyshorthourslimit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtmonthlyshorthourslimit.Location = new System.Drawing.Point(221, 42);
		this.txtmonthlyshorthourslimit.Margin = new System.Windows.Forms.Padding(4);
		this.txtmonthlyshorthourslimit.Name = "txtmonthlyshorthourslimit";
		this.txtmonthlyshorthourslimit.Size = new System.Drawing.Size(177, 23);
		this.txtmonthlyshorthourslimit.TabIndex = 0;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 72);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(151, 15);
		this.label7.TabIndex = 1;
		this.label7.Text = "No of Short leave in month";
		this.groupBox2.Controls.Add(this.txtadvancelimitpercent);
		this.groupBox2.Controls.Add(this.panel2);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtadvanceafterday);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Location = new System.Drawing.Point(447, 89);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(405, 104);
		this.groupBox2.TabIndex = 6;
		this.groupBox2.TabStop = false;
		this.txtadvancelimitpercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtadvancelimitpercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtadvancelimitpercent.Location = new System.Drawing.Point(221, 70);
		this.txtadvancelimitpercent.Margin = new System.Windows.Forms.Padding(4);
		this.txtadvancelimitpercent.Name = "txtadvancelimitpercent";
		this.txtadvancelimitpercent.Size = new System.Drawing.Size(177, 23);
		this.txtadvancelimitpercent.TabIndex = 1;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label2);
		this.panel2.Location = new System.Drawing.Point(0, 9);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(405, 28);
		this.panel2.TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 3);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 17);
		this.label2.TabIndex = 6;
		this.label2.Text = "Advance";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 43);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(106, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "Advance After Day";
		this.txtadvanceafterday.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtadvanceafterday.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtadvanceafterday.Location = new System.Drawing.Point(221, 41);
		this.txtadvanceafterday.Margin = new System.Windows.Forms.Padding(4);
		this.txtadvanceafterday.Name = "txtadvanceafterday";
		this.txtadvanceafterday.Size = new System.Drawing.Size(177, 23);
		this.txtadvanceafterday.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 72);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(126, 15);
		this.label4.TabIndex = 1;
		this.label4.Text = "Advance Limit Percent";
		this.groupBox4.Controls.Add(this.panel5);
		this.groupBox4.Controls.Add(this.chkisseprateotpayroll);
		this.groupBox4.Controls.Add(this.label9);
		this.groupBox4.Controls.Add(this.txtotfactorrate);
		this.groupBox4.Controls.Add(this.label10);
		this.groupBox4.Location = new System.Drawing.Point(13, 195);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(405, 98);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label8);
		this.panel5.Location = new System.Drawing.Point(0, 9);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(405, 28);
		this.panel5.TabIndex = 5;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(5, 3);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 17);
		this.label8.TabIndex = 6;
		this.label8.Text = "Over Time";
		this.chkisseprateotpayroll.AutoSize = true;
		this.chkisseprateotpayroll.Location = new System.Drawing.Point(220, 75);
		this.chkisseprateotpayroll.Name = "chkisseprateotpayroll";
		this.chkisseprateotpayroll.Size = new System.Drawing.Size(15, 14);
		this.chkisseprateotpayroll.TabIndex = 1;
		this.chkisseprateotpayroll.UseVisualStyleBackColor = true;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(7, 45);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(84, 15);
		this.label9.TabIndex = 0;
		this.label9.Text = "OT Factor Rate";
		this.txtotfactorrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtotfactorrate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtotfactorrate.Location = new System.Drawing.Point(220, 43);
		this.txtotfactorrate.Margin = new System.Windows.Forms.Padding(4);
		this.txtotfactorrate.Name = "txtotfactorrate";
		this.txtotfactorrate.Size = new System.Drawing.Size(177, 23);
		this.txtotfactorrate.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 75);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(116, 15);
		this.label10.TabIndex = 1;
		this.label10.Text = "Is Seprate OT Payroll";
		this.groupBox1.Controls.Add(this.panel3);
		this.groupBox1.Controls.Add(this.chkismonthendpayroll);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtpayrollcutoffday);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Location = new System.Drawing.Point(13, 89);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(405, 98);
		this.groupBox1.TabIndex = 3;
		this.groupBox1.TabStop = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label1);
		this.panel3.Location = new System.Drawing.Point(0, 9);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(405, 28);
		this.panel3.TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 3);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(58, 17);
		this.label1.TabIndex = 6;
		this.label1.Text = "PayRoll";
		this.chkismonthendpayroll.AutoSize = true;
		this.chkismonthendpayroll.Location = new System.Drawing.Point(221, 75);
		this.chkismonthendpayroll.Name = "chkismonthendpayroll";
		this.chkismonthendpayroll.Size = new System.Drawing.Size(15, 14);
		this.chkismonthendpayroll.TabIndex = 1;
		this.chkismonthendpayroll.UseVisualStyleBackColor = true;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(7, 45);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(105, 15);
		this.label48.TabIndex = 0;
		this.label48.Text = "Payroll CutOff Day";
		this.txtpayrollcutoffday.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpayrollcutoffday.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpayrollcutoffday.Location = new System.Drawing.Point(221, 43);
		this.txtpayrollcutoffday.Margin = new System.Windows.Forms.Padding(4);
		this.txtpayrollcutoffday.Name = "txtpayrollcutoffday";
		this.txtpayrollcutoffday.Size = new System.Drawing.Size(177, 23);
		this.txtpayrollcutoffday.TabIndex = 0;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(7, 75);
		this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(117, 15);
		this.label49.TabIndex = 1;
		this.label49.Text = "Is Month End Payroll";
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10.25f);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(78, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(871, 661);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tsItemType);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "GeneralPolicy";
		this.Text = "General Policy";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(GeneralPolicy_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GeneralPolicy_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbid).EndInit();
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
