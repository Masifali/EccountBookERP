using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.LoanManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.ApprovalManagement;
using Architecture.Model.HRM.LoanManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.ApprovalManagement;

public class LoanApproval : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private int[] AryColWidth;

	private DataTable dttable = new DataTable();

	private IContainer components;

	private Panel panel1;

	internal GridEX grd;

	private Panel panel3;

	internal UltraCombo cmbEmployeeName;

	private Label label1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private DateTimePicker datAppliedOn;

	private TextBox txtApprovedAmount;

	private Label label2;

	private Label label9;

	private TextBox txtNoofInstallments;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel5;

	private Panel panel2;

	private Panel panel4;

	internal GridEX grdPendingForLoan;

	private Button btnGenerate;

	private Label label3;

	private TextBox txtLoanAmount;

	private Label label4;

	public LoanApproval(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void EmployeeFamilyInfo_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			EmployeeFill();
			PendingLoanForApproval();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			datAppliedOn.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeFamilyInfo_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && !UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool Validation()
	{
		if (((UltraGridBase)cmbEmployeeName).ActiveRow == null)
		{
			MessageBox.Show("Employee Name Required!");
			cmbEmployeeName.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtApprovedAmount.Text) == 0.0)
		{
			MessageBox.Show("LoanAmount Field Required!");
			txtApprovedAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNoofInstallments.Text) == 0.0)
		{
			MessageBox.Show("No Of Installments Field Required!");
			txtNoofInstallments.Focus();
			return false;
		}
		return true;
	}

	private void EmployeeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = genEmployeeHistory.GetAllEmployeesActive(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbEmployeeName, "EmployeeId", "EmployeeName", "Employee", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			RecId = 0;
			((Control)(object)cmbEmployeeName).Text = string.Empty;
			((UltraGridBase)cmbEmployeeName).DataSource = null;
			txtApprovedAmount.Text = string.Empty;
			txtLoanAmount.Text = string.Empty;
			txtNoofInstallments.Text = string.Empty;
			txtApprovedAmount.Focus();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			grd.ClearStructure();
			PendingLoanForApproval();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void Insert()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		try
		{
			if (Validation() && MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				EmployeeLoan obj = new EmployeeLoan();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.DocumentTypeId = 1001;
				obj.IsApproved = true;
				obj.EmployeeId = Conversion.ToInt(cmbEmployeeName.Value);
				obj.EmployeeLoanId = Conversion.ToInt(((UltraDropDownBase)cmbEmployeeName).SelectedRow.Cells[2].Value);
				obj.LoanAmount = Conversion.ToDecimal((object)txtLoanAmount.Text.Trim());
				obj.ApprovedAmount = Conversion.ToDecimal((object)txtApprovedAmount.Text.Trim());
				obj.AppliedOn = Conversion.ToDateTime((object)datAppliedOn.Value);
				obj.AlteredById = Conversion.ToInt((object)UserAccount.ID);
				obj.ApprovedById = Conversion.ToInt((object)UserAccount.ID);
				obj.AlteredOn = DateTime.Now;
				obj.ApprovedOn = DateTime.Now;
				obj.CreatedOn = DateTime.Now;
				obj.NoOfInstallment = Conversion.ToInt((object)grd.GetRows().Count());
				obj.EmployeeLoanInstallmentslist = new List<EmployeeLoanInstallment>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					EmployeeLoanInstallment objdetail = new EmployeeLoanInstallment();
					objdetail.EmployeeLoanInstallmentId = Conversion.ToInt(r.Cells["EmployeeLoanInstallmentId"].Value);
					objdetail.EmployeeLoanId = Conversion.ToInt(cmbEmployeeName.Value);
					objdetail.InstallmentNo = Conversion.ToInt(r.Cells["Installments"].Value);
					objdetail.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					objdetail.Month = Conversion.ToString((object)r.Cells["Month"].Text);
					objdetail.Year = Conversion.ToInt(r.Cells["Year"].Value);
					objdetail.OrganizationId = UserAccount.OrganizationId;
					objdetail.CompanyId = UserAccount.CompanyId;
					objdetail.CreatedById = UserAccount.ID;
					objdetail.CreatedOn = DateTime.Now;
					objdetail.AlteredById = UserAccount.ID;
					objdetail.AlteredOn = DateTime.Now;
					objdetail.IsPaid = false;
					objdetail.PayrollId = 0L;
					objdetail.UserLogId = UserAccount.ID;
					objdetail.ActionTypeId = 1;
					obj.EmployeeLoanInstallmentslist.Add(objdetail);
				}
				EmployeeLoan.Save(obj);
				if (RecId > 0)
				{
					MessageBox.Show("Update Successfully");
				}
				else
				{
					MessageBox.Show("Saved Successfully");
				}
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void PendingLoanForApproval()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		try
		{
			DataTable dt = EmployeeLoan.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsApproved = false,
				CanViewAllRecord = true
			});
			if (dt.Rows.Count > 0)
			{
				grdPendingForLoan.DataSource = dt;
				grdPendingForLoan.RetrieveStructure();
				grdPendingForLoanSetting();
			}
			else
			{
				grdPendingForLoan.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingForLoanSetting()
	{
		try
		{
			grdPendingForLoan.RootTable.Columns["EmployeeId"].Visible = false;
			grdPendingForLoan.RootTable.Columns["EmployeeLoanId"].Visible = false;
			grdPendingForLoan.RootTable.Columns["RecordNo"].Visible = false;
			grdPendingForLoan.RootTable.Columns.Add("Load");
			grdPendingForLoan.RootTable.Columns["Load"].Key = "Load";
			grdPendingForLoan.RootTable.Columns["Load"].Caption = "Load";
			grdPendingForLoan.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingForLoan.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdPendingForLoan.RootTable.Columns["Load"].ButtonText = "Load";
			grdPendingForLoan.RootTable.Columns["Load"].Width = 80;
			grdPendingForLoan.RootTable.Columns["LoanAmount"].FormatString = "#,##0.00";
			grdPendingForLoan.RootTable.Columns["ApprovedAmount"].FormatString = "#,##0.00";
			grdPendingForLoan.RootTable.Columns["LoanAmount"].TotalFormatString = "#,##0.00";
			grdPendingForLoan.RootTable.Columns["ApprovedAmount"].TotalFormatString = "#,##0.00";
			grdPendingForLoan.RootTable.Columns["LoanAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingForLoan.RootTable.Columns["ApprovedAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingForLoan.RootTable.Columns["LoanAmount"].TextAlignment = (TextAlignment)3;
			grdPendingForLoan.RootTable.Columns["ApprovedAmount"].TextAlignment = (TextAlignment)3;
			CommonServices.GridAutoAdjustment(AryColWidth, grdPendingForLoan);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingForLoan_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			DataTable dtEmployee = new DataTable();
			dtEmployee.Columns.Add("Id");
			dtEmployee.Columns.Add("Name");
			dtEmployee.Columns.Add("LoanId");
			GridEXRow item = grdPendingForLoan.CurrentRow;
			if (item != null)
			{
				dtEmployee.Rows.Add(item.Cells["EmployeeId"].Value, item.Cells["EmployeeName"].Text, item.Cells["EmployeeLoanId"].Value);
				if (dtEmployee.Rows.Count > 0)
				{
					DDL.BindDDL(dtEmployee, cmbEmployeeName, "Id", "Name", "Employee", ZeroIndex: false);
					((UltraGridBase)cmbEmployeeName).Rows[0].Activate();
					((UltraGridBase)cmbEmployeeName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				datAppliedOn.Value = Conversion.ToDateTime(item.Cells["AppliedOn"].Value);
				txtNoofInstallments.Text = Conversion.ToInt(item.Cells["NoOfInstallment"].Value).ToString();
				txtLoanAmount.Text = Conversion.ToInt(item.Cells["LoanAmount"].Value).ToString();
				txtApprovedAmount.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSetting()
	{
		try
		{
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = "#,#.###";
			grd.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EmployeeLoanInstallmentId"].Visible = false;
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
			GenerateApprovedLoan();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateApprovedLoan()
	{
		try
		{
			if (((UltraGridBase)cmbEmployeeName).ActiveRow == null || Conversion.ToInt(cmbEmployeeName.Value) == 0)
			{
				grd.ClearStructure();
				dttable.Clear();
				MessageBox.Show("EmployeeName Field Required....");
				cmbEmployeeName.Focus();
				return;
			}
			if (Conversion.ToDouble((object)txtApprovedAmount.Text) == 0.0)
			{
				grd.ClearStructure();
				dttable.Clear();
				MessageBox.Show("ApprovedAmount Field Required....");
				txtApprovedAmount.Focus();
				return;
			}
			if (Conversion.ToDouble((object)txtApprovedAmount.Text) > Conversion.ToDouble((object)txtLoanAmount.Text))
			{
				grd.ClearStructure();
				dttable.Clear();
				MessageBox.Show("ApprovedAmount Can not greater than LoanAmount....");
				return;
			}
			if (Conversion.ToInt((object)txtNoofInstallments.Text) > 60)
			{
				grd.ClearStructure();
				dttable.Clear();
				MessageBox.Show("No Of Installments Can not greater than 60....");
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("EmployeeLoanInstallmentId");
			dt.Columns.Add("Installments");
			dt.Columns.Add("Month");
			dt.Columns.Add("Year");
			dt.Columns.Add("Amount", typeof(double));
			dt.Rows.Clear();
			if (Conversion.ToDouble((object)txtApprovedAmount.Text) > 0.0 && Conversion.ToInt((object)txtNoofInstallments.Text) > 0)
			{
				int NoOfInstallments = Conversion.ToInt((object)txtNoofInstallments.Text);
				double Amount = Conversion.ToDouble((object)txtApprovedAmount.Text) / (double)NoOfInstallments;
				dt.Rows.Clear();
				int month = 0;
				for (int i = 1; i <= NoOfInstallments; i++)
				{
					DateTime Months = datAppliedOn.Value.AddMonths(month);
					dt.Rows.Add(0, i, Months.ToString("MMM"), Months.Year, Amount);
					month++;
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoofInstallments_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtLoanAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtApprovedAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void LoanApproval_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Expected O, but got Unknown
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.ApprovalManagement.LoanApproval));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdPendingForLoan = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.txtLoanAmount = new System.Windows.Forms.TextBox();
		this.btnGenerate = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbEmployeeName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtNoofInstallments = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtApprovedAmount = new System.Windows.Forms.TextBox();
		this.datAppliedOn = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingForLoan).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
		this.panel1.TabIndex = 2;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 661);
		this.tabControl1.TabIndex = 2;
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingForLoan);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 420);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 212);
		this.panel4.TabIndex = 2;
		this.grdPendingForLoan.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdPendingForLoan).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingForLoan.FilterMode = (FilterMode)2;
		this.grdPendingForLoan.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingForLoan).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingForLoan.GroupByBoxVisible = false;
		this.grdPendingForLoan.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdPendingForLoan).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPendingForLoan).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdPendingForLoan).Name = "grdPendingForLoan";
		((ScrollableBase)this.grdPendingForLoan).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdPendingForLoan).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdPendingForLoan).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingForLoan).Size = new System.Drawing.Size(950, 212);
		((System.Windows.Forms.Control)(object)this.grdPendingForLoan).TabIndex = 0;
		this.grdPendingForLoan.TotalRow = (InheritableBoolean)1;
		this.grdPendingForLoan.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingForLoan).VisualStyle = (VisualStyle)4;
		this.grdPendingForLoan.ColumnButtonClick += new ColumnActionEventHandler(grdPendingForLoan_ColumnButtonClick);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 417);
		this.panel3.TabIndex = 1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 126);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(950, 291);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtLoanAmount);
		this.panel5.Controls.Add(this.btnGenerate);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeName);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtNoofInstallments);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label15);
		this.panel5.Controls.Add(this.txtApprovedAmount);
		this.panel5.Controls.Add(this.datAppliedOn);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 58);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 68);
		this.panel5.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(312, 15);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(80, 15);
		this.label3.TabIndex = 106;
		this.label3.Text = "Loan Amount";
		this.txtLoanAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLoanAmount.Location = new System.Drawing.Point(419, 11);
		this.txtLoanAmount.Name = "txtLoanAmount";
		this.txtLoanAmount.ReadOnly = true;
		this.txtLoanAmount.Size = new System.Drawing.Size(102, 23);
		this.txtLoanAmount.TabIndex = 2;
		this.txtLoanAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLoanAmount_KeyPress);
		this.btnGenerate.BackColor = System.Drawing.Color.Teal;
		this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGenerate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerate.ForeColor = System.Drawing.Color.White;
		this.btnGenerate.Location = new System.Drawing.Point(660, 37);
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.Size = new System.Drawing.Size(87, 26);
		this.btnGenerate.TabIndex = 5;
		this.btnGenerate.Text = "Generate";
		this.btnGenerate.UseVisualStyleBackColor = false;
		this.btnGenerate.Click += new System.EventHandler(btnGenerate_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(94, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Employee Name";
		this.cmbEmployeeName.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Location = new System.Drawing.Point(104, 9);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Name = "cmbEmployeeName";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(312, 43);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(106, 15);
		this.label2.TabIndex = 29;
		this.label2.Text = "Approved Amount";
		this.txtNoofInstallments.BackColor = System.Drawing.Color.White;
		this.txtNoofInstallments.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoofInstallments.Location = new System.Drawing.Point(631, 11);
		this.txtNoofInstallments.Name = "txtNoofInstallments";
		this.txtNoofInstallments.Size = new System.Drawing.Size(116, 23);
		this.txtNoofInstallments.TabIndex = 4;
		this.txtNoofInstallments.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoofInstallments_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(7, 43);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 29;
		this.label9.Text = "Applied On";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(526, 15);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(107, 15);
		this.label15.TabIndex = 29;
		this.label15.Text = "No Of Installments";
		this.txtApprovedAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovedAmount.Location = new System.Drawing.Point(419, 39);
		this.txtApprovedAmount.Name = "txtApprovedAmount";
		this.txtApprovedAmount.Size = new System.Drawing.Size(102, 23);
		this.txtApprovedAmount.TabIndex = 3;
		this.txtApprovedAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtApprovedAmount_KeyPress);
		this.datAppliedOn.CustomFormat = "dd-MMM-yyyy";
		this.datAppliedOn.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.datAppliedOn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datAppliedOn.Location = new System.Drawing.Point(104, 39);
		this.datAppliedOn.Name = "datAppliedOn";
		this.datAppliedOn.Size = new System.Drawing.Size(200, 23);
		this.datAppliedOn.TabIndex = 1;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 58);
		this.panel2.TabIndex = 105;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(6, 32);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(210, 19);
		this.label4.TabIndex = 107;
		this.label4.Text = "Employee Loan Approval";
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnupdate, this.btnsave, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(76, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(63, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(57, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1205, 5);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LoanApproval";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(LoanApproval_FormClosing);
		base.Load += new System.EventHandler(EmployeeFamilyInfo_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EmployeeFamilyInfo_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingForLoan).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
