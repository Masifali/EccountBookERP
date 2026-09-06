// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
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
namespace Architecture.WinApp.Account_Reports
{

public class SubsidiaryPayableReceiveables : Form
{
	private UserAccount UserAccount = new UserAccount();

	public int ControlAccountId;

	public DateTime FromDate;

	public DateTime ToDate;

	public int PartyTypeId;

	private DataTable dthistory = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label1;

	private Panel panel4;

	internal GridEX DataGridHistory;

	private RadioButton rdSixColumns;

	private RadioButton rdFourColumns;

	private ToolStrip toolStrip1;

	private ToolStripButton btnPrint;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar1;

	public SubsidiaryPayableReceiveables(UserAccount puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puser;
	}

	private void SubsidiaryPayableReceiveables_Load(object sender, EventArgs e)
	{
		try
		{
			if (PartyTypeId == 2)
			{
				label1.Text = "Receivables Control Account Wise";
			}
			if (ControlAccountId > 0)
			{
				GetData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetData()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dthistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.UserId = UserAccount.ID;
			obj.FromDate = FromDate;
			obj.ToDate = ToDate;
			obj.AccountId = ControlAccountId;
			obj.AccountTypeId = PartyTypeId;
			dthistory = VoucherHead.GetSubsidiaryPayablesReceivables(obj);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("PartyCode");
				dt.Columns.Add("SupplierCustomer");
				dt.Columns.Add("AccountId");
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("AccountTitle");
				dt.Columns.Add("SubReferenceId");
				dt.Columns.Add("SubsidiaryTypeId");
				dt.Columns.Add("OpeningDebit", typeof(double));
				dt.Columns.Add("OpeningCredit", typeof(double));
				dt.Columns.Add("Opening", typeof(double));
				dt.Columns.Add("CurrentDebit", typeof(double));
				dt.Columns.Add("CurrentCredit", typeof(double));
				dt.Columns.Add("ClosingDebit", typeof(double));
				dt.Columns.Add("ClosingCredit", typeof(double));
				dt.Columns.Add("Closing", typeof(double));
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["PartyCode"], dthistory.Rows[i]["SupplierCustomer"], dthistory.Rows[i]["AccountId"], dthistory.Rows[i]["AccountCode"], dthistory.Rows[i]["AccountTitle"], dthistory.Rows[i]["SubReferenceId"], dthistory.Rows[i]["SubsidiaryTypeId"], dthistory.Rows[i]["ODebit"], dthistory.Rows[i]["OCredit"], dthistory.Rows[i]["Opening"], dthistory.Rows[i]["CurrDebit"], dthistory.Rows[i]["CurCredit"], dthistory.Rows[i]["CDebit"], dthistory.Rows[i]["CCredit"], dthistory.Rows[i]["Closing"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GrdSetting();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["AccountId"].Visible = false;
			DataGridHistory.RootTable.Columns["SubReferenceId"].Visible = false;
			DataGridHistory.RootTable.Columns["SubsidiaryTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["CurrentDebit"].Caption = "Debit";
			DataGridHistory.RootTable.Columns["CurrentCredit"].Caption = "Credit";
			if (rdSixColumns.Checked)
			{
				DataGridHistory.RootTable.Columns["OpeningDebit"].Visible = true;
				DataGridHistory.RootTable.Columns["OpeningCredit"].Visible = true;
				DataGridHistory.RootTable.Columns["CurrentDebit"].Visible = true;
				DataGridHistory.RootTable.Columns["CurrentCredit"].Visible = true;
				DataGridHistory.RootTable.Columns["ClosingDebit"].Visible = true;
				DataGridHistory.RootTable.Columns["ClosingCredit"].Visible = true;
				DataGridHistory.RootTable.Columns["Opening"].Visible = false;
				DataGridHistory.RootTable.Columns["Closing"].Visible = false;
			}
			else if (rdFourColumns.Checked)
			{
				DataGridHistory.RootTable.Columns["OpeningDebit"].Visible = false;
				DataGridHistory.RootTable.Columns["OpeningCredit"].Visible = false;
				DataGridHistory.RootTable.Columns["ClosingDebit"].Visible = false;
				DataGridHistory.RootTable.Columns["ClosingCredit"].Visible = false;
				DataGridHistory.RootTable.Columns["Opening"].Visible = true;
				DataGridHistory.RootTable.Columns["CurrentDebit"].Visible = true;
				DataGridHistory.RootTable.Columns["CurrentCredit"].Visible = true;
				DataGridHistory.RootTable.Columns["Closing"].Visible = true;
			}
			DataGridHistory.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["OpeningDebit"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OpeningDebit"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OpeningDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["OpeningDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["OpeningCredit"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OpeningCredit"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OpeningCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["OpeningCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["CurrentDebit"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CurrentDebit"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CurrentDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["CurrentDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["CurrentCredit"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CurrentCredit"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CurrentCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["CurrentCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["ClosingDebit"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ClosingDebit"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ClosingDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["ClosingDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["ClosingCredit"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ClosingCredit"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ClosingCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["ClosingCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
			DataGridHistory.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void rdSixColumns_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			GrdSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "AccountCode")
			{
				GeneralLedger generalLedger = new GeneralLedger(UserAccount);
				generalLedger.cmbAccountTitle.Value = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["AccountId"].Value);
				generalLedger.CmbSubsidiaryAccount.Value = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["SubReferenceId"].Value);
				generalLedger.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dthistory != null && dthistory.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
				val.RptPerameter("@CompanyName", UserAccount.CompName);
				val.ShowReportWithDataTable(dthistory, "375-GetSubsidiaryAccountFromVouchers.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SubsidiaryPayableReceiveables_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.rdFourColumns = new System.Windows.Forms.RadioButton();
		this.rdSixColumns = new System.Windows.Forms.RadioButton();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(785, 561);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 111);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(785, 450);
		this.panel4.TabIndex = 1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(785, 450);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.rdFourColumns);
		this.panel2.Controls.Add(this.rdSixColumns);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(785, 111);
		this.panel2.TabIndex = 0;
		this.rdFourColumns.AutoSize = true;
		this.rdFourColumns.Checked = true;
		this.rdFourColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdFourColumns.Location = new System.Drawing.Point(9, 86);
		this.rdFourColumns.Name = "rdFourColumns";
		this.rdFourColumns.Size = new System.Drawing.Size(99, 19);
		this.rdFourColumns.TabIndex = 2;
		this.rdFourColumns.TabStop = true;
		this.rdFourColumns.Text = "Four Columns";
		this.rdFourColumns.UseVisualStyleBackColor = true;
		this.rdSixColumns.AutoSize = true;
		this.rdSixColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSixColumns.Location = new System.Drawing.Point(9, 63);
		this.rdSixColumns.Name = "rdSixColumns";
		this.rdSixColumns.Size = new System.Drawing.Size(91, 19);
		this.rdSixColumns.TabIndex = 1;
		this.rdSixColumns.Text = "Six Columns";
		this.rdSixColumns.UseVisualStyleBackColor = true;
		this.rdSixColumns.CheckedChanged += new System.EventHandler(rdSixColumns_CheckedChanged);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(253, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Payable Control Account Wise";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip1.Size = new System.Drawing.Size(785, 27);
		this.toolStrip1.TabIndex = 8;
		this.toolStrip1.Text = "toolStrip1";
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(79, 24);
		this.btnPrint.Text = "375-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(785, 32);
		this.panel3.TabIndex = 9;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(743, 5);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(785, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "SubsidiaryPayableReceiveables";
		this.Text = "Subsidiary Payable Receiveables";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(SubsidiaryPayableReceiveables_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SubsidiaryPayableReceiveables_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
