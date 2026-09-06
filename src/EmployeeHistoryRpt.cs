// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.HRM_Reports
{

public class EmployeeHistoryRpt : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel4;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	internal UltraCombo cmbIsActive;

	private Label label1;

	internal UltraCombo cmbEmployee;

	internal UltraCombo cmbDepartment;

	internal UltraCombo cmbSection;

	internal UltraCombo cmbShift;

	internal UltraCombo cmbDesignation;

	private Label label6;

	private Panel panel3;

	private GroupBox groupBox1;

	private Label label10;

	private Label label12;

	private Label label8;

	private Label label9;

	private Label label2;

	internal GridEX grd;

	private Panel panel1;

	private ToolStripButton btnPrint1101_01;

	public EmployeeHistoryRpt(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void DailyAttendanceRpt_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(980, 700);
		DepartmentFill();
		SectionFill();
		ShiftFill();
		DesignationFill();
		EmployeeFill();
		IsMissingFill();
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

	private void DepartmentFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("name");
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
				DDL.BindDDL(dt, cmbDepartment, "Id", "name", "Department", ZeroIndex: false);
				((UltraGridBase)cmbDepartment).DisplayLayout.Bands[0].Columns[1].Width = 201;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SectionFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("name");
			DataTable dtDepartment = genSection.Getall(new genSection
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDepartment.Rows.Count > 0)
			{
				for (int i = 0; i < dtDepartment.Rows.Count; i++)
				{
					dt.Rows.Add(dtDepartment.Rows[i]["SectionId"], dtDepartment.Rows[i]["SectionName"]);
				}
				DDL.BindDDL(dt, cmbSection, "Id", "name", "Section", ZeroIndex: false);
				((UltraGridBase)cmbSection).DisplayLayout.Bands[0].Columns[1].Width = 201;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShiftFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("name");
			DataTable dtShift = genShift.Getall(new genShift
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtShift.Rows.Count > 0)
			{
				for (int i = 0; i < dtShift.Rows.Count; i++)
				{
					dt.Rows.Add(dtShift.Rows[i]["ShiftId"], dtShift.Rows[i]["ShiftName"]);
				}
				DDL.BindDDL(dt, cmbShift, "Id", "name", "Shift", ZeroIndex: false);
				((UltraGridBase)cmbShift).DisplayLayout.Bands[0].Columns[1].Width = 201;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DesignationFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("name");
			DataTable dtDesignation = genDesignation.Getall(new genDesignation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDesignation.Rows.Count > 0)
			{
				for (int i = 0; i < dtDesignation.Rows.Count; i++)
				{
					dt.Rows.Add(dtDesignation.Rows[i]["DesignationId"], dtDesignation.Rows[i]["DesignationName"]);
				}
				DDL.BindDDL(dt, cmbDesignation, "Id", "name", "Designation", ZeroIndex: false);
				((UltraGridBase)cmbDesignation).DisplayLayout.Bands[0].Columns[1].Width = 201;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("name");
			DataTable dtEmployeeName = genEmployee.Getall(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtEmployeeName.Rows.Count > 0)
			{
				for (int i = 0; i < dtEmployeeName.Rows.Count; i++)
				{
					dt.Rows.Add(dtEmployeeName.Rows[i]["EmployeeId"], dtEmployeeName.Rows[i]["EmployeeName"]);
				}
				DDL.BindDDL(dt, cmbEmployee, "Id", "name", "Employee", ZeroIndex: false);
				((UltraGridBase)cmbEmployee).DisplayLayout.Bands[0].Columns[1].Width = 201;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void IsMissingFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("name");
			dt.Rows.Add(1, "Active");
			dt.Rows.Add(2, "InActive");
			DDL.BindDDL(dt, cmbIsActive, "Id", "name", "Status", ZeroIndex: false);
			((UltraGridBase)cmbIsActive).DisplayLayout.Bands[0].Columns[1].Width = 201;
			((UltraGridBase)cmbIsActive).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EmployeeId = Conversion.ToInt(cmbEmployee.Value);
			obj.DepartmentId = Conversion.ToInt(cmbDepartment.Value);
			obj.PartyLocationId = Conversion.ToInt(cmbDesignation.Value);
			obj.EmployeeId = Conversion.ToInt(cmbEmployee.Value);
			obj.SectionId = Conversion.ToInt(cmbSection.Value);
			obj.ShiftId = Conversion.ToInt(cmbShift.Value);
			if (((UltraGridBase)cmbIsActive).ActiveRow != null)
			{
				if (Conversion.ToInt(cmbIsActive.Value) == 1)
				{
					obj.IsApproved = true;
				}
				else
				{
					obj.IsApproved = false;
				}
			}
			else
			{
				obj.ApprovedFilter = "All";
			}
			DataSet ds = HRM_Reports.EmployeeHistoryRpt(obj);
			dtGrid = ds?.Tables[0]?.Copy();
			if (dtGrid.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtGrid, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("EmployeeId", typeof(int));
				dtcol.Columns.Add("EmployeeNo", typeof(int));
				dtcol.Columns.Add("EmployeeName", typeof(string));
				dtcol.Columns.Add("DOB", typeof(DateTime));
				dtcol.Columns.Add("LocationName", typeof(string));
				dtcol.Columns.Add("DepartmentName", typeof(string));
				dtcol.Columns.Add("DesignationName", typeof(string));
				dtcol.Columns.Add("SectionName", typeof(string));
				dtcol.Columns.Add("JoiningDate", typeof(DateTime));
				dtcol.Columns.Add("CNIC", typeof(string));
				dtcol.Columns.Add("MobileNo", typeof(string));
				dtcol.Columns.Add("TotalSalary", typeof(decimal));
				dtcol.Columns.Add("CurrentGlBalance", typeof(double));
				dtcol.Columns.Add("ProbationEnding", typeof(DateTime));
				dtcol.Columns.Add("Active", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGrid.Rows[i]["EmployeeId"], dtGrid.Rows[i]["EmployeeNo"], dtGrid.Rows[i]["EmployeeName"], dtGrid.Rows[i]["DOB"], dtGrid.Rows[i]["LocationName"], dtGrid.Rows[i]["DepartmentName"], dtGrid.Rows[i]["DesignationName"], dtGrid.Rows[i]["SectionName"], dtGrid.Rows[i]["JoiningDate"], dtGrid.Rows[i]["CNIC"], dtGrid.Rows[i]["MobileNo"], dtGrid.Rows[i]["TotalSalary"], dtGrid.Rows[i]["CurrentGlBalance"], dtGrid.Rows[i]["ProbationEnding"], dtGrid.Rows[i]["Active"]);
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
		try
		{
			grd.RootTable.Columns["EmployeeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, null, null, null, ColumnSetting: false);
			grd.RootTable.Columns["EmployeeNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["EmployeeName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["DOB"].Width = Constants.InventoryConstants.DateConstant + 20;
			grd.RootTable.Columns["LocationName"].Width = Constants.InventoryConstants.StringConstant + 30;
			grd.RootTable.Columns["DepartmentName"].Width = Constants.InventoryConstants.WareHouseName + 20;
			grd.RootTable.Columns["DesignationName"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["SectionName"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["JoiningDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grd.RootTable.Columns["CNIC"].Width = Constants.InventoryConstants.DriverCnicNoConstant + 30;
			grd.RootTable.Columns["MobileNo"].Width = Constants.InventoryConstants.CellNo;
			grd.RootTable.Columns["TotalSalary"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ProbationEnding"].Width = Constants.InventoryConstants.DateConstant + 20;
			grd.RootTable.Columns["Active"].Width = Constants.InventoryConstants.StatusConstant;
			grd.RootTable.Columns["DOB"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["JoiningDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["ProbationEnding"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["TotalSalary"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalSalary"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalSalary"].FormatString = "0,0";
			grd.RootTable.Columns["TotalSalary"].TotalFormatString = "0,0";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count > 0 && dtGrid != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dtGrid, "1101-EmployeeListDepartmentWiseActiveInactive.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DailyAttendanceRpt_KeyDown(object sender, KeyEventArgs e)
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
				print_Click(null, null);
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
		try
		{
			((Control)(object)cmbEmployee).Text = string.Empty;
			((Control)(object)cmbDepartment).Text = string.Empty;
			((UltraGridBase)cmbIsActive).Rows[0].Activate();
			((Control)(object)cmbSection).Text = string.Empty;
			((Control)(object)cmbShift).Text = string.Empty;
			((Control)(object)cmbDesignation).Text = string.Empty;
			GridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeHistoryRpt_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnPrint1101_01_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count > 0 && dtGrid != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dtGrid, "1101_01-EmployeeListDepartmentWiseActiveInactive.rpt");
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
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbDesignation = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbSection = new UltraCombo();
		this.cmbShift = new UltraCombo();
		this.cmbIsActive = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbEmployee = new UltraCombo();
		this.cmbDepartment = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.grd = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnPrint1101_01 = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbIsActive).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 32);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(982, 27);
		this.panel4.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(162, 19);
		this.label2.TabIndex = 119;
		this.label2.Text = "Employee Register";
		this.cmbDesignation.AllowNull = (DefaultableBoolean)1;
		this.cmbDesignation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDesignation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDesignation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDesignation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Location = new System.Drawing.Point(80, 49);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Name = "cmbDesignation";
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 55);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(71, 15);
		this.label6.TabIndex = 127;
		this.label6.Text = "Designation";
		this.cmbSection.AllowNull = (DefaultableBoolean)1;
		this.cmbSection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbSection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSection).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Location = new System.Drawing.Point(336, 19);
		((System.Windows.Forms.Control)(object)this.cmbSection).Name = "cmbSection";
		((System.Windows.Forms.Control)(object)this.cmbSection).Size = new System.Drawing.Size(149, 26);
		((System.Windows.Forms.Control)(object)this.cmbSection).TabIndex = 2;
		this.cmbShift.AllowNull = (DefaultableBoolean)1;
		this.cmbShift.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShift.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShift.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShift).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbShift).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShift).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShift).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShift.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShift).Location = new System.Drawing.Point(336, 49);
		((System.Windows.Forms.Control)(object)this.cmbShift).Name = "cmbShift";
		((System.Windows.Forms.Control)(object)this.cmbShift).Size = new System.Drawing.Size(149, 26);
		((System.Windows.Forms.Control)(object)this.cmbShift).TabIndex = 3;
		this.cmbIsActive.AllowNull = (DefaultableBoolean)1;
		this.cmbIsActive.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbIsActive.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbIsActive.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbIsActive).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbIsActive).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance6).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbIsActive).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbIsActive).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbIsActive.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbIsActive).Location = new System.Drawing.Point(550, 19);
		((System.Windows.Forms.Control)(object)this.cmbIsActive).Name = "cmbIsActive";
		((System.Windows.Forms.Control)(object)this.cmbIsActive).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.cmbIsActive).TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(491, 25);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 121;
		this.label1.Text = "Is Active";
		this.cmbEmployee.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance41).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Location = new System.Drawing.Point(550, 49);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Name = "cmbEmployee";
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TabIndex = 5;
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(80, 19);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 0;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(851, 49);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(982, 32);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.print, this.btnPrint1101_01 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(982, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(100, 24);
		this.print.Text = "1101-Print";
		this.print.Click += new System.EventHandler(print_Click);
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
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 186);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(982, 264);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 59);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(982, 100);
		this.panel3.TabIndex = 128;
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDesignation);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSection);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployee);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbIsActive);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShift);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(944, 87);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(491, 55);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(59, 15);
		this.label10.TabIndex = 123;
		this.label10.Text = "Employee";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(6, 25);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(71, 15);
		this.label12.TabIndex = 122;
		this.label12.Text = "Department";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(286, 55);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(32, 15);
		this.label8.TabIndex = 94;
		this.label8.Text = "Shift";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(286, 25);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(47, 15);
		this.label9.TabIndex = 90;
		this.label9.Text = "Section";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 159);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(982, 27);
		this.panel1.TabIndex = 129;
		this.btnPrint1101_01.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint1101_01.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint1101_01.Name = "btnPrint1101_01";
		this.btnPrint1101_01.Size = new System.Drawing.Size(124, 24);
		this.btnPrint1101_01.Text = "1101_01-Print";
		this.btnPrint1101_01.Click += new System.EventHandler(btnPrint1101_01_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(933, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(982, 450);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel2);
		base.KeyPreview = true;
		base.Name = "EmployeeHistoryRpt";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(EmployeeHistoryRpt_FormClosing);
		base.Load += new System.EventHandler(DailyAttendanceRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DailyAttendanceRpt_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbIsActive).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
