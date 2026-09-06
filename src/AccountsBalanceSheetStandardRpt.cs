// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Account_Reports
{

public class AccountsBalanceSheetStandardRpt : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnReset;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnBalanceSheetPrint;

	private ToolStrip miniToolStrip;

	private Panel panel3;

	private Button btnshow;

	private DateTimePicker txtToDate;

	private DateTimePicker txtfromdate;

	private Label label27;

	private Label label33;

	private Panel panel1;

	private Panel panel4;

	private Panel panel6;

	private Panel panel5;

	internal GridEX DataGridHistory1;

	internal GridEX DataGridHistory;

	private ToolStripButton btnProfitLoosePrint;

	private ToolStripButton btnProfitlossFormat2;

	private ToolStripButton btnBalanceSheetFormat2;

	private GroupBox groupBox1;

	private Panel panel7;

	private Label label1;

	public AccountsBalanceSheetStandardRpt(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void AccountsBalanceSheetStandardRpt_Load(object sender, EventArgs e)
	{
		try
		{
			txtfromdate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtfromdate.Focus();
			GridBind();
			GridBind1();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click_1(object sender, EventArgs e)
	{
		try
		{
			GridBind();
			GridBind1();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ToDate = Conversion.ToDateTime((object)txtToDate.Value)
			};
			DataTable dtb = new DataTable();
			dtb = VoucherReports.AccountsBalanceSheetStandardRpt(val);
			if (dtb.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Class_Name", typeof(string));
				dtGrid.Columns.Add("Ac1LevelCode", typeof(string));
				dtGrid.Columns.Add("Ac1LevelTitle", typeof(string));
				dtGrid.Columns.Add("Ac2LevelCode", typeof(string));
				dtGrid.Columns.Add("Ac2LevelTitle", typeof(string));
				dtGrid.Columns.Add("Ac3LevelCode", typeof(string));
				dtGrid.Columns.Add("Ac3LevelTitle", typeof(string));
				dtGrid.Columns.Add("Ac4LevelCode", typeof(string));
				dtGrid.Columns.Add("Ac4LevelTitle", typeof(string));
				dtGrid.Columns.Add("ClosingBalance", typeof(double));
				dtGrid.Columns.Add("Column1", typeof(string));
				dtGrid.Clear();
				for (int i = 0; i < dtb.Rows.Count; i++)
				{
					dtGrid.Rows.Add(Conversion.ToString(dtb.Rows[i]["Class_Name"]), Conversion.ToString(dtb.Rows[i]["Ac1LevelCode"]), Conversion.ToString(dtb.Rows[i]["Ac1LevelTitle"]), Conversion.ToString(dtb.Rows[i]["Ac2LevelCode"]), Conversion.ToString(dtb.Rows[i]["Ac2LevelTitle"]), Conversion.ToString(dtb.Rows[i]["Ac3LevelCode"]), Conversion.ToString(dtb.Rows[i]["Ac3LevelTitle"]), Conversion.ToString(dtb.Rows[i]["Ac4LevelCode"]), Conversion.ToString(dtb.Rows[i]["Ac4LevelTitle"]), Conversion.ToDouble(dtb.Rows[i]["ClosingBalance"]), Conversion.ToString(dtb.Rows[i]["Column1"]));
				}
				DataGridHistory.DataSource = dtGrid;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridBind1()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtfromdate.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value)
			};
			DataTable dt = new DataTable();
			dt = VoucherReports.AccountsProfitLoassStandard(val);
			if (dt.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Class_Name", typeof(string));
				dtGrid.Columns.Add("Ac1LevelCode", typeof(string));
				dtGrid.Columns.Add("Ac1LevelTitle", typeof(string));
				dtGrid.Columns.Add("Ac2LevelCode", typeof(string));
				dtGrid.Columns.Add("Ac2LevelTitle", typeof(string));
				dtGrid.Columns.Add("Ac3LevelCode", typeof(string));
				dtGrid.Columns.Add("Ac3LevelTitle", typeof(string));
				dtGrid.Columns.Add("Ac4LevelCode", typeof(string));
				dtGrid.Columns.Add("Ac4LevelTitle", typeof(string));
				dtGrid.Columns.Add("ClosingBalance", typeof(double));
				dtGrid.Columns.Add("Column1", typeof(string));
				dtGrid.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGrid.Rows.Add(Conversion.ToString(dt.Rows[i]["Class_Name"]), Conversion.ToString(dt.Rows[i]["Ac1LevelCode"]), Conversion.ToString(dt.Rows[i]["Ac1LevelTitle"]), Conversion.ToString(dt.Rows[i]["Ac2LevelCode"]), Conversion.ToString(dt.Rows[i]["Ac2LevelTitle"]), Conversion.ToString(dt.Rows[i]["Ac3LevelCode"]), Conversion.ToString(dt.Rows[i]["Ac3LevelTitle"]), Conversion.ToString(dt.Rows[i]["Ac4LevelCode"]), Conversion.ToString(dt.Rows[i]["Ac4LevelTitle"]), Conversion.ToDouble(dt.Rows[i]["ClosingBalance"]), Conversion.ToString(dt.Rows[i]["Column1"]));
				}
				DataGridHistory1.DataSource = dtGrid;
				DataGridHistory1.RetrieveStructure();
				Grid1Setting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["ClosingBalance"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ClosingBalance"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ClosingBalance"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ClosingBalance"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Grid1Setting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["ClosingBalance"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ClosingBalance"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ClosingBalance"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ClosingBalance"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Reset()
	{
		try
		{
			txtfromdate.Value = DateTime.Now;
			txtToDate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsBalanceSheetStandardRpt_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnProfitLoosePrint_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.AccountsProfitLoassStandard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtfromdate.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "152-AcRptAccountsProfitLoss.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBalanceSheetPrint_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.AccountsBalanceSheetStandardRpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ToDate = Conversion.ToDateTime((object)txtToDate.Value)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "151-AcRptAccountsBalanceSheetStandard.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnProfitlossFormat2_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.ProfitandLoss(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ToDate = Conversion.ToDateTime((object)txtToDate.Value)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "154-Profit&Loss.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBalanceSheetFormat2_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.AccountsBalanceSheetStandardFormatII(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FromDate = Conversion.ToDateTime((object)txtfromdate.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "153-BalanceSheetStatementRpt.rpt");
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		GridEXLayout DataGridHistory1_DesignTimeLayout = new GridEXLayout();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnReset = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnBalanceSheetPrint = new System.Windows.Forms.ToolStripButton();
		this.btnBalanceSheetFormat2 = new System.Windows.Forms.ToolStripButton();
		this.btnProfitLoosePrint = new System.Windows.Forms.ToolStripButton();
		this.btnProfitlossFormat2 = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtfromdate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.DataGridHistory1 = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory1).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		base.SuspendLayout();
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1049, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnReset, this.toolStripSeparator, this.btnBalanceSheetPrint, this.btnBalanceSheetFormat2, this.btnProfitLoosePrint, this.btnProfitlossFormat2 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1049, 27);
		this.toolStrip1.TabIndex = 6;
		this.toolStrip1.Text = "toolStrip1";
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(68, 24);
		this.btnReset.Text = "&Reset";
		this.btnReset.Click += new System.EventHandler(btnnew_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnBalanceSheetPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnBalanceSheetPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnBalanceSheetPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnBalanceSheetPrint.Name = "btnBalanceSheetPrint";
		this.btnBalanceSheetPrint.Size = new System.Drawing.Size(192, 24);
		this.btnBalanceSheetPrint.Text = "151-Balance Sheet Print";
		this.btnBalanceSheetPrint.Click += new System.EventHandler(btnBalanceSheetPrint_Click);
		this.btnBalanceSheetFormat2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnBalanceSheetFormat2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnBalanceSheetFormat2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnBalanceSheetFormat2.Name = "btnBalanceSheetFormat2";
		this.btnBalanceSheetFormat2.Size = new System.Drawing.Size(169, 24);
		this.btnBalanceSheetFormat2.Text = "153-BalanceSheet-II";
		this.btnBalanceSheetFormat2.Click += new System.EventHandler(btnBalanceSheetFormat2_Click);
		this.btnProfitLoosePrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnProfitLoosePrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnProfitLoosePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnProfitLoosePrint.Name = "btnProfitLoosePrint";
		this.btnProfitLoosePrint.Size = new System.Drawing.Size(143, 24);
		this.btnProfitLoosePrint.Text = "152-Profit Loss-I";
		this.btnProfitLoosePrint.Click += new System.EventHandler(btnProfitLoosePrint_Click);
		this.btnProfitlossFormat2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnProfitlossFormat2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnProfitlossFormat2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnProfitlossFormat2.Name = "btnProfitlossFormat2";
		this.btnProfitlossFormat2.Size = new System.Drawing.Size(148, 24);
		this.btnProfitlossFormat2.Text = "154-Profit Loss-II";
		this.btnProfitlossFormat2.Click += new System.EventHandler(btnProfitlossFormat2_Click);
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(126, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1049, 27);
		this.miniToolStrip.TabIndex = 6;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1049, 129);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtfromdate);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtToDate);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(12, 35);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(372, 80);
		this.groupBox1.TabIndex = 177;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.txtfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromdate.CustomFormat = "dd-MMM-yy";
		this.txtfromdate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtfromdate.Location = new System.Drawing.Point(26, 44);
		this.txtfromdate.Name = "txtfromdate";
		this.txtfromdate.Size = new System.Drawing.Size(115, 23);
		this.txtfromdate.TabIndex = 2;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(146, 23);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(54, 17);
		this.label33.TabIndex = 174;
		this.label33.Text = "To Date";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(267, 44);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 23);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click_1);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(26, 23);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(72, 17);
		this.label27.TabIndex = 175;
		this.label27.Text = "From Date";
		this.txtToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(146, 44);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(115, 23);
		this.txtToDate.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1049, 29);
		this.panel7.TabIndex = 176;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(195, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Account Balance Sheet";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1049, 655);
		this.panel1.TabIndex = 1;
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 154);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1049, 501);
		this.panel4.TabIndex = 1;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(698, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(351, 501);
		this.panel6.TabIndex = 1;
		this.DataGridHistory1.AllowDelete = (InheritableBoolean)1;
		DataGridHistory1_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory1.DesignTimeLayout = DataGridHistory1_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory1).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory1.FilterMode = (FilterMode)2;
		this.DataGridHistory1.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory1).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory1.GroupByBoxVisible = false;
		this.DataGridHistory1.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory1).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory1).Name = "DataGridHistory1";
		((ScrollableBase)this.DataGridHistory1).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory1).Size = new System.Drawing.Size(351, 501);
		((System.Windows.Forms.Control)(object)this.DataGridHistory1).TabIndex = 7;
		this.DataGridHistory1.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory1.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory1).VisualStyle = (VisualStyle)4;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(698, 501);
		this.panel5.TabIndex = 0;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(698, 501);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 7;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1049, 655);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "AccountsBalanceSheetStandardRpt";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(AccountsBalanceSheetStandardRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AccountsBalanceSheetStandardRpt_KeyDown);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory1).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		base.ResumeLayout(false);
	}
}

}
