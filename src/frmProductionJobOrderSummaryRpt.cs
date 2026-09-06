// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Production_Reports
{

public class frmProductionJobOrderSummaryRpt : Form
{
	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGridFromDb = new DataTable();

	private DataTable dtGrid = new DataTable();

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool FormInitialized;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel5;

	private Label label27;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnRefresh;

	private Panel panel6;

	private Label label4;

	private BackgroundWorker backgroundWorker1;

	private ToolStripButton btnShortCutKeys;

	public DateTimePicker datFromDate;

	public DateTimePicker datToDate;

	private Label label13;

	private RadioButton rdAll;

	private RadioButton rdUnApproved;

	private RadioButton rdApproved;

	public frmProductionJobOrderSummaryRpt(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentMethod();
	}

	private void frmProductionJobOrderSummaryRpt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 600);
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("DocDate", typeof(DateTime));
			dtGrid.Columns.Add("JobOrderNo", typeof(string));
			dtGrid.Columns.Add("InputWeight", typeof(double));
			dtGrid.Columns.Add("InputAmount", typeof(double));
			dtGrid.Columns.Add("OutPutWeight", typeof(double));
			dtGrid.Columns.Add("OutPutAmount", typeof(double));
			dtGrid.Columns.Add("BalanceWeight", typeof(double));
			dtGrid.Columns.Add("BalanceAmount", typeof(double));
			dtGrid.Columns.Add("LedgerBalance", typeof(double));
			dtGrid.Columns.Add("SettlementStatus", typeof(string));
			dtGrid.Columns.Add("ApprovalStatus", typeof(string));
			dtGrid.Columns.Add("JobOrderStatus", typeof(string));
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(() => new { }).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
				datFromDate.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				datToDate.Value = DateTime.Now;
				datFromDate.Checked = false;
				FormInitialized = true;
				datFromDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void gridHisory()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtGridFromDb.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (datFromDate.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datFromDate.Value);
			}
			obj.ToDate = Conversion.ToDateTime((object)datToDate.Value);
			if (!rdAll.Checked)
			{
				obj.IsApproved = rdApproved.Checked;
			}
			else
			{
				obj.ApprovedFilter = "All";
			}
			dtGridFromDb = InvFoodProduction.JobOrderSummary(obj);
			DataTable dataTable = dtGridFromDb;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				DataGridHistory.DataSource = null;
			}
			dtGrid.Clear();
			foreach (DataRow dr in dtGridFromDb.Rows)
			{
				dtGrid.Rows.Add(dr["Id"], dr["DocDate"], dr["JobOrderNo"], dr["InputWeight"], dr["InputAmount"], dr["OutPutWeight"], dr["OutPutAmount"], dr["BalWeight"], dr["BalAmount"], dr["LedgerBalAmount"], dr["SettlementStatus"], dr["ApprovalStatus"], dr["JobOrderStatus"]);
			}
			DataGridHistory.DataSource = dtGrid;
			DataGridHistory.RetrieveStructure();
			GridSetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id" };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 3, 2, hiddenColumns);
			DataGridHistory.RootTable.Columns["JobOrderNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["JobOrderNo"].Width = 120;
			DataGridHistory.RootTable.Columns["DocDate"].Width = 85;
			DataGridHistory.RootTable.Columns["InputAmount"].Width = 110;
			DataGridHistory.RootTable.Columns["OutPutAmount"].Width = 110;
			DataGridHistory.RootTable.Columns["BalanceAmount"].Width = 120;
			DataGridHistory.RootTable.Columns["LedgerBalance"].Width = 120;
			DataGridHistory.RootTable.Columns["SettlementStatus"].Width = 100;
			DataGridHistory.RootTable.Columns["ApprovalStatus"].Width = 100;
			DataGridHistory.RootTable.Columns["JobOrderStatus"].Width = 100;
			DataGridHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["BalanceAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["LedgerBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["BalanceAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["LedgerBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && e.Column.Key == "JobOrderNo")
			{
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				int ActionId = 1;
				DataTable dt = InvFoodProductionReports.ProductionSettlement_WithReferenceDocumentDetailReport(UserAccount.OrganizationId, UserAccount.CompanyId, Id, ActionId);
				if (dt.Rows.Count > 0 && dt != null)
				{
					List<SubReportObject> lstSubReports = new List<SubReportObject>();
					SubReportObject SubReport1 = new SubReportObject(ProductionOutputAllocationWithExportInvoice.ProductionOutputAllocationWithExportInvoice_SlipAndRegister(Id, 0), "612-InvRptProductionDetailWithExpValues.rpt");
					lstSubReports.Add(SubReport1);
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTableSubReprt(dt, "613_02_ProductionSettlement_WithReferenceDocumentDetailReport.rpt", lstSubReports);
				}
				else
				{
					MessageBox.Show("Record Not Found For DisPlay");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtGrid);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			DataGridHistory.DataSource = null;
			datFromDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			gridHisory();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
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

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "DataGridHistory")
			{
				datFromDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmProductionCostingReport_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dataTable = dtGridFromDb;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtGridFromDb, "672_JobOrderSummaryReport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmProductionCostingReport_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				BtnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				datFromDate.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Show Data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Main Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Date Type");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
	{
		MakeShortCutKeys();
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 32;
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 13;
				}
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production_Reports.frmProductionJobOrderSummaryRpt));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.label23 = new System.Windows.Forms.Label();
		this.rdUnApproved = new System.Windows.Forms.RadioButton();
		this.rdApproved = new System.Windows.Forms.RadioButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(984, 561);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 110);
		this.panel5.Margin = new System.Windows.Forms.Padding(4);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(984, 451);
		this.panel5.TabIndex = 3;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(984, 422);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(4);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(984, 29);
		this.panel6.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(937, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 3;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 4);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(142, 19);
		this.label4.TabIndex = 2;
		this.label4.Text = "Filtered Records";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.datFromDate);
		this.panel4.Controls.Add(this.datToDate);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(984, 57);
		this.panel4.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(151, 8);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 591;
		this.label13.Text = "To Date";
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Checked = false;
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(15, 28);
		this.datFromDate.Margin = new System.Windows.Forms.Padding(4);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(137, 23);
		this.datFromDate.TabIndex = 1;
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(154, 27);
		this.datToDate.Margin = new System.Windows.Forms.Padding(4);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(137, 23);
		this.datToDate.TabIndex = 2;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(294, 26);
		this.btnshow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(60, 24);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(12, 8);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "From Date";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.rdAll);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.rdUnApproved);
		this.panel3.Controls.Add(this.rdApproved);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Margin = new System.Windows.Forms.Padding(4);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(984, 25);
		this.panel3.TabIndex = 1;
		this.rdAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdAll.AutoSize = true;
		this.rdAll.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.ForeColor = System.Drawing.Color.White;
		this.rdAll.Location = new System.Drawing.Point(937, 2);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(43, 22);
		this.rdAll.TabIndex = 592;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(262, 19);
		this.label23.TabIndex = 2;
		this.label23.Text = "Production Job Order Summary";
		this.rdUnApproved.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdUnApproved.AutoSize = true;
		this.rdUnApproved.Checked = true;
		this.rdUnApproved.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdUnApproved.ForeColor = System.Drawing.Color.White;
		this.rdUnApproved.Location = new System.Drawing.Point(822, 2);
		this.rdUnApproved.Name = "rdUnApproved";
		this.rdUnApproved.Size = new System.Drawing.Size(108, 22);
		this.rdUnApproved.TabIndex = 592;
		this.rdUnApproved.TabStop = true;
		this.rdUnApproved.Text = "Un Approved";
		this.rdUnApproved.UseVisualStyleBackColor = true;
		this.rdApproved.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdApproved.AutoSize = true;
		this.rdApproved.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdApproved.ForeColor = System.Drawing.Color.White;
		this.rdApproved.Location = new System.Drawing.Point(728, 2);
		this.rdApproved.Name = "rdApproved";
		this.rdApproved.Size = new System.Drawing.Size(87, 22);
		this.rdApproved.TabIndex = 592;
		this.rdApproved.Text = "Approved";
		this.rdApproved.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(984, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.BtnPrint, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(984, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(115, 24);
		this.BtnPrint.Text = "672-Register";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 561);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmProductionJobOrderSummaryRpt";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmProductionCostingReport_FormClosing);
		base.Load += new System.EventHandler(frmProductionJobOrderSummaryRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmProductionCostingReport_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
