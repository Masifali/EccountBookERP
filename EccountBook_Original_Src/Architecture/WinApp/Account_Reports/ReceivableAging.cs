using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
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

public class ReceivableAging : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtrpt = new DataTable();

	private int[] AryColWidth;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker EndDate;

	private Label label27;

	private Label label1;

	private Label label29;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel6;

	private Panel panel5;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private TextBox txtIntervaldays;

	private Button btnshow;

	internal UltraCombo cmbcomp;

	public ReceivableAging(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.P)
		{
			print_Click_1(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
		}
	}

	private void CompanyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		try
		{
			List<Company> Complst = Company.GetAll(new Company
			{
				OrgCompanyTypeId = UserAccount.OrganizationId
			});
			if (Complst.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("CompName");
				for (int i = 0; i < Complst.Count; i++)
				{
					dt.Rows.Add(Complst[i].Id, Complst[i].CompName);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbcomp, "Id", "CompName", "Comapny Name", ZeroIndex: false);
				}
				((UltraGridBase)cmbcomp).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		CompanyFill();
		txtIntervaldays.Focus();
		btnshow_Click(null, null);
	}

	private void reset()
	{
		txtIntervaldays.Clear();
		btnshow_Click(null, null);
		txtIntervaldays.Focus();
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtrpt.Rows.Count > 0 && dtrpt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtrpt, "111-AcRptReceivablesAging.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.EndDate = EndDate.Value;
			obj.IntervalDays = Conversion.ToInt((object)txtIntervaldays.Text);
			dtrpt = VoucherReports.ReceivableAging(obj);
			if (dtrpt.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("Ac1LevelTitle", typeof(string));
				dt.Columns.Add("Ac2LevelTitle", typeof(string));
				dt.Columns.Add("Ac3LevelTitle", typeof(string));
				dt.Columns.Add("Ac4LevelCode", typeof(string));
				dt.Columns.Add("Interval1St", typeof(double));
				dt.Columns.Add("Interval2nd", typeof(double));
				dt.Columns.Add("Interval3rd", typeof(double));
				dt.Columns.Add("IntervalAbove", typeof(decimal));
				dt.Columns.Add("Opening", typeof(double));
				dt.Columns.Add("Closing", typeof(double));
				for (int i = 0; i < dtrpt.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtrpt.Rows[i]["AccountTitle"]), Conversion.ToInt(dtrpt.Rows[i]["Ac1LevelTitle"]), Conversion.ToInt(dtrpt.Rows[i]["Ac2LevelTitle"]), Conversion.ToInt(dtrpt.Rows[i]["Ac3LevelTitle"]), Conversion.ToInt(dtrpt.Rows[i]["Interval1St"]), Conversion.ToInt(dtrpt.Rows[i]["Interval2nd"]), Conversion.ToInt(dtrpt.Rows[i]["Interval3rd"]), Conversion.ToInt(dtrpt.Rows[i]["IntervalAbove"]), Conversion.ToInt(dtrpt.Rows[i]["Opening"]), Conversion.ToInt(dtrpt.Rows[i]["Closing"]));
				}
				grd.DataSource = dtrpt;
				grd.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grd.RootTable.Columns["Ac1LevelCode"].Visible = false;
			grd.RootTable.Columns["Ac1LevelTitle"].Visible = false;
			grd.RootTable.Columns["Ac2LevelCode"].Visible = false;
			grd.RootTable.Columns["Ac2LevelTitle"].Visible = false;
			grd.RootTable.Columns["Ac3LevelCode"].Visible = false;
			grd.RootTable.Columns["Ac3LevelTitle"].Visible = false;
			grd.RootTable.Columns["AcId"].Visible = false;
			grd.RootTable.Columns["Ac4LevelCode"].Visible = false;
			grd.RootTable.Columns["RCounter"].Visible = false;
			grd.RootTable.Columns["CompLogoImage"].Visible = false;
			grd.RootTable.Columns["DecimlePointsRate"].Visible = false;
			grd.RootTable.Columns["DecimlePointsAmount"].Visible = false;
			grd.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Opening"].FormatString = "0,0";
			grd.RootTable.Columns["Opening"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Closing"].FormatString = "0,0";
			grd.RootTable.Columns["Closing"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["IntervalAbove"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["IntervalAbove"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["IntervalAbove"].FormatString = "0,0";
			grd.RootTable.Columns["IntervalAbove"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Interval3rd"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Interval3rd"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Interval3rd"].FormatString = "0,0";
			grd.RootTable.Columns["Interval3rd"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Interval2nd"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Interval2nd"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Interval2nd"].FormatString = "0,0";
			grd.RootTable.Columns["Interval2nd"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Interval1St"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Interval1St"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Interval1St"].FormatString = "0,0";
			grd.RootTable.Columns["Interval1St"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void txtIntervaldays_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReceivableAging_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.cmbcomp = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtIntervaldays = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.EndDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcomp).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(962, 30);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(228, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Accounts Receivable Aging";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(962, 550);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 71);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(962, 479);
		this.panel6.TabIndex = 110;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(962, 479);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcomp);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.txtIntervaldays);
		this.panel5.Controls.Add(this.label29);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.EndDate);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(962, 71);
		this.panel5.TabIndex = 0;
		this.cmbcomp.AllowNull = (DefaultableBoolean)1;
		this.cmbcomp.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcomp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcomp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbcomp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcomp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcomp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcomp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbcomp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcomp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbcomp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcomp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcomp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbcomp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcomp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcomp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcomp).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcomp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcomp).Location = new System.Drawing.Point(11, 30);
		((System.Windows.Forms.Control)(object)this.cmbcomp).Name = "cmbcomp";
		((System.Windows.Forms.Control)(object)this.cmbcomp).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbcomp).TabIndex = 0;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(556, 31);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 24);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtIntervaldays.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIntervaldays.Location = new System.Drawing.Point(352, 32);
		this.txtIntervaldays.Margin = new System.Windows.Forms.Padding(2);
		this.txtIntervaldays.Name = "txtIntervaldays";
		this.txtIntervaldays.Size = new System.Drawing.Size(201, 23);
		this.txtIntervaldays.TabIndex = 2;
		this.txtIntervaldays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtIntervaldays_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(8, 8);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(66, 17);
		this.label29.TabIndex = 106;
		this.label29.Text = "Company";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(352, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 17);
		this.label1.TabIndex = 104;
		this.label1.Text = "Interval Days";
		this.EndDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.EndDate.CustomFormat = "dd-MMM-yy";
		this.EndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.EndDate.Location = new System.Drawing.Point(217, 33);
		this.EndDate.Name = "EndDate";
		this.EndDate.Size = new System.Drawing.Size(131, 21);
		this.EndDate.TabIndex = 1;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(215, 8);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 17);
		this.label27.TabIndex = 103;
		this.label27.Text = "End Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(962, 607);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(962, 27);
		this.panel2.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(910, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.print });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(962, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(79, 24);
		this.print.Text = "111-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(962, 607);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ReceivableAging";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ReceivableAging_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcomp).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
