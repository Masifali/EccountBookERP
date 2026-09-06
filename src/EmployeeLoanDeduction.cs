using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.PayrollManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.PayrollManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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
namespace Architecture.WinApp.HRM.PayRoll
{

public class EmployeeLoanDeduction : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private bool UpdateMode;

	private int[] AryColWidth;

	private DataTable dtPending = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	internal UltraCombo cmbMonth;

	internal UltraCombo cmbYear;

	private Label label10;

	private Label label9;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private TabPage tabPage2;

	private SqlCeDataAdapter sqlCeDataAdapter1;

	private Panel panel6;

	internal GridEX grd;

	private Button btnLoad;

	private Panel panel7;

	private Label label2;

	private Panel panel8;

	private Label label1;

	internal GridEX grdPendingForDeduction;

	private ToolStripButton btnUpdate;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label3;

	public EmployeeLoanDeduction(UserAccount pUserAccount)
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
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(980, 700);
			tabPage2.AutoScroll = true;
			tabPage2.AutoScrollMargin = new Size(20, 20);
			tabPage2.AutoScrollMinSize = new Size(980, 500);
			formright = CommonServices.SetRightsValueInRightsObject("EmployeeLoanDeduction");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtPending.Columns.Add("Id");
			dtPending.Columns.Add("EmployeeId");
			dtPending.Columns.Add("EmployeeName");
			dtPending.Columns.Add("RemaningNoOfInstallment");
			dtPending.Columns.Add("InstallmentAmount", typeof(double));
			dtPending.Columns.Add("NoOfInstallment");
			dtPending.Columns.Add("Amount", typeof(double));
			YearFill();
			MonthFill();
			btnsave.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MonthFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "January");
			dt.Rows.Add(2, "February");
			dt.Rows.Add(3, "March");
			dt.Rows.Add(4, "April");
			dt.Rows.Add(5, "May");
			dt.Rows.Add(6, "June");
			dt.Rows.Add(7, "July");
			dt.Rows.Add(8, "August");
			dt.Rows.Add(9, "September");
			dt.Rows.Add(10, "October");
			dt.Rows.Add(11, "November");
			dt.Rows.Add(12, "December");
			DDL.BindDDL(dt, cmbMonth, "Id", "Name", "Month", ZeroIndex: false);
			((UltraGridBase)cmbMonth).DisplayLayout.Bands[0].Columns[1].Width = 140;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void YearFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Year");
			for (int i = 1900; i <= DateTime.Now.Year; i++)
			{
				dt.Rows.Add(i, i);
			}
			DataTable sortedDt = dt.Select("", "Id DESC").CopyToDataTable();
			DDL.BindDDL(sortedDt, cmbYear, "Id", "Year", "Year", ZeroIndex: false);
			((UltraGridBase)cmbYear).DisplayLayout.Bands[0].Columns[1].Width = 140;
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
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && btnsave.Visible && !UpdateMode)
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
			cmbMonth.Value = string.Empty;
			((Control)(object)cmbYear).Text = string.Empty;
			cmbMonth.Focus();
			btnsave.Visible = true;
			dtPending.Rows.Clear();
			grd.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbMonth).ActiveRow == null)
			{
				MessageBox.Show("Month Required");
				cmbMonth.Focus();
				return;
			}
			if (((UltraGridBase)cmbYear).ActiveRow == null)
			{
				MessageBox.Show("Year Required");
				cmbYear.Focus();
				return;
			}
			EmployeeLoanDeduction obj = new EmployeeLoanDeduction();
			obj.LoanDeductionsList = new List<EmployeeLoanDeduction>();
			GridEXRow[] rows = grdPendingForDeduction.GetRows();
			foreach (GridEXRow r in rows)
			{
				EmployeeLoanDeduction obj2 = new EmployeeLoanDeduction();
				obj2.EmployeeLoanDeductionId = Conversion.ToInt(r.Cells["Id"].Value);
				obj2.EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
				obj2.InstallmentAmount = Conversion.ToDecimal(r.Cells["InstallmentAmount"].Value);
				obj2.NoOfInstallment = Conversion.ToInt(r.Cells["NoOfInstallment"].Value);
				obj2.TotalLoanAmount = Conversion.ToInt(r.Cells["Amount"].Value);
				obj2.PayrollMonth = Conversion.ToInt(cmbMonth.Value);
				obj2.PayrollYear = Conversion.ToInt(cmbYear.Value);
				obj2.OrganizationId = UserAccount.OrganizationId;
				obj2.CompanyId = UserAccount.CompanyId;
				obj2.CreatedById = UserAccount.ID;
				obj2.CreatedOn = DateTime.Now;
				obj2.AlteredById = UserAccount.ID;
				obj2.AlteredOn = DateTime.Now;
				obj.LoanDeductionsList.Add(obj2);
			}
			EmployeeLoanDeduction.Save(obj);
			MessageBox.Show("Saved Successfully");
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingForDeduction).Name);
			if (fs != null)
			{
				grdPendingForDeduction.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void PendingForDeductionFill()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbMonth).ActiveRow == null)
			{
				MessageBox.Show("Month Feild Required...");
				cmbMonth.Focus();
			}
			else if (((UltraGridBase)cmbYear).ActiveRow == null)
			{
				MessageBox.Show("Year Feild Required...");
				cmbYear.Focus();
			}
			else if (((UltraGridBase)cmbYear).ActiveRow != null && ((UltraGridBase)cmbMonth).ActiveRow != null)
			{
				DataTable dt = EmployeeLoanDeduction.GetLoanInstallmentsForLoanDeductions(new EmployeeLoanDeduction
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (dt.Rows.Count > 0)
				{
					dtPending.Rows.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtPending.Rows.Add(dt.Rows[i]["EmployeeLoanDeductionId"], dt.Rows[i]["EmployeeId"], dt.Rows[i]["EmployeeName"], dt.Rows[i]["RemaningNoOfInstallment"], dt.Rows[i]["InstallmentAmount"], 1, 0);
					}
					grdPendingForDeduction.DataSource = dtPending;
					grdPendingForDeduction.RetrieveStructure();
					grdPendingForDeductionSetting();
				}
				else
				{
					grdPendingForDeduction.ClearStructure();
				}
			}
			else
			{
				grdPendingForDeduction.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingForDeductionSetting()
	{
		try
		{
			grdPendingForDeduction.RootTable.Columns["Id"].EditType = (EditType)0;
			grdPendingForDeduction.RootTable.Columns["EmployeeId"].EditType = (EditType)0;
			grdPendingForDeduction.RootTable.Columns["EmployeeName"].EditType = (EditType)0;
			grdPendingForDeduction.RootTable.Columns["RemaningNoOfInstallment"].EditType = (EditType)0;
			grdPendingForDeduction.RootTable.Columns["InstallmentAmount"].EditType = (EditType)0;
			grdPendingForDeduction.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdPendingForDeduction.RootTable.Columns["Id"].Visible = false;
			grdPendingForDeduction.RootTable.Columns["EmployeeId"].Visible = false;
			grdPendingForDeduction.RootTable.Columns["EmployeeName"].Width = 250;
			grdPendingForDeduction.RootTable.Columns["RemaningNoOfInstallment"].Width = 200;
			grdPendingForDeduction.RootTable.Columns["InstallmentAmount"].Width = 150;
			grdPendingForDeduction.RootTable.Columns["NoOfInstallment"].Width = 150;
			grdPendingForDeduction.RootTable.Columns["Amount"].Width = 150;
			grdPendingForDeduction.RootTable.Columns["InstallmentAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingForDeduction.RootTable.Columns["InstallmentAmount"].TextAlignment = (TextAlignment)3;
			grdPendingForDeduction.RootTable.Columns["InstallmentAmount"].FormatString = "0,0";
			grdPendingForDeduction.RootTable.Columns["InstallmentAmount"].TotalFormatString = "#,##0.##";
			grdPendingForDeduction.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPendingForDeduction.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPendingForDeduction.RootTable.Columns["Amount"].FormatString = "0,0";
			grdPendingForDeduction.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			CalculateAmount();
			CommonServices.GridAutoAdjustment(AryColWidth, grdPendingForDeduction);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingForDeduction_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdPendingForDeduction.CurrentRow;
			if (e.Column.Key == "NoOfInstallment")
			{
				if (Conversion.ToDouble(r.Cells["NoOfInstallment"].Value) > Conversion.ToDouble(r.Cells["RemaningNoOfInstallment"].Value))
				{
					r.Cells["NoOfInstallment"].Value = 0;
					r.Cells["Amount"].Value = 0;
					throw new Exception("NoOfInstallment cannot be greater than RemaningNoOfInstallment Please Check");
				}
				double amount = Conversion.ToDouble(r.Cells["NoOfInstallment"].Value) * Conversion.ToDouble(r.Cells["InstallmentAmount"].Value);
				r.Cells["Amount"].Value = amount;
			}
			grdPendingForDeduction.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void CalculateAmount()
	{
		try
		{
			if (grdPendingForDeduction.GetRows().Count() <= 0)
			{
				return;
			}
			grdPendingForDeduction.UpdateData();
			GridEXRow[] rows = grdPendingForDeduction.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (Conversion.ToInt(r.Cells["NoOfInstallment"].Value) <= Conversion.ToInt(r.Cells["RemaningNoOfInstallment"].Value))
				{
					if (Conversion.ToInt(r.Cells["NoOfInstallment"].Value) > 0 && Conversion.ToDouble(r.Cells["InstallmentAmount"].Value) > 0.0)
					{
						double amount = Conversion.ToDouble(r.Cells["NoOfInstallment"].Value) * Conversion.ToDouble(r.Cells["InstallmentAmount"].Value);
						r.Cells["Amount"].Value = amount;
					}
				}
				else
				{
					r.Cells["Amount"].Value = 0;
				}
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoad_Click(object sender, EventArgs e)
	{
		try
		{
			PendingForDeductionFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		_ = tabControl1.SelectedIndex;
		_ = 1;
	}

	private void EmployeeLoanDeduction_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Expected O, but got Unknown
		Appearance appearance20 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PayRoll.EmployeeLoanDeduction));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdPendingForDeduction = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.btnLoad = new System.Windows.Forms.Button();
		this.cmbMonth = new UltraCombo();
		this.cmbYear = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.sqlCeDataAdapter1 = new SqlCeDataAdapter();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingForDeduction).BeginInit();
		this.panel7.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(922, 456);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(922, 456);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(914, 430);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(908, 424);
		this.panel2.TabIndex = 3;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingForDeduction);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 113);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(908, 311);
		this.panel4.TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.grdPendingForDeduction).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingForDeduction.FilterMode = (FilterMode)2;
		this.grdPendingForDeduction.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingForDeduction).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingForDeduction.GroupByBoxVisible = false;
		this.grdPendingForDeduction.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdPendingForDeduction).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdPendingForDeduction).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdPendingForDeduction).Name = "grdPendingForDeduction";
		((ScrollableBase)this.grdPendingForDeduction).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdPendingForDeduction).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdPendingForDeduction).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingForDeduction).Size = new System.Drawing.Size(908, 285);
		((System.Windows.Forms.Control)(object)this.grdPendingForDeduction).TabIndex = 3;
		this.grdPendingForDeduction.TotalRow = (InheritableBoolean)1;
		this.grdPendingForDeduction.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingForDeduction).VisualStyle = (VisualStyle)4;
		this.grdPendingForDeduction.CellUpdated += new ColumnActionEventHandler(grdPendingForDeduction_CellUpdated);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(908, 26);
		this.panel7.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(857, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 21;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 17);
		this.label2.TabIndex = 477;
		this.label2.Text = "Detail";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Controls.Add(this.btnLoad);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMonth);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbYear);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(908, 85);
		this.panel3.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(908, 26);
		this.panel8.TabIndex = 114;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(229, 19);
		this.label1.TabIndex = 477;
		this.label1.Text = "Employee Loan Deducation";
		this.btnLoad.BackColor = System.Drawing.Color.Teal;
		this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoad.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoad.ForeColor = System.Drawing.Color.White;
		this.btnLoad.Location = new System.Drawing.Point(295, 48);
		this.btnLoad.Name = "btnLoad";
		this.btnLoad.Size = new System.Drawing.Size(60, 28);
		this.btnLoad.TabIndex = 2;
		this.btnLoad.Text = "Load";
		this.btnLoad.UseVisualStyleBackColor = false;
		this.btnLoad.Click += new System.EventHandler(btnLoad_Click);
		this.cmbMonth.AllowNull = (DefaultableBoolean)1;
		this.cmbMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMonth).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMonth).Location = new System.Drawing.Point(9, 49);
		((System.Windows.Forms.Control)(object)this.cmbMonth).Name = "cmbMonth";
		((System.Windows.Forms.Control)(object)this.cmbMonth).Size = new System.Drawing.Size(139, 26);
		((System.Windows.Forms.Control)(object)this.cmbMonth).TabIndex = 0;
		this.cmbYear.AllowNull = (DefaultableBoolean)1;
		this.cmbYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbYear).Location = new System.Drawing.Point(152, 49);
		((System.Windows.Forms.Control)(object)this.cmbYear).Name = "cmbYear";
		((System.Windows.Forms.Control)(object)this.cmbYear).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.cmbYear).TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(152, 31);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(29, 15);
		this.label10.TabIndex = 113;
		this.label10.Text = "Year";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(9, 30);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(43, 15);
		this.label9.TabIndex = 111;
		this.label9.Text = "Month";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.toolStrip1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(908, 28);
		this.panel5.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnUpdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(908, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(914, 430);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 32);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(908, 395);
		this.panel9.TabIndex = 2;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(908, 395);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(908, 29);
		this.panel6.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(857, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar2.TabIndex = 479;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 19);
		this.label3.TabIndex = 478;
		this.label3.Text = "History";
		this.sqlCeDataAdapter1.DeleteCommand = null;
		this.sqlCeDataAdapter1.InsertCommand = null;
		this.sqlCeDataAdapter1.SelectCommand = null;
		this.sqlCeDataAdapter1.UpdateCommand = null;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(922, 456);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EmployeeLoanDeduction";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(EmployeeLoanDeduction_FormClosing);
		base.Load += new System.EventHandler(EmployeeFamilyInfo_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EmployeeFamilyInfo_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingForDeduction).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
