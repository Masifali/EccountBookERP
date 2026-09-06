using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Model;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.AnalyticDashboard;

public class PurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards : Form
{
	public UserAccount UserAccount = new UserAccount();

	public DateTime FromDate;

	public DateTime ToDate;

	public DateTime SeasonStartDate;

	public DateTime SeasonEndDate;

	public int SortNo;

	public int ItemId;

	public string Ids = "";

	private IContainer components;

	private ToolStrip tsItemType;

	private Panel panel5;

	private Label label3;

	private Label lblItemName;

	internal GridEX grd;

	public PurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void PurchaseAnalyticPeriodic_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(960, 500);
			GetData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		try
		{
			DataTable dt = InvPurchaseInvoice.PurchaseAnalyticsAB_Report(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDate,
				ToDate = ToDate,
				StartDate = SeasonStartDate,
				EndDate = SeasonEndDate,
				SortNo = SortNo,
				ItemId = ItemId,
				Ids = Ids,
				Activity = "ProductAndPartyWise"
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dataTable = new DataTable();
				dataTable.Columns.Add("Supplier");
				dataTable.Columns.Add("Qty", typeof(double));
				dataTable.Columns.Add("Weight", typeof(double));
				dataTable.Columns.Add("ItemRate", typeof(double));
				dataTable.Columns.Add("Exp40Kg", typeof(double));
				dataTable.Columns.Add("AvgRate", typeof(double));
				dataTable.Columns.Add("Exp+Amount", typeof(double));
				dataTable.Columns.Add("AnalysisQty", typeof(double));
				dataTable.Columns.Add("Moisture", typeof(double));
				dataTable.Columns.Add("Empty Shell / Trash", typeof(double));
				dataTable.Columns.Add("Dust/Stone", typeof(double));
				dataTable.Columns.Add("Broken", typeof(double));
				dataTable.Columns.Add("AGL", typeof(double));
				dataTable.Columns.Add("Damage", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					lblItemName.Text = dt.Rows[i]["ItemName"].ToString();
					dataTable.Rows.Add(dt.Rows[i]["Suppplier"], dt.Rows[i]["Qty"], dt.Rows[i]["NetWeight"], dt.Rows[i]["Rate"], dt.Rows[i]["Exp40Kg"], dt.Rows[i]["AvgRate"], dt.Rows[i]["AmountWithExp"], dt.Rows[i]["AnalysisQty"], dt.Rows[i]["Moisture"], dt.Rows[i]["Empty Shell / Trash"], dt.Rows[i]["Dust/Stone"], dt.Rows[i]["Broken"], dt.Rows[i]["AGL"], dt.Rows[i]["Damage"]);
				}
				grd.DataSource = dataTable;
				GridSettings();
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
			grd.RootTable.ColumnSets["AnalysisQty"].Visible = false;
			grd.RootTable.ColumnSets["Moisture"].Visible = false;
			grd.RootTable.ColumnSets["Damage"].Visible = false;
			grd.RootTable.ColumnSets["AGL"].Visible = false;
			grd.RootTable.ColumnSets["Broken"].Visible = false;
			grd.RootTable.ColumnSets["Dust/Stone"].Visible = false;
			grd.RootTable.ColumnSets["Empty Shell / Trash"].Visible = false;
			if (!string.IsNullOrEmpty(Ids) && Ids.Contains("1"))
			{
				grd.RootTable.ColumnSets["AnalysisQty"].Visible = true;
				grd.RootTable.ColumnSets["Moisture"].Visible = true;
				grd.RootTable.ColumnSets["Dust/Stone"].Visible = true;
				grd.RootTable.ColumnSets["Empty Shell / Trash"].Visible = true;
			}
			else if (!string.IsNullOrEmpty(Ids) && Ids.Contains("2"))
			{
				grd.RootTable.ColumnSets["AnalysisQty"].Visible = true;
				grd.RootTable.ColumnSets["Moisture"].Visible = true;
				grd.RootTable.ColumnSets["Damage"].Visible = true;
				grd.RootTable.ColumnSets["AGL"].Visible = true;
				grd.RootTable.ColumnSets["Broken"].Visible = true;
			}
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,#.##";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,#.##";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = "#,#.##";
			grd.RootTable.Columns["Exp40Kg"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Exp40Kg"].FormatString = "#,#.##";
			grd.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvgRate"].FormatString = "#,#.##";
			grd.RootTable.Columns["Exp+Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Exp+Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Exp+Amount"].FormatString = "#,#.##";
			grd.RootTable.Columns["Exp+Amount"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["AnalysisQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AnalysisQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AnalysisQty"].FormatString = "#,#.##";
			grd.RootTable.Columns["AnalysisQty"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["Moisture"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Moisture"].FormatString = "#,#.##";
			grd.RootTable.Columns["Damage"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Damage"].FormatString = "#,#.##";
			grd.RootTable.Columns["AGL"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AGL"].FormatString = "#,#.##";
			grd.RootTable.Columns["Broken"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Broken"].FormatString = "#,#.##";
			grd.RootTable.Columns["Dust/Stone"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Dust/Stone"].FormatString = "#,#.##";
			grd.RootTable.Columns["Empty Shell / Trash"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Empty Shell / Trash"].FormatString = "#,#.##";
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AnalyticDashboard.PurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards));
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblItemName = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(944, 25);
		this.tsItemType.TabIndex = 4;
		this.tsItemType.Text = "toolStrip4";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.lblItemName);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 25);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(944, 27);
		this.panel5.TabIndex = 20;
		this.lblItemName.AutoSize = true;
		this.lblItemName.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.lblItemName.ForeColor = System.Drawing.Color.White;
		this.lblItemName.Location = new System.Drawing.Point(505, 3);
		this.lblItemName.Name = "lblItemName";
		this.lblItemName.Size = new System.Drawing.Size(59, 19);
		this.lblItemName.TabIndex = 1;
		this.lblItemName.Text = "label1";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(484, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Purchase Analytic Periodic Product And Party Wise Report";
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 52);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(944, 409);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 86;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		((System.Windows.Forms.Control)(object)this.grd).Tag = "GrdClick";
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(944, 461);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.tsItemType);
		base.Name = "PurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards";
		this.Text = "PurchaseAnalyticPeriodic";
		base.Load += new System.EventHandler(PurchaseAnalyticPeriodic_Load);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
