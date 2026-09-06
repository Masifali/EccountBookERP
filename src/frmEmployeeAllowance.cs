using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PayrollManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PayrollManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.EmployeeManagement;
using Architecture.WinApp.HRM.ProfileManagement;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
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

public class frmEmployeeAllowance : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private List<EmployeeAllowance> lstDelete = new List<EmployeeAllowance>();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dthead = new DataTable();

	public bool _AutoAdjust;

	private int[] AryColWidth;

	private long RecId;

	private int updateDetailIndex;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Label label16;

	private TextBox txtTotalAmount;

	private Label label10;

	internal UltraCombo cmbEmployee;

	private TextBox txtRemarks;

	private Label label34;

	private ToolStripButton btnRefresh;

	internal GridEX grdDetails;

	private Label label4;

	private DateTimePicker txtAppliedOn;

	private CheckBox IsForPayroll;

	internal UltraCombo cmbEmployeeBenefit;

	private Label label1;

	private Panel panel7;

	private Panel panel6;

	private Panel panel5;

	private Label label2;

	private Button btnEmployee;

	private Button btnBenifits;

	private Label label3;

	public frmEmployeeAllowance(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FrmExportSalesContract_Load(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(800, 500);
			formright = CommonServices.SetRightsValueInRightsObject("EmployeeAllowance");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			EmployeeFill();
			EmployeeBenefitFill();
			dthead.Columns.Add("Id");
			dthead.Columns.Add("EmployeeId");
			dthead.Columns.Add("Employee");
			dthead.Columns.Add("EmployeeBenefitId");
			dthead.Columns.Add("EmployeeBenefit");
			dthead.Columns.Add("AppliedOn");
			dthead.Columns.Add("TotalAmount", typeof(double));
			dthead.Columns.Add("IsForPayroll", typeof(bool));
			dthead.Columns.Add("Remarks");
			grdDetails.DataSource = dthead;
			grdDetails.RetrieveStructure();
			grdSettings();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			cmbEmployee.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		Reset();
	}

	private void grdDetails_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetails.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbEmployee.Value = item.Cells["EmployeeId"].Value;
				cmbEmployeeBenefit.Value = item.Cells["EmployeeBenefitId"].Value;
				txtAppliedOn.Value = Conversion.ToDateTime(item.Cells["AppliedOn"].Value);
				txtTotalAmount.Text = Conversion.ToDouble((object)item.Cells["TotalAmount"].Text).ToString();
				IsForPayroll.Checked = Conversion.ToBool(item.Cells["IsForPayroll"].Value);
				txtRemarks.Text = item.Cells["Remarks"].Text.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
				{
					cmbEmployee.ReadOnly = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddtoGrid()
	{
		try
		{
			GridEXRow[] rows = grdDetails.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["EmployeeId"].Value) != Conversion.ToInt(cmbEmployee.Value))
				{
					MessageBox.Show("You Cannot Insert Another Employee Record");
					return;
				}
				if (Conversion.ToInt(r.Cells["EmployeeBenefitId"].Value) == Conversion.ToInt(cmbEmployeeBenefit.Value))
				{
					MessageBox.Show("You Cannot Insert Duplicate Benefit Of an Employee");
					return;
				}
			}
			if (FormValidation())
			{
				dthead.Rows.Add(0, cmbEmployee.Value, ((Control)(object)cmbEmployee).Text, cmbEmployeeBenefit.Value, ((Control)(object)cmbEmployeeBenefit).Text, Conversion.ToDateTime((object)txtAppliedOn.Value).ToString("dd-MMM-yyyy"), Conversion.ToDouble((object)txtTotalAmount.Text), IsForPayroll.Checked, txtRemarks.Text);
				grdDetails.DataSource = dthead;
				grdDetails.RetrieveStructure();
				grdSettings();
				Reset();
				cmbEmployee.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			AddtoGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidation())
			{
				dthead.Rows[updateDetailIndex]["EmployeeId"] = cmbEmployee.Value;
				dthead.Rows[updateDetailIndex]["Employee"] = ((Control)(object)cmbEmployee).Text;
				dthead.Rows[updateDetailIndex]["EmployeeBenefitId"] = cmbEmployeeBenefit.Value;
				dthead.Rows[updateDetailIndex]["EmployeeBenefit"] = ((Control)(object)cmbEmployeeBenefit).Text;
				dthead.Rows[updateDetailIndex]["AppliedOn"] = Conversion.ToDateTime((object)txtAppliedOn.Value).ToString("dd-MMM-yyyy");
				dthead.Rows[updateDetailIndex]["TotalAmount"] = txtTotalAmount.Text;
				dthead.Rows[updateDetailIndex]["IsForPayroll"] = IsForPayroll.Checked;
				dthead.Rows[updateDetailIndex]["Remarks"] = txtRemarks.Text;
				grdDetails.DataSource = dthead;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				Reset();
				cmbEmployee.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				GridEXRow item = grdDetails.CurrentRow;
				EmployeeAllowance obj = new EmployeeAllowance();
				obj.ActionTypeId = 3;
				obj.AlteredById = UserAccount.ID;
				obj.AlteredOn = DateTime.Now;
				obj.UserLogId = UserAccount.ID;
				obj.ApprovedById = UserAccount.ID;
				obj.ApprovedOn = DateTime.Now;
				obj.CompanyId = UserAccount.CompanyId;
				obj.CreatedById = UserAccount.ID;
				obj.CreatedOn = DateTime.Now;
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.AppliedOn = Conversion.ToDateTime(item.Cells["AppliedOn"].Value);
				obj.EmployeeAllowanceId = Conversion.ToInt(item.Cells["Id"].Value);
				obj.EmployeeBenefitId = Conversion.ToInt(item.Cells["EmployeeBenefitId"].Value);
				obj.EmployeeId = Conversion.ToInt(item.Cells["EmployeeId"].Value);
				obj.IsForPayroll = Conversion.ToBool(item.Cells["IsForPayroll"].Value);
				obj.TotalAmount = Conversion.ToDecimal(item.Cells["TotalAmount"].Value);
				obj.Remarks = Conversion.ToString((object)item.Cells["Remarks"].Text);
				lstDelete.Add(obj);
				dthead.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			grdDetails.UpdateData();
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
			grdDetails.RootTable.Columns["Id"].Visible = false;
			grdDetails.RootTable.Columns["EmployeeId"].Visible = false;
			grdDetails.RootTable.Columns["EmployeeBenefitId"].Visible = false;
			grdDetails.RootTable.Columns["IsForPayroll"].EditType = (EditType)8;
			grdDetails.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["TotalAmount"].FormatString = "0,0";
			grdDetails.RootTable.Columns["TotalAmount"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns.Add("Delete");
			grdDetails.RootTable.Columns["Delete"].Key = "Delete";
			grdDetails.RootTable.Columns["Delete"].Caption = "X";
			grdDetails.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetails.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetails.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetails.RootTable.Columns["Delete"].Position = 0;
			grdDetails.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, grdDetails);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbEmployee).ActiveRow == null)
		{
			MessageBox.Show("Employee Required");
			cmbEmployee.Focus();
			return false;
		}
		if (((UltraGridBase)cmbEmployeeBenefit).ActiveRow == null)
		{
			MessageBox.Show("Employee Benefit Required");
			cmbEmployeeBenefit.Focus();
			return false;
		}
		if (txtTotalAmount.Text == string.Empty)
		{
			MessageBox.Show("Total Amount Required");
			txtTotalAmount.Focus();
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
			DataTable dtEmployeeName = genEmployeeHistory.GetAllEmployeesActive(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
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

	private void EmployeeBenefitFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbEmployee).ActiveRow != null)
			{
				DataTable dt = hrmEmployeeBenefit.ReadByEmployeeId(new hrmEmployeeBenefit
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EmployeeId = Conversion.ToInt(cmbEmployee.Value)
				});
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDLNew(dt, cmbEmployeeBenefit, "BenefitId", "Description", "Benefit", false);
					return;
				}
				((Control)(object)cmbEmployeeBenefit).Text = string.Empty;
				((UltraGridBase)cmbEmployeeBenefit).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetByEmployeeId(int EmployeeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			EmployeeAllowance val = new EmployeeAllowance
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EmployeeId = EmployeeId
			};
			List<EmployeeAllowance> lst = new List<EmployeeAllowance>();
			lst = EmployeeAllowance.ReadByEmployeeId(val);
			if (lst.Count > 0)
			{
				dthead.Rows.Clear();
				for (int i = 0; i < lst.Count; i++)
				{
					dthead.Rows.Add(lst[i].EmployeeAllowanceId, lst[i].EmployeeId, lst[i].EmployeeName, lst[i].EmployeeBenefitId, lst[i].EmployeeBenefit, Conversion.ToDateTime((object)lst[i].AppliedOn).ToString("dd-MMM-yyyy"), lst[i].TotalAmount, lst[i].IsForPayroll, lst[i].Remarks);
				}
				grdDetails.DataSource = dthead;
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

	private void cmbEmployee_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			EmployeeBenefitFill();
			GetByEmployeeId(Conversion.ToInt(cmbEmployee.Value));
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
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		try
		{
			if (RecId == 0L)
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
			EmployeeAllowance obj = new EmployeeAllowance();
			obj.EmployeeAllowanceList = new List<EmployeeAllowance>();
			if (grdDetails.RowCount > 0)
			{
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow r in rows)
				{
					EmployeeAllowance vd = new EmployeeAllowance();
					vd.EmployeeAllowanceId = Conversion.ToInt(r.Cells["Id"].Value);
					if (vd.EmployeeAllowanceId == 0L)
					{
						vd.ActionTypeId = 1;
					}
					else
					{
						vd.ActionTypeId = 2;
					}
					vd.EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
					vd.EmployeeBenefitId = Conversion.ToInt(r.Cells["EmployeeBenefitId"].Value);
					vd.AppliedOn = Conversion.ToDateTime(r.Cells["AppliedOn"].Value);
					vd.TotalAmount = Conversion.ToDecimal(r.Cells["TotalAmount"].Value);
					vd.IsForPayroll = Conversion.ToBool(r.Cells["IsForPayroll"].Value);
					vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.OrganizationId = UserAccount.OrganizationId;
					vd.CompanyId = UserAccount.CompanyId;
					vd.CreatedOn = DateTime.Now;
					vd.AlteredOn = DateTime.Now;
					vd.CreatedById = UserAccount.ID;
					vd.AlteredById = UserAccount.ID;
					vd.ApprovedOn = DateTime.Now;
					vd.ApprovedById = UserAccount.ID;
					vd.IsApproved = false;
					vd.UserLogId = UserAccount.ID;
					obj.EmployeeAllowanceList.Add(vd);
				}
				if (lstDelete.Count > 0)
				{
					for (int i = 0; i < lstDelete.Count; i++)
					{
						EmployeeAllowance obj2 = new EmployeeAllowance();
						if (lstDelete[i].EmployeeAllowanceId != 0L)
						{
							obj2.ActionTypeId = lstDelete[i].ActionTypeId;
							obj2.AlteredById = lstDelete[i].AlteredById;
							obj2.AlteredOn = lstDelete[i].AlteredOn;
							obj2.UserLogId = lstDelete[i].UserLogId;
							obj2.ApprovedById = lstDelete[i].ApprovedById;
							obj2.ApprovedOn = lstDelete[i].ApprovedOn;
							obj2.CompanyId = lstDelete[i].CompanyId;
							obj2.CreatedById = lstDelete[i].CreatedById;
							obj2.CreatedOn = lstDelete[i].CreatedOn;
							obj2.OrganizationId = lstDelete[i].OrganizationId;
							obj2.AppliedOn = lstDelete[i].AppliedOn;
							obj2.EmployeeAllowanceId = lstDelete[i].EmployeeAllowanceId;
							obj2.EmployeeBenefitId = lstDelete[i].EmployeeBenefitId;
							obj2.EmployeeId = lstDelete[i].EmployeeId;
							obj2.IsForPayroll = lstDelete[i].IsForPayroll;
							obj2.TotalAmount = lstDelete[i].TotalAmount;
							obj2.Remarks = lstDelete[i].Remarks;
							obj.EmployeeAllowanceList.Add(obj2);
						}
					}
				}
				EmployeeAllowance.Save(obj);
				MessageBox.Show("Receod Save Successfully");
				RecId = 0L;
				lstDelete.Clear();
				btnsave.Visible = true;
				btnUpdate.Visible = false;
				grdDetails.ClearStructure();
				Reset();
				dthead.Clear();
				return;
			}
			throw new Exception("Grid record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnsave_Click(null, null);
	}

	private void ReadById(long ID)
	{
		try
		{
			RecId = ID;
			List<EmployeeAllowance> lst = EmployeeAllowance.GetByID(ID);
			if (lst.Count > 0)
			{
				dthead.Rows.Clear();
				for (int i = 0; i < lst.Count; i++)
				{
					dthead.Rows.Add(lst[i].EmployeeAllowanceId, lst[i].EmployeeId, lst[i].EmployeeName, lst[i].EmployeeBenefitId, lst[i].EmployeeBenefit, lst[i].AppliedOn, lst[i].TotalAmount, lst[i].IsForPayroll, lst[i].Remarks);
				}
				grdDetails.DataSource = dthead;
				grdDetails.RetrieveStructure();
				grdSettings();
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
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
			txtAppliedOn.Value = DateTime.Now;
			txtTotalAmount.Text = string.Empty;
			IsForPayroll.Checked = false;
			txtRemarks.Text = string.Empty;
			cmbEmployee.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0L;
			lstDelete.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			cmbEmployee.ReadOnly = false;
			grdDetails.ClearStructure();
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			EmployeeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click_1(object sender, EventArgs e)
	{
		try
		{
			EmployeeFill();
			EmployeeBenefitFill();
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
			frmEmployeeRegistration obj = new frmEmployeeRegistration(UserAccount);
			obj.formright = CommonServices.SetRightsValueInRightsObject("EmployeeRegistration");
			if (obj.formright.DoHaveViewRight)
			{
				obj.Show();
			}
			else
			{
				MessageBox.Show("You Dont Have rights View Of This Form..");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBenifits_Click(object sender, EventArgs e)
	{
		try
		{
			new BenifitDefine(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTotalAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbEmployee_Leave(object sender, EventArgs e)
	{
		try
		{
			EmployeeBenefitFill();
			GetByEmployeeId(Conversion.ToInt(cmbEmployee.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbEmployee_TextChanged(object sender, EventArgs e)
	{
		try
		{
			EmployeeBenefitFill();
			GetByEmployeeId(Conversion.ToInt(cmbEmployee.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmEmployeeAllowance_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Expected O, but got Unknown
		GridEXLayout grdDetails_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PayRoll.frmEmployeeAllowance));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetails = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnEmployee = new System.Windows.Forms.Button();
		this.btnBenifits = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.cmbEmployeeBenefit = new UltraCombo();
		this.txtAppliedOn = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.IsForPayroll = new System.Windows.Forms.CheckBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.cmbEmployee = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetails).BeginInit();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeBenefit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(784, 569);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(784, 569);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(776, 543);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(770, 537);
		this.panel2.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetails);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 196);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(770, 341);
		this.panel7.TabIndex = 511;
		this.grdDetails.AllowEdit = (InheritableBoolean)2;
		grdDetails_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetails.DesignTimeLayout = grdDetails_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetails).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetails.DynamicFiltering = true;
		this.grdDetails.FilterMode = (FilterMode)2;
		this.grdDetails.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetails).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetails.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetails).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetails).Name = "grdDetails";
		((ScrollableBase)this.grdDetails).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetails).Size = new System.Drawing.Size(770, 341);
		((System.Windows.Forms.Control)(object)this.grdDetails).TabIndex = 0;
		this.grdDetails.TotalRow = (InheritableBoolean)1;
		this.grdDetails.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetails).VisualStyle = (VisualStyle)4;
		this.grdDetails.ColumnButtonClick += new ColumnActionEventHandler(grdDetails_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetails).DoubleClick += new System.EventHandler(grdDetails_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 170);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(770, 26);
		this.panel6.TabIndex = 510;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 17);
		this.label3.TabIndex = 478;
		this.label3.Text = "Detail";
		this.panel5.Controls.Add(this.btnEmployee);
		this.panel5.Controls.Add(this.btnBenifits);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.label16);
		this.panel5.Controls.Add(this.label34);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeBenefit);
		this.panel5.Controls.Add(this.txtAppliedOn);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.btnCancelUpdateDetial);
		this.panel5.Controls.Add(this.txtTotalAmount);
		this.panel5.Controls.Add(this.IsForPayroll);
		this.panel5.Controls.Add(this.btnUpdateDetail);
		this.panel5.Controls.Add(this.txtRemarks);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployee);
		this.panel5.Controls.Add(this.btnplus);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 56);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(770, 114);
		this.panel5.TabIndex = 0;
		this.btnEmployee.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployee.Location = new System.Drawing.Point(228, 33);
		this.btnEmployee.Name = "btnEmployee";
		this.btnEmployee.Size = new System.Drawing.Size(28, 26);
		this.btnEmployee.TabIndex = 1;
		this.btnEmployee.TabStop = false;
		this.btnEmployee.UseVisualStyleBackColor = true;
		this.btnEmployee.Click += new System.EventHandler(btnEmployee_Click);
		this.btnBenifits.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnBenifits.Location = new System.Drawing.Point(444, 33);
		this.btnBenifits.Name = "btnBenifits";
		this.btnBenifits.Size = new System.Drawing.Size(28, 26);
		this.btnBenifits.TabIndex = 3;
		this.btnBenifits.TabStop = false;
		this.btnBenifits.UseVisualStyleBackColor = true;
		this.btnBenifits.Click += new System.EventHandler(btnBenifits_Click);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(17, 14);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(59, 15);
		this.label10.TabIndex = 476;
		this.label10.Text = "Employee";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(626, 14);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(80, 15);
		this.label16.TabIndex = 483;
		this.label16.Text = "Total Amount";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(17, 62);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(52, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Remarks";
		this.cmbEmployeeBenefit.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeBenefit.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeBenefit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeBenefit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeBenefit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeBenefit).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeBenefit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeBenefit).Location = new System.Drawing.Point(261, 33);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeBenefit).Name = "cmbEmployeeBenefit";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeBenefit).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeBenefit).TabIndex = 2;
		this.txtAppliedOn.CustomFormat = "dd-MMM-yyyy";
		this.txtAppliedOn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAppliedOn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtAppliedOn.Location = new System.Drawing.Point(476, 35);
		this.txtAppliedOn.Name = "txtAppliedOn";
		this.txtAppliedOn.Size = new System.Drawing.Size(146, 23);
		this.txtAppliedOn.TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(479, 14);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 15);
		this.label4.TabIndex = 506;
		this.label4.Text = "Applied On";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(258, 14);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(99, 15);
		this.label1.TabIndex = 508;
		this.label1.Text = "Employee Benefit";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(688, 81);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(67, 23);
		this.btnCancelUpdateDetial.TabIndex = 10;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(624, 35);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.Size = new System.Drawing.Size(131, 23);
		this.txtTotalAmount.TabIndex = 5;
		this.txtTotalAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTotalAmount_KeyPress);
		this.IsForPayroll.AutoSize = true;
		this.IsForPayroll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.IsForPayroll.Location = new System.Drawing.Point(476, 83);
		this.IsForPayroll.Name = "IsForPayroll";
		this.IsForPayroll.Size = new System.Drawing.Size(94, 19);
		this.IsForPayroll.TabIndex = 7;
		this.IsForPayroll.Text = "Is For Payroll";
		this.IsForPayroll.UseVisualStyleBackColor = true;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(617, 81);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 23);
		this.btnUpdateDetail.TabIndex = 9;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(17, 81);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(455, 23);
		this.txtRemarks.TabIndex = 6;
		this.cmbEmployee.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Location = new System.Drawing.Point(17, 33);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Name = "cmbEmployee";
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TabIndex = 0;
		this.cmbEmployee.ValueChanged += new System.EventHandler(cmbEmployee_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TextChanged += new System.EventHandler(cmbEmployee_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Leave += new System.EventHandler(cmbEmployee_Leave);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(582, 81);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(33, 23);
		this.btnplus.TabIndex = 8;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(770, 26);
		this.panel4.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(177, 19);
		this.label2.TabIndex = 477;
		this.label2.Text = "Employee Allowance";
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(770, 30);
		this.panel3.TabIndex = 0;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.btnRefresh });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(770, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(784, 569);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmEmployeeAllowance";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEmployeeAllowance_FormClosing);
		base.Load += new System.EventHandler(FrmExportSalesContract_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(FrmExportSalesContract_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetails).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeBenefit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
