// using Architecture.Reports;
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
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
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

public class AccountsPrematurePaymentsReceipts : Form
{
	private string TrackingNo = "";

	private int RecId;

	private int VoucherHeadId;

	private int updateDetailIndex;

	private bool IsBookingOffice_App;

	private bool IsCustomerPortal_App;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private DataTable table = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private ContextMenuStrip contextMenuStrip1;

	private PrintDialog printDialog1;

	private Panel panel5;

	internal UltraCombo cmbSenderbank;

	internal UltraCombo cmbRepresentiveAc;

	private Button button1;

	private Label label19;

	private Label label9;

	internal UltraCombo cmbVouchertype;

	internal UltraCombo CmbSenderAccount;

	internal UltraCombo CmbReceiverAccount;

	private DateTimePicker txtdocdate;

	private TextBox txtTrackingNo;

	private TextBox txtremarks;

	private Label label13;

	private TextBox txtdocno;

	private Label lblReceiverAc;

	private Label label3;

	private Label label10;

	private Label label4;

	private Label label2;

	private Label label40;

	private Panel panel4;

	private Label lblCaption;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton toolStripButton8;

	private ToolStripButton toolStripButton1;

	internal UltraCombo cmbStatus;

	private Label label1;

	private TabPage tabPage2;

	private Panel panel6;

	private Label label8;

	private TextBox txtSlipAmount;

	private Label label18;

	private DateTimePicker datChequeDate;

	private Label label15;

	private TextBox txtChequeNo;

	private Button btnDefineOtherParties;

	private Label label20;

	private TextBox txtAmount;

	private Panel panel7;

	private Panel panel3;

	internal GridEX grdPendingRecord;

	internal GridEX grd;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button Add;

	internal GridEX grdHistory;

	private Panel panel9;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripButton btnConfirmVoucher;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnShortCutKey;

	private Panel panel10;

	private Label label5;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel12;

	private Label label21;

	private Panel panel11;

	private GroupBox groupBox3;

	private Label label12;

	internal UltraCombo CmbReceiverAcHistory;

	private Label label17;

	internal UltraCombo CmbSenderAcHistory;

	private Label label11;

	internal UltraCombo CmbBankNameHistory;

	private Button btnshow;

	private Label label6;

	private Label label16;

	private Label label14;

	internal UltraCombo CmbVoucherTypeHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	internal UltraCombo CmbCostCenter;

	private Label lblCostCenter;

	public AccountsPrematurePaymentsReceipts(UserAccount puserAccount)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void CostCenterFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dt = UsersWithCostCenter.UsersWithCostCenter_AllocatedData(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCostCenter, "CostCenterId", "CostCenterName", "Cost Center", false);
				((UltraGridBase)CmbCostCenter).Rows[0].Activate();
			}
			else
			{
				((Control)(object)CmbCostCenter).Text = string.Empty;
				((UltraGridBase)CmbCostCenter).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int Code = AccountsPrematurePaymentsReceipts.GenerateCode(new AccountsPrematureReceipts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 159,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(3, "CRV");
			dt.Rows.Add(4, "BRV");
			if (!IsBookingOffice_App && UserAccount.AppId != 4 && UserAccount.AppId != 6)
			{
				dt.Rows.Add(5, "JV");
			}
			DDL.BindDDL(dt, cmbVouchertype, "Id", "Name", "VoucherType", ZeroIndex: false);
			((UltraGridBase)cmbVouchertype).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bankfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int bankId = Conversion.ToInt(cmbSenderbank.Value);
			DataTable dtbnk = global::Architecture.BLL.PdcBankName.Getall(new global::Architecture.Model.PdcBankName
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtbnk.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtbnk, cmbSenderbank, "Id", "BankName", "BankName", ZeroIndex: true);
			if (bankId > 0)
			{
				if (dtbnk.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == bankId))
				{
					cmbSenderbank.Value = bankId;
				}
				else
				{
					((Control)(object)cmbSenderbank).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Pending");
			DDL.BindDDL(dt, cmbStatus, "Id", "Name", "Name", ZeroIndex: false);
			((UltraGridBase)cmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherPartiesFill()
	{
		try
		{
			int PartyId = Conversion.ToInt(cmbRepresentiveAc.Value);
			DataTable dt = CommonServices.GetOtherPartiesForPrematureReciept();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, cmbRepresentiveAc, "Id", "PartyName", "Representative AC", ZeroIndex: true);
			if (PartyId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == PartyId))
				{
					cmbRepresentiveAc.Value = PartyId;
				}
				else
				{
					((Control)(object)cmbRepresentiveAc).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReceiverAccountBind()
	{
		try
		{
			if (((UltraGridBase)cmbVouchertype).ActiveRow == null)
			{
				return;
			}
			int ReceiverAc = Conversion.ToInt(CmbReceiverAccount.Value);
			DataTable dt = new DataTable();
			if (Conversion.ToInt(cmbVouchertype.Value) == 4)
			{
				dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("15");
				DDL.BindDDL(dt, CmbReceiverAccount, "Id", "AccountTitle", "Bank Account", ZeroIndex: true);
			}
			else if (Conversion.ToInt(cmbVouchertype.Value) == 3)
			{
				dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("2");
				DDL.BindDDL(dt, CmbReceiverAccount, "Id", "AccountTitle", "Cash Account", ZeroIndex: true);
			}
			else if (Conversion.ToInt(cmbVouchertype.Value) == 5)
			{
				dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8");
				DDL.BindDDL(dt, CmbReceiverAccount, "Id", "AccountTitle", "Receiver Account", ZeroIndex: true);
			}
			if (ReceiverAc > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ReceiverAc))
				{
					CmbReceiverAccount.Value = ReceiverAc;
				}
				else
				{
					((Control)(object)CmbReceiverAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SenderPartiesBind(int CostCenterId = 0)
	{
		try
		{
			int senderId = Conversion.ToInt(CmbSenderAccount.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8", null, CostCenterId);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, CmbSenderAccount, "Id", "AccountTitle", "SenderAccount", ZeroIndex: true);
			if (senderId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == senderId))
				{
					CmbSenderAccount.Value = senderId;
				}
				else
				{
					((Control)(object)CmbSenderAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("Doc No field required");
			txtdocno.Focus();
			return false;
		}
		if (txtTrackingNo.Text.Trim() == "" || txtTrackingNo.Text.Trim() == "0")
		{
			MessageBox.Show("Tracking No field required");
			txtTrackingNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbVouchertype).ActiveRow == null || Conversion.ToInt(cmbVouchertype.Value) == 0)
		{
			MessageBox.Show("Please Select Voucher type");
			cmbVouchertype.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbVouchertype.Value) == 5 && Conversion.ToInt(CmbSenderAccount.Value) == Conversion.ToInt(CmbReceiverAccount.Value))
		{
			MessageBox.Show("SenderAccount cannot be equal to ReceiverAccount Please Check!");
			CmbSenderAccount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSenderbank).ActiveRow == null || Conversion.ToInt(cmbSenderbank.Value) == 0)
		{
			MessageBox.Show("Sender Bank Field Required");
			cmbSenderbank.Focus();
			return false;
		}
		if (((UltraGridBase)cmbRepresentiveAc).ActiveRow == null || Conversion.ToInt(cmbRepresentiveAc.Value) == 0)
		{
			MessageBox.Show("Please Select Represented Account");
			cmbRepresentiveAc.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSenderAccount).ActiveRow == null || Conversion.ToInt(CmbSenderAccount.Value) == 0)
		{
			MessageBox.Show("Please Select Sender Account");
			CmbSenderAccount.Focus();
			return false;
		}
		if (!IsBookingOffice_App && !IsCustomerPortal_App && (((UltraGridBase)CmbReceiverAccount).ActiveRow == null || Conversion.ToInt(CmbReceiverAccount.Value) == 0))
		{
			MessageBox.Show("Please Select Receiver Account");
			CmbReceiverAccount.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbVouchertype.Value) == 4 && (txtChequeNo.Text == null || txtChequeNo.Text == string.Empty))
		{
			MessageBox.Show("ChequeNo Field Required");
			txtChequeNo.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmount.Text) == 0.0)
		{
			MessageBox.Show("Amount Field is Required");
			txtAmount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbStatus).ActiveRow == null || Conversion.ToInt(cmbStatus.Value) == 0)
		{
			MessageBox.Show("Please Select Status");
			cmbStatus.Focus();
			return false;
		}
		return true;
	}

	private void AcfrmDefPdcManagment_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1320, 625);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("DocDate", typeof(DateTime));
			table.Columns.Add("DocNo", typeof(int));
			table.Columns.Add("TrakingNo");
			table.Columns.Add("SlipAmount", typeof(double));
			table.Columns.Add("VoucherTypeId");
			table.Columns.Add("VoucherType");
			table.Columns.Add("BankId");
			table.Columns.Add("BankName");
			table.Columns.Add("RepresentativeAcId");
			table.Columns.Add("RepresentativeAc");
			table.Columns.Add("SenderAcId");
			table.Columns.Add("SenderAc");
			table.Columns.Add("ReceiverAcId");
			table.Columns.Add("ReceiverAc");
			table.Columns.Add("ChequeNo");
			table.Columns.Add("ChequeDate", typeof(DateTime));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("Status");
			IsBookingOffice_App = UserAccount.AppId == 5;
			if (IsBookingOffice_App)
			{
				((Control)(object)CmbCostCenter).Enabled = false;
				lblCostCenter.Visible = true;
				((Control)(object)CmbCostCenter).Visible = true;
				btnConfirmVoucher.Visible = false;
				CostCenterFill();
			}
			else
			{
				lblCostCenter.Visible = false;
				((Control)(object)CmbCostCenter).Visible = false;
			}
			IsCustomerPortal_App = ((UserAccount.AppId == 4 || UserAccount.AppId == 6) ? true : false);
			if (IsBookingOffice_App || IsCustomerPortal_App)
			{
				lblReceiverAc.Visible = false;
				((Control)(object)CmbReceiverAccount).Visible = false;
				label12.Visible = false;
				((Control)(object)CmbReceiverAcHistory).Visible = false;
				((Control)(object)CmbSenderAccount).Size = new Size(426, 26);
			}
			DocumentNoFill();
			VoucherTypeFill();
			bankfill();
			StatusFill();
			OtherPartiesFill();
			SenderPartiesBind(Conversion.ToInt(CmbCostCenter.Value));
			txtdocdate.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			GridFill();
			cmbVouchertype_Leave(null, null);
			int DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			DataTable dthistory = AccountsPrematurePaymentsReceipts.GetPendingRecords(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				EntryUser = UserAccount.ID,
				AppId = UserAccount.AppId,
				CostCenterId = Conversion.ToInt(CmbCostCenter.Value)
			});
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CostCenter", typeof(string));
				dt.Columns.Add("TrakingNo", typeof(string));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("BankName", typeof(string));
				dt.Columns.Add("SenderAccount", typeof(string));
				dt.Columns.Add("ReceiverAcId", typeof(int));
				dt.Columns.Add("ReceiverAccount", typeof(string));
				dt.Columns.Add("ChequeNo", typeof(string));
				dt.Columns.Add("ChequeDate", typeof(DateTime));
				dt.Columns.Add("RepresentativeAccount", typeof(string));
				dt.Columns.Add("SlipAmount", typeof(double));
				dt.Columns.Add("EntryStatus", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("VouchersId", typeof(int));
				dt.Columns.Add("CostCenterId", typeof(int));
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["DocNo"], dthistory.Rows[i]["DocDate"], dthistory.Rows[i]["CostCenterName"], dthistory.Rows[i]["TrakingNo"], dthistory.Rows[i]["Amount"], dthistory.Rows[i]["BankName"], dthistory.Rows[i]["SenderAccount"], dthistory.Rows[i]["ChartOfAccountIdReceiver"], dthistory.Rows[i]["RecieverAccount"], dthistory.Rows[i]["ChequeNo"], Conversion.ToDateTime(dthistory.Rows[i]["ChequeDate"]).ToShortDateString(), dthistory.Rows[i]["RepresentativeAc"], dthistory.Rows[i]["SlipAmount"], dthistory.Rows[i]["EntryStatus"], dthistory.Rows[i]["RemarksHeader"], dthistory.Rows[i]["NoOfAttachments"], dthistory.Rows[i]["VouchersId"], dthistory.Rows[i]["CostCenterId"]);
				}
				grdPendingRecord.DataSource = dt;
				grdPendingRecord.RetrieveStructure();
				gridsetting();
			}
			else
			{
				grdPendingRecord.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPendingRecord.AllowEdit = (InheritableBoolean)2;
			if (IsCustomerPortal_App || IsBookingOffice_App)
			{
				grdPendingRecord.RootTable.Columns["CostCenter"].Visible = false;
				grdPendingRecord.RootTable.Columns["ReceiverAccount"].Visible = false;
			}
			else
			{
				grdPendingRecord.RootTable.Columns["CostCenter"].Visible = ((Control)(object)CmbCostCenter).Visible;
			}
			grdPendingRecord.RootTable.Columns["Id"].Visible = false;
			grdPendingRecord.RootTable.Columns["VouchersId"].Visible = false;
			grdPendingRecord.RootTable.Columns["CostCenterId"].Visible = false;
			grdPendingRecord.RootTable.Columns["ReceiverAcId"].Visible = false;
			grdPendingRecord.RootTable.Columns["TrakingNo"].Caption = "Tracking No";
			grdPendingRecord.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdPendingRecord.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdPendingRecord.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdPendingRecord.RootTable.Columns["DocNo"].Width = 60;
			grdPendingRecord.RootTable.Columns["DocDate"].Width = 80;
			grdPendingRecord.RootTable.Columns["CostCenter"].Width = 120;
			grdPendingRecord.RootTable.Columns["TrakingNo"].Width = 80;
			grdPendingRecord.RootTable.Columns["Amount"].Width = 85;
			grdPendingRecord.RootTable.Columns["BankName"].Width = 120;
			grdPendingRecord.RootTable.Columns["SenderAccount"].Width = 140;
			grdPendingRecord.RootTable.Columns["ReceiverAccount"].Width = 140;
			grdPendingRecord.RootTable.Columns["ChequeNo"].Width = 85;
			grdPendingRecord.RootTable.Columns["ChequeDate"].Width = 90;
			grdPendingRecord.RootTable.Columns["RepresentativeAccount"].Width = 160;
			grdPendingRecord.RootTable.Columns["SlipAmount"].Width = 85;
			grdPendingRecord.RootTable.Columns["EntryStatus"].Width = 85;
			grdPendingRecord.RootTable.Columns["Remarks"].Width = 150;
			grdPendingRecord.RootTable.Columns["NoOfAttachments"].Width = 90;
			if (!IsBookingOffice_App && !IsCustomerPortal_App)
			{
				if (formrights.CanChangeOrderStatusToComplete)
				{
					grdPendingRecord.RootTable.Columns.Add("Select");
					grdPendingRecord.RootTable.Columns["Select"].ActAsSelector = true;
					grdPendingRecord.RootTable.Columns["Select"].UseHeaderSelector = true;
					grdPendingRecord.RootTable.Columns["Select"].Width = 40;
					grdPendingRecord.RootTable.Columns["Select"].Position = 0;
					grdPendingRecord.RootTable.Columns.Add("Confirm");
					grdPendingRecord.RootTable.Columns["Confirm"].Caption = "Confirm";
					grdPendingRecord.RootTable.Columns["Confirm"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdPendingRecord.RootTable.Columns["Confirm"].ButtonStyle = (ButtonStyle)5;
					grdPendingRecord.RootTable.Columns["Confirm"].ButtonText = "Confirm";
					grdPendingRecord.RootTable.Columns["Confirm"].Width = 60;
					grdPendingRecord.RootTable.Columns["Confirm"].Position = 1;
				}
				if (formrights.CanChangeOrderStatusToCancel)
				{
					grdPendingRecord.RootTable.Columns.Add("Cancel");
					grdPendingRecord.RootTable.Columns["Cancel"].Caption = "Cancel";
					grdPendingRecord.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdPendingRecord.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
					grdPendingRecord.RootTable.Columns["Cancel"].ButtonText = "Cancel";
					grdPendingRecord.RootTable.Columns["Cancel"].Width = 60;
					grdPendingRecord.RootTable.Columns["Cancel"].Position = (formrights.CanChangeOrderStatusToComplete ? 2 : 0);
				}
			}
			if (formrights.DoHaveUpdateRights)
			{
				grdPendingRecord.RootTable.Columns.Add("Edit");
				grdPendingRecord.RootTable.Columns["Edit"].Caption = "Edit";
				grdPendingRecord.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdPendingRecord.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdPendingRecord.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdPendingRecord.RootTable.Columns["Edit"].Width = 40;
				grdPendingRecord.RootTable.Columns["Edit"].Position = grdPendingRecord.RootTable.Columns["DocNo"].Position - 1;
			}
			grdPendingRecord.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPendingRecord.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPendingRecord.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingRecord.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingRecord.RootTable.Columns["SlipAmount"].TextAlignment = (TextAlignment)3;
			grdPendingRecord.RootTable.Columns["SlipAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("grid record not found");
				return;
			}
			AccountsPrematureReceipts obj = new AccountsPrematureReceipts();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.AccountsPrematureReceiptsList = new List<AccountsPrematureReceipts>();
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			double TotalGridAmount = 0.0;
			double SlipAmount = 0.0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToString(item.Cells["TrakingNo"].Value) != Conversion.ToString(table.Rows[0]["TrakingNo"]))
				{
					throw new Exception("TrackingSlipNo must be Same please check.Row #. " + (item.RowIndex + 1));
				}
				if (Conversion.ToInt(item.Cells["VoucherTypeId"].Value) != Conversion.ToInt(table.Rows[0]["VoucherTypeId"]))
				{
					throw new Exception("VoucherType must be same please check.Row #. " + (item.RowIndex + 1));
				}
				if (Conversion.ToInt(item.Cells["BankId"].Value) != Conversion.ToInt(table.Rows[0]["BankId"]))
				{
					throw new Exception("BankName must be same please check.Row #. " + (item.RowIndex + 1));
				}
				if (Conversion.ToInt(item.Cells["RepresentativeAcId"].Value) != Conversion.ToInt(table.Rows[0]["RepresentativeAcId"]))
				{
					throw new Exception("RepresentativeAc must be same please check.Row #. " + (item.RowIndex + 1));
				}
				if (Conversion.ToInt(item.Cells["ReceiverAcId"].Value) != Conversion.ToInt(table.Rows[0]["ReceiverAcId"]))
				{
					throw new Exception("ReceiverAccount must be same please check.Row #. " + (item.RowIndex + 1));
				}
				AccountsPrematureReceipts detail = new AccountsPrematureReceipts();
				if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
				{
					detail.Id = Conversion.ToInt(item.Cells["Id"].Value);
					detail.TrackingSlipRef = Conversion.ToString(item.Cells["TrakingNo"].Value);
					detail.SlipAmount = Conversion.ToDouble(item.Cells["SlipAmount"].Value);
					detail.DocNo = Conversion.ToInt(item.Cells["DocNo"].Value);
					detail.DocDate = Conversion.ToDateTime(item.Cells["DocDate"].Value);
				}
				else
				{
					detail.TrackingSlipRef = Conversion.ToString((object)txtTrackingNo.Text);
					detail.SlipAmount = Conversion.ToDouble((object)txtSlipAmount.Text);
					detail.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				}
				detail.VouchersId = Conversion.ToInt(item.Cells["VoucherTypeId"].Value);
				detail.BankId = Conversion.ToInt(item.Cells["BankId"].Value);
				detail.SupplierCustomerIdSalesMan = Conversion.ToInt(item.Cells["RepresentativeAcId"].Value);
				detail.ChartOfAccountIdSender = Conversion.ToInt(item.Cells["SenderAcId"].Value);
				detail.ChartOfAccountIdReceiver = Conversion.ToInt(item.Cells["ReceiverAcId"].Value);
				detail.ChequeNo = Conversion.ToString(item.Cells["ChequeNo"].Value);
				detail.ChequeDate = Conversion.ToDateTime(item.Cells["ChequeDate"].Value);
				detail.Amount = Conversion.ToDecimal(item.Cells["Amount"].Value);
				detail.RemarksHeader = Conversion.ToString(item.Cells["Remarks"].Value);
				detail.EntryStatus = Conversion.ToString(item.Cells["Status"].Value);
				TotalGridAmount += Conversion.ToDouble((object)detail.Amount);
				SlipAmount = detail.SlipAmount;
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.EnteryUserId = UserAccount.ID;
				detail.ModifyUserId = UserAccount.ID;
				detail.ApprovalUserId = UserAccount.ID;
				detail.EnteryDate = DateTime.Now;
				detail.ModifyDate = DateTime.Now;
				detail.ApprovedDate = DateTime.Now;
				detail.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				detail.DocumentTypeId = 159;
				detail.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				obj.AccountsPrematureReceiptsList.Add(detail);
			}
			if (TotalGridAmount != SlipAmount)
			{
				throw new Exception("SlipAmount not equal to Grid Total Amount");
			}
			FormHelper.UpdateAttachmentsForObject<AccountsPrematureReceipts>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = ((AttachmentsValues != "") ? AttachmentsValues : obj.AttachmentsValues);
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			AccountsPrematurePaymentsReceipts.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					foreach (AccountsPrematureReceipts accountsPrematureReceipts in obj.AccountsPrematureReceiptsList)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(accountsPrematureReceipts.Id, base.Name, 159, 0);
					}
				}
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 1;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			cmbVouchertype.ReadOnly = false;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtremarks.Clear();
			txtTrackingNo.Clear();
			txtSlipAmount.Clear();
			((Control)(object)CmbReceiverAccount).Text = string.Empty;
			((Control)(object)CmbSenderAccount).Text = string.Empty;
			((Control)(object)cmbSenderbank).Text = string.Empty;
			((Control)(object)cmbRepresentiveAc).Text = string.Empty;
			txtremarks.Clear();
			txtdocdate.Focus();
			txtChequeNo.Text = string.Empty;
			txtAmount.Text = string.Empty;
			table.Rows.Clear();
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			grd.ClearStructure();
			DocumentNoFill();
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			txtremarks.Clear();
			txtChequeNo.Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtChequeNo.Focus();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingRecord).Name);
			if (fs != null)
			{
				grdPendingRecord.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void toolStripButton7_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcfrmDefPdcManagment_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtdocdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtdocdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				toolStripButton8_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (((Control)(object)grd).Focused)
				{
					((Control)(object)grdPendingRecord).Focus();
				}
				else
				{
					((Control)(object)grd).Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton8_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingRecord.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 159);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GeneralReprots.PdcInventorySlip(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = RecId,
				DocumentTypeId = 159
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = null;
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "123-AcRptPdcInventorySlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		new PdcBank(UserAccount).Show();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			bankfill();
			OtherPartiesFill();
			SenderPartiesBind(Conversion.ToInt(CmbCostCenter.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcheqamount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtdocno_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			else
			{
				txtdocdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineOtherParties_Click(object sender, EventArgs e)
	{
		try
		{
			DefineOtherParties defineOtherParties = new DefineOtherParties(UserAccount);
			defineOtherParties.DocumentTypeId = 159;
			defineOtherParties.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbVouchertype_Leave(object sender, EventArgs e)
	{
		try
		{
			ReceiverAccountBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow r = grd.GetRow(0);
				if (Conversion.ToString(r.Cells["TrakingNo"].Value) != txtTrackingNo.Text)
				{
					txtTrackingNo.Text = Conversion.ToString(r.Cells["TrakingNo"].Value);
					throw new Exception("You Can't Add Different Tracking#.");
				}
				if (Conversion.ToString(r.Cells["VoucherType"].Value) != ((Control)(object)cmbVouchertype).Text)
				{
					((Control)(object)cmbVouchertype).Text = Conversion.ToString(r.Cells["VoucherType"].Value);
					throw new Exception("You Can't Add Different Voucher Type.");
				}
				if (Conversion.ToInt(r.Cells["BankId"].Value) != Conversion.ToInt(cmbSenderbank.Value))
				{
					cmbSenderbank.Value = Conversion.ToString(r.Cells["BankId"].Value);
					throw new Exception("You Can't Add Different Sender Bank.");
				}
				if (Conversion.ToInt(r.Cells["RepresentativeAcId"].Value) != Conversion.ToInt(cmbRepresentiveAc.Value))
				{
					cmbRepresentiveAc.Value = Conversion.ToString(r.Cells["RepresentativeAcId"].Value);
					throw new Exception("You Can't Add Different Representative A/c.");
				}
				if (Conversion.ToInt(r.Cells["ReceiverAcId"].Value) != Conversion.ToInt(CmbReceiverAccount.Value))
				{
					CmbReceiverAccount.Value = Conversion.ToString(r.Cells["ReceiverAcId"].Value);
					throw new Exception("You Can't Add Different Receiver A/c.");
				}
			}
			table.Rows.Add(0, txtdocdate.Value, txtdocno.Text, txtTrackingNo.Text, txtSlipAmount.Text, cmbVouchertype.Value, ((Control)(object)cmbVouchertype).Text, cmbSenderbank.Value, ((Control)(object)cmbSenderbank).Text, cmbRepresentiveAc.Value, ((Control)(object)cmbRepresentiveAc).Text, CmbSenderAccount.Value, ((Control)(object)CmbSenderAccount).Text, CmbReceiverAccount.Value, ((Control)(object)CmbReceiverAccount).Text, Conversion.ToString((object)txtChequeNo.Text), datChequeDate.Value, Conversion.ToDecimal((object)txtAmount.Text), txtremarks.Text, ((Control)(object)cmbStatus).Text);
			grd.DataSource = table;
			grd.RetrieveStructure();
			gridDetailsetting();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void gridDetailsetting()
	{
		try
		{
			grd.AllowEdit = (InheritableBoolean)2;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["BankId"].Visible = false;
			grd.RootTable.Columns["VoucherTypeId"].Visible = false;
			grd.RootTable.Columns["RepresentativeAcId"].Visible = false;
			grd.RootTable.Columns["SenderAcId"].Visible = false;
			grd.RootTable.Columns["ReceiverAcId"].Visible = false;
			if (IsBookingOffice_App && IsCustomerPortal_App)
			{
				grd.RootTable.Columns["ReceiverAc"].Visible = false;
			}
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 30;
			grd.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingRecord_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		try
		{
			if (e.Column.Key == "Edit")
			{
				ReportsParameters val = new ReportsParameters
				{
					TrackingNo = Conversion.ToString(grdPendingRecord.CurrentRow.Cells["TrakingNo"].Value),
					SlipAmount = Conversion.ToDouble(grdPendingRecord.CurrentRow.Cells["SlipAmount"].Value),
					VoucherTypeId = Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["VouchersId"].Value),
					ReceiverAcId = Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["ReceiverAcId"].Value),
					DocDate = Conversion.ToDateTime(grdPendingRecord.CurrentRow.Cells["DocDate"].Value),
					CostCenterId = Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["CostCenterId"].Value)
				};
				int CostCenterId = 0;
				DataTable dt = AccountsPrematurePaymentsReceipts.ReadByMultiParams(val);
				if (dt.Rows.Count > 0 && dt != null)
				{
					btnUpdate.Visible = true;
					btnsave.Visible = false;
					table.Rows.Clear();
					foreach (DataRow r in dt.Rows)
					{
						table.Rows.Add(r["Id"], r["DocDate"], r["DocNo"], r["TrakingNo"], r["SlipAmount"], r["VoucherTypeId"], r["VoucherType"], r["BankId"], r["BankName"], r["RepresentativeAcId"], r["RepresentativeAc"], r["SenderAcId"], r["SenderAc"], r["ReceiverAcId"], r["ReceiverAc"], r["ChequeNo"], r["ChequeDate"], r["Amount"], r["Remarks"], r["Status"]);
					}
					CostCenterId = Conversion.ToInt(dt.Rows[0]["CostCenterId"]);
					grd.DataSource = table;
					grd.RetrieveStructure();
					gridDetailsetting();
					AttachmentsValues = Conversion.ToString(dt.Rows[0]["AttachmentsValues"]);
					CustomAttachmentsValues = Conversion.ToString(dt.Rows[0]["CustomAttachmentsValues"]);
					int RecordId = Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["Id"].Value);
					FormHelper.LoadAttachmentsForObject(AT, RecordId, base.Name);
					if (CostCenterId > 0)
					{
						CmbCostCenter.Value = CostCenterId;
						SenderPartiesBind(Conversion.ToInt(CmbCostCenter.Value));
					}
				}
				else
				{
					grd.RetrieveStructure();
				}
			}
			if (e.Column.Key == "Cancel" || e.Column.Key == "Confirm")
			{
				if (e.Column.Key == "Cancel" && !formrights.CanChangeOrderStatusToCancel)
				{
					throw new Exception("You Don't Have Right To Cancel the Voucher");
				}
				if (e.Column.Key == "Confirm" && !formrights.CanChangeOrderStatusToComplete)
				{
					throw new Exception("You Don't Have Right To Confirm the Voucher");
				}
				if (Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["ReceiverAcId"].Value) == 0)
				{
					throw new Exception("Receiver Account not found. Please check!");
				}
				if (MessageBox.Show("Are you sure to " + e.Column.Key, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					AccountsPrematurePaymentsReceipts.UpdateRecord(new AccountsPrematureReceipts
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						EnteryUserId = UserAccount.ID,
						DocumentTypeId = 159,
						FinancialYearId = clsGlobalVariables.ActiveYr.Id,
						TrackingSlipRef = Conversion.ToString(grdPendingRecord.CurrentRow.Cells["TrakingNo"].Value),
						VouchersId = Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["VouchersId"].Value),
						ChartOfAccountIdReceiver = Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["ReceiverAcId"].Value),
						DocDate = Conversion.ToDateTime(grdPendingRecord.CurrentRow.Cells["DocDate"].Value),
						SlipAmount = Conversion.ToDouble(grdPendingRecord.CurrentRow.Cells["SlipAmount"].Value),
						CostCenterId = Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["CostCenterId"].Value),
						EntryStatus = Conversion.ToString((object)e.Column.Key)
					});
					MessageBox.Show("Record " + e.Column.Key + " Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				txtdocdate.Value = Conversion.ToDateTime(item.Cells["DocDate"].Value);
				txtdocno.Text = Conversion.ToString(item.Cells["DocNo"].Value);
				cmbVouchertype.Value = Conversion.ToInt(item.Cells["VoucherTypeId"].Value);
				txtTrackingNo.Text = Conversion.ToString(item.Cells["TrakingNo"].Value);
				txtSlipAmount.Text = Conversion.ToString(item.Cells["SlipAmount"].Value);
				cmbVouchertype_Leave(null, null);
				cmbSenderbank.Value = Conversion.ToInt(item.Cells["BankId"].Value);
				cmbRepresentiveAc.Value = Conversion.ToInt(item.Cells["RepresentativeAcId"].Value);
				CmbSenderAccount.Value = Conversion.ToInt(item.Cells["SenderAcId"].Value);
				if (!IsBookingOffice_App && !IsCustomerPortal_App)
				{
					CmbReceiverAccount.Value = Conversion.ToInt(item.Cells["ReceiverAcId"].Value);
				}
				txtChequeNo.Text = Conversion.ToString(item.Cells["ChequeNo"].Value);
				datChequeDate.Value = Conversion.ToDateTime(item.Cells["ChequeDate"].Value);
				txtAmount.Text = Conversion.ToString(item.Cells["Amount"].Value);
				txtremarks.Text = Conversion.ToString(item.Cells["Remarks"].Value);
				((Control)(object)cmbStatus).Text = Conversion.ToString((object)item.Cells["Status"].Text);
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				cmbVouchertype.Focus();
				TrackingNo = txtTrackingNo.Text;
			}
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
			if (!formvalidation())
			{
				return;
			}
			GridEXRow row = grd.GetRow(0);
			table.Rows[updateDetailIndex]["DocDate"] = txtdocdate.Value;
			table.Rows[updateDetailIndex]["DocNo"] = txtdocno.Text;
			table.Rows[updateDetailIndex]["TrakingNo"] = txtTrackingNo.Text;
			table.Rows[updateDetailIndex]["SlipAmount"] = txtSlipAmount.Text;
			table.Rows[updateDetailIndex]["VoucherTypeId"] = cmbVouchertype.Value;
			table.Rows[updateDetailIndex]["VoucherType"] = ((Control)(object)cmbVouchertype).Text;
			table.Rows[updateDetailIndex]["BankId"] = cmbSenderbank.Value;
			table.Rows[updateDetailIndex]["BankName"] = ((Control)(object)cmbSenderbank).Text;
			table.Rows[updateDetailIndex]["RepresentativeAcId"] = cmbRepresentiveAc.Value;
			table.Rows[updateDetailIndex]["RepresentativeAc"] = ((Control)(object)cmbRepresentiveAc).Text;
			table.Rows[updateDetailIndex]["SenderAcId"] = CmbSenderAccount.Value;
			table.Rows[updateDetailIndex]["SenderAc"] = ((Control)(object)CmbSenderAccount).Text;
			if (!IsBookingOffice_App && !IsCustomerPortal_App)
			{
				table.Rows[updateDetailIndex]["ReceiverAcId"] = CmbReceiverAccount.Value;
				table.Rows[updateDetailIndex]["ReceiverAc"] = ((Control)(object)CmbReceiverAccount).Text;
			}
			table.Rows[updateDetailIndex]["ChequeNo"] = txtChequeNo.Text;
			table.Rows[updateDetailIndex]["ChequeDate"] = datChequeDate.Value;
			table.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
			table.Rows[updateDetailIndex]["Remarks"] = txtremarks.Text;
			table.Rows[updateDetailIndex]["Status"] = ((Control)(object)cmbStatus).Text;
			grd.DataSource = table;
			GridEXRow[] rows;
			if (Conversion.ToString(row.Cells["TrakingNo"].Value) != TrackingNo)
			{
				rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["TrakingNo"].Value = txtTrackingNo.Text;
					obj.EndEdit();
				}
			}
			rows = grd.GetRows();
			foreach (GridEXRow obj2 in rows)
			{
				obj2.BeginEdit();
				obj2.Cells["VoucherTypeId"].Value = cmbVouchertype.Value;
				obj2.Cells["VoucherType"].Value = ((Control)(object)cmbVouchertype).Text;
				obj2.Cells["BankId"].Value = cmbSenderbank.Value;
				obj2.Cells["BankName"].Value = ((Control)(object)cmbSenderbank).Text;
				obj2.Cells["RepresentativeAcId"].Value = cmbRepresentiveAc.Value;
				obj2.Cells["RepresentativeAc"].Value = ((Control)(object)cmbRepresentiveAc).Text;
				obj2.Cells["ReceiverAcId"].Value = CmbReceiverAccount.Value;
				obj2.Cells["ReceiverAc"].Value = ((Control)(object)CmbReceiverAccount).Text;
				obj2.EndEdit();
			}
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetail();
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
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingRecord_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["Id"].Value), 159);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			obj.AppId = UserAccount.AppId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (FromDateHistory.Checked)
			{
				obj.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				obj.ToDate = ToDateHistory.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.VoucherTypeId = Conversion.ToInt(CmbVoucherTypeHistory.Value);
			obj.BankId = Conversion.ToInt(CmbBankNameHistory.Value);
			obj.SenderAcId = Conversion.ToInt(CmbSenderAcHistory.Value);
			obj.ReceiverAcId = Conversion.ToInt(CmbReceiverAcHistory.Value);
			obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
			DataTable dtHistort = AccountsPrematurePaymentsReceipts.FormHistory(obj);
			if (dtHistort.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CostCenter", typeof(string));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("TrakingNo", typeof(string));
				dt.Columns.Add("VoucherType", typeof(string));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("BankName", typeof(string));
				dt.Columns.Add("RepresentativeAc", typeof(string));
				dt.Columns.Add("SenderAccount", typeof(string));
				dt.Columns.Add("ReceiverAccount", typeof(string));
				dt.Columns.Add("ChequeNo", typeof(string));
				dt.Columns.Add("ChequeDate", typeof(DateTime));
				dt.Columns.Add("EntryStatus", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("TrackingWithSlipAmount", typeof(string));
				for (int i = 0; i < dtHistort.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistort.Rows[i]["Id"], dtHistort.Rows[i]["CostCenterName"], dtHistort.Rows[i]["DocNo"], Conversion.ToDateTime(dtHistort.Rows[i]["DocDate"]).ToShortDateString(), dtHistort.Rows[i]["TrakingNo"], dtHistort.Rows[i]["VoucherType"], dtHistort.Rows[i]["Amount"], dtHistort.Rows[i]["BankName"], dtHistort.Rows[i]["RepresentativeAc"], dtHistort.Rows[i]["SenderAccount"], dtHistort.Rows[i]["RecieverAccount"], dtHistort.Rows[i]["ChequeNo"], Conversion.ToDateTime(dtHistort.Rows[i]["ChequeDate"]).ToShortDateString(), dtHistort.Rows[i]["EntryStatus"], dtHistort.Rows[i]["NoOfAttachments"], dtHistort.Rows[i]["RemarksHeader"], Conversion.ToString(dtHistort.Rows[i]["TrakingNo"]) + " " + Conversion.ToString(dtHistort.Rows[i]["SlipAmount"]));
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				gridHistorysetting();
			}
			else
			{
				grdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHistorysetting()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistory.AllowEdit = (InheritableBoolean)2;
			grdHistory.AllowDelete = (InheritableBoolean)2;
			grdHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grdHistory.RootTable.HideColumnsWhenGrouped = (InheritableBoolean)1;
			if (IsCustomerPortal_App)
			{
				grdHistory.RootTable.Columns["CostCenter"].Visible = false;
			}
			grdHistory.RootTable.Columns["CostCenter"].Width = 120;
			grdHistory.RootTable.Columns["DocNo"].Width = 60;
			grdHistory.RootTable.Columns["DocDate"].Width = 80;
			grdHistory.RootTable.Columns["TrakingNo"].Width = 80;
			grdHistory.RootTable.Columns["VoucherType"].Width = 70;
			grdHistory.RootTable.Columns["Amount"].Width = 85;
			grdHistory.RootTable.Columns["BankName"].Width = 120;
			grdHistory.RootTable.Columns["SenderAccount"].Width = 140;
			grdHistory.RootTable.Columns["ReceiverAccount"].Width = 140;
			grdHistory.RootTable.Columns["RepresentativeAc"].Width = 140;
			grdHistory.RootTable.Columns["ChequeNo"].Width = 85;
			grdHistory.RootTable.Columns["ChequeDate"].Width = 90;
			grdHistory.RootTable.Columns["EntryStatus"].Width = 85;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = 100;
			grdHistory.RootTable.Columns["RemarksHeader"].Width = 160;
			grdHistory.RootTable.Columns["TrackingWithSlipAmount"].Width = 120;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value), 159);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnConfirmVoucher_Click(object sender, EventArgs e)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		try
		{
			if (grdPendingRecord.GetCheckedRows().Length == 0)
			{
				throw new Exception("Please select row first");
			}
			if (MessageBox.Show("Are you sure to Confirm Voucher", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (!formrights.CanChangeOrderStatusToComplete)
			{
				throw new Exception("You Don't Have Right To Confirm the Voucher");
			}
			GridEXRow[] checkedRows = grdPendingRecord.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				if (Conversion.ToInt(item.Cells["ReceiverAcId"].Value) == 0)
				{
					throw new Exception("Receiver Account not found. Please check!");
				}
				AccountsPrematurePaymentsReceipts.UpdateRecord(new AccountsPrematureReceipts
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EnteryUserId = UserAccount.ID,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					TrackingSlipRef = Conversion.ToString(item.Cells["TrakingNo"].Value),
					VouchersId = Conversion.ToInt(item.Cells["VouchersId"].Value),
					ChartOfAccountIdReceiver = Conversion.ToInt(item.Cells["ReceiverAcId"].Value),
					DocDate = Conversion.ToDateTime(item.Cells["DocDate"].Value),
					SlipAmount = Conversion.ToDouble(item.Cells["SlipAmount"].Value),
					CostCenterId = Conversion.ToInt(item.Cells["CostCenterId"].Value),
					EntryStatus = "Confirm"
				});
			}
			MessageBox.Show("Records Confirm Successfully");
			Reset();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void AccountsPrematurePaymentsReceipts_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Add_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar1")
			{
				txtdocdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar1")
			{
				txtdocdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Form Grids");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grd.UpdateData();
				if (grd.GetRows().Count() == 0)
				{
					cmbVouchertype.ReadOnly = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingRecord_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdPendingRecord.CurrentRow;
			int CostCenterId = 0;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formrights.DoHaveUpdateRights)
				{
					return;
				}
				table.Rows.Clear();
				DataTable dt = AccountsPrematurePaymentsReceipts.ReadByMultiParams(new ReportsParameters
				{
					TrackingNo = Conversion.ToString(item.Cells["TrakingNo"].Value),
					SlipAmount = Conversion.ToDouble(item.Cells["SlipAmount"].Value),
					VoucherTypeId = Conversion.ToInt(item.Cells["VouchersId"].Value),
					ReceiverAcId = Conversion.ToInt(item.Cells["ReceiverAcId"].Value),
					CostCenterId = Conversion.ToInt(item.Cells["CostCenterId"].Value),
					DocDate = Conversion.ToDateTime(item.Cells["DocDate"].Value)
				});
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow r in dt.Rows)
					{
						table.Rows.Add(r["Id"], r["DocDate"], r["DocNo"], r["TrakingNo"], r["SlipAmount"], r["VoucherTypeId"], r["VoucherType"], r["BankId"], r["BankName"], r["RepresentativeAcId"], r["RepresentativeAc"], r["SenderAcId"], r["SenderAc"], r["ReceiverAcId"], r["ReceiverAc"], r["ChequeNo"], r["ChequeDate"], r["Amount"], r["Remarks"], r["Status"]);
					}
					CostCenterId = Conversion.ToInt(dt.Rows[0]["CostCenterId"]);
					if (CostCenterId > 0)
					{
						CmbCostCenter.Value = CostCenterId;
						SenderPartiesBind(Conversion.ToInt(CmbCostCenter.Value));
					}
					grd.DataSource = table;
					grd.RetrieveStructure();
					gridDetailsetting();
					btnUpdate.Visible = true;
					btnsave.Visible = false;
					AttachmentsValues = Conversion.ToString(dt.Rows[0]["AttachmentsValues"]);
					CustomAttachmentsValues = Conversion.ToString(dt.Rows[0]["CustomAttachmentsValues"]);
					List<global::Architecture.Model.DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["Id"].Value), base.Name);
					AT = new Attachment();
					{
						foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
						{
							TempAttachments TA = new TempAttachments();
							TA.Id = 0;
							TA.Path = Attachment.Attachment;
							TA.CustomFileName = Attachment.UploadedFileCustomName;
							TA.ActionId = 2;
							AT.lst.Add(TA);
						}
						return;
					}
				}
				grd.RetrieveStructure();
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdPendingRecord.CurrentColumn.Key == "Edit")
				{
					if (!formrights.DoHaveUpdateRights)
					{
						return;
					}
					ReportsParameters val = new ReportsParameters
					{
						TrackingNo = Conversion.ToString(item.Cells["TrakingNo"].Value),
						SlipAmount = Conversion.ToDouble(item.Cells["SlipAmount"].Value),
						VoucherTypeId = Conversion.ToInt(item.Cells["VouchersId"].Value),
						ReceiverAcId = Conversion.ToInt(item.Cells["ReceiverAcId"].Value),
						CostCenterId = Conversion.ToInt(item.Cells["CostCenterId"].Value),
						DocDate = Conversion.ToDateTime(item.Cells["DocDate"].Value)
					};
					table.Rows.Clear();
					DataTable dt2 = AccountsPrematurePaymentsReceipts.ReadByMultiParams(val);
					if (dt2.Rows.Count > 0)
					{
						foreach (DataRow r2 in dt2.Rows)
						{
							table.Rows.Add(r2["Id"], r2["DocDate"], r2["DocNo"], r2["TrakingNo"], r2["SlipAmount"], r2["VoucherTypeId"], r2["VoucherType"], r2["BankId"], r2["BankName"], r2["RepresentativeAcId"], r2["RepresentativeAc"], r2["SenderAcId"], r2["SenderAc"], r2["ReceiverAcId"], r2["ReceiverAc"], r2["ChequeNo"], r2["ChequeDate"], r2["Amount"], r2["Remarks"], r2["Status"]);
						}
						btnUpdate.Visible = true;
						btnsave.Visible = false;
						grd.DataSource = table;
						grd.RetrieveStructure();
						gridDetailsetting();
						CostCenterId = Conversion.ToInt(dt2.Rows[0]["CostCenterId"]);
						if (CostCenterId > 0)
						{
							CmbCostCenter.Value = CostCenterId;
							SenderPartiesBind(Conversion.ToInt(CmbCostCenter.Value));
						}
						AttachmentsValues = Conversion.ToString(dt2.Rows[0]["AttachmentsValues"]);
						CustomAttachmentsValues = Conversion.ToString(dt2.Rows[0]["CustomAttachmentsValues"]);
						List<DMSAttachments> byID2 = global::Architecture.BLL.DMSAttachments.GetByID(Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["Id"].Value), base.Name);
						AT = new Attachment();
						{
							foreach (DMSAttachments Attachment2 in byID2)
							{
								TempAttachments TA2 = new TempAttachments();
								TA2.Id = 0;
								TA2.Path = Attachment2.Attachment;
								TA2.CustomFileName = Attachment2.UploadedFileCustomName;
								TA2.ActionId = 2;
								AT.lst.Add(TA2);
							}
							return;
						}
					}
					grd.RetrieveStructure();
				}
				else if (grdPendingRecord.CurrentColumn.Key == "Cancel" || grdPendingRecord.CurrentColumn.Key == "Confirm")
				{
					if (grdPendingRecord.CurrentColumn.Key == "Cancel" && !formrights.CanChangeOrderStatusToCancel)
					{
						throw new Exception("You Don't Have Right To Cancel the Voucher");
					}
					if (grdPendingRecord.CurrentColumn.Key == "Confirm" && !formrights.CanChangeOrderStatusToComplete)
					{
						throw new Exception("You Don't Have Right To Confirm the Voucher");
					}
					if (Conversion.ToInt(grdPendingRecord.CurrentRow.Cells["ReceiverAcId"].Value) == 0)
					{
						throw new Exception("Receiver Account not found. Please check!");
					}
					if (MessageBox.Show("Are you sure to " + grdPendingRecord.CurrentColumn.Key, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
					{
						AccountsPrematurePaymentsReceipts.UpdateRecord(new AccountsPrematureReceipts
						{
							OrganizationId = UserAccount.OrganizationId,
							CompanyId = UserAccount.CompanyId,
							EnteryUserId = UserAccount.ID,
							FinancialYearId = clsGlobalVariables.ActiveYr.Id,
							TrackingSlipRef = Conversion.ToString(item.Cells["TrakingNo"].Value),
							VouchersId = Conversion.ToInt(item.Cells["VouchersId"].Value),
							ChartOfAccountIdReceiver = Conversion.ToInt(item.Cells["ReceiverAcId"].Value),
							DocDate = Conversion.ToDateTime(item.Cells["DocDate"].Value),
							SlipAmount = Conversion.ToDouble(item.Cells["SlipAmount"].Value),
							EntryStatus = Conversion.ToString((object)grdPendingRecord.CurrentColumn.Key),
							CostCenterId = Conversion.ToInt(item.Cells["CostCenterId"].Value)
						});
						MessageBox.Show("Record" + grdPendingRecord.CurrentColumn.Key + "Successfully");
						Reset();
					}
				}
				else if (grdPendingRecord.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 159);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grd.UpdateData();
				if (grd.GetRows().Count() == 0)
				{
					cmbVouchertype.ReadOnly = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSlipAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSlipAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTypingOnVouchers(sender, e);
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

	private void txtAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTypingOnVouchers(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		try
		{
			DataTable dtVoucherType = new DataTable();
			dtVoucherType.Columns.Add("Id", typeof(int));
			dtVoucherType.Columns.Add("Name", typeof(string));
			DataTable dtBank = new DataTable();
			dtBank.Columns.Add("Id", typeof(int));
			dtBank.Columns.Add("Name", typeof(string));
			DataTable dtSender = new DataTable();
			dtSender.Columns.Add("Id", typeof(int));
			dtSender.Columns.Add("Name", typeof(string));
			DataTable dtReceiver = new DataTable();
			dtReceiver.Columns.Add("Id", typeof(int));
			dtReceiver.Columns.Add("Name", typeof(string));
			DataTable dt = AccountsPrematurePaymentsReceipts.GetDataForDropDown(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				AppId = UserAccount.AppId,
				CostCenterId = Conversion.ToInt(CmbCostCenter.Value)
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "VoucherType")
					{
						dtVoucherType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "SenderBank")
					{
						dtBank.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "SenderAc")
					{
						dtSender.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ReceiverAc")
					{
						dtReceiver.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtVoucherType, CmbVoucherTypeHistory, "Id", "Name", "Voucher Type", ZeroIndex: false);
			((UltraGridBase)CmbVoucherTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtBank, CmbBankNameHistory, "Id", "Name", "Bank Name", ZeroIndex: false);
			((UltraGridBase)CmbBankNameHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtSender, CmbSenderAcHistory, "Id", "Name", "Sender Ac", ZeroIndex: false);
			DDL.BindDDL(dtReceiver, CmbReceiverAcHistory, "Id", "Name", "Receiver Ac", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
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
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbVoucherTypeHistory).Text = string.Empty;
			((Control)(object)CmbBankNameHistory).Text = string.Empty;
			((Control)(object)CmbSenderAcHistory).Text = string.Empty;
			((Control)(object)CmbReceiverAcHistory).Text = string.Empty;
			grdHistory.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingRecord_FormattingRow(object sender, RowLoadEventArgs e)
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Expected O, but got Unknown
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Expected O, but got Unknown
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Expected O, but got Unknown
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Expected O, but got Unknown
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Expected O, but got Unknown
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Expected O, but got Unknown
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Expected O, but got Unknown
		//IL_0c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca2: Expected O, but got Unknown
		//IL_0e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e27: Expected O, but got Unknown
		//IL_477e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4788: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grdPendingRecord_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
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
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.AccountsPrematurePaymentsReceipts));
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdPendingRecord = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.Add = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.btnDefineOtherParties = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.datChequeDate = new System.Windows.Forms.DateTimePicker();
		this.label15 = new System.Windows.Forms.Label();
		this.txtChequeNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtSlipAmount = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbVouchertype = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbSenderAccount = new UltraCombo();
		this.CmbReceiverAccount = new UltraCombo();
		this.cmbStatus = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbRepresentiveAc = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.txtTrackingNo = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.lblReceiverAc = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbSenderbank = new UltraCombo();
		this.button1 = new System.Windows.Forms.Button();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.lblCostCenter = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
		this.btnConfirmVoucher = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbReceiverAcHistory = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.CmbSenderAcHistory = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbBankNameHistory = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbVoucherTypeHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.printDialog1 = new System.Windows.Forms.PrintDialog();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingRecord).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbVouchertype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSenderAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRepresentiveAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSenderbank).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverAcHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSenderAcHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBankNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVoucherTypeHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1297, 622);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1289, 596);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1283, 590);
		this.panel1.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingRecord);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 345);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1283, 245);
		this.panel7.TabIndex = 7;
		this.grdPendingRecord.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingRecord).BackColor = System.Drawing.Color.White;
		grdPendingRecord_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingRecord.DesignTimeLayout = grdPendingRecord_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingRecord.FilterMode = (FilterMode)2;
		this.grdPendingRecord.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingRecord.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Name = "grdPendingRecord";
		((ScrollableBase)this.grdPendingRecord).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Size = new System.Drawing.Size(1283, 245);
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).TabStop = false;
		this.grdPendingRecord.TotalRow = (InheritableBoolean)1;
		this.grdPendingRecord.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingRecord).VisualStyle = (VisualStyle)4;
		this.grdPendingRecord.FormattingRow += new RowLoadEventHandler(grdPendingRecord_FormattingRow);
		this.grdPendingRecord.ColumnButtonClick += new ColumnActionEventHandler(grdPendingRecord_ColumnButtonClick);
		this.grdPendingRecord.LinkClicked += new ColumnActionEventHandler(grdPendingRecord_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingRecord_KeyDown);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 169);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1283, 176);
		this.panel3.TabIndex = 6;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1283, 176);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.btnCancelUpdateDetial);
		this.panel5.Controls.Add(this.btnUpdateDetail);
		this.panel5.Controls.Add(this.Add);
		this.panel5.Controls.Add(this.label20);
		this.panel5.Controls.Add(this.txtAmount);
		this.panel5.Controls.Add(this.btnDefineOtherParties);
		this.panel5.Controls.Add(this.label18);
		this.panel5.Controls.Add(this.datChequeDate);
		this.panel5.Controls.Add(this.label15);
		this.panel5.Controls.Add(this.txtChequeNo);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add(this.txtSlipAmount);
		this.panel5.Controls.Add(this.label40);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbVouchertype);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSenderAccount);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReceiverAccount);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.panel5.Controls.Add(this.label19);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRepresentiveAc);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.txtdocdate);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.txtTrackingNo);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.txtremarks);
		this.panel5.Controls.Add(this.lblReceiverAc);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSenderbank);
		this.panel5.Controls.Add(this.button1);
		this.panel5.Controls.Add(this.txtdocno);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 54);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1283, 115);
		this.panel5.TabIndex = 2;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1228, 84);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(50, 22);
		this.btnCancelUpdateDetial.TabIndex = 16;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnCancelUpdateDetial.Leave += new System.EventHandler(btnCancelUpdateDetial_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1172, 84);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(54, 22);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(1145, 83);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(26, 24);
		this.Add.TabIndex = 14;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.Add.Leave += new System.EventHandler(Add_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(673, 61);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(51, 15);
		this.label20.TabIndex = 137;
		this.label20.Text = "Amount";
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(676, 84);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(95, 23);
		this.txtAmount.TabIndex = 11;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.TextChanged += new System.EventHandler(txtAmount_TextChanged);
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.btnDefineOtherParties.BackColor = System.Drawing.Color.White;
		this.btnDefineOtherParties.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineOtherParties.Location = new System.Drawing.Point(1145, 27);
		this.btnDefineOtherParties.Name = "btnDefineOtherParties";
		this.btnDefineOtherParties.Size = new System.Drawing.Size(23, 24);
		this.btnDefineOtherParties.TabIndex = 135;
		this.btnDefineOtherParties.TabStop = false;
		this.btnDefineOtherParties.UseVisualStyleBackColor = false;
		this.btnDefineOtherParties.Click += new System.EventHandler(btnDefineOtherParties_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(558, 61);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(75, 15);
		this.label18.TabIndex = 134;
		this.label18.Text = "Cheque Date";
		this.datChequeDate.CustomFormat = "dd-MMM-yy";
		this.datChequeDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datChequeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datChequeDate.Location = new System.Drawing.Point(558, 84);
		this.datChequeDate.Name = "datChequeDate";
		this.datChequeDate.Size = new System.Drawing.Size(117, 23);
		this.datChequeDate.TabIndex = 10;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(439, 61);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(66, 15);
		this.label15.TabIndex = 132;
		this.label15.Text = "Cheque No";
		this.txtChequeNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChequeNo.Location = new System.Drawing.Point(437, 84);
		this.txtChequeNo.Name = "txtChequeNo";
		this.txtChequeNo.Size = new System.Drawing.Size(119, 23);
		this.txtChequeNo.TabIndex = 9;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(324, 6);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(74, 15);
		this.label8.TabIndex = 130;
		this.label8.Text = "Slip Amount";
		this.txtSlipAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSlipAmount.Location = new System.Drawing.Point(322, 27);
		this.txtSlipAmount.Name = "txtSlipAmount";
		this.txtSlipAmount.Size = new System.Drawing.Size(114, 23);
		this.txtSlipAmount.TabIndex = 3;
		this.txtSlipAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSlipAmount.TextChanged += new System.EventHandler(txtSlipAmount_TextChanged);
		this.txtSlipAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSlipAmount_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(104, 6);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(57, 15);
		this.label40.TabIndex = 12;
		this.label40.Text = "Doc Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(438, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(79, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "Voucher Type";
		this.cmbVouchertype.AllowNull = (DefaultableBoolean)1;
		this.cmbVouchertype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbVouchertype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbVouchertype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbVouchertype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.cmbVouchertype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbVouchertype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbVouchertype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbVouchertype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbVouchertype).Location = new System.Drawing.Point(437, 25);
		((System.Windows.Forms.Control)(object)this.cmbVouchertype).Name = "cmbVouchertype";
		((System.Windows.Forms.Control)(object)this.cmbVouchertype).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.cmbVouchertype).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbVouchertype).Leave += new System.EventHandler(cmbVouchertype_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(1057, 61);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(40, 15);
		this.label1.TabIndex = 128;
		this.label1.Text = "Status";
		this.CmbSenderAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbSenderAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSenderAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSenderAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSenderAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbSenderAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSenderAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSenderAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSenderAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSenderAccount).Location = new System.Drawing.Point(10, 82);
		((System.Windows.Forms.Control)(object)this.CmbSenderAccount).Name = "CmbSenderAccount";
		((System.Windows.Forms.Control)(object)this.CmbSenderAccount).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbSenderAccount).TabIndex = 7;
		this.CmbReceiverAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbReceiverAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReceiverAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReceiverAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReceiverAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReceiverAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReceiverAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReceiverAccount).Location = new System.Drawing.Point(220, 82);
		((System.Windows.Forms.Control)(object)this.CmbReceiverAccount).Name = "CmbReceiverAccount";
		((System.Windows.Forms.Control)(object)this.CmbReceiverAccount).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.CmbReceiverAccount).TabIndex = 8;
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(1057, 82);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 13;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(560, 6);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(73, 15);
		this.label19.TabIndex = 25;
		this.label19.Text = "Sender Bank";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 12;
		this.label2.Text = "Doc No";
		this.cmbRepresentiveAc.AllowNull = (DefaultableBoolean)1;
		this.cmbRepresentiveAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRepresentiveAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRepresentiveAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRepresentiveAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRepresentiveAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRepresentiveAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRepresentiveAc).Location = new System.Drawing.Point(796, 25);
		((System.Windows.Forms.Control)(object)this.cmbRepresentiveAc).Name = "cmbRepresentiveAc";
		((System.Windows.Forms.Control)(object)this.cmbRepresentiveAc).Size = new System.Drawing.Size(348, 26);
		((System.Windows.Forms.Control)(object)this.cmbRepresentiveAc).TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(223, 6);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(85, 15);
		this.label4.TabIndex = 12;
		this.label4.Text = "Tracking Slip #";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(103, 27);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(116, 23);
		this.txtdocdate.TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(771, 61);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(52, 15);
		this.label10.TabIndex = 12;
		this.label10.Text = "Remarks";
		this.txtTrackingNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTrackingNo.Location = new System.Drawing.Point(220, 27);
		this.txtTrackingNo.Name = "txtTrackingNo";
		this.txtTrackingNo.Size = new System.Drawing.Size(101, 23);
		this.txtTrackingNo.TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(795, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(106, 15);
		this.label9.TabIndex = 12;
		this.label9.Text = "Representative A/c";
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(772, 84);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(284, 23);
		this.txtremarks.TabIndex = 12;
		this.lblReceiverAc.AutoSize = true;
		this.lblReceiverAc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblReceiverAc.Location = new System.Drawing.Point(223, 61);
		this.lblReceiverAc.Name = "lblReceiverAc";
		this.lblReceiverAc.Size = new System.Drawing.Size(99, 15);
		this.lblReceiverAc.TabIndex = 12;
		this.lblReceiverAc.Text = "Receiver Account";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(10, 61);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(92, 15);
		this.label13.TabIndex = 12;
		this.label13.Text = "Sender Account";
		this.cmbSenderbank.AllowNull = (DefaultableBoolean)1;
		this.cmbSenderbank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSenderbank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSenderbank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSenderbank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.cmbSenderbank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSenderbank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSenderbank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSenderbank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSenderbank).Location = new System.Drawing.Point(556, 25);
		((System.Windows.Forms.Control)(object)this.cmbSenderbank).Name = "cmbSenderbank";
		((System.Windows.Forms.Control)(object)this.cmbSenderbank).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.cmbSenderbank).TabIndex = 5;
		this.button1.BackColor = System.Drawing.Color.White;
		this.button1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.button1.Location = new System.Drawing.Point(772, 26);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(23, 24);
		this.button1.TabIndex = 26;
		this.button1.TabStop = false;
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(12, 27);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(89, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdocno_KeyPress);
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.panel4.Controls.Add(this.lblCostCenter);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.lblCaption);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1283, 26);
		this.panel4.TabIndex = 5;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(362, 0);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 30;
		this.lblCostCenter.AutoSize = true;
		this.lblCostCenter.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCostCenter.ForeColor = System.Drawing.Color.White;
		this.lblCostCenter.Location = new System.Drawing.Point(285, 6);
		this.lblCostCenter.Name = "lblCostCenter";
		this.lblCostCenter.Size = new System.Drawing.Size(72, 15);
		this.lblCostCenter.TabIndex = 31;
		this.lblCostCenter.Text = "Cost Center";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1244, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdPendingRecord;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(8, 3);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(249, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Accounts Premature Receipts";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1283, 28);
		this.panel2.TabIndex = 3;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.toolStripButton1, this.btnsave, this.btnUpdate, this.toolStripButton8, this.btnConfirmVoucher, this.btnShortCutKey });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1283, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "New";
		this.btnNew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripButton1.AccessibleName = "btnRefresh";
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripButton8.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton8.Image = (System.Drawing.Image)resources.GetObject("toolStripButton8.Image");
		this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton8.Name = "toolStripButton8";
		this.toolStripButton8.Size = new System.Drawing.Size(109, 24);
		this.toolStripButton8.Text = "&Attachment";
		this.toolStripButton8.Click += new System.EventHandler(toolStripButton8_Click);
		this.btnConfirmVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnConfirmVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnConfirmVoucher.Name = "btnConfirmVoucher";
		this.btnConfirmVoucher.Size = new System.Drawing.Size(145, 24);
		this.btnConfirmVoucher.Text = "Confirm Vouchers";
		this.btnConfirmVoucher.Click += new System.EventHandler(btnConfirmVoucher_Click);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1289, 596);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Register";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1283, 590);
		this.panel6.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1283, 560);
		this.panel9.TabIndex = 4;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 136);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1283, 424);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.grdHistory).TabStop = false;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label21);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 108);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1283, 28);
		this.panel12.TabIndex = 51;
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.Teal;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(3, 4);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(142, 19);
		this.label21.TabIndex = 22;
		this.label21.Text = "Filtered Records";
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 28);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1283, 80);
		this.panel11.TabIndex = 50;
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSenderAcHistory);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankNameHistory);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVoucherTypeHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(969, 74);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(612, 47);
		this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(68, 15);
		this.label12.TabIndex = 148;
		this.label12.Text = "Receiver Ac";
		this.CmbReceiverAcHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbReceiverAcHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReceiverAcHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReceiverAcHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReceiverAcHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Location = new System.Drawing.Point(681, 43);
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Name = "CmbReceiverAcHistory";
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).TabIndex = 7;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(612, 20);
		this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(61, 15);
		this.label17.TabIndex = 146;
		this.label17.Text = "Sender Ac";
		this.CmbSenderAcHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbSenderAcHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSenderAcHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSenderAcHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSenderAcHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSenderAcHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSenderAcHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSenderAcHistory).Location = new System.Drawing.Point(681, 16);
		((System.Windows.Forms.Control)(object)this.CmbSenderAcHistory).Name = "CmbSenderAcHistory";
		((System.Windows.Forms.Control)(object)this.CmbSenderAcHistory).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbSenderAcHistory).TabIndex = 6;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(393, 46);
		this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(68, 15);
		this.label11.TabIndex = 144;
		this.label11.Text = "Bank Name";
		this.CmbBankNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBankNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankNameHistory).Location = new System.Drawing.Point(477, 42);
		((System.Windows.Forms.Control)(object)this.CmbBankNameHistory).Name = "CmbBankNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbBankNameHistory).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankNameHistory).TabIndex = 5;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(911, 43);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(393, 19);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 15);
		this.label6.TabIndex = 142;
		this.label6.Text = "Voucher Type";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(6, 43);
		this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(107, 15);
		this.label16.TabIndex = 140;
		this.label16.Text = "From && To Doc No";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(6, 23);
		this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(94, 15);
		this.label14.TabIndex = 13;
		this.label14.Text = "From && To Date ";
		this.CmbVoucherTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbVoucherTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVoucherTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVoucherTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVoucherTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVoucherTypeHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVoucherTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVoucherTypeHistory).Location = new System.Drawing.Point(477, 15);
		((System.Windows.Forms.Control)(object)this.CmbVoucherTypeHistory).Name = "CmbVoucherTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbVoucherTypeHistory).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbVoucherTypeHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(253, 40);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(134, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(117, 40);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(134, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(253, 15);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(117, 15);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(135, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar2);
		this.panel10.Controls.Add(this.label5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1283, 28);
		this.panel10.TabIndex = 49;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1242, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 25);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(234, 19);
		this.label5.TabIndex = 22;
		this.label5.Text = "Premature Receipts History";
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1283, 30);
		this.panel8.TabIndex = 3;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1283, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.printDialog1.UseEXDialog = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1297, 622);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "AccountsPrematurePaymentsReceipts";
		this.Text = "Premature Receipts";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AccountsPrematurePaymentsReceipts_FormClosing);
		base.Load += new System.EventHandler(AcfrmDefPdcManagment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmDefPdcManagment_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingRecord).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbVouchertype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSenderAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRepresentiveAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSenderbank).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverAcHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSenderAcHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBankNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVoucherTypeHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}