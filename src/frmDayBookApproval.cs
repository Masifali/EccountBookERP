// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
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
namespace Architecture.WinApp.ApprovalDashboard
{

public class frmDayBookApproval : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable table = new DataTable();

	private DataTable dtHistory = new DataTable();

	public bool IsApproved;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage2;

	private Panel panel13;

	internal GridEX DataGridCreditHistory;

	internal GridEX DataGridDebitHistory;

	private Label label51;

	private Label label14;

	private DateTimePicker datVouchderDateForHistory;

	private Label label15;

	internal UltraCombo CmbCashAccountForHistory;

	private Button BtnShow;

	private Panel panel8;

	private Panel panel12;

	private Panel panel5;

	private Panel panel11;

	private Panel panel10;

	private ToolStrip toolStrip1;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnPrint144;

	private Panel panel9;

	private Label label6;

	private Panel panel7;

	private Label label5;

	private CheckBox chkZkipCashTransaction;

	private GroupBox groupBox1;

	private CheckBox chkDateEnable;

	internal GridEX DatGridMainHistory;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label1;

	private Button btnapprove;

	private CheckBox chkPrint;

	public frmDayBookApproval(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmDayBook_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1350, 750);
			if (IsApproved)
			{
				label51.Text = "DAY BOOK For Un_Approval";
				btnapprove.Text = "UnApprove";
			}
			else
			{
				btnapprove.Text = "Approve";
				label51.Text = "DAY BOOK For Approval";
			}
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			CashAccountFill();
			CmbCashAccountForHistory.Focus();
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("VoucherCode", typeof(int));
			table.Columns.Add("VoucherDate", typeof(DateTime));
			table.Columns.Add("CashAccountId", typeof(int));
			table.Columns.Add("CashAccountTitle");
			table.Columns.Add("TotalAmountDr", typeof(double));
			table.Columns.Add("TotalAmountCr", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("EntryUserName");
			table.Columns.Add("EntryDate", typeof(DateTime));
			table.Columns.Add("ModifyUserName");
			table.Columns.Add("ModifyDate", typeof(DateTime));
			table.Columns.Add("NoOfAttachments", typeof(int));
			DatGridMainHistory.DataSource = table;
			DatGridMainHistory.RetrieveStructure();
			grdMainSetting();
			GrdMainfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CashAccountFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("2");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCashAccountForHistory, "Id", "AccountTitle", "Cash Account", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GrdMainfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdMainfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (chkDateEnable.Checked)
			{
				obj.DocDate = datVouchderDateForHistory.Value;
			}
			obj.AccountId = Conversion.ToInt(CmbCashAccountForHistory.Value);
			obj.IsApproved = IsApproved;
			dtHistory = VoucherHead.DayBookApprovalHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				table.Clear();
				HashSet<int> existingIds = new HashSet<int>();
				var idTotals = (from row in dtHistory.AsEnumerable()
					group row by row.Field<int>("Id")).ToDictionary((IGrouping<int, DataRow> group) => group.Key, (IGrouping<int, DataRow> group) => new
				{
					DrAmountTotal = group.Sum((DataRow row) => row.Field<double>("DebitAmount")),
					CrAmountTotal = group.Sum((DataRow row) => row.Field<double>("CreditAmount"))
				});
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					int id = Conversion.ToInt(dtHistory.Rows[i]["Id"]);
					if (!existingIds.Contains(id))
					{
						var obj2 = (idTotals.ContainsKey(id) ? idTotals[id] : new
						{
							DrAmountTotal = 0.0,
							CrAmountTotal = 0.0
						});
						double DrAmountTotal = obj2.DrAmountTotal;
						double CrAmountTotal = obj2.CrAmountTotal;
						table.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["VoucherCode"], Conversion.ToDateTime(dtHistory.Rows[i]["VoucherDate"]).ToShortDateString(), dtHistory.Rows[i]["RefAccountId"], dtHistory.Rows[i]["CashAccount"], DrAmountTotal, CrAmountTotal, dtHistory.Rows[i]["Remarks"], dtHistory.Rows[i]["EntryUserName"], Conversion.ToDateTime(dtHistory.Rows[i]["EntryDate"]).ToShortDateString(), dtHistory.Rows[i]["ModifyUserName"], Conversion.ToDateTime(dtHistory.Rows[i]["ModifyDate"]).ToShortDateString(), dtHistory.Rows[i]["NoOfAttachments"]);
						existingIds.Add(id);
					}
				}
				DatGridMainHistory.DataSource = table;
				DatGridMainHistory.RetrieveStructure();
				grdMainSetting();
			}
			else
			{
				DataGridCreditHistory.ClearStructure();
				DatGridMainHistory.ClearStructure();
				DataGridDebitHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMainSetting()
	{
		try
		{
			DatGridMainHistory.RootTable.Columns["Id"].Visible = false;
			DatGridMainHistory.RootTable.Columns["CashAccountId"].Visible = false;
			DatGridMainHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DatGridMainHistory.RootTable.Columns["TotalAmountDr"].AggregateFunction = (AggregateFunction)2;
			DatGridMainHistory.RootTable.Columns["TotalAmountDr"].TextAlignment = (TextAlignment)3;
			DatGridMainHistory.RootTable.Columns["TotalAmountDr"].FormatString = clsGlobalVariables.stringFormatsingle;
			DatGridMainHistory.RootTable.Columns["TotalAmountDr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DatGridMainHistory.RootTable.Columns["TotalAmountCr"].AggregateFunction = (AggregateFunction)2;
			DatGridMainHistory.RootTable.Columns["TotalAmountCr"].TextAlignment = (TextAlignment)3;
			DatGridMainHistory.RootTable.Columns["TotalAmountCr"].FormatString = clsGlobalVariables.stringFormatsingle;
			DatGridMainHistory.RootTable.Columns["TotalAmountCr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(DatGridMainHistory);
			DatGridMainHistory.RootTable.Columns.Add("Select");
			DatGridMainHistory.RootTable.Columns["Select"].ActAsSelector = true;
			DatGridMainHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			DatGridMainHistory.RootTable.Columns["Select"].Width = 30;
			DatGridMainHistory.RootTable.Columns["Select"].Position = 0;
			DatGridMainHistory.RootTable.Columns.Add("Print");
			DatGridMainHistory.RootTable.Columns["Print"].Caption = "Print";
			DatGridMainHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatGridMainHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatGridMainHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatGridMainHistory.RootTable.Columns["Print"].Width = 45;
			DatGridMainHistory.RootTable.Columns["Print"].Position = 1;
			if (IsApproved)
			{
				DatGridMainHistory.RootTable.Columns.Add("UnApprove");
				DatGridMainHistory.RootTable.Columns["UnApprove"].Caption = "UnApprove";
				DatGridMainHistory.RootTable.Columns["UnApprove"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DatGridMainHistory.RootTable.Columns["UnApprove"].ButtonStyle = (ButtonStyle)5;
				DatGridMainHistory.RootTable.Columns["UnApprove"].ButtonText = "UnApprove";
				DatGridMainHistory.RootTable.Columns["UnApprove"].Width = 75;
				DatGridMainHistory.RootTable.Columns["UnApprove"].Position = 2;
			}
			else
			{
				DatGridMainHistory.RootTable.Columns.Add("Approve");
				DatGridMainHistory.RootTable.Columns["Approve"].Caption = "Approve";
				DatGridMainHistory.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DatGridMainHistory.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
				DatGridMainHistory.RootTable.Columns["Approve"].ButtonText = "Approve";
				DatGridMainHistory.RootTable.Columns["Approve"].Width = 70;
				DatGridMainHistory.RootTable.Columns["Approve"].Position = 2;
			}
			DatGridMainHistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatGridMainHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatGridMainHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DataTable dt1 = new DataTable();
				dt1.Columns.Add("Id", typeof(int));
				dt1.Columns.Add("VoucherCode", typeof(int));
				dt1.Columns.Add("CashAccount");
				dt1.Columns.Add("DetailAccount");
				dt1.Columns.Add("CreditAmount", typeof(double));
				dt1.Columns.Add("Comments");
				dt1.Columns.Add("CheqNoDetail");
				dt1.Columns.Add("EntryUserName");
				dt1.Columns.Add("EntryDate", typeof(DateTime));
				dt1.Columns.Add("ModifyUserName");
				dt1.Columns.Add("ModifyDate", typeof(DateTime));
				dt1.Columns.Add("NoOfAttachments", typeof(int));
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id", typeof(int));
				dt2.Columns.Add("VoucherCode", typeof(int));
				dt2.Columns.Add("CashAccount");
				dt2.Columns.Add("DetailAccount");
				dt2.Columns.Add("DebitAmount", typeof(double));
				dt2.Columns.Add("Comments");
				dt2.Columns.Add("CheqNoDetail");
				dt2.Columns.Add("EntryUserName");
				dt2.Columns.Add("EntryDate", typeof(DateTime));
				dt2.Columns.Add("ModifyUserName");
				dt2.Columns.Add("ModifyDate", typeof(DateTime));
				dt2.Columns.Add("NoOfAttachments");
				DataTable resultRows = (from row in dtHistory.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(r.Cells["Id"].Value)
					select row).CopyToDataTable();
				dt1.Clear();
				dt2.Clear();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					if (Conversion.ToDouble(resultRows.Rows[i]["CreditAmount"]) > 0.0)
					{
						dt1.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["VoucherCode"], resultRows.Rows[i]["CashAccount"], resultRows.Rows[i]["DetailAccount"], resultRows.Rows[i]["CreditAmount"], resultRows.Rows[i]["Comments"], resultRows.Rows[i]["CheqNoDetail"], resultRows.Rows[i]["EntryUserName"], Conversion.ToDateTime(resultRows.Rows[i]["EntryDate"]).ToShortDateString(), resultRows.Rows[i]["ModifyUserName"], Conversion.ToDateTime(resultRows.Rows[i]["ModifyDate"]).ToShortDateString(), resultRows.Rows[i]["NoOfAttachments"]);
					}
					else if (Conversion.ToDouble(resultRows.Rows[i]["DebitAmount"]) > 0.0)
					{
						dt2.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["VoucherCode"], resultRows.Rows[i]["CashAccount"], resultRows.Rows[i]["DetailAccount"], resultRows.Rows[i]["DebitAmount"], resultRows.Rows[i]["Comments"], resultRows.Rows[i]["CheqNoDetail"], resultRows.Rows[i]["EntryUserName"], Conversion.ToDateTime(resultRows.Rows[i]["EntryDate"]).ToShortDateString(), resultRows.Rows[i]["ModifyUserName"], Conversion.ToDateTime(resultRows.Rows[i]["ModifyDate"]).ToShortDateString(), resultRows.Rows[i]["NoOfAttachments"]);
					}
				}
				DataGridCreditHistory.DataSource = dt1;
				DataGridCreditHistory.RetrieveStructure();
				DataGridCreditHistorySetting();
				DataGridDebitHistory.DataSource = dt2;
				DataGridDebitHistory.RetrieveStructure();
				DataGridDebitHistorySetting();
			}
			else
			{
				DataGridCreditHistory.ClearStructure();
				DataGridDebitHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridCreditHistorySetting()
	{
		try
		{
			DataGridCreditHistory.RootTable.Columns["EntryUserName"].Visible = false;
			DataGridCreditHistory.RootTable.Columns["ModifyUserName"].Visible = false;
			DataGridCreditHistory.RootTable.Columns["EntryDate"].Visible = false;
			DataGridCreditHistory.RootTable.Columns["ModifyDate"].Visible = false;
			DataGridCreditHistory.RootTable.Columns["NoOfAttachments"].Visible = false;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(DataGridCreditHistory);
			DataGridCreditHistory.RootTable.Columns["Id"].Visible = false;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridDebitHistorySetting()
	{
		try
		{
			DataGridDebitHistory.RootTable.Columns["EntryUserName"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["ModifyUserName"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["EntryDate"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["NoOfAttachments"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["ModifyDate"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(DataGridDebitHistory);
			DataGridDebitHistory.RootTable.Columns["Id"].Visible = false;
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridCreditHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void DataGridDebitHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void DataGridCreditHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void DataGridDebitHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void DatGridMainHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		try
		{
			if (e.Column.Key == "Print")
			{
				DayBookSlip(Conversion.ToInt(DatGridMainHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Approve")
			{
				if (MessageBox.Show("Are you sure to Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList detail2 = new ApprovalList();
				detail2.OrganizationId = UserAccount.OrganizationId;
				detail2.CompanyId = UserAccount.CompanyId;
				detail2.Id = Conversion.ToInt(DatGridMainHistory.CurrentRow.Cells["Id"].Value);
				detail2.PostDate = DateTime.Now;
				detail2.EntryUser = UserAccount.ID;
				detail2.ActionTypeId = false;
				detail2.ReqType = "AP";
				val.AllApprovalLists.Add(detail2);
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(val);
				MessageBox.Show("Record Approved Successfully");
				if (chkPrint.Checked)
				{
					CommonServices.DayBookSlip(detail2.Id);
				}
				GrdMainfill();
			}
			if (e.Column.Key == "UnApprove" && MessageBox.Show("Are you sure to Un_Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val2 = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.Id = Conversion.ToInt(DatGridMainHistory.CurrentRow.Cells["Id"].Value);
				detail.PostDate = DateTime.Now;
				detail.EntryUser = UserAccount.ID;
				detail.ActionTypeId = false;
				detail.ReqType = "UP";
				val2.AllApprovalLists.Add(detail);
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(val2);
				MessageBox.Show("Record Un_Approved Successfully");
				if (chkPrint.Checked)
				{
					CommonServices.DayBookSlip(detail.Id);
				}
				GrdMainfill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatGridMainHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DatGridMainHistory.CurrentRow.Cells["Id"].Value), 8);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDayBook_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				HistoryStack.Remove(this);
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridDebitHistory).Name);
			if (fs != null)
			{
				DataGridDebitHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridCreditHistory).Name);
			if (fs != null)
			{
				DataGridCreditHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatGridMainHistory).Name);
			if (fs != null)
			{
				DatGridMainHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DayBookSlip(int PrintId)
	{
		try
		{
			CommonServices.DayBookSlip(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint144_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocDate = Conversion.ToDateTime((object)datVouchderDateForHistory.Value);
			obj.AccountId = Conversion.ToInt(CmbCashAccountForHistory.Value);
			if (chkZkipCashTransaction.Checked)
			{
				obj.ZeroBalanceType = 1;
			}
			DataTable dt = GeneralReprots.DayBookSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<double>("DebitAmount") > 0.0
				select r;
			DataTable dtDebit = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
			EnumerableRowCollection<DataRow> rowscr = from r in dt.AsEnumerable()
				where r.Field<double>("CreditAmount") > 0.0
				select r;
			DataTable obj2 = (rowscr.Any() ? rowscr.CopyToDataTable() : dt.Clone());
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject DayBookSubReport_DebitTransactions = new SubReportObject(dtDebit, "DayBookDebitSubReport.rpt");
			SubReportObject DayBookSubReport_CreditTransactions = new SubReportObject(obj2, "DayBookCreditSubReport.rpt");
			lstSubReports.Add(DayBookSubReport_DebitTransactions);
			lstSubReports.Add(DayBookSubReport_CreditTransactions);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "144-DayBookSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDayBook_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnapprove_Click(object sender, EventArgs e)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (DatGridMainHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (IsApproved)
			{
				if (MessageBox.Show("Are you sure to Un_Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = DatGridMainHistory.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ApprovalList detail = new ApprovalList();
						detail.OrganizationId = UserAccount.OrganizationId;
						detail.CompanyId = UserAccount.CompanyId;
						detail.Id = Conversion.ToInt(item.Cells["Id"].Value);
						detail.PostDate = DateTime.Now;
						detail.EntryUser = UserAccount.ID;
						detail.ActionTypeId = false;
						detail.ReqType = "UP";
						obj.AllApprovalLists.Add(detail);
					}
					VoucherHead.UpdateStatusandIsapprovedByVoucherId(obj);
					MessageBox.Show("Record Un_Approved Successfully");
					GrdMainfill();
				}
			}
			else if (MessageBox.Show("Are you sure to Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = DatGridMainHistory.GetCheckedRows();
				foreach (GridEXRow item2 in checkedRows)
				{
					ApprovalList detail2 = new ApprovalList();
					detail2.OrganizationId = UserAccount.OrganizationId;
					detail2.CompanyId = UserAccount.CompanyId;
					detail2.Id = Conversion.ToInt(item2.Cells["Id"].Value);
					detail2.PostDate = DateTime.Now;
					detail2.EntryUser = UserAccount.ID;
					detail2.ActionTypeId = false;
					detail2.ReqType = "AP";
					obj2.AllApprovalLists.Add(detail2);
				}
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(obj2);
				MessageBox.Show("Record Approved Successfully");
				GrdMainfill();
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
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a74: Expected O, but got Unknown
		//IL_0a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8b: Expected O, but got Unknown
		//IL_0d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d58: Expected O, but got Unknown
		//IL_0d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Expected O, but got Unknown
		GridEXLayout DataGridDebitHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridCreditHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatGridMainHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.DataGridDebitHistory = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.DataGridCreditHistory = new GridEX();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DatGridMainHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.btnapprove = new System.Windows.Forms.Button();
		this.chkDateEnable = new System.Windows.Forms.CheckBox();
		this.BtnShow = new System.Windows.Forms.Button();
		this.chkZkipCashTransaction = new System.Windows.Forms.CheckBox();
		this.label14 = new System.Windows.Forms.Label();
		this.datVouchderDateForHistory = new System.Windows.Forms.DateTimePicker();
		this.CmbCashAccountForHistory = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrint144 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridDebitHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridCreditHistory).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatGridMainHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.panel11.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCashAccountForHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1334, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1334, 661);
		this.tabControl1.TabIndex = 0;
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1326, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "DayBook Approval";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.panel8);
		this.panel13.Controls.Add(this.panel12);
		this.panel13.Controls.Add(this.panel5);
		this.panel13.Controls.Add(this.panel11);
		this.panel13.Controls.Add(this.panel10);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1320, 626);
		this.panel13.TabIndex = 0;
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel8.Location = new System.Drawing.Point(627, 360);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(688, 266);
		this.panel8.TabIndex = 527;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridDebitHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(688, 236);
		this.panel9.TabIndex = 49;
		this.DataGridDebitHistory.AllowEdit = (InheritableBoolean)2;
		DataGridDebitHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridDebitHistory.DesignTimeLayout = DataGridDebitHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridDebitHistory.DynamicFiltering = true;
		this.DataGridDebitHistory.FilterMode = (FilterMode)2;
		this.DataGridDebitHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridDebitHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Name = "DataGridDebitHistory";
		((ScrollableBase)this.DataGridDebitHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Size = new System.Drawing.Size(688, 236);
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).TabIndex = 4;
		this.DataGridDebitHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridDebitHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridDebitHistory).VisualStyle = (VisualStyle)4;
		this.DataGridDebitHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridDebitHistory_ColumnButtonClick);
		this.DataGridDebitHistory.LinkClicked += new ColumnActionEventHandler(DataGridDebitHistory_LinkClicked);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(639, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridDebitHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 48;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(0, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(688, 30);
		this.label6.TabIndex = 5;
		this.label6.Text = "Debit Transactions";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel12.Controls.Add(this.panel7);
		this.panel12.Controls.Add(this.ctrlGrdBar3);
		this.panel12.Controls.Add(this.label5);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel12.Location = new System.Drawing.Point(0, 360);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(627, 266);
		this.panel12.TabIndex = 526;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridCreditHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 30);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(627, 236);
		this.panel7.TabIndex = 50;
		this.DataGridCreditHistory.AllowEdit = (InheritableBoolean)2;
		DataGridCreditHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridCreditHistory.DesignTimeLayout = DataGridCreditHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridCreditHistory.DynamicFiltering = true;
		this.DataGridCreditHistory.FilterMode = (FilterMode)2;
		this.DataGridCreditHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Name = "DataGridCreditHistory";
		((ScrollableBase)this.DataGridCreditHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Size = new System.Drawing.Size(627, 236);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).TabIndex = 3;
		this.DataGridCreditHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridCreditHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridCreditHistory).VisualStyle = (VisualStyle)4;
		this.DataGridCreditHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridCreditHistory_ColumnButtonClick);
		this.DataGridCreditHistory.LinkClicked += new ColumnActionEventHandler(DataGridCreditHistory_LinkClicked);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(583, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridCreditHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar3.TabIndex = 49;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(0, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(627, 30);
		this.label5.TabIndex = 4;
		this.label5.Text = "Credit Transactions";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DatGridMainHistory);
		this.panel5.Controls.Add(this.panel2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 144);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1320, 216);
		this.panel5.TabIndex = 524;
		this.DatGridMainHistory.AllowEdit = (InheritableBoolean)2;
		DatGridMainHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatGridMainHistory.DesignTimeLayout = DatGridMainHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatGridMainHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatGridMainHistory.DynamicFiltering = true;
		this.DatGridMainHistory.FilterMode = (FilterMode)2;
		this.DatGridMainHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatGridMainHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DatGridMainHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.DatGridMainHistory).Name = "DatGridMainHistory";
		((ScrollableBase)this.DatGridMainHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatGridMainHistory).Size = new System.Drawing.Size(1320, 191);
		((System.Windows.Forms.Control)(object)this.DatGridMainHistory).TabIndex = 524;
		this.DatGridMainHistory.TotalRow = (InheritableBoolean)1;
		this.DatGridMainHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatGridMainHistory).VisualStyle = (VisualStyle)4;
		this.DatGridMainHistory.ColumnButtonClick += new ColumnActionEventHandler(DatGridMainHistory_ColumnButtonClick);
		this.DatGridMainHistory.SelectionChanged += new System.EventHandler(DatGridMainHistory_SelectionChanged);
		this.DatGridMainHistory.LinkClicked += new ColumnActionEventHandler(DatGridMainHistory_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1320, 25);
		this.panel2.TabIndex = 523;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1270, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.DatGridMainHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar2.TabIndex = 51;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(119, 17);
		this.label1.TabIndex = 25;
		this.label1.Text = "Filtered Records";
		this.panel11.BackColor = System.Drawing.Color.White;
		this.panel11.Controls.Add(this.groupBox1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 57);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1320, 87);
		this.panel11.TabIndex = 523;
		this.groupBox1.Controls.Add(this.chkPrint);
		this.groupBox1.Controls.Add(this.btnapprove);
		this.groupBox1.Controls.Add(this.chkDateEnable);
		this.groupBox1.Controls.Add(this.BtnShow);
		this.groupBox1.Controls.Add(this.chkZkipCashTransaction);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.datVouchderDateForHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(758, 83);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.Location = new System.Drawing.Point(496, 20);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(67, 19);
		this.chkPrint.TabIndex = 523;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.btnapprove.BackColor = System.Drawing.Color.Teal;
		this.btnapprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnapprove.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnapprove.ForeColor = System.Drawing.Color.White;
		this.btnapprove.Location = new System.Drawing.Point(387, 45);
		this.btnapprove.Name = "btnapprove";
		this.btnapprove.Size = new System.Drawing.Size(92, 26);
		this.btnapprove.TabIndex = 522;
		this.btnapprove.Text = "Approve";
		this.btnapprove.UseVisualStyleBackColor = false;
		this.btnapprove.Click += new System.EventHandler(btnapprove_Click);
		this.chkDateEnable.AutoSize = true;
		this.chkDateEnable.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.chkDateEnable.Location = new System.Drawing.Point(302, 22);
		this.chkDateEnable.Name = "chkDateEnable";
		this.chkDateEnable.Size = new System.Drawing.Size(15, 14);
		this.chkDateEnable.TabIndex = 521;
		this.chkDateEnable.UseVisualStyleBackColor = true;
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(324, 45);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(61, 26);
		this.BtnShow.TabIndex = 519;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.chkZkipCashTransaction.AutoSize = true;
		this.chkZkipCashTransaction.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkZkipCashTransaction.Location = new System.Drawing.Point(496, 49);
		this.chkZkipCashTransaction.Name = "chkZkipCashTransaction";
		this.chkZkipCashTransaction.Size = new System.Drawing.Size(259, 19);
		this.chkZkipCashTransaction.TabIndex = 520;
		this.chkZkipCashTransaction.Text = "Preview Without Cash Account Transactions";
		this.chkZkipCashTransaction.UseVisualStyleBackColor = true;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(6, 22);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(79, 15);
		this.label14.TabIndex = 465;
		this.label14.Text = "Voucher Date";
		this.datVouchderDateForHistory.CustomFormat = "dd-MMM-yyyy";
		this.datVouchderDateForHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datVouchderDateForHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datVouchderDateForHistory.Location = new System.Drawing.Point(90, 18);
		this.datVouchderDateForHistory.Name = "datVouchderDateForHistory";
		this.datVouchderDateForHistory.Size = new System.Drawing.Size(206, 23);
		this.datVouchderDateForHistory.TabIndex = 464;
		this.CmbCashAccountForHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCashAccountForHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCashAccountForHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCashAccountForHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance4;
		((SpecialBoxBase)((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance9).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCashAccountForHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Location = new System.Drawing.Point(90, 45);
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Name = "CmbCashAccountForHistory";
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).TabIndex = 517;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(6, 51);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(80, 15);
		this.label15.TabIndex = 518;
		this.label15.Text = "Cash Account";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label51);
		this.panel10.Controls.Add(this.toolStrip1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1320, 57);
		this.panel10.TabIndex = 522;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(3, 32);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(175, 19);
		this.label51.TabIndex = 25;
		this.label51.Text = "DAY BOOK HISTORY";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnPrint144 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1320, 25);
		this.toolStrip1.TabIndex = 6;
		this.toolStrip1.Text = "toolStrip1";
		this.btnPrint144.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint144.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint144.Name = "btnPrint144";
		this.btnPrint144.Size = new System.Drawing.Size(79, 24);
		this.btnPrint144.Text = "144-Print";
		this.btnPrint144.Visible = false;
		this.btnPrint144.Click += new System.EventHandler(btnPrint144_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1334, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmDayBookApproval";
		this.Text = "Day Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmDayBook_FormClosing);
		base.Load += new System.EventHandler(frmDayBook_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDayBook_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridDebitHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridCreditHistory).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatGridMainHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCashAccountForHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
