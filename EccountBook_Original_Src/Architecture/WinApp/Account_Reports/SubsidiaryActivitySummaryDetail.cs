using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class SubsidiaryActivitySummaryDetail : Form
{
	private UserAccount UserAccount = new UserAccount();

	public DataTable dtReport = new DataTable();

	public string ParentTitle = "";

	public DateTime FromDate;

	public DateTime ToDate;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label1;

	private Panel panel4;

	internal GridEX grd;

	private RadioButton rdSixColumns;

	private RadioButton rdFourColumns;

	private ToolStrip toolStrip1;

	private ToolStripButton btnPrint;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar1;

	private RichTextBox txtCriteria;

	private Label label2;

	private Label lblParentAccountTitle;

	public SubsidiaryActivitySummaryDetail(UserAccount puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puser;
	}

	private void SubsidiaryPayableReceiveables_Load(object sender, EventArgs e)
	{
		try
		{
			lblParentAccountTitle.Text = ParentTitle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GetData(DateTime FromDate, DateTime ToDate, int AccountId, string DocumenTypeIds = null)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtReport.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = FromDate;
			obj.ToDate = ToDate;
			obj.AccountId = AccountId;
			obj.Ids = DocumenTypeIds;
			obj.ReportTypeId = 2;
			dtReport = VoucherReports.ActivitySummaryReport(obj);
			if (dtReport.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("AccountId", typeof(int));
			dt.Columns.Add("SubsidiaryAccoutId", typeof(int));
			dt.Columns.Add("ParentAccount", typeof(string));
			dt.Columns.Add("ParentAccountTitle", typeof(string));
			dt.Columns.Add("AccountCode", typeof(string));
			dt.Columns.Add("AccountTitle", typeof(string));
			dt.Columns.Add("OpeningDr", typeof(double));
			dt.Columns.Add("OpeningCr", typeof(double));
			dt.Columns.Add("Opening", typeof(double));
			dt.Columns.Add("Debit", typeof(double));
			dt.Columns.Add("Credit", typeof(double));
			dt.Columns.Add("Closing", typeof(double));
			dt.Columns.Add("ClosingDr", typeof(double));
			dt.Columns.Add("ClosingCr", typeof(double));
			dt.Clear();
			for (int i = 0; i < dtReport.Rows.Count; i++)
			{
				if (i == 0)
				{
					txtCriteria.Text = Conversion.ToString(dtReport.Rows[i]["ReportCriteria"]);
				}
				dt.Rows.Add(Conversion.ToInt(dtReport.Rows[i]["AccountId"]), Conversion.ToInt(dtReport.Rows[i]["SubsidiaryAccoutId"]), Conversion.ToString(dtReport.Rows[i]["ParentAccount"]), Conversion.ToString(dtReport.Rows[i]["ParentAccountTitle"]), Conversion.ToString(dtReport.Rows[i]["AccountCode"]), Conversion.ToString(dtReport.Rows[i]["AccountTitle"]), Conversion.ToDouble(dtReport.Rows[i]["OpeningDr"]), Conversion.ToDouble(dtReport.Rows[i]["OpeningCr"]), Conversion.ToDouble(dtReport.Rows[i]["Opening"]), Conversion.ToDouble(dtReport.Rows[i]["Debit"]), Conversion.ToDouble(dtReport.Rows[i]["Credit"]), Conversion.ToDouble(dtReport.Rows[i]["Closing"]), Conversion.ToDouble(dtReport.Rows[i]["ClosingDr"]), Conversion.ToDouble(dtReport.Rows[i]["ClosingCr"]));
			}
			grd.DataSource = dt;
			grd.RetrieveStructure();
			GridSettings();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grd.RootTable.Columns["ParentAccount"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["ParentAccountTitle"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccoutId"].Visible = false;
			if (rdFourColumns.Checked)
			{
				grd.RootTable.Columns["OpeningDr"].Visible = false;
				grd.RootTable.Columns["OpeningCr"].Visible = false;
				grd.RootTable.Columns["ClosingDr"].Visible = false;
				grd.RootTable.Columns["ClosingCr"].Visible = false;
				grd.RootTable.Columns["Opening"].Visible = true;
				grd.RootTable.Columns["Closing"].Visible = true;
			}
			else if (rdSixColumns.Checked)
			{
				grd.RootTable.Columns["Opening"].Visible = false;
				grd.RootTable.Columns["Closing"].Visible = false;
				grd.RootTable.Columns["OpeningDr"].Visible = true;
				grd.RootTable.Columns["OpeningCr"].Visible = true;
				grd.RootTable.Columns["ClosingDr"].Visible = true;
				grd.RootTable.Columns["ClosingCr"].Visible = true;
			}
			grd.RootTable.Columns["OpeningDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OpeningDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OpeningDr"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["OpeningDr"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["OpeningCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OpeningCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OpeningCr"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["OpeningCr"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Opening"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Opening"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Debit"].FormatString = "0,0";
			grd.RootTable.Columns["Debit"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Credit"].FormatString = "0,0";
			grd.RootTable.Columns["Credit"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Closing"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["Closing"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["ClosingDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ClosingDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClosingDr"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["ClosingDr"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["ClosingCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ClosingCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClosingCr"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["ClosingCr"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["AccountTitle"].Width = 350;
			CommonServices.GridAutoAdjustmentNew(grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSixColumns_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtReport.Rows.Count > 0)
			{
				if (rdFourColumns.Checked)
				{
					grd.RootTable.Columns["OpeningDr"].Visible = false;
					grd.RootTable.Columns["OpeningCr"].Visible = false;
					grd.RootTable.Columns["ClosingDr"].Visible = false;
					grd.RootTable.Columns["ClosingCr"].Visible = false;
					grd.RootTable.Columns["Opening"].Visible = true;
					grd.RootTable.Columns["Closing"].Visible = true;
				}
				else if (rdSixColumns.Checked)
				{
					grd.RootTable.Columns["Opening"].Visible = false;
					grd.RootTable.Columns["Closing"].Visible = false;
					grd.RootTable.Columns["OpeningDr"].Visible = true;
					grd.RootTable.Columns["OpeningCr"].Visible = true;
					grd.RootTable.Columns["ClosingDr"].Visible = true;
					grd.RootTable.Columns["ClosingCr"].Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "AccountCode")
			{
				GeneralLedger obj = new GeneralLedger(UserAccount)
				{
					txtFromDate = 
					{
						Value = Conversion.ToDateTime((object)FromDate)
					},
					txtToDate = 
					{
						Value = Conversion.ToDateTime((object)ToDate)
					}
				};
				obj.cmbAccountTitle.Value = Conversion.ToInt(grd.CurrentRow.Cells["AccountId"].Value);
				obj.CmbSubsidiaryAccount.Value = Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccoutId"].Value);
				obj.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtReport == null || dtReport.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtReport, "110-AcRptActivitySummery.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SubsidiaryPayableReceiveables_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.txtCriteria = new System.Windows.Forms.RichTextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.rdFourColumns = new System.Windows.Forms.RadioButton();
		this.rdSixColumns = new System.Windows.Forms.RadioButton();
		this.lblParentAccountTitle = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(785, 561);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 164);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(785, 397);
		this.panel4.TabIndex = 1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(785, 397);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.panel2.Controls.Add(this.lblParentAccountTitle);
		this.panel2.Controls.Add(this.txtCriteria);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.rdFourColumns);
		this.panel2.Controls.Add(this.rdSixColumns);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(785, 164);
		this.panel2.TabIndex = 0;
		this.txtCriteria.Location = new System.Drawing.Point(3, 83);
		this.txtCriteria.Name = "txtCriteria";
		this.txtCriteria.Size = new System.Drawing.Size(674, 45);
		this.txtCriteria.TabIndex = 11;
		this.txtCriteria.Text = "";
		this.label2.BackColor = System.Drawing.Color.LightGray;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 62);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(672, 19);
		this.label2.TabIndex = 10;
		this.label2.Text = "Report Criteria";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(785, 32);
		this.panel3.TabIndex = 9;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(743, 5);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(385, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Activity Summary Detail Control Account Wise";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip1.Size = new System.Drawing.Size(785, 27);
		this.toolStrip1.TabIndex = 8;
		this.toolStrip1.Text = "toolStrip1";
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(79, 24);
		this.btnPrint.Text = "110-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.rdFourColumns.AutoSize = true;
		this.rdFourColumns.Checked = true;
		this.rdFourColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdFourColumns.Location = new System.Drawing.Point(683, 89);
		this.rdFourColumns.Name = "rdFourColumns";
		this.rdFourColumns.Size = new System.Drawing.Size(99, 19);
		this.rdFourColumns.TabIndex = 2;
		this.rdFourColumns.TabStop = true;
		this.rdFourColumns.Text = "Four Columns";
		this.rdFourColumns.UseVisualStyleBackColor = true;
		this.rdSixColumns.AutoSize = true;
		this.rdSixColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSixColumns.Location = new System.Drawing.Point(683, 66);
		this.rdSixColumns.Name = "rdSixColumns";
		this.rdSixColumns.Size = new System.Drawing.Size(91, 19);
		this.rdSixColumns.TabIndex = 1;
		this.rdSixColumns.Text = "Six Columns";
		this.rdSixColumns.UseVisualStyleBackColor = true;
		this.rdSixColumns.CheckedChanged += new System.EventHandler(rdSixColumns_CheckedChanged);
		this.lblParentAccountTitle.BackColor = System.Drawing.Color.LightGray;
		this.lblParentAccountTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblParentAccountTitle.Location = new System.Drawing.Point(3, 131);
		this.lblParentAccountTitle.Name = "lblParentAccountTitle";
		this.lblParentAccountTitle.Size = new System.Drawing.Size(672, 30);
		this.lblParentAccountTitle.TabIndex = 12;
		this.lblParentAccountTitle.Text = "Group Account Title";
		this.lblParentAccountTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(785, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "SubsidiaryActivitySummaryDetail";
		this.Text = "Activity Summary Detail Control Account Wise";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(SubsidiaryPayableReceiveables_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SubsidiaryPayableReceiveables_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
