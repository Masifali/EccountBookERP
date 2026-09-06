// // using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using FilterEditType = Janus.Windows.GridEX.FilterEditType;
using FcYBankChargesTypes = System.Object;
using SpecialSreenRightsUserWise = System.Object;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
using UserAccount = global::Architecture.Model.UserAccount;
using VoucherHead = global::Architecture.Model.Accounts.VoucherHead;
using DMSAttachments = global::Architecture.BLL.DMSAttachments;
using tblUserRights = global::Architecture.Model.tblUserRights;
using CheqBookHeader = global::Architecture.BLL.Accounts.CheqBookHeader;
using TaxScheduleMain = global::Architecture.BLL.Inventory.TaxScheduleMain;
using PdcInventory = global::Architecture.Model.Accounts.PdcInventory;
using MultiCurrency = global::Architecture.BLL.MultiCurrency;
using BankBalanceManual = global::Architecture.BLL.BankBalanceManual;
using BranchesAllocationToUser = global::Architecture.BLL.BranchesAllocationToUser;
using SupCustBankDetail = global::Architecture.Model.Inventory.SupCustBankDetail;
using Projects = global::Architecture.BLL.Projects;
using UsersWithCostCenter = global::Architecture.BLL.Inventory.UsersWithCostCenter;
using AccountsPrematurePaymentsReceipts = global::Architecture.BLL.Accounts.AccountsPrematurePaymentsReceipts;
using AccountsReconciliationHeader = global::Architecture.BLL.Accounts.AccountsReconciliationHeader;
using BankReconciliation = global::Architecture.BLL.Accounts.BankReconciliation;
using City = global::Architecture.Model.City;
using Country = global::Architecture.Model.Country;
using ChartofAccount = global::Architecture.BLL.ChartofAccountBLL;
using pdcTransaction = global::Architecture.Model.BankSchema.pdcTransaction;
using LicenseContext = OfficeOpenXml.LicenseContext;
using SortOrder = Janus.Windows.GridEX.SortOrder;
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using SreenRightsUserWise = global::Architecture.Model.SreenRightsUserWise;
using TaxesTypes = global::Architecture.Model.Inventory.TaxesTypes;
using COAAllocation = global::Architecture.Model.Accounts.COAAllocation;

namespace Architecture.WinApp.Account_Definition
{

public class PostDatedCheqPaymentVouchers : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private bool UpdateMode;

	private int HeaderId;

	private int updateDetailIndex;

	private DataTable table = new DataTable();

	private DataTable dtDrAc = new DataTable();

	private DataTable dtBank = new DataTable();

	private DataTable CoaList = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private ToolStripButton btnUpdate;

	private ToolStripButton Save;

	private ToolStripButton Print;

	private Label label23;

	private Panel panel5;

	private Panel panel4;

	private BackgroundWorker backgroundWorker1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel7;

	private ToolStrip toolStrip3;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnplus;

	internal UltraCombo CmbDebitAccount;

	private Label label11;

	private TextBox txtRmks;

	private Label label10;

	private Label label9;

	private Label label8;

	private TextBox txtAmount;

	private TextBox txtBankName;

	private Label label3;

	private Label label2;

	private DateTimePicker DocDate;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private TextBox txtDocNo;

	private ToolStripButton btnRefresh;

	private Label LableCreditBal;

	private Label LbCr;

	private DateTimePicker CheqDate;

	private Label label17;

	internal UltraCombo CmbCheqno;

	private Label label16;

	internal UltraCombo CmbCheqPartyAccount;

	private Label label15;

	internal UltraCombo CmbCreditAccount;

	private Label label18;

	private CheckBox ChkBox;

	private Label lblDr;

	private Label lbldrcr;

	private Label lblCr;

	private Label lblCrDr;

	private Panel panel6;

	private Panel panel9;

	internal GridEX grdDetail;

	private Panel panel8;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnDelete;

	private ToolStripButton btnLoadAll;

	internal UltraCombo CmbTransType;

	private Label label1;

	private ToolStripButton btnVoucher;

	private Label label4;

	private TextBox txtPayTitle;

	private Label label12;

	private TextBox txtPayeeTmp;

	private Label label7;

	private DateTimePicker datCheqDateTmp;

	private TextBox txtChequeNoTmp;

	private Label label6;

	private Label label5;

	private Label label19;

	private Label label14;

	private TextBox txtAmountTmp;

	private RichTextBox txtRupeesTmp;

	private Label label13;

	private Panel PnlCheqTmplete;

	private Label lblSignature;

	private Label lblBankName;

	private PictureBox pictureBox1;

	private Label label20;

	private TextBox txtManualNo;

	private TextBox txtReceivedBy;

	private Label label21;

	private CheckBox ChkPrint2;

	private CheckBox ChkPrint1;

	public PostDatedCheqPaymentVouchers(UserAccount user)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool FormValidation()
	{
		if (((Control)(object)CmbDebitAccount).Text == string.Empty || ((Control)(object)CmbDebitAccount).Text == null)
		{
			MessageBox.Show("Account Debit Field is Required");
			CmbDebitAccount.Focus();
			return false;
		}
		if (txtDocNo.Text == string.Empty || txtDocNo.Text == null || txtDocNo.Text == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtDocNo.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((Control)(object)CmbTransType).Text == "PDC")
		{
			if (((Control)(object)CmbCheqno).Text == string.Empty || ((Control)(object)CmbCheqno).Text == null || ((Control)(object)CmbCheqno).Text == "0")
			{
				MessageBox.Show("Cheq No Filed Requird");
				CmbCheqno.Focus();
				return false;
			}
			if (txtBankName.Text == string.Empty || txtBankName.Text == null || txtBankName.Text == "0")
			{
				MessageBox.Show("Bank Name Filed Requird");
				txtBankName.Focus();
				return false;
			}
		}
		if (((Control)(object)CmbCheqPartyAccount).Text == string.Empty || ((Control)(object)CmbCheqPartyAccount).Text == null)
		{
			MessageBox.Show("CheqPartyName Field is Required");
			CmbCheqPartyAccount.Focus();
			return false;
		}
		if (((Control)(object)CmbCreditAccount).Text == string.Empty || ((Control)(object)CmbCreditAccount).Text == null || txtBankName.Text == "0")
		{
			MessageBox.Show("Credit Account Filed Requird");
			CmbCreditAccount.Focus();
			return false;
		}
		return true;
	}

	private void DebitAccountFill()
	{
		try
		{
			CoaList = CommonServices.CoaAllocationGetForComboServiceBind();
			if (CoaList.Rows.Count > 0)
			{
				DDL.BindDDL(CoaList, CmbDebitAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				DDL.BindDDL(CoaList, CmbCreditAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				DDL.BindDDL(CoaList, CmbCheqPartyAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)CmbDebitAccount).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("TransType");
			dt.Rows.Add(1, "PDC");
			dt.Rows.Add(2, "Other");
			DDL.BindDDL(dt, CmbTransType, "Id", "TransType", "TransType", ZeroIndex: false);
			((UltraGridBase)CmbTransType).DisplayLayout.Bands[0].Columns[1].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankAccountFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			global::Architecture.Model.Bank obj = new global::Architecture.Model.Bank();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBank = global::Architecture.BLL.Bank.GetAllDt(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		try
		{
			txtDocNo.Text = Conversion.ToString((object)CommonServices.GenerateVoucherCode(24));
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

	private void gridsetting()
	{
		try
		{
			grd.RootTable.Columns["CheqPartyId"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["CheqId"].Visible = false;
			grd.RootTable.Columns["AccountTitle"].Caption = "Credit Account";
			grd.RootTable.Columns["CheqParty"].EditType = (EditType)0;
			grd.RootTable.Columns["CheqNo"].EditType = (EditType)0;
			grd.RootTable.Columns["BankName"].EditType = (EditType)0;
			grd.RootTable.Columns["CheqDate"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["CheqParty"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["CheqNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["BankName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["CheqDate"].FilterEditType = (FilterEditType)5;
			grd.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = "#,#.##";
			grd.RootTable.Columns["Amount"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns.Add("ChequeView");
			grd.RootTable.Columns["ChequeView"].Caption = "ChequeView";
			grd.RootTable.Columns["ChequeView"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["ChequeView"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["ChequeView"].ButtonText = "ChequeView";
			grd.RootTable.Columns["ChequeView"].Width = 80;
			grd.RootTable.Columns.Add("ChequePrint");
			grd.RootTable.Columns["ChequePrint"].Caption = "ChequePrint";
			grd.RootTable.Columns["ChequePrint"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["ChequePrint"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["ChequePrint"].ButtonText = "ChequePrint";
			grd.RootTable.Columns["ChequePrint"].Width = 80;
			grd.ColumnAutoResize = true;
			ctrlGrdBar1_Load(null, null);
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
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			HeaderId = 0;
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			((Control)(object)CmbCheqPartyAccount).Text = string.Empty;
			((Control)(object)CmbCheqno).Text = string.Empty;
			txtBankName.Text = string.Empty;
			txtReceivedBy.Text = string.Empty;
			CmbDebitAccount.Focus();
			txtRmks.Clear();
			txtAmount.Clear();
			table.Clear();
			grd.DataSource = table;
			Save.Visible = true;
			btnUpdate.Visible = false;
			DocumentNoFill();
			lblDr.Visible = false;
			lbldrcr.Visible = false;
			LableCreditBal.Visible = false;
			LbCr.Visible = false;
			lblCr.Visible = false;
			lblCrDr.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			CheqDate.Value = DateTime.Now;
			UpdateMode = false;
			((UltraGridBase)CmbTransType).Rows[0].Activate();
			txtChequeNoTmp.Text = string.Empty;
			txtPayeeTmp.Text = string.Empty;
			txtAmountTmp.Text = string.Empty;
			txtRupeesTmp.Text = string.Empty;
			lblSignature.Text = string.Empty;
			lblSignature.Visible = false;
			PnlCheqTmplete.Visible = false;
			datCheqDateTmp.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		((Control)(object)CmbCheqno).Text = string.Empty;
		((Control)(object)CmbCreditAccount).Text = string.Empty;
		txtAmount.Text = string.Empty;
		txtBankName.Text = string.Empty;
		txtPayTitle.Text = string.Empty;
		CmbTransType.Focus();
	}

	private void DayBookVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			Save.Enabled = formrights.DoHaveSaveRight;
			Print.Enabled = formrights.DoHavePrintRights;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			btnDelete.Enabled = formrights.DoHaveCanDelete;
			dtViewAttachments.Columns.Add("AttchmentPath");
			DebitAccountFill();
			BankAccountFill();
			TransTypeFill();
			table.Columns.Add("TransType");
			table.Columns.Add("CheqPartyId");
			table.Columns.Add("CheqParty");
			table.Columns.Add("CheqId");
			table.Columns.Add("CheqNo");
			table.Columns.Add("BankName");
			table.Columns.Add("CheqDate");
			table.Columns.Add("AccountId");
			table.Columns.Add("AccountTitle");
			table.Columns.Add("PayTitle");
			table.Columns.Add("Amount", typeof(double));
			DocDate.Focus();
			DocumentNoFill();
			grd.DataSource = table;
			Save.Visible = true;
			btnUpdate.Visible = false;
			ChkBox.Checked = true;
			lblDr.Visible = false;
			lbldrcr.Visible = false;
			LableCreditBal.Visible = false;
			LbCr.Visible = false;
			lblCr.Visible = false;
			lblCrDr.Visible = false;
			((UltraGridBase)CmbTransType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill(int NoofRecords = 0)
	{
		try
		{
			DataTable lst = CommonServices.VoucherFormHistory(formrights.DoHaveCanViewAllRecordRights, "24", NoofRecords);
			if (lst.Rows.Count > 0)
			{
				DataGridHistory.DataSource = lst;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdDetail.ClearStructure();
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns.Add("View");
			DataGridHistory.RootTable.Columns["View"].Caption = "View";
			DataGridHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["View"].ButtonText = "View";
			DataGridHistory.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void gridDetailhistoryfill(int ID)
	{
		try
		{
			VoucherHead grdlst = global::Architecture.BLL.Accounts.VoucherHead.GetByID(ID);
			if (grdlst != null)
			{
				DataTable tablehistroy = new DataTable();
				tablehistroy.Columns.Add("TransType");
				tablehistroy.Columns.Add("AccountId");
				tablehistroy.Columns.Add("AccounTitle");
				tablehistroy.Columns.Add("PayTitle");
				tablehistroy.Columns.Add("CheqPartyName");
				tablehistroy.Columns.Add("CheqDate");
				tablehistroy.Columns.Add("CheqNo");
				tablehistroy.Columns.Add("BankName");
				tablehistroy.Columns.Add("DebitAmount", typeof(double));
				tablehistroy.Columns.Add("CreditAmount", typeof(double));
				tablehistroy.Clear();
				for (int i = 0; i < grdlst.voucherDetailList.Count; i++)
				{
					tablehistroy.Rows.Add(grdlst.voucherDetailList[i].RefInvoiceNo, grdlst.voucherDetailList[i].AccountId, grdlst.voucherDetailList[i].AccountTitle, grdlst.voucherDetailList[i].PaymentType, grdlst.voucherDetailList[i].CheqPartyName, grdlst.voucherDetailList[i].DCheqDate, grdlst.voucherDetailList[i].CheqNoDetail, grdlst.voucherDetailList[i].CommentsOtherLingo, grdlst.voucherDetailList[i].DebitAmount, grdlst.voucherDetailList[i].CreditAmount);
				}
				grdDetail.DataSource = tablehistroy;
				grdDetail.RetrieveStructure();
				HistoryGridDetailSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["AccountId"].Visible = false;
			grdDetail.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["AccounTitle"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CheqPartyName"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CheqDate"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CheqNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["BankName"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["DebitAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CreditAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CheqDate"].FilterEditType = (FilterEditType)6;
			grdDetail.RootTable.Columns["CheqPartyName"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["AccounTitle"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["CheqNo"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["BankName"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["DebitAmount"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["CreditAmount"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DebitAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["CreditAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns.Add("ChequePrint");
			grdDetail.RootTable.Columns["ChequePrint"].Caption = "ChequePrint";
			grdDetail.RootTable.Columns["ChequePrint"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["ChequePrint"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["ChequePrint"].ButtonText = "ChequePrint";
			grdDetail.RootTable.Columns["ChequePrint"].Width = 80;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new VoucherDetail();
			HeaderId = Conversion.ToInt((object)ID);
			VoucherHead vh = global::Architecture.BLL.Accounts.VoucherHead.GetByID(HeaderId);
			if (vh == null)
			{
				return;
			}
			Save.Visible = false;
			btnUpdate.Visible = true;
			txtDocNo.Text = Conversion.ToString((object)vh.VoucherCode);
			DocDate.Value = Conversion.ToDateTime((object)vh.VoucherDate);
			CmbDebitAccount.Value = Conversion.ToInt((object)vh.RefAccountId);
			txtRmks.Text = Conversion.ToString((object)vh.Remarks);
			txtReceivedBy.Text = Conversion.ToString((object)vh.RemarksOtherLingo);
			txtManualNo.Text = Conversion.ToString((object)vh.ManualBillNo);
			table.Rows.Clear();
			table.Columns.Clear();
			if (table.Columns.Count == 0)
			{
				table.Columns.Add("TransType");
				table.Columns.Add("CheqPartyId");
				table.Columns.Add("CheqParty");
				table.Columns.Add("CheqId");
				table.Columns.Add("CheqNo");
				table.Columns.Add("BankName");
				table.Columns.Add("CheqDate");
				table.Columns.Add("AccountId");
				table.Columns.Add("AccountTitle");
				table.Columns.Add("PayTitle");
				table.Columns.Add("Amount", typeof(double));
			}
			for (int i = 0; i < vh.voucherDetailList.Count; i++)
			{
				if (Conversion.ToInt(CmbDebitAccount.Value) != Conversion.ToInt((object)vh.voucherDetailList[i].AccountId))
				{
					table.Rows.Add(vh.voucherDetailList[i].RefInvoiceNo, vh.voucherDetailList[i].SupplierCustomerId, vh.voucherDetailList[i].CheqPartyName, vh.voucherDetailList[i].InvoiceNoRefId, vh.voucherDetailList[i].CheqNoDetail, vh.voucherDetailList[i].CommentsOtherLingo, Conversion.ToDateTime((object)vh.voucherDetailList[i].DCheqDate).ToString("dd-MMM-yy"), vh.voucherDetailList[i].AccountId, vh.voucherDetailList[i].AccountTitle, vh.voucherDetailList[i].PaymentType, vh.voucherDetailList[i].CreditAmount);
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			gridsetting();
			UpdateMode = true;
			tabControl1.SelectedIndex = 0;
			FormHelper.LoadAttachmentsForObject(AT, HeaderId, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			global::Architecture.Model.Accounts.VoucherHead vh = new global::Architecture.Model.Accounts.VoucherHead();
			if (HeaderId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				vh.Id = HeaderId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (grd.RowCount > 0)
			{
				vh.DocumentTypeId = 24;
				vh.VoucherCode = Conversion.ToInt((object)txtDocNo.Text.Trim());
				vh.VoucherDate = DocDate.Value;
				vh.Remarks = Conversion.ToString((object)txtRmks.Text);
				vh.RemarksOtherLingo = Conversion.ToString((object)txtReceivedBy.Text);
				vh.ManualBillNo = Conversion.ToString((object)txtManualNo.Text.Trim());
				object total = grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2);
				vh.VoucherAmount = float.Parse(total.ToString());
				vh.ChequeDate = DateTime.Today;
				vh.RefAccountId = Conversion.ToInt(CmbDebitAccount.Value);
				vh.IncludeWHT = false;
				vh.OrganizationId = UserAccount.OrganizationId;
				vh.CompanyId = UserAccount.CompanyId;
				vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				vh.EntryDate = DateTime.Now;
				vh.ModifyDate = DateTime.Now;
				vh.EntryUser = UserAccount.ID;
				vh.ModifyUser = UserAccount.ID;
				vh.voucherDetailList = new List<VoucherDetail>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.RefInvoiceNo = Conversion.ToString((object)r.Cells["TransType"].Text);
					vd.SupplierCustomerId = Conversion.ToInt(r.Cells["CheqPartyId"].Value);
					vd.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					vd.AgainstAccountId = Conversion.ToInt(CmbDebitAccount.Value);
					if (Conversion.ToInt(r.Cells["CheqId"].Value) > 0)
					{
						vd.InvoiceNoRefId = Conversion.ToInt(r.Cells["CheqId"].Value);
					}
					vd.CheqNoDetail = Conversion.ToString((object)r.Cells["CheqNo"].Text);
					vd.PaymentType = Conversion.ToString((object)r.Cells["PayTitle"].Text);
					vd.CommentsOtherLingo = Conversion.ToString(r.Cells["BankName"].Value);
					vd.DCheqDate = Conversion.ToDateTime(r.Cells["CheqDate"].Value);
					vd.CreditAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vh.voucherDetailList.Add(vd);
				}
				vh.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				vh.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<global::Architecture.Model.Accounts.VoucherHead>(vh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = global::Architecture.BLL.Accounts.VoucherHead.Save(vh);
				if (HeaderId > 0)
				{
					MessageBox.Show("Voucher Update Successfully...[" + vh.VoucherCode + "]");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(HeaderId, base.Name, 24, 0);
					}
				}
				else
				{
					MessageBox.Show("Voucher Save Successfully...[" + vh.VoucherCode + "]");
				}
				if (ChkPrint1.Checked)
				{
					SlipPrint(success);
				}
				if (ChkPrint1.Checked)
				{
					CommonServices.VoucherReport_118(success, 24);
				}
				Reset();
				return;
			}
			throw new Exception("Fields Required");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Save_Click(object sender, EventArgs e)
	{
		try
		{
			HeaderId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (HeaderId == 0)
			{
				throw new Exception("Record Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				int CheqId = Conversion.ToInt((object)rows[i].Cells["CheqId"].Value.ToString());
				if (CheqId > 0 && Conversion.ToInt(CmbCheqno.Value) == CheqId)
				{
					throw new Exception("ChequeNo already in grid");
				}
			}
			table.Rows.Add(((Control)(object)CmbTransType).Text, CmbCheqPartyAccount.Value, ((Control)(object)CmbCheqPartyAccount).Text, CmbCheqno.Value, ((Control)(object)CmbCheqno).Text, txtBankName.Text.Trim(), Conversion.ToDateTime((object)CheqDate.Value).ToString("dd-MMM-yy"), CmbCreditAccount.Value, ((Control)(object)CmbCreditAccount).Text, Conversion.ToString((object)txtPayTitle.Text), txtAmount.Text.Trim());
			grd.DataSource = table;
			grd.RetrieveStructure();
			gridsetting();
			ChequeView();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChequeView()
	{
		try
		{
			if (((Control)(object)CmbTransType).Text == "Other" && ((Control)(object)CmbCheqno).Text != string.Empty)
			{
				PnlCheqTmplete.Visible = true;
				lblBankName.Visible = true;
				lblSignature.Visible = true;
				txtPayeeTmp.Text = Conversion.ToString((object)txtPayTitle.Text).ToUpper();
				int Amount = Conversion.ToInt((object)txtAmount.Text);
				txtAmountTmp.Text = Amount.ToString("#,#") + " /=";
				datCheqDateTmp.Value = Conversion.ToDateTime((object)CheqDate.Value);
				lblBankName.Text = Conversion.ToString((object)txtBankName.Text).ToUpper();
				txtChequeNoTmp.Text = Conversion.ToString((object)((Control)(object)CmbCheqno).Text);
				lblSignature.Text = UserAccount.UserName;
				if (Conversion.ToInt((object)Amount) > 0)
				{
					txtRupeesTmp.Text = ConvertWholeNumber(Conversion.ToString((object)Amount)).ToUpper();
				}
			}
			else
			{
				PnlCheqTmplete.Visible = false;
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

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null)
			{
				return;
			}
			updateDetailIndex = item.RowIndex;
			((Control)(object)CmbTransType).Text = item.Cells["TransType"].Value.ToString();
			CmbCheqPartyAccount.Value = item.Cells["CheqPartyId"].Value.ToString();
			((Control)(object)CmbCheqPartyAccount).Text = item.Cells["CheqParty"].Value.ToString();
			CmbCreditAccount.Value = item.Cells["AccountId"].Value.ToString();
			((Control)(object)CmbCreditAccount).Text = item.Cells["AccountTitle"].Value.ToString();
			txtPayTitle.Text = item.Cells["PayTitle"].Value.ToString();
			if (UpdateMode)
			{
				if (Conversion.ToInt((object)item.Cells["CheqId"].Value.ToString()) > 0)
				{
					DataTable dtCheq = new DataTable();
					dtCheq.Columns.Add("Id");
					dtCheq.Columns.Add("CheqNo");
					dtCheq.Rows.Add(item.Cells["CheqId"].Value.ToString(), item.Cells["CheqNo"].Value.ToString());
					if (dtCheq.Rows.Count > 0)
					{
						DDL.BindDDL(dtCheq, CmbCheqno, "Id", "CheqNo", "Cheq No", ZeroIndex: false);
						((UltraGridBase)CmbCheqno).Rows[0].Activate();
					}
				}
			}
			else
			{
				CmbCheqno.Value = item.Cells["CheqNo"].Value.ToString();
				((Control)(object)CmbCheqno).Text = item.Cells["CheqNo"].Text.ToString();
			}
			txtBankName.Text = item.Cells["BankName"].Value.ToString();
			CheqDate.Value = Conversion.ToDateTime(item.Cells["CheqDate"].Value);
			txtAmount.Text = item.Cells["Amount"].Value.ToString();
			btnplus.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelUpdateDetial.Visible = true;
			CmbTransType.Focus();
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
			if (FormValidationDetail())
			{
				table.Rows[updateDetailIndex]["TransType"] = ((Control)(object)CmbTransType).Text;
				table.Rows[updateDetailIndex]["CheqPartyId"] = CmbCheqPartyAccount.Value;
				table.Rows[updateDetailIndex]["CheqParty"] = ((Control)(object)CmbCheqPartyAccount).Text.Trim();
				table.Rows[updateDetailIndex]["CheqId"] = CmbCheqno.Value;
				table.Rows[updateDetailIndex]["CheqNo"] = ((Control)(object)CmbCheqno).Text.Trim();
				table.Rows[updateDetailIndex]["BankName"] = txtBankName.Text.Trim();
				table.Rows[updateDetailIndex]["CheqDate"] = Conversion.ToDateTime((object)CheqDate.Value).ToString("dd-MMM-yy");
				table.Rows[updateDetailIndex]["AccountId"] = CmbCreditAccount.Value;
				table.Rows[updateDetailIndex]["AccountTitle"] = ((Control)(object)CmbCreditAccount).Text.Trim();
				table.Rows[updateDetailIndex]["PayTitle"] = txtPayTitle.Text.Trim();
				table.Rows[updateDetailIndex]["Amount"] = txtAmount.Text.Trim();
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ChequeView();
				((Control)(object)CmbCheqno).Text = string.Empty;
				((UltraGridBase)CmbCheqno).DataSource = null;
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtAmount.Text = "";
			txtBankName.Text = "";
			CmbDebitAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void New_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DayBookVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (Save.Visible && Save.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				Save_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				New_Click(null, null);
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
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				AT.Show();
			}
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
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				gridhistoryfill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			SlipPrint(HeaderId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SlipPrint(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = null;
			if (UserAccount.CompAddress.ToString() == string.Empty)
			{
				rpt.RptPerameter("CompanyAddress", (string)null);
			}
			else
			{
				rpt.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			}
			if (UserAccount.CompName.ToString() == string.Empty)
			{
				rpt.RptPerameter("CompanyName", (string)null);
			}
			else
			{
				rpt.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			}
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@Id", PrintId.ToString());
			rpt.RptPerameter("@DocumentTypeId", "24");
			rpt.ShowReport("124-AcRptPostDatedCheqPaymentVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNo_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			int code = global::Architecture.BLL.Accounts.VoucherHead.GetVoucherHeadIdByVoucherCode(new global::Architecture.Model.Accounts.VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				VoucherCode = Conversion.ToInt((object)txtDocNo.Text.Trim()),
				DocumentTypeId = 24
			});
			if (code > 0)
			{
				HeaderId = code;
				ReadById(HeaderId);
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
			DebitAccountFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbac_Leave(object sender, EventArgs e)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		try
		{
			LableCreditBal.Text = GetCurrentBalance(Conversion.ToInt(CmbCheqPartyAccount.Value));
			double num = Conversion.ToDouble((object)LableCreditBal.Text);
			if (num > 0.0)
			{
				LbCr.Text = "Dr";
			}
			else
			{
				LbCr.Text = "Cr";
			}
			if (num == 0.0)
			{
				LbCr.Text = "Nill";
			}
			LableCreditBal.Visible = true;
			LbCr.Visible = true;
			if (((Control)(object)CmbTransType).Text == "PDC")
			{
				ReportsParameters val = new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					AccountId = Conversion.ToInt(CmbCheqPartyAccount.Value),
					Id = Conversion.ToInt(CmbCreditAccount.Value)
				};
				DataTable dtgetby = new DataTable();
				dtgetby = global::Architecture.BLL.Accounts.PdcInventory.GetByType(val);
				if (dtgetby.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtgetby, CmbCheqno, "Id", "CheqNo", "Cheq No", false);
					((Control)(object)CmbCheqno).Text = string.Empty;
				}
				else
				{
					((UltraGridBase)CmbCheqno).DataSource = null;
					((Control)(object)CmbCheqno).Text = string.Empty;
				}
			}
			else if (((Control)(object)CmbCheqPartyAccount).Text != string.Empty)
			{
				CmbCreditAccount.Value = Conversion.ToInt(CmbCheqPartyAccount.Value);
				((Control)(object)CmbCreditAccount).Text = ((Control)(object)CmbCheqPartyAccount).Text.Trim();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCheqno_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters pdc = new ReportsParameters();
			pdc.OrganizationId = UserAccount.OrganizationId;
			pdc.CompanyId = UserAccount.CompanyId;
			pdc.AccountId = Conversion.ToInt(CmbCheqPartyAccount.Value);
			pdc.Id = Conversion.ToInt(CmbCreditAccount.Value);
			pdc.CheqId = Conversion.ToInt(CmbCheqno.Value);
			if (pdc.CheqId > 0)
			{
				DataTable dtchqno = new DataTable();
				dtchqno = global::Architecture.BLL.Accounts.PdcInventory.GetByType(pdc);
				if (dtchqno.Rows.Count > 0)
				{
					txtBankName.Text = dtchqno.Rows[0]["BankName"].ToString();
					CheqDate.Value = Conversion.ToDateTime(dtchqno.Rows[0]["CheqDate"]);
					txtAmount.Text = dtchqno.Rows[0]["CheqAmount"].ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDebitAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			lblDr.Text = GetCurrentBalance(Conversion.ToInt(CmbDebitAccount.Value));
			double num = Conversion.ToDouble((object)lblDr.Text);
			if (num > 0.0)
			{
				lbldrcr.Text = "Dr";
			}
			else
			{
				lbldrcr.Text = "Cr";
			}
			if (num == 0.0)
			{
				lbldrcr.Text = "Nill";
			}
			lblDr.Visible = true;
			lbldrcr.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCreditAccount_Leave(object sender, EventArgs e)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		try
		{
			lblCr.Text = GetCurrentBalance(Conversion.ToInt(CmbCreditAccount.Value));
			double num = Conversion.ToDouble((object)LableCreditBal.Text);
			if (num > 0.0)
			{
				lblCrDr.Text = "Dr";
			}
			else
			{
				lblCrDr.Text = "Cr";
			}
			if (num == 0.0)
			{
				lblCrDr.Text = "Nill";
			}
			lblCr.Visible = true;
			lblCrDr.Visible = true;
			if (((Control)(object)CmbTransType).Text == "PDC")
			{
				ReportsParameters val = new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					AccountId = Conversion.ToInt(CmbCheqPartyAccount.Value),
					Id = Conversion.ToInt(CmbCreditAccount.Value)
				};
				DataTable dtgetby = new DataTable();
				dtgetby = global::Architecture.BLL.Accounts.PdcInventory.GetByType(val);
				if (dtgetby.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtgetby, CmbCheqno, "Id", "CheqNo", "Cheq No", false);
					((Control)(object)CmbCheqno).Text = string.Empty;
				}
				else
				{
					((UltraGridBase)CmbCheqno).DataSource = null;
					((Control)(object)CmbCheqno).Text = string.Empty;
				}
				return;
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CheqBook Enabled").ToList();
			if (list.Count <= 0 || !bool.Parse(list[0].ConfigKey.ToString()))
			{
				return;
			}
			global::Architecture.Model.Accounts.CheqBookHeader val2 = new global::Architecture.Model.Accounts.CheqBookHeader
			{
				BankId = Conversion.ToInt(CmbCreditAccount.Value)
			};
			DataTable dtout = new DataTable();
			dtout = global::Architecture.BLL.Accounts.CheqBookHeader.OutstandingCheqNo(val2);
			if (dtout.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtout, CmbCheqno, "Id", "CheqNo", "Cheq No", false);
				CmbCheqno.LimitToList = true;
				((Control)(object)CmbCheqno).Text = string.Empty;
				return;
			}
			((UltraGridBase)CmbCheqno).DataSource = null;
			((Control)(object)CmbCheqno).Text = string.Empty;
			if (!UpdateMode)
			{
				txtBankName.Text = string.Empty;
				txtAmount.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string GetCurrentBalance(int AccountId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		string Balance = "0";
		try
		{
			DataTable dtcu = global::Architecture.BLL.Accounts.VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new global::Architecture.Model.Accounts.VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DateTime.Now,
				RefAccountId = AccountId
			});
			if (dtcu.Rows.Count > 0)
			{
				Balance = Math.Round(Conversion.ToDouble(dtcu.Rows[0]["Balance"])).ToString("#,#");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Balance;
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "View")
			{
				GridEXRow item = DataGridHistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Cells["Id"].Value.ToString();
					gridDetailhistoryfill(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
				}
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

	private void btnAttachment_Click_1(object sender, EventArgs e)
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

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<global::Architecture.Model.DMSAttachments> dMSList = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTransType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbTransType).Text == "PDC")
			{
				CmbCheqno.LimitToList = true;
				txtBankName.Enabled = false;
				CheqDate.Enabled = false;
				txtAmount.Enabled = false;
			}
			else
			{
				CmbCheqno.LimitToList = false;
				txtBankName.Enabled = true;
				CheqDate.Enabled = true;
				txtAmount.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(HeaderId, 24);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (e.Column.Key == "ChequePrint")
			{
				GridEXRow r2 = grd.CurrentRow;
				if (!(Conversion.ToString(r2.Cells["TransType"].Value) == "Other"))
				{
					throw new Exception("Post Dated Cheque you cannot print");
				}
				if (!(Conversion.ToString(r2.Cells["PayTitle"].Value) != string.Empty))
				{
					throw new Exception("PayTitle Field Required");
				}
				Reporting rpt = null;
				if (dtBank.Rows.Count <= 0)
				{
					throw new Exception("Please Define Bank First against Relevent Check Template");
				}
				string ChequeTemplete = "";
				DataRow[] dr = dtBank.Select("ChartOfAccountId='" + Conversion.ToInt(r2.Cells["AccountId"].Value) + "'");
				if (dr.Length != 0)
				{
					ChequeTemplete = Conversion.ToString(dr[0].ItemArray[9]);
				}
				if (!(ChequeTemplete != string.Empty))
				{
					throw new Exception("Please Define Bank First against Relevent Check Template");
				}
				string obj = Conversion.ToDateTime(r2.Cells["CheqDate"].Value).ToString("dd-MM-yyyy");
				int digit1 = int.Parse(obj.Substring(0, 1));
				int digit2 = int.Parse(obj.Substring(1, 1));
				int digit3 = int.Parse(obj.Substring(3, 1));
				int digit4 = int.Parse(obj.Substring(4, 1));
				int digit5 = int.Parse(obj.Substring(6, 1));
				int digit6 = int.Parse(obj.Substring(7, 1));
				int digit7 = int.Parse(obj.Substring(8, 1));
				int digit8 = int.Parse(obj.Substring(9, 1));
				rpt.RptPerameter("@Account_Name", r2.Cells["PayTitle"].Value.ToString());
				rpt.RptPerameter("@Amount", r2.Cells["Amount"].Value.ToString());
				rpt.RptPerameter("@Cheque_Date", Conversion.ToDateTime(r2.Cells["CheqDate"].Value).ToString("dd-MM-yyyy"));
				rpt.RptPerameter("@digit1Date", Conversion.ToString((object)digit1));
				rpt.RptPerameter("@digit2Date", Conversion.ToString((object)digit2));
				rpt.RptPerameter("@digit3Date", Conversion.ToString((object)digit3));
				rpt.RptPerameter("@digit4Date", Conversion.ToString((object)digit4));
				rpt.RptPerameter("@digit5Date", Conversion.ToString((object)digit5));
				rpt.RptPerameter("@digit6Date", Conversion.ToString((object)digit6));
				rpt.RptPerameter("@digit7Date", Conversion.ToString((object)digit7));
				rpt.RptPerameter("@digit8Date", Conversion.ToString((object)digit8));
				rpt.RptPerameter("@ShowHeader", "False");
				rpt.ShowReport(ChequeTemplete + ".rpt");
			}
			if (!(e.Column.Key == "ChequeView"))
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (Conversion.ToString(r.Cells["TransType"].Value) == "Other" && Conversion.ToString(r.Cells["CheqNo"].Value) != string.Empty && Conversion.ToString(r.Cells["PayTitle"].Value) != string.Empty)
			{
				PnlCheqTmplete.Visible = true;
				lblBankName.Visible = true;
				lblSignature.Visible = true;
				txtPayeeTmp.Text = Conversion.ToString(r.Cells["PayTitle"].Value).ToUpper();
				int Amount = Conversion.ToInt(r.Cells["Amount"].Value);
				txtAmountTmp.Text = Amount.ToString("#,#") + " /=";
				datCheqDateTmp.Value = Conversion.ToDateTime(r.Cells["CheqDate"].Value);
				lblBankName.Text = Conversion.ToString(r.Cells["BankName"].Value).ToUpper();
				txtChequeNoTmp.Text = Conversion.ToString(r.Cells["CheqNo"].Value);
				lblSignature.Text = UserAccount.UserName;
				if (Conversion.ToInt(r.Cells["Amount"].Value) > 0)
				{
					txtRupeesTmp.Text = ConvertWholeNumber(Conversion.ToString(r.Cells["Amount"].Value)).ToUpper();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (!(e.Column.Key == "ChequePrint"))
			{
				return;
			}
			GridEXRow r = grdDetail.CurrentRow;
			if (Conversion.ToString(r.Cells["TransType"].Value) == "Other")
			{
				if (Conversion.ToString(r.Cells["PayTitle"].Value) != string.Empty)
				{
					Reporting rpt = null;
					if (dtBank.Rows.Count > 0)
					{
						string ChequeTemplete = "";
						for (int i = 0; i < dtBank.Rows.Count; i++)
						{
							if (Conversion.ToInt(r.Cells["AccountId"].Value) == Conversion.ToInt(dtBank.Rows[i]["ChartOfAccountId"]))
							{
								ChequeTemplete = Conversion.ToString(dtBank.Rows[i]["ChequeTemplete"]);
								break;
							}
						}
						if (ChequeTemplete != string.Empty)
						{
							string obj = Conversion.ToDateTime(r.Cells["CheqDate"].Value).ToString("dd-MM-yyyy");
							int digit1 = int.Parse(obj.Substring(0, 1));
							int digit2 = int.Parse(obj.Substring(1, 1));
							int digit3 = int.Parse(obj.Substring(3, 1));
							int digit4 = int.Parse(obj.Substring(4, 1));
							int digit5 = int.Parse(obj.Substring(6, 1));
							int digit6 = int.Parse(obj.Substring(7, 1));
							int digit7 = int.Parse(obj.Substring(8, 1));
							int digit8 = int.Parse(obj.Substring(9, 1));
							rpt.RptPerameter("@Account_Name", r.Cells["PayTitle"].Value.ToString());
							rpt.RptPerameter("@Amount", r.Cells["CreditAmount"].Value.ToString());
							rpt.RptPerameter("@Cheque_Date", Conversion.ToDateTime(r.Cells["CheqDate"].Value).ToString("dd-MM-yyyy"));
							rpt.RptPerameter("@digit1Date", Conversion.ToString((object)digit1));
							rpt.RptPerameter("@digit2Date", Conversion.ToString((object)digit2));
							rpt.RptPerameter("@digit3Date", Conversion.ToString((object)digit3));
							rpt.RptPerameter("@digit4Date", Conversion.ToString((object)digit4));
							rpt.RptPerameter("@digit5Date", Conversion.ToString((object)digit5));
							rpt.RptPerameter("@digit6Date", Conversion.ToString((object)digit6));
							rpt.RptPerameter("@digit7Date", Conversion.ToString((object)digit7));
							rpt.RptPerameter("@digit8Date", Conversion.ToString((object)digit8));
							rpt.RptPerameter("@ShowHeader", "False");
							rpt.ShowReport(ChequeTemplete + ".rpt");
							return;
						}
						throw new Exception("Please Define Bank First against Relevent Check Template");
					}
					throw new Exception("Please Define Bank First against Relevent Check Template");
				}
				throw new Exception("PayTitle Field Required");
			}
			throw new Exception("Post Dated Cheque you cannot print");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private static string ones(string Number)
	{
		int _Number = Convert.ToInt32(Number);
		string name = "";
		switch (_Number)
		{
		case 1:
			name = "One";
			break;
		case 2:
			name = "Two";
			break;
		case 3:
			name = "Three";
			break;
		case 4:
			name = "Four";
			break;
		case 5:
			name = "Five";
			break;
		case 6:
			name = "Six";
			break;
		case 7:
			name = "Seven";
			break;
		case 8:
			name = "Eight";
			break;
		case 9:
			name = "Nine";
			break;
		}
		return name;
	}

	private static string tens(string Number)
	{
		int _Number = Convert.ToInt32(Number);
		string name = null;
		switch (_Number)
		{
		case 10:
			name = "Ten";
			break;
		case 11:
			name = "Eleven";
			break;
		case 12:
			name = "Twelve";
			break;
		case 13:
			name = "Thirteen";
			break;
		case 14:
			name = "Fourteen";
			break;
		case 15:
			name = "Fifteen";
			break;
		case 16:
			name = "Sixteen";
			break;
		case 17:
			name = "Seventeen";
			break;
		case 18:
			name = "Eighteen";
			break;
		case 19:
			name = "Nineteen";
			break;
		case 20:
			name = "Twenty";
			break;
		case 30:
			name = "Thirty";
			break;
		case 40:
			name = "Fourty";
			break;
		case 50:
			name = "Fifty";
			break;
		case 60:
			name = "Sixty";
			break;
		case 70:
			name = "Seventy";
			break;
		case 80:
			name = "Eighty";
			break;
		case 90:
			name = "Ninety";
			break;
		default:
			if (_Number > 0)
			{
				name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
			}
			break;
		}
		return name;
	}

	private static string ConvertWholeNumber(string Number)
	{
		string word = "";
		try
		{
			bool isDone = false;
			if (Convert.ToDouble(Number) > 0.0)
			{
				Number.StartsWith("0");
				int numDigits = Number.Length;
				int pos = 0;
				string place = "";
				switch (numDigits)
				{
				case 1:
					word = ones(Number);
					isDone = true;
					break;
				case 2:
					word = tens(Number);
					isDone = true;
					break;
				case 3:
					pos = numDigits % 3 + 1;
					place = " Hundred ";
					break;
				case 4:
				case 5:
				case 6:
					pos = numDigits % 4 + 1;
					place = " Thousand ";
					break;
				case 7:
				case 8:
				case 9:
					pos = numDigits % 7 + 1;
					place = " Million ";
					break;
				case 10:
				case 11:
				case 12:
					pos = numDigits % 10 + 1;
					place = " Billion ";
					break;
				default:
					isDone = true;
					break;
				}
				if (!isDone)
				{
					if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
					{
						try
						{
							word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
						}
						catch
						{
						}
					}
					else
					{
						word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
					}
				}
				if (word.Trim().Equals(place.Trim()))
				{
					word = "";
				}
			}
		}
		catch
		{
		}
		return word.Trim();
	}

	private void PostDatedCheqPaymentVouchers_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
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
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Expected O, but got Unknown
		//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a49: Expected O, but got Unknown
		//IL_4859: Unknown result type (might be due to invalid IL or missing references)
		//IL_4863: Expected O, but got Unknown
		//IL_49cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_49d7: Expected O, but got Unknown
		//IL_49e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_49ee: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.PostDatedCheqPaymentVouchers));
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtReceivedBy = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtManualNo = new System.Windows.Forms.TextBox();
		this.PnlCheqTmplete = new System.Windows.Forms.Panel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.lblBankName = new System.Windows.Forms.Label();
		this.lblSignature = new System.Windows.Forms.Label();
		this.txtPayeeTmp = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtAmountTmp = new System.Windows.Forms.TextBox();
		this.txtChequeNoTmp = new System.Windows.Forms.TextBox();
		this.txtRupeesTmp = new System.Windows.Forms.RichTextBox();
		this.datCheqDateTmp = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbTransType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.lblCr = new System.Windows.Forms.Label();
		this.lblCrDr = new System.Windows.Forms.Label();
		this.lblDr = new System.Windows.Forms.Label();
		this.lbldrcr = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.CmbCreditAccount = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.CheqDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.CmbCheqno = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.LableCreditBal = new System.Windows.Forms.Label();
		this.LbCr = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.btnplus = new System.Windows.Forms.Button();
		this.CmbCheqPartyAccount = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbDebitAccount = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtRmks = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtPayTitle = new System.Windows.Forms.TextBox();
		this.txtBankName = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnVoucher = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.ChkPrint2 = new System.Windows.Forms.CheckBox();
		this.ChkPrint1 = new System.Windows.Forms.CheckBox();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.PnlCheqTmplete.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqno).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqPartyAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1301, 698);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1301, 698);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1293, 672);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 300);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1289, 370);
		this.panel5.TabIndex = 3;
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1289, 370);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.txtReceivedBy);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.txtManualNo);
		this.panel4.Controls.Add(this.PnlCheqTmplete);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransType);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.lblCr);
		this.panel4.Controls.Add(this.lblCrDr);
		this.panel4.Controls.Add(this.lblDr);
		this.panel4.Controls.Add(this.lbldrcr);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.CheqDate);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCheqno);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.LableCreditBal);
		this.panel4.Controls.Add(this.LbCr);
		this.panel4.Controls.Add(this.txtDocNo);
		this.panel4.Controls.Add(this.btnCancelUpdateDetial);
		this.panel4.Controls.Add(this.btnUpdateDetail);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.btnplus);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCheqPartyAccount);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccount);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtRmks);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtAmount);
		this.panel4.Controls.Add(this.txtPayTitle);
		this.panel4.Controls.Add(this.txtBankName);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 67);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1289, 233);
		this.panel4.TabIndex = 2;
		this.txtReceivedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.txtReceivedBy.Location = new System.Drawing.Point(525, 88);
		this.txtReceivedBy.Name = "txtReceivedBy";
		this.txtReceivedBy.Size = new System.Drawing.Size(157, 22);
		this.txtReceivedBy.TabIndex = 5;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(526, 67);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(74, 15);
		this.label21.TabIndex = 87;
		this.label21.Text = "Received By";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(222, 9);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(68, 15);
		this.label20.TabIndex = 86;
		this.label20.Text = "Manual No";
		this.txtManualNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualNo.Location = new System.Drawing.Point(225, 34);
		this.txtManualNo.Name = "txtManualNo";
		this.txtManualNo.Size = new System.Drawing.Size(82, 22);
		this.txtManualNo.TabIndex = 2;
		this.PnlCheqTmplete.BackgroundImage = (System.Drawing.Image)resources.GetObject("PnlCheqTmplete.BackgroundImage");
		this.PnlCheqTmplete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.PnlCheqTmplete.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.PnlCheqTmplete.Controls.Add(this.pictureBox1);
		this.PnlCheqTmplete.Controls.Add(this.lblBankName);
		this.PnlCheqTmplete.Controls.Add(this.lblSignature);
		this.PnlCheqTmplete.Controls.Add(this.txtPayeeTmp);
		this.PnlCheqTmplete.Controls.Add(this.label19);
		this.PnlCheqTmplete.Controls.Add(this.label5);
		this.PnlCheqTmplete.Controls.Add(this.label14);
		this.PnlCheqTmplete.Controls.Add(this.label6);
		this.PnlCheqTmplete.Controls.Add(this.label13);
		this.PnlCheqTmplete.Controls.Add(this.txtAmountTmp);
		this.PnlCheqTmplete.Controls.Add(this.txtChequeNoTmp);
		this.PnlCheqTmplete.Controls.Add(this.txtRupeesTmp);
		this.PnlCheqTmplete.Controls.Add(this.datCheqDateTmp);
		this.PnlCheqTmplete.Controls.Add(this.label12);
		this.PnlCheqTmplete.Controls.Add(this.label7);
		this.PnlCheqTmplete.Location = new System.Drawing.Point(706, 9);
		this.PnlCheqTmplete.Name = "PnlCheqTmplete";
		this.PnlCheqTmplete.Size = new System.Drawing.Size(577, 182);
		this.PnlCheqTmplete.TabIndex = 84;
		this.PnlCheqTmplete.Visible = false;
		this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
		this.pictureBox1.Image = Architecture.WinApp.Properties.Resources.DummyBank;
		this.pictureBox1.Location = new System.Drawing.Point(5, 2);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(58, 50);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 86;
		this.pictureBox1.TabStop = false;
		this.lblBankName.AutoSize = true;
		this.lblBankName.BackColor = System.Drawing.Color.Transparent;
		this.lblBankName.Font = new System.Drawing.Font("Segoe UI", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBankName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblBankName.Location = new System.Drawing.Point(71, 8);
		this.lblBankName.Name = "lblBankName";
		this.lblBankName.Size = new System.Drawing.Size(137, 32);
		this.lblBankName.TabIndex = 85;
		this.lblBankName.Text = "Bank Name";
		this.lblBankName.Visible = false;
		this.lblSignature.AutoSize = true;
		this.lblSignature.BackColor = System.Drawing.Color.Transparent;
		this.lblSignature.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSignature.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblSignature.Location = new System.Drawing.Point(481, 121);
		this.lblSignature.Name = "lblSignature";
		this.lblSignature.Size = new System.Drawing.Size(58, 15);
		this.lblSignature.TabIndex = 84;
		this.lblSignature.Text = "Signature";
		this.lblSignature.Visible = false;
		this.txtPayeeTmp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPayeeTmp.Enabled = false;
		this.txtPayeeTmp.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPayeeTmp.ForeColor = System.Drawing.Color.Black;
		this.txtPayeeTmp.Location = new System.Drawing.Point(67, 61);
		this.txtPayeeTmp.Name = "txtPayeeTmp";
		this.txtPayeeTmp.Size = new System.Drawing.Size(296, 23);
		this.txtPayeeTmp.TabIndex = 78;
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.Transparent;
		this.label19.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label19.Location = new System.Drawing.Point(495, 156);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(58, 15);
		this.label19.TabIndex = 83;
		this.label19.Text = "Signature";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Transparent;
		this.label5.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label5.Location = new System.Drawing.Point(375, 8);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(66, 15);
		this.label5.TabIndex = 74;
		this.label5.Text = "Cheque No";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.Transparent;
		this.label14.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label14.Location = new System.Drawing.Point(461, 138);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(107, 15);
		this.label14.TabIndex = 83;
		this.label14.Text = "-------------------------";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Transparent;
		this.label6.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label6.Location = new System.Drawing.Point(375, 34);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(75, 15);
		this.label6.TabIndex = 74;
		this.label6.Text = "Cheque Date";
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.Transparent;
		this.label13.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label13.Location = new System.Drawing.Point(376, 80);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(51, 15);
		this.label13.TabIndex = 74;
		this.label13.Text = "Amount";
		this.txtAmountTmp.Enabled = false;
		this.txtAmountTmp.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountTmp.ForeColor = System.Drawing.SystemColors.MenuText;
		this.txtAmountTmp.Location = new System.Drawing.Point(463, 77);
		this.txtAmountTmp.Name = "txtAmountTmp";
		this.txtAmountTmp.Size = new System.Drawing.Size(105, 23);
		this.txtAmountTmp.TabIndex = 81;
		this.txtChequeNoTmp.Enabled = false;
		this.txtChequeNoTmp.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChequeNoTmp.Location = new System.Drawing.Point(463, 5);
		this.txtChequeNoTmp.Name = "txtChequeNoTmp";
		this.txtChequeNoTmp.Size = new System.Drawing.Size(105, 23);
		this.txtChequeNoTmp.TabIndex = 75;
		this.txtRupeesTmp.Enabled = false;
		this.txtRupeesTmp.ForeColor = System.Drawing.SystemColors.ControlText;
		this.txtRupeesTmp.Location = new System.Drawing.Point(67, 88);
		this.txtRupeesTmp.Name = "txtRupeesTmp";
		this.txtRupeesTmp.Size = new System.Drawing.Size(296, 48);
		this.txtRupeesTmp.TabIndex = 80;
		this.txtRupeesTmp.Text = "";
		this.datCheqDateTmp.CalendarForeColor = System.Drawing.Color.Transparent;
		this.datCheqDateTmp.CustomFormat = "dd-MM-yyyy";
		this.datCheqDateTmp.Enabled = false;
		this.datCheqDateTmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datCheqDateTmp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datCheqDateTmp.Location = new System.Drawing.Point(463, 30);
		this.datCheqDateTmp.Name = "datCheqDateTmp";
		this.datCheqDateTmp.Size = new System.Drawing.Size(105, 21);
		this.datCheqDateTmp.TabIndex = 76;
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.Transparent;
		this.label12.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label12.Location = new System.Drawing.Point(7, 102);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(47, 15);
		this.label12.TabIndex = 79;
		this.label12.Text = "Rupees";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Transparent;
		this.label7.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label7.Location = new System.Drawing.Point(7, 64);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(39, 15);
		this.label7.TabIndex = 77;
		this.label7.Text = "Payee";
		this.CmbTransType.AllowNull = (DefaultableBoolean)1;
		this.CmbTransType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbTransType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbTransType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbTransType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransType).Location = new System.Drawing.Point(10, 141);
		((System.Windows.Forms.Control)(object)this.CmbTransType).Name = "CmbTransType";
		((System.Windows.Forms.Control)(object)this.CmbTransType).Size = new System.Drawing.Size(103, 25);
		((System.Windows.Forms.Control)(object)this.CmbTransType).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbTransType).Leave += new System.EventHandler(CmbTransType_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 117);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 15);
		this.label1.TabIndex = 72;
		this.label1.Text = "Trans Type";
		this.lblCr.AutoSize = true;
		this.lblCr.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCr.Location = new System.Drawing.Point(464, 118);
		this.lblCr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblCr.Name = "lblCr";
		this.lblCr.Size = new System.Drawing.Size(44, 13);
		this.lblCr.TabIndex = 70;
		this.lblCr.Text = "label17";
		this.lblCrDr.AutoSize = true;
		this.lblCrDr.Location = new System.Drawing.Point(444, 118);
		this.lblCrDr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblCrDr.Name = "lblCrDr";
		this.lblCrDr.Size = new System.Drawing.Size(16, 13);
		this.lblCrDr.TabIndex = 69;
		this.lblCrDr.Text = "Bl";
		this.lblDr.AutoSize = true;
		this.lblDr.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDr.Location = new System.Drawing.Point(512, 9);
		this.lblDr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblDr.Name = "lblDr";
		this.lblDr.Size = new System.Drawing.Size(44, 13);
		this.lblDr.TabIndex = 68;
		this.lblDr.Text = "label17";
		this.lbldrcr.AutoSize = true;
		this.lbldrcr.Location = new System.Drawing.Point(492, 9);
		this.lbldrcr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lbldrcr.Name = "lbldrcr";
		this.lbldrcr.Size = new System.Drawing.Size(16, 13);
		this.lbldrcr.TabIndex = 67;
		this.lbldrcr.Text = "Bl";
		this.ChkBox.AutoSize = true;
		this.ChkBox.Location = new System.Drawing.Point(593, 39);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(88, 17);
		this.ChkBox.TabIndex = 6;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(333, 142);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(190, 25);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Leave += new System.EventHandler(CmbCreditAccount_Leave);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(329, 117);
		this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(100, 15);
		this.label18.TabIndex = 65;
		this.label18.Text = "Control Credit A/c";
		this.CheqDate.CustomFormat = "dd-MMM-yyyy";
		this.CheqDate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.CheqDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.CheqDate.Location = new System.Drawing.Point(329, 198);
		this.CheqDate.Name = "CheqDate";
		this.CheqDate.Size = new System.Drawing.Size(126, 25);
		this.CheqDate.TabIndex = 13;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label17.Location = new System.Drawing.Point(333, 172);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(69, 17);
		this.label17.TabIndex = 62;
		this.label17.Text = "Cheq Date";
		this.CmbCheqno.AllowNull = (DefaultableBoolean)1;
		this.CmbCheqno.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCheqno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCheqno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCheqno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCheqno).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCheqno).Location = new System.Drawing.Point(528, 142);
		((System.Windows.Forms.Control)(object)this.CmbCheqno).Name = "CmbCheqno";
		((System.Windows.Forms.Control)(object)this.CmbCheqno).Size = new System.Drawing.Size(154, 25);
		((System.Windows.Forms.Control)(object)this.CmbCheqno).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.CmbCheqno).Leave += new System.EventHandler(CmbCheqno_Leave);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(527, 117);
		this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(69, 15);
		this.label16.TabIndex = 60;
		this.label16.Text = "Cheque No";
		this.LableCreditBal.AutoSize = true;
		this.LableCreditBal.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.LableCreditBal.Location = new System.Drawing.Point(253, 118);
		this.LableCreditBal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.LableCreditBal.Name = "LableCreditBal";
		this.LableCreditBal.Size = new System.Drawing.Size(44, 13);
		this.LableCreditBal.TabIndex = 52;
		this.LableCreditBal.Text = "label17";
		this.LbCr.AutoSize = true;
		this.LbCr.Location = new System.Drawing.Point(233, 118);
		this.LbCr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.LbCr.Name = "LbCr";
		this.LbCr.Size = new System.Drawing.Size(16, 13);
		this.LbCr.TabIndex = 51;
		this.LbCr.Text = "Bl";
		this.txtDocNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(130, 34);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(92, 22);
		this.txtDocNo.TabIndex = 1;
		this.txtDocNo.Leave += new System.EventHandler(txtDocNo_TextChanged);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(688, 197);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(64, 26);
		this.btnCancelUpdateDetial.TabIndex = 17;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(614, 197);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 26);
		this.btnUpdateDetail.TabIndex = 16;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(126, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 45;
		this.label3.Text = "Doc No";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(7, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 43;
		this.label2.Text = "Doc Date";
		this.DocDate.CustomFormat = "dd-MMM-yyyy";
		this.DocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(10, 34);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(117, 22);
		this.DocDate.TabIndex = 0;
		this.btnplus.BackColor = System.Drawing.Color.White;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnplus.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.Location = new System.Drawing.Point(573, 197);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 15;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.CmbCheqPartyAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCheqPartyAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCheqPartyAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCheqPartyAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCheqPartyAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCheqPartyAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCheqPartyAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCheqPartyAccount).Location = new System.Drawing.Point(116, 142);
		((System.Windows.Forms.Control)(object)this.CmbCheqPartyAccount).Name = "CmbCheqPartyAccount";
		((System.Windows.Forms.Control)(object)this.CmbCheqPartyAccount).Size = new System.Drawing.Size(213, 25);
		((System.Windows.Forms.Control)(object)this.CmbCheqPartyAccount).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbCheqPartyAccount).Leave += new System.EventHandler(cmbac_Leave);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(113, 117);
		this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(80, 15);
		this.label15.TabIndex = 23;
		this.label15.Text = "Cheque Party";
		this.CmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Location = new System.Drawing.Point(310, 33);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Name = "CmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Size = new System.Drawing.Size(280, 25);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Leave += new System.EventHandler(CmbDebitAccount_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(307, 7);
		this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(55, 15);
		this.label11.TabIndex = 23;
		this.label11.Text = "Debit A/c";
		this.txtRmks.BackColor = System.Drawing.Color.White;
		this.txtRmks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRmks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRmks.Location = new System.Drawing.Point(10, 88);
		this.txtRmks.Name = "txtRmks";
		this.txtRmks.Size = new System.Drawing.Size(513, 22);
		this.txtRmks.TabIndex = 4;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(10, 64);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(69, 15);
		this.label10.TabIndex = 31;
		this.label10.Text = "Description";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(460, 173);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(49, 15);
		this.label9.TabIndex = 32;
		this.label9.Text = "Amount";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label4.Location = new System.Drawing.Point(11, 172);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(56, 17);
		this.label4.TabIndex = 33;
		this.label4.Text = "Pay Title";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label8.Location = new System.Drawing.Point(178, 172);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(74, 17);
		this.label8.TabIndex = 33;
		this.label8.Text = "Bank Name";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(457, 198);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(113, 25);
		this.txtAmount.TabIndex = 14;
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.txtPayTitle.BackColor = System.Drawing.Color.White;
		this.txtPayTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPayTitle.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtPayTitle.Location = new System.Drawing.Point(10, 198);
		this.txtPayTitle.Name = "txtPayTitle";
		this.txtPayTitle.Size = new System.Drawing.Size(164, 25);
		this.txtPayTitle.TabIndex = 11;
		this.txtBankName.BackColor = System.Drawing.Color.White;
		this.txtBankName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBankName.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtBankName.Location = new System.Drawing.Point(177, 198);
		this.txtBankName.Name = "txtBankName";
		this.txtBankName.Size = new System.Drawing.Size(151, 25);
		this.txtBankName.TabIndex = 12;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ChkPrint2);
		this.panel3.Controls.Add(this.ChkPrint1);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 29);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1289, 38);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(307, 24);
		this.label23.TabIndex = 1;
		this.label23.Text = "Post Dated Cheq Payment Voucher";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1289, 27);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1239, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.New, this.btnUpdate, this.Save, this.Print, this.btnRefresh, this.btnAttachment, this.btnDelete, this.btnVoucher });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1289, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(55, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(New_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(55, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(79, 24);
		this.Print.Text = "&124-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click_1);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucher.Name = "btnVoucher";
		this.btnVoucher.Size = new System.Drawing.Size(97, 24);
		this.btnVoucher.Text = "118-Voucher";
		this.btnVoucher.Click += new System.EventHandler(btnVoucher_Click);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1293, 672);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 28);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1289, 642);
		this.panel6.TabIndex = 1;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 377);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1289, 265);
		this.panel9.TabIndex = 1;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1289, 265);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1289, 377);
		this.panel8.TabIndex = 0;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1289, 377);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.panel7.Controls.Add(this.ctrlGrdBar3);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.toolStrip3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1289, 26);
		this.panel7.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1188, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1244, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1289, 25);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(51, 22);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.ChkPrint2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint2.AutoSize = true;
		this.ChkPrint2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint2.ForeColor = System.Drawing.Color.White;
		this.ChkPrint2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint2.Location = new System.Drawing.Point(1163, 10);
		this.ChkPrint2.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint2.Name = "ChkPrint2";
		this.ChkPrint2.Size = new System.Drawing.Size(121, 19);
		this.ChkPrint2.TabIndex = 14;
		this.ChkPrint2.TabStop = false;
		this.ChkPrint2.Text = "Voucher Preview";
		this.ChkPrint2.UseVisualStyleBackColor = true;
		this.ChkPrint1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint1.AutoSize = true;
		this.ChkPrint1.Checked = true;
		this.ChkPrint1.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrint1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint1.ForeColor = System.Drawing.Color.White;
		this.ChkPrint1.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint1.Location = new System.Drawing.Point(1057, 10);
		this.ChkPrint1.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint1.Name = "ChkPrint1";
		this.ChkPrint1.Size = new System.Drawing.Size(95, 19);
		this.ChkPrint1.TabIndex = 13;
		this.ChkPrint1.TabStop = false;
		this.ChkPrint1.Text = "Slip Preview";
		this.ChkPrint1.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1301, 698);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PostDatedCheqPaymentVouchers";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PostDatedCheqPaymentVouchers_FormClosing);
		base.Load += new System.EventHandler(DayBookVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DayBookVoucher_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.PnlCheqTmplete.ResumeLayout(false);
		this.PnlCheqTmplete.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqno).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqPartyAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}