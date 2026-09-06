using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.ModernHistories;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ModernHistoryModels;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.ModernHistories
{

public class VoucherModernHistory : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtHeaderCard = new DataTable();

	public string RequestFromScreen;

	public int DocumentTypeId;

	public bool ApproveOrUnApprove;

	private bool FiltersVisibilty;

	private int VoucherIdForEdit;

	private List<int> ListForApproveOrUnapprove = new List<int>();

	private double VoucherTotalAmount;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnLoadAll;

	private ToolStripButton btnEdit;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnApprove;

	private ToolStripLabel ApprovedCount;

	private ToolStripButton btnPendingForView;

	private ToolStripLabel PendingForReviewCount;

	private Panel panel2;

	private Panel panel3;

	private Label CaptionForVoucherHistory;

	private Panel panel4;

	private Panel FilterButtonPanel;

	private Button FilterRecords;

	private PictureBox FilterBoxIcon;

	private Label HandleVisibilityOfFiletrPanel;

	private Panel FilterPanel;

	private Label label64;

	internal UltraCombo cmbSupplierName;

	private TextBox txtNumberTo;

	private TextBox txtNoFrom;

	private Label label63;

	private Label label62;

	private Label label60;

	private DateTimePicker DateTo;

	private Label label61;

	private DateTimePicker DateFrom;

	private FlowLayoutPanel HeaderFlowLayoutPanel;

	private Panel TotalRecordPanel;

	private Label label5;

	private Label TotalHeaderRecords;

	private Panel VoucherPdfReportDataPanel;

	private Panel panel5;

	private Panel panel6;

	private Label lblHeaderAccountTitle;

	private Label lblChequeDate;

	private Label lblChequeNumber;

	private Label lblPayeeTitle;

	private Label label12;

	private Label label13;

	private Label label14;

	private Label lblVoucherCode;

	private Label lblVoucherDate;

	private Label lblVoucherType;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label ApprovalUserName;

	private Label ModifyUserName;

	private Label EntryUserName;

	private Label lblApprovalUser;

	private Label lblModifyUser;

	private Label label18;

	private Label lblRemarks;

	private Label label16;

	private Panel panel7;

	private Panel panel9;

	private FlowLayoutPanel VoucherDetailCardsFlowLayout;

	private Label lblTotalCreditAmount;

	private Label lblTotalDebitAmount;

	private Label label20;

	private Panel panel8;

	private Label label25;

	private Label label26;

	private Label label27;

	private Label label28;

	private Label label29;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel panel17;

	private Button button7;

	private Label label65;

	private Label label66;

	private Label label67;

	private Label label68;

	private Label label69;

	private Panel panel18;

	private Button button8;

	private Label label70;

	private Label label71;

	private Label label72;

	private Label label73;

	private Label label74;

	private Panel panel19;

	private Button button9;

	private Label label75;

	private Label label76;

	private Label label77;

	private Label label78;

	private Label label79;

	private Panel panel20;

	private Button button10;

	private Label label80;

	private Label label81;

	private Label label82;

	private Label label83;

	private Label label84;

	private Panel panel21;

	private Button button11;

	private Label label85;

	private Label label86;

	private Label label87;

	private Label label88;

	private Label label89;

	private Panel panel22;

	private Button button12;

	private Label label90;

	private Label label91;

	private Label label92;

	private Label label93;

	private Label label94;

	private Panel panel23;

	private Button button13;

	private Label label95;

	private Label label96;

	private Label label97;

	private Label label98;

	private Label label99;

	private Panel panel11;

	private Panel panel12;

	private Panel panel13;

	private Label GrandTotal;

	private Label label40;

	private Panel panel24;

	private Label label43;

	private Label label42;

	private PictureBox pictureBox2;

	private Label label41;

	private ToolStripButton btnVouvher102;

	private ToolStripButton btnCheckAll;

	public VoucherModernHistory(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void VoucherModernHistory_Load(object sender, EventArgs e)
	{
		try
		{
			panel1.AutoScroll = true;
			panel1.HorizontalScroll.Visible = true;
			panel1.HorizontalScroll.Enabled = true;
			if (DocumentTypeId == 1)
			{
				CaptionForVoucherHistory.Text = "CASH PAYMENT VOUCHER HISTORY";
			}
			else if (DocumentTypeId == 2)
			{
				CaptionForVoucherHistory.Text = "BANK PAYMENT VOUCHER HISTORY";
			}
			else if (DocumentTypeId == 3)
			{
				CaptionForVoucherHistory.Text = "CASH RECEIPT VOUCHER HISTORY";
			}
			else if (DocumentTypeId == 4)
			{
				CaptionForVoucherHistory.Text = "BANK RECEIPT VOUCHER HISTORY";
			}
			else if (DocumentTypeId == 5)
			{
				CaptionForVoucherHistory.Text = "JOURNAL VOUCHER HISTORY";
			}
			else if (DocumentTypeId == 6)
			{
				CaptionForVoucherHistory.Text = "PAYABLE BILL VOUCHER HISTORY";
			}
			else if (DocumentTypeId == 7)
			{
				CaptionForVoucherHistory.Text = "RECEIVABLE BILL VOUCHER HISTORY";
			}
			else if (DocumentTypeId == 10)
			{
				CaptionForVoucherHistory.Text = "CONTRA VOUCHER HISTORY";
			}
			else if (DocumentTypeId == 26)
			{
				CaptionForVoucherHistory.Text = "EXPENSE VOUCHER HISTORY";
			}
			else if (DocumentTypeId == 27)
			{
				CaptionForVoucherHistory.Text = "EXPORT INVOICE VOUCHER HISTORY";
			}
			if (RequestFromScreen == "ApproveOrUnApprove Voucher" || RequestFromScreen == "Pending For Review")
			{
				btnApprove.Visible = true;
				ApprovedCount.Visible = true;
				if (RequestFromScreen == "ApproveOrUnApprove Voucher")
				{
					btnPendingForView.Visible = true;
					PendingForReviewCount.Visible = true;
				}
				else
				{
					btnPendingForView.Visible = false;
					PendingForReviewCount.Visible = false;
				}
				btnEdit.Visible = false;
				if (ApproveOrUnApprove)
				{
					btnApprove.Text = "UnApprove";
					btnApprove.Image = Resources.close_line;
					CaptionForVoucherHistory.Text += " FOR UNAPPROVAL";
				}
				else
				{
					btnApprove.Text = "Approve";
					btnApprove.Image = Resources.check_line;
					CaptionForVoucherHistory.Text += " FOR APPROVAL";
				}
			}
			FiltersVisibilty = false;
			FilterBoxIcon.Image = Resources.arrow_down_s_line;
			FilterPanel.Hide();
			FilterRecords.Hide();
			DateFrom.Value = clsGlobalVariables.ActiveYr.Start_Period;
			DateTo.Value = DateTime.Now;
			GenerateHeaderCardDataOnLoad();
			VoucherPdfReportDataPanel.Hide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateHeaderCardDataOnLoad()
	{
		try
		{
			dtHeaderCard.Rows.Clear();
			if (RequestFromScreen == "ApproveOrUnApprove Voucher")
			{
				dtHeaderCard = ModernHistory.VouchersModernHisotry_Header(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, DocumentTypeId.ToString(), true, UserAccount.ID, 21, "", ApproveOrUnApprove);
				if (dtHeaderCard.AsEnumerable().Count((DataRow r) => !r.Field<bool>("ActionTypeId")) > 0)
				{
					dtHeaderCard = (from r in dtHeaderCard.AsEnumerable()
						where !r.Field<bool>("ActionTypeId")
						select r).CopyToDataTable();
				}
				else
				{
					dtHeaderCard.Rows.Clear();
					HeaderFlowLayoutPanel.Controls.Clear();
				}
			}
			else if (RequestFromScreen == "Pending For Review")
			{
				dtHeaderCard = ModernHistory.VouchersModernHisotry_Header(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, DocumentTypeId.ToString(), true, UserAccount.ID, 21, "", ApproveOrUnApprove);
				if (dtHeaderCard.AsEnumerable().Count((DataRow r) => r.Field<bool>("ActionTypeId")) > 0)
				{
					dtHeaderCard = (from r in dtHeaderCard.AsEnumerable()
						where r.Field<bool>("ActionTypeId")
						select r).CopyToDataTable();
				}
				else
				{
					dtHeaderCard.Rows.Clear();
					HeaderFlowLayoutPanel.Controls.Clear();
				}
			}
			else
			{
				dtHeaderCard = ModernHistory.VouchersModernHisotry_Header(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, DocumentTypeId.ToString(), true, UserAccount.ID, 21, "All", false);
			}
			if (dtHeaderCard.Rows.Count > 0)
			{
				GenerateHeaderCards(dtHeaderCard);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateHeaderCards(DataTable dt)
	{
		try
		{
			HeaderFlowLayoutPanel.Controls.Clear();
			VoucherInfoHeaderCard[] ItemHeaderCardsList = new VoucherInfoHeaderCard[dt.Rows.Count];
			for (int i = 0; i < ItemHeaderCardsList.Length; i++)
			{
				ItemHeaderCardsList[i] = new VoucherInfoHeaderCard();
				ItemHeaderCardsList[i].RequestFrom = RequestFromScreen;
				ItemHeaderCardsList[i].VoucherId = Conversion.ToInt(dt.Rows[i]["VoucherHeadId"]);
				ItemHeaderCardsList[i].VoucherNumber = Conversion.ToString(dt.Rows[i]["VoucherCode"]);
				ItemHeaderCardsList[i].VoucherDate = Conversion.ToDateTime(dt.Rows[i]["VoucherDate"]).ToShortDateString();
				ItemHeaderCardsList[i].PartyName = Conversion.ToString(dt.Rows[i]["HeaderAccountTitle"]);
				if (!Conversion.ToBool(dt.Rows[i]["IsApproved"]))
				{
					ItemHeaderCardsList[i].ApprovedOrNot = "Not Approved";
				}
				else if (Conversion.ToBool(dt.Rows[i]["IsApproved"]))
				{
					ItemHeaderCardsList[i].ApprovedOrNot = "Approved";
				}
				ItemHeaderCardsList[i].VoucherType = dt.Rows[i]["DocumentType"].ToString();
				VoucherTotalAmount = Conversion.ToDouble(dt.Rows[i]["VoucherAmount"]);
				ItemHeaderCardsList[i].VoucherAmount = VoucherTotalAmount.ToString("0,0");
				HeaderFlowLayoutPanel.Controls.Add(ItemHeaderCardsList[i]);
				ItemHeaderCardsList[i].UserControlClicked += HeaderCard_Click;
			}
			TotalHeaderRecords.Text = Conversion.ToString((object)dt.Rows.Count);
			PartyComboFill(dt);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HeaderCard_Click(object sender, EventArgs e)
	{
		try
		{
			VoucherInfoHeaderCard obj = (VoucherInfoHeaderCard)sender;
			VoucherIdForEdit = obj.VoucherId;
			VoucherTotalAmount = Conversion.ToDouble((object)obj.VoucherAmount);
			GrandTotal.Text = VoucherTotalAmount.ToString("0,0");
			GenerateVoucherHistoryAgainstVoucherID(obj.VoucherId);
			if ((!(RequestFromScreen == "ApproveOrUnApprove Voucher") && !(RequestFromScreen == "Pending For Review")) || !(Conversion.ToString(obj.Tag) == "FromCheckBox"))
			{
				return;
			}
			if (!obj.IsSelected)
			{
				int id = 0;
				if (ListForApproveOrUnapprove.Count > 0)
				{
					id = ListForApproveOrUnapprove.Find((int Id) => Id == obj.VoucherId);
				}
				if (id > 0)
				{
					ListForApproveOrUnapprove.Remove(id);
				}
				ApprovedCount.Text = Conversion.ToString((object)ListForApproveOrUnapprove.Count);
				PendingForReviewCount.Text = Conversion.ToString((object)ListForApproveOrUnapprove.Count);
				return;
			}
			int id2 = 0;
			if (ListForApproveOrUnapprove.Count > 0)
			{
				id2 = ListForApproveOrUnapprove.Find((int Id) => Id == obj.VoucherId);
			}
			if (id2 > 0)
			{
				ListForApproveOrUnapprove.Remove(id2);
			}
			else
			{
				ListForApproveOrUnapprove.Add(obj.VoucherId);
			}
			ApprovedCount.Text = Conversion.ToString((object)ListForApproveOrUnapprove.Count);
			PendingForReviewCount.Text = Conversion.ToString((object)ListForApproveOrUnapprove.Count);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyComboFill(DataTable dt)
	{
		try
		{
			DataTable dtParty = new DataTable();
			dtParty.Columns.Add("Id", typeof(int));
			dtParty.Columns.Add("Name");
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			int i;
			for (i = 0; i < dt.Rows.Count; i++)
			{
				if (i == 0)
				{
					dtParty.Rows.Add(Conversion.ToInt(dt.Rows[i]["RefAccountId"]), dt.Rows[i]["HeaderAccountTitle"]);
				}
				if ((from c in dtParty.AsEnumerable()
					where c.Field<int>("Id").Equals(Conversion.ToInt(dt.Rows[i]["RefAccountId"]))
					select c).Count() <= 0)
				{
					dtParty.Rows.Add(Conversion.ToInt(dt.Rows[i]["RefAccountId"]), dt.Rows[i]["HeaderAccountTitle"]);
				}
			}
			DDL.BindDDLNew(dtParty, cmbSupplierName, "Id", "Name", "Party Name", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateVoucherHistoryAgainstVoucherID(int VoucherId)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			VoucherPdfReportDataPanel.Show();
			VouchersModernHistory VoucherHistory = new VouchersModernHistory();
			VoucherHistory = ModernHistory.VouchersModernHisotry(UserAccount.OrganizationId, UserAccount.CompanyId, VoucherId, DocumentTypeId.ToString());
			if (VoucherHistory == null)
			{
				return;
			}
			btnAttachment.Text = VoucherHistory.VoucherHistoryHeader.NoOfAttachments.ToString();
			lblHeaderAccountTitle.Text = VoucherHistory.VoucherHistoryHeader.HeaderAccountTitle;
			lblVoucherType.Text = VoucherHistory.VoucherHistoryHeader.DocumentType;
			lblVoucherDate.Text = VoucherHistory.VoucherHistoryHeader.VoucherDate.ToShortDateString();
			lblVoucherCode.Text = VoucherHistory.VoucherHistoryHeader.VoucherCode.ToString();
			lblPayeeTitle.Text = VoucherHistory.VoucherHistoryHeader.PayTitle;
			lblChequeNumber.Text = VoucherHistory.VoucherHistoryHeader.ChequeNo;
			lblChequeDate.Text = VoucherHistory.VoucherHistoryHeader.ChequeDate.ToShortDateString();
			EntryUserName.Text = VoucherHistory.VoucherHistoryHeader.EntryUserName;
			if (VoucherHistory.VoucherHistoryHeader.ModifyUser > 0)
			{
				lblModifyUser.Show();
				ModifyUserName.Show();
				ModifyUserName.Text = VoucherHistory.VoucherHistoryHeader.ModifyUserName;
			}
			else
			{
				lblModifyUser.Hide();
				ModifyUserName.Hide();
			}
			if (VoucherHistory.VoucherHistoryHeader.PostUser > 0)
			{
				lblApprovalUser.Show();
				ApprovalUserName.Show();
				ApprovalUserName.Text = VoucherHistory.VoucherHistoryHeader.ApprovalUserName;
			}
			else
			{
				lblApprovalUser.Hide();
				ApprovalUserName.Hide();
			}
			lblRemarks.Text = VoucherHistory.VoucherHistoryHeader.Remarks;
			VoucherDetailCardsFlowLayout.Controls.Clear();
			double TotalDebitAmount = 0.0;
			double TotalCreditAmount = 0.0;
			if (VoucherHistory.VoucherHistoryDetail.Count > 0)
			{
				VoucherDetailInfoCard[] CardsList = new VoucherDetailInfoCard[VoucherHistory.VoucherHistoryDetail.Count];
				for (int i = 0; i < CardsList.Length; i++)
				{
					CardsList[i] = new VoucherDetailInfoCard();
					TotalDebitAmount += VoucherHistory.VoucherHistoryDetail[i].DebitAmount;
					TotalCreditAmount += VoucherHistory.VoucherHistoryDetail[i].CreditAmount;
					CardsList[i].AccountId = VoucherHistory.VoucherHistoryDetail[i].DetailAccountId;
					CardsList[i].AccountTitle = VoucherHistory.VoucherHistoryDetail[i].DetailAccountTitle;
					CardsList[i].OffsetAccountId = VoucherHistory.VoucherHistoryDetail[i].AgainstAccountId;
					CardsList[i].OffsetAccountTitle = VoucherHistory.VoucherHistoryDetail[i].OffsetAccountTitle;
					CardsList[i].VoucherDate = VoucherHistory.VoucherHistoryHeader.VoucherDate;
					CardsList[i].JobLot = VoucherHistory.VoucherHistoryDetail[i].Job_Lot;
					CardsList[i].DebitAmount = Conversion.ToString((object)VoucherHistory.VoucherHistoryDetail[i].DebitAmount.ToString("0,0"));
					CardsList[i].CreditAmount = Conversion.ToString((object)VoucherHistory.VoucherHistoryDetail[i].CreditAmount.ToString("0,0"));
					CardsList[i].Remarks = VoucherHistory.VoucherHistoryDetail[i].Comments;
					VoucherDetailCardsFlowLayout.Controls.Add(CardsList[i]);
				}
			}
			lblTotalCreditAmount.Text = TotalDebitAmount.ToString("0,0");
			lblTotalDebitAmount.Text = TotalCreditAmount.ToString("0,0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVouvher102_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(VoucherIdForEdit);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			if (RequestFromScreen == "ApproveOrUnApprove Voucher")
			{
				dtHeaderCard = ModernHistory.VouchersModernHisotry_Header(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, DocumentTypeId.ToString(), true, UserAccount.ID, 0, "", ApproveOrUnApprove);
				if (dtHeaderCard.AsEnumerable().Count((DataRow r) => !r.Field<bool>("ActionTypeId")) > 0)
				{
					dtHeaderCard = (from r in dtHeaderCard.AsEnumerable()
						where !r.Field<bool>("ActionTypeId")
						select r).CopyToDataTable();
				}
				else
				{
					dtHeaderCard.Rows.Clear();
					HeaderFlowLayoutPanel.Controls.Clear();
				}
			}
			else if (RequestFromScreen == "Pending For Review")
			{
				dtHeaderCard = ModernHistory.VouchersModernHisotry_Header(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, DocumentTypeId.ToString(), true, UserAccount.ID, 0, "", ApproveOrUnApprove);
				if (dtHeaderCard.AsEnumerable().Count((DataRow r) => r.Field<bool>("ActionTypeId")) > 0)
				{
					dtHeaderCard = (from r in dtHeaderCard.AsEnumerable()
						where r.Field<bool>("ActionTypeId")
						select r).CopyToDataTable();
				}
				else
				{
					dtHeaderCard.Rows.Clear();
					HeaderFlowLayoutPanel.Controls.Clear();
				}
			}
			else
			{
				dtHeaderCard = ModernHistory.VouchersModernHisotry_Header(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, DocumentTypeId.ToString(), true, UserAccount.ID, 0, "All", false);
			}
			if (dtHeaderCard.Rows.Count > 0)
			{
				GenerateHeaderCards(dtHeaderCard);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HandleVisibilityOfFiletrPanel_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FiltersVisibilty)
			{
				FiltersVisibilty = true;
				FilterPanel.Show();
				FilterRecords.Show();
				FilterBoxIcon.Image = Resources.arrow_up_s_line;
				return;
			}
			FiltersVisibilty = false;
			txtNoFrom.Text = string.Empty;
			txtNumberTo.Text = string.Empty;
			((Control)(object)cmbSupplierName).Text = string.Empty;
			FilterPanel.Hide();
			FilterRecords.Hide();
			FilterBoxIcon.Image = Resources.arrow_down_s_line;
			GenerateHeaderCardDataOnLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FilterRecords_Click(object sender, EventArgs e)
	{
		try
		{
			if (dtHeaderCard.AsEnumerable().Count((DataRow r) => (r.Field<DateTime>("VoucherDate") >= Conversion.ToDateTime((object)DateFrom.Value.ToShortDateString()) && r.Field<DateTime>("VoucherDate") <= Conversion.ToDateTime((object)DateTo.Value.ToShortDateString()) && r.Field<int>("VoucherCode") >= Conversion.ToInt((object)txtNoFrom.Text) && r.Field<int>("VoucherCode") <= Conversion.ToInt((object)txtNumberTo.Text)) || r.Field<int>("RefAccountId") == Conversion.ToInt(cmbSupplierName.Value)) > 0)
			{
				dtHeaderCard = (from r in dtHeaderCard.AsEnumerable()
					where (r.Field<DateTime>("VoucherDate") >= Conversion.ToDateTime((object)DateFrom.Value.ToShortDateString()) && r.Field<DateTime>("VoucherDate") <= Conversion.ToDateTime((object)DateTo.Value.ToShortDateString()) && r.Field<int>("VoucherCode") >= Conversion.ToInt((object)txtNoFrom.Text) && r.Field<int>("VoucherCode") <= Conversion.ToInt((object)txtNumberTo.Text)) || r.Field<int>("RefAccountId") == Conversion.ToInt(cmbSupplierName.Value)
					select r).CopyToDataTable();
				GenerateHeaderCards(dtHeaderCard);
				return;
			}
			throw new Exception("No Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(VoucherIdForEdit, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			if (VoucherIdForEdit > 0)
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, 0, VoucherIdForEdit);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprove_Click(object sender, EventArgs e)
	{
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			if (ListForApproveOrUnapprove.Count == 0)
			{
				throw new Exception("No Record Selected");
			}
			if (ApproveOrUnApprove)
			{
				if (MessageBox.Show("Are you sure to UnApprove Voucher?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				foreach (int Id in ListForApproveOrUnapprove)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.DocumentTypeId = DocumentTypeId;
					item.Id = Id;
					item.PostDate = DateTime.Now;
					item.EntryUser = UserAccount.ID;
					item.ReqType = "UP";
					obj.AllApprovalLists.Add(item);
				}
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(obj);
				MessageBox.Show("Record UnApproved Successfully");
				ListForApproveOrUnapprove = new List<int>();
				ApprovedCount.Text = "0";
				PendingForReviewCount.Text = "0";
				BtnLoadAll_Click(null, null);
			}
			else
			{
				if (MessageBox.Show("Are you sure to Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				foreach (int Id2 in ListForApproveOrUnapprove)
				{
					ApprovalList item2 = new ApprovalList();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.DocumentTypeId = DocumentTypeId;
					item2.Id = Id2;
					item2.PostDate = DateTime.Now;
					item2.EntryUser = UserAccount.ID;
					item2.ReqType = "AP";
					obj2.AllApprovalLists.Add(item2);
				}
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(obj2);
				MessageBox.Show("Record Approved Successfully");
				ListForApproveOrUnapprove = new List<int>();
				ApprovedCount.Text = "0";
				PendingForReviewCount.Text = "0";
				BtnLoadAll_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPendingForView_Click(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			if (ListForApproveOrUnapprove.Count == 0)
			{
				throw new Exception("Please select Record first");
			}
			if (MessageBox.Show("Are you sure to Move Voucher in Pending Review?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ApprovalList obj = new ApprovalList();
			obj.AllApprovalLists = new List<ApprovalList>();
			foreach (int Id in ListForApproveOrUnapprove)
			{
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.Id = Id;
				detail.PostDate = DateTime.Now;
				detail.EntryUser = UserAccount.ID;
				detail.ActionTypeId = true;
				detail.ReqType = "AP";
				obj.AllApprovalLists.Add(detail);
			}
			VoucherHead.UpdateStatusandIsapprovedByVoucherId(obj);
			MessageBox.Show("Record Move To Pending Review Successfully");
			ListForApproveOrUnapprove = new List<int>();
			ApprovedCount.Text = "0";
			PendingForReviewCount.Text = "0";
			BtnLoadAll_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCheckAll_Click(object sender, EventArgs e)
	{
		try
		{
			ListForApproveOrUnapprove.Clear();
			foreach (Control control in HeaderFlowLayoutPanel.Controls)
			{
				if (control is VoucherInfoHeaderCard voucherInfoHeaderCard)
				{
					voucherInfoHeaderCard.CbApprove.Checked = true;
					ListForApproveOrUnapprove.Add(voucherInfoHeaderCard.VoucherId);
					ApprovedCount.Text = Conversion.ToString((object)ListForApproveOrUnapprove.Count);
				}
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
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ModernHistories.VoucherModernHistory));
		this.panel1 = new System.Windows.Forms.Panel();
		this.VoucherPdfReportDataPanel = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.label41 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.GrandTotal = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.VoucherDetailCardsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.button7 = new System.Windows.Forms.Button();
		this.label65 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.button8 = new System.Windows.Forms.Button();
		this.label70 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.button9 = new System.Windows.Forms.Button();
		this.label75 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.label78 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.button10 = new System.Windows.Forms.Button();
		this.label80 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.label83 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.panel21 = new System.Windows.Forms.Panel();
		this.button11 = new System.Windows.Forms.Button();
		this.label85 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.label87 = new System.Windows.Forms.Label();
		this.label88 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.button12 = new System.Windows.Forms.Button();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.label92 = new System.Windows.Forms.Label();
		this.label93 = new System.Windows.Forms.Label();
		this.label94 = new System.Windows.Forms.Label();
		this.panel23 = new System.Windows.Forms.Panel();
		this.button13 = new System.Windows.Forms.Button();
		this.label95 = new System.Windows.Forms.Label();
		this.label96 = new System.Windows.Forms.Label();
		this.label97 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.label99 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.lblTotalCreditAmount = new System.Windows.Forms.Label();
		this.lblTotalDebitAmount = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblRemarks = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.ApprovalUserName = new System.Windows.Forms.Label();
		this.ModifyUserName = new System.Windows.Forms.Label();
		this.EntryUserName = new System.Windows.Forms.Label();
		this.lblApprovalUser = new System.Windows.Forms.Label();
		this.lblModifyUser = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.lblChequeDate = new System.Windows.Forms.Label();
		this.lblChequeNumber = new System.Windows.Forms.Label();
		this.lblPayeeTitle = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.lblVoucherCode = new System.Windows.Forms.Label();
		this.lblVoucherDate = new System.Windows.Forms.Label();
		this.lblVoucherType = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.lblHeaderAccountTitle = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.TotalRecordPanel = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.TotalHeaderRecords = new System.Windows.Forms.Label();
		this.HeaderFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.FilterPanel = new System.Windows.Forms.Panel();
		this.label64 = new System.Windows.Forms.Label();
		this.cmbSupplierName = new UltraCombo();
		this.txtNumberTo = new System.Windows.Forms.TextBox();
		this.txtNoFrom = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.DateTo = new System.Windows.Forms.DateTimePicker();
		this.label61 = new System.Windows.Forms.Label();
		this.DateFrom = new System.Windows.Forms.DateTimePicker();
		this.FilterButtonPanel = new System.Windows.Forms.Panel();
		this.FilterRecords = new System.Windows.Forms.Button();
		this.FilterBoxIcon = new System.Windows.Forms.PictureBox();
		this.HandleVisibilityOfFiletrPanel = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CaptionForVoucherHistory = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnEdit = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnApprove = new System.Windows.Forms.ToolStripButton();
		this.ApprovedCount = new System.Windows.Forms.ToolStripLabel();
		this.btnPendingForView = new System.Windows.Forms.ToolStripButton();
		this.PendingForReviewCount = new System.Windows.Forms.ToolStripLabel();
		this.btnVouvher102 = new System.Windows.Forms.ToolStripButton();
		this.btnCheckAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.VoucherPdfReportDataPanel.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		this.panel11.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel7.SuspendLayout();
		this.VoucherDetailCardsFlowLayout.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel18.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel21.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel23.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.TotalRecordPanel.SuspendLayout();
		this.FilterPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).BeginInit();
		this.FilterButtonPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.FilterBoxIcon).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.VoucherPdfReportDataPanel);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1280, 802);
		this.panel1.TabIndex = 0;
		this.VoucherPdfReportDataPanel.AutoScroll = true;
		this.VoucherPdfReportDataPanel.Controls.Add(this.panel24);
		this.VoucherPdfReportDataPanel.Controls.Add(this.panel11);
		this.VoucherPdfReportDataPanel.Controls.Add(this.panel7);
		this.VoucherPdfReportDataPanel.Controls.Add(this.panel5);
		this.VoucherPdfReportDataPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.VoucherPdfReportDataPanel.Location = new System.Drawing.Point(345, 62);
		this.VoucherPdfReportDataPanel.Name = "VoucherPdfReportDataPanel";
		this.VoucherPdfReportDataPanel.Size = new System.Drawing.Size(839, 740);
		this.VoucherPdfReportDataPanel.TabIndex = 6;
		this.panel24.BackColor = System.Drawing.Color.Teal;
		this.panel24.Controls.Add(this.label43);
		this.panel24.Controls.Add(this.label42);
		this.panel24.Controls.Add(this.pictureBox2);
		this.panel24.Controls.Add(this.label41);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel24.Location = new System.Drawing.Point(0, 698);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(839, 42);
		this.panel24.TabIndex = 3;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(611, 13);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(187, 17);
		this.label43.TabIndex = 3;
		this.label43.Text = "Synergic Corporate Solutions";
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.White;
		this.label42.Location = new System.Drawing.Point(535, 13);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(84, 24);
		this.label42.TabIndex = 2;
		this.label42.Text = "Powered By ";
		this.pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
		this.pictureBox2.Location = new System.Drawing.Point(146, 2);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(50, 40);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox2.TabIndex = 1;
		this.pictureBox2.TabStop = false;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.White;
		this.label41.Location = new System.Drawing.Point(21, 6);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(134, 32);
		this.label41.TabIndex = 0;
		this.label41.Text = "Thank You";
		this.panel11.Controls.Add(this.panel12);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 516);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(839, 64);
		this.panel11.TabIndex = 2;
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Location = new System.Drawing.Point(520, 12);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(320, 50);
		this.panel12.TabIndex = 0;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.GrandTotal);
		this.panel13.Controls.Add(this.label40);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(320, 47);
		this.panel13.TabIndex = 15;
		this.GrandTotal.AutoSize = true;
		this.GrandTotal.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GrandTotal.ForeColor = System.Drawing.Color.White;
		this.GrandTotal.Location = new System.Drawing.Point(181, 10);
		this.GrandTotal.Name = "GrandTotal";
		this.GrandTotal.Size = new System.Drawing.Size(23, 25);
		this.GrandTotal.TabIndex = 5;
		this.GrandTotal.Text = "0";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(14, 10);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(169, 25);
		this.label40.TabIndex = 4;
		this.label40.Text = "Voucher Amount:";
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.VoucherDetailCardsFlowLayout);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 134);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(839, 382);
		this.panel7.TabIndex = 1;
		this.VoucherDetailCardsFlowLayout.AutoScroll = true;
		this.VoucherDetailCardsFlowLayout.Controls.Add(this.flowLayoutPanel1);
		this.VoucherDetailCardsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.VoucherDetailCardsFlowLayout.Location = new System.Drawing.Point(0, 41);
		this.VoucherDetailCardsFlowLayout.Margin = new System.Windows.Forms.Padding(0);
		this.VoucherDetailCardsFlowLayout.Name = "VoucherDetailCardsFlowLayout";
		this.VoucherDetailCardsFlowLayout.Padding = new System.Windows.Forms.Padding(5);
		this.VoucherDetailCardsFlowLayout.Size = new System.Drawing.Size(839, 304);
		this.VoucherDetailCardsFlowLayout.TabIndex = 2;
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.Controls.Add(this.panel17);
		this.flowLayoutPanel1.Controls.Add(this.panel18);
		this.flowLayoutPanel1.Controls.Add(this.panel19);
		this.flowLayoutPanel1.Controls.Add(this.panel20);
		this.flowLayoutPanel1.Controls.Add(this.panel21);
		this.flowLayoutPanel1.Controls.Add(this.panel22);
		this.flowLayoutPanel1.Controls.Add(this.panel23);
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(8, 8);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(839, 0);
		this.flowLayoutPanel1.TabIndex = 16;
		this.panel17.BackColor = System.Drawing.Color.White;
		this.panel17.Controls.Add(this.button7);
		this.panel17.Controls.Add(this.label65);
		this.panel17.Controls.Add(this.label66);
		this.panel17.Controls.Add(this.label67);
		this.panel17.Controls.Add(this.label68);
		this.panel17.Controls.Add(this.label69);
		this.panel17.Location = new System.Drawing.Point(3, 3);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(825, 35);
		this.panel17.TabIndex = 1;
		this.button7.BackColor = System.Drawing.Color.Teal;
		this.button7.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button7.ForeColor = System.Drawing.Color.White;
		this.button7.Location = new System.Drawing.Point(6, 6);
		this.button7.Name = "button7";
		this.button7.Size = new System.Drawing.Size(20, 20);
		this.button7.TabIndex = 12;
		this.button7.Text = "+";
		this.button7.UseVisualStyleBackColor = false;
		this.label65.AutoEllipsis = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Black;
		this.label65.Location = new System.Drawing.Point(313, 6);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(181, 20);
		this.label65.TabIndex = 8;
		this.label65.Text = "Offset Account";
		this.label66.AutoEllipsis = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.Black;
		this.label66.Location = new System.Drawing.Point(728, 6);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(94, 20);
		this.label66.TabIndex = 11;
		this.label66.Text = "Credit";
		this.label67.AutoEllipsis = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.ForeColor = System.Drawing.Color.Black;
		this.label67.Location = new System.Drawing.Point(29, 6);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(278, 20);
		this.label67.TabIndex = 7;
		this.label67.Text = "Account Title";
		this.label68.AutoEllipsis = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Black;
		this.label68.Location = new System.Drawing.Point(620, 6);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(102, 20);
		this.label68.TabIndex = 10;
		this.label68.Text = "Debit";
		this.label69.AutoEllipsis = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.Black;
		this.label69.Location = new System.Drawing.Point(500, 6);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(114, 20);
		this.label69.TabIndex = 9;
		this.label69.Text = "Job/Lot";
		this.panel18.BackColor = System.Drawing.Color.White;
		this.panel18.Controls.Add(this.button8);
		this.panel18.Controls.Add(this.label70);
		this.panel18.Controls.Add(this.label71);
		this.panel18.Controls.Add(this.label72);
		this.panel18.Controls.Add(this.label73);
		this.panel18.Controls.Add(this.label74);
		this.panel18.Location = new System.Drawing.Point(3, 44);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(825, 35);
		this.panel18.TabIndex = 13;
		this.button8.BackColor = System.Drawing.Color.Teal;
		this.button8.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button8.ForeColor = System.Drawing.Color.White;
		this.button8.Location = new System.Drawing.Point(6, 6);
		this.button8.Name = "button8";
		this.button8.Size = new System.Drawing.Size(20, 20);
		this.button8.TabIndex = 12;
		this.button8.Text = "+";
		this.button8.UseVisualStyleBackColor = false;
		this.label70.AutoEllipsis = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.ForeColor = System.Drawing.Color.Black;
		this.label70.Location = new System.Drawing.Point(313, 6);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(181, 20);
		this.label70.TabIndex = 8;
		this.label70.Text = "Offset Account";
		this.label71.AutoEllipsis = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.ForeColor = System.Drawing.Color.Black;
		this.label71.Location = new System.Drawing.Point(728, 6);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(94, 20);
		this.label71.TabIndex = 11;
		this.label71.Text = "Credit";
		this.label72.AutoEllipsis = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.ForeColor = System.Drawing.Color.Black;
		this.label72.Location = new System.Drawing.Point(29, 6);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(278, 20);
		this.label72.TabIndex = 7;
		this.label72.Text = "Account Title";
		this.label73.AutoEllipsis = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.ForeColor = System.Drawing.Color.Black;
		this.label73.Location = new System.Drawing.Point(620, 6);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(102, 20);
		this.label73.TabIndex = 10;
		this.label73.Text = "Debit";
		this.label74.AutoEllipsis = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.ForeColor = System.Drawing.Color.Black;
		this.label74.Location = new System.Drawing.Point(500, 6);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(114, 20);
		this.label74.TabIndex = 9;
		this.label74.Text = "Job/Lot";
		this.panel19.BackColor = System.Drawing.Color.White;
		this.panel19.Controls.Add(this.button9);
		this.panel19.Controls.Add(this.label75);
		this.panel19.Controls.Add(this.label76);
		this.panel19.Controls.Add(this.label77);
		this.panel19.Controls.Add(this.label78);
		this.panel19.Controls.Add(this.label79);
		this.panel19.Location = new System.Drawing.Point(3, 85);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(825, 35);
		this.panel19.TabIndex = 13;
		this.button9.BackColor = System.Drawing.Color.Teal;
		this.button9.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button9.ForeColor = System.Drawing.Color.White;
		this.button9.Location = new System.Drawing.Point(6, 6);
		this.button9.Name = "button9";
		this.button9.Size = new System.Drawing.Size(20, 20);
		this.button9.TabIndex = 12;
		this.button9.Text = "+";
		this.button9.UseVisualStyleBackColor = false;
		this.label75.AutoEllipsis = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.ForeColor = System.Drawing.Color.Black;
		this.label75.Location = new System.Drawing.Point(313, 6);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(181, 20);
		this.label75.TabIndex = 8;
		this.label75.Text = "Offset Account";
		this.label76.AutoEllipsis = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.ForeColor = System.Drawing.Color.Black;
		this.label76.Location = new System.Drawing.Point(728, 6);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(94, 20);
		this.label76.TabIndex = 11;
		this.label76.Text = "Credit";
		this.label77.AutoEllipsis = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.ForeColor = System.Drawing.Color.Black;
		this.label77.Location = new System.Drawing.Point(29, 6);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(278, 20);
		this.label77.TabIndex = 7;
		this.label77.Text = "Account Title";
		this.label78.AutoEllipsis = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.Black;
		this.label78.Location = new System.Drawing.Point(620, 6);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(102, 20);
		this.label78.TabIndex = 10;
		this.label78.Text = "Debit";
		this.label79.AutoEllipsis = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.ForeColor = System.Drawing.Color.Black;
		this.label79.Location = new System.Drawing.Point(500, 6);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(114, 20);
		this.label79.TabIndex = 9;
		this.label79.Text = "Job/Lot";
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add(this.button10);
		this.panel20.Controls.Add(this.label80);
		this.panel20.Controls.Add(this.label81);
		this.panel20.Controls.Add(this.label82);
		this.panel20.Controls.Add(this.label83);
		this.panel20.Controls.Add(this.label84);
		this.panel20.Location = new System.Drawing.Point(3, 126);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(825, 35);
		this.panel20.TabIndex = 13;
		this.button10.BackColor = System.Drawing.Color.Teal;
		this.button10.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button10.ForeColor = System.Drawing.Color.White;
		this.button10.Location = new System.Drawing.Point(6, 6);
		this.button10.Name = "button10";
		this.button10.Size = new System.Drawing.Size(20, 20);
		this.button10.TabIndex = 12;
		this.button10.Text = "+";
		this.button10.UseVisualStyleBackColor = false;
		this.label80.AutoEllipsis = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.ForeColor = System.Drawing.Color.Black;
		this.label80.Location = new System.Drawing.Point(313, 6);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(181, 20);
		this.label80.TabIndex = 8;
		this.label80.Text = "Offset Account";
		this.label81.AutoEllipsis = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.Black;
		this.label81.Location = new System.Drawing.Point(728, 6);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(94, 20);
		this.label81.TabIndex = 11;
		this.label81.Text = "Credit";
		this.label82.AutoEllipsis = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.ForeColor = System.Drawing.Color.Black;
		this.label82.Location = new System.Drawing.Point(29, 6);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(278, 20);
		this.label82.TabIndex = 7;
		this.label82.Text = "Account Title";
		this.label83.AutoEllipsis = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.ForeColor = System.Drawing.Color.Black;
		this.label83.Location = new System.Drawing.Point(620, 6);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(102, 20);
		this.label83.TabIndex = 10;
		this.label83.Text = "Debit";
		this.label84.AutoEllipsis = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.ForeColor = System.Drawing.Color.Black;
		this.label84.Location = new System.Drawing.Point(500, 6);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(114, 20);
		this.label84.TabIndex = 9;
		this.label84.Text = "Job/Lot";
		this.panel21.BackColor = System.Drawing.Color.White;
		this.panel21.Controls.Add(this.button11);
		this.panel21.Controls.Add(this.label85);
		this.panel21.Controls.Add(this.label86);
		this.panel21.Controls.Add(this.label87);
		this.panel21.Controls.Add(this.label88);
		this.panel21.Controls.Add(this.label89);
		this.panel21.Location = new System.Drawing.Point(3, 167);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(825, 35);
		this.panel21.TabIndex = 13;
		this.button11.BackColor = System.Drawing.Color.Teal;
		this.button11.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button11.ForeColor = System.Drawing.Color.White;
		this.button11.Location = new System.Drawing.Point(6, 6);
		this.button11.Name = "button11";
		this.button11.Size = new System.Drawing.Size(20, 20);
		this.button11.TabIndex = 12;
		this.button11.Text = "+";
		this.button11.UseVisualStyleBackColor = false;
		this.label85.AutoEllipsis = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.ForeColor = System.Drawing.Color.Black;
		this.label85.Location = new System.Drawing.Point(313, 6);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(181, 20);
		this.label85.TabIndex = 8;
		this.label85.Text = "Offset Account";
		this.label86.AutoEllipsis = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.ForeColor = System.Drawing.Color.Black;
		this.label86.Location = new System.Drawing.Point(728, 6);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(94, 20);
		this.label86.TabIndex = 11;
		this.label86.Text = "Credit";
		this.label87.AutoEllipsis = true;
		this.label87.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.ForeColor = System.Drawing.Color.Black;
		this.label87.Location = new System.Drawing.Point(29, 6);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(278, 20);
		this.label87.TabIndex = 7;
		this.label87.Text = "Account Title";
		this.label88.AutoEllipsis = true;
		this.label88.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label88.ForeColor = System.Drawing.Color.Black;
		this.label88.Location = new System.Drawing.Point(620, 6);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(102, 20);
		this.label88.TabIndex = 10;
		this.label88.Text = "Debit";
		this.label89.AutoEllipsis = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.ForeColor = System.Drawing.Color.Black;
		this.label89.Location = new System.Drawing.Point(500, 6);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(114, 20);
		this.label89.TabIndex = 9;
		this.label89.Text = "Job/Lot";
		this.panel22.BackColor = System.Drawing.Color.White;
		this.panel22.Controls.Add(this.button12);
		this.panel22.Controls.Add(this.label90);
		this.panel22.Controls.Add(this.label91);
		this.panel22.Controls.Add(this.label92);
		this.panel22.Controls.Add(this.label93);
		this.panel22.Controls.Add(this.label94);
		this.panel22.Location = new System.Drawing.Point(3, 208);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(825, 35);
		this.panel22.TabIndex = 14;
		this.button12.BackColor = System.Drawing.Color.Teal;
		this.button12.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button12.ForeColor = System.Drawing.Color.White;
		this.button12.Location = new System.Drawing.Point(6, 6);
		this.button12.Name = "button12";
		this.button12.Size = new System.Drawing.Size(20, 20);
		this.button12.TabIndex = 12;
		this.button12.Text = "+";
		this.button12.UseVisualStyleBackColor = false;
		this.label90.AutoEllipsis = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.ForeColor = System.Drawing.Color.Black;
		this.label90.Location = new System.Drawing.Point(313, 6);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(181, 20);
		this.label90.TabIndex = 8;
		this.label90.Text = "Offset Account";
		this.label91.AutoEllipsis = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.ForeColor = System.Drawing.Color.Black;
		this.label91.Location = new System.Drawing.Point(728, 6);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(94, 20);
		this.label91.TabIndex = 11;
		this.label91.Text = "Credit";
		this.label92.AutoEllipsis = true;
		this.label92.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.Black;
		this.label92.Location = new System.Drawing.Point(29, 6);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(278, 20);
		this.label92.TabIndex = 7;
		this.label92.Text = "Account Title";
		this.label93.AutoEllipsis = true;
		this.label93.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label93.ForeColor = System.Drawing.Color.Black;
		this.label93.Location = new System.Drawing.Point(620, 6);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(102, 20);
		this.label93.TabIndex = 10;
		this.label93.Text = "Debit";
		this.label94.AutoEllipsis = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.ForeColor = System.Drawing.Color.Black;
		this.label94.Location = new System.Drawing.Point(500, 6);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(114, 20);
		this.label94.TabIndex = 9;
		this.label94.Text = "Job/Lot";
		this.panel23.BackColor = System.Drawing.Color.White;
		this.panel23.Controls.Add(this.button13);
		this.panel23.Controls.Add(this.label95);
		this.panel23.Controls.Add(this.label96);
		this.panel23.Controls.Add(this.label97);
		this.panel23.Controls.Add(this.label98);
		this.panel23.Controls.Add(this.label99);
		this.panel23.Location = new System.Drawing.Point(3, 249);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(825, 35);
		this.panel23.TabIndex = 15;
		this.button13.BackColor = System.Drawing.Color.Teal;
		this.button13.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button13.ForeColor = System.Drawing.Color.White;
		this.button13.Location = new System.Drawing.Point(6, 6);
		this.button13.Name = "button13";
		this.button13.Size = new System.Drawing.Size(20, 20);
		this.button13.TabIndex = 12;
		this.button13.Text = "+";
		this.button13.UseVisualStyleBackColor = false;
		this.label95.AutoEllipsis = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.ForeColor = System.Drawing.Color.Black;
		this.label95.Location = new System.Drawing.Point(313, 6);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(181, 20);
		this.label95.TabIndex = 8;
		this.label95.Text = "Offset Account";
		this.label96.AutoEllipsis = true;
		this.label96.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label96.ForeColor = System.Drawing.Color.Black;
		this.label96.Location = new System.Drawing.Point(728, 6);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(94, 20);
		this.label96.TabIndex = 11;
		this.label96.Text = "Credit";
		this.label97.AutoEllipsis = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label97.ForeColor = System.Drawing.Color.Black;
		this.label97.Location = new System.Drawing.Point(29, 6);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(278, 20);
		this.label97.TabIndex = 7;
		this.label97.Text = "Account Title";
		this.label98.AutoEllipsis = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.ForeColor = System.Drawing.Color.Black;
		this.label98.Location = new System.Drawing.Point(620, 6);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(102, 20);
		this.label98.TabIndex = 10;
		this.label98.Text = "Debit";
		this.label99.AutoEllipsis = true;
		this.label99.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label99.ForeColor = System.Drawing.Color.Black;
		this.label99.Location = new System.Drawing.Point(500, 6);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(114, 20);
		this.label99.TabIndex = 9;
		this.label99.Text = "Job/Lot";
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.lblTotalCreditAmount);
		this.panel9.Controls.Add(this.lblTotalDebitAmount);
		this.panel9.Controls.Add(this.label20);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(0, 345);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(839, 37);
		this.panel9.TabIndex = 1;
		this.lblTotalCreditAmount.AutoSize = true;
		this.lblTotalCreditAmount.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalCreditAmount.ForeColor = System.Drawing.Color.White;
		this.lblTotalCreditAmount.Location = new System.Drawing.Point(733, 9);
		this.lblTotalCreditAmount.Name = "lblTotalCreditAmount";
		this.lblTotalCreditAmount.Size = new System.Drawing.Size(18, 20);
		this.lblTotalCreditAmount.TabIndex = 14;
		this.lblTotalCreditAmount.Text = "0";
		this.lblTotalDebitAmount.AutoSize = true;
		this.lblTotalDebitAmount.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalDebitAmount.ForeColor = System.Drawing.Color.White;
		this.lblTotalDebitAmount.Location = new System.Drawing.Point(623, 9);
		this.lblTotalDebitAmount.Name = "lblTotalDebitAmount";
		this.lblTotalDebitAmount.Size = new System.Drawing.Size(18, 20);
		this.lblTotalDebitAmount.TabIndex = 13;
		this.lblTotalDebitAmount.Text = "0";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(32, 9);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(96, 20);
		this.label20.TabIndex = 12;
		this.label20.Text = "Detail Totals";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label25);
		this.panel8.Controls.Add(this.label26);
		this.panel8.Controls.Add(this.label27);
		this.panel8.Controls.Add(this.label28);
		this.panel8.Controls.Add(this.label29);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(839, 41);
		this.panel8.TabIndex = 0;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(319, 10);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(115, 20);
		this.label25.TabIndex = 8;
		this.label25.Text = "Offset Account";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(721, 10);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 20);
		this.label26.TabIndex = 11;
		this.label26.Text = "Credit";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(33, 10);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(102, 20);
		this.label27.TabIndex = 7;
		this.label27.Text = "Account Title";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(620, 10);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(47, 20);
		this.label28.TabIndex = 10;
		this.label28.Text = "Debit";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(495, 10);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(64, 20);
		this.label29.TabIndex = 9;
		this.label29.Text = "Job/Lot";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.lblRemarks);
		this.panel5.Controls.Add(this.label16);
		this.panel5.Controls.Add(this.ApprovalUserName);
		this.panel5.Controls.Add(this.ModifyUserName);
		this.panel5.Controls.Add(this.EntryUserName);
		this.panel5.Controls.Add(this.lblApprovalUser);
		this.panel5.Controls.Add(this.lblModifyUser);
		this.panel5.Controls.Add(this.label18);
		this.panel5.Controls.Add(this.lblChequeDate);
		this.panel5.Controls.Add(this.lblChequeNumber);
		this.panel5.Controls.Add(this.lblPayeeTitle);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add(this.label14);
		this.panel5.Controls.Add(this.lblVoucherCode);
		this.panel5.Controls.Add(this.lblVoucherDate);
		this.panel5.Controls.Add(this.lblVoucherType);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(839, 134);
		this.panel5.TabIndex = 0;
		this.lblRemarks.AutoEllipsis = true;
		this.lblRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblRemarks.Location = new System.Drawing.Point(100, 96);
		this.lblRemarks.Name = "lblRemarks";
		this.lblRemarks.Size = new System.Drawing.Size(736, 35);
		this.lblRemarks.TabIndex = 37;
		this.lblRemarks.Text = "Remarks";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.Teal;
		this.label16.Location = new System.Drawing.Point(15, 96);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(54, 15);
		this.label16.TabIndex = 36;
		this.label16.Text = "Remarks";
		this.ApprovalUserName.AutoSize = true;
		this.ApprovalUserName.BackColor = System.Drawing.Color.Teal;
		this.ApprovalUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovalUserName.ForeColor = System.Drawing.Color.White;
		this.ApprovalUserName.Location = new System.Drawing.Point(700, 77);
		this.ApprovalUserName.Name = "ApprovalUserName";
		this.ApprovalUserName.Size = new System.Drawing.Size(40, 15);
		this.ApprovalUserName.TabIndex = 35;
		this.ApprovalUserName.Text = "Name";
		this.ModifyUserName.AutoSize = true;
		this.ModifyUserName.BackColor = System.Drawing.Color.Teal;
		this.ModifyUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ModifyUserName.ForeColor = System.Drawing.Color.White;
		this.ModifyUserName.Location = new System.Drawing.Point(700, 59);
		this.ModifyUserName.Name = "ModifyUserName";
		this.ModifyUserName.Size = new System.Drawing.Size(40, 15);
		this.ModifyUserName.TabIndex = 34;
		this.ModifyUserName.Text = "Name";
		this.EntryUserName.AutoSize = true;
		this.EntryUserName.BackColor = System.Drawing.Color.Teal;
		this.EntryUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EntryUserName.ForeColor = System.Drawing.Color.White;
		this.EntryUserName.Location = new System.Drawing.Point(700, 41);
		this.EntryUserName.Name = "EntryUserName";
		this.EntryUserName.Size = new System.Drawing.Size(40, 15);
		this.EntryUserName.TabIndex = 33;
		this.EntryUserName.Text = "Name";
		this.lblApprovalUser.AutoSize = true;
		this.lblApprovalUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovalUser.Location = new System.Drawing.Point(613, 77);
		this.lblApprovalUser.Name = "lblApprovalUser";
		this.lblApprovalUser.Size = new System.Drawing.Size(91, 15);
		this.lblApprovalUser.TabIndex = 32;
		this.lblApprovalUser.Text = "Approval User: ";
		this.lblModifyUser.AutoSize = true;
		this.lblModifyUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModifyUser.Location = new System.Drawing.Point(613, 59);
		this.lblModifyUser.Name = "lblModifyUser";
		this.lblModifyUser.Size = new System.Drawing.Size(80, 15);
		this.lblModifyUser.TabIndex = 31;
		this.lblModifyUser.Text = "Modify User: ";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(613, 41);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(71, 15);
		this.label18.TabIndex = 30;
		this.label18.Text = "Entry User: ";
		this.lblChequeDate.AutoSize = true;
		this.lblChequeDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblChequeDate.Location = new System.Drawing.Point(417, 77);
		this.lblChequeDate.Name = "lblChequeDate";
		this.lblChequeDate.Size = new System.Drawing.Size(33, 15);
		this.lblChequeDate.TabIndex = 12;
		this.lblChequeDate.Text = "Date";
		this.lblChequeNumber.AutoSize = true;
		this.lblChequeNumber.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblChequeNumber.Location = new System.Drawing.Point(417, 59);
		this.lblChequeNumber.Name = "lblChequeNumber";
		this.lblChequeNumber.Size = new System.Drawing.Size(47, 15);
		this.lblChequeNumber.TabIndex = 11;
		this.lblChequeNumber.Text = "cheque";
		this.lblPayeeTitle.AutoSize = true;
		this.lblPayeeTitle.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblPayeeTitle.Location = new System.Drawing.Point(417, 41);
		this.lblPayeeTitle.Name = "lblPayeeTitle";
		this.lblPayeeTitle.Size = new System.Drawing.Size(31, 15);
		this.lblPayeeTitle.TabIndex = 10;
		this.lblPayeeTitle.Text = "Title";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.Teal;
		this.label12.Location = new System.Drawing.Point(338, 77);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 15);
		this.label12.TabIndex = 9;
		this.label12.Text = "Cheque Date";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.Teal;
		this.label13.Location = new System.Drawing.Point(338, 59);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(71, 15);
		this.label13.TabIndex = 8;
		this.label13.Text = "Cheque No.";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.Teal;
		this.label14.Location = new System.Drawing.Point(338, 41);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(67, 15);
		this.label14.TabIndex = 7;
		this.label14.Text = "Payee Title";
		this.lblVoucherCode.AutoSize = true;
		this.lblVoucherCode.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblVoucherCode.Location = new System.Drawing.Point(100, 77);
		this.lblVoucherCode.Name = "lblVoucherCode";
		this.lblVoucherCode.Size = new System.Drawing.Size(35, 15);
		this.lblVoucherCode.TabIndex = 6;
		this.lblVoucherCode.Text = "Code";
		this.lblVoucherDate.AutoSize = true;
		this.lblVoucherDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblVoucherDate.Location = new System.Drawing.Point(100, 59);
		this.lblVoucherDate.Name = "lblVoucherDate";
		this.lblVoucherDate.Size = new System.Drawing.Size(33, 15);
		this.lblVoucherDate.TabIndex = 5;
		this.lblVoucherDate.Text = "Date";
		this.lblVoucherType.AutoSize = true;
		this.lblVoucherType.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblVoucherType.Location = new System.Drawing.Point(100, 41);
		this.lblVoucherType.Name = "lblVoucherType";
		this.lblVoucherType.Size = new System.Drawing.Size(33, 15);
		this.lblVoucherType.TabIndex = 4;
		this.lblVoucherType.Text = "Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(15, 77);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(83, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "Voucher Code";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(15, 59);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(81, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "Voucher Date";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.Teal;
		this.label2.Location = new System.Drawing.Point(15, 41);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(81, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Voucher Type";
		this.panel6.Controls.Add(this.lblHeaderAccountTitle);
		this.panel6.Location = new System.Drawing.Point(0, 1);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(839, 36);
		this.panel6.TabIndex = 0;
		this.lblHeaderAccountTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblHeaderAccountTitle.Font = new System.Drawing.Font("Segoe UI", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeaderAccountTitle.ForeColor = System.Drawing.Color.Teal;
		this.lblHeaderAccountTitle.Location = new System.Drawing.Point(0, 0);
		this.lblHeaderAccountTitle.Name = "lblHeaderAccountTitle";
		this.lblHeaderAccountTitle.Size = new System.Drawing.Size(839, 36);
		this.lblHeaderAccountTitle.TabIndex = 0;
		this.lblHeaderAccountTitle.Text = "Petty Cash";
		this.lblHeaderAccountTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel4.Controls.Add(this.TotalRecordPanel);
		this.panel4.Controls.Add(this.HeaderFlowLayoutPanel);
		this.panel4.Controls.Add(this.FilterPanel);
		this.panel4.Controls.Add(this.FilterButtonPanel);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(0, 62);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(345, 740);
		this.panel4.TabIndex = 5;
		this.TotalRecordPanel.BackColor = System.Drawing.Color.White;
		this.TotalRecordPanel.Controls.Add(this.label5);
		this.TotalRecordPanel.Controls.Add(this.TotalHeaderRecords);
		this.TotalRecordPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.TotalRecordPanel.Location = new System.Drawing.Point(0, 713);
		this.TotalRecordPanel.Name = "TotalRecordPanel";
		this.TotalRecordPanel.Size = new System.Drawing.Size(345, 27);
		this.TotalRecordPanel.TabIndex = 10;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(96, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(95, 17);
		this.label5.TabIndex = 1;
		this.label5.Text = "Total Records: ";
		this.TotalHeaderRecords.AutoSize = true;
		this.TotalHeaderRecords.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalHeaderRecords.Location = new System.Drawing.Point(188, 5);
		this.TotalHeaderRecords.Name = "TotalHeaderRecords";
		this.TotalHeaderRecords.Size = new System.Drawing.Size(15, 17);
		this.TotalHeaderRecords.TabIndex = 2;
		this.TotalHeaderRecords.Text = "0";
		this.HeaderFlowLayoutPanel.AutoScroll = true;
		this.HeaderFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.HeaderFlowLayoutPanel.Location = new System.Drawing.Point(0, 110);
		this.HeaderFlowLayoutPanel.Name = "HeaderFlowLayoutPanel";
		this.HeaderFlowLayoutPanel.Size = new System.Drawing.Size(345, 630);
		this.HeaderFlowLayoutPanel.TabIndex = 9;
		this.FilterPanel.BackColor = System.Drawing.Color.White;
		this.FilterPanel.Controls.Add(this.label64);
		this.FilterPanel.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierName);
		this.FilterPanel.Controls.Add(this.txtNumberTo);
		this.FilterPanel.Controls.Add(this.txtNoFrom);
		this.FilterPanel.Controls.Add(this.label63);
		this.FilterPanel.Controls.Add(this.label62);
		this.FilterPanel.Controls.Add(this.label60);
		this.FilterPanel.Controls.Add(this.DateTo);
		this.FilterPanel.Controls.Add(this.label61);
		this.FilterPanel.Controls.Add(this.DateFrom);
		this.FilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.FilterPanel.Location = new System.Drawing.Point(0, 26);
		this.FilterPanel.Name = "FilterPanel";
		this.FilterPanel.Size = new System.Drawing.Size(345, 84);
		this.FilterPanel.TabIndex = 7;
		this.label64.AutoSize = true;
		this.label64.BackColor = System.Drawing.Color.White;
		this.label64.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(3, 57);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(70, 15);
		this.label64.TabIndex = 213;
		this.label64.Text = "Party Name";
		this.cmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Location = new System.Drawing.Point(75, 54);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Name = "cmbSupplierName";
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Size = new System.Drawing.Size(266, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).TabIndex = 212;
		this.txtNumberTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNumberTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNumberTo.Location = new System.Drawing.Point(253, 28);
		this.txtNumberTo.Name = "txtNumberTo";
		this.txtNumberTo.Size = new System.Drawing.Size(88, 23);
		this.txtNumberTo.TabIndex = 210;
		this.txtNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoFrom.Location = new System.Drawing.Point(253, 3);
		this.txtNoFrom.Name = "txtNoFrom";
		this.txtNoFrom.Size = new System.Drawing.Size(88, 23);
		this.txtNoFrom.TabIndex = 208;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(176, 32);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(62, 15);
		this.label63.TabIndex = 211;
		this.label63.Text = "Invoice To";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(176, 7);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(77, 15);
		this.label62.TabIndex = 209;
		this.label62.Text = "Invoice From";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(3, 32);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(47, 15);
		this.label60.TabIndex = 207;
		this.label60.Text = "Date To";
		this.DateTo.CustomFormat = "dd-MMM-yy";
		this.DateTo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateTo.Location = new System.Drawing.Point(75, 28);
		this.DateTo.Name = "DateTo";
		this.DateTo.Size = new System.Drawing.Size(83, 23);
		this.DateTo.TabIndex = 205;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(3, 7);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(63, 15);
		this.label61.TabIndex = 206;
		this.label61.Text = "Date From";
		this.DateFrom.CustomFormat = "dd-MMM-yy";
		this.DateFrom.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateFrom.Location = new System.Drawing.Point(75, 3);
		this.DateFrom.Name = "DateFrom";
		this.DateFrom.Size = new System.Drawing.Size(83, 23);
		this.DateFrom.TabIndex = 204;
		this.FilterButtonPanel.BackColor = System.Drawing.Color.White;
		this.FilterButtonPanel.Controls.Add(this.FilterRecords);
		this.FilterButtonPanel.Controls.Add(this.FilterBoxIcon);
		this.FilterButtonPanel.Controls.Add(this.HandleVisibilityOfFiletrPanel);
		this.FilterButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.FilterButtonPanel.Location = new System.Drawing.Point(0, 0);
		this.FilterButtonPanel.Name = "FilterButtonPanel";
		this.FilterButtonPanel.Size = new System.Drawing.Size(345, 26);
		this.FilterButtonPanel.TabIndex = 6;
		this.FilterRecords.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.FilterRecords.Location = new System.Drawing.Point(255, 3);
		this.FilterRecords.Name = "FilterRecords";
		this.FilterRecords.Size = new System.Drawing.Size(85, 19);
		this.FilterRecords.TabIndex = 6;
		this.FilterRecords.Text = "Filter Records";
		this.FilterRecords.UseVisualStyleBackColor = true;
		this.FilterBoxIcon.Cursor = System.Windows.Forms.Cursors.Hand;
		this.FilterBoxIcon.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.FilterBoxIcon.Location = new System.Drawing.Point(124, 3);
		this.FilterBoxIcon.Name = "FilterBoxIcon";
		this.FilterBoxIcon.Size = new System.Drawing.Size(33, 18);
		this.FilterBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.FilterBoxIcon.TabIndex = 5;
		this.FilterBoxIcon.TabStop = false;
		this.FilterBoxIcon.Click += new System.EventHandler(HandleVisibilityOfFiletrPanel_Click);
		this.HandleVisibilityOfFiletrPanel.AutoSize = true;
		this.HandleVisibilityOfFiletrPanel.Cursor = System.Windows.Forms.Cursors.Hand;
		this.HandleVisibilityOfFiletrPanel.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.HandleVisibilityOfFiletrPanel.Location = new System.Drawing.Point(155, 1);
		this.HandleVisibilityOfFiletrPanel.Name = "HandleVisibilityOfFiletrPanel";
		this.HandleVisibilityOfFiletrPanel.Size = new System.Drawing.Size(56, 21);
		this.HandleVisibilityOfFiletrPanel.TabIndex = 4;
		this.HandleVisibilityOfFiletrPanel.Text = "Filters";
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1280, 35);
		this.panel2.TabIndex = 4;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.CaptionForVoucherHistory);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1280, 34);
		this.panel3.TabIndex = 5;
		this.CaptionForVoucherHistory.AutoSize = true;
		this.CaptionForVoucherHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.CaptionForVoucherHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CaptionForVoucherHistory.ForeColor = System.Drawing.Color.White;
		this.CaptionForVoucherHistory.Location = new System.Drawing.Point(6, 5);
		this.CaptionForVoucherHistory.Name = "CaptionForVoucherHistory";
		this.CaptionForVoucherHistory.Size = new System.Drawing.Size(145, 24);
		this.CaptionForVoucherHistory.TabIndex = 1;
		this.CaptionForVoucherHistory.Text = "Voucher History";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.BtnLoadAll, this.btnEdit, this.btnAttachment, this.btnCheckAll, this.btnApprove, this.ApprovedCount, this.btnPendingForView, this.PendingForReviewCount, this.btnVouvher102 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1280, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_search_16;
		this.BtnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadAll.Name = "BtnLoadAll";
		this.BtnLoadAll.Size = new System.Drawing.Size(74, 24);
		this.BtnLoadAll.Text = "Load All";
		this.BtnLoadAll.Click += new System.EventHandler(BtnLoadAll_Click);
		this.btnEdit.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.Size = new System.Drawing.Size(51, 24);
		this.btnEdit.Text = "Edit";
		this.btnEdit.Click += new System.EventHandler(btnEdit_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(37, 24);
		this.btnAttachment.Text = "0";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnApprove.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApprove.Name = "btnApprove";
		this.btnApprove.Size = new System.Drawing.Size(79, 24);
		this.btnApprove.Text = "Approve ";
		this.btnApprove.Visible = false;
		this.btnApprove.Click += new System.EventHandler(btnApprove_Click);
		this.ApprovedCount.Name = "ApprovedCount";
		this.ApprovedCount.Size = new System.Drawing.Size(13, 24);
		this.ApprovedCount.Text = "0";
		this.ApprovedCount.Visible = false;
		this.btnPendingForView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnPendingForView.Image = (System.Drawing.Image)resources.GetObject("btnPendingForView.Image");
		this.btnPendingForView.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPendingForView.Name = "btnPendingForView";
		this.btnPendingForView.Size = new System.Drawing.Size(115, 24);
		this.btnPendingForView.Text = "Pending For Review";
		this.btnPendingForView.Visible = false;
		this.btnPendingForView.Click += new System.EventHandler(btnPendingForView_Click);
		this.PendingForReviewCount.Name = "PendingForReviewCount";
		this.PendingForReviewCount.Size = new System.Drawing.Size(13, 24);
		this.PendingForReviewCount.Text = "0";
		this.PendingForReviewCount.Visible = false;
		this.btnVouvher102.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVouvher102.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVouvher102.Name = "btnVouvher102";
		this.btnVouvher102.Size = new System.Drawing.Size(97, 24);
		this.btnVouvher102.Text = "102-Voucher";
		this.btnVouvher102.Click += new System.EventHandler(btnVouvher102_Click);
		this.btnCheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCheckAll.Image = (System.Drawing.Image)resources.GetObject("btnCheckAll.Image");
		this.btnCheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCheckAll.Name = "btnCheckAll";
		this.btnCheckAll.Size = new System.Drawing.Size(61, 24);
		this.btnCheckAll.Text = "Check All";
		this.btnCheckAll.Click += new System.EventHandler(btnCheckAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1280, 802);
		base.Controls.Add(this.panel1);
		base.Name = "VoucherModernHistory";
		this.Text = "Voucher Modern History";
		base.Load += new System.EventHandler(VoucherModernHistory_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.VoucherPdfReportDataPanel.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.VoucherDetailCardsFlowLayout.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.TotalRecordPanel.ResumeLayout(false);
		this.TotalRecordPanel.PerformLayout();
		this.FilterPanel.ResumeLayout(false);
		this.FilterPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).EndInit();
		this.FilterButtonPanel.ResumeLayout(false);
		this.FilterButtonPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.FilterBoxIcon).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
