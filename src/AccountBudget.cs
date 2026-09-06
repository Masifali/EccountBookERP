using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
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
namespace Architecture.WinApp.Account_Definition
{

public class AccountBudget : Form
{
	private Attachment AT = new Attachment();

	private DataTable dtDateCombo = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dth = new DataTable();

	private bool UpdateMode;

	private bool ChkAmountFiled;

	private int RecId;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNew;

	private ToolStripButton BtnSave;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnRefresh;

	internal UltraCombo combbrnch;

	internal UltraCombo combproject;

	internal UltraCombo combcompany;

	private Label label1;

	private Label label13;

	private DateTimePicker DocDate;

	private Label label18;

	private Label label5;

	private TextBox txtdocno;

	private Label label4;

	internal UltraCombo cmbExpenseAcc;

	private Label label11;

	private TextBox txtAmount;

	private Button btnGenerate;

	internal GridEX grdDetail;

	private ToolStripButton btnUpdate;

	private Panel panel6;

	private Panel panel9;

	private Panel panel8;

	private Panel panel7;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX DataGridHistoryDetail;

	private GroupBox groupBox11;

	private Button BtnShowInputHistory;

	private Label label145;

	internal UltraCombo CmbExpenseAccountHistory;

	private DateTimePicker txtToDateHistory;

	private Label label146;

	private Label label147;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label148;

	private Label label149;

	private TextBox txtFromNoHistory;

	internal GridEX DataGridHistory;

	private Panel panel10;

	internal UltraCombo cmbMonthFrom;

	private Label label9;

	internal UltraCombo CmbMonthTo;

	private Label label7;

	private ToolStripButton btnattachment;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private ToolStripButton btnshortcutkeys;

	private Label label3;

	private Label label2;

	private CheckBox ChkPrint;

	private Label label6;

	private Label label10;

	private Label label8;

	public AccountBudget(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void AccountBudget_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(980, 600);
			formright = CommonServices.SetRightsValueInRightsObject(Conversion.ToString((object)base.Name));
			BtnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			BtnPrint.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtDateCombo.Columns.Add("Id");
			dtDateCombo.Columns.Add("Month");
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("Month");
			dtdetail.Columns.Add("Percent%", typeof(double));
			dtdetail.Columns.Add("Amount", typeof(double));
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdDetailsetting();
			ProjectFill();
			GenerateCode();
			COABind();
			MonthFill();
			DocDate.Focus();
			if (((DisposableObjectCollectionBase)((UltraGridBase)cmbMonthFrom).Rows).Count > 0)
			{
				string fromMonthValue = DateTime.Now.ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + fromMonthValue + "'").Length != 0)
				{
					((Control)(object)cmbMonthFrom).Text = fromMonthValue;
				}
				string toMonthValue = DateTime.Now.AddMonths(1).ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + toMonthValue + "'").Length != 0)
				{
					((Control)(object)CmbMonthTo).Text = toMonthValue;
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
			dtDateCombo.Clear();
			DateTime start_Period = clsGlobalVariables.ActiveYr.Start_Period;
			DateTime financialEndDate = clsGlobalVariables.ActiveYr.End_Period;
			List<DateTime> monthsList = new List<DateTime>();
			DateTime date = start_Period;
			while (date <= financialEndDate)
			{
				monthsList.Add(date);
				date = date.AddMonths(1);
			}
			foreach (DateTime month in monthsList)
			{
				dtDateCombo.Rows.Add(month.ToString("MMM yyyy"), month.ToString("MMM yyyy"));
			}
			if (dtDateCombo.Rows.Count > 0)
			{
				DDL.BindDDL(dtDateCombo, cmbMonthFrom, "Id", "Month", "Month", ZeroIndex: false);
				DDL.BindDDL(dtDateCombo, CmbMonthTo, "Id", "Month", "Month", ZeroIndex: false);
				((UltraGridBase)cmbMonthFrom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbMonthTo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			cmbMonthFrom.Value = DateTime.Now.Month;
			CmbMonthTo.Value = DateTime.Now.AddMonths(1).Month;
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
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			int DocNo = AccountsBudgetHeader.GenerateCode(new global::Architecture.Model.Accounts.AccountsBudgetHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtdocno.Text = DocNo.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProjectFill()
	{
		try
		{
			int Id = Conversion.ToInt(combproject.Value);
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)combproject).Rows[0].Activate();
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						combproject.Value = Id;
					}
					else
					{
						((Control)(object)combproject).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combproject).Text = string.Empty;
				((UltraGridBase)combproject).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void COABind()
	{
		try
		{
			int Id = Conversion.ToInt(cmbExpenseAcc.Value);
			int Id2 = Conversion.ToInt(CmbExpenseAccountHistory.Value);
			DataTable dtacc = new DataTable();
			dtacc = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtacc.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("AccountTitle");
				for (int i = 0; i < dtacc.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtacc.Rows[i]["AccountTypeId"]) == 11)
					{
						dt.Rows.Add(dtacc.Rows[i]["Id"], dtacc.Rows[i]["AccountTitle"]);
					}
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbExpenseAcc, "Id", "AccountTitle", "Account Title ", ZeroIndex: true);
					DDL.BindDDL(dt, CmbExpenseAccountHistory, "Id", "AccountTitle", "Account Title ", ZeroIndex: true);
					if (Id > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
						{
							cmbExpenseAcc.Value = Id;
						}
						else
						{
							((Control)(object)cmbExpenseAcc).Text = string.Empty;
						}
					}
					if (Id2 > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
						{
							CmbExpenseAccountHistory.Value = Id;
						}
						else
						{
							((Control)(object)CmbExpenseAccountHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)cmbExpenseAcc).Text = string.Empty;
					((UltraGridBase)cmbExpenseAcc).DataSource = null;
					((Control)(object)CmbExpenseAccountHistory).Text = string.Empty;
					((UltraGridBase)CmbExpenseAccountHistory).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbExpenseAcc).Text = string.Empty;
				((UltraGridBase)cmbExpenseAcc).DataSource = null;
				((Control)(object)CmbExpenseAccountHistory).Text = string.Empty;
				((UltraGridBase)CmbExpenseAccountHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)combproject).ActiveRow == null || Conversion.ToInt(combproject.Value) == 0)
		{
			MessageBox.Show("Project  Field Required");
			combproject.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("Doc No   Field Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbMonthFrom).ActiveRow == null || Conversion.ToString((object)((Control)(object)cmbMonthFrom).Text) == string.Empty)
		{
			MessageBox.Show("MonthFrom  Field Required");
			cmbMonthFrom.Focus();
			return false;
		}
		if (((UltraGridBase)CmbMonthTo).ActiveRow == null || Conversion.ToString((object)((Control)(object)CmbMonthTo).Text) == string.Empty)
		{
			MessageBox.Show("Month To  Field Required");
			CmbMonthTo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbExpenseAcc).ActiveRow == null || Conversion.ToInt(cmbExpenseAcc.Value) == 0)
		{
			MessageBox.Show("Exp Account Field Required");
			cmbExpenseAcc.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmount.Text.Trim()) == 0.0 || Conversion.ToDouble((object)txtAmount.Text) == 0.0)
		{
			MessageBox.Show("Amount  Field Required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationGenerate()
	{
		if (txtAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmount.Text) == 0.0)
		{
			MessageBox.Show("Amount  Field Required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void BtnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FormReset()
	{
		try
		{
			((Control)(object)cmbExpenseAcc).Text = string.Empty;
			txtAmount.Text = string.Empty;
			UpdateMode = false;
			ChkAmountFiled = false;
			RecId = 0;
			GenerateCode();
			grdDetail.ClearStructure();
			dtdetail.Clear();
			BtnSave.Visible = true;
			btnUpdate.Visible = false;
			DocDate.Focus();
			if (((DisposableObjectCollectionBase)((UltraGridBase)cmbMonthFrom).Rows).Count > 0)
			{
				string fromMonthValue = DateTime.Now.ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + fromMonthValue + "'").Length != 0)
				{
					((Control)(object)cmbMonthFrom).Text = fromMonthValue;
				}
				string toMonthValue = DateTime.Now.AddMonths(1).ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + toMonthValue + "'").Length != 0)
				{
					((Control)(object)CmbMonthTo).Text = toMonthValue;
				}
			}
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
			ProjectFill();
			COABind();
			MonthFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnGenerate_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationGenerate())
			{
				return;
			}
			dtdetail.Clear();
			grdDetail.ClearStructure();
			DateTime fromMonth = DateTime.ParseExact(((Control)(object)cmbMonthFrom).Text, "MMM yyyy", CultureInfo.InvariantCulture);
			DateTime toMonth = DateTime.ParseExact(((Control)(object)CmbMonthTo).Text, "MMM yyyy", CultureInfo.InvariantCulture);
			int monthsDifference = (int)((double)(toMonth - fromMonth).Days / 30.4) + 1;
			decimal amountPerMonth = Conversion.ToDecimal((object)Conversion.ToDouble((object)txtAmount.Text)) / (decimal)monthsDifference;
			double percentagePerMonth = 100.0 / (double)monthsDifference;
			for (int i = 0; i < monthsDifference; i++)
			{
				if (!(fromMonth <= toMonth))
				{
					break;
				}
				dtdetail.Rows.Add(0, fromMonth.ToString("MMM-yyyy"), percentagePerMonth, amountPerMonth);
				fromMonth = fromMonth.AddMonths(1);
				ChkAmountFiled = true;
			}
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdDetailsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailsetting()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Month"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Month"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["Percent%"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["Percent%"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Percent%"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Percent%"].FormatString = "##,#.###";
			grdDetail.RootTable.Columns["Percent%"].TotalFormatString = "##,#.###";
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Month"].Width = 100;
			grdDetail.RootTable.Columns["Percent%"].Width = 60;
			grdDetail.RootTable.Columns["Amount"].Width = 120;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.UpdateData();
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Percent%")
				{
					double amont2 = Conversion.ToDouble((object)txtAmount.Text) * Conversion.ToDouble((object)item.Cells["Percent%"].Text) / 100.0;
					item.Cells["Amount"].Value = amont2;
				}
				if (e.Column.Key == "Amount")
				{
					double amont = Conversion.ToDouble((object)item.Cells["Amount"].Text) / Conversion.ToDouble((object)txtAmount.Text) * 100.0;
					item.Cells["Percent%"].Value = amont;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
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

	private void Insert()
	{
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			DateTime fromMonth = DateTime.ParseExact(((Control)(object)cmbMonthFrom).Text, "MMM yyyy", CultureInfo.InvariantCulture);
			int expectedRowCount = Conversion.ToInt((object)((double)(DateTime.ParseExact(((Control)(object)CmbMonthTo).Text, "MMM yyyy", CultureInfo.InvariantCulture) - fromMonth).Days / 30.4));
			if (Math.Abs(expectedRowCount - grdDetail.RowCount) > 1 || Math.Abs(expectedRowCount - grdDetail.RowCount) < 1)
			{
				throw new Exception("Month count does not match with Detail Record");
			}
			if (!FormValidation())
			{
				return;
			}
			double a = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Percent%"], (AggregateFunction)2));
			double sumofAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
			if (Math.Round(a) != 100.0)
			{
				MessageBox.Show("Grid Total Percent Not Equal To 100");
				return;
			}
			if (Math.Round(sumofAmount) != Conversion.ToDouble((object)txtAmount.Text.Trim()))
			{
				MessageBox.Show("Grid Total Amount and Header Amount Not Match ");
				return;
			}
			AccountsBudgetHeader header = new global::Architecture.Model.Accounts.AccountsBudgetHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				header.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			header.OrganizationId = UserAccount.OrganizationId;
			header.CompanyId = UserAccount.CompanyId;
			header.BranchId = Conversion.ToInt((object)UserAccount.BranchesId);
			header.ProjectId = Conversion.ToInt(combproject.Value);
			header.ExpenseGLAcId = Conversion.ToInt(cmbExpenseAcc.Value);
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.DocumentTypeId = 11;
			header.DocNo = Conversion.ToInt((object)txtdocno.Text);
			header.DocDate = DocDate.Value;
			DateTime FromDate = DateTime.ParseExact(((Control)(object)cmbMonthFrom).Text, "MMM yyyy", CultureInfo.InvariantCulture);
			DateTime ToDate = DateTime.ParseExact(((Control)(object)CmbMonthTo).Text, "MMM yyyy", CultureInfo.InvariantCulture);
			header.FnFromMonth = FromDate;
			header.FnToMonth = ToDate;
			header.BudgetTotalAmount = Conversion.ToDecimal((object)txtAmount.Text);
			header.EntryUserId = UserAccount.ID;
			header.ModifyUserId = UserAccount.ID;
			header.EntryDate = DateTime.Now;
			header.ModifyDate = DateTime.Now;
			header.accountBudgetDetails = new List<AccountsBudgetDetail>();
			header.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			header.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				AccountsBudgetDetail detail = new AccountsBudgetDetail();
				detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				detail.BudgetMonth = Conversion.ToDateTime(r.Cells["Month"].Value);
				detail.BudgetPrcnt = Conversion.ToDecimal(r.Cells["Percent%"].Value);
				detail.BudgetAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
				header.accountBudgetDetails.Add(detail);
			}
			FormHelper.UpdateAttachmentsForObject<AccountsBudgetHeader>(header, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = AccountsBudgetHeader.Save(header);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + header.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 11, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + header.DocNo);
			}
			FormReset();
			if (ChkPrint.Checked)
			{
				CommonServices.AccountsBudgetSlip_11(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
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

	private void ReadById(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			AccountsBudgetHeader header = new global::Architecture.Model.Accounts.AccountsBudgetHeader();
			header = AccountsBudgetHeader.GetByID(Id);
			if (header != null && header.accountBudgetDetails.Count > 0)
			{
				tabControl1.SelectedIndex = 0;
				RecId = Id;
				combproject.Value = header.ProjectId;
				txtdocno.Text = header.DocNo.ToString();
				DocDate.Value = header.DocDate;
				string fromMonthValue = header.FnFromMonth.ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + fromMonthValue + "'").Length != 0)
				{
					((Control)(object)cmbMonthFrom).Text = fromMonthValue;
				}
				string toMonthValue = header.FnToMonth.ToString("MMM yyyy");
				if (dtDateCombo.Select("Month='" + toMonthValue + "'").Length != 0)
				{
					((Control)(object)CmbMonthTo).Text = toMonthValue;
				}
				txtAmount.Text = header.BudgetTotalAmount.ToString();
				cmbExpenseAcc.Value = header.ExpenseGLAcId;
				dtdetail.Clear();
				for (int i = 0; i < header.accountBudgetDetails.Count; i++)
				{
					dtdetail.Rows.Add(header.accountBudgetDetails[i].Id, header.accountBudgetDetails[i].BudgetMonth.ToString("MMM-yyyy"), header.accountBudgetDetails[i].BudgetPrcnt, header.accountBudgetDetails[i].BudgetAmount);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdDetailsetting();
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
				BtnSave.Visible = false;
				btnUpdate.Visible = true;
				UpdateMode = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				DocDate.Focus();
			}
			else
			{
				txtFromdateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Resethistory()
	{
		try
		{
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbExpenseAccountHistory).Text = string.Empty;
			CmbExpenseAccountHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
	}

	private void BtnShowInputHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters header = new ReportsParameters();
			header.OrganizationId = UserAccount.OrganizationId;
			header.CompanyId = UserAccount.CompanyId;
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.DocumentTypeId = 11;
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!header.CanViewAllRecord)
			{
				header.EntryUser = UserAccount.ID;
			}
			header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			header.AccountId = Conversion.ToInt((object)((Control)(object)CmbExpenseAccountHistory).Text);
			dth = AccountsBudgetHeader.GetHisoty(header);
			if (dth.Rows.Count > 0)
			{
				DataTable dthistory = new DataTable();
				dthistory.Columns.Add("Id", typeof(int));
				dthistory.Columns.Add("DocumentTypeId", typeof(int));
				dthistory.Columns.Add("DocNo", typeof(int));
				dthistory.Columns.Add("DocDate", typeof(DateTime));
				dthistory.Columns.Add("FromMonth");
				dthistory.Columns.Add("ToMonth");
				dthistory.Columns.Add("ExpenseAccount");
				dthistory.Columns.Add("Amount", typeof(double));
				dthistory.Columns.Add("EntryUserName");
				dthistory.Columns.Add("EntryDate", typeof(DateTime));
				dthistory.Columns.Add("ModifyUserName");
				dthistory.Columns.Add("ModifyDate", typeof(DateTime));
				dthistory.Columns.Add("IsApproved", typeof(bool));
				dthistory.Columns.Add("ApprovedUserName");
				dthistory.Columns.Add("ApprovedDate", typeof(DateTime));
				dthistory.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dth.Rows.Count; i++)
				{
					dthistory.Rows.Add(dth.Rows[i]["Id"], dth.Rows[i]["DocumentTypeId"], dth.Rows[i]["DocNo"], Conversion.ToDateTime(dth.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dth.Rows[i]["FnFromMonth"]).ToString("MMM-yyyy"), Conversion.ToDateTime(dth.Rows[i]["FnToMonth"]).ToString("MMM-yyyy"), dth.Rows[i]["AccountTitle"], dth.Rows[i]["BudgetTotalAmount"], dth.Rows[i]["EntryUserName"], Conversion.ToDateTime(dth.Rows[i]["EntryDate"]).ToShortDateString(), dth.Rows[i]["ModifyUserName"], Conversion.ToDateTime(dth.Rows[i]["ModifyDate"]).ToShortDateString(), dth.Rows[i]["IsApproved"], dth.Rows[i]["ApprovedUserName"], Conversion.ToDateTime(dth.Rows[i]["ApprovedDate"]).ToShortDateString(), dth.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dthistory;
				DataGridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
				DataGridHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Edit")
				{
					ReadById(Conversion.ToInt(item.Cells["Id"].Value));
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.AccountsBudgetSlip_11(Conversion.ToInt(item.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		GridEXRow item = DataGridHistory.CurrentRow;
		if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 11);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				ReadById(Conversion.ToInt(item.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				grdHistoryDetailbind(Conversion.ToInt(item.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryDetailbind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			AccountsBudgetHeader po = new global::Architecture.Model.Accounts.AccountsBudgetHeader();
			po = AccountsBudgetHeader.GetByID(Id);
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("Month");
				dt.Columns.Add("Percent%", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Clear();
				for (int i = 0; i < po.accountBudgetDetails.Count; i++)
				{
					dt.Rows.Add(po.accountBudgetDetails[i].Id, po.accountBudgetDetails[i].BudgetMonth.ToString("MMM-yyyy"), po.accountBudgetDetails[i].BudgetPrcnt, po.accountBudgetDetails[i].BudgetAmount);
				}
				DataGridHistoryDetail.DataSource = dt;
				DataGridHistoryDetail.RetrieveStructure();
				DataGridHistoryDetailsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void DataGridHistoryDetailsetting()
	{
		try
		{
			DataGridHistoryDetail.RootTable.Columns["Id"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["Percent%"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["Percent%"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["Percent%"].FormatString = "##,#.###";
			DataGridHistoryDetail.RootTable.Columns["Percent%"].TotalFormatString = "##,#.###";
			DataGridHistoryDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["Month"].Width = 100;
			DataGridHistoryDetail.RootTable.Columns["Percent%"].Width = 60;
			DataGridHistoryDetail.RootTable.Columns["Amount"].Width = 120;
			CommonServices.GridAutoAdjustmentNew(DataGridHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AccountsBudgetSlip_11(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountBudget_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					txtFromdateHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && BtnSave.Visible && BtnSave.Enabled)
				{
					BtnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					BtnPrint_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistoryDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)DataGridHistoryDetail).Focused)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistoryDetail).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtFromdateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print Slip 1853");
			dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		GridEXRow item = DataGridHistory.CurrentRow;
		if (DataGridHistory.CurrentColumn == null || item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
		{
			return;
		}
		if (e.Control && e.KeyCode == Keys.Space)
		{
			if (DataGridHistory.CurrentColumn.Key == "Edit")
			{
				ReadById(Conversion.ToInt(item.Cells["Id"].Value));
			}
			if (DataGridHistory.CurrentColumn.Key == "Print")
			{
				CommonServices.AccountsBudgetSlip_11(Conversion.ToInt(item.Cells["Id"].Value));
			}
			if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 11);
			}
		}
		if (e.Control && e.KeyCode == Keys.Return)
		{
			ReadById(Conversion.ToInt(item.Cells["Id"].Value));
		}
		if (e.Control && e.KeyCode == Keys.P)
		{
			CommonServices.AccountsBudgetSlip_11(Conversion.ToInt(item.Cells["Id"].Value));
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void ChkPrint_CheckedChanged(object sender, EventArgs e)
	{
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
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_09db: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Expected O, but got Unknown
		//IL_354e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3558: Expected O, but got Unknown
		//IL_357c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3586: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbMonthTo = new UltraCombo();
		this.cmbMonthFrom = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.btnGenerate = new System.Windows.Forms.Button();
		this.label11 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbExpenseAcc = new UltraCombo();
		this.combbrnch = new UltraCombo();
		this.combproject = new UltraCombo();
		this.combcompany = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label18 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.label2 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.DataGridHistoryDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox11 = new System.Windows.Forms.GroupBox();
		this.BtnShowInputHistory = new System.Windows.Forms.Button();
		this.label145 = new System.Windows.Forms.Label();
		this.CmbExpenseAccountHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label146 = new System.Windows.Forms.Label();
		this.label147 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label148 = new System.Windows.Forms.Label();
		this.label149 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMonthTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMonthFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExpenseAcc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcompany).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseAccountHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 561);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 561);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 526);
		this.panel2.TabIndex = 0;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 145);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(950, 381);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 120);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 25);
		this.panel5.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1209, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(51, 26);
		this.ctrlGrdBar1.TabIndex = 21;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 2);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 17);
		this.label3.TabIndex = 8;
		this.label3.Text = "Detail";
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMonthTo);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMonthFrom);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.btnGenerate);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtAmount);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbExpenseAcc);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combbrnch);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combproject);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combcompany);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 53);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 67);
		this.panel4.TabIndex = 0;
		this.CmbMonthTo.AllowNull = (DefaultableBoolean)1;
		this.CmbMonthTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMonthTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMonthTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMonthTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.CmbMonthTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMonthTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMonthTo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMonthTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMonthTo).Location = new System.Drawing.Point(351, 35);
		((System.Windows.Forms.Control)(object)this.CmbMonthTo).Name = "CmbMonthTo";
		((System.Windows.Forms.Control)(object)this.CmbMonthTo).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbMonthTo).TabIndex = 4;
		this.cmbMonthFrom.AllowNull = (DefaultableBoolean)1;
		this.cmbMonthFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMonthFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMonthFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonthFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbMonthFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMonthFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMonthFrom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbMonthFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMonthFrom).Location = new System.Drawing.Point(351, 6);
		((System.Windows.Forms.Control)(object)this.cmbMonthFrom).Name = "cmbMonthFrom";
		((System.Windows.Forms.Control)(object)this.cmbMonthFrom).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbMonthFrom).TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(277, 41);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(59, 15);
		this.label7.TabIndex = 148;
		this.label7.Text = "To Month";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(277, 12);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(74, 15);
		this.label9.TabIndex = 148;
		this.label9.Text = "From Month";
		this.btnGenerate.BackColor = System.Drawing.Color.Teal;
		this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGenerate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerate.ForeColor = System.Drawing.Color.White;
		this.btnGenerate.Location = new System.Drawing.Point(846, 35);
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.Size = new System.Drawing.Size(95, 26);
		this.btnGenerate.TabIndex = 7;
		this.btnGenerate.Text = "Generate";
		this.btnGenerate.UseVisualStyleBackColor = false;
		this.btnGenerate.Click += new System.EventHandler(btnGenerate_Click);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(587, 41);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(51, 15);
		this.label11.TabIndex = 8;
		this.label11.Text = "Amount";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(663, 37);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(181, 23);
		this.txtAmount.TabIndex = 6;
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(587, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(74, 15);
		this.label4.TabIndex = 19;
		this.label4.Text = "Exp Account";
		this.cmbExpenseAcc.AllowNull = (DefaultableBoolean)1;
		this.cmbExpenseAcc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExpenseAcc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExpenseAcc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExpenseAcc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExpenseAcc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbExpenseAcc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExpenseAcc).Location = new System.Drawing.Point(663, 6);
		((System.Windows.Forms.Control)(object)this.cmbExpenseAcc).Name = "cmbExpenseAcc";
		((System.Windows.Forms.Control)(object)this.cmbExpenseAcc).Size = new System.Drawing.Size(278, 26);
		((System.Windows.Forms.Control)(object)this.cmbExpenseAcc).TabIndex = 5;
		this.combbrnch.AllowNull = (DefaultableBoolean)1;
		this.combbrnch.AutoCompleteMode = (AutoCompleteMode)4;
		this.combbrnch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combbrnch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combbrnch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.combbrnch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combbrnch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combbrnch).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combbrnch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combbrnch).Location = new System.Drawing.Point(1151, 32);
		((System.Windows.Forms.Control)(object)this.combbrnch).Name = "combbrnch";
		((System.Windows.Forms.Control)(object)this.combbrnch).Size = new System.Drawing.Size(60, 26);
		((System.Windows.Forms.Control)(object)this.combbrnch).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combbrnch).Visible = false;
		this.combproject.AllowNull = (DefaultableBoolean)1;
		this.combproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.combproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.combproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject).Location = new System.Drawing.Point(77, 6);
		((System.Windows.Forms.Control)(object)this.combproject).Name = "combproject";
		((System.Windows.Forms.Control)(object)this.combproject).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.combproject).TabIndex = 0;
		this.combcompany.AllowNull = (DefaultableBoolean)1;
		this.combcompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.combcompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.combcompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.combcompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.combcompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcompany).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcompany).Location = new System.Drawing.Point(1151, 4);
		((System.Windows.Forms.Control)(object)this.combcompany).Name = "combcompany";
		((System.Windows.Forms.Control)(object)this.combcompany).Size = new System.Drawing.Size(60, 26);
		((System.Windows.Forms.Control)(object)this.combcompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combcompany).Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(1086, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(58, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "Company";
		this.label1.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(1086, 37);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 7;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(150, 37);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(124, 23);
		this.DocDate.TabIndex = 2;
		this.DocDate.TabStop = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(8, 12);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(67, 15);
		this.label18.TabIndex = 8;
		this.label18.Text = "Cost Center";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(8, 41);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 11;
		this.label5.Text = "Doc No";
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(77, 37);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(71, 23);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.TextChanged += new System.EventHandler(txtdocno_TextChanged);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 53);
		this.panel3.TabIndex = 0;
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Checked = true;
		this.ChkPrint.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(862, 30);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(85, 20);
		this.ChkPrint.TabIndex = 29;
		this.ChkPrint.Text = "Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.ChkPrint.CheckedChanged += new System.EventHandler(ChkPrint_CheckedChanged);
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.toolStripSeparator4, this.BtnNew, this.btnRefresh, this.toolStripSeparator3, this.BtnSave, this.btnUpdate, this.toolStripSeparator2, this.BtnPrint, this.btnattachment, this.toolStripSeparator1,
			this.btnshortcutkeys
		});
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(950, 27);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(60, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(81, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(65, 24);
		this.BtnSave.Text = "&Save";
		this.BtnSave.Click += new System.EventHandler(BtnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(79, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(85, 24);
		this.BtnPrint.Text = "11-&Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(110, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(127, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 30);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(145, 19);
		this.label2.TabIndex = 8;
		this.label2.Text = "Accounts Budget";
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 532);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 526);
		this.panel6.TabIndex = 0;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 128);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(950, 188);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 74;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label6);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel10.Location = new System.Drawing.Point(0, 316);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(950, 24);
		this.panel10.TabIndex = 73;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(5, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(230, 17);
		this.label6.TabIndex = 9;
		this.label6.Text = "Detail of the above Selected row";
		this.DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(0, 340);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(950, 186);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 72;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label10);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 105);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(950, 23);
		this.panel9.TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(3, 4);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(40, 17);
		this.label10.TabIndex = 9;
		this.label10.Text = "Main";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(903, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel8.Controls.Add(this.groupBox11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 56);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 49);
		this.panel8.TabIndex = 1;
		this.groupBox11.Controls.Add(this.BtnShowInputHistory);
		this.groupBox11.Controls.Add(this.label145);
		this.groupBox11.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExpenseAccountHistory);
		this.groupBox11.Controls.Add(this.txtToDateHistory);
		this.groupBox11.Controls.Add(this.label146);
		this.groupBox11.Controls.Add(this.label147);
		this.groupBox11.Controls.Add(this.txtToDocNoHistory);
		this.groupBox11.Controls.Add(this.txtFromdateHistory);
		this.groupBox11.Controls.Add(this.label148);
		this.groupBox11.Controls.Add(this.label149);
		this.groupBox11.Controls.Add(this.txtFromNoHistory);
		this.groupBox11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox11.Location = new System.Drawing.Point(1, 2);
		this.groupBox11.Name = "groupBox11";
		this.groupBox11.Size = new System.Drawing.Size(946, 44);
		this.groupBox11.TabIndex = 2;
		this.groupBox11.TabStop = false;
		this.groupBox11.Text = "Filters";
		this.BtnShowInputHistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowInputHistory.FlatAppearance.BorderSize = 0;
		this.BtnShowInputHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowInputHistory.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.BtnShowInputHistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowInputHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowInputHistory.Location = new System.Drawing.Point(883, 15);
		this.BtnShowInputHistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowInputHistory.Name = "BtnShowInputHistory";
		this.BtnShowInputHistory.Size = new System.Drawing.Size(62, 25);
		this.BtnShowInputHistory.TabIndex = 5;
		this.BtnShowInputHistory.Text = "Show";
		this.BtnShowInputHistory.UseVisualStyleBackColor = false;
		this.BtnShowInputHistory.Click += new System.EventHandler(BtnShowInputHistory_Click);
		this.label145.AutoSize = true;
		this.label145.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label145.Location = new System.Drawing.Point(629, 20);
		this.label145.Name = "label145";
		this.label145.Size = new System.Drawing.Size(74, 15);
		this.label145.TabIndex = 178;
		this.label145.Text = "Exp Account";
		this.CmbExpenseAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbExpenseAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExpenseAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExpenseAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExpenseAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExpenseAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccountHistory).Location = new System.Drawing.Point(703, 14);
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccountHistory).Name = "CmbExpenseAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccountHistory).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccountHistory).TabIndex = 4;
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(244, 16);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(126, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label146.AutoSize = true;
		this.label146.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label146.Location = new System.Drawing.Point(196, 20);
		this.label146.Name = "label146";
		this.label146.Size = new System.Drawing.Size(48, 15);
		this.label146.TabIndex = 171;
		this.label146.Text = "Date To";
		this.label147.AutoSize = true;
		this.label147.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label147.Location = new System.Drawing.Point(2, 20);
		this.label147.Name = "label147";
		this.label147.Size = new System.Drawing.Size(63, 15);
		this.label147.TabIndex = 171;
		this.label147.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(554, 16);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(66, 16);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(126, 23);
		this.txtFromdateHistory.TabIndex = 0;
		this.label148.AutoSize = true;
		this.label148.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label148.Location = new System.Drawing.Point(509, 20);
		this.label148.Name = "label148";
		this.label148.Size = new System.Drawing.Size(45, 15);
		this.label148.TabIndex = 173;
		this.label148.Text = "Doc To";
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(374, 20);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(60, 15);
		this.label149.TabIndex = 173;
		this.label149.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(435, 16);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtFromNoHistory.TabIndex = 2;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.toolStrip1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 56);
		this.panel7.TabIndex = 0;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(5, 34);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(181, 17);
		this.label8.TabIndex = 9;
		this.label8.Text = "Accounts Budget History";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.BtnRefreshHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 10f);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(61, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(81, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Visible = false;
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "AccountBudget";
		this.Text = "AccountBudget";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(AccountBudget_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AccountBudget_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMonthTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMonthFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExpenseAcc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcompany).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.groupBox11.ResumeLayout(false);
		this.groupBox11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseAccountHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
