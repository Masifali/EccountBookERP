using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PayrollManagement;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM_Reports;

public class PayRollSalarySheetRpt : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private DataTable dtcol = new DataTable();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private Button btnshow;

	internal UltraCombo cmbEmployee;

	private Label label4;

	internal UltraCombo cmbDepartment;

	private Label label2;

	internal UltraCombo cmbMonth;

	internal UltraCombo cmbYear;

	private Label label10;

	private Label label9;

	private ToolStripDropDownButton tsDropDownPrint;

	private Panel panel5;

	private Label label1;

	private GroupBox groupBox1;

	private ToolStripButton btn1110SalaryList;

	private ToolStripButton btn1110ASalarySlip;

	internal UltraCombo CmbDebitAccount;

	private Label label3;

	private RadioButton RadPosted;

	private RadioButton RadUnPosted;

	private Label label6;

	internal UltraCombo cmbSection;

	public PayRollSalarySheetRpt(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void DailyAttendanceRpt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			dtcol.Columns.Add("EmployeeId", typeof(int));
			dtcol.Columns.Add("EmployeeHistoryId", typeof(int));
			dtcol.Columns.Add("AccountCode", typeof(string));
			dtcol.Columns.Add("EmployeeNo", typeof(int));
			dtcol.Columns.Add("EmployeeName", typeof(string));
			dtcol.Columns.Add("JoinDate", typeof(DateTime));
			dtcol.Columns.Add("DesignationName", typeof(string));
			dtcol.Columns.Add("DepartmentName", typeof(string));
			dtcol.Columns.Add("SectionName", typeof(string));
			dtcol.Columns.Add("DepartmentId", typeof(int));
			dtcol.Columns.Add("MONTH");
			dtcol.Columns.Add("YEAR");
			dtcol.Columns.Add("TotalYear");
			dtcol.Columns.Add("PresentDays");
			dtcol.Columns.Add("OffDays");
			dtcol.Columns.Add("LeaveDays");
			dtcol.Columns.Add("SalaryDays");
			dtcol.Columns.Add("AbsentDays");
			dtcol.Columns.Add("EmployeeDays", typeof(double));
			dtcol.Columns.Add("EmployeePerDaySalary", typeof(double));
			dtcol.Columns.Add("EmployeePayrollSalary", typeof(double));
			dtcol.Columns.Add("BasicSalary", typeof(double));
			dtcol.Columns.Add("GrossSalary", typeof(double));
			dtcol.Columns.Add("LateDeductionAmount", typeof(double));
			dtcol.Columns.Add("IncomeTaxAmount", typeof(double));
			dtcol.Columns.Add("LeaveAmount", typeof(double));
			dtcol.Columns.Add("AdvanceAmount", typeof(double));
			dtcol.Columns.Add("LoanAmount", typeof(double));
			dtcol.Columns.Add("NoOfInstallments", typeof(double));
			dtcol.Columns.Add("PFAmount", typeof(double));
			dtcol.Columns.Add("EOBIAmount", typeof(double));
			dtcol.Columns.Add("TotalLessAmount", typeof(double));
			dtcol.Columns.Add("OTAmount", typeof(double));
			dtcol.Columns.Add("TravellingAmount", typeof(double));
			dtcol.Columns.Add("MedicalAmount", typeof(double));
			dtcol.Columns.Add("FuelAmount", typeof(double));
			dtcol.Columns.Add("MiscLessAmount", typeof(double));
			dtcol.Columns.Add("MiscAddAmount", typeof(double));
			dtcol.Columns.Add("MobileAmount", typeof(double));
			dtcol.Columns.Add("FoodAmount", typeof(double));
			dtcol.Columns.Add("ArrearAmount", typeof(double));
			dtcol.Columns.Add("TotalAddition", typeof(double));
			dtcol.Columns.Add("AddLessAmount", typeof(double));
			dtcol.Columns.Add("NetSalary", typeof(double));
			YearFill();
			MonthFill();
			DepartmentFill();
			EmployeeFill();
			GetAccountsFromEmployee();
			cmbMonth.Focus();
			ReportsLoad();
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

	private void DepartmentFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDepartment = genDepartment.Getall(new genDepartment
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDepartment.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDepartment, cmbDepartment, "DepartmentId", "DepartmentName", "DepartmentName", false);
				((UltraGridBase)cmbDepartment).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
				DepartmentId = Conversion.ToInt(cmbDepartment.Value)
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

	private void Section()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			genSection val = new genSection
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			int departmentId = Conversion.ToInt(cmbDepartment.Value);
			EnumerableRowCollection<DataRow> filteredRows = from r in genSection.Getall(val).AsEnumerable()
				where r.Field<int>("DepartmentId") == departmentId
				select r;
			if (filteredRows.Any())
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("SectionName", typeof(string));
				foreach (DataRow row in filteredRows)
				{
					dt.Rows.Add(row["SectionId"], row["SectionName"]);
				}
				DDL.BindDDL(dt, cmbSection, "Id", "SectionName", "Section Name", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)cmbSection).DataSource = null;
				((Control)(object)cmbSection).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAccountsFromEmployee()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DepartmentId = Conversion.ToInt(cmbDepartment.Value)
			};
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

	private void cmbDepartment_Leave(object sender, EventArgs e)
	{
		try
		{
			EmployeeFill();
			Section();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistory()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			DataSet ds = new DataSet();
			if (((UltraGridBase)cmbMonth).ActiveRow == null)
			{
				MessageBox.Show("Month field Required...");
				cmbMonth.Focus();
			}
			else if (((UltraGridBase)cmbYear).ActiveRow == null)
			{
				MessageBox.Show("Year field Required...");
				cmbYear.Focus();
			}
			else if (((UltraGridBase)cmbMonth).ActiveRow != null && ((UltraGridBase)cmbYear).ActiveRow != null)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.DepartmentId = Conversion.ToInt(cmbDepartment.Value);
				obj.SectionId = Conversion.ToInt(cmbSection.Value);
				obj.EmployeeId = Conversion.ToInt(cmbEmployee.Value);
				obj.Month = ((Control)(object)cmbMonth).Text.Trim();
				obj.Year = Conversion.ToInt(cmbYear.Value);
				obj.MonthValue = Conversion.ToInt(cmbMonth.Value);
				obj.DebitAccountId = Conversion.ToInt(CmbDebitAccount.Value);
				if (RadUnPosted.Checked)
				{
					ds = PayRollReports.GetEmployeePayrollSalary(obj);
					dtGrid = ds?.Tables[0]?.Copy();
				}
				else if (RadPosted.Checked)
				{
					ds = PayRollReports.GetEmployeePostedSalary(obj);
					dtGrid = ds?.Tables[0]?.Copy();
				}
				if (dtGrid.Rows.Count > 0)
				{
					DatatableHelper.ReplaceCompLogoColumn(dtGrid, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
					dtcol.Rows.Clear();
					for (int i = 0; i < dtGrid.Rows.Count; i++)
					{
						dtcol.Rows.Add(dtGrid.Rows[i]["EmployeeId"], dtGrid.Rows[i]["EmployeeHistoryId"], dtGrid.Rows[i]["AccountCode"], dtGrid.Rows[i]["EmployeeNo"], dtGrid.Rows[i]["EmployeeName"], Conversion.ToDateTime(dtGrid.Rows[i]["JoinDate"]).ToString("dd-MMM-yyyy"), dtGrid.Rows[i]["DesignationName"], dtGrid.Rows[i]["DepartmentName"], dtGrid.Rows[i]["SectionName"], dtGrid.Rows[i]["DepartmentId"], dtGrid.Rows[i]["MONTH"], dtGrid.Rows[i]["YEAR"], dtGrid.Rows[i]["TotalYear"], dtGrid.Rows[i]["PresentDays"], dtGrid.Rows[i]["OffDays"], dtGrid.Rows[i]["LeaveDays"], dtGrid.Rows[i]["SalaryDays"], dtGrid.Rows[i]["AbsentDays"], dtGrid.Rows[i]["EmployeeDays"], dtGrid.Rows[i]["EmployeePerDaySalary"], dtGrid.Rows[i]["EmployeePayrollSalary"], dtGrid.Rows[i]["BasicSalary"], dtGrid.Rows[i]["GrossSalary"], dtGrid.Rows[i]["LateDeductionAmount"], dtGrid.Rows[i]["IncomeTaxAmount"], dtGrid.Rows[i]["LeaveAmount"], dtGrid.Rows[i]["AdvanceAmount"], dtGrid.Rows[i]["LoanAmount"], dtGrid.Rows[i]["NoOfInstallments"], dtGrid.Rows[i]["PFAmount"], dtGrid.Rows[i]["EOBIAmount"], dtGrid.Rows[i]["TotalLessAmount"], dtGrid.Rows[i]["OTAmount"], dtGrid.Rows[i]["TravellingAmount"], dtGrid.Rows[i]["MedicalAmount"], dtGrid.Rows[i]["FuelAmount"], dtGrid.Rows[i]["MiscLessAmount"], dtGrid.Rows[i]["MiscAddAmount"], dtGrid.Rows[i]["MobileAmount"], dtGrid.Rows[i]["FoodAmount"], dtGrid.Rows[i]["ArearAmount"], dtGrid.Rows[i]["TotalAddition"], dtGrid.Rows[i]["AddLessAmount"], dtGrid.Rows[i]["NetSalary"]);
					}
					grd.DataSource = dtcol;
					grd.RetrieveStructure();
					GridSetting();
				}
				else
				{
					grd.ClearStructure();
				}
			}
			else
			{
				grd.ClearStructure();
			}
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
			GridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["EmployeeId"].Visible = false;
			grd.RootTable.Columns["DepartmentId"].Visible = false;
			grd.RootTable.Columns["EmployeeHistoryId"].Visible = false;
			grd.RootTable.Columns["ArrearAmount"].Caption = "Arear Amount";
			grd.RootTable.Columns["EmployeeNo"].ColumnType = (ColumnType)5;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode - 20;
			grd.RootTable.Columns["EmployeeNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["EmployeeName"].Width = Constants.InventoryConstants.SupplierName + 30;
			grd.RootTable.Columns["JoinDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grd.RootTable.Columns["DesignationName"].Width = 150;
			grd.RootTable.Columns["DepartmentName"].Width = 130;
			grd.RootTable.Columns["SectionName"].Width = 120;
			grd.RootTable.Columns["MONTH"].Width = 70;
			grd.RootTable.Columns["YEAR"].Width = 60;
			grd.RootTable.Columns["TotalYear"].Width = 65;
			grd.RootTable.Columns["PresentDays"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["OffDays"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["LeaveDays"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["SalaryDays"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["AbsentDays"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["EmployeeDays"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["EmployeePerDaySalary"].Width = Constants.InventoryConstants.AmountConstant + 20;
			grd.RootTable.Columns["EmployeePayrollSalary"].Width = Constants.InventoryConstants.AmountConstant + 20;
			grd.RootTable.Columns["BasicSalary"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["GrossSalary"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["LateDeductionAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["IncomeTaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["LeaveAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["AdvanceAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["LoanAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["NoOfInstallments"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["PFAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["EOBIAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["TotalLessAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["OTAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["TravellingAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["MedicalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FuelAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["MiscLessAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["MiscAddAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["MobileAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FoodAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ArrearAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["TotalAddition"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["AddLessAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["NetSalary"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["NetSalary"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetSalary"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetSalary"].FormatString = "0,0";
			grd.RootTable.Columns["NetSalary"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["AddLessAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLessAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLessAmount"].FormatString = "0,0";
			grd.RootTable.Columns["AddLessAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["TotalAddition"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalAddition"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalAddition"].FormatString = "0,0";
			grd.RootTable.Columns["TotalAddition"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ArrearAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ArrearAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ArrearAmount"].FormatString = "0,0";
			grd.RootTable.Columns["ArrearAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["FoodAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FoodAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FoodAmount"].FormatString = "0,0";
			grd.RootTable.Columns["FoodAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["MobileAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["MobileAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["MobileAmount"].FormatString = "0,0";
			grd.RootTable.Columns["MobileAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["MiscAddAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["MiscAddAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["MiscAddAmount"].FormatString = "0,0";
			grd.RootTable.Columns["MiscAddAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["MiscLessAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["MiscLessAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["MiscLessAmount"].FormatString = "0,0";
			grd.RootTable.Columns["MiscLessAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["FuelAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FuelAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FuelAmount"].FormatString = "0,0";
			grd.RootTable.Columns["FuelAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["MedicalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["MedicalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["MedicalAmount"].FormatString = "0,0";
			grd.RootTable.Columns["MedicalAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["TravellingAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TravellingAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TravellingAmount"].FormatString = "0,0";
			grd.RootTable.Columns["TravellingAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["OTAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OTAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OTAmount"].FormatString = "0,0";
			grd.RootTable.Columns["OTAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["TotalLessAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalLessAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalLessAmount"].FormatString = "0,0";
			grd.RootTable.Columns["TotalLessAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EOBIAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EOBIAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EOBIAmount"].FormatString = "0,0";
			grd.RootTable.Columns["EOBIAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["PFAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["PFAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PFAmount"].FormatString = "0,0";
			grd.RootTable.Columns["PFAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NoOfInstallments"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NoOfInstallments"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NoOfInstallments"].FormatString = "0,0";
			grd.RootTable.Columns["NoOfInstallments"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["LoanAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LoanAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LoanAmount"].FormatString = "0,0";
			grd.RootTable.Columns["LoanAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["AdvanceAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AdvanceAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AdvanceAmount"].FormatString = "0,0";
			grd.RootTable.Columns["AdvanceAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["LeaveAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LeaveAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LeaveAmount"].FormatString = "0,0";
			grd.RootTable.Columns["LeaveAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["IncomeTaxAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["IncomeTaxAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["IncomeTaxAmount"].FormatString = "0,0";
			grd.RootTable.Columns["IncomeTaxAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["LateDeductionAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LateDeductionAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LateDeductionAmount"].FormatString = "0,0";
			grd.RootTable.Columns["LateDeductionAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["GrossSalary"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossSalary"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossSalary"].FormatString = "0,0";
			grd.RootTable.Columns["GrossSalary"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["BasicSalary"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BasicSalary"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BasicSalary"].FormatString = "0,0";
			grd.RootTable.Columns["BasicSalary"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EmployeePayrollSalary"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmployeePayrollSalary"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmployeePayrollSalary"].FormatString = "0,0";
			grd.RootTable.Columns["EmployeePayrollSalary"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EmployeePerDaySalary"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmployeePerDaySalary"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmployeePerDaySalary"].FormatString = "0,0";
			grd.RootTable.Columns["EmployeePerDaySalary"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EmployeeDays"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmployeeDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmployeeDays"].FormatString = "0,0";
			grd.RootTable.Columns["EmployeeDays"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["PresentDays"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["PresentDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PresentDays"].FormatString = "0,0";
			grd.RootTable.Columns["PresentDays"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["OffDays"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OffDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OffDays"].FormatString = "0,0";
			grd.RootTable.Columns["OffDays"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["LeaveDays"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LeaveDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LeaveDays"].FormatString = "0,0";
			grd.RootTable.Columns["LeaveDays"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["SalaryDays"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SalaryDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SalaryDays"].FormatString = "0,0";
			grd.RootTable.Columns["SalaryDays"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["AbsentDays"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AbsentDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AbsentDays"].FormatString = "0,0";
			grd.RootTable.Columns["AbsentDays"].TotalFormatString = "#,##0.##";
			ctrlGrdBar1_Load(null, null);
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

	private void tsDropDownPrint_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count > 0 && dtGrid != null)
			{
				new Reporting().ShowReportWithDataTable(dtGrid, e.ClickedItem.Text + ".rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PayRollSalarySheetRpt_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				tsDropDownPrint_DropDownItemClicked(null, null);
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		YearFill();
		MonthFill();
		DepartmentFill();
		EmployeeFill();
		cmbDepartment.Focus();
	}

	private void PayRollSalarySheetRpt_FormClosed(object sender, FormClosedEventArgs e)
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

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (e.Column.Key == "EmployeeNo")
			{
				SalarySlipRpt(Conversion.ToInt(r.Cells["EmployeeId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn1110SalaryList_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((UltraGridBase)cmbMonth).ActiveRow == null || ((UltraGridBase)cmbYear).ActiveRow == null)
			{
				throw new Exception("Month Or Year Field Required");
			}
			DataTable dt = PayRollReports.EmployeeSalarySlip_Rpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Year = Conversion.ToInt(cmbYear.Value),
				MonthValue = Conversion.ToInt(cmbMonth.Value),
				DepartmentId = Conversion.ToInt(cmbDepartment.Value),
				EmployeeId = Conversion.ToInt(cmbEmployee.Value),
				DebitAccountId = Conversion.ToInt(CmbDebitAccount.Value)
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "1110-EmployeeSalarySlip.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn1110ASalarySlip_Click(object sender, EventArgs e)
	{
		try
		{
			SalarySlipRpt(Conversion.ToInt(cmbEmployee.Value), Conversion.ToInt(cmbDepartment.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SalarySlipRpt(int EmployeeId = 0, int DepartmentId = 0)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((UltraGridBase)cmbMonth).ActiveRow == null || ((UltraGridBase)cmbYear).ActiveRow == null)
			{
				throw new Exception("Month Or Year Field Required");
			}
			DataTable dt = PayRollReports.EmployeeSalarySlip_Rpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Year = Conversion.ToInt(cmbYear.Value),
				MonthValue = Conversion.ToInt(cmbMonth.Value),
				DepartmentId = DepartmentId,
				EmployeeId = EmployeeId,
				DebitAccountId = Conversion.ToInt(CmbDebitAccount.Value)
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "1110B_EmployeeSalarySlip.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
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
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance63 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
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
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RadPosted = new System.Windows.Forms.RadioButton();
		this.CmbDebitAccount = new UltraCombo();
		this.RadUnPosted = new System.Windows.Forms.RadioButton();
		this.label3 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbMonth = new UltraCombo();
		this.cmbYear = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbEmployee = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbDepartment = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.tsDropDownPrint = new System.Windows.Forms.ToolStripDropDownButton();
		this.btn1110SalaryList = new System.Windows.Forms.ToolStripButton();
		this.btn1110ASalarySlip = new System.Windows.Forms.ToolStripButton();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbSection = new UltraCombo();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1002, 661);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 166);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1002, 495);
		this.panel5.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1002, 495);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 140);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1002, 26);
		this.panel4.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(953, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 54);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1002, 86);
		this.panel3.TabIndex = 1;
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSection);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccount);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMonth);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbYear);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployee);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(955, 77);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.RadPosted.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.RadPosted.AutoSize = true;
		this.RadPosted.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadPosted.ForeColor = System.Drawing.Color.White;
		this.RadPosted.Location = new System.Drawing.Point(493, 29);
		this.RadPosted.Name = "RadPosted";
		this.RadPosted.Size = new System.Drawing.Size(233, 20);
		this.RadPosted.TabIndex = 7;
		this.RadPosted.Text = "Salary with Financial Posting";
		this.RadPosted.UseVisualStyleBackColor = true;
		this.CmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Location = new System.Drawing.Point(740, 15);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Name = "CmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).TabIndex = 5;
		this.RadUnPosted.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.RadUnPosted.AutoSize = true;
		this.RadUnPosted.Checked = true;
		this.RadUnPosted.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadUnPosted.ForeColor = System.Drawing.Color.White;
		this.RadUnPosted.Location = new System.Drawing.Point(737, 29);
		this.RadUnPosted.Name = "RadUnPosted";
		this.RadUnPosted.Size = new System.Drawing.Size(257, 20);
		this.RadUnPosted.TabIndex = 6;
		this.RadUnPosted.TabStop = true;
		this.RadUnPosted.Text = "Salary without Financial Posting";
		this.RadUnPosted.UseVisualStyleBackColor = true;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(646, 21);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 127;
		this.label3.Text = "Debit Account";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(190, 24);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(29, 15);
		this.label10.TabIndex = 1;
		this.label10.Text = "Year";
		this.cmbMonth.AllowNull = (DefaultableBoolean)1;
		this.cmbMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
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
		((System.Windows.Forms.Control)(object)this.cmbMonth).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMonth).Location = new System.Drawing.Point(82, 18);
		((System.Windows.Forms.Control)(object)this.cmbMonth).Name = "cmbMonth";
		((System.Windows.Forms.Control)(object)this.cmbMonth).Size = new System.Drawing.Size(104, 26);
		((System.Windows.Forms.Control)(object)this.cmbMonth).TabIndex = 0;
		this.cmbYear.AllowNull = (DefaultableBoolean)1;
		this.cmbYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance62;
		((SpecialBoxBase)((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbYear).Location = new System.Drawing.Point(224, 18);
		((System.Windows.Forms.Control)(object)this.cmbYear).Name = "cmbYear";
		((System.Windows.Forms.Control)(object)this.cmbYear).Size = new System.Drawing.Size(105, 26);
		((System.Windows.Forms.Control)(object)this.cmbYear).TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(8, 24);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(43, 15);
		this.label9.TabIndex = 124;
		this.label9.Text = "Month";
		this.cmbEmployee.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Location = new System.Drawing.Point(400, 44);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Name = "cmbEmployee";
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TabIndex = 4;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.SystemColors.Window;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(893, 44);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(335, 50);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(59, 15);
		this.label4.TabIndex = 119;
		this.label4.Text = "Employee";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 51);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 15);
		this.label2.TabIndex = 115;
		this.label2.Text = "Department";
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(82, 45);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Leave += new System.EventHandler(cmbDepartment_Leave);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.RadPosted);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Controls.Add(this.RadUnPosted);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1002, 54);
		this.panel2.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(11, 31);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(112, 19);
		this.label1.TabIndex = 116;
		this.label1.Text = "Salary Sheet";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.tsDropDownPrint, this.btn1110SalaryList, this.btn1110ASalarySlip });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1002, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.tsDropDownPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsDropDownPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDownPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDownPrint.Name = "tsDropDownPrint";
		this.tsDropDownPrint.Size = new System.Drawing.Size(70, 24);
		this.tsDropDownPrint.Text = "Print";
		this.tsDropDownPrint.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDownPrint_DropDownItemClicked);
		this.btn1110SalaryList.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn1110SalaryList.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn1110SalaryList.Name = "btn1110SalaryList";
		this.btn1110SalaryList.Size = new System.Drawing.Size(109, 24);
		this.btn1110SalaryList.Text = "1110-SalaryList";
		this.btn1110SalaryList.Click += new System.EventHandler(btn1110SalaryList_Click);
		this.btn1110ASalarySlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn1110ASalarySlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn1110ASalarySlip.Name = "btn1110ASalarySlip";
		this.btn1110ASalarySlip.Size = new System.Drawing.Size(117, 24);
		this.btn1110ASalarySlip.Text = "1110B-SalarySlip";
		this.btn1110ASalarySlip.Click += new System.EventHandler(btn1110ASalarySlip_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(335, 21);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(47, 15);
		this.label6.TabIndex = 129;
		this.label6.Text = "Section";
		this.cmbSection.AllowNull = (DefaultableBoolean)1;
		this.cmbSection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbSection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSection).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Location = new System.Drawing.Point(400, 15);
		((System.Windows.Forms.Control)(object)this.cmbSection).Name = "cmbSection";
		((System.Windows.Forms.Control)(object)this.cmbSection).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.cmbSection).TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1002, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PayRollSalarySheetRpt";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(PayRollSalarySheetRpt_FormClosed);
		base.Load += new System.EventHandler(DailyAttendanceRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PayRollSalarySheetRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMonth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).EndInit();
		base.ResumeLayout(false);
	}
}
