using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.DynamicCards;

namespace Architecture.WinApp.AnalyticDashboard;

public class PurchaseAnalyticPeriodicPartyAndItemWise : Form
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

	private FlowLayoutPanel flowLayoutPanel1;

	public PurchaseAnalyticPeriodicPartyAndItemWise(UserAccount pUserAccount)
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
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(974, 788);
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
		//IL_009b: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
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
			};
			flowLayoutPanel1.Controls.Clear();
			DataTable dt = InvPurchaseInvoice.PurchaseAnalyticsAB_Report(val);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			List<int> distinctIds = (from row in dt.AsEnumerable()
				select Conversion.ToInt(row["ItemId"])).Distinct().ToList();
			PurchaseAnalyticCard[] Cards = new PurchaseAnalyticCard[distinctIds.Count()];
			int i;
			for (i = 0; i < Cards.Length; i++)
			{
				DataTable distinctData = (from row in dt.AsEnumerable()
					where row.Field<int>("ItemId") == Conversion.ToInt((object)distinctIds[i])
					select row).CopyToDataTable();
				Cards[i] = new PurchaseAnalyticCard(UserAccount);
				Cards[i].Caption = Conversion.ToString(distinctData.Rows[0]["ItemName"]);
				Cards[i].Id = Conversion.ToInt(distinctData.Rows[0]["ItemId"]);
				Cards[i].RequestFor = "ProductAndPartyWise";
				Cards[i].dataTable = new DataTable();
				if (Cards[i].dataTable.Columns.Count == 0)
				{
					Cards[i].dataTable.Columns.Add("SortNo", typeof(int));
					Cards[i].dataTable.Columns.Add("Supplier");
					Cards[i].dataTable.Columns.Add("Descriptions");
					Cards[i].dataTable.Columns.Add("Qty", typeof(double));
					Cards[i].dataTable.Columns.Add("Weight", typeof(double));
					Cards[i].dataTable.Columns.Add("ItemRate", typeof(double));
					Cards[i].dataTable.Columns.Add("Exp40Kg", typeof(double));
					Cards[i].dataTable.Columns.Add("AvgRate", typeof(double));
					Cards[i].dataTable.Columns.Add("Exp+Amount", typeof(double));
					Cards[i].dataTable.Columns.Add("AnalysisQty", typeof(double));
					Cards[i].dataTable.Columns.Add("Moisture", typeof(double));
					Cards[i].dataTable.Columns.Add("Empty Shell / Trash", typeof(double));
					Cards[i].dataTable.Columns.Add("Dust / Stone", typeof(double));
					Cards[i].dataTable.Columns.Add("Broken", typeof(double));
					Cards[i].dataTable.Columns.Add("AGL", typeof(double));
					Cards[i].dataTable.Columns.Add("Damage", typeof(double));
				}
				for (int j = 0; j < distinctData.Rows.Count; j++)
				{
					Cards[i].dataTable.Rows.Add(distinctData.Rows[j]["SortNo"], distinctData.Rows[j]["Suppplier"], distinctData.Rows[j]["Descriptions"], distinctData.Rows[j]["Qty"], distinctData.Rows[j]["NetWeight"], distinctData.Rows[j]["Rate"], distinctData.Rows[j]["Exp40Kg"], distinctData.Rows[j]["AvgRate"], distinctData.Rows[j]["AmountWithExp"], distinctData.Rows[j]["AnalysisQty"], distinctData.Rows[j]["Moisture"], distinctData.Rows[j]["Empty Shell / Trash"], distinctData.Rows[j]["Dust/Stone"], distinctData.Rows[j]["Broken"], distinctData.Rows[j]["AGL"], distinctData.Rows[j]["Damage"]);
				}
				flowLayoutPanel1.Controls.Add(Cards[i]);
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
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(909, 25);
		this.tsItemType.TabIndex = 4;
		this.tsItemType.Text = "toolStrip4";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 25);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(909, 27);
		this.panel5.TabIndex = 20;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(484, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Purchase Analytic Periodic Product And Party Wise Report";
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 52);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(909, 503);
		this.flowLayoutPanel1.TabIndex = 23;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(909, 555);
		base.Controls.Add(this.flowLayoutPanel1);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.tsItemType);
		base.Name = "PurchaseAnalyticPeriodicPartyAndItemWise";
		this.Text = "PurchaseAnalyticPeriodic";
		base.Load += new System.EventHandler(PurchaseAnalyticPeriodic_Load);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
