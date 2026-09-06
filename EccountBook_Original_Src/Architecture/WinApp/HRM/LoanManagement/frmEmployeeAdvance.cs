using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.LoanManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.LoanManagement;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.EmployeeManagement;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.LoanManagement;

public class frmEmployeeAdvance : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private int Success;

	private DataTable dtemployee = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private Label label6;

	private Panel panel1;

	private Panel panel4;

	internal GridEX grd;

	private Panel panel3;

	internal UltraCombo cmbEmployeeName;

	private Label label1;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private GroupBox groupBox1;

	private TextBox txtSection;

	private Label label7;

	private TextBox txtDepartment;

	private Label label5;

	private TextBox txtDesgination;

	private Label label4;

	private TextBox txtEmployeeNo;

	private Label label3;

	private DateTimePicker datAppliedOn;

	private TextBox txtAmount;

	private Label label2;

	private TextBox txtSalary;

	private Label label8;

	private Label label9;

	private TextBox txtCurrentAmount;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private Label label16;

	private GroupBox groupBox2;

	private Panel panel7;

	private Panel panel6;

	private Label label10;

	private ToolStripButton btnRefresh;

	private Button btnEmployeeRegister;

	private Label label17;

	private Label label11;

	private TextBox txtReason;

	private TextBox txtPreivousApprovedAmount;

	private TextBox txtPreviousAdvance;

	private TextBox txtDocNo;

	private Label label12;

	private Label label13;

	private DateTimePicker datRequestDate;

	public frmEmployeeAdvance(UserAccount pUserAccount)
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
			base.AutoScrollMinSize = new Size(980, 500);
			formright = CommonServices.SetRightsValueInRightsObject("EmployeeAdvance");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			GenerateCode();
			EmployeeName();
			GridBind();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			cmbEmployeeName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			genEmployeeHistory obj = new genEmployeeHistory();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtemployee = genEmployeeHistory.GetAllActiveEmployee(obj);
			if (dtemployee.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtemployee, cmbEmployeeName, "EmployeeId", "EmployeeName", "Employee Name", false);
			}
			else
			{
				((UltraGridBase)cmbEmployeeName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbEmployeeName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbEmployeeName).ActiveRow != null)
			{
				if (dtemployee.Rows.Count > 0)
				{
					for (int i = 0; i < dtemployee.Rows.Count; i++)
					{
						if (Conversion.ToInt(cmbEmployeeName.Value) == Conversion.ToInt(dtemployee.Rows[i]["EmployeeId"]))
						{
							txtEmployeeNo.Text = Conversion.ToString(dtemployee.Rows[i]["EmployeeNo"]);
							txtDesgination.Text = Conversion.ToString(dtemployee.Rows[i]["DesignationName"]);
							txtDepartment.Text = Conversion.ToString(dtemployee.Rows[i]["DepartmentName"]);
							txtSection.Text = Conversion.ToString(dtemployee.Rows[i]["SectionName"]);
							txtSalary.Text = Conversion.ToString(dtemployee.Rows[i]["TotalSalary"]);
						}
					}
				}
				else
				{
					txtEmployeeNo.Text = string.Empty;
					txtDesgination.Text = string.Empty;
					txtDepartment.Text = string.Empty;
					txtSalary.Text = string.Empty;
					txtSection.Text = string.Empty;
				}
			}
			else
			{
				txtEmployeeNo.Text = string.Empty;
				txtDesgination.Text = string.Empty;
				txtDepartment.Text = string.Empty;
				txtSalary.Text = string.Empty;
				txtSection.Text = string.Empty;
			}
			SalaryAmountCalculation();
			PreviousAdvanceAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			txtDocNo.Text = Conversion.ToString((object)EmployeeAdvance.GenerateCode(Obj));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			if (e.Control && e.KeyCode == Keys.Return && Conversion.ToString(base.ActiveControl.Tag) == "grd")
			{
				grdEmployeeFamilyInfo_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		if (Conversion.ToDouble((object)txtAmount.Text) == 0.0)
		{
			MessageBox.Show("Advance Amount Field Required!");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		try
		{
			RecId = 0;
			((Control)(object)cmbEmployeeName).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtCurrentAmount.Text = string.Empty;
			txtReason.Text = string.Empty;
			txtSalary.Text = string.Empty;
			txtEmployeeNo.Text = string.Empty;
			txtDepartment.Text = string.Empty;
			txtDesgination.Text = string.Empty;
			txtSection.Text = string.Empty;
			cmbEmployeeName.Focus();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			UpdateMode = false;
			GridBind();
			GenerateCode();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			EmployeeName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmEmployeeAdvance_FormClosing(object sender, FormClosingEventArgs e)
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

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = EmployeeAdvance.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtEmp = new DataTable();
				dtEmp.Columns.Add("RequestDate", typeof(DateTime));
				dtEmp.Columns.Add("RequestNo", typeof(int));
				dtEmp.Columns.Add("AdvanceId");
				dtEmp.Columns.Add("EmployeeName");
				dtEmp.Columns.Add("Department");
				dtEmp.Columns.Add("Designation");
				dtEmp.Columns.Add("AppliedDate", typeof(DateTime));
				dtEmp.Columns.Add("AdvanceAmount", typeof(double));
				dtEmp.Columns.Add("ApprovedAmount", typeof(double));
				dtEmp.Columns.Add("Reason");
				dtEmp.Columns.Add("EntryUser");
				dtEmp.Columns.Add("EntryDate", typeof(DateTime));
				dtEmp.Columns.Add("ModifyUser");
				dtEmp.Columns.Add("ApprovedUser");
				dtEmp.Columns.Add("IsApproved", typeof(bool));
				dtEmp.Columns.Add("ApprovedDate", typeof(DateTime));
				dtEmp.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtEmp.Rows.Add(Conversion.ToDateTime(dt.Rows[i]["RequestDate"]).ToShortDateString(), dt.Rows[i]["RequestNo"], dt.Rows[i]["EmployeeAdvanceId"], dt.Rows[i]["EmployeeName"], dt.Rows[i]["DepartmentName"], dt.Rows[i]["DesignationName"], Conversion.ToDateTime(dt.Rows[i]["AppliedOn"]).ToShortDateString(), dt.Rows[i]["AdvanceAmount"], dt.Rows[i]["ApprovedAmount"], dt.Rows[i]["Reason"], dt.Rows[i]["CreatedBy"], Conversion.ToDateTime(dt.Rows[i]["CreatedOn"]).ToShortDateString(), dt.Rows[i]["AlteredBy"], dt.Rows[i]["ApprovedBy"], Conversion.ToBool(dt.Rows[i]["IsApproved"]), Conversion.ToDateTime(dt.Rows[i]["ApprovedOn"]).ToShortDateString());
				}
				grd.DataSource = dtEmp;
				grd.RetrieveStructure();
				grdSetting();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdEmployeeFamilyInfo_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RetrivedData(Conversion.ToInt((object)grd.CurrentRow.Cells["AdvanceId"].Value.ToString()));
			UpdateMode = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdSetting()
	{
		try
		{
			grd.RootTable.Columns["EmployeeName"].Width = 250;
			grd.RootTable.Columns["AdvanceId"].Visible = false;
			grd.RootTable.Columns["AdvanceAmount"].FormatString = "#,#";
			grd.RootTable.Columns["ApprovedAmount"].FormatString = "#,#";
			grd.RootTable.Columns["AdvanceAmount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["ApprovedAmount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["AdvanceAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ApprovedAmount"].AggregateFunction = (AggregateFunction)2;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].Caption = "Print";
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].Width = 50;
			grd.RootTable.Columns["Print"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				GenerateSlip(Conversion.ToInt(grd.CurrentRow.Cells["AdvanceId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void RetrivedData(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			EmployeeAdvance obj = new EmployeeAdvance();
			RecId = Id;
			obj = EmployeeAdvance.GetByID(RecId);
			if (obj != null)
			{
				txtDocNo.Text = Conversion.ToString((object)obj.DocNo);
				datRequestDate.Value = Conversion.ToDateTime((object)obj.DocDate);
				datAppliedOn.Value = Conversion.ToDateTime((object)obj.AppliedOn);
				cmbEmployeeName.Value = Conversion.ToInt((object)obj.EmployeeId);
				txtReason.Text = Conversion.ToString((object)obj.Reason);
				txtAmount.Text = Conversion.ToString((object)obj.AdvanceAmount);
				txtPreviousAdvance.Text = Conversion.ToString((object)obj.PreviousAdvanceAmount);
				txtPreivousApprovedAmount.Text = Conversion.ToString((object)obj.PreviousApprovedAmount);
				cmbEmployeeName_Leave(null, null);
				btnupdate.Visible = true;
				btnsave.Visible = false;
				cmbEmployeeName.Focus();
			}
			else
			{
				MessageBox.Show("Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (Validation())
			{
				EmployeeAdvance obj = new EmployeeAdvance();
				if (RecId > 0)
				{
					obj.EmployeeAdvanceId = RecId;
				}
				obj.EmployeeId = Conversion.ToInt(cmbEmployeeName.Value);
				obj.DocNo = Conversion.ToInt((object)txtDocNo.Text);
				obj.DocDate = Conversion.ToDateTime((object)datRequestDate.Value);
				obj.AdvanceAmount = Conversion.ToDecimal((object)txtAmount.Text.Trim());
				obj.PreviousAdvanceAmount = Conversion.ToDecimal((object)txtPreviousAdvance.Text.Trim());
				obj.PreviousApprovedAmount = Conversion.ToDecimal((object)txtPreivousApprovedAmount.Text.Trim());
				obj.AppliedOn = datAppliedOn.Value;
				obj.Reason = Conversion.ToString((object)txtReason.Text.Trim());
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.BranchId = Conversion.ToInt((object)UserAccount.BranchesId);
				obj.CreatedById = UserAccount.ID;
				obj.CreatedOn = DateTime.Now;
				obj.AlteredById = UserAccount.ID;
				obj.AlteredOn = DateTime.Now;
				obj.ApprovedOn = DateTime.Now;
				obj.ApprovedById = UserAccount.ID;
				obj.PayrollMonth = obj.AppliedOn.Month;
				obj.PayrollYear = obj.AppliedOn.Year;
				Success = EmployeeAdvance.Save(obj);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PreviousAdvanceAmount()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		try
		{
			DataTable dt = EmployeeAdvance.GetPreviousAdvanceAmount(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EmployeeId = Conversion.ToInt(cmbEmployeeName.Value),
				MonthValue = Conversion.ToInt((object)datAppliedOn.Value.Month),
				GetYear = Conversion.ToInt((object)datAppliedOn.Value.Year)
			});
			if (dt.Rows.Count > 0)
			{
				double AdvanceAmount = Conversion.ToDouble(dt.Rows[0]["AdvanceAmount"]);
				double ApprovedAmount = Conversion.ToDouble(dt.Rows[0]["ApprovedAmount"]);
				txtPreviousAdvance.Text = AdvanceAmount.ToString();
				txtPreivousApprovedAmount.Text = ApprovedAmount.ToString();
			}
			else
			{
				txtPreviousAdvance.Text = "0";
				txtPreivousApprovedAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SalaryAmountCalculation()
	{
		try
		{
			int days = datAppliedOn.Value.Day;
			if (days > 0)
			{
				double SalaryAmount = Math.Round(Conversion.ToDouble((object)txtSalary.Text.Trim()) / 30.0 * (double)days, 2);
				txtCurrentAmount.Text = SalaryAmount.ToString();
			}
			else
			{
				txtCurrentAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void datAppliedOn_Leave(object sender, EventArgs e)
	{
		try
		{
			SalaryAmountCalculation();
			PreviousAdvanceAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void datAppliedOn_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			PreviousAdvanceAmount();
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtCurrentAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnEmployeeRegister_Click(object sender, EventArgs e)
	{
		try
		{
			frmEmployeeRegistration obj = new frmEmployeeRegistration(UserAccount);
			obj.formright = CommonServices.SetRightsValueInRightsObject("EmployeeRegistration");
			if (obj.formright.DoHaveViewRight)
			{
				obj.Show();
			}
			else
			{
				MessageBox.Show("You don't have the right to view this form...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip(int PrintId)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = EmployeeAdvance.EmployeeAdvanceSlip(PrintId);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1008-EmployeeAdvanceSlip.rpt");
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
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.LoanManagement.frmEmployeeAdvance));
		this.label6 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label10 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnEmployeeRegister = new System.Windows.Forms.Button();
		this.txtCurrentAmount = new System.Windows.Forms.TextBox();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbEmployeeName = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtReason = new System.Windows.Forms.TextBox();
		this.txtPreivousApprovedAmount = new System.Windows.Forms.TextBox();
		this.txtPreviousAdvance = new System.Windows.Forms.TextBox();
		this.datAppliedOn = new System.Windows.Forms.DateTimePicker();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtSalary = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtSection = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDepartment = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtDesgination = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtEmployeeNo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.datRequestDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(4, 132);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(45, 15);
		this.label6.TabIndex = 29;
		this.label6.Text = "Reason";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1133, 604);
		this.panel1.TabIndex = 2;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 225);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1133, 379);
		this.panel4.TabIndex = 2;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 33);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1133, 346);
		this.panel7.TabIndex = 2;
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1133, 346);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).Tag = "grd";
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grdEmployeeFamilyInfo_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label10);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1133, 33);
		this.panel6.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1084, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(6, 6);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 19);
		this.label10.TabIndex = 27;
		this.label10.Text = "History";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1133, 197);
		this.panel3.TabIndex = 1;
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.datRequestDate);
		this.groupBox2.Controls.Add(this.btnEmployeeRegister);
		this.groupBox2.Controls.Add(this.txtCurrentAmount);
		this.groupBox2.Controls.Add(this.txtAmount);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeName);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtDocNo);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.txtReason);
		this.groupBox2.Controls.Add(this.txtPreivousApprovedAmount);
		this.groupBox2.Controls.Add(this.txtPreviousAdvance);
		this.groupBox2.Controls.Add(this.datAppliedOn);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 39);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(605, 156);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Advance Information";
		this.btnEmployeeRegister.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeRegister.Location = new System.Drawing.Point(255, 77);
		this.btnEmployeeRegister.Name = "btnEmployeeRegister";
		this.btnEmployeeRegister.Size = new System.Drawing.Size(27, 23);
		this.btnEmployeeRegister.TabIndex = 1;
		this.btnEmployeeRegister.TabStop = false;
		this.btnEmployeeRegister.UseVisualStyleBackColor = true;
		this.btnEmployeeRegister.Click += new System.EventHandler(btnEmployeeRegister_Click);
		this.txtCurrentAmount.BackColor = System.Drawing.SystemColors.Window;
		this.txtCurrentAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCurrentAmount.Location = new System.Drawing.Point(443, 101);
		this.txtCurrentAmount.Name = "txtCurrentAmount";
		this.txtCurrentAmount.ReadOnly = true;
		this.txtCurrentAmount.Size = new System.Drawing.Size(157, 23);
		this.txtCurrentAmount.TabIndex = 8;
		this.txtCurrentAmount.TabStop = false;
		this.txtCurrentAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCurrentAmount_KeyPress);
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(443, 23);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(157, 23);
		this.txtAmount.TabIndex = 5;
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 81);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(59, 15);
		this.label1.TabIndex = 19;
		this.label1.Text = "Employee";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(288, 27);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(100, 15);
		this.label2.TabIndex = 29;
		this.label2.Text = "Advance Amount";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(288, 105);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(147, 15);
		this.label15.TabIndex = 29;
		this.label15.Text = "Salary Amt Upto Apply On";
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
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Location = new System.Drawing.Point(86, 75);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Name = "cmbEmployeeName";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Leave += new System.EventHandler(cmbEmployeeName_Leave);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(288, 79);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(126, 15);
		this.label17.TabIndex = 29;
		this.label17.Text = "Pre Approved Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(288, 52);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(101, 15);
		this.label11.TabIndex = 29;
		this.label11.Text = "Previous Advance";
		this.txtDocNo.BackColor = System.Drawing.Color.White;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(86, 22);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(196, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TabStop = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 106);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(67, 15);
		this.label12.TabIndex = 29;
		this.label12.Text = "Applied On";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(4, 25);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(68, 15);
		this.label9.TabIndex = 29;
		this.label9.Text = "Request No";
		this.txtReason.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReason.Location = new System.Drawing.Point(86, 128);
		this.txtReason.Name = "txtReason";
		this.txtReason.Size = new System.Drawing.Size(514, 23);
		this.txtReason.TabIndex = 4;
		this.txtPreivousApprovedAmount.Enabled = false;
		this.txtPreivousApprovedAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPreivousApprovedAmount.Location = new System.Drawing.Point(443, 75);
		this.txtPreivousApprovedAmount.Name = "txtPreivousApprovedAmount";
		this.txtPreivousApprovedAmount.Size = new System.Drawing.Size(157, 23);
		this.txtPreivousApprovedAmount.TabIndex = 7;
		this.txtPreivousApprovedAmount.TabStop = false;
		this.txtPreivousApprovedAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.txtPreviousAdvance.Enabled = false;
		this.txtPreviousAdvance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPreviousAdvance.Location = new System.Drawing.Point(443, 48);
		this.txtPreviousAdvance.Name = "txtPreviousAdvance";
		this.txtPreviousAdvance.Size = new System.Drawing.Size(157, 23);
		this.txtPreviousAdvance.TabIndex = 6;
		this.txtPreviousAdvance.TabStop = false;
		this.txtPreviousAdvance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.datAppliedOn.CustomFormat = "dd-MMM-yyyy";
		this.datAppliedOn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datAppliedOn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datAppliedOn.Location = new System.Drawing.Point(86, 102);
		this.datAppliedOn.Name = "datAppliedOn";
		this.datAppliedOn.Size = new System.Drawing.Size(196, 23);
		this.datAppliedOn.TabIndex = 3;
		this.datAppliedOn.ValueChanged += new System.EventHandler(datAppliedOn_ValueChanged);
		this.datAppliedOn.Leave += new System.EventHandler(datAppliedOn_Leave);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label16);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1133, 33);
		this.panel5.TabIndex = 104;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(8, 6);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(162, 19);
		this.label16.TabIndex = 26;
		this.label16.Text = "Employee Advance";
		this.groupBox1.Controls.Add(this.txtSalary);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtSection);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDepartment);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtDesgination);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtEmployeeNo);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(610, 39);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(351, 156);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Employee Information";
		this.txtSalary.BackColor = System.Drawing.Color.White;
		this.txtSalary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSalary.Location = new System.Drawing.Point(261, 20);
		this.txtSalary.Name = "txtSalary";
		this.txtSalary.ReadOnly = true;
		this.txtSalary.Size = new System.Drawing.Size(86, 23);
		this.txtSalary.TabIndex = 1;
		this.txtSalary.TabStop = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(166, 24);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(94, 15);
		this.label8.TabIndex = 29;
		this.label8.Text = "Employee Salary";
		this.txtSection.BackColor = System.Drawing.Color.White;
		this.txtSection.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSection.Location = new System.Drawing.Point(83, 96);
		this.txtSection.Name = "txtSection";
		this.txtSection.ReadOnly = true;
		this.txtSection.Size = new System.Drawing.Size(264, 23);
		this.txtSection.TabIndex = 4;
		this.txtSection.TabStop = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 100);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(47, 15);
		this.label7.TabIndex = 29;
		this.label7.Text = "Section";
		this.txtDepartment.BackColor = System.Drawing.Color.White;
		this.txtDepartment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDepartment.Location = new System.Drawing.Point(83, 45);
		this.txtDepartment.Name = "txtDepartment";
		this.txtDepartment.ReadOnly = true;
		this.txtDepartment.Size = new System.Drawing.Size(264, 23);
		this.txtDepartment.TabIndex = 2;
		this.txtDepartment.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 49);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 15);
		this.label5.TabIndex = 29;
		this.label5.Text = "Department";
		this.txtDesgination.BackColor = System.Drawing.Color.White;
		this.txtDesgination.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDesgination.Location = new System.Drawing.Point(83, 71);
		this.txtDesgination.Name = "txtDesgination";
		this.txtDesgination.ReadOnly = true;
		this.txtDesgination.Size = new System.Drawing.Size(264, 23);
		this.txtDesgination.TabIndex = 3;
		this.txtDesgination.TabStop = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 75);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 29;
		this.label4.Text = "Desgination";
		this.txtEmployeeNo.BackColor = System.Drawing.Color.White;
		this.txtEmployeeNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmployeeNo.Location = new System.Drawing.Point(83, 20);
		this.txtEmployeeNo.Name = "txtEmployeeNo";
		this.txtEmployeeNo.ReadOnly = true;
		this.txtEmployeeNo.Size = new System.Drawing.Size(80, 23);
		this.txtEmployeeNo.TabIndex = 0;
		this.txtEmployeeNo.TabStop = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 24);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 29;
		this.label3.Text = "Employee No";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1133, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1133, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1008-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.datRequestDate.CustomFormat = "dd-MMM-yyyy";
		this.datRequestDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datRequestDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datRequestDate.Location = new System.Drawing.Point(86, 49);
		this.datRequestDate.Name = "datRequestDate";
		this.datRequestDate.Size = new System.Drawing.Size(196, 23);
		this.datRequestDate.TabIndex = 1;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 51);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(77, 15);
		this.label13.TabIndex = 31;
		this.label13.Text = "Request Date";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1133, 604);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmEmployeeAdvance";
		this.Text = "Employee Advance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEmployeeAdvance_FormClosing);
		base.Load += new System.EventHandler(EmployeeFamilyInfo_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EmployeeFamilyInfo_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
