using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.Account_Reports;

public class frmPayablesReports : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Label lblComparison;

	private TabPage tabPage4;

	private ToolStrip toolStrip2;

	private ToolStripButton btnShortcut;

	private Panel PanelForViewForm;

	private Button Btn1008PayablesAndReceivableForecast;

	private Button Btn1004ReceivablesAndReceiptsSchedule;

	private Button Btn1007ReceivablesByDueDatesNew;

	private Button Btn1003PayablesAndPaymentSchedule;

	private Button Btn1006ReceivablesAging;

	private Button Btn1002TradeDebitorsReport;

	private Button Btn1005PayablesAging;

	private Button Btn1001TradeCreditorsReport;

	private Button btnPayablesReport;

	private Button btnReceivablesReport;

	public frmPayablesReports(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void frmPayablesReports_Load(object sender, EventArgs e)
	{
	}

	private void frmPayablesReports_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPayablesReports_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
			{
				Btn1001TradeCreditorsReport_Click(null, null);
			}
			else if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
			{
				Btn1002TradeDebitorsReport_Click(null, null);
			}
			else if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3))
			{
				Btn1003PayablesAndPaymentSchedule_Click(null, null);
			}
			else if (e.Alt && (e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4))
			{
				Btn1004ReceivablesAndReceiptsSchedule_Click(null, null);
			}
			else if (e.Alt && (e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.D5))
			{
				Btn1005PayablesAging_Click(null, null);
			}
			else if (e.Alt && (e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.D6))
			{
				Btn1006ReceivablesAging_Click(null, null);
			}
			else if (e.Alt && (e.KeyCode == Keys.NumPad7 || e.KeyCode == Keys.D7))
			{
				Btn1007ReceivablesByDueDatesNew_Click(null, null);
			}
			else if (e.Alt && (e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.D8))
			{
				Btn1008PayablesAndReceivableForecast_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Alt+1", "1001 Trade Creditors Report");
			dt.Rows.Add("Alt+2", "1002 Trade Debtors Report");
			dt.Rows.Add("Alt+3", "1003 Payables & Payment Schedule");
			dt.Rows.Add("Alt+4", "1004 Receivables And Receipts Schedule");
			dt.Rows.Add("Alt+5", "1005 Payables Aging");
			dt.Rows.Add("Alt+6", "1006 Receivables Aging");
			dt.Rows.Add("Alt+7", "1007 Receivables By Due Dates New");
			dt.Rows.Add("Alt+8", "1008 Payables & Receivable Forecast");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcut_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Btn1001TradeCreditorsReport_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "TradeCreditorsReport";
			TradeDebitorsReport form = new TradeDebitorsReport(UserAccount);
			OpenReportForm(screenName, form);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Btn1002TradeDebitorsReport_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "TradeDebitorsReport";
			TradeDebitorsReport form = new TradeDebitorsReport(UserAccount);
			OpenReportForm(screenName, form);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Btn1003PayablesAndPaymentSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "PayablesAndPaymentSchedule";
			PayablesAndPaymentSchedule form = new PayablesAndPaymentSchedule(UserAccount);
			OpenReportForm(screenName, form);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Btn1004ReceivablesAndReceiptsSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "ReceivablesAndReceiptSchedule";
			ReceivablesAndReceiptSchedule form = new ReceivablesAndReceiptSchedule(UserAccount);
			OpenReportForm(screenName, form);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Btn1005PayablesAging_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "PayablesAging_New";
			PayablesAging_New form = new PayablesAging_New(UserAccount);
			OpenReportForm(screenName, form);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Btn1006ReceivablesAging_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "Receivables_New";
			Receivables_New form = new Receivables_New(UserAccount);
			OpenReportForm(screenName, form);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Btn1007ReceivablesByDueDatesNew_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "ReceivablesByDueDatesNew";
			ReceivablesByDueDatesNew form = new ReceivablesByDueDatesNew(UserAccount);
			OpenReportForm(screenName, form);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Btn1008PayablesAndReceivableForecast_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "DueDateAnalysisPayablesAndReceivablesForcast";
			DueDateAnalysisPayablesAndReceivablesForcast form = new DueDateAnalysisPayablesAndReceivablesForcast(UserAccount);
			OpenReportForm(screenName, form);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenReportForm(string screenName, Form formInstance)
	{
		try
		{
			Form existingForm = PanelForViewForm.Controls.OfType<Form>().FirstOrDefault((Form f) => f.Tag != null && f.Tag.ToString() == screenName);
			if (existingForm != null)
			{
				existingForm.BringToFront();
				return;
			}
			formInstance.Tag = screenName;
			formInstance.TopLevel = false;
			formInstance.AutoScroll = true;
			formInstance.WindowState = FormWindowState.Maximized;
			PanelForViewForm.Controls.Add(formInstance);
			formInstance.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReceivablesReport_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "Receivables";
			Receivables form = new Receivables(UserAccount);
			OpenReportForm(screenName, form);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPayablesReport_Click(object sender, EventArgs e)
	{
		try
		{
			string screenName = "Payables";
			Payables form = new Payables(UserAccount);
			OpenReportForm(screenName, form);
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.Btn1008PayablesAndReceivableForecast = new System.Windows.Forms.Button();
		this.Btn1004ReceivablesAndReceiptsSchedule = new System.Windows.Forms.Button();
		this.Btn1007ReceivablesByDueDatesNew = new System.Windows.Forms.Button();
		this.Btn1003PayablesAndPaymentSchedule = new System.Windows.Forms.Button();
		this.Btn1006ReceivablesAging = new System.Windows.Forms.Button();
		this.Btn1002TradeDebitorsReport = new System.Windows.Forms.Button();
		this.Btn1005PayablesAging = new System.Windows.Forms.Button();
		this.Btn1001TradeCreditorsReport = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnShortcut = new System.Windows.Forms.ToolStripButton();
		this.lblComparison = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.PanelForViewForm = new System.Windows.Forms.Panel();
		this.btnPayablesReport = new System.Windows.Forms.Button();
		this.btnReceivablesReport = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.btnPayablesReport);
		this.panel1.Controls.Add(this.btnReceivablesReport);
		this.panel1.Controls.Add(this.Btn1008PayablesAndReceivableForecast);
		this.panel1.Controls.Add(this.Btn1004ReceivablesAndReceiptsSchedule);
		this.panel1.Controls.Add(this.Btn1007ReceivablesByDueDatesNew);
		this.panel1.Controls.Add(this.Btn1003PayablesAndPaymentSchedule);
		this.panel1.Controls.Add(this.Btn1006ReceivablesAging);
		this.panel1.Controls.Add(this.Btn1002TradeDebitorsReport);
		this.panel1.Controls.Add(this.Btn1005PayablesAging);
		this.panel1.Controls.Add(this.Btn1001TradeCreditorsReport);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1179, 66);
		this.panel1.TabIndex = 0;
		this.Btn1008PayablesAndReceivableForecast.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.Btn1008PayablesAndReceivableForecast.FlatAppearance.BorderSize = 0;
		this.Btn1008PayablesAndReceivableForecast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Btn1008PayablesAndReceivableForecast.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Btn1008PayablesAndReceivableForecast.ForeColor = System.Drawing.Color.White;
		this.Btn1008PayablesAndReceivableForecast.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1008PayablesAndReceivableForecast.Location = new System.Drawing.Point(630, 36);
		this.Btn1008PayablesAndReceivableForecast.Margin = new System.Windows.Forms.Padding(2);
		this.Btn1008PayablesAndReceivableForecast.Name = "Btn1008PayablesAndReceivableForecast";
		this.Btn1008PayablesAndReceivableForecast.Size = new System.Drawing.Size(271, 26);
		this.Btn1008PayablesAndReceivableForecast.TabIndex = 7;
		this.Btn1008PayablesAndReceivableForecast.Text = "1008 Payables && Receivable Forecast";
		this.Btn1008PayablesAndReceivableForecast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1008PayablesAndReceivableForecast.UseVisualStyleBackColor = false;
		this.Btn1008PayablesAndReceivableForecast.Click += new System.EventHandler(Btn1008PayablesAndReceivableForecast_Click);
		this.Btn1004ReceivablesAndReceiptsSchedule.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.Btn1004ReceivablesAndReceiptsSchedule.FlatAppearance.BorderSize = 0;
		this.Btn1004ReceivablesAndReceiptsSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Btn1004ReceivablesAndReceiptsSchedule.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Btn1004ReceivablesAndReceiptsSchedule.ForeColor = System.Drawing.Color.White;
		this.Btn1004ReceivablesAndReceiptsSchedule.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1004ReceivablesAndReceiptsSchedule.Location = new System.Drawing.Point(173, 36);
		this.Btn1004ReceivablesAndReceiptsSchedule.Margin = new System.Windows.Forms.Padding(2);
		this.Btn1004ReceivablesAndReceiptsSchedule.Name = "Btn1004ReceivablesAndReceiptsSchedule";
		this.Btn1004ReceivablesAndReceiptsSchedule.Size = new System.Drawing.Size(276, 26);
		this.Btn1004ReceivablesAndReceiptsSchedule.TabIndex = 3;
		this.Btn1004ReceivablesAndReceiptsSchedule.Text = "1004 Receivables && Receipts Schedule";
		this.Btn1004ReceivablesAndReceiptsSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1004ReceivablesAndReceiptsSchedule.UseVisualStyleBackColor = false;
		this.Btn1004ReceivablesAndReceiptsSchedule.Click += new System.EventHandler(Btn1004ReceivablesAndReceiptsSchedule_Click);
		this.Btn1007ReceivablesByDueDatesNew.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.Btn1007ReceivablesByDueDatesNew.FlatAppearance.BorderSize = 0;
		this.Btn1007ReceivablesByDueDatesNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Btn1007ReceivablesByDueDatesNew.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Btn1007ReceivablesByDueDatesNew.ForeColor = System.Drawing.Color.White;
		this.Btn1007ReceivablesByDueDatesNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1007ReceivablesByDueDatesNew.Location = new System.Drawing.Point(630, 7);
		this.Btn1007ReceivablesByDueDatesNew.Margin = new System.Windows.Forms.Padding(2);
		this.Btn1007ReceivablesByDueDatesNew.Name = "Btn1007ReceivablesByDueDatesNew";
		this.Btn1007ReceivablesByDueDatesNew.Size = new System.Drawing.Size(271, 26);
		this.Btn1007ReceivablesByDueDatesNew.TabIndex = 6;
		this.Btn1007ReceivablesByDueDatesNew.Text = "1007 Receivables By Due Dates New";
		this.Btn1007ReceivablesByDueDatesNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1007ReceivablesByDueDatesNew.UseVisualStyleBackColor = false;
		this.Btn1007ReceivablesByDueDatesNew.Click += new System.EventHandler(Btn1007ReceivablesByDueDatesNew_Click);
		this.Btn1003PayablesAndPaymentSchedule.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.Btn1003PayablesAndPaymentSchedule.FlatAppearance.BorderSize = 0;
		this.Btn1003PayablesAndPaymentSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Btn1003PayablesAndPaymentSchedule.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Btn1003PayablesAndPaymentSchedule.ForeColor = System.Drawing.Color.White;
		this.Btn1003PayablesAndPaymentSchedule.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1003PayablesAndPaymentSchedule.Location = new System.Drawing.Point(173, 7);
		this.Btn1003PayablesAndPaymentSchedule.Margin = new System.Windows.Forms.Padding(2);
		this.Btn1003PayablesAndPaymentSchedule.Name = "Btn1003PayablesAndPaymentSchedule";
		this.Btn1003PayablesAndPaymentSchedule.Size = new System.Drawing.Size(276, 26);
		this.Btn1003PayablesAndPaymentSchedule.TabIndex = 2;
		this.Btn1003PayablesAndPaymentSchedule.Text = "1003 Payables && Payment Schedule";
		this.Btn1003PayablesAndPaymentSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1003PayablesAndPaymentSchedule.UseVisualStyleBackColor = false;
		this.Btn1003PayablesAndPaymentSchedule.Click += new System.EventHandler(Btn1003PayablesAndPaymentSchedule_Click);
		this.Btn1006ReceivablesAging.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.Btn1006ReceivablesAging.FlatAppearance.BorderSize = 0;
		this.Btn1006ReceivablesAging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Btn1006ReceivablesAging.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Btn1006ReceivablesAging.ForeColor = System.Drawing.Color.White;
		this.Btn1006ReceivablesAging.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1006ReceivablesAging.Location = new System.Drawing.Point(453, 36);
		this.Btn1006ReceivablesAging.Margin = new System.Windows.Forms.Padding(2);
		this.Btn1006ReceivablesAging.Name = "Btn1006ReceivablesAging";
		this.Btn1006ReceivablesAging.Size = new System.Drawing.Size(173, 26);
		this.Btn1006ReceivablesAging.TabIndex = 5;
		this.Btn1006ReceivablesAging.Text = "1006 Receivables Aging";
		this.Btn1006ReceivablesAging.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1006ReceivablesAging.UseVisualStyleBackColor = false;
		this.Btn1006ReceivablesAging.Click += new System.EventHandler(Btn1006ReceivablesAging_Click);
		this.Btn1002TradeDebitorsReport.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.Btn1002TradeDebitorsReport.FlatAppearance.BorderSize = 0;
		this.Btn1002TradeDebitorsReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Btn1002TradeDebitorsReport.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Btn1002TradeDebitorsReport.ForeColor = System.Drawing.Color.White;
		this.Btn1002TradeDebitorsReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1002TradeDebitorsReport.Location = new System.Drawing.Point(10, 36);
		this.Btn1002TradeDebitorsReport.Margin = new System.Windows.Forms.Padding(2);
		this.Btn1002TradeDebitorsReport.Name = "Btn1002TradeDebitorsReport";
		this.Btn1002TradeDebitorsReport.Size = new System.Drawing.Size(159, 26);
		this.Btn1002TradeDebitorsReport.TabIndex = 1;
		this.Btn1002TradeDebitorsReport.Text = "1002 Trade Debtors";
		this.Btn1002TradeDebitorsReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1002TradeDebitorsReport.UseVisualStyleBackColor = false;
		this.Btn1002TradeDebitorsReport.Click += new System.EventHandler(Btn1002TradeDebitorsReport_Click);
		this.Btn1005PayablesAging.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.Btn1005PayablesAging.FlatAppearance.BorderSize = 0;
		this.Btn1005PayablesAging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Btn1005PayablesAging.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Btn1005PayablesAging.ForeColor = System.Drawing.Color.White;
		this.Btn1005PayablesAging.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1005PayablesAging.Location = new System.Drawing.Point(453, 7);
		this.Btn1005PayablesAging.Margin = new System.Windows.Forms.Padding(2);
		this.Btn1005PayablesAging.Name = "Btn1005PayablesAging";
		this.Btn1005PayablesAging.Size = new System.Drawing.Size(173, 26);
		this.Btn1005PayablesAging.TabIndex = 4;
		this.Btn1005PayablesAging.Text = "1005 Payables Aging";
		this.Btn1005PayablesAging.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1005PayablesAging.UseVisualStyleBackColor = false;
		this.Btn1005PayablesAging.Click += new System.EventHandler(Btn1005PayablesAging_Click);
		this.Btn1001TradeCreditorsReport.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.Btn1001TradeCreditorsReport.FlatAppearance.BorderSize = 0;
		this.Btn1001TradeCreditorsReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Btn1001TradeCreditorsReport.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Btn1001TradeCreditorsReport.ForeColor = System.Drawing.Color.White;
		this.Btn1001TradeCreditorsReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1001TradeCreditorsReport.Location = new System.Drawing.Point(10, 7);
		this.Btn1001TradeCreditorsReport.Margin = new System.Windows.Forms.Padding(2);
		this.Btn1001TradeCreditorsReport.Name = "Btn1001TradeCreditorsReport";
		this.Btn1001TradeCreditorsReport.Size = new System.Drawing.Size(159, 26);
		this.Btn1001TradeCreditorsReport.TabIndex = 0;
		this.Btn1001TradeCreditorsReport.Text = "1001 Trade Creditors";
		this.Btn1001TradeCreditorsReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Btn1001TradeCreditorsReport.UseVisualStyleBackColor = false;
		this.Btn1001TradeCreditorsReport.Click += new System.EventHandler(Btn1001TradeCreditorsReport_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Controls.Add(this.lblComparison);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1179, 27);
		this.panel3.TabIndex = 1;
		this.toolStrip2.AutoSize = false;
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Right;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9f);
		this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnShortcut });
		this.toolStrip2.Location = new System.Drawing.Point(1039, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(140, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnShortcut.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnShortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcut.Name = "btnShortcut";
		this.btnShortcut.Size = new System.Drawing.Size(138, 24);
		this.btnShortcut.Text = "ShortCut Keys";
		this.btnShortcut.Click += new System.EventHandler(btnShortcut_Click);
		this.lblComparison.AutoSize = true;
		this.lblComparison.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblComparison.ForeColor = System.Drawing.Color.White;
		this.lblComparison.Location = new System.Drawing.Point(3, 4);
		this.lblComparison.Name = "lblComparison";
		this.lblComparison.Size = new System.Drawing.Size(271, 19);
		this.lblComparison.TabIndex = 0;
		this.lblComparison.Text = "Payables && Receivables Reports";
		this.tabPage4.Location = new System.Drawing.Point(0, 0);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(200, 100);
		this.tabPage4.TabIndex = 0;
		this.tabPage4.Text = "Supplier Wise Card View";
		this.PanelForViewForm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelForViewForm.Location = new System.Drawing.Point(0, 93);
		this.PanelForViewForm.Name = "PanelForViewForm";
		this.PanelForViewForm.Size = new System.Drawing.Size(1179, 371);
		this.PanelForViewForm.TabIndex = 1;
		this.btnPayablesReport.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnPayablesReport.FlatAppearance.BorderSize = 0;
		this.btnPayablesReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPayablesReport.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPayablesReport.ForeColor = System.Drawing.Color.White;
		this.btnPayablesReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPayablesReport.Location = new System.Drawing.Point(903, 35);
		this.btnPayablesReport.Margin = new System.Windows.Forms.Padding(2);
		this.btnPayablesReport.Name = "btnPayablesReport";
		this.btnPayablesReport.Size = new System.Drawing.Size(137, 26);
		this.btnPayablesReport.TabIndex = 9;
		this.btnPayablesReport.Text = "1010 Payables ";
		this.btnPayablesReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPayablesReport.UseVisualStyleBackColor = false;
		this.btnPayablesReport.Click += new System.EventHandler(btnPayablesReport_Click);
		this.btnReceivablesReport.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnReceivablesReport.FlatAppearance.BorderSize = 0;
		this.btnReceivablesReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReceivablesReport.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReceivablesReport.ForeColor = System.Drawing.Color.White;
		this.btnReceivablesReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnReceivablesReport.Location = new System.Drawing.Point(903, 6);
		this.btnReceivablesReport.Margin = new System.Windows.Forms.Padding(2);
		this.btnReceivablesReport.Name = "btnReceivablesReport";
		this.btnReceivablesReport.Size = new System.Drawing.Size(137, 26);
		this.btnReceivablesReport.TabIndex = 8;
		this.btnReceivablesReport.Text = "1009 Receivables ";
		this.btnReceivablesReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnReceivablesReport.UseVisualStyleBackColor = false;
		this.btnReceivablesReport.Click += new System.EventHandler(btnReceivablesReport_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1179, 464);
		base.Controls.Add(this.PanelForViewForm);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.KeyPreview = true;
		base.Name = "frmPayablesReports";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmPayablesReports_FormClosing);
		base.Load += new System.EventHandler(frmPayablesReports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPayablesReports_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
