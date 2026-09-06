using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp.Graph;

public class MonthWisePurchaseReport : Form
{
	private UserAccount userAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label label23;

	private Chart chart1;

	internal UltraCombo CmbMonth;

	private Label label3;

	private Button btnshow;

	internal UltraCombo CmbParentCategory;

	private Label label7;

	internal UltraCombo CmbClassGroup;

	private Label label1;

	private Label label5;

	private Label label4;

	private Label label2;

	private Panel MonthAmountPanel;

	private Panel panel23;

	private Label GrandTotal;

	private Label label48;

	private FlowLayoutPanel FlowLayout;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Chart chart2;

	private Panel panel4;

	private Label label10;

	private Label label6;

	private Label label8;

	private Label label9;

	private Panel QuarterlyPanel;

	private FlowLayoutPanel QuarterlyFlowLayout;

	private Panel panel6;

	private Label TotalQuarterAmount;

	private Label label12;

	private Label label11;

	public MonthWisePurchaseReport(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		userAccount = Puser;
	}

	private void MonthWisePurchaseReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(880, 820);
			Month();
			CombosAgainstPurchaseInvoice();
			ShowReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Month()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Month");
			dt.Rows.Add(1, "January");
			dt.Rows.Add(2, "February");
			dt.Rows.Add(3, "March");
			dt.Rows.Add(4, "April");
			dt.Rows.Add(5, "May");
			dt.Rows.Add(6, "June");
			dt.Rows.Add(7, "July");
			dt.Rows.Add(8, "August");
			dt.Rows.Add(9, "September");
			dt.Rows.Add(10, "October");
			dt.Rows.Add(11, "November");
			dt.Rows.Add(12, "December");
			DDL.BindDDL(dt, CmbMonth, "Id", "Month", "Month", ZeroIndex: false);
			((UltraGridBase)CmbMonth).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CombosAgainstPurchaseInvoice()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(new ReportsParameters
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtParentCat = new DataTable();
			dtParentCat.Columns.Add("Id");
			dtParentCat.Columns.Add("ParentCategory");
			DataTable dtClass = new DataTable();
			dtClass.Columns.Add("Id");
			dtClass.Columns.Add("ClassGroup");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemParentCategory")
				{
					dtParentCat.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemClassGroup")
				{
					dtClass.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			if (dtParentCat.Rows.Count > 0)
			{
				DDL.BindDDL(dtParentCat, CmbParentCategory, "Id", "ParentCategory", "Category", ZeroIndex: false);
				((UltraGridBase)CmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((UltraGridBase)CmbParentCategory).DataSource = null;
			}
			if (dtClass.Rows.Count > 0)
			{
				DDL.BindDDL(dtClass, CmbClassGroup, "Id", "ClassGroup", "Class Group", ZeroIndex: false);
				((UltraGridBase)CmbClassGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((UltraGridBase)CmbClassGroup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			ShowReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ShowReport()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		try
		{
			DataSet ds = InvPurchaseInvoice.InventoryPurchsaeComparisonsMonthWise(new ReportsParameters
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId,
				FromDate = clsGlobalVariables.ActiveYr.Start_Period,
				ToDate = clsGlobalVariables.ActiveYr.End_Period,
				MonthValue = Conversion.ToInt(CmbMonth.Value),
				InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value),
				ItemClassGroupId = Conversion.ToInt(CmbClassGroup.Value)
			});
			if (ds.Tables.Count > 0)
			{
				chart1.DataSource = ds.Tables[2];
				chart1.Series["Purchase"].XValueMember = "PurchaseMonth";
				chart1.Series["Purchase"].YValueMembers = "CurrPurchaseAmount";
				chart1.Series["Purchase"].ChartType = SeriesChartType.Column;
				chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
				chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
				chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
				chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe Ui", 10f, FontStyle.Bold);
				chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Auto;
				chart1.ChartAreas[0].AxisX.Interval = 1.0;
				chart1.Series["Purchase"].ToolTip = "#VALY{#,##0.##}";
				chart1.Series["Purchase"].YValueType = ChartValueType.Double;
				DataTable dtQuarter = new DataTable();
				dtQuarter.Columns.Add("QuarterMonths");
				dtQuarter.Columns.Add("QuarterAmount", typeof(double));
				for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
				{
					dtQuarter.Rows.Add(Conversion.ToDateTime(ds.Tables[1].Rows[k]["QuarterStartDate"]).ToString("MMM") + " To " + Conversion.ToDateTime(ds.Tables[1].Rows[k]["QuarterEndDate"]).ToString("MMM") + "  " + Conversion.ToDateTime(ds.Tables[1].Rows[k]["QuarterStartDate"]).ToString("yy"), ds.Tables[1].Rows[k]["CurrAmount"]);
				}
				chart2.DataSource = dtQuarter;
				chart2.Series["Purchase"].XValueMember = "QuarterMonths";
				chart2.Series["Purchase"].YValueMembers = "QuarterAmount";
				chart2.Series["Purchase"]["PieLabelStyle"] = "Disabled";
				chart2.Series["Purchase"].ToolTip = "#VALY{#,##0.##}";
				chart2.Series["Purchase"].YValueType = ChartValueType.Double;
				int TotalMonths = ds.Tables[2].Rows.Count;
				double totalAmount = 0.0;
				FlowLayout.Controls.Clear();
				MonthWiseReportUserControl[] CardsList = new MonthWiseReportUserControl[TotalMonths];
				for (int j = 0; j < CardsList.Length; j++)
				{
					CardsList[j] = new MonthWiseReportUserControl();
					CardsList[j].Month = Conversion.ToString(ds.Tables[2].Rows[j]["PurchaseMonth"]);
					double amount2 = Conversion.ToDouble(ds.Tables[2].Rows[j]["CurrPurchaseAmount"]);
					CardsList[j].Amount = amount2.ToString(clsGlobalVariables.stringFormatsingle);
					FlowLayout.Controls.Add(CardsList[j]);
					totalAmount += Conversion.ToDouble(ds.Tables[2].Rows[j]["CurrPurchaseAmount"]);
				}
				int TotalQuarters = ds.Tables[1].Rows.Count;
				double totalQuarterAmount = 0.0;
				QuarterlyFlowLayout.Controls.Clear();
				QuarterlyWiseReportUserControl[] QuarterCardsList = new QuarterlyWiseReportUserControl[TotalQuarters];
				for (int i = 0; i < QuarterCardsList.Length; i++)
				{
					QuarterCardsList[i] = new QuarterlyWiseReportUserControl();
					QuarterCardsList[i].QuarterStartDate = Conversion.ToDateTime(ds.Tables[1].Rows[i]["QuarterStartDate"]).ToShortDateString();
					QuarterCardsList[i].QuarterEndDate = Conversion.ToDateTime(ds.Tables[1].Rows[i]["QuarterEndDate"]).ToShortDateString();
					QuarterCardsList[i].Year = Conversion.ToString(ds.Tables[1].Rows[i]["YearNo"]);
					double amount = Conversion.ToDouble(ds.Tables[1].Rows[i]["CurrAmount"]);
					QuarterCardsList[i].Amount = amount.ToString(clsGlobalVariables.stringFormatsingle);
					QuarterlyFlowLayout.Controls.Add(QuarterCardsList[i]);
					totalQuarterAmount += Conversion.ToDouble(ds.Tables[1].Rows[i]["CurrAmount"]);
				}
				if (TotalMonths == 1)
				{
					FlowLayout.Height = 26;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 2)
				{
					FlowLayout.Height = 52;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 3)
				{
					FlowLayout.Height = 78;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 4)
				{
					FlowLayout.Height = 104;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 5)
				{
					FlowLayout.Height = 130;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 6)
				{
					FlowLayout.Height = 156;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 7)
				{
					FlowLayout.Height = 182;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 8)
				{
					FlowLayout.Height = 208;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 9)
				{
					FlowLayout.Height = 234;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 10)
				{
					FlowLayout.Height = 260;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths == 11)
				{
					FlowLayout.Height = 286;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				else if (TotalMonths >= 12)
				{
					FlowLayout.Height = 312;
					MonthAmountPanel.Height = FlowLayout.Height + 32;
				}
				GrandTotal.Text = totalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				switch (TotalQuarters)
				{
				case 1:
					QuarterlyFlowLayout.Height = 26;
					QuarterlyPanel.Height = QuarterlyFlowLayout.Height + 32;
					break;
				case 2:
					QuarterlyFlowLayout.Height = 52;
					QuarterlyPanel.Height = QuarterlyFlowLayout.Height + 32;
					break;
				case 3:
					QuarterlyFlowLayout.Height = 78;
					QuarterlyPanel.Height = QuarterlyFlowLayout.Height + 32;
					break;
				case 4:
					QuarterlyFlowLayout.Height = 104;
					QuarterlyPanel.Height = QuarterlyFlowLayout.Height + 32;
					break;
				case 5:
					QuarterlyFlowLayout.Height = 130;
					QuarterlyPanel.Height = QuarterlyFlowLayout.Height + 32;
					break;
				case 6:
					QuarterlyFlowLayout.Height = 156;
					QuarterlyPanel.Height = QuarterlyFlowLayout.Height + 32;
					break;
				}
				TotalQuarterAmount.Text = totalQuarterAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbParentCategory).Text = string.Empty;
			((Control)(object)CmbClassGroup).Text = string.Empty;
			ShowReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MonthWisePurchaseReport_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Title title5 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title6 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title7 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Title title8 = new System.Windows.Forms.DataVisualization.Charting.Title();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.label2 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.label4 = new System.Windows.Forms.Label();
		this.MonthAmountPanel = new System.Windows.Forms.Panel();
		this.FlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel23 = new System.Windows.Forms.Panel();
		this.GrandTotal = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.label11 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.QuarterlyPanel = new System.Windows.Forms.Panel();
		this.QuarterlyFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.TotalQuarterAmount = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbMonth = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbClassGroup = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbParentCategory = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.chart1).BeginInit();
		this.MonthAmountPanel.SuspendLayout();
		this.panel23.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.QuarterlyPanel.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.chart2).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMonth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClassGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(861, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl1.Location = new System.Drawing.Point(0, 99);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(861, 669);
		this.tabControl1.TabIndex = 148;
		this.tabPage1.Controls.Add(this.label2);
		this.tabPage1.Controls.Add(this.label5);
		this.tabPage1.Controls.Add(this.chart1);
		this.tabPage1.Controls.Add(this.label4);
		this.tabPage1.Controls.Add(this.MonthAmountPanel);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(853, 643);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Month Wise Bar Chart";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(9, 277);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 16);
		this.label2.TabIndex = 143;
		this.label2.Text = "Month";
		this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label5.Location = new System.Drawing.Point(4, 295);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(763, 2);
		this.label5.TabIndex = 145;
		chartArea3.Name = "ChartArea1";
		this.chart1.ChartAreas.Add(chartArea3);
		this.chart1.Dock = System.Windows.Forms.DockStyle.Top;
		legend3.Name = "Legend1";
		this.chart1.Legends.Add(legend3);
		this.chart1.Location = new System.Drawing.Point(3, 3);
		this.chart1.Name = "chart1";
		series3.ChartArea = "ChartArea1";
		series3.Color = System.Drawing.Color.Green;
		series3.Legend = "Legend1";
		series3.Name = "Purchase";
		series3.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
		series3.YValuesPerPoint = 2;
		this.chart1.Series.Add(series3);
		this.chart1.Size = new System.Drawing.Size(847, 271);
		this.chart1.TabIndex = 6;
		this.chart1.Text = "chart1";
		title5.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title5.Name = "Monthly Purchase Graph";
		title5.Text = "Monthly Purchase Graph";
		title6.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title6.Name = "YAxisTitle";
		title6.Text = "Amount In Rs";
		title7.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title7.Name = "XAxisTitle";
		title7.Text = "Months";
		this.chart1.Titles.Add(title5);
		this.chart1.Titles.Add(title6);
		this.chart1.Titles.Add(title7);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(681, 277);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(58, 16);
		this.label4.TabIndex = 144;
		this.label4.Text = "Amount";
		this.MonthAmountPanel.Controls.Add(this.FlowLayout);
		this.MonthAmountPanel.Controls.Add(this.panel23);
		this.MonthAmountPanel.Location = new System.Drawing.Point(4, 297);
		this.MonthAmountPanel.Name = "MonthAmountPanel";
		this.MonthAmountPanel.Size = new System.Drawing.Size(763, 344);
		this.MonthAmountPanel.TabIndex = 146;
		this.FlowLayout.AutoScroll = true;
		this.FlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
		this.FlowLayout.Location = new System.Drawing.Point(0, 0);
		this.FlowLayout.Margin = new System.Windows.Forms.Padding(0);
		this.FlowLayout.Name = "FlowLayout";
		this.FlowLayout.Size = new System.Drawing.Size(763, 315);
		this.FlowLayout.TabIndex = 22;
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.GrandTotal);
		this.panel23.Controls.Add(this.label48);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel23.Location = new System.Drawing.Point(0, 312);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(763, 32);
		this.panel23.TabIndex = 21;
		this.GrandTotal.AutoSize = true;
		this.GrandTotal.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GrandTotal.ForeColor = System.Drawing.Color.White;
		this.GrandTotal.Location = new System.Drawing.Point(597, 3);
		this.GrandTotal.Name = "GrandTotal";
		this.GrandTotal.Size = new System.Drawing.Size(22, 23);
		this.GrandTotal.TabIndex = 5;
		this.GrandTotal.Text = "0";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.White;
		this.label48.Location = new System.Drawing.Point(533, 3);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(66, 23);
		this.label48.TabIndex = 4;
		this.label48.Text = "Total:";
		this.tabPage2.Controls.Add(this.label11);
		this.tabPage2.Controls.Add(this.label6);
		this.tabPage2.Controls.Add(this.QuarterlyPanel);
		this.tabPage2.Controls.Add(this.label10);
		this.tabPage2.Controls.Add(this.chart2);
		this.tabPage2.Controls.Add(this.label9);
		this.tabPage2.Controls.Add(this.label8);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(853, 643);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Quarter Wise Pie Chart";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(92, 275);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(65, 16);
		this.label11.TabIndex = 151;
		this.label11.Text = "End Date";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(4, 275);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(76, 16);
		this.label6.TabIndex = 146;
		this.label6.Text = "Start Date";
		this.QuarterlyPanel.Controls.Add(this.QuarterlyFlowLayout);
		this.QuarterlyPanel.Controls.Add(this.panel6);
		this.QuarterlyPanel.Location = new System.Drawing.Point(3, 296);
		this.QuarterlyPanel.Name = "QuarterlyPanel";
		this.QuarterlyPanel.Size = new System.Drawing.Size(763, 160);
		this.QuarterlyPanel.TabIndex = 150;
		this.QuarterlyFlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
		this.QuarterlyFlowLayout.Location = new System.Drawing.Point(0, 0);
		this.QuarterlyFlowLayout.Margin = new System.Windows.Forms.Padding(0);
		this.QuarterlyFlowLayout.Name = "QuarterlyFlowLayout";
		this.QuarterlyFlowLayout.Size = new System.Drawing.Size(763, 131);
		this.QuarterlyFlowLayout.TabIndex = 22;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.TotalQuarterAmount);
		this.panel6.Controls.Add(this.label12);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(0, 128);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(763, 32);
		this.panel6.TabIndex = 21;
		this.TotalQuarterAmount.AutoSize = true;
		this.TotalQuarterAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 13.75f, System.Drawing.FontStyle.Bold);
		this.TotalQuarterAmount.ForeColor = System.Drawing.Color.White;
		this.TotalQuarterAmount.Location = new System.Drawing.Point(597, 3);
		this.TotalQuarterAmount.Name = "TotalQuarterAmount";
		this.TotalQuarterAmount.Size = new System.Drawing.Size(23, 25);
		this.TotalQuarterAmount.TabIndex = 5;
		this.TotalQuarterAmount.Text = "0";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 13.75f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(533, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(58, 25);
		this.label12.TabIndex = 4;
		this.label12.Text = "Total:";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(179, 275);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(93, 16);
		this.label10.TabIndex = 149;
		this.label10.Text = "Quarter Year";
		chartArea4.Name = "ChartArea1";
		this.chart2.ChartAreas.Add(chartArea4);
		legend4.Name = "Legend1";
		this.chart2.Legends.Add(legend4);
		this.chart2.Location = new System.Drawing.Point(3, 3);
		this.chart2.Name = "chart2";
		this.chart2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
		series4.ChartArea = "ChartArea1";
		series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
		series4.Color = System.Drawing.Color.Green;
		series4.Legend = "Legend1";
		series4.Name = "Purchase";
		series4.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent;
		series4.YValuesPerPoint = 2;
		this.chart2.Series.Add(series4);
		this.chart2.Size = new System.Drawing.Size(583, 267);
		this.chart2.TabIndex = 7;
		this.chart2.Text = "chart2";
		title8.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title8.Name = "QuarterlyPurchaseGraph";
		title8.Text = "Quarterly Purchase Graph";
		this.chart2.Titles.Add(title8);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(708, 275);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(58, 16);
		this.label9.TabIndex = 147;
		this.label9.Text = "Amount";
		this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label8.Location = new System.Drawing.Point(3, 293);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(763, 2);
		this.label8.TabIndex = 148;
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMonth);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbClassGroup);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 59);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(861, 40);
		this.panel4.TabIndex = 147;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(464, 5);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(60, 26);
		this.btnshow.TabIndex = 110;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbMonth.AllowNull = (DefaultableBoolean)1;
		this.CmbMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMonth).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbMonth).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMonth).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMonth).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMonth).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMonth).Location = new System.Drawing.Point(1187, 6);
		((System.Windows.Forms.Control)(object)this.CmbMonth).Name = "CmbMonth";
		((System.Windows.Forms.Control)(object)this.CmbMonth).Size = new System.Drawing.Size(102, 26);
		((System.Windows.Forms.Control)(object)this.CmbMonth).TabIndex = 108;
		((System.Windows.Forms.Control)(object)this.CmbMonth).Visible = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1139, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(43, 15);
		this.label3.TabIndex = 109;
		this.label3.Text = "Month";
		this.label3.Visible = false;
		this.CmbClassGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbClassGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbClassGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbClassGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbClassGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Location = new System.Drawing.Point(293, 5);
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Name = "CmbClassGroup";
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).TabIndex = 141;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(9, 11);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(91, 15);
		this.label7.TabIndex = 140;
		this.label7.Text = "Parent Category";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(223, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 142;
		this.label1.Text = "Class Group";
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(106, 5);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 139;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(861, 30);
		this.panel3.TabIndex = 5;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(144, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Purchase Report";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(861, 29);
		this.panel2.TabIndex = 4;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(71, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(861, 749);
		base.Controls.Add(this.panel1);
		base.Name = "MonthWisePurchaseReport";
		this.Text = "MonthWisePurchaseReport";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MonthWisePurchaseReport_FormClosing);
		base.Load += new System.EventHandler(MonthWisePurchaseReport_Load);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.chart1).EndInit();
		this.MonthAmountPanel.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		this.QuarterlyPanel.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.chart2).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMonth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClassGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
