using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Common;
using Architecture.BLL.Inventory;
using Architecture.BLL.SystemUtilities;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SystemUtilities;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.AnalyticDashboard;

public class PurchaseAnalyticPeriodicItemWise : Form
{
	public UserAccount UserAccount = new UserAccount();

	private bool isScheduleFound;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtParent = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	public int ParentCategoryId;

	public bool RequestedByOtherDocument;

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

	private Label label6;

	public UltraCombo CmbParentCategory;

	public PurchaseAnalyticPeriodicItemWise(UserAccount pUserAccount)
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
			ParentCategoryComboFill();
			((Control)(object)CmbParentCategory).Enabled = true;
			if (RequestedByOtherDocument)
			{
				DateFrom.Value = FromDate;
				DateTo.Value = ToDate;
				string ParentCategory = "";
				DataRow parentRow = dtParent.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == ParentCategoryId);
				if (parentRow != null)
				{
					ParentCategory = parentRow.Field<string>("InvParentCateDescription");
				}
				((Control)(object)CmbParentCategory).Text = ParentCategory;
				((Control)(object)CmbParentCategory).Enabled = false;
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryComboFill()
	{
		try
		{
			dtParent.Rows.Clear();
			dtParent = CommonServies.GetParentCategoriesForPurchaseAndSaleReports(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dtParent.Rows.Count > 0)
			{
				DDL.BindDDL(dtParent, CmbParentCategory, "Id", "InvParentCateDescription", "Parent Categories", ZeroIndex: false);
				((UltraGridBase)CmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				UltraGridColumn obj = ((UltraGridBase)CmbParentCategory).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbParentCategory.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbParentCategory.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbParentCategory.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbParentCategory.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
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
				DateFrom.Value = Conversion.ToDateTime(dt.Rows[0]["SeasonStartDate"]);
				datSeasonEndDate.Value = Conversion.ToDateTime(dt.Rows[0]["SeasonEndDate"]);
				DateTo.Value = Conversion.ToDateTime(dt.Rows[0]["SeasonEndDate"]);
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
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		try
		{
			if (!isScheduleFound)
			{
				return;
			}
			string Ids = "";
			if (((Control)(object)CmbParentCategory).Text != null && ((Control)(object)CmbParentCategory).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbParentCategory).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtParent.Select("InvParentCateDescription='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						Ids = Ids + Conversion.ToString(dr[0].ItemArray[0]) + ",";
					}
				}
			}
			if (string.IsNullOrEmpty(Ids))
			{
				throw new Exception("Please Select a Parent Category");
			}
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = DateFrom.Value,
				ToDate = DateTo.Value,
				StartDate = datSeasonStartDate.Value,
				EndDate = datSeasonEndDate.Value,
				Ids = ((!RequestedByOtherDocument) ? Ids : ParentCategoryId.ToString()),
				Activity = "Product_Wise"
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
				Cards[i].RequestFor = "Product_Wise";
				Cards[i].ParentIds = ((!RequestedByOtherDocument) ? Ids : ParentCategoryId.ToString());
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
					Cards[i].dataTable.Rows.Add(distinctData.Rows[j]["SortNo"], "", Description, distinctData.Rows[j]["Qty"], distinctData.Rows[j]["NetWeight"], distinctData.Rows[j]["Rate"], distinctData.Rows[j]["Exp40Kg"], distinctData.Rows[j]["AvgRate"], 0, distinctData.Rows[j]["AnalysisQty"], distinctData.Rows[j]["Moisture"], distinctData.Rows[j]["Empty Shell / Trash"], distinctData.Rows[j]["Dust/Stone"], distinctData.Rows[j]["Broken"], distinctData.Rows[j]["AGL"], distinctData.Rows[j]["Damage"]);
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			PurchaseAnalyticCard obj1 = (PurchaseAnalyticCard)sender;
			int ItemId = obj1.Id;
			_ = obj1.Caption;
			if (obj1.LableClicked)
			{
				return;
			}
			GridEXRow item = obj1.grdfrm.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			string Ids = "";
			if (((Control)(object)CmbParentCategory).Text != null && ((Control)(object)CmbParentCategory).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbParentCategory).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtParent.Select("InvParentCateDescription='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						Ids = Ids + Conversion.ToString(dr[0].ItemArray[0]) + ",";
					}
				}
			}
			PurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards = new PurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards(UserAccount);
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.WindowState = FormWindowState.Maximized;
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.FromDate = DateFrom.Value;
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.ToDate = DateTo.Value;
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.SeasonStartDate = datSeasonStartDate.Value;
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.SeasonEndDate = datSeasonEndDate.Value;
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.SortNo = Conversion.ToInt(item.Cells["SortNo"].Value);
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.ItemId = ItemId;
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.Ids = ((!RequestedByOtherDocument) ? Ids : ParentCategoryId.ToString());
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.ShowDialog();
			purchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.BringToFront();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AnalyticDashboard.PurchaseAnalyticPeriodicItemWise));
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
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnSeasonYearSchedule = new System.Windows.Forms.ToolStripButton();
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
		this.CmbParentCategory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.tsItemType.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnnew, this.btnSeasonYearSchedule });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(909, 27);
		this.tsItemType.TabIndex = 4;
		this.tsItemType.Text = "toolStrip4";
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
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 27);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(909, 27);
		this.panel5.TabIndex = 20;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(374, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Purchase Analytic Periodic Item Wise Report";
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
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
		this.groupBox1.Size = new System.Drawing.Size(755, 70);
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
		this.btnshow.Location = new System.Drawing.Point(691, 38);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 26);
		this.btnshow.TabIndex = 5;
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
		this.panel1.Size = new System.Drawing.Size(909, 75);
		this.panel1.TabIndex = 22;
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 129);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(909, 426);
		this.flowLayoutPanel1.TabIndex = 23;
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(508, 38);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(508, 20);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(91, 15);
		this.label6.TabIndex = 215;
		this.label6.Text = "Parent Category";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(909, 555);
		base.Controls.Add(this.flowLayoutPanel1);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.tsItemType);
		base.Name = "PurchaseAnalyticPeriodicItemWise";
		this.Text = "PurchaseAnalyticPeriodic";
		base.Load += new System.EventHandler(PurchaseAnalyticPeriodic_Load);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
