// using Architecture.Reports;
using System;
using System.Collections;
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
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Reports;
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
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using AccountsReconciliationHeader = global::Architecture.Model.Accounts.AccountsReconciliationHeader;
using SupCustBankDetail = global::Architecture.Model.Inventory.SupCustBankDetail;
using Reporting = global::Architecture.Common.Reporting;
using MultiCurrencyBLL = global::Architecture.BLL.MultiCurrency;
using ProjectsBLL = global::Architecture.BLL.Projects;
using DMSAttachmentsBLL = global::Architecture.BLL.DMSAttachments;
using VoucherHead = global::Architecture.Model.Accounts.VoucherHead;
using MultiCurrency = global::Architecture.Model.MultiCurrency;
using DMSAttachments = global::Architecture.Model.DMSAttachments;
using ImInvoiceBLL = global::Architecture.BLL.Import.ImInvoice;
using ExImFCBankPaymenstsBLL = global::Architecture.BLL.Import.ExImFCBankPaymensts;
using FcYBankChargesTypes = Architecture.WinApp.Common.FcYBankChargesTypes;
using SpecialSreenRightsUserWise = Architecture.WinApp.Common.SpecialSreenRightsUserWise;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Definition.VouchersWithTax
{

public class JournalVoucher_New : Form
{
	private bool shiftTabPressed;

	private bool BranchFeature;

	private bool AfterCloseupSubsidiary;

	private bool MultiCurrencyFeatureVisibilty;

	private bool SubsidiaryAccountAllowOnVouchers;

	private bool UpdateMode;

	private bool IsBookingOffice;

	private bool Approved;

	private int BaseCurrency;

	private double BaseRate;

	private int SubsidiaryAccountDatasourceLength;

	private int updateRowIndex = -1;

	public int RecId;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	public bool _AutoAdjust;

	private DataTable table = new DataTable();

	private DataTable dtDrAc = new DataTable();

	private DataTable dtSubsidory = new DataTable();

	private TrialBalance LoadTrialBalance;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private bool _isChanging;

	private bool _isChangingFcy;

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

	private ToolStripButton btnAttachment;

	private Label label23;

	private Panel panel5;

	private Panel panel4;

	private BackgroundWorker backgroundWorker1;

	private TextBox txtdocno;

	private DateTimePicker DocDate;

	private Label label13;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip3;

	private CtrlGrdBar ctrlGrdBar2;

	private Label lbDr;

	internal UltraCombo CmbAccount;

	private Label label6;

	private TextBox txtDrRmks;

	private Label label9;

	private Label label8;

	private Label lblComments;

	private TextBox txtAmount;

	private TextBox txtCheq;

	private TextBox txtDebitBalance;

	internal GridEX grd;

	internal GridEX DataGridHistory;

	private ToolStripButton Print;

	private ToolStripButton btnRefresh;

	internal GridEX grdDetail;

	private CheckBox ChkBoxPrintPreview;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private Label label1;

	private Label label2;

	private Label label3;

	private Panel panel12;

	private Label label4;

	internal UltraCombo CmbSubsidiaryAccount;

	private Label lblSubsidiaryaccount;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo CmbDrJobLot;

	private Label label7;

	private ToolStripButton btnShortCutKey;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

	private TextBox txtExchangeRate;

	private Label lblExchangeRate;

	private Label lblFcyAmount;

	private TextBox txtFcyAmount;

	internal UltraCombo CmbProjectId;

	private Label label10;

	private Panel panel6;

	private Panel panel7;

	private GroupBox groupBox3;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	internal UltraCombo cmbApproveStatus;

	private Label label15;

	private Label label14;

	private Label label18;

	private Label label17;

	private Label label16;

	private Button btnshow;

	private Panel panel8;

	private Panel panel9;

	private Panel panel10;

	private Panel panel11;

	private ToolStripButton btnNewHistory;

	private GroupBox VoucherInfoBox;

	private Label label36;

	private Label lblUnApprovedVouchers;

	private Label lblTotalVouchers;

	private Label label38;

	private Label lblApprovedVouchers;

	private Label label37;

	private RadioButton rdentapproved;

	private RadioButton rdentmodify;

	private RadioButton rdentdoc;

	private RadioButton rdententry;

	private TextBox txtVoucherremarks;

	internal UltraCombo CmbDrBranch;

	private Label label12;

	private Label labelSubsidiaryDebitBal;

	private Label labelDrSubsidiary;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel DebitAccountPanel;

	private Panel DebitSubsidiaryPanel;

	private Panel DrCommentsPanel;

	private Panel DrJobLotPanel;

	private Panel DrBranchPanel;

	private Panel DrCostCenterPanel;

	internal UltraCombo CmbDrCostCenter;

	private Label label20;

	private Panel panelDetailButtons;

	private Button btnCancelDetail;

	private Button btnplus;

	private Button btnUpdateDetail;

	private Panel panel13;

	private Panel panel14;

	private Panel panel15;

	private TextBox txtCredit;

	private Label label5;

	private Label label11;

	private TextBox txtDifference;

	private ToolStripButton btnDelete;

	private Label labelLocationType;

	internal UltraCombo cmbLocationType;

	private CheckBox chkCustomAccounts;

	private Panel FcyDebitPanel;

	private TextBox txtFcyDebit;

	private Label label19;

	private Panel FcyCreditPanel;

	private TextBox txtFcyCredit;

	private Label label21;

	private Label lblFcyAmountCr;

	private TextBox txtFcyAmountCr;

	private ToolStripButton btnLoadTrialBalance;

	private ToolStripButton btnGenerateRemarks;

	private Button btnUpdateJoblot;

	public JournalVoucher_New(UserAccount user)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbAccount).ActiveRow == null || Conversion.ToInt(CmbAccount.Value) == 0)
		{
			MessageBox.Show("Account Field is Required");
			CmbAccount.Focus();
			return false;
		}
		if (SubsidiaryAccountAllowOnVouchers && SubsidiaryAccountDatasourceLength > 0 && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow == null)
		{
			MessageBox.Show("Subsidiary Account Field Required");
			CmbSubsidiaryAccount.Focus();
			return false;
		}
		if (txtDrRmks.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Remarks Field is Required");
			txtDrRmks.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDrJobLot).ActiveRow == null || Conversion.ToInt(CmbDrJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot Field is Required");
			CmbDrJobLot.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmount.Text.Trim()) == 0.0 && Conversion.ToDouble((object)txtCredit.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Debit and Credit cannot be equal to zero....!");
			txtAmount.Focus();
			return false;
		}
		if (BranchFeature && (((UltraGridBase)CmbDrBranch).ActiveRow == null || Conversion.ToInt(CmbDrBranch.Value) == 0))
		{
			MessageBox.Show("Branch Field is Required");
			CmbDrBranch.Focus();
			return false;
		}
		return true;
	}

	private void VoucherCode()
	{
		try
		{
			txtdocno.Text = Conversion.ToString((object)CommonServices.GenerateVoucherCode(17));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			DataTable dtBranch = global::Architecture.BLL.BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbDrBranch, "BranchId", "BranchName", "Dr Branch", ZeroIndex: false);
				CmbDrBranch.Value = UserAccount.BranchesId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = MultiCurrencyBLL.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", false);
				((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LocationType()
	{
		try
		{
			DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GetLocationType();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbLocationType, "Id", "Location", "Location Type", ZeroIndex: true);
				((UltraGridBase)cmbLocationType).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DebitAccountTitleFill()
	{
		try
		{
			int DrAccountId = Conversion.ToInt(CmbAccount.Value);
			DataTable CoaList = CommonServices.Accounts_GetAccountTitleByAccountTypeIds();
			if (CoaList.Rows.Count > 0)
			{
				if (dtDrAc.Columns.Count == 0)
				{
					dtDrAc.Columns.Add("Id", typeof(int));
					dtDrAc.Columns.Add("AccountTitle");
					dtDrAc.Columns.Add("AccountCode");
					dtDrAc.Columns.Add("ParentAccount");
					dtDrAc.Columns.Add("AccountClass");
				}
				dtDrAc.Rows.Clear();
				for (int i = 0; i < CoaList.Rows.Count; i++)
				{
					dtDrAc.Rows.Add(CoaList.Rows[i]["Id"], CoaList.Rows[i]["AccountTitle"], CoaList.Rows[i]["AccountCode"], CoaList.Rows[i]["ParentAccountTitle"], CoaList.Rows[i]["AccountClass"]);
				}
				if (dtDrAc.Rows.Count > 0)
				{
					DDL.BindDDL(dtDrAc, CmbAccount, "Id", "AccountTitle", "Debit Account", ZeroIndex: true);
				}
			}
			if (DrAccountId > 0)
			{
				if (dtDrAc.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DrAccountId))
				{
					CmbAccount.Value = DrAccountId;
					CmbDr_Leave(null, null);
				}
				else
				{
					((Control)(object)CmbAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDr_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		try
		{
			DataTable detail = global::Architecture.BLL.Accounts.VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DocDate.Value,
				RefAccountId = Conversion.ToInt(CmbAccount.Value)
			});
			if (detail.Rows.Count > 0)
			{
				txtDebitBalance.Text = Math.Round(Conversion.ToDouble(detail.Rows[0]["Balance"])).ToString(clsGlobalVariables.stringFormatboth);
				double num = Conversion.ToDouble((object)txtDebitBalance.Text);
				if (num > 0.0)
				{
					lbDr.Text = "Dr";
				}
				else
				{
					lbDr.Text = "Cr";
				}
				if (num == 0.0)
				{
					lbDr.Text = "Nill";
				}
				txtDebitBalance.Visible = true;
				lbDr.Visible = true;
			}
			else
			{
				txtDebitBalance.Text = "0";
			}
			if (((UltraGridBase)CmbAccount).ActiveRow != null)
			{
				BindDebitSubsidiaryAccount();
			}
			else
			{
				BindAllSubsidiaryaccounts();
			}
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

	private void CombProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbProjectId, "Id", "ProjectName", "Cost Center", false);
				((UltraGridBase)CmbProjectId).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CostCenterFill()
	{
		try
		{
			int DrId = Conversion.ToInt(CmbDrCostCenter.Value);
			DataTable dt = global::Architecture.BLL.Projects.GetSubCostCenters(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, UserAccount.AppId, 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDrCostCenter, "Id", "CostCenterName", "Cost Center", ZeroIndex: false);
				((UltraGridBase)CmbDrCostCenter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbDrCostCenter).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbDrCostCenter).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (DrId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DrId))
					{
						CmbDrCostCenter.Value = DrId;
					}
					else
					{
						((Control)(object)CmbDrCostCenter).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbDrCostCenter).DataSource = null;
				((Control)(object)CmbDrCostCenter).Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int DrjobLot = Conversion.ToInt(CmbDrJobLot.Value);
			List<JobLot> lst = jobLot.GetList(new JobLot
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("JobLotDescription");
			if (lst.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < lst.Count; i++)
			{
				dt.Rows.Add(lst[i].Id, lst[i].JobLotDescription);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDrJobLot, "Id", "JobLotDescription", "JobLot", ZeroIndex: true);
			}
			if (DrjobLot > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DrjobLot))
				{
					CmbDrJobLot.Value = DrjobLot;
				}
				else
				{
					((Control)(object)CmbDrJobLot).Text = string.Empty;
				}
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
			if (FormValidation())
			{
				table.Rows.Add(((UltraDropDownBase)CmbAccount).SelectedRow.Cells[2].Value, CmbAccount.Value, ((Control)(object)CmbAccount).Text, (!SubsidiaryAccountAllowOnVouchers) ? ((object)0) : ((SubsidiaryAccountDatasourceLength > 0) ? CmbSubsidiaryAccount.Value : CmbAccount.Value), (!SubsidiaryAccountAllowOnVouchers) ? "" : ((SubsidiaryAccountDatasourceLength > 0) ? ((Control)(object)CmbSubsidiaryAccount).Text : ((Control)(object)CmbAccount).Text), (!SubsidiaryAccountAllowOnVouchers) ? ((object)0) : ((SubsidiaryAccountDatasourceLength > 0) ? ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value : ((object)4)), txtDrRmks.Text.Trim(), CmbDrJobLot.Value, ((Control)(object)CmbDrJobLot).Text, txtCheq.Text.Trim(), Conversion.ToDouble((object)txtFcyDebit.Text.Trim()), txtAmount.Text.Trim(), Conversion.ToDouble((object)txtFcyCredit.Text.Trim()), txtCredit.Text.Trim(), BranchFeature ? Conversion.ToInt(CmbDrBranch.Value) : 0, BranchFeature ? Conversion.ToString((object)((Control)(object)CmbDrBranch).Text) : "", (Conversion.ToInt(CmbDrCostCenter.Value) > 0) ? Conversion.ToInt(CmbDrCostCenter.Value) : 0, (Conversion.ToInt(CmbDrCostCenter.Value) > 0) ? Conversion.ToString((object)((Control)(object)CmbDrCostCenter).Text) : "");
				grd.DataSource = table;
				grd.RetrieveStructure();
				gridsetting();
				txtDrRmks.Clear();
				txtFcyDebit.Clear();
				txtAmount.Clear();
				txtFcyCredit.Clear();
				txtCredit.Clear();
				txtCheq.Clear();
				CmbAccount.Focus();
				CalculateTotalInformation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grd.AllowEdit = (InheritableBoolean)2;
			grd.AllowDelete = (InheritableBoolean)2;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["CostCenterId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountTypeId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllowOnVouchers;
			grd.RootTable.Columns["FcyAmountCr"].Visible = MultiCurrencyFeatureVisibilty;
			grd.RootTable.Columns["FcyAmountDr"].Visible = MultiCurrencyFeatureVisibilty;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grd.RootTable.Columns["CostCenter"].Visible = IsBookingOffice;
			grd.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AmountDr"].FormatString = "#,##0.####";
			grd.RootTable.Columns["AmountDr"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AmountCr"].FormatString = "#,##0.####";
			grd.RootTable.Columns["AmountCr"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["FcyAmountDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmountDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmountDr"].FormatString = "#,##0.####";
			grd.RootTable.Columns["FcyAmountDr"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["FcyAmountCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmountCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmountCr"].FormatString = "#,##0.####";
			grd.RootTable.Columns["FcyAmountCr"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Caption = "Delete";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "Delete";
			grd.RootTable.Columns["Delete"].Width = 50;
			CommonServices.GridAutoAdjustmentNew(grd);
			ctrlGrdBar1_Load(null, null);
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
			btnplus.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelDetail.Visible = true;
			updateRowIndex = Conversion.ToInt((object)grd.CurrentRow.RowIndex);
			CmbAccount.Value = grd.CurrentRow.Cells["AccountId"].Text.ToString();
			CmbDr_Leave(null, null);
			if (SubsidiaryAccountDatasourceLength == 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("SubsidiaryAccount");
				dt.Columns.Add("TypeId");
				dt.Columns.Add("AccountId");
				dt.Columns.Add("AccountTitle");
				dt.Rows.Add(Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccountId"].Value), grd.CurrentRow.Cells["SubsidiaryAccount"].Text, 4, grd.CurrentRow.Cells["AccountId"].Text, grd.CurrentRow.Cells["AccountTitle"].Text);
				DDL.BindDDL(dt, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			if (Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccountId"].Value) > 0)
			{
				CmbSubsidiaryAccount.Value = Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccountId"].Value);
			}
			txtCheq.Text = grd.CurrentRow.Cells["CheqNo"].Text.ToString();
			txtDrRmks.Text = grd.CurrentRow.Cells["Remarks"].Text.ToString();
			if (Conversion.ToInt(grd.CurrentRow.Cells["JobLotId"].Value) > 0)
			{
				CmbDrJobLot.Value = Conversion.ToInt(grd.CurrentRow.Cells["JobLotId"].Value);
			}
			if (Conversion.ToInt(grd.CurrentRow.Cells["CostCenterId"].Value) > 0)
			{
				CmbDrCostCenter.Value = Conversion.ToInt(grd.CurrentRow.Cells["CostCenterId"].Value);
			}
			txtAmount.Text = grd.CurrentRow.Cells["AmountDr"].Text.ToString();
			txtCredit.Text = grd.CurrentRow.Cells["AmountCr"].Text.ToString();
			if (BranchFeature)
			{
				CmbDrBranch.Value = Conversion.ToInt(grd.CurrentRow.Cells["BranchId"].Value);
			}
			CmbAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (updateRowIndex > -1)
			{
				table.Rows[updateRowIndex]["AccountCode"] = ((UltraDropDownBase)CmbAccount).SelectedRow.Cells[2].Value;
				table.Rows[updateRowIndex]["AccountId"] = CmbAccount.Value;
				table.Rows[updateRowIndex]["AccountTitle"] = ((Control)(object)CmbAccount).Text;
				if (((Control)(object)CmbSubsidiaryAccount).Visible)
				{
					if (((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && ((Control)(object)CmbSubsidiaryAccount).Text != string.Empty)
					{
						table.Rows[updateRowIndex]["SubsidiaryAccountId"] = CmbSubsidiaryAccount.Value;
						table.Rows[updateRowIndex]["SubsidiaryAccount"] = ((Control)(object)CmbSubsidiaryAccount).Text;
						table.Rows[updateRowIndex]["SubsidiaryAccountTypeId"] = ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value;
					}
					else
					{
						table.Rows[updateRowIndex]["SubsidiaryAccountId"] = CmbAccount.Value;
						table.Rows[updateRowIndex]["SubsidiaryAccount"] = ((Control)(object)CmbAccount).Text;
						table.Rows[updateRowIndex]["SubsidiaryAccountTypeId"] = 4;
					}
				}
				else
				{
					table.Rows[updateRowIndex]["SubsidiaryAccountId"] = 0;
					table.Rows[updateRowIndex]["SubsidiaryAccount"] = "";
					table.Rows[updateRowIndex]["SubsidiaryAccountTypeId"] = 0;
				}
				table.Rows[updateRowIndex]["Remarks"] = txtDrRmks.Text;
				table.Rows[updateRowIndex]["JobLotId"] = CmbDrJobLot.Value;
				table.Rows[updateRowIndex]["JobLot"] = ((Control)(object)CmbDrJobLot).Text;
				table.Rows[updateRowIndex]["CheqNo"] = txtCheq.Text;
				table.Rows[updateRowIndex]["FcyAmountDr"] = Conversion.ToDouble((object)txtFcyDebit.Text);
				table.Rows[updateRowIndex]["AmountDr"] = txtAmount.Text;
				table.Rows[updateRowIndex]["FcyAmountCr"] = Conversion.ToDouble((object)txtFcyCredit.Text);
				table.Rows[updateRowIndex]["AmountCr"] = txtCredit.Text;
				table.Rows[updateRowIndex]["BranchId"] = Conversion.ToInt(CmbDrBranch.Value);
				table.Rows[updateRowIndex]["BranchName"] = ((Control)(object)CmbDrBranch).Text;
				table.Rows[updateRowIndex]["CostCenterId"] = Conversion.ToInt(CmbDrCostCenter.Value);
				table.Rows[updateRowIndex]["CostCenter"] = ((Control)(object)CmbDrCostCenter).Text;
				updateRowIndex = -1;
			}
			((Control)(object)CmbAccount).Text = string.Empty;
			CmbDr_Leave(null, null);
			txtDrRmks.Clear();
			txtAmount.Clear();
			txtCheq.Clear();
			lbDr.Text = "";
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			CmbAccount.Focus();
			CommonServices.GridAutoAdjustmentNew(grd);
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			((Control)(object)CmbAccount).Text = string.Empty;
			CmbDr_Leave(null, null);
			txtDrRmks.Clear();
			txtAmount.Clear();
			txtCheq.Clear();
			lbDr.Text = "";
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
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "Delete")
			{
				item.Delete();
			}
			grd.UpdateData();
			CalculateTotalInformation();
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
			BranchesFill();
			combojoblotfill();
			DebitAccountTitleFill();
			if (SubsidiaryAccountAllowOnVouchers)
			{
				BindAllSubsidiaryaccounts();
			}
			DefaultConfigurations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void New_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			RecId = 0;
			txtFcyAmount.Text = string.Empty;
			Approved = false;
			((Control)(object)CmbAccount).Text = string.Empty;
			BindDebitSubsidiaryAccount();
			txtVoucherremarks.Text = string.Empty;
			lbDr.Text = "";
			txtDebitBalance.Text = "";
			txtCredit.Text = "";
			txtFcyAmountCr.Text = "";
			txtFcyAmount.Text = "";
			txtDrRmks.Clear();
			txtAmount.Clear();
			txtCheq.Clear();
			table.Clear();
			Save.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			grd.ClearStructure();
			VoucherCode();
			DocDate.Focus();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			MultiCurrencyFeatureVisibilty = CommonServices.GetERPFeatureById(6);
			if (MultiCurrencyFeatureVisibilty)
			{
				lblFcyCode.Visible = true;
				((Control)(object)cmbCurrency).Visible = true;
				lblExchangeRate.Visible = true;
				txtExchangeRate.Visible = true;
				lblFcyAmount.Visible = true;
				txtFcyAmount.Visible = true;
				FcyDebitPanel.Visible = true;
				FcyCreditPanel.Visible = true;
				txtVoucherremarks.Size = new Size(437, 53);
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				lblFcyCode.Visible = false;
				lblExchangeRate.Visible = false;
				lblFcyAmount.Visible = false;
				FcyDebitPanel.Visible = false;
				FcyCreditPanel.Visible = false;
				txtVoucherremarks.Size = new Size(846, 53);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DayBookVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 600);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			Save.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
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
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			CurrencyFill();
			BranchesFill();
			CombProjectFill();
			LocationType();
			MultiCurrencyFeature();
			BranchFeature = CommonServices.GetERPFeatureById(17);
			SubsidiaryAccountAllowOnVouchers = CommonServices.GetERPFeatureById(4);
			if (SubsidiaryAccountAllowOnVouchers)
			{
				BindAllSubsidiaryaccounts();
				DebitSubsidiaryPanel.Visible = true;
				lblSubsidiaryaccount.Visible = true;
				((Control)(object)CmbSubsidiaryAccount).Visible = true;
			}
			else
			{
				DebitSubsidiaryPanel.Visible = false;
				labelDrSubsidiary.Visible = false;
				labelSubsidiaryDebitBal.Visible = false;
				lblSubsidiaryaccount.Visible = false;
				((Control)(object)CmbSubsidiaryAccount).Visible = false;
			}
			if (BranchFeature)
			{
				DrBranchPanel.Visible = true;
				label12.Visible = true;
				((Control)(object)CmbDrBranch).Visible = true;
			}
			else
			{
				DrBranchPanel.Visible = false;
				label12.Visible = false;
				((Control)(object)CmbDrBranch).Visible = false;
			}
			table.Columns.Add("AccountCode");
			table.Columns.Add("AccountId");
			table.Columns.Add("AccountTitle");
			table.Columns.Add("SubsidiaryAccountId");
			table.Columns.Add("SubsidiaryAccount");
			table.Columns.Add("SubsidiaryAccountTypeId");
			table.Columns.Add("Remarks");
			table.Columns.Add("JobLotId");
			table.Columns.Add("JobLot");
			table.Columns.Add("CheqNo");
			table.Columns.Add("FcyAmountDr", typeof(double));
			table.Columns.Add("AmountDr", typeof(double));
			table.Columns.Add("FcyAmountCr", typeof(double));
			table.Columns.Add("AmountCr", typeof(double));
			table.Columns.Add("BranchId", typeof(int));
			table.Columns.Add("BranchName");
			table.Columns.Add("CostCenterId", typeof(int));
			table.Columns.Add("CostCenter");
			grd.DataSource = table;
			grd.RetrieveStructure();
			gridsetting();
			dtDrAc.Columns.Add("Id", typeof(int));
			dtDrAc.Columns.Add("AccountTitle");
			dtDrAc.Columns.Add("AccountCode");
			dtDrAc.Columns.Add("ParentAccount");
			dtDrAc.Columns.Add("AccountClass");
			combojoblotfill();
			DebitAccountTitleFill();
			CostCenterFill();
			ApplicationsAllocateToCompany obj = clsGlobalVariables.GetApplicationsAllocateToCompanyList.Find((ApplicationsAllocateToCompany row) => row.AppId == 5 && row.CompanyId == UserAccount.CompanyId);
			IsBookingOffice = obj != null && obj.IsActive;
			if (!IsBookingOffice)
			{
				DrCostCenterPanel.Visible = false;
			}
			if (!SubsidiaryAccountAllowOnVouchers)
			{
				DrCommentsPanel.Size = new Size(353, 47);
				txtDrRmks.Size = new Size(353, 23);
			}
			if (MultiCurrencyFeatureVisibilty)
			{
				flowLayoutPanel1.Size = new Size(1186, 97);
				groupBox2.Size = new Size(1195, 119);
			}
			else if (!MultiCurrencyFeatureVisibilty && !BranchFeature && !IsBookingOffice && !SubsidiaryAccountAllowOnVouchers)
			{
				flowLayoutPanel1.Size = new Size(1186, 47);
				groupBox2.Size = new Size(1195, 75);
				panel4.Size = new Size(1200, 160);
			}
			lbDr.Visible = false;
			txtDebitBalance.Visible = false;
			ChkBoxPrintPreview.Checked = true;
			DocDate.Focus();
			Save.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			VoucherCode();
			DefaultConfigurations();
			StatusFill();
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
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
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Not Approved");
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApproveStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefaultConfigurations()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbDrJobLot.Value = JobId;
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int Currency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				BaseCurrency = Currency;
				cmbCurrency.Value = BaseCurrency;
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				double BaseCurrencyRate = (BaseRate = Conversion.ToDouble((object)BaseRatelist[0].ConfigKey.ToString()));
				txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Save_Click(object sender, EventArgs e)
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
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Expected O, but got Unknown
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_0b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9b: Expected O, but got Unknown
		//IL_0bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Conversion.ToInt(CmbProjectId.Value) == 0)
			{
				MessageBox.Show("Cost Center Field is Required");
				CmbProjectId.Focus();
			}
			else if (grd.RowCount > 0)
			{
				if (MultiCurrencyFeatureVisibilty)
				{
					if (Conversion.ToInt(cmbCurrency.Value) == 0)
					{
						MessageBox.Show("Fcy Code Field is Required");
						cmbCurrency.Focus();
						return;
					}
					if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
					{
						MessageBox.Show("Exchange Rate Field is Required");
						txtExchangeRate.Focus();
						return;
					}
					if (Conversion.ToDouble((object)txtFcyAmount.Text) == 0.0)
					{
						MessageBox.Show("Fcy Amount Field is Required");
						txtFcyAmount.Focus();
						return;
					}
				}
				else
				{
					if (Conversion.ToInt(cmbCurrency.Value) == 0)
					{
						MessageBox.Show("Please Configure Your Base Currency In configurations");
						return;
					}
					if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
					{
						MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
						return;
					}
				}
				VoucherHead vh = new VoucherHead();
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					vh.Id = RecId;
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (Conversion.ToDouble(r3.Cells["AmountDr"].Value) > 0.0 && Conversion.ToInt(r3.Cells["AccountId"].Value) == 0)
					{
						MessageBox.Show($"Please Select Account Title First in Row#{r3.RowIndex + 1}");
						return;
					}
				}
				rows = grd.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToDouble(r2.Cells["AmountCr"].Value) > 0.0 && Conversion.ToInt(r2.Cells["AccountId"].Value) == 0)
					{
						MessageBox.Show($"Please Select Account Title First in Row#{r2.RowIndex + 1}");
						return;
					}
				}
				vh.DocumentTypeId = 17;
				vh.DocumentTypeSrNo = 0;
				vh.VoucherCode = Conversion.ToInt((object)txtdocno.Text.Trim());
				vh.ProjectId = Conversion.ToInt((object)CmbProjectId.Value.ToString());
				vh.VoucherDate = DocDate.Value;
				vh.RemarksOtherLingo = "";
				vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				vh.ChequeNo = txtCheq.Text.Trim().ToString();
				vh.Remarks = Conversion.ToString((object)txtVoucherremarks.Text);
				vh.EntryDate = DateTime.Now;
				vh.EntryUser = UserAccount.ID;
				vh.ModifyDate = DateTime.Now;
				vh.ModifyUser = UserAccount.ID;
				vh.OrganizationId = UserAccount.OrganizationId;
				vh.CompanyId = UserAccount.CompanyId;
				vh.BranchId = UserAccount.BranchesId;
				vh.MultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
				vh.ExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				vh.FcAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
				vh.CustomAccounts = chkCustomAccounts.Checked;
				vh.voucherDetailList = new List<VoucherDetail>();
				int LineId = 1;
				rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					vd.AccountTitle = Conversion.ToString(r.Cells["AccountTitle"].Value);
					vd.AgainstAccountId = vd.AccountId;
					vd.SubsidiaryTypeId = Conversion.ToInt(r.Cells["SubsidiaryAccountTypeId"].Value);
					int SubsidiaryAccountId = Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
					if (SubsidiaryAccountId > 0)
					{
						vd.SupplierCustomerId = ((vd.SubsidiaryTypeId == 1) ? SubsidiaryAccountId : 0);
						vd.EmployeeId = ((vd.SubsidiaryTypeId == 2) ? SubsidiaryAccountId : 0);
						vd.SubsidiaryAccountId = SubsidiaryAccountId;
					}
					vd.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.CheqNoDetail = Conversion.ToString(r.Cells["CheqNo"].Value);
					vd.DebitAmount = Conversion.ToDouble(r.Cells["AmountDr"].Value);
					vd.CreditAmount = Conversion.ToDouble(r.Cells["AmountCr"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.LineId = LineId++;
					vd.SortNo = Conversion.ToInt((object)(r.RowIndex + 1));
					vd.DMultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
					vd.DExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
					vd.DCurrencyAmount = ((vd.CreditAmount > 0.0) ? Conversion.ToDouble(r.Cells["FcyAmountCr"].Value) : Conversion.ToDouble(r.Cells["FcyAmountDr"].Value));
					vd.ActionId = ((RecId <= 0) ? 1 : 2);
					vd.BranchesId = (BranchFeature ? Conversion.ToInt(r.Cells["BranchId"].Value) : UserAccount.BranchesId);
					vd.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
					vd.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
					vh.voucherDetailList.Add(vd);
				}
				object Debit = grd.GetTotal(grd.RootTable.Columns["AmountDr"], (AggregateFunction)2);
				object total = grd.GetTotal(grd.RootTable.Columns["AmountCr"], (AggregateFunction)2);
				double DebitAmount = Conversion.ToDouble(Debit);
				double CreditAmount = Conversion.ToDouble(total);
				if (Math.Abs(DebitAmount - CreditAmount) >= 1.0)
				{
					MessageBox.Show("Debit & Credit side not equal");
					return;
				}
				DataTable source = (DataTable)grd.DataSource;
				var rowsDr = (from x in source.AsEnumerable().Select((DataRow row, int index) => new
					{
						Row = row,
						RowIndex = index
					})
					where x.Row.Field<int>("CostCenterId") > 0
					select new
					{
						RowIndex = x.RowIndex + 1,
						Amount = x.Row.Field<double>("AmountDr"),
						CostCenterId = x.Row.Field<int>("CostCenterId")
					}).ToList();
				var rowsCr = (from x in source.AsEnumerable().Select((DataRow row, int index) => new
					{
						Row = row,
						RowIndex = index
					})
					where x.Row.Field<int>("CostCenterId") > 0
					select new
					{
						RowIndex = x.RowIndex + 1,
						Amount = x.Row.Field<double>("AmountCr"),
						CostCenterId = x.Row.Field<int>("CostCenterId")
					}).ToList();
				vh.costCenterDetailList = new List<voucherCostCenterDetail>();
				if (rowsDr.Count > 0 || rowsCr.Count > 0)
				{
					foreach (var item2 in rowsDr)
					{
						voucherCostCenterDetail d2 = new voucherCostCenterDetail();
						d2.Id = 0;
						d2.SortNo = Conversion.ToInt((object)item2.RowIndex);
						d2.CostCenterId = Conversion.ToInt((object)item2.CostCenterId);
						d2.costPrcent = 100m;
						d2.costAmount = Conversion.ToDouble((object)item2.Amount);
						vh.costCenterDetailList.Add(d2);
					}
					foreach (var item in rowsCr)
					{
						voucherCostCenterDetail d = new voucherCostCenterDetail();
						d.Id = 0;
						d.SortNo = Conversion.ToInt((object)item.RowIndex);
						d.CostCenterId = Conversion.ToInt((object)item.CostCenterId);
						d.costPrcent = 100m;
						d.costAmount = Conversion.ToDouble((object)item.Amount);
						vh.costCenterDetailList.Add(d);
					}
				}
				vh.costCenterDetailList.Sort((voucherCostCenterDetail p1, voucherCostCenterDetail p2) => p1.SortNo.CompareTo(p2.SortNo));
				vh.VoucherAmount = DebitAmount;
				vh.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				vh.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<VoucherHead>(vh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				if (string.IsNullOrEmpty(AttachmentsValues) || AT.lst.Count > 0)
				{
					AttachmentsValues = "";
				}
				vh.AttachmentsValues = AttachmentsValues;
				vh.CustomAttachmentsValues = CustomAttachmentsValues;
				if (!global::Architecture.BLL.Accounts.VoucherHead.VoucherExistWithSameAmountInSameDate(vh))
				{
					return;
				}
				int success = global::Architecture.BLL.Accounts.VoucherHead.Save(vh);
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully...[" + vh.VoucherCode + "]");
				}
				else
				{
					MessageBox.Show("Record Update Successfully...[" + vh.VoucherCode + "]");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 17, 0);
					}
				}
				Reset();
				if (ChkBoxPrintPreview.Checked)
				{
					DataTable dt = new DataTable();
					dt = VoucherReports.VoucherReport(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 17,
						Id = success,
						ApprovedFilter = "All"
					});
					if (dt == null || dt.Rows.Count == 0)
					{
						MessageBox.Show("Not Record Found For Display");
					}
					else
					{
						Reporting val = null;
						val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
					}
				}
				Save.Visible = true;
				grd.ClearStructure();
			}
			else
			{
				MessageBox.Show("Grid Fields Required");
			}
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
			if (RecId == 0)
			{
				throw new Exception("Record Not Update  " + RecId);
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Reset();
			RecId = Conversion.ToInt((object)ID);
			new VoucherDetail();
			VoucherHead voucherHead = global::Architecture.BLL.Accounts.VoucherHead.GetByID(RecId);
			if (voucherHead != null)
			{
				tabControl1.SelectedIndex = 0;
				CmbProjectId.Value = voucherHead.ProjectId;
				DocDate.Value = voucherHead.VoucherDate;
				txtCheq.Text = voucherHead.ChequeNo;
				txtdocno.Text = voucherHead.VoucherCode.ToString();
				txtVoucherremarks.Text = Conversion.ToString((object)voucherHead.Remarks);
				cmbCurrency.Value = voucherHead.MultiCurrencyId;
				txtExchangeRate.Text = voucherHead.ExchangeCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtFcyAmount.Text = voucherHead.FcAmount.ToString("#,##0.###");
				chkCustomAccounts.Checked = voucherHead.CustomAccounts;
				if (voucherHead.MultiCurrencyId == 0 || voucherHead.ExchangeCurrencyRate == 0.0)
				{
					DefaultConfigurations();
				}
				AttachmentsValues = voucherHead.AttachmentsValues;
				CustomAttachmentsValues = voucherHead.CustomAttachmentsValues;
				table.Columns.Clear();
				if (table.Columns.Count == 0)
				{
					table.Columns.Add("AccountCode");
					table.Columns.Add("AccountId", typeof(int));
					table.Columns.Add("AccountTitle");
					table.Columns.Add("SubsidiaryAccountId", typeof(int));
					table.Columns.Add("SubsidiaryAccount");
					table.Columns.Add("SubsidiaryAccountTypeId", typeof(int));
					table.Columns.Add("Remarks");
					table.Columns.Add("JobLotId");
					table.Columns.Add("JobLot");
					table.Columns.Add("CheqNo");
					table.Columns.Add("FcyAmountDr", typeof(double));
					table.Columns.Add("AmountDr", typeof(double));
					table.Columns.Add("FcyAmountCr", typeof(double));
					table.Columns.Add("AmountCr", typeof(double));
					table.Columns.Add("BranchId", typeof(int));
					table.Columns.Add("BranchName", typeof(string));
					table.Columns.Add("CostCenterId", typeof(int));
					table.Columns.Add("CostCenter", typeof(string));
				}
				table.Rows.Clear();
				for (int i = 0; i < voucherHead.voucherDetailList.Count; i++)
				{
					table.Rows.Add(voucherHead.voucherDetailList[i].AccountCode, voucherHead.voucherDetailList[i].AccountId, voucherHead.voucherDetailList[i].AccountTitle, voucherHead.voucherDetailList[i].SubsidiaryAccountId, voucherHead.voucherDetailList[i].SubsidiaryAccountTitle, voucherHead.voucherDetailList[i].SubsidiaryTypeId, voucherHead.voucherDetailList[i].Comments, voucherHead.voucherDetailList[i].JobLotId, voucherHead.voucherDetailList[i].JobLotDescription, voucherHead.voucherDetailList[i].CheqNoDetail, (voucherHead.voucherDetailList[i].DebitAmount > 0.0) ? voucherHead.voucherDetailList[i].DCurrencyAmount : 0.0, voucherHead.voucherDetailList[i].DebitAmount, (voucherHead.voucherDetailList[i].CreditAmount > 0.0) ? voucherHead.voucherDetailList[i].DCurrencyAmount : 0.0, voucherHead.voucherDetailList[i].CreditAmount, BranchFeature ? voucherHead.voucherDetailList[i].BranchesId : 0, BranchFeature ? voucherHead.voucherDetailList[i].BranchName : "", voucherHead.voucherDetailList[i].CostCenterId, voucherHead.voucherDetailList[i].CostCenterName);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				gridsetting();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				Save.Visible = false;
				btnUpdate.Visible = true;
				UpdateMode = true;
				cmbLocationType.Value = voucherHead.voucherDetailList[0].LocationTypeId;
				txtExchangeRate_TextChanged(null, null);
				DocDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill(int NoofRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.Ids = "17";
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = UserAccount.ID;
			}
			if (rdentdoc.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdententry.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdentmodify.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdentapproved.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			vh.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			vh.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			if (((Control)(object)cmbApproveStatus).Text == "Approved")
			{
				vh.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatus).Text == "Not Apporved")
			{
				vh.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatus).Text == "All")
			{
				vh.ApprovedFilter = "All";
			}
			DataTable lst = global::Architecture.BLL.Accounts.VoucherHead.VoucherFormHistory(vh);
			if (lst.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id");
				dtHistory.Columns.Add("VoucherCode", typeof(int));
				dtHistory.Columns.Add("DocumentTypeId");
				dtHistory.Columns.Add("DocumentTypeCode", typeof(string));
				dtHistory.Columns.Add("VoucherDate", typeof(DateTime));
				dtHistory.Columns.Add("ManualBillNo");
				dtHistory.Columns.Add("AccountTitle");
				dtHistory.Columns.Add("AgainstAccount");
				dtHistory.Columns.Add("Remarks");
				dtHistory.Columns.Add("VoucherAmount", typeof(double));
				dtHistory.Columns.Add("FcyCode");
				dtHistory.Columns.Add("ExchangeRate", typeof(double));
				dtHistory.Columns.Add("FcyAmount", typeof(double));
				dtHistory.Columns.Add("EntryUser");
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser");
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("IsApproved");
				dtHistory.Columns.Add("ApprovedUser");
				dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
				dtHistory.Columns.Add("CheqNo");
				dtHistory.Columns.Add("Attachment");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtHistory.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["ManualBillNo"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["AgainstAccount"], lst.Rows[i]["Remarks"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["IsApproved"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistory;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
				VoucherInfoBox.Visible = true;
				lblTotalVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalVouchers"]).ToString();
				lblApprovedVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalApprovedVoucher"]).ToString();
				lblUnApprovedVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalUnApprovedVoucher"]).ToString();
			}
			else
			{
				VoucherInfoBox.Visible = false;
				DataGridHistory.ClearStructure();
				grdDetail.ClearStructure();
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
			DataGridHistory.RootTable.Columns["FcyCode"].Visible = MultiCurrencyFeatureVisibilty;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Visible = MultiCurrencyFeatureVisibilty;
			DataGridHistory.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["VoucherAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			DataGridHistory.RootTable.Columns["VoucherCode"].Caption = "V.No";
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Caption = "V.Type";
			DataGridHistory.RootTable.Columns["VoucherDate"].Width = 90;
			DataGridHistory.RootTable.Columns["VoucherCode"].Width = 15;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Width = 30;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("AddAttachment");
			DataGridHistory.RootTable.Columns["AddAttachment"].Caption = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonText = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].Width = 110;
			DataGridHistory.RootTable.Columns.Add("Delete");
			DataGridHistory.RootTable.Columns["Delete"].Caption = "\u200cX";
			DataGridHistory.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Delete"].ButtonText = "X";
			DataGridHistory.RootTable.Columns["Delete"].Width = 20;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["VoucherCode"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["VoucherDate"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["EntryUser"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["VoucherAmount"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns["Edit"].Position = 1;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["IsApproved"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
			DataGridHistory.RootTable.Columns["AccountTitle"].Visible = false;
			DataGridHistory.RootTable.Columns["CheqNo"].Visible = false;
			DataGridHistory.RootTable.Columns["Remarks"].Visible = false;
			DataGridHistory.RootTable.Columns["AgainstAccount"].Visible = false;
			DataGridHistory.RootTable.Columns["Remarks"].Width = 350;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridDetailsetting()
	{
		try
		{
			grdDetail.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllowOnVouchers;
			grdDetail.RootTable.Columns["DebitFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			grdDetail.RootTable.Columns["CreditFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grdDetail.RootTable.Columns["CostCenter"].Visible = IsBookingOffice;
			grdDetail.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["AccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["AccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["AccountId"].Caption = "Account Title";
			grdDetail.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtDrAc.DefaultView, "Id", "AccountTitle");
			grdDetail.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["CheqNo"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["AmountDr"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["AmountCr"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AmountDr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountDr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AmountCr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountCr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DebitFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DebitFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DebitFcyAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["DebitFcyAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["CreditFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CreditFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CreditFcyAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["CreditFcyAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["SubsidiaryAccount"].Visible = false;
			grdDetail.RootTable.Columns["AmountDr"].Caption = "Debit";
			grdDetail.RootTable.Columns["AmountCr"].Caption = "Credit";
			grdDetail.RootTable.Columns["DebitFcyAmount"].Caption = "Fcy Debit";
			grdDetail.RootTable.Columns["CreditFcyAmount"].Caption = "Fcy Credit";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick_1(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Print")
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 17);
				}
				else if (e.Column.Key == "Edit")
				{
					RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
					ReadById(RecId);
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 17, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
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
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 17);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		HistoryGridFill();
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
			MessageBox.Show(ex.Message);
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

	private void DayBookVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				if (Save.Visible && Save.Enabled)
				{
					Save_Click(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				btnshow_Click(null, null);
			}
		}
		if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if (btnDelete.Visible && btnDelete.Enabled && e.Control && e.KeyCode == Keys.Delete)
		{
			btnDelete_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				New_Click(null, null);
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				btnNewHistory_Click(null, null);
			}
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
				FromDateHistory.Focus();
			}
		}
		if (e.Alt && e.Shift && e.KeyCode == Keys.T && tabControl1.SelectedIndex == 0)
		{
			btnLoadTrialBalance.Visible = true;
			btnGenerateRemarks.Visible = true;
		}
		if (e.Control && e.Shift && e.KeyCode == Keys.L && tabControl1.SelectedIndex == 0)
		{
			btnLoadTrialBalance_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.E)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
		{
			Print_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.R && tabControl1.SelectedIndex == 0)
		{
			btnRefresh_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F10)
		{
			btnAttachment_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				DocDate.Focus();
			}
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grd).Focus();
			}
			if (tabControl1.SelectedIndex == 1)
			{
				((Control)(object)DataGridHistory).Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				CmbAccount.Focus();
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
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

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.AcRptPaymentReceiptsVoucherSlip_102(RecId, 17);
				return;
			}
			throw new Exception("No Data Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtCrRmks_Leave(object sender, EventArgs e)
	{
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
			if (!_isChanging)
			{
				_isChanging = true;
				if (!string.IsNullOrWhiteSpace(txtAmount.Text) && decimal.TryParse(txtAmount.Text, out var debit) && debit > 0m)
				{
					txtCredit.Text = "0";
				}
				_isChanging = false;
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0 && base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "DebitAmount")
				{
					txtExchangeRate.Focus();
					txtFcyDebit.Text = "0";
					txtAmount.Text = "0";
					throw new Exception("Please enter exchange Rate First");
				}
				CommonServices.UpdateRelatedFieldsCalculation(txtAmount, txtFcyDebit, Conversion.ToDouble((object)txtExchangeRate.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDebitSubsidiaryAccount()
	{
		try
		{
			if (((UltraGridBase)CmbAccount).ActiveRow != null && ((Control)(object)CmbAccount).Text != string.Empty)
			{
				if (dtSubsidory.Rows.Count > 0)
				{
					if (((UltraGridBase)CmbAccount).ActiveRow != null && ((Control)(object)CmbAccount).Text != string.Empty)
					{
						int selectedAcId = Conversion.ToInt(CmbAccount.Value);
						if (selectedAcId != 0)
						{
							DataTable matchingTable = dtSubsidory.Clone();
							foreach (DataRow row2 in dtSubsidory.Rows)
							{
								if (Conversion.ToInt(row2["AccountId"]) == selectedAcId)
								{
									matchingTable.ImportRow(row2);
								}
							}
							int SubsidiaryIdInCaseOfUpdateDetail = 0;
							if (((Control)(object)CmbSubsidiaryAccount).Text != string.Empty)
							{
								SubsidiaryIdInCaseOfUpdateDetail = Conversion.ToInt(CmbSubsidiaryAccount.Value);
							}
							if (matchingTable.Rows.Count > 0)
							{
								SubsidiaryAccountDatasourceLength = matchingTable.Rows.Count;
								DDL.BindDDL(matchingTable, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
								((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
								((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
								((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
								((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
								if (matchingTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SubsidiaryIdInCaseOfUpdateDetail))
								{
									CmbSubsidiaryAccount.Value = SubsidiaryIdInCaseOfUpdateDetail;
								}
								else
								{
									((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
								}
							}
							else
							{
								((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
								((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
								SubsidiaryAccountDatasourceLength = 0;
							}
						}
						else
						{
							((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
							((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
							SubsidiaryAccountDatasourceLength = 0;
						}
					}
					else
					{
						SubsidiaryAccountDatasourceLength = dtSubsidory.Rows.Count;
						DDL.BindDDL(dtSubsidory, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
						((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
						((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					}
				}
				else
				{
					((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
					((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
					SubsidiaryAccountDatasourceLength = 0;
				}
			}
			else
			{
				((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
				((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
				SubsidiaryAccountDatasourceLength = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				VoucherHead byID = global::Architecture.BLL.Accounts.VoucherHead.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
				DataTable table = new DataTable();
				table.Columns.Add("AccountCode");
				table.Columns.Add("AccountId");
				table.Columns.Add("SubsidiaryAccount");
				table.Columns.Add("JobLot");
				table.Columns.Add("Remarks");
				table.Columns.Add("CheqNo");
				table.Columns.Add("DebitFcyAmount", typeof(double));
				table.Columns.Add("AmountDr", typeof(double));
				table.Columns.Add("CreditFcyAmount", typeof(double));
				table.Columns.Add("AmountCr", typeof(double));
				table.Columns.Add("BranchName", typeof(string));
				table.Columns.Add("CostCenter", typeof(string));
				table.Rows.Clear();
				List<VoucherDetail> sortingVoucherDetailList = byID.voucherDetailList.OrderBy((VoucherDetail x) => x.SubNo).ToList();
				for (int i = 0; i < sortingVoucherDetailList.Count; i++)
				{
					table.Rows.Add(sortingVoucherDetailList[i].AccountCode, sortingVoucherDetailList[i].AccountId, sortingVoucherDetailList[i].SubsidiaryAccountTitle, sortingVoucherDetailList[i].JobLotDescription, sortingVoucherDetailList[i].Comments, sortingVoucherDetailList[i].CheqNoDetail, (sortingVoucherDetailList[i].DebitAmount > 0.0) ? sortingVoucherDetailList[i].DCurrencyAmount : 0.0, sortingVoucherDetailList[i].DebitAmount, (sortingVoucherDetailList[i].CreditAmount > 0.0) ? sortingVoucherDetailList[i].DCurrencyAmount : 0.0, sortingVoucherDetailList[i].CreditAmount, sortingVoucherDetailList[i].BranchName, sortingVoucherDetailList[i].CostCenterName);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				gridDetailsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindAllSubsidiaryaccounts()
	{
		try
		{
			dtSubsidory = global::Architecture.BLL.Accounts.VoucherHead.BindSubsidiaryAccount(UserAccount.CompanyId, 0, (string)null);
			if (dtSubsidory.Rows.Count > 0)
			{
				DDL.BindDDL(dtSubsidory, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbDrSubsidiaryAccount_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbAccount.Value) > 0 && Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
			{
				labelDrSubsidiary.Visible = true;
				labelSubsidiaryDebitBal.Visible = true;
				labelSubsidiaryDebitBal.Text = "0";
				double ClosingBalance = CommonServices.GetSubsiadiaryBalance(Conversion.ToInt(CmbAccount.Value), Conversion.ToInt(CmbSubsidiaryAccount.Value), DocDate.Value);
				labelSubsidiaryDebitBal.Text = ClosingBalance.ToString("#,#;(#,#);0");
				if (ClosingBalance > 0.0)
				{
					labelDrSubsidiary.Text = "Dr";
				}
				else
				{
					labelDrSubsidiary.Text = "Cr";
				}
				if (ClosingBalance == 0.0)
				{
					labelDrSubsidiary.Text = "Nill";
				}
			}
			else
			{
				labelDrSubsidiary.Visible = false;
				labelSubsidiaryDebitBal.Visible = false;
				labelSubsidiaryDebitBal.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindDebitParentAccount()
	{
		try
		{
			if (!AfterCloseupSubsidiary && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && updateRowIndex <= -1)
			{
				CmbAccount.Value = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[4].Value);
				CmbDr_Leave(null, null);
			}
			AfterCloseupSubsidiary = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherEntry_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtDrRmks_Leave(object sender, EventArgs e)
	{
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Debit Account in Detail Box");
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

	private void btnplus_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "grd")
			{
				CmbAccount.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!shiftTabPressed)
			{
				CmbAccount.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grd.CurrentColumn.Key == "Delete")
				{
					int UniqueId = Conversion.ToInt(item.Cells["LineId"].Value);
					if (UniqueId % 2 == 0)
					{
						for (int i = 0; i < table.Rows.Count; i++)
						{
							if (Conversion.ToInt(table.Rows[i]["LineId"]) == UniqueId - 1)
							{
								table.Rows[i].Delete();
								break;
							}
						}
						item.Delete();
					}
					else
					{
						for (int j = 0; j < table.Rows.Count; j++)
						{
							if (Conversion.ToInt(table.Rows[j]["LineId"]) == UniqueId + 1)
							{
								table.Rows[j].Delete();
								break;
							}
						}
						item.Delete();
					}
				}
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(id);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(id, 17);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(id);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Attachment")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, 17);
				}
				else if (DataGridHistory.CurrentColumn.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, id, 17, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		try
		{
			if (e.Shift && e.KeyCode == Keys.Tab)
			{
				shiftTabPressed = true;
			}
			else
			{
				shiftTabPressed = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double amount = Conversion.ToDouble(obj.Cells["AmountDr"].Value) / Conversion.ToDouble((object)txtExchangeRate.Text);
					obj.Cells["FcyAmountDr"].Value = amount;
					double amountCr = Conversion.ToDouble(obj.Cells["AmountCr"].Value) / Conversion.ToDouble((object)txtExchangeRate.Text);
					obj.Cells["FcyAmountCr"].Value = amountCr;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FcyAmountDr"].Value = 0;
					obj2.Cells["FcyAmountCr"].Value = 0;
					obj2.EndEdit();
				}
			}
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				double TotalFcyAmountDr = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["FcyAmountDr"], (AggregateFunction)2));
				double TotalFcyAmountCr = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["FcyAmountCr"], (AggregateFunction)2));
				txtFcyAmount.Text = TotalFcyAmountDr.ToString("#,##0.###");
				txtFcyAmountCr.Text = TotalFcyAmountCr.ToString("#,##0.###");
				double num = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["AmountDr"], (AggregateFunction)2));
				double Credit = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["AmountCr"], (AggregateFunction)2));
				txtDifference.Text = (num - Credit).ToString("#,##0.####");
			}
			else
			{
				txtFcyAmount.Text = "0";
				txtDifference.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate.Text = Conversion.ToDecimal((object)txtExchangeRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "17",
					Ids = Conversion.ToString(cmbCurrency.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtExchangeRate.Text = Conversion.ToDouble(dt.Rows[0]["LastExchRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else
				{
					txtExchangeRate.Text = "0";
				}
			}
			else
			{
				txtExchangeRate.Text = BaseRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
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

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
			VoucherInfoBox.Visible = false;
			DataGridHistory.ClearStructure();
			grdDetail.ClearStructure();
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
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDrSubsidiaryAccount_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloseupSubsidiary = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbAccount.Value = GLOfSubSidory;
					CmbDr_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "DataGridHistory")
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDrBranch_Leave(object sender, EventArgs e)
	{
	}

	private void CmbDrSubsidiaryAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			BindDebitParentAccount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtCredit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (!_isChanging)
			{
				_isChanging = true;
				if (!string.IsNullOrWhiteSpace(txtCredit.Text) && decimal.TryParse(txtCredit.Text, out var credit) && credit > 0m)
				{
					txtAmount.Text = "0";
				}
				_isChanging = false;
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0 && base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "FcyAmountDebit")
				{
					txtExchangeRate.Focus();
					txtFcyDebit.Text = "0";
					txtAmount.Text = "0";
					throw new Exception("Please enter exchange Rate First");
				}
				CommonServices.UpdateRelatedFieldsCalculation(txtCredit, txtFcyCredit, Conversion.ToDouble((object)txtExchangeRate.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete because Record has approved");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyDebit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (!_isChangingFcy)
			{
				_isChangingFcy = true;
				if (!string.IsNullOrWhiteSpace(txtFcyDebit.Text) && decimal.TryParse(txtFcyDebit.Text, out var credit) && credit > 0m)
				{
					txtFcyCredit.Text = "0";
				}
				_isChangingFcy = false;
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0 && base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "FcyAmountDebit")
				{
					txtExchangeRate.Focus();
					txtFcyDebit.Text = "0";
					txtAmount.Text = "0";
					throw new Exception("Please enter exchange Rate First");
				}
				CommonServices.UpdateRelatedFieldsCalculation(txtAmount, txtFcyDebit, Conversion.ToDouble((object)txtExchangeRate.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyCredit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (!_isChangingFcy)
			{
				_isChangingFcy = true;
				if (!string.IsNullOrWhiteSpace(txtFcyCredit.Text) && decimal.TryParse(txtFcyCredit.Text, out var credit) && credit > 0m)
				{
					txtFcyDebit.Text = "0";
				}
				_isChangingFcy = false;
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0 && base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "FcyAmountCredit")
				{
					txtExchangeRate.Focus();
					txtFcyCredit.Text = "0";
					txtAmount.Text = "0";
					throw new Exception("Please enter exchange Rate First");
				}
				CommonServices.UpdateRelatedFieldsCalculation(txtCredit, txtFcyCredit, Conversion.ToDouble((object)txtExchangeRate.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadTrialBalance_Click(object sender, EventArgs e)
	{
		try
		{
			LoadTrialBalance = new TrialBalance(UserAccount);
			LoadTrialBalance.RequestFromJournalVoucher = true;
			LoadTrialBalance.BringToFront();
			LoadTrialBalance.ShowDialog();
			LoadTrialData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadTrialData()
	{
		try
		{
			DataTable data = LoadTrialBalance.dtForLoad;
			if (data.Rows.Count <= 0)
			{
				return;
			}
			bool Flag = false;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				if (grd.GetRows().Count() > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["AccountId"].Value) == Conversion.ToInt(data.Rows[i]["AccountId"]))
						{
							Flag = true;
							break;
						}
					}
				}
				if (!Flag)
				{
					if (Conversion.ToDouble(data.Rows[i]["Closing"]) > 0.0)
					{
						table.Rows.Add(data.Rows[i]["AccountCode"], data.Rows[i]["AccountId"], data.Rows[i]["AccountTitle"], 0, "", 0, "", 0, "", "", 0, 0, Conversion.ToDouble(data.Rows[i]["Closing"]), Conversion.ToDouble(data.Rows[i]["Closing"]), data.Rows[i]["BranchesId"], data.Rows[i]["BranchName"], 0, "");
					}
					else if (Conversion.ToDouble(data.Rows[i]["Closing"]) < 0.0)
					{
						table.Rows.Add(data.Rows[i]["AccountCode"], data.Rows[i]["AccountId"], data.Rows[i]["AccountTitle"], 0, "", 0, "", 0, "", "", 0.0 - Conversion.ToDouble(data.Rows[i]["Closing"]), 0.0 - Conversion.ToDouble(data.Rows[i]["Closing"]), 0, 0, data.Rows[i]["BranchesId"], data.Rows[i]["BranchName"], 0, "");
					}
				}
				Flag = false;
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			gridsetting();
			CalculateTotalInformation();
			if (txtVoucherremarks.Text == "")
			{
				txtVoucherremarks.Text = "Adjustment";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnGenerateRemarks_Click(object sender, EventArgs e)
	{
		try
		{
			double num = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["AmountDr"], (AggregateFunction)2));
			double CreditAmount = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["AmountCr"], (AggregateFunction)2));
			if (!(num > 0.0) || !(CreditAmount > 0.0) || !(Conversion.ToString((object)txtVoucherremarks.Text) != ""))
			{
				return;
			}
			DataRow CreditSingleRow = null;
			DataRow debitSingleRow = null;
			bool IfSingleDebitRow = table.AsEnumerable().Count((DataRow row) => row.Field<double>("AmountDr") > 0.0) == 1;
			if (table.AsEnumerable().Count((DataRow row) => row.Field<double>("AmountCr") > 0.0) == 1)
			{
				CreditSingleRow = (from row in table.AsEnumerable()
					where row.Field<double>("AmountCr") > 0.0
					select row).SingleOrDefault();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToString((object)item.Cells["Remarks"].Text) == "")
					{
						item.BeginEdit();
						item.Cells["Remarks"].Value = item.Cells["AccountTitle"].Text + " " + txtVoucherremarks.Text.Trim() + " " + CreditSingleRow["AccountTitle"];
						item.EndEdit();
					}
				}
				grd.UpdateData();
			}
			else if (IfSingleDebitRow)
			{
				debitSingleRow = (from row in table.AsEnumerable()
					where row.Field<double>("AmountDr") > 0.0
					select row).SingleOrDefault();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item2 in rows)
				{
					if (Conversion.ToString((object)item2.Cells["Remarks"].Text) == "")
					{
						item2.BeginEdit();
						item2.Cells["Remarks"].Value = item2.Cells["AccountTitle"].Text + " " + txtVoucherremarks.Text.Trim() + " " + debitSingleRow["AccountTitle"];
						item2.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item3 in rows)
				{
					if (Conversion.ToString((object)item3.Cells["Remarks"].Text) == "")
					{
						item3.BeginEdit();
						item3.Cells["Remarks"].Value = txtVoucherremarks.Text.Trim();
						item3.EndEdit();
					}
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateJoblot_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbDrJobLot).ActiveRow != null)
			{
				if (grd.GetRows().Count() > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow r in rows)
					{
						r.BeginEdit();
						if (Conversion.ToInt(CmbDrJobLot.Value) > 0)
						{
							r.Cells["JobLotId"].Value = Conversion.ToInt(CmbDrJobLot.Value);
							r.Cells["JobLot"].Value = Conversion.ToString((object)((Control)(object)CmbDrJobLot).Text);
						}
						r.EndEdit();
					}
					return;
				}
				throw new Exception("No Detail Rows Found");
			}
			throw new Exception("Please Select Job Lot to update in Detail Rows");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
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
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Expected O, but got Unknown
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Expected O, but got Unknown
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Expected O, but got Unknown
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Expected O, but got Unknown
		//IL_0e47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e51: Expected O, but got Unknown
		//IL_6054: Unknown result type (might be due to invalid IL or missing references)
		//IL_605e: Expected O, but got Unknown
		//IL_6082: Unknown result type (might be due to invalid IL or missing references)
		//IL_608c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance67 = new Appearance();
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
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.JournalVoucher_New));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.DebitAccountPanel = new System.Windows.Forms.Panel();
		this.CmbAccount = new UltraCombo();
		this.lbDr = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtDebitBalance = new System.Windows.Forms.TextBox();
		this.DebitSubsidiaryPanel = new System.Windows.Forms.Panel();
		this.CmbSubsidiaryAccount = new UltraCombo();
		this.labelDrSubsidiary = new System.Windows.Forms.Label();
		this.labelSubsidiaryDebitBal = new System.Windows.Forms.Label();
		this.lblSubsidiaryaccount = new System.Windows.Forms.Label();
		this.DrCommentsPanel = new System.Windows.Forms.Panel();
		this.txtDrRmks = new System.Windows.Forms.TextBox();
		this.lblComments = new System.Windows.Forms.Label();
		this.DrJobLotPanel = new System.Windows.Forms.Panel();
		this.CmbDrJobLot = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.txtCheq = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.FcyDebitPanel = new System.Windows.Forms.Panel();
		this.txtFcyDebit = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.FcyCreditPanel = new System.Windows.Forms.Panel();
		this.txtFcyCredit = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.txtCredit = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.DrBranchPanel = new System.Windows.Forms.Panel();
		this.CmbDrBranch = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.DrCostCenterPanel = new System.Windows.Forms.Panel();
		this.CmbDrCostCenter = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.panelDetailButtons = new System.Windows.Forms.Panel();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtVoucherremarks = new System.Windows.Forms.TextBox();
		this.lblFcyAmountCr = new System.Windows.Forms.Label();
		this.txtFcyAmountCr = new System.Windows.Forms.TextBox();
		this.CmbProjectId = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtDifference = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBoxPrintPreview = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.VoucherInfoBox = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchers = new System.Windows.Forms.Label();
		this.lblTotalVouchers = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.lblApprovedVouchers = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdentapproved = new System.Windows.Forms.RadioButton();
		this.rdentmodify = new System.Windows.Forms.RadioButton();
		this.rdentdoc = new System.Windows.Forms.RadioButton();
		this.rdententry = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbApproveStatus = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.btnLoadTrialBalance = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateRemarks = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.btnUpdateJoblot = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel12.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.DebitAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).BeginInit();
		this.DebitSubsidiaryPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).BeginInit();
		this.DrCommentsPanel.SuspendLayout();
		this.DrJobLotPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrJobLot).BeginInit();
		this.panel13.SuspendLayout();
		this.FcyDebitPanel.SuspendLayout();
		this.panel14.SuspendLayout();
		this.FcyCreditPanel.SuspendLayout();
		this.panel15.SuspendLayout();
		this.DrBranchPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrBranch).BeginInit();
		this.DrCostCenterPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrCostCenter).BeginInit();
		this.panelDetailButtons.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel10.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.VoucherInfoBox.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1216, 684);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1216, 684);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1208, 658);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 256);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1204, 400);
		this.panel5.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1204, 370);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel12.Controls.Add(this.btnUpdateJoblot);
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Controls.Add(this.label4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1204, 30);
		this.panel12.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(7, 6);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 17);
		this.label4.TabIndex = 1;
		this.label4.Text = "Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1204, 199);
		this.panel4.TabIndex = 2;
		this.groupBox2.Controls.Add(this.flowLayoutPanel1);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 77);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1195, 119);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.flowLayoutPanel1.Controls.Add(this.DebitAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.DebitSubsidiaryPanel);
		this.flowLayoutPanel1.Controls.Add(this.DrCommentsPanel);
		this.flowLayoutPanel1.Controls.Add(this.DrJobLotPanel);
		this.flowLayoutPanel1.Controls.Add(this.panel13);
		this.flowLayoutPanel1.Controls.Add(this.FcyDebitPanel);
		this.flowLayoutPanel1.Controls.Add(this.panel14);
		this.flowLayoutPanel1.Controls.Add(this.FcyCreditPanel);
		this.flowLayoutPanel1.Controls.Add(this.panel15);
		this.flowLayoutPanel1.Controls.Add(this.DrBranchPanel);
		this.flowLayoutPanel1.Controls.Add(this.DrCostCenterPanel);
		this.flowLayoutPanel1.Controls.Add(this.panelDetailButtons);
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 19);
		this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(1186, 97);
		this.flowLayoutPanel1.TabIndex = 0;
		this.DebitAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccount);
		this.DebitAccountPanel.Controls.Add(this.lbDr);
		this.DebitAccountPanel.Controls.Add(this.label6);
		this.DebitAccountPanel.Controls.Add(this.txtDebitBalance);
		this.DebitAccountPanel.Location = new System.Drawing.Point(0, 0);
		this.DebitAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitAccountPanel.Name = "DebitAccountPanel";
		this.DebitAccountPanel.Size = new System.Drawing.Size(238, 47);
		this.DebitAccountPanel.TabIndex = 0;
		this.CmbAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Location = new System.Drawing.Point(2, 21);
		((System.Windows.Forms.Control)(object)this.CmbAccount).Name = "CmbAccount";
		((System.Windows.Forms.Control)(object)this.CmbAccount).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccount).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Leave += new System.EventHandler(CmbDr_Leave);
		this.lbDr.AutoSize = true;
		this.lbDr.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDr.Location = new System.Drawing.Point(91, 2);
		this.lbDr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lbDr.Name = "lbDr";
		this.lbDr.Size = new System.Drawing.Size(17, 15);
		this.lbDr.TabIndex = 38;
		this.lbDr.Text = "Bl";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 2);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 15);
		this.label6.TabIndex = 24;
		this.label6.Text = "Account Title";
		this.txtDebitBalance.BackColor = System.Drawing.Color.White;
		this.txtDebitBalance.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtDebitBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDebitBalance.Location = new System.Drawing.Point(112, 3);
		this.txtDebitBalance.Name = "txtDebitBalance";
		this.txtDebitBalance.ReadOnly = true;
		this.txtDebitBalance.Size = new System.Drawing.Size(92, 16);
		this.txtDebitBalance.TabIndex = 36;
		this.txtDebitBalance.TabStop = false;
		this.txtDebitBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.DebitSubsidiaryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount);
		this.DebitSubsidiaryPanel.Controls.Add(this.labelDrSubsidiary);
		this.DebitSubsidiaryPanel.Controls.Add(this.labelSubsidiaryDebitBal);
		this.DebitSubsidiaryPanel.Controls.Add(this.lblSubsidiaryaccount);
		this.DebitSubsidiaryPanel.Location = new System.Drawing.Point(238, 0);
		this.DebitSubsidiaryPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitSubsidiaryPanel.Name = "DebitSubsidiaryPanel";
		this.DebitSubsidiaryPanel.Size = new System.Drawing.Size(168, 47);
		this.DebitSubsidiaryPanel.TabIndex = 1;
		this.CmbSubsidiaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbSubsidiaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSubsidiaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSubsidiaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Name = "CmbSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).TabIndex = 1;
		this.CmbSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbDrSubsidiaryAccount_AfterCloseUp);
		this.CmbSubsidiaryAccount.ValueChanged += new System.EventHandler(CmbDrSubsidiaryAccount_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Leave += new System.EventHandler(CmbDrSubsidiaryAccount_Leave);
		this.labelDrSubsidiary.AutoSize = true;
		this.labelDrSubsidiary.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold);
		this.labelDrSubsidiary.Location = new System.Drawing.Point(75, 4);
		this.labelDrSubsidiary.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.labelDrSubsidiary.Name = "labelDrSubsidiary";
		this.labelDrSubsidiary.Size = new System.Drawing.Size(17, 13);
		this.labelDrSubsidiary.TabIndex = 37;
		this.labelDrSubsidiary.Text = "Bl";
		this.labelSubsidiaryDebitBal.AutoSize = true;
		this.labelSubsidiaryDebitBal.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelSubsidiaryDebitBal.Location = new System.Drawing.Point(92, 4);
		this.labelSubsidiaryDebitBal.Name = "labelSubsidiaryDebitBal";
		this.labelSubsidiaryDebitBal.Size = new System.Drawing.Size(47, 13);
		this.labelSubsidiaryDebitBal.TabIndex = 597;
		this.labelSubsidiaryDebitBal.Text = "label8";
		this.labelSubsidiaryDebitBal.Visible = false;
		this.lblSubsidiaryaccount.AutoSize = true;
		this.lblSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryaccount.Location = new System.Drawing.Point(0, 3);
		this.lblSubsidiaryaccount.Name = "lblSubsidiaryaccount";
		this.lblSubsidiaryaccount.Size = new System.Drawing.Size(62, 15);
		this.lblSubsidiaryaccount.TabIndex = 46;
		this.lblSubsidiaryaccount.Text = "Subsidiary";
		this.DrCommentsPanel.Controls.Add(this.txtDrRmks);
		this.DrCommentsPanel.Controls.Add(this.lblComments);
		this.DrCommentsPanel.Location = new System.Drawing.Point(406, 0);
		this.DrCommentsPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrCommentsPanel.Name = "DrCommentsPanel";
		this.DrCommentsPanel.Size = new System.Drawing.Size(185, 47);
		this.DrCommentsPanel.TabIndex = 2;
		this.txtDrRmks.BackColor = System.Drawing.Color.White;
		this.txtDrRmks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDrRmks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDrRmks.Location = new System.Drawing.Point(0, 22);
		this.txtDrRmks.Name = "txtDrRmks";
		this.txtDrRmks.Size = new System.Drawing.Size(185, 23);
		this.txtDrRmks.TabIndex = 2;
		this.txtDrRmks.Leave += new System.EventHandler(txtDrRmks_Leave);
		this.lblComments.AutoSize = true;
		this.lblComments.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblComments.Location = new System.Drawing.Point(1, 4);
		this.lblComments.Name = "lblComments";
		this.lblComments.Size = new System.Drawing.Size(65, 15);
		this.lblComments.TabIndex = 34;
		this.lblComments.Text = "Comments";
		this.DrJobLotPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDrJobLot);
		this.DrJobLotPanel.Controls.Add(this.label7);
		this.DrJobLotPanel.Location = new System.Drawing.Point(591, 0);
		this.DrJobLotPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrJobLotPanel.Name = "DrJobLotPanel";
		this.DrJobLotPanel.Size = new System.Drawing.Size(135, 47);
		this.DrJobLotPanel.TabIndex = 3;
		this.CmbDrJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbDrJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDrJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDrJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance75).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDrJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).Name = "CmbDrJobLot";
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(49, 15);
		this.label7.TabIndex = 50;
		this.label7.Text = "Job Lot ";
		this.panel13.Controls.Add(this.txtCheq);
		this.panel13.Controls.Add(this.label8);
		this.panel13.Location = new System.Drawing.Point(726, 0);
		this.panel13.Margin = new System.Windows.Forms.Padding(0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(127, 47);
		this.panel13.TabIndex = 4;
		this.txtCheq.BackColor = System.Drawing.Color.White;
		this.txtCheq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCheq.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCheq.Location = new System.Drawing.Point(2, 23);
		this.txtCheq.Name = "txtCheq";
		this.txtCheq.Size = new System.Drawing.Size(125, 23);
		this.txtCheq.TabIndex = 5;
		this.txtCheq.UseWaitCursor = true;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(2, 6);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 33;
		this.label8.Text = "Cheq / Ref";
		this.FcyDebitPanel.Controls.Add(this.txtFcyDebit);
		this.FcyDebitPanel.Controls.Add(this.label19);
		this.FcyDebitPanel.Location = new System.Drawing.Point(853, 0);
		this.FcyDebitPanel.Margin = new System.Windows.Forms.Padding(0);
		this.FcyDebitPanel.Name = "FcyDebitPanel";
		this.FcyDebitPanel.Size = new System.Drawing.Size(113, 47);
		this.FcyDebitPanel.TabIndex = 5;
		this.txtFcyDebit.BackColor = System.Drawing.Color.White;
		this.txtFcyDebit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyDebit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyDebit.Location = new System.Drawing.Point(1, 23);
		this.txtFcyDebit.Name = "txtFcyDebit";
		this.txtFcyDebit.ShortcutsEnabled = false;
		this.txtFcyDebit.Size = new System.Drawing.Size(110, 23);
		this.txtFcyDebit.TabIndex = 6;
		this.txtFcyDebit.Tag = "FcyAmountDebit";
		this.txtFcyDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyDebit.TextChanged += new System.EventHandler(txtFcyDebit_TextChanged);
		this.txtFcyDebit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(1, 6);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(57, 15);
		this.label19.TabIndex = 32;
		this.label19.Text = "Fcy Debit";
		this.panel14.Controls.Add(this.txtAmount);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Location = new System.Drawing.Point(966, 0);
		this.panel14.Margin = new System.Windows.Forms.Padding(0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(112, 47);
		this.panel14.TabIndex = 6;
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(1, 23);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ShortcutsEnabled = false;
		this.txtAmount.Size = new System.Drawing.Size(110, 23);
		this.txtAmount.TabIndex = 6;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.TextChanged += new System.EventHandler(txtAmount_TextChanged);
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(1, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(36, 15);
		this.label9.TabIndex = 32;
		this.label9.Text = "Debit";
		this.FcyCreditPanel.Controls.Add(this.txtFcyCredit);
		this.FcyCreditPanel.Controls.Add(this.label21);
		this.FcyCreditPanel.Location = new System.Drawing.Point(0, 47);
		this.FcyCreditPanel.Margin = new System.Windows.Forms.Padding(0);
		this.FcyCreditPanel.Name = "FcyCreditPanel";
		this.FcyCreditPanel.Size = new System.Drawing.Size(113, 47);
		this.FcyCreditPanel.TabIndex = 7;
		this.txtFcyCredit.BackColor = System.Drawing.Color.White;
		this.txtFcyCredit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyCredit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyCredit.Location = new System.Drawing.Point(1, 23);
		this.txtFcyCredit.Name = "txtFcyCredit";
		this.txtFcyCredit.ShortcutsEnabled = false;
		this.txtFcyCredit.Size = new System.Drawing.Size(110, 23);
		this.txtFcyCredit.TabIndex = 6;
		this.txtFcyCredit.Tag = "FcyAmountCredit";
		this.txtFcyCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyCredit.TextChanged += new System.EventHandler(txtFcyCredit_TextChanged);
		this.txtFcyCredit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(1, 6);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(59, 15);
		this.label21.TabIndex = 32;
		this.label21.Text = "Fcy Credit";
		this.panel15.Controls.Add(this.txtCredit);
		this.panel15.Controls.Add(this.label5);
		this.panel15.Location = new System.Drawing.Point(113, 47);
		this.panel15.Margin = new System.Windows.Forms.Padding(0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(113, 47);
		this.panel15.TabIndex = 8;
		this.txtCredit.BackColor = System.Drawing.Color.White;
		this.txtCredit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCredit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCredit.Location = new System.Drawing.Point(1, 23);
		this.txtCredit.Name = "txtCredit";
		this.txtCredit.ShortcutsEnabled = false;
		this.txtCredit.Size = new System.Drawing.Size(110, 23);
		this.txtCredit.TabIndex = 6;
		this.txtCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCredit.TextChanged += new System.EventHandler(txtCredit_TextChanged);
		this.txtCredit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(1, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(38, 15);
		this.label5.TabIndex = 32;
		this.label5.Text = "Credit";
		this.DrBranchPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDrBranch);
		this.DrBranchPanel.Controls.Add(this.label12);
		this.DrBranchPanel.Location = new System.Drawing.Point(226, 47);
		this.DrBranchPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrBranchPanel.Name = "DrBranchPanel";
		this.DrBranchPanel.Size = new System.Drawing.Size(147, 47);
		this.DrBranchPanel.TabIndex = 9;
		this.CmbDrBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbDrBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDrBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDrBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDrBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Location = new System.Drawing.Point(2, 21);
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Name = "CmbDrBranch";
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Leave += new System.EventHandler(CmbDrBranch_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 4);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(44, 15);
		this.label12.TabIndex = 52;
		this.label12.Text = "Branch";
		this.DrCostCenterPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDrCostCenter);
		this.DrCostCenterPanel.Controls.Add(this.label20);
		this.DrCostCenterPanel.Location = new System.Drawing.Point(373, 47);
		this.DrCostCenterPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrCostCenterPanel.Name = "DrCostCenterPanel";
		this.DrCostCenterPanel.Size = new System.Drawing.Size(123, 47);
		this.DrCostCenterPanel.TabIndex = 10;
		this.CmbDrCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbDrCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDrCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDrCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDrCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).Name = "CmbDrCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).TabIndex = 4;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(-1, 4);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(67, 15);
		this.label20.TabIndex = 52;
		this.label20.Text = "Cost Center";
		this.panelDetailButtons.Controls.Add(this.btnCancelDetail);
		this.panelDetailButtons.Controls.Add(this.btnplus);
		this.panelDetailButtons.Controls.Add(this.btnUpdateDetail);
		this.panelDetailButtons.Location = new System.Drawing.Point(496, 47);
		this.panelDetailButtons.Margin = new System.Windows.Forms.Padding(0);
		this.panelDetailButtons.Name = "panelDetailButtons";
		this.panelDetailButtons.Size = new System.Drawing.Size(106, 47);
		this.panelDetailButtons.TabIndex = 11;
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(56, 20);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(49, 27);
		this.btnCancelDetail.TabIndex = 2;
		this.btnCancelDetail.Text = "Clear";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(btnCancelDetail_Leave);
		this.btnCancelDetail.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(btnCancelDetail_PreviewKeyDown);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(2, 21);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(30, 24);
		this.btnplus.TabIndex = 0;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(2, 20);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(56, 27);
		this.btnUpdateDetail.TabIndex = 1;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.groupBox1.Controls.Add(this.txtVoucherremarks);
		this.groupBox1.Controls.Add(this.lblFcyAmountCr);
		this.groupBox1.Controls.Add(this.txtFcyAmountCr);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProjectId);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.lblFcyAmount);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.lblExchangeRate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.lblFcyCode);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1195, 72);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtVoucherremarks.BackColor = System.Drawing.Color.White;
		this.txtVoucherremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoucherremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVoucherremarks.Location = new System.Drawing.Point(305, 17);
		this.txtVoucherremarks.Multiline = true;
		this.txtVoucherremarks.Name = "txtVoucherremarks";
		this.txtVoucherremarks.Size = new System.Drawing.Size(437, 46);
		this.txtVoucherremarks.TabIndex = 2;
		this.txtVoucherremarks.Leave += new System.EventHandler(txtDrRmks_Leave);
		this.lblFcyAmountCr.AutoSize = true;
		this.lblFcyAmountCr.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFcyAmountCr.Location = new System.Drawing.Point(946, 45);
		this.lblFcyAmountCr.Name = "lblFcyAmountCr";
		this.lblFcyAmountCr.Size = new System.Drawing.Size(86, 15);
		this.lblFcyAmountCr.TabIndex = 568;
		this.lblFcyAmountCr.Text = "Fcy Amount Cr";
		this.txtFcyAmountCr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountCr.Enabled = false;
		this.txtFcyAmountCr.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmountCr.Location = new System.Drawing.Point(1034, 41);
		this.txtFcyAmountCr.Name = "txtFcyAmountCr";
		this.txtFcyAmountCr.Size = new System.Drawing.Size(117, 23);
		this.txtFcyAmountCr.TabIndex = 567;
		this.txtFcyAmountCr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbProjectId.AllowNull = (DefaultableBoolean)1;
		this.CmbProjectId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProjectId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProjectId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProjectId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Location = new System.Drawing.Point(74, 14);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Name = "CmbProjectId";
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Size = new System.Drawing.Size(167, 24);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabStop = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(6, 19);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 15);
		this.label10.TabIndex = 566;
		this.label10.Text = "Cost Center";
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFcyAmount.Location = new System.Drawing.Point(946, 19);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(88, 15);
		this.lblFcyAmount.TabIndex = 46;
		this.lblFcyAmount.Text = "Fcy Amount Dr";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(1034, 15);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(117, 23);
		this.txtFcyAmount.TabIndex = 5;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(832, 42);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(111, 23);
		this.txtExchangeRate.TabIndex = 4;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.lblExchangeRate.AutoSize = true;
		this.lblExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblExchangeRate.Location = new System.Drawing.Point(745, 46);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(84, 15);
		this.lblExchangeRate.TabIndex = 44;
		this.lblExchangeRate.Text = "Exchange Rate";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(832, 14);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(745, 20);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 41;
		this.lblFcyCode.Text = "Fcy Code";
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(246, 17);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(56, 38);
		this.label1.TabIndex = 12;
		this.label1.Text = "Voucher Remarks";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(125, 40);
		this.DocDate.Margin = new System.Windows.Forms.Padding(2);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(116, 23);
		this.DocDate.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(74, 40);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(50, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(6, 39);
		this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 32);
		this.label13.TabIndex = 0;
		this.label13.Text = "Doc No && Date";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.chkCustomAccounts);
		this.panel3.Controls.Add(this.labelLocationType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.txtDifference);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBoxPrintPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 29);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1204, 28);
		this.panel3.TabIndex = 1;
		this.chkCustomAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(986, 5);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(120, 19);
		this.chkCustomAccounts.TabIndex = 511;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(159, 8);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 510;
		this.labelLocationType.Text = "Location Type";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(248, 2);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 509;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(526, 5);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(68, 16);
		this.label11.TabIndex = 52;
		this.label11.Text = "Difference";
		this.txtDifference.Location = new System.Drawing.Point(595, 4);
		this.txtDifference.Name = "txtDifference";
		this.txtDifference.ReadOnly = true;
		this.txtDifference.Size = new System.Drawing.Size(152, 20);
		this.txtDifference.TabIndex = 51;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(140, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Journal Voucher";
		this.ChkBoxPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintPreview.AutoSize = true;
		this.ChkBoxPrintPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkBoxPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintPreview.Location = new System.Drawing.Point(1110, 5);
		this.ChkBoxPrintPreview.Name = "ChkBoxPrintPreview";
		this.ChkBoxPrintPreview.Size = new System.Drawing.Size(95, 19);
		this.ChkBoxPrintPreview.TabIndex = 6;
		this.ChkBoxPrintPreview.Text = "Print Preview";
		this.ChkBoxPrintPreview.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1204, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.New, this.btnRefresh, this.Save, this.btnUpdate, this.btnDelete, this.btnAttachment, this.Print, this.btnShortCutKey, this.btnGenerateRemarks, this.btnLoadTrialBalance });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1204, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(59, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(New_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(64, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "102-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.toolStrip3);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1208, 658);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 379);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1204, 277);
		this.panel9.TabIndex = 50;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 26);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1204, 251);
		this.panel11.TabIndex = 1;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1204, 251);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 46;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label2);
		this.panel10.Controls.Add(this.ctrlGrdBar3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1204, 26);
		this.panel10.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(242, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "Detail of above selected row";
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 135);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1204, 244);
		this.panel8.TabIndex = 49;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1204, 244);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 2;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick_1);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel7.Controls.Add(this.VoucherInfoBox);
		this.panel7.Controls.Add(this.groupBox3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 57);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1204, 78);
		this.panel7.TabIndex = 48;
		this.VoucherInfoBox.Controls.Add(this.label36);
		this.VoucherInfoBox.Controls.Add(this.lblUnApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.lblTotalVouchers);
		this.VoucherInfoBox.Controls.Add(this.label38);
		this.VoucherInfoBox.Controls.Add(this.lblApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.label37);
		this.VoucherInfoBox.Location = new System.Drawing.Point(933, 5);
		this.VoucherInfoBox.Name = "VoucherInfoBox";
		this.VoucherInfoBox.Size = new System.Drawing.Size(242, 69);
		this.VoucherInfoBox.TabIndex = 9;
		this.VoucherInfoBox.TabStop = false;
		this.VoucherInfoBox.Visible = false;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(6, 11);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(96, 17);
		this.label36.TabIndex = 1;
		this.label36.Text = "Total Vouchers";
		this.lblUnApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUnApprovedVouchers.Location = new System.Drawing.Point(153, 47);
		this.lblUnApprovedVouchers.Name = "lblUnApprovedVouchers";
		this.lblUnApprovedVouchers.Size = new System.Drawing.Size(80, 17);
		this.lblUnApprovedVouchers.TabIndex = 6;
		this.lblUnApprovedVouchers.Text = "0";
		this.lblUnApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalVouchers.Location = new System.Drawing.Point(153, 11);
		this.lblTotalVouchers.Name = "lblTotalVouchers";
		this.lblTotalVouchers.Size = new System.Drawing.Size(80, 17);
		this.lblTotalVouchers.TabIndex = 4;
		this.lblTotalVouchers.Text = "0";
		this.lblTotalVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(6, 47);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(144, 17);
		this.label38.TabIndex = 3;
		this.label38.Text = "UnApproved Vouchers";
		this.lblApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedVouchers.Location = new System.Drawing.Point(153, 28);
		this.lblApprovedVouchers.Name = "lblApprovedVouchers";
		this.lblApprovedVouchers.Size = new System.Drawing.Size(80, 17);
		this.lblApprovedVouchers.TabIndex = 5;
		this.lblApprovedVouchers.Text = "0";
		this.lblApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(6, 28);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(127, 17);
		this.label37.TabIndex = 2;
		this.label37.Text = "Approved Vouchers";
		this.groupBox3.Controls.Add(this.rdentapproved);
		this.groupBox3.Controls.Add(this.rdentmodify);
		this.groupBox3.Controls.Add(this.rdentdoc);
		this.groupBox3.Controls.Add(this.rdententry);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatus);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(926, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdentapproved.AutoSize = true;
		this.rdentapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentapproved.Location = new System.Drawing.Point(816, 48);
		this.rdentapproved.Name = "rdentapproved";
		this.rdentapproved.Size = new System.Drawing.Size(105, 19);
		this.rdentapproved.TabIndex = 8;
		this.rdentapproved.Text = "Approved Date";
		this.rdentapproved.UseVisualStyleBackColor = true;
		this.rdentmodify.AutoSize = true;
		this.rdentmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentmodify.Location = new System.Drawing.Point(816, 28);
		this.rdentmodify.Name = "rdentmodify";
		this.rdentmodify.Size = new System.Drawing.Size(91, 19);
		this.rdentmodify.TabIndex = 7;
		this.rdentmodify.Text = "Modify Date";
		this.rdentmodify.UseVisualStyleBackColor = true;
		this.rdentdoc.AutoSize = true;
		this.rdentdoc.Checked = true;
		this.rdentdoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentdoc.Location = new System.Drawing.Point(735, 28);
		this.rdentdoc.Name = "rdentdoc";
		this.rdentdoc.Size = new System.Drawing.Size(75, 19);
		this.rdentdoc.TabIndex = 5;
		this.rdentdoc.TabStop = true;
		this.rdentdoc.Text = "Doc Date";
		this.rdentdoc.UseVisualStyleBackColor = true;
		this.rdententry.AutoSize = true;
		this.rdententry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdententry.Location = new System.Drawing.Point(735, 48);
		this.rdententry.Name = "rdententry";
		this.rdententry.Size = new System.Drawing.Size(80, 19);
		this.rdententry.TabIndex = 6;
		this.rdententry.Text = "Entry Date";
		this.rdententry.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(673, 39);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(501, 22);
		this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(95, 15);
		this.label18.TabIndex = 142;
		this.label18.Text = "Approved Status";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(393, 22);
		this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(64, 15);
		this.label17.TabIndex = 141;
		this.label17.Text = "To Doc No";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(288, 22);
		this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(79, 15);
		this.label16.TabIndex = 140;
		this.label16.Text = "From Doc No";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(146, 22);
		this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(48, 15);
		this.label15.TabIndex = 139;
		this.label15.Text = "To Date";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(5, 23);
		this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(63, 15);
		this.label14.TabIndex = 13;
		this.label14.Text = "From Date";
		this.cmbApproveStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApproveStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Location = new System.Drawing.Point(499, 39);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Name = "cmbApproveStatus";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(393, 41);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(288, 41);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(146, 41);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 41);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(135, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 29);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1204, 28);
		this.panel6.TabIndex = 47;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(203, 19);
		this.label3.TabIndex = 22;
		this.label3.Text = "Journal Voucher History";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNewHistory });
		this.toolStrip3.Location = new System.Drawing.Point(2, 2);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1204, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnLoadTrialBalance.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoadTrialBalance.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadTrialBalance.Name = "btnLoadTrialBalance";
		this.btnLoadTrialBalance.Size = new System.Drawing.Size(150, 24);
		this.btnLoadTrialBalance.Text = "Load Trial Balance";
		this.btnLoadTrialBalance.Visible = false;
		this.btnLoadTrialBalance.Click += new System.EventHandler(btnLoadTrialBalance_Click);
		this.btnGenerateRemarks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnGenerateRemarks.Image = (System.Drawing.Image)resources.GetObject("btnGenerateRemarks.Image");
		this.btnGenerateRemarks.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateRemarks.Name = "btnGenerateRemarks";
		this.btnGenerateRemarks.Size = new System.Drawing.Size(130, 24);
		this.btnGenerateRemarks.Text = "Generate Remarks";
		this.btnGenerateRemarks.ToolTipText = "Generate Remarks";
		this.btnGenerateRemarks.Visible = false;
		this.btnGenerateRemarks.Click += new System.EventHandler(btnGenerateRemarks_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1163, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1165, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1164, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.btnUpdateJoblot.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUpdateJoblot.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnUpdateJoblot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateJoblot.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateJoblot.ForeColor = System.Drawing.Color.White;
		this.btnUpdateJoblot.Location = new System.Drawing.Point(944, 3);
		this.btnUpdateJoblot.Name = "btnUpdateJoblot";
		this.btnUpdateJoblot.Size = new System.Drawing.Size(216, 24);
		this.btnUpdateJoblot.TabIndex = 64;
		this.btnUpdateJoblot.TabStop = false;
		this.btnUpdateJoblot.Text = "Update Job Lot to Detail Rows";
		this.btnUpdateJoblot.UseVisualStyleBackColor = false;
		this.btnUpdateJoblot.Click += new System.EventHandler(btnUpdateJoblot_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1216, 684);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "JournalVoucher_New";
		this.Text = "Journal Voucher";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(VoucherEntry_FormClosing);
		base.Load += new System.EventHandler(DayBookVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DayBookVoucher_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		this.DebitAccountPanel.ResumeLayout(false);
		this.DebitAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).EndInit();
		this.DebitSubsidiaryPanel.ResumeLayout(false);
		this.DebitSubsidiaryPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).EndInit();
		this.DrCommentsPanel.ResumeLayout(false);
		this.DrCommentsPanel.PerformLayout();
		this.DrJobLotPanel.ResumeLayout(false);
		this.DrJobLotPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrJobLot).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.FcyDebitPanel.ResumeLayout(false);
		this.FcyDebitPanel.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.FcyCreditPanel.ResumeLayout(false);
		this.FcyCreditPanel.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.DrBranchPanel.ResumeLayout(false);
		this.DrBranchPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrBranch).EndInit();
		this.DrCostCenterPanel.ResumeLayout(false);
		this.DrCostCenterPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrCostCenter).EndInit();
		this.panelDetailButtons.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.VoucherInfoBox.ResumeLayout(false);
		this.VoucherInfoBox.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
