// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.DAW;
using Architecture.BLL.HRM.PayrollManagement;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Commentaries;
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
namespace Architecture.WinApp.MultiApprovalDashboard
{

public class PayRollPostingForMultiApproval : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable grdlst = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private bool AllowEditOnVoucherNoinReports;

	public int DocumentTypeId;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private Label label23;

	private DateTimePicker fromdate;

	private Label label33;

	private DateTimePicker ToDate;

	internal GridEX grdsaleorder;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar3;

	private Button btnshow;

	private Button btnApproved;

	private Panel panel6;

	private Panel panel7;

	private Label label5;

	private Panel panel8;

	internal GridEX GridDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label7;

	private RichTextBox txtComments;

	private Label label8;

	private Label label9;

	private Label label3;

	internal UltraCombo cmbEmployeeDepartment;

	private Label label10;

	internal UltraCombo cmbEmployeeCategory;

	private Label label11;

	private GroupBox groupBox1;

	public PayRollPostingForMultiApproval(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllowEditOnVoucherNoinReports"));
			dtViewAttachments.Columns.Add("AttchmentPath");
			EmployeeDepartmentFill();
			EmployeeCategoryFill();
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			gridHisory();
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

	private void reset()
	{
		try
		{
			((Control)(object)cmbEmployeeCategory).Text = string.Empty;
			((Control)(object)cmbEmployeeDepartment).Text = string.Empty;
			fromdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnshow_Click_1(object sender, EventArgs e)
	{
		try
		{
			gridHisory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void gridHisory()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			ReportsParameters order = new ReportsParameters();
			order.OrganizationId = UserAccount.OrganizationId;
			order.CompanyId = UserAccount.CompanyId;
			order.FromDate = fromdate.Value;
			order.ToDate = ToDate.Value;
			order.EmployeeCategoryId = Conversion.ToInt(cmbEmployeeCategory.Value);
			order.DepartmentId = Conversion.ToInt(cmbEmployeeDepartment.Value);
			order.IsApproved = false;
			order.UserId = UserAccount.ID;
			grdlst = Payroll.PayrollPostingForMultiApproval(order);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("PayrollId", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("PayrollMonth", typeof(int));
				dt.Columns.Add("PayrollYear", typeof(int));
				dt.Columns.Add("TotalAmount", typeof(double));
				dt.Columns.Add("RemarksHeader");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("ModifyDate");
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("FinalApprovalUser");
				dt.Columns.Add("IsFinalStatus");
				dt.Columns.Add("IsMandatoryUsers");
				dt.Columns.Add("IsMandatoryStatus");
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				int i;
				for (i = 0; i < grdlst.Rows.Count; i++)
				{
					bool num = dt.AsEnumerable().Any((DataRow row) => row.Field<int>("PayrollId") == Conversion.ToInt(grdlst.Rows[i]["PayrollId"]));
					decimal AmountTotal = default(decimal);
					_ = from r in grdlst.AsEnumerable()
						where r.Field<int>("PayrollId") == Conversion.ToInt(grdlst.Rows[i]["PayrollId"])
						select r;
					AmountTotal = grdlst.AsEnumerable().Sum((DataRow row) => row.Field<decimal>("TotalAmount"));
					if (!num)
					{
						dt.Rows.Add(grdlst.Rows[i]["PayrollId"], grdlst.Rows[i]["DocMovementId"], grdlst.Rows[i]["DocNo"], Convert.ToDateTime(grdlst.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), grdlst.Rows[i]["PayrollMonth"], grdlst.Rows[i]["PayrollYear"], Conversion.ToDouble((object)AmountTotal), grdlst.Rows[i]["RemarksHeader"], Conversion.ToDateTime(grdlst.Rows[i]["EntryDate"]).ToShortDateString(), grdlst.Rows[i]["EntryUserName"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(grdlst.Rows[i]["ModifyDate"])) ? "" : Conversion.ToDateTime(grdlst.Rows[i]["ModifyDate"]).ToShortDateString(), grdlst.Rows[i]["ModifyUserName"], grdlst.Rows[i]["FinalApprovalUser"], grdlst.Rows[i]["IsFinalStatus"], grdlst.Rows[i]["IsMandatoryUsers"], grdlst.Rows[i]["IsMandatoryStatus"], grdlst.Rows[i]["NoOfAttachments"]);
					}
				}
				grdsaleorder.DataSource = dt;
				grdsaleorder.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grdsaleorder.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdsaleorder.RootTable.Columns["PayrollId"].Visible = false;
			grdsaleorder.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdsaleorder.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdsaleorder.RootTable.GroupTotals = (GroupTotals)2;
			CommonServices.GridAutoAdjustment(AryColWidth, grdsaleorder);
			grdsaleorder.RootTable.Columns.Add("Detail");
			grdsaleorder.RootTable.Columns["Detail"].Caption = "Detail";
			grdsaleorder.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdsaleorder.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdsaleorder.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdsaleorder.RootTable.Columns["Detail"].Width = 60;
			grdsaleorder.RootTable.Columns["Detail"].Position = 0;
			grdsaleorder.RootTable.Columns.Add("IsApproved");
			grdsaleorder.RootTable.Columns["IsApproved"].Caption = "IsApproved";
			grdsaleorder.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdsaleorder.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
			grdsaleorder.RootTable.Columns["IsApproved"].ButtonText = "IsApproved";
			grdsaleorder.RootTable.Columns["IsApproved"].Width = 80;
			grdsaleorder.RootTable.Columns["IsApproved"].Position = 1;
			grdsaleorder.RootTable.Columns.Add("Select");
			grdsaleorder.RootTable.Columns["Select"].ActAsSelector = true;
			grdsaleorder.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdsaleorder.RootTable.Columns["Select"].Width = 40;
			grdsaleorder.RootTable.Columns["Select"].Position = 2;
			grdsaleorder.FrozenColumns = 3;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt((object)grdsaleorder.CurrentRow.Cells["PayrollId"].Value.ToString());
			_ = e.Column.Key == "Print";
			if (e.Column.Key == "Detail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grdsaleorder.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = DocumentTypeId;
					obj.RefDocNoId = Id;
					obj.Show();
				}
			}
			if (e.Column.Key == "IsApproved")
			{
				GridColumnApproveButtonClick();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdsaleorder.CurrentRow;
			int Id = int.Parse(item.Cells["PayrollId"].Value.ToString());
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdsaleorder.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = int.Parse(item.Cells["Id"].Value.ToString());
			if (grdsaleorder.CurrentColumn == null)
			{
				return;
			}
			if (grdsaleorder.CurrentColumn.Key == "OrderNo")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id);
				}
			}
			else if (grdsaleorder.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (grdsaleorder.CurrentColumn.Key == "Print")
			{
				if (DocumentTypeId == 81)
				{
					CommonServices.SaleOrderReports273(Id);
				}
				else if (DocumentTypeId == 127)
				{
					CommonServices.BookOrderReports273A(Id);
				}
			}
			else if (grdsaleorder.CurrentColumn.Key == "IsApproved")
			{
				GridColumnApproveButtonClick();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst.Rows.Count == 0 || grdlst == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(grdlst, "1116_PayrollPostingForMultiApprovalReport.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst.Rows.Count == 0 || grdlst == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(grdlst, "1116_PayrollPostingForMultiApprovalReport.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void frmSaleOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdsaleorder).Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				fromdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				print_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.A && btnApproved.Visible)
			{
				btnApproved_Click(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetail).Name);
			if (fs != null)
			{
				GridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdsaleorder).Name);
			if (fs != null)
			{
				grdsaleorder.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GridColumnApproveButtonClick()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				ApprovalList item = new ApprovalList();
				if (txtComments.Text == string.Empty)
				{
					MessageBox.Show("Comments Required For Approval", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					txtComments.Focus();
					return;
				}
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Comments = txtComments.Text.Trim();
				item.DocumentTypeId = Conversion.ToInt((object)DocumentTypeId);
				item.Id = Conversion.ToInt((object)grdsaleorder.CurrentRow.Cells["PayrollId"].Value.ToString());
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				obj.AllApprovalLists.Add(item);
				DocumentApprovalPolicy.MultiApproveAndRejectDocumentByUserId(obj);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		try
		{
			if (grdsaleorder.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			ApprovalList obj = new ApprovalList();
			obj.AllApprovalLists = new List<ApprovalList>();
			if (txtComments.Text == string.Empty)
			{
				MessageBox.Show("Comments Required For Approval", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtComments.Focus();
				return;
			}
			GridEXRow[] checkedRows = grdsaleorder.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Comments = txtComments.Text.Trim();
				item.DocumentTypeId = Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
				item.Id = Conversion.ToInt((object)r.Cells["PayrollId"].Value.ToString());
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				obj.AllApprovalLists.Add(item);
			}
			DocumentApprovalPolicy.MultiApproveAndRejectDocumentByUserId(obj);
			MessageBox.Show("Record Approve Successfully");
			gridHisory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdsaleorder_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdsaleorder.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (item.Cells.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("PayrollId", typeof(int));
					dt.Columns.Add("EmployeeId", typeof(int));
					dt.Columns.Add("EmployeeNo", typeof(int));
					dt.Columns.Add("EmployeeName", typeof(string));
					dt.Columns.Add("EmployeeHistoryId", typeof(int));
					dt.Columns.Add("GrossSalary", typeof(double));
					dt.Columns.Add("EmployeeDays", typeof(int));
					dt.Columns.Add("EmployeePerDaySalary", typeof(double));
					dt.Columns.Add("EmployeePayrollSalary", typeof(double));
					dt.Columns.Add("LateDeductionAmount", typeof(double));
					dt.Columns.Add("IncomeTaxAmount", typeof(double));
					dt.Columns.Add("LeaveAmount", typeof(double));
					dt.Columns.Add("AdvanceAmount", typeof(double));
					dt.Columns.Add("LoanAmount", typeof(double));
					dt.Columns.Add("LoanNOS", typeof(int));
					dt.Columns.Add("PFAmount", typeof(double));
					dt.Columns.Add("EOBIAmount", typeof(double));
					dt.Columns.Add("MiscLessAmount", typeof(double));
					dt.Columns.Add("TotalAmount", typeof(double));
					dt.Columns.Add("OTAmount", typeof(double));
					dt.Columns.Add("TravellingAmount", typeof(double));
					dt.Columns.Add("MedicalAmount", typeof(double));
					dt.Columns.Add("MobileAmount", typeof(double));
					dt.Columns.Add("FuelAmount", typeof(double));
					dt.Columns.Add("FoodAmount", typeof(double));
					dt.Columns.Add("MiscAddAmount", typeof(double));
					dt.Columns.Add("ArrearAmount", typeof(double));
					dt.Columns.Add("SalaryWithoutAddLess", typeof(double));
					dt.Columns.Add("AddLessAmount", typeof(double));
					dt.Columns.Add("NetSalary", typeof(double));
					DataTable resultRows = (from row in grdlst.AsEnumerable()
						where row.Field<long>("PayrollId") == Conversion.ToInt(item.Cells["PayrollId"].Value)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dt.Rows.Add(resultRows.Rows[i]["PayrollId"], resultRows.Rows[i]["EmployeeId"], resultRows.Rows[i]["EmployeeNo"], resultRows.Rows[i]["EmployeeName"], resultRows.Rows[i]["EmployeeHistoryId"], resultRows.Rows[i]["GrossSalary"], resultRows.Rows[i]["EmployeeDays"], resultRows.Rows[i]["EmployeePerDaySalary"], resultRows.Rows[i]["EmployeePayrollSalary"], resultRows.Rows[i]["LateDeductionAmount"], resultRows.Rows[i]["IncomeTaxAmount"], resultRows.Rows[i]["LeaveAmount"], resultRows.Rows[i]["AdvanceAmount"], resultRows.Rows[i]["LoanAmount"], resultRows.Rows[i]["LoanNOS"], resultRows.Rows[i]["PFAmount"], resultRows.Rows[i]["EOBIAmount"], resultRows.Rows[i]["MiscLessAmount"], resultRows.Rows[i]["TotalAmount"], resultRows.Rows[i]["OTAmount"], resultRows.Rows[i]["TravellingAmount"], resultRows.Rows[i]["MedicalAmount"], resultRows.Rows[i]["MobileAmount"], resultRows.Rows[i]["FuelAmount"], resultRows.Rows[i]["FoodAmount"], resultRows.Rows[i]["MiscAddAmount"], resultRows.Rows[i]["ArrearAmount"], resultRows.Rows[i]["SalaryWithoutAddLess"], resultRows.Rows[i]["AddLessAmount"], resultRows.Rows[i]["NetSalary"]);
					}
					GridDetail.DataSource = dt;
					GridDetail.RetrieveStructure();
					GridDetailSetting();
				}
			}
			else
			{
				GridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			GridDetail.RootTable.Columns["PayrollId"].Visible = false;
			GridDetail.RootTable.Columns["EmployeeHistoryId"].Visible = false;
			GridDetail.RootTable.Columns["EmployeeId"].Visible = false;
			GridDetail.RootTable.Columns["GrossSalary"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["GrossSalary"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["GrossSalary"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["GrossSalary"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["EmployeeDays"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["EmployeeDays"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["EmployeeDays"].FormatString = "##0.###";
			GridDetail.RootTable.Columns["EmployeeDays"].TotalFormatString = "##0.###";
			GridDetail.RootTable.Columns["EmployeePerDaySalary"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["EmployeePerDaySalary"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["EmployeePerDaySalary"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["EmployeePerDaySalary"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["EmployeePayrollSalary"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["EmployeePayrollSalary"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["EmployeePayrollSalary"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["EmployeePayrollSalary"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["LateDeductionAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["LateDeductionAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["LateDeductionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["LateDeductionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["IncomeTaxAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["IncomeTaxAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["IncomeTaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["IncomeTaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["LeaveAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["LeaveAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["LeaveAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["LeaveAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["AdvanceAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["AdvanceAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["AdvanceAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["AdvanceAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["LoanAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["LoanAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["LoanAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["LoanAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["LoanNOS"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["LoanNOS"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["LoanNOS"].FormatString = "##0.###";
			GridDetail.RootTable.Columns["LoanNOS"].TotalFormatString = "##0.###";
			GridDetail.RootTable.Columns["PFAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["PFAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["PFAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["PFAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["EOBIAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["EOBIAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["EOBIAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["EOBIAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["MiscLessAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["MiscLessAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["MiscLessAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["MiscLessAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["OTAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["OTAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["OTAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["OTAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["TravellingAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["TravellingAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["TravellingAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["TravellingAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["MedicalAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["MedicalAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["MedicalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["MedicalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["MobileAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["MobileAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["MobileAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["MobileAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["FuelAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["FuelAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["FuelAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["FuelAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["FoodAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["FoodAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["FoodAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["FoodAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["MiscAddAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["MiscAddAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["MiscAddAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["MiscAddAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ArrearAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ArrearAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ArrearAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ArrearAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["NetSalary"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["NetSalary"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["NetSalary"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["NetSalary"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["AddLessAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["AddLessAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["AddLessAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["AddLessAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["SalaryWithoutAddLess"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["SalaryWithoutAddLess"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["SalaryWithoutAddLess"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["SalaryWithoutAddLess"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_0b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b19: Expected O, but got Unknown
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Expected O, but got Unknown
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdsaleorder_DesignTimeLayout = new GridEXLayout();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.MultiApprovalDashboard.PayRollPostingForMultiApproval));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.GridDetail = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdsaleorder = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.btnApproved = new System.Windows.Forms.Button();
		this.txtComments = new System.Windows.Forms.RichTextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbEmployeeDepartment = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.cmbEmployeeCategory = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).BeginInit();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCategory).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 749);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 181);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(964, 568);
		this.panel5.TabIndex = 3;
		this.panel8.Controls.Add(this.label9);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Controls.Add(this.label7);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 310);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(964, 258);
		this.panel8.TabIndex = 3;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(4, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(355, 19);
		this.label9.TabIndex = 27;
		this.label9.Text = "Detail Information Of Above Selected Row";
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		this.GridDetail.AlternatingColors = true;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxFormatStyle.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.GridDetail.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(10, 110, 110);
		this.GridDetail.GroupByBoxInfoFormatStyle.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.GridDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(964, 232);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 26;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		this.GridDetail.TreeLineColor = System.Drawing.Color.FromArgb(10, 110, 110);
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(923, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 25;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(964, 26);
		this.label7.TabIndex = 3;
		this.panel6.Controls.Add(this.label3);
		this.panel6.Controls.Add(this.ctrlGrdBar3);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(964, 310);
		this.panel6.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(8, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(155, 19);
		this.label3.TabIndex = 25;
		this.label3.Text = "Main Information ";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(923, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdsaleorder;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.grdsaleorder.AllowEdit = (InheritableBoolean)2;
		this.grdsaleorder.AlternatingColors = true;
		((ScrollableBase)this.grdsaleorder).BackColor = System.Drawing.Color.White;
		grdsaleorder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsaleorder.DesignTimeLayout = grdsaleorder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdsaleorder.FilterMode = (FilterMode)2;
		this.grdsaleorder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsaleorder.GroupByBoxFormatStyle.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.grdsaleorder.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(10, 110, 110);
		this.grdsaleorder.GroupByBoxInfoFormatStyle.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.grdsaleorder.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Name = "grdsaleorder";
		((ScrollableBase)this.grdsaleorder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Size = new System.Drawing.Size(964, 285);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Tag = "MasterGrid";
		this.grdsaleorder.TotalRow = (InheritableBoolean)1;
		this.grdsaleorder.TotalRowPosition = (TotalRowPosition)1;
		this.grdsaleorder.TreeLineColor = System.Drawing.Color.FromArgb(10, 110, 110);
		((ScrollableBase)this.grdsaleorder).VisualStyle = (VisualStyle)4;
		this.grdsaleorder.ColumnButtonClick += new ColumnActionEventHandler(grdsaleorder_ColumnButtonClick);
		this.grdsaleorder.SelectionChanged += new System.EventHandler(grdsaleorder_SelectionChanged);
		this.grdsaleorder.LinkClicked += new ColumnActionEventHandler(grdsaleorder_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).KeyDown += new System.Windows.Forms.KeyEventHandler(grdsaleorder_KeyDown);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdsaleorder);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 25);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(964, 285);
		this.panel7.TabIndex = 3;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Location = new System.Drawing.Point(0, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(964, 25);
		this.label5.TabIndex = 2;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 58);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 123);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.btnApproved);
		this.groupBox1.Controls.Add(this.txtComments);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.ToDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(946, 114);
		this.groupBox1.TabIndex = 523;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(103, 19);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(121, 23);
		this.fromdate.TabIndex = 0;
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(808, 19);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(100, 40);
		this.btnApproved.TabIndex = 6;
		this.btnApproved.Text = "Approve All Selected";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Visible = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.txtComments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtComments.Location = new System.Drawing.Point(357, 48);
		this.txtComments.Name = "txtComments";
		this.txtComments.Size = new System.Drawing.Size(446, 56);
		this.txtComments.TabIndex = 4;
		this.txtComments.Text = "";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(808, 78);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click_1);
		this.label8.BackColor = System.Drawing.Color.LightGray;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(357, 19);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(446, 23);
		this.label8.TabIndex = 208;
		this.label8.Text = "Comments";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbEmployeeDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbEmployeeDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).Location = new System.Drawing.Point(103, 78);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).Name = "cmbEmployeeDepartment";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeDepartment).TabIndex = 520;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(5, 23);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(91, 15);
		this.label33.TabIndex = 127;
		this.label33.Text = "From && To Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 78);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(98, 15);
		this.label10.TabIndex = 522;
		this.label10.Text = "Emp Department";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(230, 19);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(121, 23);
		this.ToDate.TabIndex = 1;
		this.cmbEmployeeCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbEmployeeCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Location = new System.Drawing.Point(103, 48);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Name = "cmbEmployeeCategory";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCategory).TabIndex = 519;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 54);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(81, 15);
		this.label11.TabIndex = 521;
		this.label11.Text = "Emp Category";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 28);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(292, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "PayRoll Posting For Multi Approval";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.print });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(964, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(123, 24);
		this.print.Text = "1116-Register";
		this.print.ToolTipText = "1116-Register";
		this.print.Click += new System.EventHandler(print_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PayRollPostingForMultiApproval";
		this.Text = "frmSaleOrderHistory";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmSaleOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleOrderHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCategory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
