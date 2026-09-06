using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class BankBalanceManualEntry : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtSummery = new DataTable();

	private Rightsobjects formRights = new Rightsobjects();

	private DataTable dtSourceBy = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Button btnshow;

	private DateTimePicker txtBalanceDate;

	private Label label27;

	private Panel panel7;

	internal GridEX grdHistory;

	private Panel panel8;

	private Label label5;

	private CtrlGrdBar ctrlGrdSummary;

	private Panel panel9;

	private Panel panel4;

	private Label label1;

	private ToolStripButton btnSave;

	public BankBalanceManualEntry(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void FrmBankBalancesRpt_Load(object sender, EventArgs e)
	{
		try
		{
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formRights.DoHaveSaveRight;
			SourceByBindFromStaticColumns();
			txtBalanceDate.Value = DateTime.Now;
			SummeryGrd();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SourceByBindFromStaticColumns()
	{
		try
		{
			dtSourceBy = CommonServices.StaticColumnsService("SourceBy");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SummeryGrd()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		try
		{
			dtSummery.Rows.Clear();
			DataSet ds = VoucherReports.CashandBankBalancesSummery(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CompanyIdCoa = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtBalanceDate.Value),
				ToDate = Conversion.ToDateTime((object)txtBalanceDate.Value),
				AccountTypeId = 15,
				ApprovedFilter = "All"
			});
			dtSummery = ds?.Tables[0];
			DataTable dtcol = new DataTable();
			dtcol.Columns.Add("Time", typeof(DateTime));
			dtcol.Columns.Add("AccountId");
			dtcol.Columns.Add("AccountCode");
			dtcol.Columns.Add("AccountTitle");
			dtcol.Columns.Add("ERPBalance", typeof(double));
			dtcol.Columns.Add("BankBalance", typeof(double));
			dtcol.Columns.Add("Source");
			dtcol.Columns.Add("ConfirmBy");
			for (int i = 0; i < dtSummery.Rows.Count; i++)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtSummery, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				DateTime dateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
				string formattedTime = "";
				formattedTime = ((!(Conversion.ToString(dtSummery.Rows[i]["BalanceTime"]) != string.Empty) || Conversion.ToString(dtSummery.Rows[i]["BalanceTime"]) == null) ? dateTime.ToString("hh:mm tt", CultureInfo.InvariantCulture) : Conversion.ToString(dtSummery.Rows[i]["BalanceTime"]));
				dtcol.Rows.Add(formattedTime, dtSummery.Rows[i]["AccountId"], dtSummery.Rows[i]["AccountCode"], dtSummery.Rows[i]["AccountTitle"], dtSummery.Rows[i]["Closing"], dtSummery.Rows[i]["ManualBankBalance"], dtSummery.Rows[i]["BySourceId"], dtSummery.Rows[i]["ConfirmedBy"]);
			}
			grdHistory.DataSource = dtcol;
			grdHistory.RetrieveStructure();
			GridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdHistory.RootTable.Columns["AccountId"].Visible = false;
			grdHistory.RootTable.Columns["AccountTitle"].Width = 350;
			grdHistory.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["ERPBalance"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["ConfirmBy"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["Source"].ColumnType = (ColumnType)0;
			grdHistory.RootTable.Columns["Source"].HasValueList = true;
			grdHistory.RootTable.Columns["Source"].LimitToList = true;
			grdHistory.RootTable.Columns["Source"].EditType = (EditType)4;
			grdHistory.RootTable.Columns["Source"].ValueList.PopulateValueList((IEnumerable)dtSourceBy.DefaultView, "Id", "type");
			grdHistory.RootTable.Columns["Time"].FormatString = "hh:mm tt";
			grdHistory.RootTable.Columns["Time"].EditType = (EditType)3;
			grdHistory.RootTable.Columns["BankBalance"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BankBalance"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BankBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["BankBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["ERPBalance"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["ERPBalance"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["ERPBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["ERPBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			ctrlGrdSummary_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			SummeryGrd();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintSummery_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSummery.Rows.Count > 0 && dtSummery != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummery, "593-CashBankBalancesSummery_Rpt.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			txtBalanceDate.Focus();
			SummeryGrd();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(grdHistory.CurrentRow.Cells["AccountId"].Value.ToString()), txtBalanceDate.Value, txtBalanceDate.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void ctrlGrdSummary_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankBalances_FormClosing(object sender, FormClosingEventArgs e)
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

	private void grdHistory_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "BankBalance" && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		try
		{
			if (grdHistory.GetRows().Count() <= 0)
			{
				return;
			}
			BankBalanceManual obj = new BankBalanceManual();
			obj.BankBalanceManualList = new List<BankBalanceManual>();
			GridEXRow[] rows = grdHistory.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToDouble(item.Cells["BankBalance"].Value) > 0.0)
				{
					BankBalanceManual bal = new BankBalanceManual();
					bal.OrganizationId = UserAccount.OrganizationId;
					bal.CompanyId = UserAccount.CompanyId;
					bal.EntryUserId = UserAccount.ID;
					bal.EntryDate = DateTime.Now;
					bal.BalanceDate = txtBalanceDate.Value;
					string BalDate = Conversion.ToString((object)bal.BalanceDate.ToString("dd/MMM/yyyy"));
					bal.BalanceTime = Convert.ToDateTime(BalDate + " " + Conversion.ToDateTime(item.Cells["Time"].Value).TimeOfDay.ToString());
					bal.ChartOfAccountId = Conversion.ToInt(item.Cells["AccountId"].Value);
					bal.Balance = Conversion.ToDecimal(item.Cells["BankBalance"].Value);
					bal.BySrource = Conversion.ToInt(item.Cells["Source"].Value);
					bal.BySourceDescription = Conversion.ToString((object)item.Cells["Source"].Text);
					obj.BankBalanceManualList.Add(bal);
				}
			}
			BankBalanceManual.Save(obj);
			SummeryGrd();
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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Expected O, but got Unknown
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Expected O, but got Unknown
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtBalanceDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdSummary = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(793, 441);
		this.panel1.TabIndex = 0;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 103);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(793, 338);
		this.panel7.TabIndex = 3;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 26);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(793, 312);
		this.panel9.TabIndex = 48;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(793, 312);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 46;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.UpdatingCell += new UpdatingCellEventHandler(grdHistory_UpdatingCell);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label5);
		this.panel8.Controls.Add(this.ctrlGrdSummary);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(793, 26);
		this.panel8.TabIndex = 47;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 2);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(74, 19);
		this.label5.TabIndex = 7;
		this.label5.Text = "Summary";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add(this.txtBalanceDate);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(793, 78);
		this.panel3.TabIndex = 1;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(793, 30);
		this.panel4.TabIndex = 112;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(231, 19);
		this.label1.TabIndex = 8;
		this.label1.Text = "Bank Balance Manual Entry";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(141, 50);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 23);
		this.btnshow.TabIndex = 109;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtBalanceDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceDate.CustomFormat = "dd-MMM-yy";
		this.txtBalanceDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBalanceDate.Location = new System.Drawing.Point(12, 50);
		this.txtBalanceDate.Name = "txtBalanceDate";
		this.txtBalanceDate.Size = new System.Drawing.Size(124, 23);
		this.txtBalanceDate.TabIndex = 106;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(12, 32);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(76, 15);
		this.label27.TabIndex = 111;
		this.label27.Text = "Balance Date";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(793, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.btnSave });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(793, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.ForeColor = System.Drawing.Color.Black;
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnSave.ForeColor = System.Drawing.Color.Black;
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.ctrlGrdSummary.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdSummary.FormName = this;
		this.ctrlGrdSummary.Location = new System.Drawing.Point(751, 2);
		this.ctrlGrdSummary.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdSummary.MyGrid = this.grdHistory;
		this.ctrlGrdSummary.Name = "ctrlGrdSummary";
		this.ctrlGrdSummary.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdSummary.TabIndex = 542;
		this.ctrlGrdSummary.Load += new System.EventHandler(ctrlGrdSummary_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(793, 441);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "BankBalanceManualEntry";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(BankBalances_FormClosing);
		base.Load += new System.EventHandler(FrmBankBalancesRpt_Load);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
