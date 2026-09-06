using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SmartApp;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class CompanyWiseAccountsDetail : Form
{
	public string Descrition = "";

	public int SortId;

	public DateTime FromDate;

	public DateTime ToDate;

	private List<Dashboard> Data = new List<Dashboard>();

	private DataTable dtExprt = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label lblHeading;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	public CompanyWiseAccountsDetail(UserAccount Puser, List<Dashboard> data)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
		Data = data;
	}

	public CompanyWiseAccountsDetail(UserAccount Puser, DataTable dt)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
		dtExprt = dt;
	}

	private void CompanyWiseAccountsDetail_Load(object sender, EventArgs e)
	{
		try
		{
			lblHeading.Text = Descrition;
			List<Dashboard> data = Data;
			if (data != null && data.Count > 0)
			{
				GenerateData();
			}
			else
			{
				GenerateDataForExport();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateData()
	{
		try
		{
			List<Dashboard> data = Data;
			if (data != null && data.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("CompanyId", typeof(int));
				dt.Columns.Add("CompanyName");
				dt.Columns.Add("Opening", typeof(double));
				dt.Columns.Add("Debit", typeof(double));
				dt.Columns.Add("Credit", typeof(double));
				dt.Columns.Add("Closing", typeof(double));
				for (int i = 0; i < Data.Count; i++)
				{
					dt.Rows.Add(Data[i].CompanyId, Data[i].Company, Data[i].Opening, Data[i].CurrDr, Data[i].CurrCr, Data[i].Closing);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grd.RootTable.Columns["CompanyId"].Visible = false;
				grd.RootTable.Columns["CompanyName"].ColumnType = (ColumnType)5;
				grd.ColumnAutoResize = true;
				grd.RootTable.Columns["CompanyName"].Width = 400;
				grd.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
				grd.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
				grd.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
				grd.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
				grd.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatboth;
				grd.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
				grd.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
				grd.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatboth;
				grd.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
				grd.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
				grd.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
				grd.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
				CommonServices.GridAutoAdjustmentNew(grd);
				ctrlGrdBar1_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GenerateDataForExport()
	{
		try
		{
			DataTable dataTable = dtExprt;
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("CompanyId", typeof(int));
			dt.Columns.Add("CompanyName", typeof(string));
			dt.Columns.Add("CurrencyId", typeof(int));
			dt.Columns.Add("FcyCode", typeof(string));
			dt.Columns.Add("LastExRate", typeof(double));
			dt.Columns.Add("FcyAmount", typeof(double));
			dt.Columns.Add("LcyAmount", typeof(double));
			foreach (DataRow row in dtExprt.Rows)
			{
				dt.Rows.Add(row["CompanyId"], row["CompName"], row["CurrencyId"], row["CurrencyCode"], row["LastExRate"], row["FcyAmount"], row["LcyAmount"]);
			}
			grd.DataSource = dt;
			grd.RetrieveStructure();
			grd.RootTable.Columns["CurrencyId"].Visible = false;
			grd.RootTable.Columns["CompanyId"].Visible = false;
			grd.RootTable.Columns["CompanyName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["CompanyName"].Width = 300;
			grd.RootTable.Columns["FcyCode"].Width = 120;
			grd.RootTable.Columns["LastExRate"].Width = 120;
			grd.RootTable.Columns["FcyAmount"].Width = 150;
			grd.RootTable.Columns["LcyAmount"].Width = 150;
			grd.RootTable.Columns["LastExRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LastExRate"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastExRate"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LcyAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			CommonServices.GridAutoAdjustmentNew(grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int companyId = Conversion.ToInt(grd.CurrentRow.Cells["CompanyId"].Value);
			if (e.Column.Key == "CompanyName")
			{
				if (SortId == 1)
				{
					CommonServices.GoToActivitySummaryFromLinkedEvent(FromDate, ToDate, companyId);
				}
				else if (SortId == 2)
				{
					CommonServices.GoToCashBalancesFromLinkedEvent(FromDate, ToDate, companyId);
				}
				else if (SortId == 3)
				{
					CommonServices.GoToBankBalancesFromLinkedEvent(FromDate, ToDate, companyId);
				}
				else if (SortId == 4)
				{
					CommonServices.GoToPayablesWithBillAmountFromLinkedEvent(FromDate, ToDate, companyId);
				}
				else if (SortId == 5)
				{
					CommonServices.GoToReceivablesFromLinkedEvent(FromDate, ToDate, companyId);
				}
				else if (SortId == 6)
				{
					CommonServices.GoToTradeCreditorsReportFromLinkedEvent(FromDate, ToDate, companyId);
				}
				else if (SortId == 7)
				{
					CommonServices.GoToTradeDebitorsReportFromLinkedEvent(FromDate, ToDate, companyId);
				}
				else if (SortId == 8)
				{
					CommonServices.GoToInventoryPayablesandReceivablesFromLinkedEvent(FromDate, ToDate, 0, companyId);
				}
				else if (SortId == 9)
				{
					CommonServices.GoToInventoryPayablesandReceivablesFromLinkedEvent(FromDate, ToDate, 1, companyId);
				}
				else if (SortId == 10)
				{
					FCYPayablesAndReceivablesRpt fCYPayablesAndReceivablesRpt = new FCYPayablesAndReceivablesRpt(UserAccount);
					fCYPayablesAndReceivablesRpt.RequestedByOtherDocument = true;
					fCYPayablesAndReceivablesRpt.fromdate.Value = FromDate;
					fCYPayablesAndReceivablesRpt.todate.Value = ToDate;
					fCYPayablesAndReceivablesRpt.ShowDialog();
					fCYPayablesAndReceivablesRpt.BringToFront();
				}
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
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.lblHeading = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.lblHeading);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(868, 387);
		this.panel1.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(827, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 113;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(868, 360);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.lblHeading.BackColor = System.Drawing.Color.Teal;
		this.lblHeading.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeading.ForeColor = System.Drawing.Color.White;
		this.lblHeading.Location = new System.Drawing.Point(0, 0);
		this.lblHeading.Name = "lblHeading";
		this.lblHeading.Size = new System.Drawing.Size(868, 27);
		this.lblHeading.TabIndex = 0;
		this.lblHeading.Text = "label1";
		this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(868, 387);
		base.Controls.Add(this.panel1);
		base.Name = "CompanyWiseAccountsDetail";
		this.Text = "CompanyWiseAccountsDetail";
		base.Load += new System.EventHandler(CompanyWiseAccountsDetail_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
	}
}
