using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.SystemUtilities;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SystemUtilities;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.AnalyticDashboard;

public class PurchaseAnalyticPeriodic : Form
{
	public UserAccount UserAccount = new UserAccount();

	private bool isScheduleFound;

	private IContainer components;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private Panel panel5;

	private Label label3;

	private GroupBox groupBox1;

	public DateTimePicker DateFrom;

	private Button btnshow;

	private Label label5;

	private Label label4;

	public DateTimePicker DateTo;

	private Panel panel1;

	public DateTimePicker datSeasonStartDate;

	private Label label1;

	private Label label2;

	public DateTimePicker datSeasonEndDate;

	private ToolStripButton btnSeasonYearSchedule;

	private FlowLayoutPanel flowLayoutPanel1;

	public PurchaseAnalyticPeriodic(UserAccount pUserAccount)
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
			GetSeasonScheduleDates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSeasonScheduleDates()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SeasonYearSchedule.GetAll(new SeasonYearSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				isScheduleFound = true;
				datSeasonStartDate.Value = Conversion.ToDateTime(dt.Rows[0]["SeasonStartDate"]);
				datSeasonEndDate.Value = Conversion.ToDateTime(dt.Rows[0]["SeasonEndDate"]);
				DateFrom.Value = Conversion.ToDateTime(dt.Rows[0]["SeasonStartDate"]);
				DateTo.Value = DateTime.Now;
				btnshow.Enabled = true;
			}
			else
			{
				isScheduleFound = false;
				btnshow.Enabled = false;
				MessageBox.Show("No Schedule found. Please Make a season year schedule first!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DateFrom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (isScheduleFound)
			{
				if (DateFrom.Value.Date < datSeasonStartDate.Value.Date)
				{
					DateFrom.Value = datSeasonStartDate.Value;
					MessageBox.Show("FromDate can't be less than season start date!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if (DateFrom.Value.Date > datSeasonEndDate.Value.Date)
				{
					DateFrom.Value = datSeasonStartDate.Value;
					MessageBox.Show("FromDate can't be greater than season end date!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else
			{
				MessageBox.Show("No Schedule found. Please Make a season year schedule first!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DateTo_ValueChanged(object sender, EventArgs e)
	{
		if (isScheduleFound)
		{
			if (DateTo.Value.Date < datSeasonStartDate.Value.Date)
			{
				DateTo.Value = datSeasonEndDate.Value;
				MessageBox.Show("ToDate can't be less than season start date!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (DateTo.Value.Date > datSeasonEndDate.Value.Date)
			{
				DateTo.Value = datSeasonEndDate.Value;
				MessageBox.Show("ToDate can't be greater than season end date!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		else
		{
			MessageBox.Show("No Schedule found. Please Make a season year schedule first!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		try
		{
			if (!isScheduleFound)
			{
				return;
			}
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = DateFrom.Value,
				ToDate = DateTo.Value,
				StartDate = datSeasonStartDate.Value,
				EndDate = datSeasonEndDate.Value
			};
			flowLayoutPanel1.Controls.Clear();
			DataTable dt = InvPurchaseInvoice.PurchaseAnalyticsA_Report(val);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			List<int> distinctIds = (from row in dt.AsEnumerable()
				select Conversion.ToInt(row["IpcId"])).Distinct().ToList();
			PurchaseAnalyticCard[] Cards = new PurchaseAnalyticCard[distinctIds.Count()];
			int i;
			for (i = 0; i < Cards.Length; i++)
			{
				DataTable distinctData = (from row in dt.AsEnumerable()
					where row.Field<int>("IpcId") == Conversion.ToInt((object)distinctIds[i])
					select row).CopyToDataTable();
				Cards[i] = new PurchaseAnalyticCard(UserAccount);
				Cards[i].Caption = Conversion.ToString(distinctData.Rows[0]["ParentCategory"]);
				Cards[i].Id = Conversion.ToInt(distinctData.Rows[0]["IpcId"]);
				Cards[i].ParentIds = Conversion.ToString(distinctData.Rows[0]["IpcId"]);
				Cards[i].RequestFor = "Product_Wise_Summ";
				Cards[i].lblTitle.LinkBehavior = LinkBehavior.AlwaysUnderline;
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
					string Description = Conversion.ToString(distinctData.Rows[j]["Descriptions"]);
					if (Description == "This Season")
					{
						Description = "Current Season";
					}
					Cards[i].dataTable.Rows.Add(distinctData.Rows[j]["SortNo"], "", Description, distinctData.Rows[j]["Qty"], distinctData.Rows[j]["NetWeight"], distinctData.Rows[j]["Rate"], distinctData.Rows[j]["Exp40Kg"], distinctData.Rows[j]["AvgRate"], 0, 0, 0, 0, 0, 0, 0, 0);
				}
				Cards[i].UserControlClicked += UserControl_Click;
				flowLayoutPanel1.Controls.Add(Cards[i]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			PurchaseAnalyticCard obj1 = (PurchaseAnalyticCard)sender;
			int parentId = obj1.Id;
			_ = obj1.Caption;
			if (!obj1.LableClicked)
			{
				if (Conversion.ToString(obj1.ActiveControl.Tag) == "GrdClick")
				{
					GridEXRow item = obj1.grdfrm.CurrentRow;
					if (item != null && (int)item.RowType == 0)
					{
						PurchaseAnalyticPeriodicPartyAndItemWise purchaseAnalyticPeriodicPartyAndItemWise = new PurchaseAnalyticPeriodicPartyAndItemWise(UserAccount);
						purchaseAnalyticPeriodicPartyAndItemWise.WindowState = FormWindowState.Maximized;
						purchaseAnalyticPeriodicPartyAndItemWise.FromDate = DateFrom.Value;
						purchaseAnalyticPeriodicPartyAndItemWise.ToDate = DateTo.Value;
						purchaseAnalyticPeriodicPartyAndItemWise.SeasonStartDate = datSeasonStartDate.Value;
						purchaseAnalyticPeriodicPartyAndItemWise.SeasonEndDate = datSeasonEndDate.Value;
						purchaseAnalyticPeriodicPartyAndItemWise.SortNo = Conversion.ToInt(item.Cells["SortNo"].Value);
						purchaseAnalyticPeriodicPartyAndItemWise.Ids = parentId.ToString();
						purchaseAnalyticPeriodicPartyAndItemWise.ShowDialog();
						purchaseAnalyticPeriodicPartyAndItemWise.BringToFront();
					}
				}
			}
			else
			{
				PurchaseAnalyticPeriodicItemWise purchaseAnalyticPeriodicItemWise = new PurchaseAnalyticPeriodicItemWise(UserAccount);
				purchaseAnalyticPeriodicItemWise.WindowState = FormWindowState.Maximized;
				purchaseAnalyticPeriodicItemWise.RequestedByOtherDocument = true;
				purchaseAnalyticPeriodicItemWise.FromDate = DateFrom.Value;
				purchaseAnalyticPeriodicItemWise.ToDate = DateTo.Value;
				purchaseAnalyticPeriodicItemWise.ParentCategoryId = parentId;
				purchaseAnalyticPeriodicItemWise.ShowDialog();
				purchaseAnalyticPeriodicItemWise.BringToFront();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSeasonYearSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			SeasonYearSchedule seasonYearSchedule = new SeasonYearSchedule(UserAccount);
			seasonYearSchedule.StartPosition = FormStartPosition.CenterScreen;
			seasonYearSchedule.BringToFront();
			seasonYearSchedule.ShowDialog();
			GetSeasonScheduleDates();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AnalyticDashboard.PurchaseAnalyticPeriodic));
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.datSeasonStartDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.datSeasonEndDate = new System.Windows.Forms.DateTimePicker();
		this.DateFrom = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.DateTo = new System.Windows.Forms.DateTimePicker();
		this.panel1 = new System.Windows.Forms.Panel();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnSeasonYearSchedule = new System.Windows.Forms.ToolStripButton();
		this.tsItemType.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnnew, this.btnSeasonYearSchedule });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(811, 27);
		this.tsItemType.TabIndex = 4;
		this.tsItemType.Text = "toolStrip4";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 27);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(811, 27);
		this.panel5.TabIndex = 20;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(286, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Purchase Analytic Periodic Report";
		this.groupBox1.Controls.Add(this.datSeasonStartDate);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.datSeasonEndDate);
		this.groupBox1.Controls.Add(this.DateFrom);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.DateTo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(572, 70);
		this.groupBox1.TabIndex = 21;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.datSeasonStartDate.CustomFormat = "dd-MMM-yy";
		this.datSeasonStartDate.Enabled = false;
		this.datSeasonStartDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datSeasonStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datSeasonStartDate.Location = new System.Drawing.Point(253, 40);
		this.datSeasonStartDate.Name = "datSeasonStartDate";
		this.datSeasonStartDate.Size = new System.Drawing.Size(126, 23);
		this.datSeasonStartDate.TabIndex = 2;
		this.datSeasonStartDate.TabStop = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(381, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(96, 15);
		this.label1.TabIndex = 213;
		this.label1.Text = "Season End Date";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(253, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(101, 15);
		this.label2.TabIndex = 212;
		this.label2.Text = "Season Start Date";
		this.datSeasonEndDate.CustomFormat = "dd-MMM-yy";
		this.datSeasonEndDate.Enabled = false;
		this.datSeasonEndDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datSeasonEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datSeasonEndDate.Location = new System.Drawing.Point(381, 40);
		this.datSeasonEndDate.Name = "datSeasonEndDate";
		this.datSeasonEndDate.Size = new System.Drawing.Size(125, 23);
		this.datSeasonEndDate.TabIndex = 3;
		this.datSeasonEndDate.TabStop = false;
		this.DateFrom.CustomFormat = "dd-MMM-yy";
		this.DateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateFrom.Location = new System.Drawing.Point(10, 40);
		this.DateFrom.Name = "DateFrom";
		this.DateFrom.Size = new System.Drawing.Size(121, 23);
		this.DateFrom.TabIndex = 0;
		this.DateFrom.ValueChanged += new System.EventHandler(DateFrom_ValueChanged);
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.Location = new System.Drawing.Point(508, 38);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 26);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(133, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 203;
		this.label5.Text = "To Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(10, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 202;
		this.label4.Text = "From Date";
		this.DateTo.CustomFormat = "dd-MMM-yy";
		this.DateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateTo.Location = new System.Drawing.Point(133, 40);
		this.DateTo.Name = "DateTo";
		this.DateTo.Size = new System.Drawing.Size(119, 23);
		this.DateTo.TabIndex = 1;
		this.DateTo.ValueChanged += new System.EventHandler(DateTo_ValueChanged);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 54);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(811, 75);
		this.panel1.TabIndex = 22;
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 129);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(811, 426);
		this.flowLayoutPanel1.TabIndex = 23;
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnSeasonYearSchedule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnSeasonYearSchedule.Image = (System.Drawing.Image)resources.GetObject("btnSeasonYearSchedule.Image");
		this.btnSeasonYearSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSeasonYearSchedule.Name = "btnSeasonYearSchedule";
		this.btnSeasonYearSchedule.Size = new System.Drawing.Size(204, 24);
		this.btnSeasonYearSchedule.Text = "Define Season Year Schedule";
		this.btnSeasonYearSchedule.Click += new System.EventHandler(btnSeasonYearSchedule_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(811, 555);
		base.Controls.Add(this.flowLayoutPanel1);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.tsItemType);
		base.Name = "PurchaseAnalyticPeriodic";
		this.Text = "PurchaseAnalyticPeriodic";
		base.Load += new System.EventHandler(PurchaseAnalyticPeriodic_Load);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
