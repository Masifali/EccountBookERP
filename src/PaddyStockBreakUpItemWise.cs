// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.Audit_Dashboard
{

public class PaddyStockBreakUpItemWise : Form
{
	public bool linkFlag;

	public int SortNo;

	public string ActivityName = "";

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtReportData = new DataTable();

	private DataTable OriginalDataForCompare = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label lblCaption;

	private ToolStrip toolStrip1;

	private Panel panel3;

	private Panel panel5;

	private Label label4;

	private Label label2;

	private Panel panel2;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private ToolStripButton toolStripButton1;

	public DateTimePicker ToDate;

	public DateTimePicker FromDate;

	private ToolStripButton btnPrint;

	private ToolStripButton btnSave;

	public PaddyStockBreakUpItemWise(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
			OriginalDataForCompare.Rows.Clear();
			grdfrm.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaddyStockBreakUpItemWise_Load(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			if (!linkFlag)
			{
				FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				ToDate.Value = DateTime.Now;
				FromDate.Checked = false;
			}
			else
			{
				lblCaption.Text = ((SortNo == 3) ? "Paddy Stock Break Up Item Wise" : (ActivityName + " Break Up Item Wise Report "));
				GridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		try
		{
			dtReportData.Rows.Clear();
			OriginalDataForCompare.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (FromDate.Checked)
			{
				obj.FromDate = FromDate.Value;
			}
			obj.ToDate = ToDate.Value;
			if (SortNo == 3)
			{
				dtReportData = InventoryStockEvalautionDetail.GetPurchaseAndProductionFromPaddy(obj);
			}
			else
			{
				obj.Activity = "Detail";
				obj.SortNo = SortNo;
				dtReportData = InventoryStockEvalautionDetail.TotalStockBreakup_InAndOut_Summary(obj);
			}
			DataTable dataTable = dtReportData;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("Id", typeof(int));
				table.Columns.Add("SortNo", typeof(int));
				table.Columns.Add("TransType", typeof(string));
				table.Columns.Add("ParentCategory", typeof(string));
				table.Columns.Add("ClassGroup", typeof(string));
				table.Columns.Add("ItemId", typeof(int));
				table.Columns.Add("ItemName", typeof(string));
				table.Columns.Add("ItemQty", typeof(double));
				table.Columns.Add("Weight", typeof(double));
				table.Columns.Add("Amount", typeof(double));
				table.Columns.Add("AvgRate", typeof(double));
				table.Columns.Add("ManualRate/40Kg", typeof(decimal));
				table.Columns.Add("ManualAmount", typeof(decimal));
				table.Columns.Add("Recovery%", typeof(decimal));
				int count = 1;
				foreach (DataRow row in dtReportData.Rows)
				{
					table.Rows.Add(count++, row["SortNo"], row["TransType"], row["ParentCategory"], row["ClassGroup"], row["ItemId"], row["ItemName"], row["ItemQty"], row["Weight"], row["Amount"], row["AvgRate"], Conversion.ToDecimal(row["CustomRate"]), Conversion.ToDecimal(row["CustomAmount"]), Conversion.ToDecimal(row["RecoveryPercent"]));
				}
				grdfrm.DataSource = table;
				OriginalDataForCompare = table.Copy();
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
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["SortNo"].Visible = false;
			grdfrm.RootTable.Columns["ItemId"].Visible = false;
			grdfrm.RootTable.Columns["ClassGroup"].Visible = false;
			grdfrm.RootTable.Columns["Recovery%"].Visible = SortNo == 3;
			if (SortNo != 3)
			{
				grdfrm.RootTable.Columns["ManualRate/40Kg"].Visible = false;
				grdfrm.RootTable.Columns["ManualAmount"].Visible = false;
			}
			else
			{
				grdfrm.RootTable.Columns["ManualRate/40Kg"].Visible = true;
				grdfrm.RootTable.Columns["ManualAmount"].Visible = true;
			}
			grdfrm.RootTable.Groups.Add("TransType");
			grdfrm.RootTable.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			grdfrm.RootTable.Columns["ParentCategory"].Width = 80;
			grdfrm.RootTable.Columns["ItemName"].Width = 200;
			grdfrm.RootTable.Columns["ItemQty"].Width = 120;
			grdfrm.RootTable.Columns["Weight"].Width = 120;
			grdfrm.RootTable.Columns["Amount"].Width = 150;
			grdfrm.RootTable.Columns["AvgRate"].Width = 120;
			grdfrm.RootTable.HeaderLines = 2;
			grdfrm.RootTable.Columns["ParentCategory"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["SortNo"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["TransType"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ParentCategory"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ClassGroup"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["AvgRate"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ManualAmount"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Recovery%"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["SortNo"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["TransType"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ParentCategory"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ClassGroup"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ItemQty"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["AvgRate"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ManualAmount"].FilterEditType = (FilterEditType)1;
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
			grdfrm.RootTable.Columns["ManualAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["ManualAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ManualAmount"].FormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["ManualAmount"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AvgRate"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["AvgRate"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["Recovery%"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Recovery%"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["Recovery%"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["ManualRate/40Kg"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ManualRate/40Kg"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["ManualRate/40Kg"].TotalFormatString = "#,##0.##";
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
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtReportData, "843-PaddyStockBreakUpItemWise.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		try
		{
			if (grdfrm.GetDataRows().Count() <= 0)
			{
				return;
			}
			ProductionFromPaddyManualRate obj = new ProductionFromPaddyManualRate();
			obj.ProductionFromPaddyManualRatesList = new List<ProductionFromPaddyManualRate>();
			((Control)(object)grdfrm).Refresh();
			List<DataRow> SaveList = ((DataTable)grdfrm.DataSource).AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow = OriginalDataForCompare.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow != null && originalRow.Field<decimal>("ManualRate/40Kg") != dataRow.Field<decimal>("ManualRate/40Kg");
			}).ToList();
			if (SaveList.Count > 0)
			{
				for (int i = 0; i < SaveList.Count; i++)
				{
					ProductionFromPaddyManualRate d = new ProductionFromPaddyManualRate();
					d.CompanyId = UserAccount.CompanyId;
					d.OrganizationId = UserAccount.OrganizationId;
					d.EntryUserId = UserAccount.ID;
					d.ModifyUserId = UserAccount.ID;
					d.ItemId = Conversion.ToInt(SaveList[i]["ItemId"]);
					d.ItemQty = Math.Abs(Conversion.ToDecimal(SaveList[i]["ItemQty"]));
					d.Weight = Math.Abs(Conversion.ToDecimal(SaveList[i]["Weight"]));
					d.RateUom = 40m;
					d.ItemRate = Math.Abs(Conversion.ToDecimal(SaveList[i]["ManualRate/40Kg"]));
					d.Amount = Math.Abs(Conversion.ToDecimal(SaveList[i]["ManualAmount"]));
					obj.ProductionFromPaddyManualRatesList.Add(d);
				}
				ProductionFromPaddyManualRate.Save(obj);
				MessageBox.Show("Successfully Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				GridFill();
			}
			else
			{
				MessageBox.Show("No Change found!!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdfrm.UpdateData();
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "ManualRate/40Kg")
			{
				if (Conversion.ToInt(r.Cells["SortNo"].Value) != 3)
				{
					r.Cells["ManualRate/40Kg"].Value = 0;
					throw new Exception("You can only give rate to TransactionType: 'Production From Paddy'");
				}
				double Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
				double Rate = Conversion.ToDouble(r.Cells["ManualRate/40Kg"].Value);
				int RateUOM = 40;
				if (Weight != 0.0 && Rate != 0.0)
				{
					double ItemAmount = Math.Abs(Weight / (double)RateUOM * Rate);
					r.Cells["ManualAmount"].Value = ItemAmount;
				}
				else
				{
					r.Cells["ManualAmount"].Value = 0;
				}
			}
			grdfrm.UpdateData();
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.grdfrm = new GridEX();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
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
		this.panel1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(916, 30);
		this.panel1.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripButton1, this.btnPrint, this.btnSave });
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
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 4;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.CellUpdated += new ColumnActionEventHandler(grdfrm_CellUpdated);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(4, 2);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(277, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Paddy Stock Break Up Item Wise";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.lblCaption);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(916, 27);
		this.panel3.TabIndex = 3;
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
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(287, 24);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 5;
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
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "Print-843";
		this.btnPrint.ToolTipText = "843-PaddyStockBreakUpItemWise";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(868, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(916, 610);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel1);
		base.Name = "PaddyStockBreakUpItemWise";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StockBreakupInAndOutSummary_FormClosing);
		base.Load += new System.EventHandler(PaddyStockBreakUpItemWise_Load);
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

}
