using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.DAW;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.ModernHistories;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.MultiApprovalDashboard;

public class frmVoucherApproval : Form
{
	private DropDownBind DDL = new DropDownBind();

	public static int Id;

	public bool flagAproved;

	public int DocumentTypeId;

	private bool AllowEditOnVoucherNoinReports;

	private DataTable dtApproval = new DataTable();

	private DataTable dtUnApproval = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar3;

	private Button btnApproved;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPendingVouchersForView;

	internal GridEX grdPendingViewVoucher;

	private Button btnPendingForView;

	private Panel panel7;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label27;

	private Label label33;

	private Label label3;

	private Label label4;

	private TextBox txtdocnoto;

	private TextBox txtdocnofrom;

	private Button btnShow;

	private CheckBox ChkPrint;

	private TabControl tabControl2;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private Panel panel8;

	private Panel panel2;

	private Panel panel9;

	private TabPage tabPage4;

	private Panel panel5;

	private Panel panel4;

	private Label label1;

	private Panel panel10;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar4;

	private RichTextBox txtComments;

	private Label label2;

	internal GridEX grdDetail;

	private Panel panel11;

	private Label label5;

	private Label label6;

	private Panel panel12;

	internal GridEX grddetailpendingforview;

	private Panel panel13;

	private Label label7;

	private CtrlGrdBar ctrlGrdBar5;

	private Panel panel15;

	private Label label9;

	private Label label8;

	private Button btnApprovePendForview;

	private Panel panel14;

	private Label label10;

	public frmVoucherApproval(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PendingApprovalVouchersHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 710);
			tabControl2.TabPages.Remove(tabPage3);
			tabControl2.TabPages.Remove(tabPage4);
			fromdate.Focus();
			dtApproval.Columns.Add("Id");
			dtApproval.Columns.Add("DocumentTypeId");
			dtApproval.Columns.Add("DocumentTypeCode");
			dtApproval.Columns.Add("DocumentTypeSrNo");
			dtApproval.Columns.Add("VoucherCode", typeof(int));
			dtApproval.Columns.Add("VoucherDate", typeof(DateTime));
			dtApproval.Columns.Add("RefAccountId");
			dtApproval.Columns.Add("AccountTitle");
			dtApproval.Columns.Add("Remarks");
			dtApproval.Columns.Add("VoucherAmount", typeof(double));
			dtApproval.Columns.Add("ChequeNo");
			dtApproval.Columns.Add("ChequeDate");
			dtApproval.Columns.Add("PayTitle");
			dtApproval.Columns.Add("EntryUser");
			dtApproval.Columns.Add("IsApproved");
			dtApproval.Columns.Add("FinalApprovalUser");
			dtApproval.Columns.Add("IsFinalStatus");
			dtApproval.Columns.Add("IsMandatoryUsers");
			dtApproval.Columns.Add("IsMandatoryStatus");
			dtApproval.Columns.Add("NoOfAttachments");
			dtUnApproval.Columns.Add("Id");
			dtUnApproval.Columns.Add("DocumentTypeId");
			dtUnApproval.Columns.Add("DocumentTypeCode");
			dtUnApproval.Columns.Add("DocumentTypeSrNo");
			dtUnApproval.Columns.Add("VoucherCode", typeof(int));
			dtUnApproval.Columns.Add("VoucherDate", typeof(DateTime));
			dtUnApproval.Columns.Add("RefAccountId");
			dtUnApproval.Columns.Add("AccountTitle");
			dtUnApproval.Columns.Add("Remarks");
			dtUnApproval.Columns.Add("VoucherAmount", typeof(double));
			dtUnApproval.Columns.Add("ChequeNo");
			dtUnApproval.Columns.Add("ChequeDate");
			dtUnApproval.Columns.Add("PayTitle");
			dtUnApproval.Columns.Add("EntryUser");
			dtUnApproval.Columns.Add("IsApproved");
			dtUnApproval.Columns.Add("NoOfAttachments");
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllowEditOnVoucherNoinReports"));
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			dtViewAttachments.Columns.Add("AttchmentPath");
			DocumentTypeId = Conversion.ToInt(base.Tag);
			btnPendingForView.Visible = true;
			btnApproved.Text = "Approve";
			label23.Text = "Vouchers For Approval";
			PendingForViewVouchers(Id);
			GetUnapprovedRecord(Id);
			TabControlCaption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetUnapprovedRecord(int Id)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			dtApproval.Rows.Clear();
			DataTable dt = VoucherHead.VoucherForApprovalDashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				UserId = UserAccount.ID,
				PendingForViewFilter = "All",
				IsApproved = true
			});
			if (dt.Rows.Count > 0)
			{
				dtApproval.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtApproval.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["DocumentTypeCode"], dt.Rows[i]["DocumentTypeSrNo"], dt.Rows[i]["Vouchercode"], Conversion.ToDateTime(dt.Rows[i]["VoucherDate"]).ToString("dd-MMM-yy"), dt.Rows[i]["RefAccountId"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["Remarks"], dt.Rows[i]["VoucherAmount"], dt.Rows[i]["ChequeNo"], Conversion.ToDateTime(dt.Rows[i]["ChequeDate"]).ToString("dd-MMM-yy"), dt.Rows[i]["PayTitle"], dt.Rows[i]["UserName"], dt.Rows[i]["IsApproved"], dt.Rows[i]["FinalApprovalUser"], dt.Rows[i]["IsFinalStatus"], dt.Rows[i]["IsMandatoryUsers"], dt.Rows[i]["IsMandatoryStatus"], dt.Rows[i]["NoOfAttachments"]);
				}
				RecordFilterForApproval();
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

	private void Gridsetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["IsApproved"].Visible = false;
			grd.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grd.RootTable.Columns["RefAccountId"].Visible = false;
			grd.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VoucherAmount"].FormatString = "#,#";
			grd.RootTable.Columns["VoucherAmount"].TotalFormatString = "#,#";
			if (AllowEditOnVoucherNoinReports)
			{
				grd.RootTable.Columns["VoucherCode"].ColumnType = (ColumnType)5;
			}
			grd.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["VoucherCode"].Caption = "V Code";
			grd.RootTable.Columns["DocumentTypeCode"].Caption = "V Type";
			grd.RootTable.Columns["VoucherDate"].Caption = "V Date";
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 40;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].Caption = "Print";
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].Width = 60;
			grd.RootTable.Columns["Print"].Position = 1;
			grd.RootTable.Columns.Add("Detail");
			grd.RootTable.Columns["Detail"].Caption = "Detail";
			grd.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Detail"].ButtonText = "Detail";
			grd.RootTable.Columns["Detail"].Width = 60;
			grd.RootTable.Columns["Detail"].Position = 2;
			grd.FrozenColumns = 3;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar2_Load(null, null);
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
			int Id = Conversion.ToInt(grd.CurrentRow.Cells[0].Value);
			int documentTypeId = Conversion.ToInt(grd.CurrentRow.Cells[1].Value);
			if (e.Column.Key == "Print")
			{
				Print(Id, documentTypeId);
			}
			if (e.Column.Key == "Detail" && e.Column.Key == "Detail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grd.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = DocumentTypeId;
					obj.RefDocNoId = Id;
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "VoucherCode")
			{
				int Id = int.Parse(item.Cells["Id"].Value.ToString());
				CommonServices.EditMethodFromLinked(int.Parse(item.Cells["DocumentTypeId"].Value.ToString()), Id);
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(int.Parse(item.Cells["Id"].Value.ToString()), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
			if (e.Column.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(item.Cells["RefAccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.CurrentRow.Cells.Count > 0)
			{
				GridDetailByVoucherHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdDetail.CurrentColumn != null)
			{
				if (grdDetail.CurrentColumn.Key == "AccountTitle")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(r.Cells["AccountId"].Value.ToString()), fromdate.Value, todate.Value);
				}
				else if (grdDetail.CurrentColumn.Key == "OffsetAccount")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(r.Cells["AgainstAccountId"].Value.ToString()), fromdate.Value, todate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailsetting()
	{
		try
		{
			grdDetail.RootTable.Columns["AccountId"].Visible = false;
			grdDetail.RootTable.Columns["AgainstAccountId"].Visible = false;
			grdDetail.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["OffsetAccount"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DebitAmount"].FormatString = "#,#";
			grdDetail.RootTable.Columns["CreditAmount"].FormatString = "#,#";
			grdDetail.RootTable.Columns["DebitAmount"].TotalFormatString = "#,#";
			grdDetail.RootTable.Columns["CreditAmount"].TotalFormatString = "#,#";
			CommonServices.GridAutoAdjustment(AryColWidth, grdDetail);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (e.Column.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(item.Cells["AccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
			if (e.Column.Key == "OffsetAccount")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(item.Cells["AgainstAccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailByVoucherHeadId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grd.CurrentRow != null && (int)grd.CurrentRow.RowType == 0 && grd.CurrentRow.Cells.Count > 0)
			{
				DataTable dt = VoucherHead.GetDetailByVoucherHeadId(Conversion.ToInt((object)grd.CurrentRow.Cells["Id"].Value.ToString()));
				if (dt.Rows.Count > 0)
				{
					grdDetail.DataSource = dt;
					grdDetail.RetrieveStructure();
					GridDetailsetting();
				}
				else
				{
					grdDetail.ClearStructure();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		try
		{
			RecordApprovedandPendingForView("Approve");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPendingForView_Click(object sender, EventArgs e)
	{
		try
		{
			RecordApprovedandPendingForView("PendingForView");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RecordApprovedandPendingForView(string ReqStatus)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (ReqStatus == "Approve")
			{
				if (MessageBox.Show("Are you sure to Approve" + label23.Text + " ? ", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (ReqStatus == "PendingForView" && MessageBox.Show("Are you sure to PendingForView" + label23.Text + " ? ", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ApprovalList obj = new ApprovalList();
			obj.AllApprovalLists = new List<ApprovalList>();
			if (txtComments.Text == string.Empty)
			{
				MessageBox.Show("Comments Required For Approval", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtComments.Focus();
				return;
			}
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Comments = txtComments.Text.Trim();
				item.DocumentTypeId = DocumentTypeId;
				item.Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				item.ReqType = ReqStatus;
				item.EntryUser = UserAccount.ID;
				obj.AllApprovalLists.Add(item);
			}
			DocumentApprovalPolicy.MultiApproveAndRejectDocumentByUserId(obj);
			MessageBox.Show("Record" + ReqStatus + " Successfully");
			GetUnapprovedRecord(Id);
			PendingForViewVouchers(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PendingForViewVouchers(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		try
		{
			DataTable lst = VoucherHead.VoucherForApprovalDashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsApproved = flagAproved,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				UserId = UserAccount.ID,
				ActionTypeId = true,
				ApprovedFilter = "All"
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dtPenView = new DataTable();
				dtPenView.Columns.Add("Id");
				dtPenView.Columns.Add("DocumentTypeId");
				dtPenView.Columns.Add("DocumentTypeCode");
				dtPenView.Columns.Add("DocumentTypeSrNo");
				dtPenView.Columns.Add("VoucherCode", typeof(int));
				dtPenView.Columns.Add("VoucherDate", typeof(DateTime));
				dtPenView.Columns.Add("RefAccountId");
				dtPenView.Columns.Add("AccountTitle");
				dtPenView.Columns.Add("Remarks");
				dtPenView.Columns.Add("VoucherAmount", typeof(double));
				dtPenView.Columns.Add("ChequeNo");
				dtPenView.Columns.Add("ChequeDate");
				dtPenView.Columns.Add("PayTitle");
				dtPenView.Columns.Add("EntryUser");
				dtPenView.Columns.Add("IsApproved");
				dtPenView.Columns.Add("FinalApprovalUser");
				dtPenView.Columns.Add("IsFinalStatus");
				dtPenView.Columns.Add("IsMandatoryUsers");
				dtPenView.Columns.Add("IsMandatoryStatus");
				dtPenView.Columns.Add("NoOfAttachments");
				dtPenView.Rows.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtPenView.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], lst.Rows[i]["DocumentTypeSrNo"], lst.Rows[i]["Vouchercode"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yy"), lst.Rows[i]["RefAccountId"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["Remarks"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["ChequeDate"], lst.Rows[i]["PayTitle"], lst.Rows[i]["UserName"], lst.Rows[i]["IsApproved"], lst.Rows[i]["FinalApprovalUser"], lst.Rows[i]["IsFinalStatus"], lst.Rows[i]["IsMandatoryUsers"], lst.Rows[i]["IsMandatoryStatus"], lst.Rows[i]["NoOfAttachments"]);
				}
				grdPendingViewVoucher.DataSource = dtPenView;
				grdPendingViewVoucher.RetrieveStructure();
				GridPendingViewsetting();
			}
			else
			{
				grdPendingViewVoucher.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPendingViewsetting()
	{
		try
		{
			grdPendingViewVoucher.RootTable.Columns["Id"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["IsApproved"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["RefAccountId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["VoucherAmount"].FormatString = "#,#";
			grdPendingViewVoucher.RootTable.Columns["VoucherAmount"].TotalFormatString = "#,#";
			if (AllowEditOnVoucherNoinReports)
			{
				grdPendingViewVoucher.RootTable.Columns["VoucherCode"].ColumnType = (ColumnType)5;
			}
			grdPendingViewVoucher.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["VoucherCode"].Caption = "V Code";
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeCode"].Caption = "V Type";
			grdPendingViewVoucher.RootTable.Columns["VoucherDate"].Caption = "V Date";
			grdPendingViewVoucher.RootTable.Columns.Add("Print");
			grdPendingViewVoucher.RootTable.Columns["Print"].Caption = "Print";
			grdPendingViewVoucher.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingViewVoucher.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdPendingViewVoucher.RootTable.Columns["Print"].ButtonText = "Print";
			grdPendingViewVoucher.RootTable.Columns["Print"].Width = 60;
			grdPendingViewVoucher.RootTable.Columns["Print"].Position = 1;
			grdPendingViewVoucher.RootTable.Columns.Add("Select");
			grdPendingViewVoucher.RootTable.Columns["Select"].ActAsSelector = true;
			grdPendingViewVoucher.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdPendingViewVoucher.RootTable.Columns["Select"].Width = 40;
			grdPendingViewVoucher.RootTable.Columns["Select"].Position = 0;
			grdPendingViewVoucher.RootTable.Columns.Add("Detail");
			grdPendingViewVoucher.RootTable.Columns["Detail"].Caption = "Detail";
			grdPendingViewVoucher.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingViewVoucher.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdPendingViewVoucher.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdPendingViewVoucher.RootTable.Columns["Detail"].Width = 60;
			grdPendingViewVoucher.RootTable.Columns["Detail"].Position = 2;
			grdPendingViewVoucher.FrozenColumns = 3;
			CommonServices.GridAutoAdjustment(AryColWidth, grdPendingViewVoucher);
			ctrlGrdBar4_Load(null, null);
			TabControlCaption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			if (e.Column.Key == "VoucherCode")
			{
				int Id = int.Parse(item.Cells["Id"].Value.ToString());
				CommonServices.EditMethodFromLinked(int.Parse(item.Cells["DocumentTypeId"].Value.ToString()), Id);
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
			if (e.Column.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(item.Cells["RefAccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(grdPendingViewVoucher.CurrentRow.Cells["Id"].Value);
			int documentTypeId = Conversion.ToInt(grdPendingViewVoucher.CurrentRow.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "Print")
			{
				Print(Id, documentTypeId);
			}
			if (e.Column.Key == "Detail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grdPendingViewVoucher.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = DocumentTypeId;
					obj.RefDocNoId = Id;
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailPendingForviewByVoucherHeadId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdPendingViewVoucher.CurrentRow != null && (int)grdPendingViewVoucher.CurrentRow.RowType == 0 && grdPendingViewVoucher.CurrentRow.Cells.Count > 0)
			{
				DataTable dt = VoucherHead.GetDetailByVoucherHeadId(Conversion.ToInt((object)grdPendingViewVoucher.CurrentRow.Cells["Id"].Value.ToString()));
				if (dt.Rows.Count > 0)
				{
					grddetailpendingforview.DataSource = dt;
					grddetailpendingforview.RetrieveStructure();
					GridDetailPendingForviewsetting();
				}
				else
				{
					grdDetail.ClearStructure();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailPendingForviewsetting()
	{
		try
		{
			grddetailpendingforview.RootTable.Columns["AccountId"].Visible = false;
			grddetailpendingforview.RootTable.Columns["AgainstAccountId"].Visible = false;
			grddetailpendingforview.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grddetailpendingforview.RootTable.Columns["OffsetAccount"].ColumnType = (ColumnType)5;
			grddetailpendingforview.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grddetailpendingforview.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grddetailpendingforview.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grddetailpendingforview.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grddetailpendingforview.RootTable.Columns["DebitAmount"].FormatString = "#,#";
			grddetailpendingforview.RootTable.Columns["CreditAmount"].FormatString = "#,#";
			grddetailpendingforview.RootTable.Columns["DebitAmount"].TotalFormatString = "#,#";
			grddetailpendingforview.RootTable.Columns["CreditAmount"].TotalFormatString = "#,#";
			CommonServices.GridAutoAdjustment(AryColWidth, grddetailpendingforview);
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TabControlCaption()
	{
		try
		{
			int rowCount = grd.RowCount;
			tabControl1.TabPages[0].Text = "Pending For Approval (" + rowCount + " rows)";
			int rowCount2 = grdPendingViewVoucher.RowCount;
			tabControl1.TabPages[1].Text = "Pending For View (" + rowCount2 + " rows)";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		TabControlCaption();
	}

	private void grddetailpendingforview_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grddetailpendingforview.CurrentRow;
			if (e.Column.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(item.Cells["AccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
			if (e.Column.Key == "OffsetAccount")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(item.Cells["AgainstAccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdPendingViewVoucher.CurrentRow.Cells.Count > 0)
			{
				GridDetailPendingForviewByVoucherHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprovePendForview_Click(object sender, EventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		try
		{
			if (grdPendingViewVoucher.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Approve" + label23.Text + " ? ", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ApprovalList obj = new ApprovalList();
			obj.AllApprovalLists = new List<ApprovalList>();
			if (txtComments.Text == string.Empty)
			{
				MessageBox.Show("Comments Required For Approval", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtComments.Focus();
				return;
			}
			GridEXRow[] checkedRows = grdPendingViewVoucher.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Comments = txtComments.Text.Trim();
				item.DocumentTypeId = DocumentTypeId;
				item.Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				obj.AllApprovalLists.Add(item);
			}
			DocumentApprovalPolicy.MultiApproveAndRejectDocumentByUserId(obj);
			MessageBox.Show("Record Approve Successfully");
			PendingForViewVouchers(Id);
			tabControl1.SelectedIndex = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PendingApprovalVouchersHistory_FormClosed(object sender, FormClosedEventArgs e)
	{
		Id = 0;
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			RecordFilterForApproval();
			PendingForViewVouchers(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RecordFilterForApproval()
	{
		try
		{
			DataTable dt = new DataTable();
			if (Conversion.ToInt((object)txtdocnofrom.Text) > 0 && Conversion.ToInt((object)txtdocnoto.Text) > 0)
			{
				if (flagAproved)
				{
					EnumerableRowCollection<DataRow> rows = from r in dtUnApproval.AsEnumerable()
						where r.Field<DateTime>("VoucherDate") >= Conversion.ToDateTime((object)fromdate.Value.ToShortDateString()) && r.Field<DateTime>("VoucherDate") <= Conversion.ToDateTime((object)todate.Value.ToShortDateString()) && r.Field<int>("VoucherCode") >= Conversion.ToInt((object)txtdocnofrom.Text) && r.Field<int>("VoucherCode") <= Conversion.ToInt((object)txtdocnoto.Text)
						select r;
					dt = (rows.Any() ? rows.CopyToDataTable() : dtApproval.Clone());
				}
				else
				{
					EnumerableRowCollection<DataRow> rows2 = from r in dtApproval.AsEnumerable()
						where r.Field<DateTime>("VoucherDate") >= Conversion.ToDateTime((object)fromdate.Value.ToShortDateString()) && r.Field<DateTime>("VoucherDate") <= Conversion.ToDateTime((object)todate.Value.ToShortDateString()) && r.Field<int?>("VoucherCode") >= Conversion.ToInt((object)txtdocnofrom.Text) && r.Field<int?>("VoucherCode") <= Conversion.ToInt((object)txtdocnoto.Text)
						select r;
					dt = (rows2.Any() ? rows2.CopyToDataTable() : dtApproval.Clone());
				}
			}
			else if (flagAproved)
			{
				EnumerableRowCollection<DataRow> rows3 = from r in dtUnApproval.AsEnumerable()
					where r.Field<DateTime>("VoucherDate") >= Conversion.ToDateTime((object)fromdate.Value.ToShortDateString()) && r.Field<DateTime>("VoucherDate") <= Conversion.ToDateTime((object)todate.Value.ToShortDateString())
					select r;
				dt = (rows3.Any() ? rows3.CopyToDataTable() : dtApproval.Clone());
			}
			else
			{
				EnumerableRowCollection<DataRow> rows4 = from r in dtApproval.AsEnumerable()
					where r.Field<DateTime>("VoucherDate") >= Conversion.ToDateTime((object)fromdate.Value.ToShortDateString()) && r.Field<DateTime>("VoucherDate") <= Conversion.ToDateTime((object)todate.Value.ToShortDateString())
					select r;
				dt = (rows4.Any() ? rows4.CopyToDataTable() : dtApproval.Clone());
			}
			if (dt.Rows.Count > 0)
			{
				grd.DataSource = dt;
				grd.RetrieveStructure();
				Gridsetting();
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

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl2.SelectedIndex == 1)
		{
			VoucherModernHistory frm = new VoucherModernHistory(UserAccount);
			frm.ApproveOrUnApprove = flagAproved;
			frm.DocumentTypeId = DocumentTypeId;
			frm.RequestFromScreen = "ApproveOrUnApprove Voucher";
			frm.TopLevel = false;
			frm.Visible = true;
			frm.FormBorderStyle = FormBorderStyle.None;
			frm.Dock = DockStyle.Fill;
			tabControl2.TabPages["tabPage3"].Controls.Add(frm);
		}
		else if (tabControl2.SelectedIndex == 2)
		{
			VoucherModernHistory form = new VoucherModernHistory(UserAccount);
			form.ApproveOrUnApprove = flagAproved;
			form.DocumentTypeId = DocumentTypeId;
			form.RequestFromScreen = "Pending For Review";
			form.TopLevel = false;
			form.Visible = true;
			form.FormBorderStyle = FormBorderStyle.None;
			form.Dock = DockStyle.Fill;
			tabControl2.TabPages["tabPage4"].Controls.Add(form);
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingViewVoucher).Name);
			if (fs != null)
			{
				grdPendingViewVoucher.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grddetailpendingforview).Name);
			if (fs != null)
			{
				grddetailpendingforview.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void Print(int id, int documentTypeId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = documentTypeId,
				Id = id,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			int Id = int.Parse(r.Cells["Id"].Value.ToString());
			int documentTypeId = int.Parse(r.Cells["DocumentTypeId"].Value.ToString());
			if (grd.CurrentColumn.Key == "VoucherCode")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.EditMethodFromLinked(documentTypeId, Id);
				}
			}
			else if (grd.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, documentTypeId);
			}
			else if (grd.CurrentColumn.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(r.Cells["RefAccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
			else if (grd.CurrentColumn.Key == "Print")
			{
				Print(Id, documentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingApprovalVouchersHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 0)
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
				{
					((Control)(object)grdDetail).Focus();
				}
				else
				{
					((Control)(object)grd).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				tabControl1.SelectedIndex = 0;
				fromdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					tabControl2.SelectedIndex = 1;
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					tabControl2.SelectedIndex = 2;
				}
				else if (tabControl2.SelectedIndex == 2)
				{
					tabControl2.SelectedIndex = 0;
				}
			}
			if (e.Control && e.KeyCode == Keys.A)
			{
				btnApproved_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPendingForView_Click(null, null);
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

	private void grdPendingViewVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdPendingViewVoucher.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPendingViewVoucher.CurrentColumn == null)
			{
				return;
			}
			int Id = int.Parse(r.Cells["Id"].Value.ToString());
			int DocTypeId = int.Parse(r.Cells["DocumentTypeId"].Value.ToString());
			if (grdPendingViewVoucher.CurrentColumn.Key == "VoucherCode")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.EditMethodFromLinked(DocTypeId, Id);
				}
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocTypeId);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(r.Cells["RefAccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Print")
			{
				Print(Id, DocTypeId);
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
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_0f84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8e: Expected O, but got Unknown
		//IL_0fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbc: Expected O, but got Unknown
		//IL_12b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bb: Expected O, but got Unknown
		//IL_1b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2b: Expected O, but got Unknown
		//IL_1e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8e: Expected O, but got Unknown
		//IL_1eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ebc: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grddetailpendingforview_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingViewVoucher_DesignTimeLayout = new GridEXLayout();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.txtComments = new System.Windows.Forms.RichTextBox();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtdocnoto = new System.Windows.Forms.TextBox();
		this.txtdocnofrom = new System.Windows.Forms.TextBox();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.btnApproved = new System.Windows.Forms.Button();
		this.btnPendingForView = new System.Windows.Forms.Button();
		this.grd = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.tabPendingVouchersForView = new System.Windows.Forms.TabPage();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grddetailpendingforview = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdPendingViewVoucher = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.btnApprovePendForview = new System.Windows.Forms.Button();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel3.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel15.SuspendLayout();
		this.tabPendingVouchersForView.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailpendingforview).BeginInit();
		this.panel13.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).BeginInit();
		this.panel6.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.txtComments);
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Controls.Add(this.btnShow);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtdocnoto);
		this.panel3.Controls.Add(this.txtdocnofrom);
		this.panel3.Controls.Add(this.todate);
		this.panel3.Controls.Add(this.fromdate);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Controls.Add(this.label33);
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 102);
		this.panel3.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(360, 45);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(65, 15);
		this.label2.TabIndex = 131;
		this.label2.Text = "Comments";
		this.txtComments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtComments.Location = new System.Drawing.Point(426, 41);
		this.txtComments.Name = "txtComments";
		this.txtComments.Size = new System.Drawing.Size(281, 50);
		this.txtComments.TabIndex = 4;
		this.txtComments.Text = "";
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.Location = new System.Drawing.Point(713, 44);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(67, 19);
		this.ChkPrint.TabIndex = 5;
		this.ChkPrint.Text = "Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.ChkPrint.Visible = false;
		this.btnShow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShow.Location = new System.Drawing.Point(712, 65);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(68, 26);
		this.btnShow.TabIndex = 6;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(192, 72);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 129;
		this.label3.Text = "Doc No To";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(192, 45);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 128;
		this.label4.Text = "Doc No From";
		this.txtdocnoto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnoto.Location = new System.Drawing.Point(275, 68);
		this.txtdocnoto.Name = "txtdocnoto";
		this.txtdocnoto.Size = new System.Drawing.Size(80, 23);
		this.txtdocnoto.TabIndex = 3;
		this.txtdocnofrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnofrom.Location = new System.Drawing.Point(275, 41);
		this.txtdocnofrom.Name = "txtdocnofrom";
		this.txtdocnofrom.Size = new System.Drawing.Size(80, 23);
		this.txtdocnofrom.TabIndex = 2;
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(74, 68);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(108, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(74, 41);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(108, 23);
		this.fromdate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(8, 45);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 115;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(8, 72);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 114;
		this.label33.Text = "To Date";
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.label23);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 35);
		this.panel7.TabIndex = 111;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(83, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Vouchers";
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(235, 36);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(83, 26);
		this.btnApproved.TabIndex = 6;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.btnPendingForView.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnPendingForView.FlatAppearance.BorderSize = 0;
		this.btnPendingForView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPendingForView.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPendingForView.ForeColor = System.Drawing.Color.White;
		this.btnPendingForView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPendingForView.Location = new System.Drawing.Point(767, 36);
		this.btnPendingForView.Margin = new System.Windows.Forms.Padding(2);
		this.btnPendingForView.Name = "btnPendingForView";
		this.btnPendingForView.Size = new System.Drawing.Size(134, 26);
		this.btnPendingForView.TabIndex = 7;
		this.btnPendingForView.Text = "PendingForView";
		this.btnPendingForView.UseVisualStyleBackColor = false;
		this.btnPendingForView.Click += new System.EventHandler(btnPendingForView_Click);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(936, 301);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPendingVouchersForView);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(950, 627);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel9);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(942, 601);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 370);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(936, 228);
		this.panel5.TabIndex = 2;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(936, 196);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 3;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grdDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label5);
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(936, 32);
		this.panel11.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(58, 19);
		this.label5.TabIndex = 2;
		this.label5.Text = "Detail";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(887, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 69);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(936, 301);
		this.panel9.TabIndex = 5;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.panel15);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.btnApproved);
		this.panel4.Controls.Add(this.btnPendingForView);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(936, 66);
		this.panel4.TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.DarkBlue;
		this.label10.Location = new System.Drawing.Point(447, 41);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(315, 17);
		this.label10.TabIndex = 135;
		this.label10.Text = "Click To Transfer Selected rows In Pending for View";
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label1);
		this.panel15.Controls.Add(this.ctrlGrdBar2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(936, 33);
		this.panel15.TabIndex = 134;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10.75f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(2, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(165, 18);
		this.label1.TabIndex = 2;
		this.label1.Text = "Pending for Approval";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(888, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.DarkBlue;
		this.label9.Location = new System.Drawing.Point(5, 41);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(222, 17);
		this.label9.TabIndex = 133;
		this.label9.Text = "Click To Approve The Selected rows";
		this.tabPendingVouchersForView.Controls.Add(this.panel12);
		this.tabPendingVouchersForView.Controls.Add(this.panel13);
		this.tabPendingVouchersForView.Controls.Add(this.panel10);
		this.tabPendingVouchersForView.Controls.Add(this.panel6);
		this.tabPendingVouchersForView.ForeColor = System.Drawing.SystemColors.ControlText;
		this.tabPendingVouchersForView.Location = new System.Drawing.Point(4, 22);
		this.tabPendingVouchersForView.Name = "tabPendingVouchersForView";
		this.tabPendingVouchersForView.Padding = new System.Windows.Forms.Padding(3);
		this.tabPendingVouchersForView.Size = new System.Drawing.Size(942, 601);
		this.tabPendingVouchersForView.TabIndex = 1;
		this.tabPendingVouchersForView.Text = "PendingVoucherForView";
		this.tabPendingVouchersForView.UseVisualStyleBackColor = true;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailpendingforview);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(3, 375);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(936, 223);
		this.panel12.TabIndex = 51;
		this.grddetailpendingforview.AllowEdit = (InheritableBoolean)2;
		grddetailpendingforview_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailpendingforview.DesignTimeLayout = grddetailpendingforview_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailpendingforview).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailpendingforview.DynamicFiltering = true;
		this.grddetailpendingforview.FilterMode = (FilterMode)2;
		this.grddetailpendingforview.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailpendingforview).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailpendingforview.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddetailpendingforview).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grddetailpendingforview).Name = "grddetailpendingforview";
		this.grddetailpendingforview.NewRowFormatStyle.BackColor = System.Drawing.Color.White;
		this.grddetailpendingforview.NewRowFormatStyle.ForeColor = System.Drawing.Color.White;
		((ScrollableBase)this.grddetailpendingforview).Office2007CustomColor = System.Drawing.Color.White;
		((ScrollableBase)this.grddetailpendingforview).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailpendingforview).Size = new System.Drawing.Size(936, 223);
		((System.Windows.Forms.Control)(object)this.grddetailpendingforview).TabIndex = 47;
		this.grddetailpendingforview.TotalRow = (InheritableBoolean)1;
		this.grddetailpendingforview.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailpendingforview).VisualStyle = (VisualStyle)4;
		this.grddetailpendingforview.LinkClicked += new ColumnActionEventHandler(grddetailpendingforview_LinkClicked);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label7);
		this.panel13.Controls.Add(this.ctrlGrdBar5);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 346);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(936, 29);
		this.panel13.TabIndex = 50;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10.75f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(7, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(53, 18);
		this.label7.TabIndex = 24;
		this.label7.Text = "Detail";
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(888, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.grddetailpendingforview;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar5.TabIndex = 23;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 69);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(936, 277);
		this.panel10.TabIndex = 49;
		this.grdPendingViewVoucher.AllowEdit = (InheritableBoolean)2;
		grdPendingViewVoucher_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingViewVoucher.DesignTimeLayout = grdPendingViewVoucher_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingViewVoucher.DynamicFiltering = true;
		this.grdPendingViewVoucher.FilterMode = (FilterMode)2;
		this.grdPendingViewVoucher.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingViewVoucher.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Name = "grdPendingViewVoucher";
		this.grdPendingViewVoucher.NewRowFormatStyle.BackColor = System.Drawing.Color.White;
		this.grdPendingViewVoucher.NewRowFormatStyle.ForeColor = System.Drawing.Color.White;
		((ScrollableBase)this.grdPendingViewVoucher).Office2007CustomColor = System.Drawing.Color.White;
		((ScrollableBase)this.grdPendingViewVoucher).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Size = new System.Drawing.Size(936, 277);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).TabIndex = 0;
		this.grdPendingViewVoucher.TotalRow = (InheritableBoolean)1;
		this.grdPendingViewVoucher.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingViewVoucher).VisualStyle = (VisualStyle)4;
		this.grdPendingViewVoucher.ColumnButtonClick += new ColumnActionEventHandler(grdPendingViewVoucher_ColumnButtonClick);
		this.grdPendingViewVoucher.SelectionChanged += new System.EventHandler(grdPendingViewVoucher_SelectionChanged);
		this.grdPendingViewVoucher.LinkClicked += new ColumnActionEventHandler(grdPendingViewVoucher_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingViewVoucher_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.label8);
		this.panel6.Controls.Add(this.btnApprovePendForview);
		this.panel6.Controls.Add(this.panel14);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(936, 66);
		this.panel6.TabIndex = 48;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.DarkBlue;
		this.label8.Location = new System.Drawing.Point(12, 41);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(222, 17);
		this.label8.TabIndex = 132;
		this.label8.Text = "Click To Approve The Selected rows";
		this.btnApprovePendForview.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApprovePendForview.FlatAppearance.BorderSize = 0;
		this.btnApprovePendForview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApprovePendForview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprovePendForview.ForeColor = System.Drawing.Color.White;
		this.btnApprovePendForview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApprovePendForview.Location = new System.Drawing.Point(239, 36);
		this.btnApprovePendForview.Margin = new System.Windows.Forms.Padding(2);
		this.btnApprovePendForview.Name = "btnApprovePendForview";
		this.btnApprovePendForview.Size = new System.Drawing.Size(79, 26);
		this.btnApprovePendForview.TabIndex = 132;
		this.btnApprovePendForview.Text = "Approve";
		this.btnApprovePendForview.UseVisualStyleBackColor = false;
		this.btnApprovePendForview.Click += new System.EventHandler(btnApprovePendForview_Click);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label6);
		this.panel14.Controls.Add(this.ctrlGrdBar4);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(936, 34);
		this.panel14.TabIndex = 25;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 10.75f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(6, 9);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(222, 18);
		this.label6.TabIndex = 24;
		this.label6.Text = "Pending for View && Approval";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(888, 3);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdPendingViewVoucher;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar4.TabIndex = 23;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 761);
		this.panel1.TabIndex = 0;
		this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(964, 761);
		this.tabControl2.TabIndex = 48;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 735);
		this.tabPage2.TabIndex = 0;
		this.tabPage2.Text = "Grid View";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel2);
		this.panel8.Controls.Add(this.panel3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 729);
		this.panel8.TabIndex = 0;
		this.panel2.Controls.Add(this.tabControl1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 102);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 627);
		this.panel2.TabIndex = 6;
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(956, 735);
		this.tabPage3.TabIndex = 1;
		this.tabPage3.Text = "Cards View";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.tabPage4.Location = new System.Drawing.Point(4, 4);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(956, 735);
		this.tabPage4.TabIndex = 2;
		this.tabPage4.Text = "Pending For Review";
		this.tabPage4.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 761);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmVoucherApproval";
		this.Text = "Approvals";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(PendingApprovalVouchersHistory_FormClosed);
		base.Load += new System.EventHandler(PendingApprovalVouchersHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PendingApprovalVouchersHistory_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.tabPendingVouchersForView.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetailpendingforview).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
