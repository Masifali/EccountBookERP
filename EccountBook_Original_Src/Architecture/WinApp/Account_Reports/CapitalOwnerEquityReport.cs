using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class CapitalOwnerEquityReport : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtHeader = new DataTable();

	private DateTime ToDate;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel1;

	private Panel panel2;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel6;

	private ToolStrip tsSelectedTrialBalance;

	private ToolStripButton btnRefreshSelectedTrial;

	private Button btnShowSelectedTrial;

	internal UltraCombo cmbAccountTitle;

	private DateTimePicker datToDateST;

	private Label label3;

	private Label label4;

	internal GridEX grdSelected;

	private TextBox txtProfit;

	private Label label1;

	private ToolStripButton btnPrint;

	private CheckBox ChkZkipZeroSelected;

	public CapitalOwnerEquityReport(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			btnRefreshSelectedTrial_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			datToDateST.Focus();
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			((Control)(object)grdSelected).Focus();
		}
	}

	private void AccountTitleFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			DataTable dt = ChartofAccount.ReadAllAccountgroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0)
			{
				dt = dt.Select("AccountClass = 1 AND Account_Level = 3").CopyToDataTable();
				DDL.BindDDLNew(dt, cmbAccountTitle, "Id", "AccountTitle", "Account Title", false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			AccountTitleFill();
			cmbAccountTitle.Focus();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	private void btnShowSelectedTrial_Click(object sender, EventArgs e)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		try
		{
			wf.Show();
			dtHeader.Rows.Clear();
			if (((UltraGridBase)cmbAccountTitle).ActiveRow == null)
			{
				cmbAccountTitle.Focus();
				throw new Exception("Account Title Required");
			}
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.GroupAccountId = Conversion.ToInt(cmbAccountTitle.Value);
			vh.FromDate = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			vh.ToDate = datToDateST.Value;
			vh.ApprovedFilter = "All";
			if (ChkZkipZeroSelected.Checked)
			{
				vh.ZeroBalanceType = 1;
			}
			DataTable dtTrialSelected = VoucherReports.SelectedTrialBalanceNew(vh).Tables[0].Copy();
			if (dtTrialSelected.Rows.Count > 0)
			{
				if (dtHeader.Columns.Count == 0)
				{
					dtHeader.Columns.Add("AccountId");
					dtHeader.Columns.Add("AccountCode");
					dtHeader.Columns.Add("AccountTitle");
					dtHeader.Columns.Add("Closing", typeof(double));
					dtHeader.Columns.Add("Prct", typeof(double));
					dtHeader.Columns.Add("Profit", typeof(double));
				}
				for (int i = 0; i < dtTrialSelected.Rows.Count; i++)
				{
					double totalAmount = Conversion.ToDouble(dtTrialSelected.Compute("SUM(Closing)", string.Empty));
					dtHeader.Rows.Add(dtTrialSelected.Rows[i]["AccountId"], dtTrialSelected.Rows[i]["AccountCode"], dtTrialSelected.Rows[i]["AccountTitle"], dtTrialSelected.Rows[i]["Closing"], (Conversion.ToDouble(dtTrialSelected.Rows[i]["Closing"]) != 0.0) ? (Conversion.ToDouble(dtTrialSelected.Rows[i]["Closing"]) / totalAmount * 100.0) : 0.0, (Conversion.ToDouble((object)txtProfit.Text) > 0.0) ? ((Conversion.ToDouble(dtTrialSelected.Rows[i]["Closing"]) != 0.0) ? (Conversion.ToDouble(dtTrialSelected.Rows[i]["Closing"]) / totalAmount * Conversion.ToDouble((object)txtProfit.Text)) : 0.0) : 0.0);
				}
				grdSelected.DataSource = dtHeader;
				grdSelected.RetrieveStructure();
				GridSettingsSelected();
			}
			else
			{
				grdSelected.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			wf.Close();
		}
	}

	private void GridSettingsSelected()
	{
		try
		{
			grdSelected.RootTable.Columns["AccountId"].Visible = false;
			grdSelected.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grdSelected.RootTable.Columns["AccountCode"].Width = 110;
			grdSelected.RootTable.Columns["AccountTitle"].Width = 250;
			grdSelected.RootTable.Columns["Closing"].Width = 120;
			grdSelected.RootTable.Columns["Prct"].Width = 90;
			grdSelected.RootTable.Columns["Profit"].Width = 120;
			grdSelected.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["Profit"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["Profit"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["Profit"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["Profit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["Prct"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["Prct"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["Prct"].FormatString = "#,##0.####;(#,0.####);#";
			grdSelected.RootTable.Columns["Prct"].TotalFormatString = "#,##0.####;(#,0.####);#";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void grdSelected_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(grdSelected.CurrentRow.Cells["AccountId"].Value), Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period), datToDateST.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void grdSelected_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = grdSelected.CurrentRow;
				if (r != null && (int)r.RowType == 0 && grdSelected.CurrentColumn != null && grdSelected.CurrentColumn.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["AccountId"].Value), Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period), datToDateST.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdSelected).Name);
			if (fs != null)
			{
				grdSelected.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefreshSelectedTrial_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			AccountTitleFill();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	private void txtProfit_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtProfit_Leave(object sender, EventArgs e)
	{
		try
		{
			txtProfit.Text = Conversion.ToDouble((object)txtProfit.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtHeader.Rows.Count > 0 && dtHeader != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtHeader, "120A-CapitalOwnerEquityReport.rpt");
				return;
			}
			throw new Exception("No Data Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CapitalOwnerEquityReport_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdSelected = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ChkZkipZeroSelected = new System.Windows.Forms.CheckBox();
		this.txtProfit = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.btnShowSelectedTrial = new System.Windows.Forms.Button();
		this.cmbAccountTitle = new UltraCombo();
		this.datToDateST = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tsSelectedTrialBalance = new System.Windows.Forms.ToolStrip();
		this.btnRefreshSelectedTrial = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSelected).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsSelectedTrialBalance.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1105, 27);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(242, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Capital Owner Equity Report";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 54);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1105, 555);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdSelected);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 84);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1105, 471);
		this.panel6.TabIndex = 108;
		this.grdSelected.AllowEdit = (InheritableBoolean)2;
		this.grdSelected.AlternatingColors = true;
		((System.Windows.Forms.Control)(object)this.grdSelected).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSelected.DynamicFiltering = true;
		this.grdSelected.FilterMode = (FilterMode)2;
		this.grdSelected.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSelected).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSelected.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSelected).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSelected).Name = "grdSelected";
		((ScrollableBase)this.grdSelected).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSelected).Size = new System.Drawing.Size(1105, 471);
		((System.Windows.Forms.Control)(object)this.grdSelected).TabIndex = 6;
		this.grdSelected.TotalRow = (InheritableBoolean)1;
		this.grdSelected.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSelected).VisualStyle = (VisualStyle)4;
		this.grdSelected.LinkClicked += new ColumnActionEventHandler(grdSelected_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdSelected).KeyDown += new System.Windows.Forms.KeyEventHandler(grdSelected_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.ChkZkipZeroSelected);
		this.panel5.Controls.Add(this.txtProfit);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.btnShowSelectedTrial);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitle);
		this.panel5.Controls.Add(this.datToDateST);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1105, 84);
		this.panel5.TabIndex = 0;
		this.ChkZkipZeroSelected.AutoSize = true;
		this.ChkZkipZeroSelected.Location = new System.Drawing.Point(367, 7);
		this.ChkZkipZeroSelected.Margin = new System.Windows.Forms.Padding(2);
		this.ChkZkipZeroSelected.Name = "ChkZkipZeroSelected";
		this.ChkZkipZeroSelected.Size = new System.Drawing.Size(72, 17);
		this.ChkZkipZeroSelected.TabIndex = 149;
		this.ChkZkipZeroSelected.Text = "Skip Zero";
		this.ChkZkipZeroSelected.UseVisualStyleBackColor = true;
		this.txtProfit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProfit.Location = new System.Drawing.Point(109, 57);
		this.txtProfit.Name = "txtProfit";
		this.txtProfit.Size = new System.Drawing.Size(192, 23);
		this.txtProfit.TabIndex = 2;
		this.txtProfit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtProfit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtProfit_KeyPress);
		this.txtProfit.Leave += new System.EventHandler(txtProfit_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(24, 61);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(36, 15);
		this.label1.TabIndex = 143;
		this.label1.Text = "Profit";
		this.btnShowSelectedTrial.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowSelectedTrial.FlatAppearance.BorderSize = 0;
		this.btnShowSelectedTrial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowSelectedTrial.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowSelectedTrial.ForeColor = System.Drawing.Color.White;
		this.btnShowSelectedTrial.Location = new System.Drawing.Point(302, 56);
		this.btnShowSelectedTrial.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowSelectedTrial.Name = "btnShowSelectedTrial";
		this.btnShowSelectedTrial.Size = new System.Drawing.Size(56, 24);
		this.btnShowSelectedTrial.TabIndex = 3;
		this.btnShowSelectedTrial.Text = "Show";
		this.btnShowSelectedTrial.UseVisualStyleBackColor = false;
		this.btnShowSelectedTrial.Click += new System.EventHandler(btnShowSelectedTrial_Click);
		this.cmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Location = new System.Drawing.Point(109, 28);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Name = "cmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).TabIndex = 1;
		this.datToDateST.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDateST.CustomFormat = "dd-MMM-yy";
		this.datToDateST.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDateST.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDateST.Location = new System.Drawing.Point(109, 3);
		this.datToDateST.Name = "datToDateST";
		this.datToDateST.Size = new System.Drawing.Size(249, 23);
		this.datToDateST.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(24, 34);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 142;
		this.label3.Text = "Account Title";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(24, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 140;
		this.label4.Text = "To Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1105, 609);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.tsSelectedTrialBalance);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1105, 27);
		this.panel2.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1063, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdSelected;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tsSelectedTrialBalance.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsSelectedTrialBalance.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefreshSelectedTrial, this.btnPrint });
		this.tsSelectedTrialBalance.Location = new System.Drawing.Point(0, 0);
		this.tsSelectedTrialBalance.Name = "tsSelectedTrialBalance";
		this.tsSelectedTrialBalance.Size = new System.Drawing.Size(1105, 27);
		this.tsSelectedTrialBalance.TabIndex = 8;
		this.tsSelectedTrialBalance.Text = "toolStrip1";
		this.btnRefreshSelectedTrial.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSelectedTrial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSelectedTrial.Name = "btnRefreshSelectedTrial";
		this.btnRefreshSelectedTrial.Size = new System.Drawing.Size(70, 24);
		this.btnRefreshSelectedTrial.Text = "&Refresh";
		this.btnRefreshSelectedTrial.Click += new System.EventHandler(btnRefreshSelectedTrial_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(87, 24);
		this.btnPrint.Text = "Print-120A";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1105, 609);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "CapitalOwnerEquityReport";
		this.Text = "Trial Balance Selected";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CapitalOwnerEquityReport_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSelected).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsSelectedTrialBalance.ResumeLayout(false);
		this.tsSelectedTrialBalance.PerformLayout();
		base.ResumeLayout(false);
	}
}
