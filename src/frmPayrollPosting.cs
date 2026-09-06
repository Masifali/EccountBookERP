// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PayrollManagement;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.PayrollManagement;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.ProfileManagement;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
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

public class frmPayrollPosting : Form
{
	private List<PayrollDetail> lstDelete = new List<PayrollDetail>();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public Rightsobjects formright = new Rightsobjects();

	private DataTable dtdetail = new DataTable();

	private DataTable dtGrid = new DataTable();

	private int RecId;

	public bool ApprovedStatus;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private Panel panel13;

	private Panel panel17;

	private ToolStrip toolStrip3;

	private ToolStripButton btnUpdate;

	private Label label3;

	private DateTimePicker txtDocDate;

	private ToolStripButton toolStripButton1;

	private Panel panel6;

	private Label label18;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX DataGridHistory;

	private Panel panel7;

	internal GridEX grdHistoryDetail;

	private Panel panel18;

	private Panel panel5;

	private Label label51;

	private Label label4;

	internal UltraCombo cmbEmployeeCategory;

	private Button btnShowDetail;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo cmbEmployeeDepartment;

	private Label label1;

	internal UltraCombo cmbMonth;

	internal UltraCombo cmbYear;

	private Label label10;

	private Label label9;

	private Button btnEmployeeCategory;

	private Button btnEmployee;

	internal GridEX grdDetails;

	private Panel panel8;

	private Label label2;

	private Label label5;

	private TextBox txtDocNo;

	private ToolStripDropDownButton tsDropDownPrint;

	internal UltraCombo CmbDebitAccount;

	private Label label6;

	internal UltraCombo cmbEmployee;

	private Label label7;

	public frmPayrollPosting(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FrmExportSalesContract_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			formright = CommonServices.SetRightsValueInRightsObject("PayrollPosting");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			EmployeeDepartmentFill();
			EmployeeCategoryFill();
			YearFill();
			MonthFill();
			GenerateCode();
			GetAccountsFromEmployee();
			EmployeeFill();
			ReportsLoad();
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("AccountId", typeof(int));
			dtdetail.Columns.Add("AccountCode", typeof(string));
			dtdetail.Columns.Add("EmployeeNo", typeof(int));
			dtdetail.Columns.Add("EmployeeId", typeof(int));
			dtdetail.Columns.Add("EmployeeName");
			dtdetail.Columns.Add("DesignationName");
			dtdetail.Columns.Add("DepartmentName");
			dtdetail.Columns.Add("SectionName");
			dtdetail.Columns.Add("EmployeeHistoryId", typeof(int));
			dtdetail.Columns.Add("GrossSalary", typeof(double));
			dtdetail.Columns.Add("EmployeeDays");
			dtdetail.Columns.Add("EmployeePerDaySalary", typeof(double));
			dtdetail.Columns.Add("EmployeePayrollSalary", typeof(double));
			dtdetail.Columns.Add("LateDeductionAmount", typeof(double));
			dtdetail.Columns.Add("IncomeTaxAmount", typeof(double));
			dtdetail.Columns.Add("LeaveAmount", typeof(double));
			dtdetail.Columns.Add("AdvanceAmount", typeof(double));
			dtdetail.Columns.Add("LoanAmount", typeof(double));
			dtdetail.Columns.Add("LoanNOS", typeof(int));
			dtdetail.Columns.Add("PFAmount", typeof(double));
			dtdetail.Columns.Add("EOBIAmount", typeof(double));
			dtdetail.Columns.Add("MiscLessAmount", typeof(double));
			dtdetail.Columns.Add("TotalLessAmount", typeof(double));
			dtdetail.Columns.Add("OTAmount", typeof(double));
			dtdetail.Columns.Add("TravellingAmount", typeof(double));
			dtdetail.Columns.Add("MedicalAmount", typeof(double));
			dtdetail.Columns.Add("MobileAmount", typeof(double));
			dtdetail.Columns.Add("FuelAmount", typeof(double));
			dtdetail.Columns.Add("FoodAmount", typeof(double));
			dtdetail.Columns.Add("MiscAddAmount", typeof(double));
			dtdetail.Columns.Add("ArearAmount", typeof(double));
			dtdetail.Columns.Add("Salary", typeof(double));
			dtdetail.Columns.Add("AddLessAmount", typeof(double));
			dtdetail.Columns.Add("NetSalary", typeof(double));
			grdDetails.DataSource = dtdetail;
			grdDetails.RetrieveStructure();
			grdSettings();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtDocDate.Focus();
			if (Conversion.ToString(base.Tag) == "1000" || ApprovedStatus)
			{
				ApprovedStatus = true;
				btnShowDetail.Visible = false;
				tabControl1.SelectedIndex = 1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportsLoad()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("SalarySheet");
			if (files != null && files.Length != 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					tsDropDownPrint.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[i]));
				}
			}
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
			DataTable dty = CommonServices.GetMonths();
			DDL.BindDDL(dty, cmbMonth, "Id", "Name", "Month", ZeroIndex: false);
			((UltraGridBase)cmbMonth).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			DataTable dty = CommonServices.GetYears();
			DDL.BindDDL(dty, cmbYear, "Id", "Year", "Year", ZeroIndex: false);
			((UltraGridBase)cmbYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeDepartmentFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			DataTable dtDepartment = genDepartment.Getall(new genDepartment
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDepartment.Rows.Count > 0)
			{
				for (int i = 0; i < dtDepartment.Rows.Count; i++)
				{
					dt.Rows.Add(dtDepartment.Rows[i]["DepartmentId"], dtDepartment.Rows[i]["DepartmentName"]);
				}
				DDL.BindDDL(dt, cmbEmployeeDepartment, "Id", "Name", "Department Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeCategoryFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			DataTable dtCategory = genEmployeeCategory.Getall(new genEmployeeCategory
			{
				OrginizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCategory.Rows.Count > 0)
			{
				for (int i = 0; i < dtCategory.Rows.Count; i++)
				{
					dt.Rows.Add(dtCategory.Rows[i]["EmployeeCategoryId"], dtCategory.Rows[i]["EmployeeCategoryName"]);
				}
				DDL.BindDDL(dt, cmbEmployeeCategory, "Id", "Name", "CategoryName Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			int DocCode = Payroll.GenerateCode(new Payroll
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId,
				DocMovementId = 1000
			});
			txtDocNo.Text = DocCode.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAccountsFromEmployee()
	{
		try
		{
			DataTable dt = genEmployeeHistory.GetAccountsFromEmployee(UserAccount.OrganizationId, UserAccount.CompanyId, "Expense");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDebitAccount, "Id", "AccountTitle", "Debit Account", ZeroIndex: false);
				((UltraGridBase)CmbDebitAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			DataTable dtEmployeeName = genEmployeeHistory.GetAllEmployeesActive(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DepartmentId = Conversion.ToInt(cmbEmployeeDepartment.Value)
			});
			if (dtEmployeeName.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtEmployeeName, cmbEmployee, "EmployeeId", "EmployeeName", "EmployeeName", false);
				((UltraGridBase)cmbEmployee).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdDetails.RootTable.Columns["GrossSalary"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["GrossSalary"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["GrossSalary"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["GrossSalary"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["EmployeePerDaySalary"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["EmployeePerDaySalary"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["EmployeePerDaySalary"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["EmployeePerDaySalary"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["EmployeePayrollSalary"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["EmployeePayrollSalary"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["EmployeePayrollSalary"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["EmployeePayrollSalary"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["LateDeductionAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["LateDeductionAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["LateDeductionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["LateDeductionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["IncomeTaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["IncomeTaxAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["IncomeTaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["IncomeTaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["LeaveAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["LeaveAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["LeaveAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["LeaveAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["AdvanceAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["AdvanceAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["AdvanceAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["AdvanceAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["LoanAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["LoanAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["LoanAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["LoanAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["PFAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["PFAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["PFAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["PFAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["EOBIAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["EOBIAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["EOBIAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["EOBIAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["FoodAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["FoodAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["FoodAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["FoodAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["MiscLessAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["MiscLessAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["MiscLessAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["MiscLessAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["TotalLessAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["TotalLessAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["TotalLessAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["TotalLessAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["OTAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["OTAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["OTAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["OTAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["TravellingAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["TravellingAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["TravellingAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["TravellingAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["MedicalAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["MedicalAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["MedicalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["MedicalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["MobileAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["MobileAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["MobileAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["MobileAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["FuelAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["FuelAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["FuelAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["FuelAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["MiscAddAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["MiscAddAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["MiscAddAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["MiscAddAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["ArearAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["ArearAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["ArearAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["ArearAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["NetSalary"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["NetSalary"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["NetSalary"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["NetSalary"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["AddLessAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["AddLessAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["AddLessAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["AddLessAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["Salary"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["Salary"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Salary"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["Salary"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetails.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["EmployeeHistoryId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["EmployeeNo"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["EmployeeId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["EmployeeName"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["DesignationName"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["DepartmentName"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["SectionName"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["EmployeeHistoryId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["GrossSalary"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["EmployeeDays"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["EmployeePerDaySalary"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["EmployeePayrollSalary"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["LateDeductionAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["IncomeTaxAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["IncomeTaxAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["LeaveAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["AdvanceAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["LoanAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["LoanNOS"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["PFAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["EOBIAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["FoodAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["MiscLessAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TotalLessAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["OTAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TravellingAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["MedicalAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["MobileAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["FuelAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["MiscAddAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["ArearAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["NetSalary"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["Salary"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["EmployeeNo"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["EmployeeName"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["DesignationName"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["DepartmentName"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["SectionName"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["GrossSalary"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["EmployeeDays"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["EmployeePerDaySalary"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["EmployeePayrollSalary"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["LateDeductionAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["IncomeTaxAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["IncomeTaxAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["LeaveAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["AdvanceAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["LoanAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["LoanNOS"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["PFAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["EOBIAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["FoodAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["MiscLessAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["TotalLessAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["OTAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["TravellingAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["MedicalAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["MobileAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["FuelAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["MiscAddAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["ArearAmount"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["NetSalary"].FilterEditType = (FilterEditType)1;
			grdDetails.RootTable.Columns["Salary"].FilterEditType = (FilterEditType)1;
			if (RecId > 0)
			{
				grdDetails.RootTable.Columns["DesignationName"].Visible = false;
				grdDetails.RootTable.Columns["DepartmentName"].Visible = false;
				grdDetails.RootTable.Columns["SectionName"].Visible = false;
			}
			grdDetails.RootTable.Columns["Id"].Visible = false;
			grdDetails.RootTable.Columns["AccountId"].Visible = false;
			grdDetails.RootTable.Columns["EmployeeHistoryId"].Visible = false;
			grdDetails.RootTable.Columns["EmployeeId"].Visible = false;
			grdDetails.RootTable.Columns["ArearAmount"].Visible = false;
			grdDetails.RootTable.Columns.Add("Delete");
			grdDetails.RootTable.Columns["Delete"].Key = "Delete";
			grdDetails.RootTable.Columns["Delete"].Caption = "X";
			grdDetails.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetails.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetails.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetails.RootTable.Columns["Delete"].Width = 20;
			CommonServices.GridAutoAdjustment(AryColWidth, grdDetails);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				GridEXRow r = grdDetails.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					PayrollDetail vd = new PayrollDetail();
					vd.ActionTypeId = 3;
					vd.PayrollDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					vd.EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
					vd.EmployeeHistoryId = Conversion.ToInt(r.Cells["EmployeeHistoryId"].Value);
					vd.GrossSalary = Conversion.ToDecimal(r.Cells["GrossSalary"].Value);
					vd.EmployeeDays = Conversion.ToInt(r.Cells["EmployeeDays"].Value);
					vd.EmployeePerDaySalary = Conversion.ToDecimal(r.Cells["EmployeePerDaySalary"].Value);
					vd.EmployeePayrollSalary = Conversion.ToDecimal(r.Cells["EmployeePayrollSalary"].Value);
					vd.LateDeductionAmount = Conversion.ToDecimal(r.Cells["LateDeductionAmount"].Value);
					vd.IncomeTaxAmount = Conversion.ToDecimal(r.Cells["IncomeTaxAmount"].Value);
					vd.LeaveAmount = Conversion.ToDecimal(r.Cells["LeaveAmount"].Value);
					vd.AdvanceAmount = Conversion.ToDecimal(r.Cells["AdvanceAmount"].Value);
					vd.LoanAmount = Conversion.ToDecimal(r.Cells["LoanAmount"].Value);
					vd.LoanNOS = Conversion.ToInt(r.Cells["LoanNOS"].Value);
					vd.PFAmount = Conversion.ToDecimal(r.Cells["PFAmount"].Value);
					vd.EOBIAmount = Conversion.ToDecimal(r.Cells["EOBIAmount"].Value);
					vd.FoodAmount = Conversion.ToDecimal(r.Cells["FoodAmount"].Value);
					vd.MiscLessAmount = Conversion.ToDecimal(r.Cells["MiscLessAmount"].Value);
					vd.TotalAmount = Conversion.ToDecimal(r.Cells["TotalLessAmount"].Value);
					vd.OTAmount = Conversion.ToDecimal(r.Cells["OTAmount"].Value);
					vd.TravellingAmount = Conversion.ToDecimal(r.Cells["TravellingAmount"].Value);
					vd.MedicalAmount = Conversion.ToDecimal(r.Cells["MedicalAmount"].Value);
					vd.MobileAmount = Conversion.ToDecimal(r.Cells["MobileAmount"].Value);
					vd.FuelAmount = Conversion.ToDecimal(r.Cells["FuelAmount"].Value);
					vd.MiscAddAmount = Conversion.ToDecimal(r.Cells["MiscAddAmount"].Value);
					vd.ArrearAmount = Conversion.ToDecimal(r.Cells["ArearAmount"].Value);
					vd.NetSalary = Conversion.ToDecimal(r.Cells["NetSalary"].Value);
					vd.InwardId = 0L;
					vd.CreatedById = UserAccount.ID;
					vd.CreatedOn = DateTime.Now;
					vd.AlteredById = UserAccount.ID;
					vd.AlteredOn = DateTime.Now;
					vd.UserLogId = UserAccount.ID;
					lstDelete.Add(vd);
				}
				dtdetail.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			grdDetails.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetails.UpdateData();
			GridEXRow item = grdDetails.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				double AddLessAmount = Conversion.ToDouble(item.Cells["AddLessAmount"].Value);
				double Salary = Conversion.ToDouble(item.Cells["Salary"].Value);
				item.Cells["NetSalary"].Value = (Salary + AddLessAmount).ToString();
			}
			grdDetails.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			Payroll obj = new Payroll();
			if (RecId > 0)
			{
				obj.PayrollId = RecId;
			}
			obj.LocationId = 0L;
			obj.DocMovementId = 1000;
			obj.PayrollDate = Conversion.ToDateTime((object)txtDocDate.Value);
			obj.PayrollMonth = Conversion.ToInt(cmbMonth.Value);
			obj.PayrollYear = Conversion.ToInt(cmbYear.Value);
			obj.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			obj.PostedById = UserAccount.ID;
			obj.CreatedById = UserAccount.ID;
			obj.CreatedOn = DateTime.Now;
			obj.AlteredById = UserAccount.ID;
			obj.AlteredOn = DateTime.Now;
			if (RecId > 0)
			{
				obj.ActionTypeId = 2;
			}
			else
			{
				obj.ActionTypeId = 1;
			}
			obj.UserLogId = UserAccount.ID;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.BranchesId = UserAccount.BranchesId;
			obj.IsApproved = ApprovedStatus;
			obj.PayrollDetailList = new List<PayrollDetail>();
			if (grdDetails.RowCount > 0)
			{
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow r in rows)
				{
					PayrollDetail vd = new PayrollDetail();
					vd.PayrollDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					if (Conversion.ToInt(r.Cells["EmployeeId"].Value) == 0)
					{
						MessageBox.Show("Employee Name Required");
						return;
					}
					vd.EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
					if (Conversion.ToInt(r.Cells["EmployeeHistoryId"].Value) == 0)
					{
						MessageBox.Show("Employee History Required");
						return;
					}
					vd.EmployeeHistoryId = Conversion.ToInt(r.Cells["EmployeeHistoryId"].Value);
					if (Conversion.ToDecimal(r.Cells["GrossSalary"].Value) == 0m)
					{
						MessageBox.Show("Gross Salary Required");
						return;
					}
					vd.GrossSalary = Conversion.ToDecimal(r.Cells["GrossSalary"].Value);
					if (Conversion.ToInt(r.Cells["EmployeeDays"].Value) == 0)
					{
						MessageBox.Show("Employee Days Required");
						return;
					}
					vd.EmployeeDays = Conversion.ToInt(r.Cells["EmployeeDays"].Value);
					vd.EmployeePerDaySalary = Conversion.ToDecimal(r.Cells["EmployeePerDaySalary"].Value);
					vd.EmployeePayrollSalary = Conversion.ToDecimal(r.Cells["EmployeePayrollSalary"].Value);
					vd.LateDeductionAmount = Conversion.ToDecimal(r.Cells["LateDeductionAmount"].Value);
					vd.IncomeTaxAmount = Conversion.ToDecimal(r.Cells["IncomeTaxAmount"].Value);
					vd.LeaveAmount = Conversion.ToDecimal(r.Cells["LeaveAmount"].Value);
					vd.AdvanceAmount = Conversion.ToDecimal(r.Cells["AdvanceAmount"].Value);
					vd.LoanAmount = Conversion.ToDecimal(r.Cells["LoanAmount"].Value);
					vd.LoanNOS = Conversion.ToInt(r.Cells["LoanNOS"].Value);
					vd.PFAmount = Conversion.ToDecimal(r.Cells["PFAmount"].Value);
					vd.EOBIAmount = Conversion.ToDecimal(r.Cells["EOBIAmount"].Value);
					vd.FoodAmount = Conversion.ToDecimal(r.Cells["FoodAmount"].Value);
					vd.MiscLessAmount = Conversion.ToDecimal(r.Cells["MiscLessAmount"].Value);
					vd.TotalAmount = Conversion.ToDecimal(r.Cells["TotalLessAmount"].Value);
					vd.OTAmount = Conversion.ToDecimal(r.Cells["OTAmount"].Value);
					vd.TravellingAmount = Conversion.ToDecimal(r.Cells["TravellingAmount"].Value);
					vd.MedicalAmount = Conversion.ToDecimal(r.Cells["MedicalAmount"].Value);
					vd.MobileAmount = Conversion.ToDecimal(r.Cells["MobileAmount"].Value);
					vd.FuelAmount = Conversion.ToDecimal(r.Cells["FuelAmount"].Value);
					vd.MiscAddAmount = Conversion.ToDecimal(r.Cells["MiscAddAmount"].Value);
					vd.ArrearAmount = Conversion.ToDecimal(r.Cells["ArearAmount"].Value);
					vd.AddLessAmount = Conversion.ToDecimal(r.Cells["AddLessAmount"].Value);
					vd.SalaryWithoutAddLess = Math.Round(Conversion.ToDecimal(r.Cells["Salary"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					vd.NetSalary = Math.Round(Conversion.ToDecimal(r.Cells["NetSalary"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					vd.InwardId = 0L;
					vd.CreatedById = UserAccount.ID;
					vd.CreatedOn = DateTime.Now;
					vd.AlteredById = UserAccount.ID;
					vd.AlteredOn = DateTime.Now;
					if (vd.PayrollDetailId == 0L)
					{
						vd.ActionTypeId = 1;
					}
					else
					{
						vd.ActionTypeId = 2;
					}
					vd.UserLogId = UserAccount.ID;
					obj.PayrollDetailList.Add(vd);
				}
				if (lstDelete.Count > 0)
				{
					for (int i = 0; i < lstDelete.Count; i++)
					{
						obj.PayrollDetailList.Add(lstDelete[i]);
					}
				}
				Payroll.Save(obj);
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully");
				}
				else
				{
					MessageBox.Show("Record Update Successfully");
				}
				Reset();
				return;
			}
			throw new Exception("Grid record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnsave_Click(null, null);
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = ID;
			Payroll Obj = Payroll.GetByID(ID);
			if (Obj != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				btnShowDetail.Enabled = false;
				tabControl1.SelectedIndex = 0;
				txtDocDate.Value = Obj.PayrollDate;
				cmbMonth.Value = Conversion.ToInt((object)Obj.PayrollMonth);
				cmbYear.Value = Conversion.ToInt((object)Obj.PayrollYear);
				txtDocNo.Text = Conversion.ToString((object)Obj.DocNo);
				dtdetail.Rows.Clear();
				for (int i = 0; i < Obj.PayrollDetailList.Count; i++)
				{
					dtdetail.Rows.Add(Obj.PayrollDetailList[i].PayrollDetailId, Obj.PayrollDetailList[i].AccountId, Obj.PayrollDetailList[i].AccountCode, Obj.PayrollDetailList[i].EmployeeNo, Obj.PayrollDetailList[i].EmployeeId, Obj.PayrollDetailList[i].EmployeeName, "", "", "", Obj.PayrollDetailList[i].EmployeeHistoryId, Obj.PayrollDetailList[i].GrossSalary, Obj.PayrollDetailList[i].EmployeeDays, Obj.PayrollDetailList[i].EmployeePerDaySalary, Obj.PayrollDetailList[i].EmployeePayrollSalary, Obj.PayrollDetailList[i].LateDeductionAmount, Obj.PayrollDetailList[i].IncomeTaxAmount, Obj.PayrollDetailList[i].LeaveAmount, Obj.PayrollDetailList[i].AdvanceAmount, Obj.PayrollDetailList[i].LoanAmount, Obj.PayrollDetailList[i].LoanNOS, Obj.PayrollDetailList[i].PFAmount, Obj.PayrollDetailList[i].EOBIAmount, Obj.PayrollDetailList[i].MiscLessAmount, Obj.PayrollDetailList[i].TotalAmount, Obj.PayrollDetailList[i].OTAmount, Obj.PayrollDetailList[i].TravellingAmount, Obj.PayrollDetailList[i].MedicalAmount, Obj.PayrollDetailList[i].MobileAmount, Obj.PayrollDetailList[i].FuelAmount, Obj.PayrollDetailList[i].FoodAmount, Obj.PayrollDetailList[i].MiscAddAmount, Obj.PayrollDetailList[i].ArrearAmount, Obj.PayrollDetailList[i].SalaryWithoutAddLess, Obj.PayrollDetailList[i].AddLessAmount, Obj.PayrollDetailList[i].NetSalary);
				}
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
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
			txtDocDate.Value = DateTime.Now;
			((Control)(object)cmbMonth).Text = string.Empty;
			((Control)(object)cmbYear).Text = string.Empty;
			((Control)(object)cmbEmployeeCategory).Text = string.Empty;
			((Control)(object)cmbEmployeeDepartment).Text = string.Empty;
			RecId = 0;
			dtdetail.Rows.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnShowDetail.Enabled = true;
			txtDocDate.Focus();
			lstDelete.Clear();
			GenerateCode();
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			EmployeeDepartmentFill();
			EmployeeCategoryFill();
			YearFill();
			MonthFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			HistoryFill();
		}
	}

	private void HistoryFill()
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("PayrollId");
			dt.Columns.Add("PayrollDocNo", typeof(int));
			dt.Columns.Add("PayrollDate", typeof(DateTime));
			dt.Columns.Add("PayrollMonth", typeof(int));
			dt.Columns.Add("PayrollYear", typeof(int));
			dt.Columns.Add("EntryUserName");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 1000;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.BranchesId = UserAccount.BranchesId;
			if (ApprovedStatus)
			{
				obj.IsApproved = false;
			}
			else
			{
				obj.ApprovedFilter = "All";
			}
			DataTable dtHistory = Payroll.ReadAll(obj);
			if (dtHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["PayrollId"], dtHistory.Rows[i]["DocNo"], Conversion.ToDateTime(dtHistory.Rows[i]["PayrollDate"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["PayrollMonth"], dtHistory.Rows[i]["PayrollYear"], dtHistory.Rows[i]["EntryUserName"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["PayrollId"].Value);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["PayrollId"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(CommonServices.VoucherHeadIdGet(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["PayrollId"].Value), 1000));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["PayrollId"].Visible = false;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 60;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			DataGridHistory.ColumnAutoResize = false;
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FrmExportSalesContract_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.Return && Conversion.ToString(base.ActiveControl.Tag) == "grdhistory")
			{
				DataGridHistory_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowDetail_Click(object sender, EventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbMonth).ActiveRow == null)
			{
				MessageBox.Show("Month feild is required...");
				cmbMonth.Focus();
				return;
			}
			if (((UltraGridBase)cmbYear).ActiveRow == null)
			{
				MessageBox.Show("Year feild is required...");
				cmbYear.Focus();
				return;
			}
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DepartmentId = Conversion.ToInt(cmbEmployeeDepartment.Value),
				EmployeeCategoryId = Conversion.ToInt(cmbEmployeeCategory.Value),
				Month = Conversion.ToString((object)((Control)(object)cmbMonth).Text),
				Year = Conversion.ToInt(cmbYear.Value),
				MonthValue = Conversion.ToInt(cmbMonth.Value),
				DebitAccountId = Conversion.ToInt(CmbDebitAccount.Value),
				EmployeeId = Conversion.ToInt(cmbEmployee.Value)
			};
			dtGrid.Rows.Clear();
			dtdetail.Rows.Clear();
			DataSet ds = PayRollReports.GetEmployeePayrollSalary(val);
			dtGrid = ds?.Tables[0]?.Copy();
			if (dtGrid.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtGrid, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dtdetail.Rows.Add(0, dtGrid.Rows[i]["AccountId"], dtGrid.Rows[i]["AccountCode"], dtGrid.Rows[i]["EmployeeNo"], dtGrid.Rows[i]["EmployeeId"], dtGrid.Rows[i]["EmployeeName"], dtGrid.Rows[i]["DesignationName"], dtGrid.Rows[i]["DepartmentName"], dtGrid.Rows[i]["SectionName"], dtGrid.Rows[i]["EmployeeHistoryId"], dtGrid.Rows[i]["GrossSalary"], dtGrid.Rows[i]["EmployeeDays"], dtGrid.Rows[i]["EmployeePerDaySalary"], dtGrid.Rows[i]["EmployeePayrollSalary"], dtGrid.Rows[i]["LateDeductionAmount"], dtGrid.Rows[i]["IncomeTaxAmount"], dtGrid.Rows[i]["LeaveAmount"], dtGrid.Rows[i]["AdvanceAmount"], dtGrid.Rows[i]["LoanAmount"], dtGrid.Rows[i]["NoOfInstallments"], dtGrid.Rows[i]["PFAmount"], dtGrid.Rows[i]["EOBIAmount"], dtGrid.Rows[i]["MiscLessAmount"], dtGrid.Rows[i]["TotalLessAmount"], dtGrid.Rows[i]["OTAmount"], dtGrid.Rows[i]["TravellingAmount"], dtGrid.Rows[i]["MedicalAmount"], dtGrid.Rows[i]["MobileAmount"], dtGrid.Rows[i]["FuelAmount"], dtGrid.Rows[i]["FoodAmount"], dtGrid.Rows[i]["MiscAddAmount"], dtGrid.Rows[i]["ArearAmount"], dtGrid.Rows[i]["NetSalary"], 0, dtGrid.Rows[i]["NetSalary"]);
				}
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grdDetails.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetails).Name);
			if (fs != null)
			{
				grdDetails.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			new frmEmployeeCategory(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnEmployee_Click(object sender, EventArgs e)
	{
		try
		{
			new genDepartment(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.CurrentRow == null || (int)DataGridHistory.CurrentRow.RowType != 0 || DataGridHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			Payroll Obj = Payroll.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["PayrollId"].Value));
			if (Obj == null)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("EmployeeNo", typeof(int));
			dt.Columns.Add("EmployeeName");
			dt.Columns.Add("GrossSalary", typeof(double));
			dt.Columns.Add("EmployeeDays");
			dt.Columns.Add("EmployeePerDaySalary", typeof(decimal));
			dt.Columns.Add("EmployeePayrollSalary", typeof(double));
			dt.Columns.Add("LateDeductionAmount", typeof(double));
			dt.Columns.Add("IncomeTaxAmount", typeof(double));
			dt.Columns.Add("LeaveAmount", typeof(double));
			dt.Columns.Add("AdvanceAmount", typeof(double));
			dt.Columns.Add("LoanAmount", typeof(double));
			dt.Columns.Add("LoanNOS");
			dt.Columns.Add("PFAmount", typeof(double));
			dt.Columns.Add("EOBIAmount", typeof(double));
			dt.Columns.Add("FoodAmount", typeof(double));
			dt.Columns.Add("MiscLessAmount", typeof(double));
			dt.Columns.Add("TotalLessAmount", typeof(double));
			dt.Columns.Add("OTAmount", typeof(double));
			dt.Columns.Add("TravellingAmount", typeof(double));
			dt.Columns.Add("MedicalAmount", typeof(double));
			dt.Columns.Add("MobileAmount", typeof(double));
			dt.Columns.Add("FuelAmount", typeof(double));
			dt.Columns.Add("MiscAddAmount", typeof(double));
			dt.Columns.Add("ArrearAmount", typeof(double));
			dt.Columns.Add("Salary", typeof(double));
			dt.Columns.Add("AddLessAmount", typeof(double));
			dt.Columns.Add("NetSalary", typeof(double));
			dt.Rows.Clear();
			for (int i = 0; i < Obj.PayrollDetailList.Count; i++)
			{
				dt.Rows.Add(Obj.PayrollDetailList[i].EmployeeNo, Obj.PayrollDetailList[i].EmployeeName, Obj.PayrollDetailList[i].GrossSalary, Obj.PayrollDetailList[i].EmployeeDays, Obj.PayrollDetailList[i].EmployeePerDaySalary, Obj.PayrollDetailList[i].EmployeePayrollSalary, Obj.PayrollDetailList[i].LateDeductionAmount, Obj.PayrollDetailList[i].IncomeTaxAmount, Obj.PayrollDetailList[i].LeaveAmount, Obj.PayrollDetailList[i].AdvanceAmount, Obj.PayrollDetailList[i].LoanAmount, Obj.PayrollDetailList[i].LoanNOS, Obj.PayrollDetailList[i].PFAmount, Obj.PayrollDetailList[i].EOBIAmount, Obj.PayrollDetailList[i].FoodAmount, Obj.PayrollDetailList[i].MiscLessAmount, Obj.PayrollDetailList[i].TotalAmount, Obj.PayrollDetailList[i].OTAmount, Obj.PayrollDetailList[i].TravellingAmount, Obj.PayrollDetailList[i].MedicalAmount, Obj.PayrollDetailList[i].MobileAmount, Obj.PayrollDetailList[i].FuelAmount, Obj.PayrollDetailList[i].MiscAddAmount, Obj.PayrollDetailList[i].ArrearAmount, Conversion.ToDouble((object)Obj.PayrollDetailList[i].SalaryWithoutAddLess), Obj.PayrollDetailList[i].AddLessAmount, Obj.PayrollDetailList[i].NetSalary);
			}
			grdHistoryDetail.DataSource = dt;
			grdHistoryDetail.RetrieveStructure();
			grdHistoryDetail.ColumnAutoResize = false;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistoryDetail.RootTable.Columns)
			{
				GridEXColumn column = item;
				DataColumn dataColumn = dt.Columns[column.Key];
				if (dataColumn != null)
				{
					Type dataType = dataColumn.DataType;
					if (dataType == typeof(double))
					{
						column.AggregateFunction = (AggregateFunction)2;
						column.TextAlignment = (TextAlignment)3;
						column.FormatString = "#,#0.##";
						column.TotalFormatString = "#,#0.##";
					}
					else if (dataType == typeof(decimal))
					{
						column.AggregateFunction = (AggregateFunction)3;
						column.TextAlignment = (TextAlignment)3;
						column.FormatString = "#,#0.##";
						column.TotalFormatString = "#,#0.##";
					}
				}
			}
			CommonServices.GridAutoAdjustment(AryColWidth, grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsDropDownPrint_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((UltraGridBase)cmbMonth).ActiveRow == null)
			{
				MessageBox.Show("Month feild Required...");
				cmbMonth.Focus();
			}
			else if (((UltraGridBase)cmbYear).ActiveRow == null)
			{
				MessageBox.Show("Year feild Required...");
				cmbYear.Focus();
			}
			else if (((UltraGridBase)cmbMonth).ActiveRow != null && ((UltraGridBase)cmbYear).ActiveRow != null)
			{
				DataSet ds = PayRollReports.GetEmployeePostedSalary(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Month = ((Control)(object)cmbMonth).Text.Trim(),
					Year = Conversion.ToInt(cmbYear.Value),
					MonthValue = Conversion.ToInt(cmbMonth.Value),
					Id = RecId
				});
				DataTable dtrpt = ds?.Tables[0]?.Copy();
				if (dtrpt.Rows.Count <= 0 || dtrpt == null)
				{
					throw new Exception("Not Record Found For Display");
				}
				DatatableHelper.ReplaceCompLogoColumn(dtrpt, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				new Reporting().ShowReportWithDataTable(dtrpt, e.ClickedItem.Text + ".rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPayrollPosting_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Expected O, but got Unknown
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Expected O, but got Unknown
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_2aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aaf: Expected O, but got Unknown
		GridEXLayout grdDetails_DesignTimeLayout = new GridEXLayout();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PayRoll.frmPayrollPosting));
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance60 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdDetails = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.btnEmployeeCategory = new System.Windows.Forms.Button();
		this.btnEmployee = new System.Windows.Forms.Button();
		this.cmbMonth = new UltraCombo();
		this.cmbYear = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbEmployeeDepartment = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.btnShowDetail = new System.Windows.Forms.Button();
		this.cmbEmployeeCategory = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.tsDropDownPrint = new System.Windows.Forms.ToolStripDropDownButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdHistoryDetail = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.CmbDebitAccount = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.cmbEmployee = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetails).BeginInit();
		this.panel8.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCategory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1046, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1046, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1038, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetails);
		this.panel2.Controls.Add(this.panel8);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1032, 629);
		this.panel2.TabIndex = 0;
		grdDetails_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetails.DesignTimeLayout = grdDetails_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetails).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetails.DynamicFiltering = true;
		this.grdDetails.FilterMode = (FilterMode)2;
		this.grdDetails.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetails).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetails.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetails).Location = new System.Drawing.Point(0, 196);
		((System.Windows.Forms.Control)(object)this.grdDetails).Name = "grdDetails";
		((ScrollableBase)this.grdDetails).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetails).Size = new System.Drawing.Size(1032, 433);
		((System.Windows.Forms.Control)(object)this.grdDetails).TabIndex = 0;
		this.grdDetails.TotalRow = (InheritableBoolean)1;
		this.grdDetails.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetails).VisualStyle = (VisualStyle)4;
		this.grdDetails.CellUpdated += new ColumnActionEventHandler(grdDetails_CellUpdated);
		this.grdDetails.ColumnButtonClick += new ColumnActionEventHandler(grdDetails_ColumnButtonClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Controls.Add(this.label2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 169);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1032, 27);
		this.panel8.TabIndex = 505;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 17);
		this.label2.TabIndex = 0;
		this.label2.Text = "Detail";
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployee);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccount);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtDocNo);
		this.panel4.Controls.Add(this.btnEmployeeCategory);
		this.panel4.Controls.Add(this.btnEmployee);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMonth);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbYear);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.btnShowDetail);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtDocDate);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1032, 139);
		this.panel4.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(10, 34);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 522;
		this.label5.Text = "Doc No";
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNo.Location = new System.Drawing.Point(10, 56);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(91, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TabStop = false;
		this.btnEmployeeCategory.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeCategory.Location = new System.Drawing.Point(212, 105);
		this.btnEmployeeCategory.Name = "btnEmployeeCategory";
		this.btnEmployeeCategory.Size = new System.Drawing.Size(31, 26);
		this.btnEmployeeCategory.TabIndex = 6;
		this.btnEmployeeCategory.TabStop = false;
		this.btnEmployeeCategory.UseVisualStyleBackColor = true;
		this.btnEmployeeCategory.Click += new System.EventHandler(button1_Click);
		this.btnEmployee.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployee.Location = new System.Drawing.Point(442, 105);
		this.btnEmployee.Name = "btnEmployee";
		this.btnEmployee.Size = new System.Drawing.Size(31, 26);
		this.btnEmployee.TabIndex = 8;
		this.btnEmployee.TabStop = false;
		this.btnEmployee.UseVisualStyleBackColor = true;
		this.btnEmployee.Click += new System.EventHandler(btnEmployee_Click);
		this.cmbMonth.AllowNull = (DefaultableBoolean)1;
		this.cmbMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMonth).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMonth).Location = new System.Drawing.Point(242, 54);
		((System.Windows.Forms.Control)(object)this.cmbMonth).Name = "cmbMonth";
		((System.Windows.Forms.Control)(object)this.cmbMonth).Size = new System.Drawing.Size(96, 26);
		((System.Windows.Forms.Control)(object)this.cmbMonth).TabIndex = 2;
		this.cmbYear.AllowNull = (DefaultableBoolean)1;
		this.cmbYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance46;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbYear).Location = new System.Drawing.Point(340, 54);
		((System.Windows.Forms.Control)(object)this.cmbYear).Name = "cmbYear";
		((System.Windows.Forms.Control)(object)this.cmbYear).Size = new System.Drawing.Size(96, 26);
		((System.Windows.Forms.Control)(object)this.cmbYear).TabIndex = 3;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(340, 34);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(29, 15);
		this.label10.TabIndex = 520;
		this.label10.Text = "Year";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(242, 34);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(43, 15);
		this.label9.TabIndex = 519;
		this.label9.Text = "Month";
		this.cmbEmployeeDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbEmployeeDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).Location = new System.Drawing.Point(246, 105);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).Name = "cmbEmployeeDepartment";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).TabIndex = 7;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(246, 85);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(126, 15);
		this.label1.TabIndex = 518;
		this.label1.Text = "Employee Department";
		this.btnShowDetail.BackColor = System.Drawing.Color.Teal;
		this.btnShowDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowDetail.ForeColor = System.Drawing.Color.White;
		this.btnShowDetail.Location = new System.Drawing.Point(747, 105);
		this.btnShowDetail.Name = "btnShowDetail";
		this.btnShowDetail.Size = new System.Drawing.Size(58, 26);
		this.btnShowDetail.TabIndex = 10;
		this.btnShowDetail.Text = "Show Detail";
		this.btnShowDetail.UseVisualStyleBackColor = false;
		this.btnShowDetail.Click += new System.EventHandler(btnShowDetail_Click);
		this.cmbEmployeeCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbEmployeeCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Location = new System.Drawing.Point(10, 105);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Name = "cmbEmployeeCategory";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(10, 85);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(109, 15);
		this.label4.TabIndex = 516;
		this.label4.Text = "Employee Category";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label18);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1032, 27);
		this.panel6.TabIndex = 504;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(165, 19);
		this.label18.TabIndex = 25;
		this.label18.Text = "PAYROLL POSTING";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(104, 34);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 15);
		this.label3.TabIndex = 463;
		this.label3.Text = "Doc Date";
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(104, 56);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(134, 23);
		this.txtDocDate.TabIndex = 1;
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1032, 30);
		this.panel3.TabIndex = 0;
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripButton1, this.tsDropDownPrint });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1032, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.tsDropDownPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDownPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDownPrint.Name = "tsDropDownPrint";
		this.tsDropDownPrint.Size = new System.Drawing.Size(65, 24);
		this.tsDropDownPrint.Text = "Print";
		this.tsDropDownPrint.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDownPrint_DropDownItemClicked);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1038, 635);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.panel7);
		this.panel13.Controls.Add(this.panel18);
		this.panel13.Controls.Add(this.panel17);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1032, 629);
		this.panel13.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 403);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1032, 226);
		this.panel7.TabIndex = 2;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		this.grdHistoryDetail.ColumnAutoResize = true;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1032, 226);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 4;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel18.Controls.Add(this.panel5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 27);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1032, 376);
		this.panel18.TabIndex = 1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.ColumnAutoResize = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1032, 347);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Tag = "grdhistory";
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label51);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1032, 29);
		this.panel5.TabIndex = 505;
		this.label51.AutoSize = true;
		this.label51.BackColor = System.Drawing.Color.Teal;
		this.label51.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(5, 3);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(84, 19);
		this.label51.TabIndex = 25;
		this.label51.Text = "HISTORY";
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1032, 27);
		this.panel17.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(950, 25);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStrip3.Visible = false;
		this.CmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Location = new System.Drawing.Point(439, 54);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Name = "CmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Size = new System.Drawing.Size(305, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(439, 34);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(84, 15);
		this.label6.TabIndex = 524;
		this.label6.Text = "Debit Account";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(981, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetails;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar2.TabIndex = 47;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(982, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 46;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.cmbEmployee.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Location = new System.Drawing.Point(476, 105);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Name = "cmbEmployee";
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TabIndex = 9;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(479, 85);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(94, 15);
		this.label7.TabIndex = 526;
		this.label7.Text = "Employee Name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1046, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmPayrollPosting";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPayrollPosting_FormClosed);
		base.Load += new System.EventHandler(FrmExportSalesContract_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(FrmExportSalesContract_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetails).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCategory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).EndInit();
		base.ResumeLayout(false);
	}
}

}
