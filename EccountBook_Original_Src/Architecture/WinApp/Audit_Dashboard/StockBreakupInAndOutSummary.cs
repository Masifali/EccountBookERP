using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Audit_Dashboard;

public class StockBreakupInAndOutSummary : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtReportData = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label lblCaption;

	private ToolStrip toolStrip1;

	private Panel panel3;

	private Panel panel5;

	private Label label4;

	private Label label2;

	private DateTimePicker ToDate;

	private DateTimePicker FromDate;

	private Panel panel2;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private ToolStripButton toolStripButton1;

	private RadioButton RdCustomRateAmount;

	private RadioButton RdSystemRateAmount;

	private ToolStripButton btnPrint;

	private ToolStripButton btnGenerate;

	private Label label1;

	private TextBox txtShortPercent;

	public StockBreakupInAndOutSummary(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Reset()
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			ToDate.Value = DateTime.Now;
			FromDate.Checked = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockBreakupInAndOutSummary_Load(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			ToDate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtReportData.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (FromDate.Checked)
			{
				obj.FromDate = FromDate.Value;
			}
			obj.ToDate = ToDate.Value;
			obj.ActionId = (RdCustomRateAmount.Checked ? 2 : 0);
			obj.Activity = "Summary";
			dtReportData = InventoryStockEvalautionDetail.TotalStockBreakup_InAndOut_Summary(obj);
			DataTable dataTable = dtReportData;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("SortNo", typeof(int));
				table.Columns.Add("TransType", typeof(string));
				table.Columns.Add("Activity", typeof(string));
				table.Columns.Add("ParentCategory", typeof(string));
				table.Columns.Add("ClassGroup", typeof(string));
				table.Columns.Add("ItemQty", typeof(double));
				table.Columns.Add("Weight", typeof(double));
				table.Columns.Add("Amount", typeof(double));
				table.Columns.Add("AvgRate", typeof(double));
				table.Columns.Add("Recovery%", typeof(double));
				foreach (DataRow row in dtReportData.Rows)
				{
					table.Rows.Add(row["SortNo"], row["TransType"], row["Activity"], row["ParentCategory"], row["ClassGroup"], row["ItemQty"], row["Weight"], row["Amount"], row["AvgRate"], row["RecoveryPercent"]);
				}
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
				gridSetting();
			}
			else
			{
				grdfrm.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridSetting()
	{
		try
		{
			grdfrm.RootTable.Columns["SortNo"].Visible = false;
			grdfrm.RootTable.Columns["ParentCategory"].Visible = false;
			grdfrm.RootTable.Groups.Add("TransType");
			grdfrm.RootTable.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			grdfrm.RootTable.Columns["ClassGroup"].Width = 120;
			grdfrm.RootTable.Columns["Activity"].Width = 150;
			grdfrm.RootTable.Columns["ItemQty"].Width = 120;
			grdfrm.RootTable.Columns["Weight"].Width = 120;
			grdfrm.RootTable.Columns["Amount"].Width = 150;
			grdfrm.RootTable.Columns["AvgRate"].Width = 120;
			grdfrm.RootTable.HeaderLines = 2;
			grdfrm.RootTable.Columns["Activity"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["ClassGroup"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ItemQty"].FormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["ItemQty"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Weight"].FormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["Weight"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Amount"].FormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["Amount"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AvgRate"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["AvgRate"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["Recovery%"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Recovery%"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["Recovery%"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns.Add("Select");
			grdfrm.RootTable.Columns["Select"].ActAsSelector = true;
			grdfrm.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdfrm.RootTable.Columns["Select"].Width = 30;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
			GridFill();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
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

	private void StockBreakupInAndOutSummary_FormClosing(object sender, FormClosingEventArgs e)
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

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdfrm.CurrentRow;
			int SortNo = Conversion.ToInt(currentRow.Cells["SortNo"].Value);
			string Activity = Conversion.ToString(currentRow.Cells["Activity"].Value);
			if (e.Column.Key == "Activity" && SortNo != 7)
			{
				PaddyStockBreakUpItemWise paddyStockBreakUpItemWise = new PaddyStockBreakUpItemWise(UserAccount);
				paddyStockBreakUpItemWise.linkFlag = true;
				paddyStockBreakUpItemWise.SortNo = SortNo;
				paddyStockBreakUpItemWise.ActivityName = Activity;
				paddyStockBreakUpItemWise.FromDate.Value = FromDate.Value;
				paddyStockBreakUpItemWise.FromDate.Checked = FromDate.Checked;
				paddyStockBreakUpItemWise.ToDate.Value = ToDate.Value;
				paddyStockBreakUpItemWise.Show();
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
			if (dtReportData == null || dtReportData.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtReportData, "842-StockBreakUpInOutSummary.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtShortPercent.Text.Trim()) == 0.0)
			{
				txtShortPercent.Focus();
				throw new Exception("Short Percent Field is Required");
			}
			if (grdfrm.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please check the row first...");
			}
			double TotalAmount = 0.0;
			double TotalWeight = 0.0;
			double ShortPercent = Conversion.ToDouble((object)txtShortPercent.Text.Trim());
			GridEXRow[] checkedRows = grdfrm.GetCheckedRows();
			foreach (GridEXRow item2 in checkedRows)
			{
				TotalWeight += Conversion.ToDouble(item2.Cells["Weight"].Value);
				TotalAmount += Conversion.ToDouble(item2.Cells["Amount"].Value);
			}
			if (TotalWeight > 0.0 && ShortPercent > 0.0)
			{
				double AvgRate = TotalAmount / TotalWeight;
				double TotalShortWeight = Math.Round(TotalWeight * ShortPercent / 100.0, 2);
				double TotalQty = TotalShortWeight / 100.0;
				double TotalShortAmount = Math.Round(TotalShortWeight * AvgRate, 2);
				double ShortAvgRate = Math.Round(TotalShortAmount / TotalShortWeight * 40.0, 2);
				foreach (DataRow row in dtReportData.Rows)
				{
					if (Convert.ToInt32(row["SortNo"]) == 7)
					{
						row["ItemQty"] = 0.0 - TotalQty;
						row["Weight"] = 0.0 - TotalShortWeight;
						row["Amount"] = 0.0 - TotalShortAmount;
						row["AvgRate"] = ShortAvgRate;
					}
				}
				checkedRows = grdfrm.GetDataRows();
				foreach (GridEXRow item in checkedRows)
				{
					if (Conversion.ToInt(item.Cells["SortNo"].Value) == 7)
					{
						item.BeginEdit();
						item.Cells["ItemQty"].Value = Conversion.ToDouble((object)(0.0 - TotalQty)).ToString();
						item.Cells["Weight"].Value = Conversion.ToDouble((object)(0.0 - TotalShortWeight)).ToString();
						item.Cells["Amount"].Value = Conversion.ToDouble((object)(0.0 - TotalShortAmount)).ToString();
						item.Cells["AvgRate"].Value = ShortAvgRate.ToString();
						item.EndEdit();
					}
				}
			}
			grdfrm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtShortPercent_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.grdfrm = new GridEX();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.RdCustomRateAmount = new System.Windows.Forms.RadioButton();
		this.RdSystemRateAmount = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnGenerate = new System.Windows.Forms.ToolStripButton();
		this.txtShortPercent = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(916, 30);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripButton1, this.btnPrint, this.btnGenerate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(916, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "Print-842";
		this.btnPrint.ToolTipText = "842-StockBreakUpInOutSummary";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(916, 494);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(4, 2);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(630, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Stock Opening,Purchase,Production From Paddy Process and Sales Summary";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.lblCaption);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(916, 27);
		this.panel3.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(868, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.txtShortPercent);
		this.panel5.Controls.Add(this.RdCustomRateAmount);
		this.panel5.Controls.Add(this.RdSystemRateAmount);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.ToDate);
		this.panel5.Controls.Add(this.FromDate);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 57);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(916, 59);
		this.panel5.TabIndex = 4;
		this.RdCustomRateAmount.AutoSize = true;
		this.RdCustomRateAmount.Location = new System.Drawing.Point(457, 32);
		this.RdCustomRateAmount.Name = "RdCustomRateAmount";
		this.RdCustomRateAmount.Size = new System.Drawing.Size(347, 20);
		this.RdCustomRateAmount.TabIndex = 5;
		this.RdCustomRateAmount.Text = "Production From Paddy (Custom Rate && Amount)";
		this.RdCustomRateAmount.UseVisualStyleBackColor = true;
		this.RdSystemRateAmount.AutoSize = true;
		this.RdSystemRateAmount.Checked = true;
		this.RdSystemRateAmount.Location = new System.Drawing.Point(457, 8);
		this.RdSystemRateAmount.Name = "RdSystemRateAmount";
		this.RdSystemRateAmount.Size = new System.Drawing.Size(347, 20);
		this.RdSystemRateAmount.TabIndex = 4;
		this.RdSystemRateAmount.TabStop = true;
		this.RdSystemRateAmount.Text = "Production From Paddy (System Rate && Amount)";
		this.RdSystemRateAmount.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(364, 24);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnSearch_Click);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(147, 8);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 61;
		this.label4.Text = "Date To";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 8);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 15);
		this.label2.TabIndex = 59;
		this.label2.Text = "Date From";
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(150, 27);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(134, 23);
		this.ToDate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(13, 27);
		this.FromDate.Name = "FromDate";
		this.FromDate.ShowCheckBox = true;
		this.FromDate.Size = new System.Drawing.Size(134, 23);
		this.FromDate.TabIndex = 0;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 116);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(916, 494);
		this.panel2.TabIndex = 5;
		this.btnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.Size = new System.Drawing.Size(71, 24);
		this.btnGenerate.Text = "Generate";
		this.btnGenerate.Click += new System.EventHandler(btnGenerate_Click);
		this.txtShortPercent.Location = new System.Drawing.Point(287, 27);
		this.txtShortPercent.Name = "txtShortPercent";
		this.txtShortPercent.Size = new System.Drawing.Size(73, 23);
		this.txtShortPercent.TabIndex = 2;
		this.txtShortPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtShortPercent_KeyPress);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(284, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(49, 15);
		this.label1.TabIndex = 65;
		this.label1.Text = "Short %";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(916, 610);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel1);
		base.Name = "StockBreakupInAndOutSummary";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StockBreakupInAndOutSummary_FormClosing);
		base.Load += new System.EventHandler(StockBreakupInAndOutSummary_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
