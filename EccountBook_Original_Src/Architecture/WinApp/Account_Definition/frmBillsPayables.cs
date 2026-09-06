using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class frmBillsPayables : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtDetail = new DataTable();

	private DataTable dtSchedule = new DataTable();

	private DataTable AllAccountsWithTypeId = new DataTable();

	private DataTable dtSubsidory = new DataTable();

	private int RecId;

	private bool UpdateMode;

	private int updateDetailIndex = -1;

	private int SubsdiaryAccountDatasourceLengthDetail;

	private int SubsdiaryAccountDatasourceLengthCr;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int DocumentTypeId = 6;

	private bool SubsidiaryAccountAllownOnVouchers;

	private int BaseCurrency;

	private double BaseRate;

	private bool FormInitialized;

	private string ScreenName = "frmBillsPayables";

	private int DefaultDaysToLessFromHistoryFromDate;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private GroupBox groupBox1;

	internal UltraCombo cmbAccount;

	private Label label7;

	private DateTimePicker txtDocDate;

	private TextBox txtDocNo;

	private DateTimePicker txtInvoiceDate;

	private Label label4;

	private Label label8;

	private GroupBox groupBox2;

	internal UltraCombo cmbJobLotFile;

	private Label label10;

	internal UltraCombo cmbAccountDetail;

	private Label label9;

	private Label label2;

	private Label label14;

	private TextBox txtRateDetail;

	private Label label12;

	private TextBox txtQtyDetial;

	private Label label11;

	private Label label15;

	private TextBox txtAmountDetail;

	internal GridEX grd;

	internal GridEX grdSchedule;

	internal UltraCombo CmbTaxAccount;

	private Label label17;

	private TextBox txtTaxAmount;

	private TextBox txtTaxPercent;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Button btnAdd;

	private TextBox txtInvoiceNo;

	private Panel panel9;

	private Panel panel11;

	internal GridEX DataGridHistory;

	private Panel panel12;

	private Panel panel13;

	internal GridEX grdDetailHistory;

	private Label label16;

	private ToolStripButton btnDelete;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnSaveAs;

	private Label lblbalancedetail;

	private Label lbldetaildrcr;

	private Label label19;

	private TextBox txtInvoiceNoDetail;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar3;

	private Label lblDrCrS;

	private CheckBox CHKPREVIEW;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label20;

	private Panel panel16;

	private Label label3;

	private ToolStripButton btnShortCutKey;

	internal UltraCombo CmbSubsidiaryAccountCr;

	private Label lblSubsidiaryaccountCr;

	internal UltraCombo CmbProjectId;

	private Label label13;

	internal UltraCombo CmbSubsidiaryAccount;

	private Label lblSubsidiaryaccount;

	private TextBox txtRemarksDetail;

	private TextBox txtRemarks;

	private Panel panel19;

	private Panel panel8;

	private Panel panel20;

	private Label label21;

	private TextBox txtVoucherAmountFooter;

	private Label label22;

	private TextBox txtTotalAmount;

	private Label label23;

	internal UltraCombo CmbTaxType;

	private Label label1;

	private Label label5;

	private TextBox txtExchangeRate;

	private Label lblExchangeRate;

	private Label lblFcyAmount;

	private TextBox txtFcyAmount;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

	private TextBox lblCreditAcBalance;

	private Label label6;

	private CheckBox chkCustomAccounts;

	private Label labelLocationType;

	internal UltraCombo cmbLocationType;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel PanelCustomGroupDetail;

	private Panel PanelAccountTitleDetail;

	private Panel PanelSubsidiaryDetail;

	private Panel PanelJobLotDetail;

	private Panel panel6;

	private Panel PaanelInvoiceNoDetail;

	private Panel PanelQty;

	private Panel PanelRateDetail;

	private Panel PanelAmountDetail;

	private Panel PanelFcyAmountDetail;

	private TextBox txtFcyAmountDetail;

	private Label label18;

	private Panel PanelRemarksDetail;

	private Panel PanelAddUpdateBtn;

	internal UltraCombo CmbCustomGroup;

	private Label label24;

	internal UltraCombo CmbAdvanceTaxAccount;

	private Label label25;

	private TextBox txtAdvanceTaxAmount;

	private Label label26;

	private TextBox txtOtherChargesAmount;

	private Label labelNoteTitleDetailAccount;

	internal UltraCombo CmbOtherChargesAccount;

	private Label label27;

	private Panel ReferenceAccountPanel;

	internal UltraCombo CmbReferenceAccount;

	private Label label46;

	private Label label29;

	private GroupBox VoucherInfoBox;

	private Label label36;

	private Label lblUnApprovedVouchers;

	private Label lblTotalVouchers;

	private Label label38;

	private Label lblApprovedVouchers;

	private Label label37;

	private GroupBox groupBox3;

	private RadioButton rdexpapproved;

	private RadioButton rdexpmodify;

	private RadioButton rdexpdocdate;

	private RadioButton rdexpentry;

	private Label label28;

	internal UltraCombo CmbAccountTitleHistory;

	private Button btnShowHistory;

	private Label label30;

	private Label label31;

	private Label label32;

	private Label label33;

	private Label label34;

	internal UltraCombo cmbApproveStatus;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnLoadAll;

	private Label label39;

	private Label label35;

	private Label label40;

	private ToolStripButton BtnLookup;

	internal UltraCombo CmbAccountType;

	private Label label41;

	private ToolStripButton BtnPrintII;

	public frmBillsPayables(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		InitializeComponentCustom();
		InitializeComponentMethod();
		UserAccount = user;
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1040, 750);
			ScreenName = Conversion.ToString((object)base.Name);
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtDetail.Columns.Add("AccountCode");
			dtDetail.Columns.Add("AccId");
			dtDetail.Columns.Add("Account");
			dtDetail.Columns.Add("SubsidiaryAccountId");
			dtDetail.Columns.Add("SubsidiaryAccount");
			dtDetail.Columns.Add("SubsidiaryAccountTypeId");
			dtDetail.Columns.Add("JobLotId");
			dtDetail.Columns.Add("JobLot");
			dtDetail.Columns.Add("InvoiceNo");
			dtDetail.Columns.Add("Qty", typeof(double));
			dtDetail.Columns.Add("Rate", typeof(double));
			dtDetail.Columns.Add("Amount", typeof(double));
			dtDetail.Columns.Add("FcyAmount", typeof(double));
			dtDetail.Columns.Add("ReferenceAccountId", typeof(int));
			dtDetail.Columns.Add("ReferenceAccount", typeof(string));
			dtDetail.Columns.Add("Remarks");
			dtSchedule.Columns.Add("DueDays");
			dtSchedule.Columns.Add("DueDate");
			dtSchedule.Columns.Add("Due%", typeof(double));
			dtSchedule.Columns.Add("Amount", typeof(double));
			btnsave.Visible = true;
			btnSaveAs.Visible = false;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			txtDocDate.Focus();
			CHKPREVIEW.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			string documentNoData = DocumentNoDBCall();
			DataTable locationType = VoucherHead.GetLocationType();
			DataTable costCenterData = CommonServices.ProjectServiceBind();
			AllAccountsWithTypeIdDbCall();
			DataTable currencyData = CurrencyDbCall();
			DataTable customAccountGroupsByDocumentType = AccountsAllocateToCustomAccountGroupByDocumentType.GetCustomAccountGroupsByDocumentType(DocumentTypeId, (string)null);
			DataTable jobLotData = CommonServices.JobLotGetAllService();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				AllSubsidoryAccountDbCall();
			}
			DataTable taxTypeData = TaxTypesDbCall();
			DataTable accountTitleHistoryData = AccountTitleHistoryDbCall();
			return new
			{
				documentNoData = documentNoData,
				LocationTypeData = locationType,
				CostCenterData = costCenterData,
				CurrencyData = currencyData,
				CustomGroupData = customAccountGroupsByDocumentType,
				JobLotData = jobLotData,
				TaxTypeData = taxTypeData,
				AccountTitleHistoryData = accountTitleHistoryData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnSaveAs.Enabled = formright.DoHaveSaveRight;
				Print.Enabled = formright.DoHavePrintRights;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				if (SubsidiaryAccountAllownOnVouchers)
				{
					PanelSubsidiaryDetail.Visible = true;
					((Control)(object)CmbSubsidiaryAccountCr).Visible = true;
					lblSubsidiaryaccountCr.Visible = true;
					PanelAccountTitleDetail.Width = 272;
					((Control)(object)cmbAccountDetail).Width = 270;
				}
				else
				{
					PanelSubsidiaryDetail.Visible = false;
					((Control)(object)CmbSubsidiaryAccountCr).Visible = false;
					lblSubsidiaryaccountCr.Visible = false;
					PanelAccountTitleDetail.Width = 487;
					((Control)(object)cmbAccountDetail).Width = 485;
				}
				lblbalancedetail.Visible = false;
				lbldetaildrcr.Visible = false;
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				LocationTypeBind(result.LocationTypeData);
				CostCenterBind(result.CostCenterData);
				AccountTypeBind();
				CreditAccountAccountBind();
				SalesTaxAccountBind();
				CurrencyBind(result.CurrencyData);
				CustomGroupBind(result.CustomGroupData);
				int num = Conversion.ToInt(CmbCustomGroup.Value);
				if (num > 0)
				{
					DetailAccountsBind(AccountsAllocateToCustomAccountGroupByDocumentType.GetCustomAccountsByGroup(UserAccount.CompanyId, num));
				}
				JobLotBind(result.JobLotData);
				ReferenceAccountBind();
				TaxTypesBind(result.TaxTypeData);
				AdvanceTaxAccountBind();
				DefaultConfigurations();
				if (dtDetail != null && dtDetail.Columns.Count > 0)
				{
					grd.DataSource = dtDetail;
					grd.RetrieveStructure();
					DetailGridSetting();
				}
				if (dtSchedule != null && dtSchedule.Columns.Count > 0)
				{
					AddRowInScheduleGrid();
					grdSchedule.RetrieveStructure();
					ScheduleGridSetting();
				}
				StatusFillForHistory();
				ComboBindForHistory(result.AccountTitleHistoryData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				txtDocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void DefaultConfigurations()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				cmbJobLotFile.Value = JobId;
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
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj != null) ? obj.ConfigKey : null));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDBCall()
	{
		try
		{
			return Conversion.ToString((object)CommonServices.GenerateVoucherCode(DocumentTypeId));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtDocNo.Text = documentData;
	}

	private void LocationTypeBind(DataTable dt)
	{
		try
		{
			int Id = Conversion.ToInt(cmbLocationType.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbLocationType, "Id", "Location", "Location Type", ZeroIndex: true);
				((UltraGridBase)cmbLocationType).Rows[1].Activate();
				CommonServices.SetComboValue(cmbLocationType, "Id", Id, dt, ActivateRow: true, 1);
			}
			else
			{
				((Control)(object)cmbLocationType).Text = string.Empty;
				((UltraGridBase)cmbLocationType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CostCenterBind(DataTable dtproject)
	{
		try
		{
			int Id = Conversion.ToInt(CmbProjectId.Value);
			if (dtproject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtproject, CmbProjectId, "Id", "ProjectName", "Cost Center", false);
				((UltraGridBase)CmbProjectId).DisplayLayout.Bands[0].Columns[1].Width = 215;
				((UltraGridBase)CmbProjectId).Rows[0].Activate();
				CommonServices.SetComboValue(CmbProjectId, "Id", Id, dtproject, ActivateRow: true);
			}
			else
			{
				((Control)(object)CmbProjectId).Text = string.Empty;
				((UltraGridBase)CmbProjectId).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllAccountsWithTypeIdDbCall()
	{
		try
		{
			AllAccountsWithTypeId.Clear();
			AllAccountsWithTypeId = CommonServices.Accounts_GetAccountTitleByAccountTypeIds();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable GetAccountByTypeIdFromAllAccountsWithTypeId(string accountTypeIds)
	{
		try
		{
			int result;
			HashSet<int> accountTypeIdSet = new HashSet<int>((from id in accountTypeIds.Split(',')
				select id.Trim() into id
				where int.TryParse(id, out result)
				select id).Select(int.Parse));
			DataTable dt = new DataTable();
			dt.Columns.AddRange(new DataColumn[5]
			{
				new DataColumn("Id", typeof(int)),
				new DataColumn("AccountTitle", typeof(string)),
				new DataColumn("AccountCode", typeof(string)),
				new DataColumn("AccountTypeId", typeof(int)),
				new DataColumn("NoteTitle", typeof(string))
			});
			foreach (DataRow dr in AllAccountsWithTypeId.Rows)
			{
				int accountTypeId = Conversion.ToInt(dr["AccountTypeId"]);
				if (accountTypeIdSet.Contains(accountTypeId))
				{
					dt.Rows.Add(dr["Id"], dr["AccountTitle"], dr["AccountCode"], accountTypeId, dr["NoteTitle"]);
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception("Error while filtering accounts by type ID: " + ex.Message, ex);
		}
	}

	private void CreditAccountAccountBind()
	{
		try
		{
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("3");
			InfragisticsHelper.BindAndRetainSelection(cmbAccount, dt, "Id", "AccountTitle", "Credit Account", AllColumns: true, Conversion.ToInt(cmbAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SalesTaxAccountBind()
	{
		try
		{
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("3,6,8,19");
			InfragisticsHelper.BindAndRetainSelection(CmbTaxAccount, dt, "Id", "AccountTitle", "Sales Tax Account", AllColumns: true, Conversion.ToInt(CmbTaxAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbTaxAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbTaxAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AdvanceTaxAccountBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbAdvanceTaxAccount.Value);
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("6,16,17");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbAdvanceTaxAccount, "Id", "AccountTitle", "Advance Tax Account", ZeroIndex: true);
				((UltraGridBase)CmbAdvanceTaxAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbAdvanceTaxAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				CommonServices.SetComboValue(CmbAdvanceTaxAccount, "Id", Id, dt);
			}
			else
			{
				((Control)(object)CmbAdvanceTaxAccount).Text = string.Empty;
				((UltraGridBase)CmbAdvanceTaxAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomGroupBind(DataTable dt)
	{
		try
		{
			int Id = Conversion.ToInt(CmbCustomGroup.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCustomGroup, "Id", "CustomGroupName", "Custom Group", false);
				((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns[1].Width = 350;
				CommonServices.SetComboValue(CmbCustomGroup, "Id", Id, dt, ActivateRow: true);
			}
			else
			{
				((Control)(object)CmbCustomGroup).Text = string.Empty;
				((UltraGridBase)CmbCustomGroup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailAccountsBind(DataTable dtFromDb)
	{
		try
		{
			int Id = Conversion.ToInt(cmbAccountDetail.Value);
			if (dtFromDb.Rows.Count > 0)
			{
				DDL.BindDDL(dtFromDb, cmbAccountDetail, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
				((UltraGridBase)cmbAccountDetail).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbAccountDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbAccountDetail).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				CommonServices.SetComboValue(cmbAccountDetail, "Id", Id, dtFromDb);
				DDL.BindDDL(dtFromDb, CmbOtherChargesAccount, "Id", "AccountTitle", "Other Charges", ZeroIndex: true);
				((UltraGridBase)CmbOtherChargesAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbOtherChargesAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbOtherChargesAccount).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				CommonServices.SetComboValue(CmbOtherChargesAccount, "Id", Id, dtFromDb);
			}
			else
			{
				((Control)(object)cmbAccountDetail).Text = string.Empty;
				((UltraGridBase)cmbAccountDetail).DataSource = null;
				((Control)(object)CmbOtherChargesAccount).Text = string.Empty;
				((UltraGridBase)CmbOtherChargesAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void JobLotBind(DataTable dtJobLot)
	{
		try
		{
			int joblot = Conversion.ToInt(cmbJobLotFile.Value);
			if (dtJobLot.Rows.Count > 0)
			{
				DDL.BindDDL(dtJobLot, cmbJobLotFile, "Id", "JobLotDescription", "Job Lot", ZeroIndex: true);
				CommonServices.SetComboValue(cmbJobLotFile, "Id", joblot, dtJobLot);
			}
			else
			{
				((Control)(object)cmbJobLotFile).Text = string.Empty;
				((UltraGridBase)cmbJobLotFile).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReferenceAccountBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbReferenceAccount.Value);
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("3, 5, 6, 7, 8");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbReferenceAccount, "Id", "AccountTitle", "Reference Account", ZeroIndex: true);
				((UltraGridBase)CmbReferenceAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbReferenceAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				CommonServices.SetComboValue(CmbReferenceAccount, "Id", Id, dt);
			}
			else
			{
				((Control)(object)CmbReferenceAccount).Text = string.Empty;
				((UltraGridBase)CmbReferenceAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable TaxTypesDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			return TaxesTypes.GetForComboBind(new TaxesTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Type = 2
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TaxTypesBind(DataTable dt)
	{
		try
		{
			int Id = Conversion.ToInt(CmbTaxType.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTaxType, "Id", "TaxName", "TaxType", ZeroIndex: false);
				CommonServices.SetComboValue(CmbTaxType, "Id", Id, dt);
			}
			else
			{
				((Control)(object)CmbTaxType).Text = string.Empty;
				((UltraGridBase)CmbTaxType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllSubsidoryAccountDbCall()
	{
		try
		{
			dtSubsidory.Rows.Clear();
			dtSubsidory = VoucherHead.BindSubsidiaryAccount(UserAccount.CompanyId, 0, (string)null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSubsidiaryAccountDrWithDbCall()
	{
		try
		{
			if (dtSubsidory.Rows.Count > 0)
			{
				if (Conversion.ToInt(cmbAccountDetail.Value) != 0)
				{
					if (Conversion.ToInt(((UltraDropDownBase)cmbAccountDetail).SelectedRow.Cells[3].Value) != 2 && Conversion.ToInt(((UltraDropDownBase)cmbAccountDetail).SelectedRow.Cells[3].Value) != 15 && SubsidiaryAccountAllownOnVouchers)
					{
						PanelSubsidiaryDetail.Visible = true;
						PanelAccountTitleDetail.Width = 272;
						((Control)(object)cmbAccountDetail).Width = 270;
					}
					else
					{
						PanelSubsidiaryDetail.Visible = false;
						PanelAccountTitleDetail.Width = 487;
						((Control)(object)cmbAccountDetail).Width = 485;
					}
					int selectedAcId = Conversion.ToInt(cmbAccountDetail.Value);
					if (selectedAcId != 0)
					{
						DataTable matchingTable = dtSubsidory.Clone();
						foreach (DataRow row in dtSubsidory.Rows)
						{
							if (Conversion.ToInt(row["AccountId"]) == selectedAcId)
							{
								matchingTable.ImportRow(row);
							}
						}
						int SubsidiaryIdInCaseOfUpdateDetail = 0;
						if (((Control)(object)CmbSubsidiaryAccount).Text != string.Empty)
						{
							SubsidiaryIdInCaseOfUpdateDetail = Conversion.ToInt(CmbSubsidiaryAccount.Value);
						}
						if (matchingTable.Rows.Count > 0)
						{
							SubsdiaryAccountDatasourceLengthDetail = matchingTable.Rows.Count;
							DDL.BindDDL(matchingTable, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
							((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
							((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
							CommonServices.SetComboValue(CmbSubsidiaryAccount, "Id", SubsidiaryIdInCaseOfUpdateDetail, matchingTable);
						}
						else
						{
							((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
							((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
							SubsdiaryAccountDatasourceLengthDetail = 0;
						}
					}
					else
					{
						((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
						((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
						SubsdiaryAccountDatasourceLengthDetail = 0;
					}
				}
				else
				{
					SubsdiaryAccountDatasourceLengthDetail = dtSubsidory.Rows.Count;
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
				SubsdiaryAccountDatasourceLengthDetail = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSubsidiaryAccountCrWithDbCall()
	{
		try
		{
			if (dtSubsidory.Rows.Count > 0)
			{
				if (Conversion.ToInt(cmbAccount.Value) != 0)
				{
					int selectedAcId = Conversion.ToInt(cmbAccount.Value);
					if (selectedAcId != 0)
					{
						DataTable matchingTable = dtSubsidory.Clone();
						foreach (DataRow row in dtSubsidory.Rows)
						{
							if (Conversion.ToInt(row["AccountId"]) == selectedAcId)
							{
								matchingTable.ImportRow(row);
							}
						}
						int SubsidiaryIdInCaseOfUpdateDetail = 0;
						if (((Control)(object)CmbSubsidiaryAccountCr).Text != string.Empty)
						{
							SubsidiaryIdInCaseOfUpdateDetail = Conversion.ToInt(CmbSubsidiaryAccountCr.Value);
						}
						if (matchingTable.Rows.Count > 0)
						{
							SubsdiaryAccountDatasourceLengthCr = matchingTable.Rows.Count;
							DDL.BindDDL(matchingTable, CmbSubsidiaryAccountCr, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
							((UltraGridBase)CmbSubsidiaryAccountCr).DisplayLayout.Bands[0].Columns[2].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccountCr).DisplayLayout.Bands[0].Columns[3].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccountCr).DisplayLayout.Bands[0].Columns[4].Hidden = true;
							((Control)(object)CmbSubsidiaryAccountCr).Text = string.Empty;
							CommonServices.SetComboValue(CmbSubsidiaryAccountCr, "Id", SubsidiaryIdInCaseOfUpdateDetail, matchingTable);
						}
						else
						{
							((UltraGridBase)CmbSubsidiaryAccountCr).DataSource = null;
							((Control)(object)CmbSubsidiaryAccountCr).Text = string.Empty;
							SubsdiaryAccountDatasourceLengthCr = 0;
						}
					}
					else
					{
						((UltraGridBase)CmbSubsidiaryAccountCr).DataSource = null;
						((Control)(object)CmbSubsidiaryAccountCr).Text = string.Empty;
						SubsdiaryAccountDatasourceLengthCr = 0;
					}
				}
				else
				{
					SubsdiaryAccountDatasourceLengthCr = dtSubsidory.Rows.Count;
					DDL.BindDDL(dtSubsidory, CmbSubsidiaryAccountCr, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccountCr).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbSubsidiaryAccountCr).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)CmbSubsidiaryAccountCr).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				}
			}
			else
			{
				((UltraGridBase)CmbSubsidiaryAccountCr).DataSource = null;
				((Control)(object)CmbSubsidiaryAccountCr).Text = string.Empty;
				SubsdiaryAccountDatasourceLengthCr = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbAccountDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			string Note = "";
			if (((UltraGridBase)cmbAccountDetail).ActiveRow != null && Conversion.ToInt(cmbAccountDetail.Value) > 0)
			{
				lblbalancedetail.Visible = true;
				lbldetaildrcr.Visible = true;
				DetailAccountCurrentBalance();
				Note = Conversion.ToString(((UltraDropDownBase)cmbAccountDetail).SelectedRow.Cells["NoteTitle"].Value);
			}
			else
			{
				lblbalancedetail.Visible = false;
				lbldetaildrcr.Visible = false;
				lblbalancedetail.Text = "0";
			}
			label29.Text = Note;
			BindSubsidiaryAccountDrWithDbCall();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && Conversion.ToInt(CmbSubsidiaryAccount.Value) != 0)
			{
				int GLOfSubSidory = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					cmbAccountDetail.Value = GLOfSubSidory;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccountDr_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbSubsidiaryAccountCr).ActiveRow != null && Conversion.ToInt(CmbSubsidiaryAccountCr.Value) != 0)
			{
				int GLOfSubSidory = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccountCr).SelectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					cmbAccount.Value = GLOfSubSidory;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			lblCreditAcBalance.Visible = true;
			CreditAccountCurrentBalance();
			BindSubsidiaryAccountCrWithDbCall();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomGroup_Leave(object sender, EventArgs e)
	{
		try
		{
			int CustomId = Conversion.ToInt(CmbCustomGroup.Value);
			if (CustomId > 0)
			{
				DetailAccountsBind(AccountsAllocateToCustomAccountGroupByDocumentType.GetCustomAccountsByGroup(UserAccount.CompanyId, CustomId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbAccountDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string Note = "";
			if (((UltraGridBase)cmbAccountDetail).ActiveRow != null && Conversion.ToInt(cmbAccountDetail.Value) > 0)
			{
				Note = Conversion.ToString(((UltraDropDownBase)cmbAccountDetail).SelectedRow.Cells["NoteTitle"].Value);
			}
			label29.Text = Note;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = DocumentTypeId.ToString(),
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

	private DataTable CurrencyDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CurrencyBind(DataTable dt)
	{
		try
		{
			int Id = Conversion.ToInt(cmbCurrency.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", false);
				((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				CommonServices.SetComboValue(cmbCurrency, "Id", Id, dt);
			}
			else
			{
				((Control)(object)cmbCurrency).Text = string.Empty;
				((UltraGridBase)cmbCurrency).DataSource = null;
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
					double amount = Conversion.ToDouble(obj.Cells["Amount"].Value) / Conversion.ToDouble((object)txtExchangeRate.Text);
					amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					obj.Cells["FcyAmount"].Value = amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FcyAmount"].Value = 0;
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
			double TotalFcyAmount = 0.0;
			if (grd.GetRows().Count() > 0)
			{
				TotalFcyAmount = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
			}
			txtFcyAmount.Text = TotalFcyAmount.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbLocationType).ActiveRow == null || Conversion.ToInt(cmbLocationType.Value) == 0)
		{
			MessageBox.Show("Location Type Field is Required");
			cmbLocationType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbProjectId).ActiveRow == null || Conversion.ToInt(CmbProjectId.Value) == 0)
		{
			MessageBox.Show("Cost Center  Field Required");
			CmbProjectId.Focus();
			return false;
		}
		if (txtDocNo.Text.Trim() == string.Empty || txtDocNo.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo  Field Required");
			txtDocNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbAccount).ActiveRow == null || Conversion.ToInt(cmbAccount.Value) == 0)
		{
			MessageBox.Show("Account  Field Required");
			cmbAccount.Focus();
			return false;
		}
		if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountDatasourceLengthCr > 0 && (((UltraGridBase)CmbSubsidiaryAccountCr).ActiveRow == null || Conversion.ToInt(CmbSubsidiaryAccountCr.Value) == 0))
		{
			MessageBox.Show("Subsidiary Account Title Field Required");
			CmbSubsidiaryAccountCr.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbCurrency.Value) == 0)
		{
			MessageBox.Show("Fcy Code Field is Required");
			cmbCurrency.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
		{
			MessageBox.Show("Exchange Rate Field is Required");
			txtExchangeRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFcyAmount.Text) == 0.0)
		{
			MessageBox.Show("Fcy Amount Field is Required");
			txtFcyAmount.Focus();
			return false;
		}
		return true;
	}

	private bool FormDetailValidation()
	{
		if (((UltraGridBase)cmbAccountDetail).ActiveRow == null)
		{
			MessageBox.Show("Account  Field Required");
			cmbAccountDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbSubsidiaryAccount).Visible && SubsdiaryAccountDatasourceLengthDetail > 0 && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow == null)
		{
			MessageBox.Show("Subsidiary Account Title Field Required");
			CmbSubsidiaryAccount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobLotFile).ActiveRow == null)
		{
			MessageBox.Show("JobLot  Field Required");
			cmbJobLotFile.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmountDetail.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount  Field Required");
			txtAmountDetail.Focus();
			return false;
		}
		return true;
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbAccountDetail.Value = item.Cells["AccId"].Value.ToString();
				cmbAccountDetail_Leave(null, null);
				if (SubsdiaryAccountDatasourceLengthDetail == 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("SubsidiaryAccount");
					dt.Columns.Add("TypeId");
					dt.Columns.Add("AccountId");
					dt.Columns.Add("AccountTitle");
					dt.Rows.Add(Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value), item.Cells["SubsidiaryAccount"].Text, 4, Conversion.ToInt(item.Cells["AccId"].Value), item.Cells["Account"].Text);
					DDL.BindDDL(dt, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				CmbSubsidiaryAccount.Value = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
				cmbJobLotFile.Value = item.Cells["JobLotId"].Text.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Text.ToString();
				txtInvoiceNoDetail.Text = item.Cells["InvoiceNo"].Text.ToString();
				txtQtyDetial.Text = item.Cells["Qty"].Value.ToString();
				txtRateDetail.Text = item.Cells["Rate"].Text.ToString();
				txtAmountDetail.Text = item.Cells["Amount"].Text.ToString();
				txtFcyAmountDetail.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString("#,##0.###");
				if (Conversion.ToInt(item.Cells["ReferenceAccountId"].Value) > 0)
				{
					CmbReferenceAccount.Value = Conversion.ToInt(item.Cells["ReferenceAccountId"].Value);
				}
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				btnAdd.Visible = false;
				cmbAccountDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				dtDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			grd.UpdateData();
			UpdateDataInScheduleGrid();
			txtExchangeRate_TextChanged(null, null);
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
			if (!FormDetailValidation())
			{
				return;
			}
			lblbalancedetail.Visible = false;
			lbldetaildrcr.Visible = false;
			dtDetail.Rows[updateDetailIndex]["AccountCode"] = Conversion.ToString(((UltraDropDownBase)cmbAccountDetail).SelectedRow.Cells[2].Value);
			dtDetail.Rows[updateDetailIndex]["AccId"] = Conversion.ToInt(cmbAccountDetail.Value);
			dtDetail.Rows[updateDetailIndex]["Account"] = Conversion.ToString((object)((Control)(object)cmbAccountDetail).Text);
			if (((Control)(object)CmbSubsidiaryAccount).Visible)
			{
				if (((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && ((Control)(object)CmbSubsidiaryAccount).Text != string.Empty)
				{
					dtDetail.Rows[updateDetailIndex]["SubsidiaryAccountId"] = Conversion.ToInt(CmbSubsidiaryAccount.Value);
					dtDetail.Rows[updateDetailIndex]["SubsidiaryAccount"] = Conversion.ToString((object)((Control)(object)CmbSubsidiaryAccount).Text);
					dtDetail.Rows[updateDetailIndex]["SubsidiaryAccountTypeId"] = ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value;
				}
				else
				{
					dtDetail.Rows[updateDetailIndex]["SubsidiaryAccountId"] = Conversion.ToInt(cmbAccountDetail.Value);
					dtDetail.Rows[updateDetailIndex]["SubsidiaryAccount"] = Conversion.ToString((object)((Control)(object)cmbAccountDetail).Text);
					dtDetail.Rows[updateDetailIndex]["SubsidiaryAccountTypeId"] = 4;
				}
			}
			else
			{
				dtDetail.Rows[updateDetailIndex]["SubsidiaryAccountId"] = 0;
				dtDetail.Rows[updateDetailIndex]["SubsidiaryAccount"] = "";
				dtDetail.Rows[updateDetailIndex]["SubsidiaryAccountTypeId"] = 0;
			}
			dtDetail.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(cmbJobLotFile.Value);
			dtDetail.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)cmbJobLotFile).Text);
			dtDetail.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
			dtDetail.Rows[updateDetailIndex]["InvoiceNo"] = Conversion.ToString((object)txtInvoiceNoDetail.Text);
			dtDetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDouble((object)txtQtyDetial.Text);
			dtDetail.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble((object)txtRateDetail.Text);
			dtDetail.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmountDetail.Text);
			dtDetail.Rows[updateDetailIndex]["FcyAmount"] = Conversion.ToDouble((object)txtFcyAmountDetail.Text);
			dtDetail.Rows[updateDetailIndex]["ReferenceAccountId"] = Conversion.ToInt(CmbReferenceAccount.Value);
			dtDetail.Rows[updateDetailIndex]["ReferenceAccount"] = ((Conversion.ToInt(CmbReferenceAccount.Value) > 0) ? Conversion.ToString(((UltraDropDownBase)CmbReferenceAccount).SelectedRow.Cells[1].Value) : "");
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			btnAdd.Visible = true;
			ResetFormDetail();
			cmbAccountDetail.Focus();
			txtExchangeRate_TextChanged(null, null);
			UpdateDataInScheduleGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormDetailValidation())
			{
				dtDetail.Rows.Add(Conversion.ToString(((UltraDropDownBase)cmbAccountDetail).SelectedRow.Cells[2].Value), Conversion.ToInt(cmbAccountDetail.Value), Conversion.ToString((object)((Control)(object)cmbAccountDetail).Text), (!((Control)(object)CmbSubsidiaryAccount).Visible) ? ((object)0) : ((SubsdiaryAccountDatasourceLengthDetail > 0) ? CmbSubsidiaryAccount.Value : cmbAccountDetail.Value), (!((Control)(object)CmbSubsidiaryAccount).Visible) ? "" : ((SubsdiaryAccountDatasourceLengthDetail > 0) ? ((Control)(object)CmbSubsidiaryAccount).Text : ((Control)(object)cmbAccountDetail).Text), (!((Control)(object)CmbSubsidiaryAccount).Visible) ? ((object)0) : ((SubsdiaryAccountDatasourceLengthDetail > 0) ? ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value : ((object)4)), Conversion.ToInt(cmbJobLotFile.Value), Conversion.ToString((object)((Control)(object)cmbJobLotFile).Text), Conversion.ToString((object)txtInvoiceNoDetail.Text.Trim()), Conversion.ToDouble((object)txtQtyDetial.Text), Conversion.ToDouble((object)txtRateDetail.Text), Conversion.ToDouble((object)txtAmountDetail.Text), Conversion.ToDouble((object)txtFcyAmountDetail.Text), Conversion.ToInt(CmbReferenceAccount.Value), (Conversion.ToInt(CmbReferenceAccount.Value) > 0) ? Conversion.ToString(((UltraDropDownBase)CmbReferenceAccount).SelectedRow.Cells[1].Value) : "", Conversion.ToString((object)txtRemarksDetail.Text));
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				lblbalancedetail.Visible = false;
				lbldetaildrcr.Visible = false;
				DetailGridSetting();
				cmbAccountDetail.Focus();
				ResetFormDetail();
				UpdateDataInScheduleGrid();
				txtExchangeRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void DetailGridSetting()
	{
		try
		{
			grd.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["AccId"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccount"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccountTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["Account"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grd.RootTable.Columns["InvoiceNo"].EditType = (EditType)0;
			grd.RootTable.Columns["Qty"].EditType = (EditType)0;
			grd.RootTable.Columns["Rate"].EditType = (EditType)0;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Account"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["AccId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountTypeId"].Visible = false;
			grd.RootTable.Columns["ReferenceAccountId"].Visible = false;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			salesTex();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grd.UpdateData();
		salesTex();
	}

	private void btnAdd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "tabControl2")
			{
				cmbAccountDetail.Focus();
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
			if (base.ActiveControl.Name == "tabControl2")
			{
				cmbAccountDetail.Focus();
			}
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
			ResetFormDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateDataInScheduleGrid()
	{
		try
		{
			double amount = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2));
			if (grdSchedule.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdSchedule.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double dueAmount = Conversion.ToDouble(obj.Cells["Due%"].Value) * amount / 100.0;
					obj.Cells["Amount"].Value = dueAmount;
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void AddRowInScheduleGrid()
	{
		try
		{
			if (grdSchedule.RowCount > 0)
			{
				double totalPrcnt = Conversion.ToDouble(grdSchedule.GetTotal(grdSchedule.RootTable.Columns["Due%"], (AggregateFunction)2));
				if (totalPrcnt != 100.0)
				{
					double num = ((grd.RowCount > 0) ? Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString()) : 0.0);
					double percent = 100.0 - totalPrcnt;
					double percentAmount = num * percent / 100.0;
					dtSchedule.Rows.Add("", "", percent, percentAmount);
				}
			}
			else
			{
				dtSchedule.Rows.Add("", "", 0, 0);
			}
			grdSchedule.DataSource = dtSchedule;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ScheduleGridSetting()
	{
		grdSchedule.RootTable.Columns["DueDays"].Width = 70;
		grdSchedule.RootTable.Columns["DueDate"].Width = 100;
		grdSchedule.RootTable.Columns["Due%"].Width = 50;
		grdSchedule.RootTable.Columns["Amount"].Width = 80;
		grdSchedule.RootTable.Columns["DueDate"].EditType = (EditType)3;
		grdSchedule.RootTable.Columns.Add("Delete");
		grdSchedule.RootTable.Columns["Delete"].Key = "Delete";
		grdSchedule.RootTable.Columns["Delete"].Caption = "X";
		grdSchedule.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
		grdSchedule.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
		grdSchedule.RootTable.Columns["Delete"].ButtonText = "X";
		grdSchedule.RootTable.Columns["Delete"].Width = 20;
		grdSchedule.RootTable.Columns.Add("Add");
		grdSchedule.RootTable.Columns["Add"].Key = "Add";
		grdSchedule.RootTable.Columns["Add"].Caption = "+";
		grdSchedule.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
		grdSchedule.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
		grdSchedule.RootTable.Columns["Add"].ButtonText = "+";
		grdSchedule.RootTable.Columns["Add"].Width = 20;
		grdSchedule.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
	}

	private void grdSchedule_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdSchedule.CurrentRow;
				dtSchedule.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtSchedule.Rows.Count == 0)
				{
					AddRowInScheduleGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInScheduleGrid();
			}
			grdSchedule.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSchedule_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdSchedule.UpdateData();
			GridEXRow item = grdSchedule.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "DueDays")
				{
					item.Cells["DueDate"].Value = txtDocDate.Value.AddDays(Conversion.ToInt((object)item.Cells["DueDays"].Text.ToString()));
				}
				if (e.Column.Key == "Due%")
				{
					string sumof = "";
					sumof = ((grd.RowCount <= 0) ? 0.ToString() : grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
					double percent = Conversion.ToDouble((object)sumof) * (Conversion.ToDouble((object)item.Cells["Due%"].Value.ToString()) / 100.0);
					item.Cells["Amount"].Value = Math.Round(percent, 2);
				}
			}
			grdSchedule.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ResetForm()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			RecId = 0;
			txtInvoiceNoDetail.Text = string.Empty;
			((Control)(object)cmbAccount).Text = string.Empty;
			txtInvoiceNo.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)CmbTaxAccount).Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			lblCreditAcBalance.Text = "0";
			cmbAccount.Focus();
			dtDetail.Rows.Clear();
			grd.ClearStructure();
			dtSchedule.Rows.Clear();
			grdSchedule.ClearStructure();
			AddRowInScheduleGrid();
			grdSchedule.RetrieveStructure();
			ScheduleGridSetting();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			((Control)(object)CmbAdvanceTaxAccount).Text = string.Empty;
			((Control)(object)CmbOtherChargesAccount).Text = string.Empty;
			((Control)(object)CmbTaxType).Text = "";
			txtVoucherAmountFooter.Text = 0.ToString("#,##");
			txtAdvanceTaxAmount.Text = 0.ToString("#,##");
			txtOtherChargesAmount.Text = 0.ToString("#,##");
			txtTotalAmount.Text = 0.ToString("#,##");
			UpdateDocumentNoUI(DocumentNoDBCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ResetFormDetail()
	{
		try
		{
			((Control)(object)cmbAccountDetail).Text = string.Empty;
			txtInvoiceNoDetail.Text = string.Empty;
			txtQtyDetial.Text = "0";
			txtRateDetail.Text = "0";
			txtAmountDetail.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			btnAdd.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		ResetForm();
		ResetFormDetail();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			LocationTypeBind(VoucherHead.GetLocationType());
			CostCenterBind(CommonServices.ProjectServiceBind());
			AllAccountsWithTypeIdDbCall();
			AccountTypeBind();
			if (Conversion.ToInt(CmbAccountType.Value) > 0)
			{
				CmbAccountType_Leave(null, null);
			}
			else
			{
				CreditAccountAccountBind();
			}
			SalesTaxAccountBind();
			CurrencyBind(CurrencyDbCall());
			CustomGroupBind(AccountsAllocateToCustomAccountGroupByDocumentType.GetCustomAccountGroupsByDocumentType(DocumentTypeId, (string)null));
			int CustomId = Conversion.ToInt(CmbCustomGroup.Value);
			if (CustomId > 0)
			{
				DetailAccountsBind(AccountsAllocateToCustomAccountGroupByDocumentType.GetCustomAccountsByGroup(UserAccount.CompanyId, CustomId));
			}
			JobLotBind(CommonServices.JobLotGetAllService());
			ReferenceAccountBind();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				AllSubsidoryAccountDbCall();
			}
			TaxTypesBind(TaxTypesDbCall());
			AdvanceTaxAccountBind();
			DefaultConfigurations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillsPayables_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void txtDocNo_Leave(object sender, EventArgs e)
	{
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
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
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Expected O, but got Unknown
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0982: Expected O, but got Unknown
		//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Expected O, but got Unknown
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Expected O, but got Unknown
		//IL_11a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Expected O, but got Unknown
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c90: Expected O, but got Unknown
		//IL_0f90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f97: Expected O, but got Unknown
		//IL_0ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de2: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record not found");
			}
			if (!FormValidation())
			{
				return;
			}
			VoucherHead voucher = new VoucherHead();
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				voucher.Id = RecId;
			}
			voucher.ProjectId = Conversion.ToInt(CmbProjectId.Value);
			voucher.DocumentTypeId = DocumentTypeId;
			voucher.VoucherCode = Conversion.ToInt((object)txtDocNo.Text);
			voucher.VoucherDate = txtDocDate.Value;
			voucher.RefAccountId = Conversion.ToInt(cmbAccount.Value);
			voucher.AgainstAccountId = Conversion.ToInt(CmbTaxAccount.Value);
			voucher.ManualBillNo = txtInvoiceNo.Text;
			voucher.DueDate = txtInvoiceDate.Value;
			voucher.Remarks = txtRemarks.Text;
			voucher.OrganizationId = UserAccount.OrganizationId;
			voucher.CompanyId = UserAccount.CompanyId;
			voucher.BranchId = UserAccount.BranchesId;
			voucher.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			voucher.EntryUser = UserAccount.ID;
			voucher.ModifyUser = UserAccount.ID;
			voucher.IncludeWHT = Conversion.ToInt(CmbTaxType.Value) > 0 && Conversion.ToInt(CmbTaxAccount.Value) > 0;
			voucher.AdvanceTaxAccountId = Conversion.ToInt(CmbAdvanceTaxAccount.Value);
			voucher.AdvanceTaxAmount = Conversion.ToDouble((object)txtAdvanceTaxAmount.Text);
			voucher.OtherChargesAccountId = Conversion.ToInt(CmbOtherChargesAccount.Value);
			voucher.OtherChargesAmount = Conversion.ToDouble((object)txtOtherChargesAmount.Text);
			voucher.MultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
			voucher.ExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			voucher.FcAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
			voucher.CustomAccounts = chkCustomAccounts.Checked;
			voucher.Type = (VoucherType)2;
			voucher.voucherDetailList = new List<VoucherDetail>();
			voucher.PaymentDuesSchedules = new List<PaymentDueSchedule>();
			int LineId = 1;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(cmbAccount.Value) == Conversion.ToInt(r2.Cells["AccId"].Value) && !SubsidiaryAccountAllownOnVouchers)
				{
					MessageBox.Show("Dr Account Title Cannot Be Equal To Cr Account Title...");
					return;
				}
				if (Conversion.ToInt(CmbSubsidiaryAccountCr.Value) > 0 && Conversion.ToInt(CmbSubsidiaryAccountCr.Value) == Conversion.ToInt(r2.Cells["SubsidiaryAccountId"].Value) && SubsidiaryAccountAllownOnVouchers)
				{
					MessageBox.Show("Dr Subsidiary Account Cannot Be Equal To Cr Subsidiary Account...");
					return;
				}
				VoucherDetail vd = new VoucherDetail();
				vd.LineId = LineId;
				vd.AccountId = Conversion.ToInt(r2.Cells["AccId"].Value);
				vd.AgainstAccountId = Conversion.ToInt(cmbAccount.Value);
				vd.SubsidiaryTypeId = Conversion.ToInt(r2.Cells["SubsidiaryAccountTypeId"].Value);
				int SubsidiaryAccountId = Conversion.ToInt(r2.Cells["SubsidiaryAccountId"].Value);
				if (SubsidiaryAccountId > 0)
				{
					vd.SupplierCustomerId = ((vd.SubsidiaryTypeId == 1) ? SubsidiaryAccountId : 0);
					vd.EmployeeId = ((vd.SubsidiaryTypeId == 2) ? SubsidiaryAccountId : 0);
					vd.SubsidiaryAccountId = SubsidiaryAccountId;
				}
				vd.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				vd.RefInvoiceNo = Conversion.ToString(r2.Cells["InvoiceNo"].Value);
				vd.QtyIn = Conversion.ToDouble(r2.Cells["Qty"].Value);
				vd.ItemRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
				vd.ItemAmount = Conversion.ToDouble(r2.Cells["Amount"].Value);
				vd.DebitAmount = Conversion.ToDouble(r2.Cells["Amount"].Value);
				vd.Comments = Conversion.ToString(r2.Cells["Remarks"].Value);
				vd.IsTaxable = "False";
				vd.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
				vd.DMultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
				vd.ReferenceAccountId = Conversion.ToInt(r2.Cells["ReferenceAccountId"].Value);
				vd.DExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				vd.DCurrencyAmount = Conversion.ToDouble(r2.Cells["FcyAmount"].Value);
				if (vd.DCurrencyAmount == 0.0)
				{
					throw new Exception("Fcy Amount Not Found In Detail Grid at Row#" + (r2.RowIndex + 1));
				}
				voucher.voucherDetailList.Add(vd);
				LineId++;
				voucher.VoucherAmount += vd.DebitAmount;
				VoucherDetail vdCredit = new VoucherDetail();
				vdCredit.LineId = LineId;
				vdCredit.AccountId = Conversion.ToInt(cmbAccount.Value);
				vdCredit.AgainstAccountId = Conversion.ToInt(r2.Cells["AccId"].Value);
				vdCredit.SubsidiaryTypeId = ((((UltraGridBase)CmbSubsidiaryAccountCr).ActiveRow != null && Conversion.ToInt(CmbSubsidiaryAccountCr.Value) > 0) ? Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccountCr).SelectedRow.Cells[2].Value) : 0);
				int SubsidiaryAccountIdDr4 = Conversion.ToInt(CmbSubsidiaryAccountCr.Value);
				if (SubsidiaryAccountIdDr4 > 0)
				{
					vdCredit.SupplierCustomerId = ((vdCredit.SubsidiaryTypeId == 1) ? SubsidiaryAccountIdDr4 : 0);
					vdCredit.EmployeeId = ((vdCredit.SubsidiaryTypeId == 2) ? SubsidiaryAccountIdDr4 : 0);
					vdCredit.SubsidiaryAccountId = SubsidiaryAccountIdDr4;
				}
				vdCredit.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				vdCredit.RefInvoiceNo = Conversion.ToString(r2.Cells["InvoiceNo"].Value);
				vdCredit.QtyIn = Conversion.ToDouble(r2.Cells["Qty"].Value);
				vdCredit.ItemRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
				vdCredit.ItemAmount = Conversion.ToDouble(r2.Cells["Amount"].Value);
				vdCredit.CreditAmount = Conversion.ToDouble(r2.Cells["Amount"].Value);
				vdCredit.Comments = Conversion.ToString(r2.Cells["Remarks"].Value);
				vdCredit.IsTaxable = "False";
				vdCredit.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
				vdCredit.DMultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
				vdCredit.ReferenceAccountId = Conversion.ToInt(r2.Cells["ReferenceAccountId"].Value);
				vdCredit.DExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				vdCredit.DCurrencyAmount = Conversion.ToDouble(r2.Cells["FcyAmount"].Value);
				if (vdCredit.DCurrencyAmount == 0.0)
				{
					throw new Exception("Fcy Amount Not Found In Detail Grid at Row#" + (r2.RowIndex + 1));
				}
				voucher.voucherDetailList.Add(vdCredit);
				LineId++;
			}
			if (Conversion.ToDouble((object)txtTaxAmount.Text) > 0.0 && Conversion.ToInt(CmbTaxType.Value) > 0)
			{
				VoucherDetail vd7 = new VoucherDetail();
				vd7.AccountId = voucher.AgainstAccountId;
				vd7.AgainstAccountId = voucher.RefAccountId;
				vd7.Comments = "Tax % " + txtTaxPercent.Text.ToString();
				vd7.DebitAmount = Conversion.ToDouble((object)txtTaxAmount.Text);
				vd7.TaxesTotalAmount = vd7.DebitAmount;
				vd7.CreditAmount = 0.0;
				vd7.IsTaxable = "True";
				vd7.TaxPrcnt = Conversion.ToDouble((object)txtTaxPercent.Text);
				vd7.TaxTypeId = Conversion.ToInt(CmbTaxType.Value);
				voucher.voucherDetailList.Add(vd7);
				VoucherDetail vd4 = new VoucherDetail();
				vd4.AccountId = voucher.RefAccountId;
				vd4.Comments = "Tax%" + txtTaxPercent.Text.ToString();
				vd4.AgainstAccountId = voucher.AgainstAccountId;
				vd4.CreditAmount = Conversion.ToDouble((object)txtTaxAmount.Text);
				vd4.SubsidiaryTypeId = ((Conversion.ToInt(CmbSubsidiaryAccountCr.Value) > 0) ? Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccountCr).SelectedRow.Cells[2].Value) : 0);
				int SubsidiaryAccountIdDr3 = Conversion.ToInt(CmbSubsidiaryAccountCr.Value);
				if (SubsidiaryAccountIdDr3 > 0)
				{
					vd4.SupplierCustomerId = ((vd4.SubsidiaryTypeId == 1) ? SubsidiaryAccountIdDr3 : 0);
					vd4.EmployeeId = ((vd4.SubsidiaryTypeId == 2) ? SubsidiaryAccountIdDr3 : 0);
					vd4.SubsidiaryAccountId = SubsidiaryAccountIdDr3;
				}
				vd4.TaxesTotalAmount = vd4.CreditAmount;
				vd4.DebitAmount = 0.0;
				vd4.IsTaxable = "True";
				vd4.TaxPrcnt = Conversion.ToDouble((object)txtTaxPercent.Text);
				vd4.TaxTypeId = Conversion.ToInt(CmbTaxType.Value);
				voucher.voucherDetailList.Add(vd4);
			}
			if (Conversion.ToDouble((object)txtAdvanceTaxAmount.Text) > 0.0 && Conversion.ToInt(CmbAdvanceTaxAccount.Value) > 0)
			{
				VoucherDetail vd6 = new VoucherDetail();
				vd6.AccountId = Conversion.ToInt(CmbAdvanceTaxAccount.Value);
				vd6.AgainstAccountId = voucher.RefAccountId;
				vd6.Comments = "Advance Tax";
				vd6.DebitAmount = Conversion.ToDouble((object)txtAdvanceTaxAmount.Text);
				vd6.CreditAmount = 0.0;
				voucher.voucherDetailList.Add(vd6);
				VoucherDetail vd3 = new VoucherDetail();
				vd3.AccountId = voucher.RefAccountId;
				vd3.AgainstAccountId = Conversion.ToInt(CmbAdvanceTaxAccount.Value);
				vd3.Comments = "Advance Tax";
				vd3.CreditAmount = Conversion.ToDouble((object)txtAdvanceTaxAmount.Text);
				vd3.SubsidiaryTypeId = ((Conversion.ToInt(CmbSubsidiaryAccountCr.Value) > 0) ? Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccountCr).SelectedRow.Cells[2].Value) : 0);
				int SubsidiaryAccountIdDr2 = Conversion.ToInt(CmbSubsidiaryAccountCr.Value);
				if (SubsidiaryAccountIdDr2 > 0)
				{
					vd3.SupplierCustomerId = ((vd3.SubsidiaryTypeId == 1) ? SubsidiaryAccountIdDr2 : 0);
					vd3.EmployeeId = ((vd3.SubsidiaryTypeId == 2) ? SubsidiaryAccountIdDr2 : 0);
					vd3.SubsidiaryAccountId = SubsidiaryAccountIdDr2;
				}
				vd3.DebitAmount = 0.0;
				voucher.voucherDetailList.Add(vd3);
			}
			if (Conversion.ToDouble((object)txtOtherChargesAmount.Text) != 0.0 && Conversion.ToInt(CmbOtherChargesAccount.Value) > 0)
			{
				VoucherDetail vd5 = new VoucherDetail();
				vd5.AccountId = ((Conversion.ToDouble((object)txtOtherChargesAmount.Text) > 0.0) ? Conversion.ToInt(CmbOtherChargesAccount.Value) : voucher.RefAccountId);
				vd5.AgainstAccountId = ((Conversion.ToDouble((object)txtOtherChargesAmount.Text) > 0.0) ? voucher.RefAccountId : Conversion.ToInt(CmbOtherChargesAccount.Value));
				vd5.Comments = "Other Charges";
				vd5.DebitAmount = Conversion.ToDouble((object)txtOtherChargesAmount.Text);
				if (Conversion.ToDouble((object)txtOtherChargesAmount.Text) < 0.0)
				{
					vd5.SubsidiaryTypeId = ((Conversion.ToInt(CmbSubsidiaryAccountCr.Value) > 0) ? Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccountCr).SelectedRow.Cells[2].Value) : 0);
					int SubsidiaryAccountIdCr = Conversion.ToInt(CmbSubsidiaryAccountCr.Value);
					if (SubsidiaryAccountIdCr > 0)
					{
						vd5.SupplierCustomerId = ((vd5.SubsidiaryTypeId == 1) ? SubsidiaryAccountIdCr : 0);
						vd5.EmployeeId = ((vd5.SubsidiaryTypeId == 2) ? SubsidiaryAccountIdCr : 0);
						vd5.SubsidiaryAccountId = SubsidiaryAccountIdCr;
					}
				}
				voucher.voucherDetailList.Add(vd5);
				VoucherDetail vd2 = new VoucherDetail();
				vd2.AccountId = ((Conversion.ToDouble((object)txtOtherChargesAmount.Text) > 0.0) ? voucher.RefAccountId : Conversion.ToInt(CmbOtherChargesAccount.Value));
				vd2.AgainstAccountId = ((Conversion.ToDouble((object)txtOtherChargesAmount.Text) > 0.0) ? Conversion.ToInt(CmbOtherChargesAccount.Value) : voucher.RefAccountId);
				vd2.Comments = "Other Charges";
				vd2.CreditAmount = Conversion.ToDouble((object)txtOtherChargesAmount.Text);
				if (Conversion.ToDouble((object)txtOtherChargesAmount.Text) > 0.0)
				{
					vd2.SubsidiaryTypeId = ((Conversion.ToInt(CmbSubsidiaryAccountCr.Value) > 0) ? Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccountCr).SelectedRow.Cells[2].Value) : 0);
					int SubsidiaryAccountIdDr = Conversion.ToInt(CmbSubsidiaryAccountCr.Value);
					if (SubsidiaryAccountIdDr > 0)
					{
						vd2.SupplierCustomerId = ((vd2.SubsidiaryTypeId == 1) ? SubsidiaryAccountIdDr : 0);
						vd2.EmployeeId = ((vd2.SubsidiaryTypeId == 2) ? SubsidiaryAccountIdDr : 0);
						vd2.SubsidiaryAccountId = SubsidiaryAccountIdDr;
					}
				}
				voucher.voucherDetailList.Add(vd2);
			}
			if (Conversion.ToDouble((object)grdSchedule.GetTotal(grdSchedule.RootTable.Columns["Amount"], (AggregateFunction)2).ToString()) > 0.0)
			{
				double TotalAmount = 0.0;
				rows = grdSchedule.GetRows();
				foreach (GridEXRow r in rows)
				{
					PaymentDueSchedule dueSchedule = new PaymentDueSchedule();
					if (r.Cells["DueDays"].Text == string.Empty || Conversion.ToInt((object)r.Cells["DueDays"].Value.ToString()) == 0)
					{
						MessageBox.Show("Due Days Filed Required in Tax Grid");
						return;
					}
					dueSchedule.DueDays = Conversion.ToInt(r.Cells["DueDays"].Value);
					if (r.Cells["DueDate"].Text == string.Empty)
					{
						MessageBox.Show("Due Days Filed Required in Tax Grid");
						return;
					}
					dueSchedule.DueDate = Conversion.ToDateTime(r.Cells["DueDate"].Value);
					if (dueSchedule.DueDate < voucher.VoucherDate)
					{
						throw new Exception("Due date Should not greater than VoucherDate In Tax Grid");
					}
					dueSchedule.DuePercentage = Conversion.ToDouble(r.Cells["Due%"].Value);
					if (r.Cells["Amount"].Text == string.Empty || Conversion.ToDouble((object)r.Cells["Amount"].Value.ToString()) == 0.0)
					{
						MessageBox.Show("Amount Filed Required in Tax Grid");
						return;
					}
					dueSchedule.Amount = Conversion.ToDouble((object)r.Cells["Amount"].Value.ToString());
					TotalAmount += dueSchedule.Amount;
					dueSchedule.refDocumentTypeId = DocumentTypeId;
					voucher.PaymentDuesSchedules.Add(dueSchedule);
				}
				if (Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString()) != TotalAmount)
				{
					MessageBox.Show("Amount Doesn't Equal To Detail Grid Amount in Tax Schedule Grid");
					return;
				}
			}
			else
			{
				PaymentDueSchedule dueSchedule2 = new PaymentDueSchedule();
				dueSchedule2.DueDays = 1;
				dueSchedule2.DueDate = txtDocDate.Value;
				dueSchedule2.DuePercentage = 100.0;
				dueSchedule2.Amount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				dueSchedule2.refDocumentTypeId = DocumentTypeId;
				voucher.PaymentDuesSchedules.Add(dueSchedule2);
			}
			if (string.IsNullOrEmpty(AttachmentsValues) || AT.lst.Count > 0)
			{
				AttachmentsValues = "";
			}
			voucher.AttachmentsList = new List<DMSAttachments>();
			voucher.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<VoucherHead>(voucher, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			voucher.AttachmentsValues = ((AttachmentsValues != "") ? AttachmentsValues : voucher.AttachmentsValues);
			voucher.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = VoucherHead.Save(voucher);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully" + voucher.VoucherCode);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, DocumentTypeId, 0);
				}
			}
			else
			{
				MessageBox.Show("Record Save Successfully" + voucher.VoucherCode);
			}
			ResetForm();
			if (CHKPREVIEW.Checked)
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(success);
			}
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			ResetForm();
			RecId = ID;
			VoucherHead vh = new VoucherHead();
			vh = VoucherHead.GetByID(RecId);
			if (vh != null)
			{
				UpdateMode = true;
				btnsave.Visible = false;
				btnSaveAs.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				txtDocDate.Value = vh.VoucherDate;
				cmbAccount.Value = vh.RefAccountId;
				CmbProjectId.Value = vh.ProjectId;
				txtRemarks.Text = vh.Remarks;
				txtInvoiceNo.Text = vh.ManualBillNo;
				txtDocNo.Text = vh.VoucherCode.ToString();
				cmbCurrency.Value = vh.MultiCurrencyId;
				chkCustomAccounts.Checked = vh.CustomAccounts;
				txtExchangeRate.Text = vh.ExchangeCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtFcyAmount.Text = vh.FcAmount.ToString("#,##0.###");
				AttachmentsValues = vh.AttachmentsValues;
				CustomAttachmentsValues = vh.CustomAttachmentsValues;
				CmbAdvanceTaxAccount.Value = vh.AdvanceTaxAccountId;
				txtAdvanceTaxAmount.Text = vh.AdvanceTaxAmount.ToString("#,##0.###");
				CmbOtherChargesAccount.Value = vh.OtherChargesAccountId;
				txtOtherChargesAmount.Text = vh.OtherChargesAmount.ToString("#,##0.###");
				dtDetail.Clear();
				bool subsidiaryflag = false;
				if (((UltraGridBase)cmbLocationType).ActiveRow == null || Conversion.ToInt(cmbLocationType.Value) == 0)
				{
					VoucherDetail firstValidLocation = vh.voucherDetailList.FirstOrDefault((VoucherDetail x) => Conversion.ToInt((object)x.LocationTypeId) > 0);
					cmbLocationType.Value = ((firstValidLocation != null) ? Conversion.ToInt((object)firstValidLocation.LocationTypeId) : 0);
				}
				for (int k = 0; k < vh.voucherDetailList.Count; k++)
				{
					if (SubsidiaryAccountAllownOnVouchers && vh.voucherDetailList[k].IsTaxable == "False" && vh.voucherDetailList[k].CreditAmount > 0.0 && !subsidiaryflag)
					{
						cmbAccount_Leave(null, null);
						CmbSubsidiaryAccountCr.Value = vh.voucherDetailList[k].SubsidiaryAccountId;
						subsidiaryflag = true;
					}
					if (!(vh.voucherDetailList[k].IsTaxable == "False"))
					{
						continue;
					}
					if (Conversion.ToInt(CmbSubsidiaryAccountCr.Value) > 0)
					{
						if ((vh.voucherDetailList[k].AccountId != Convert.ToInt32(cmbAccount.Value) && !SubsidiaryAccountAllownOnVouchers) || (vh.voucherDetailList[k].SubsidiaryAccountId != Convert.ToInt32(CmbSubsidiaryAccountCr.Value) && SubsidiaryAccountAllownOnVouchers && vh.voucherDetailList[k].DebitAmount > 0.0))
						{
							dtDetail.Rows.Add(vh.voucherDetailList[k].AccountCode, vh.voucherDetailList[k].AccountId, vh.voucherDetailList[k].AccountTitle, vh.voucherDetailList[k].SubsidiaryAccountId, vh.voucherDetailList[k].SubsidiaryAccountTitle, vh.voucherDetailList[k].SubsidiaryTypeId, vh.voucherDetailList[k].JobLotId, vh.voucherDetailList[k].JobLotDescription, vh.voucherDetailList[k].RefInvoiceNo, vh.voucherDetailList[k].QtyIn, vh.voucherDetailList[k].ItemRate, vh.voucherDetailList[k].DebitAmount, vh.voucherDetailList[k].DCurrencyAmount, vh.voucherDetailList[k].ReferenceAccountId, vh.voucherDetailList[k].ReferenceAccount, vh.voucherDetailList[k].Comments);
						}
					}
					else if ((vh.voucherDetailList[k].AccountId != Convert.ToInt32(cmbAccount.Value) && !SubsidiaryAccountAllownOnVouchers) || (SubsidiaryAccountAllownOnVouchers && vh.voucherDetailList[k].DebitAmount > 0.0))
					{
						dtDetail.Rows.Add(vh.voucherDetailList[k].AccountCode, vh.voucherDetailList[k].AccountId, vh.voucherDetailList[k].AccountTitle, vh.voucherDetailList[k].SubsidiaryAccountId, vh.voucherDetailList[k].SubsidiaryAccountTitle, vh.voucherDetailList[k].SubsidiaryTypeId, vh.voucherDetailList[k].JobLotId, vh.voucherDetailList[k].JobLotDescription, vh.voucherDetailList[k].RefInvoiceNo, vh.voucherDetailList[k].QtyIn, vh.voucherDetailList[k].ItemRate, vh.voucherDetailList[k].DebitAmount, vh.voucherDetailList[k].DCurrencyAmount, vh.voucherDetailList[k].ReferenceAccountId, vh.voucherDetailList[k].ReferenceAccount, vh.voucherDetailList[k].Comments);
					}
				}
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				DetailGridSetting();
				for (int j = 0; j < vh.voucherDetailList.Count; j++)
				{
					if (vh.voucherDetailList[j].IsTaxable == "True")
					{
						if (vh.voucherDetailList[j].DebitAmount > 0.0)
						{
							CmbTaxType.Value = vh.voucherDetailList[j].TaxTypeId;
							CmbTaxAccount.Value = vh.voucherDetailList[j].AccountId;
							txtTaxPercent.Text = vh.voucherDetailList[j].TaxPrcnt.ToString();
							txtTaxAmount.Text = vh.voucherDetailList[j].TaxesTotalAmount.ToString();
							break;
						}
					}
					else
					{
						txtTaxPercent.Text = "0";
						txtTaxAmount.Text = "0";
					}
				}
				dtSchedule.Columns.Clear();
				dtSchedule.Columns.Add("DueDays");
				dtSchedule.Columns.Add("DueDate");
				dtSchedule.Columns.Add("Due%", typeof(double));
				dtSchedule.Columns.Add("Amount", typeof(double));
				dtSchedule.Rows.Clear();
				if (vh.PaymentDuesSchedules.Count > 0)
				{
					for (int i = 0; i < vh.PaymentDuesSchedules.Count; i++)
					{
						dtSchedule.Rows.Add(vh.PaymentDuesSchedules[i].DueDays, vh.PaymentDuesSchedules[i].DueDate.ToString("dd-MMM-yyyy"), vh.PaymentDuesSchedules[i].DuePercentage, vh.PaymentDuesSchedules[i].Amount);
					}
				}
				else
				{
					AddRowInScheduleGrid();
				}
				grdSchedule.DataSource = dtSchedule;
				grdSchedule.RetrieveStructure();
				ScheduleGridSetting();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				txtExchangeRate_TextChanged(null, null);
			}
			else
			{
				ResetForm();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Calculation()
	{
		try
		{
			double Amount = 0.0;
			if (Conversion.ToDouble((object)txtQtyDetial.Text) != 0.0 && Conversion.ToDouble((object)txtRateDetail.Text) != 0.0)
			{
				Amount = Conversion.ToDouble((object)txtQtyDetial.Text) * Conversion.ToDouble((object)txtRateDetail.Text);
			}
			txtAmountDetail.Text = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyDetial_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRateDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAmountDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			try
			{
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
				{
					txtExchangeRate.Focus();
					throw new Exception("Please enter exchange Rate First");
				}
				CommonServices.UpdateRelatedFieldsCalculation(txtAmountDetail, txtFcyAmountDetail, Conversion.ToDouble((object)txtExchangeRate.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void DetailAccountCurrentBalance()
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
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = txtDocDate.Value,
				RefAccountId = Conversion.ToInt(cmbAccountDetail.Value)
			});
			if (detail.Rows.Count > 0)
			{
				lblbalancedetail.Text = detail.Rows[0]["Balance"].ToString();
				double num = Conversion.ToDouble((object)lblbalancedetail.Text);
				if (num > 0.0)
				{
					lbldetaildrcr.Text = "Dr";
				}
				else
				{
					lbldetaildrcr.Text = "Cr";
				}
				if (num == 0.0)
				{
					lbldetaildrcr.Text = "Nill";
				}
				lbldetaildrcr.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CreditAccountCurrentBalance()
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
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = txtDocDate.Value,
				RefAccountId = Conversion.ToInt(cmbAccount.Value)
			});
			if (detail.Rows.Count > 0)
			{
				lblCreditAcBalance.Text = detail.Rows[0]["Balance"].ToString();
				double num = Conversion.ToDouble((object)lblCreditAcBalance.Text);
				if (num > 0.0)
				{
					lblDrCrS.Text = "Dr";
				}
				else
				{
					lblDrCrS.Text = "Cr";
				}
				if (num == 0.0)
				{
					lblDrCrS.Text = "Nill";
				}
				lblDrCrS.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			salesTex();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void salesTex()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			int TaxId = Conversion.ToInt(CmbTaxType.Value);
			if (TaxId > 0)
			{
				DataTable detail = TaxScheduleMain.ReadTaxSchedule(new TaxScheduleMain
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EffectedDate = txtDocDate.Value,
					TaxNameId = TaxId
				});
				if (detail.Rows.Count > 0)
				{
					txtTaxPercent.Text = detail.Rows[0]["TaxPercent"].ToString();
					if (!UpdateMode || Conversion.ToInt(CmbTaxAccount.Value) == 0)
					{
						DataTable dataTable = (DataTable)((UltraGridBase)CmbTaxAccount).DataSource;
						int Idd = Conversion.ToInt(detail.Rows[0]["TaxGLAccountId"]);
						if (dataTable != null && dataTable.Rows.Count > 0 && Idd > 0)
						{
							CommonServices.SetComboValue(CmbTaxAccount, "Id", Idd, dataTable);
						}
					}
					double GridTotalAmount = ((grd.RowCount > 0) ? Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
					double TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
					double taxamount = ((GridTotalAmount > 0.0 && TaxPercent > 0.0) ? Math.Round(GridTotalAmount * TaxPercent / 100.0, 2) : 0.0);
					txtTaxAmount.Text = taxamount.ToString();
				}
				else
				{
					txtTaxPercent.Text = "0";
					txtTaxAmount.Text = "0";
				}
			}
			else
			{
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
			}
			TotalAmountInfooter();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmountInfooter()
	{
		try
		{
			double GridTotalAmount = ((grd.RowCount > 0) ? Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
			txtVoucherAmountFooter.Text = GridTotalAmount.ToString("#,##");
			double SalesTaxamount = Conversion.ToDouble((object)txtTaxAmount.Text);
			double AdvanceTaxamount = Conversion.ToDouble((object)txtAdvanceTaxAmount.Text);
			double OtherChargesAmount = Conversion.ToDouble((object)txtOtherChargesAmount.Text);
			double TotalAmount = GridTotalAmount + SalesTaxamount + AdvanceTaxamount + OtherChargesAmount;
			txtTotalAmount.Text = TotalAmount.ToString("#,##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyAmountDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
			{
				txtExchangeRate.Focus();
				throw new Exception("Please enter exchange Rate First");
			}
			CommonServices.UpdateRelatedFieldsCalculation(txtAmountDetail, txtFcyAmountDetail, Conversion.ToDouble((object)txtExchangeRate.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherChargesAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmountInfooter();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAdvanceTaxAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmountInfooter();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void StatusFillForHistory()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Not Apporved");
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

	private DataTable AccountTitleHistoryDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			return VoucherReports.LedgerByJobLotDropDownAndLists(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.OrganizationId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = DocumentTypeId.ToString()
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ComboBindForHistory(DataTable dt)
	{
		try
		{
			int Id = Conversion.ToInt(CmbAccountTitleHistory.Value);
			DataTable dtAccount = new DataTable();
			dtAccount.Columns.Add("Id", typeof(int));
			dtAccount.Columns.Add("AccountTitle");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Account Header")
					{
						dtAccount.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			if (dtAccount.Rows.Count > 0)
			{
				DDL.BindDDL(dtAccount, CmbAccountTitleHistory, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				CmbAccountTitleHistory.Value = Id;
				CommonServices.SetComboValue(CmbAccountTitleHistory, "Id", Id, dt);
			}
			else
			{
				CmbAccountTitleHistory.Value = 0;
				((Control)(object)CmbAccountTitleHistory).Text = string.Empty;
			}
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
				FromDateHistory.Focus();
			}
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
			((Control)(object)CmbAccountTitleHistory).Text = string.Empty;
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
			VoucherInfoBox.Visible = false;
			DataGridHistory.ClearStructure();
			grd.ClearStructure();
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
			ComboBindForHistory(AccountTitleHistoryDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
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

	private void btnShowHistory_Leave(object sender, EventArgs e)
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

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.AppId = UserAccount.AppId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.Ids = DocumentTypeId.ToString();
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (rdexpdocdate.Checked)
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
			else if (rdexpentry.Checked)
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
			else if (rdexpmodify.Checked)
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
			else if (rdexpapproved.Checked)
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
			vh.AccountId = Conversion.ToInt(CmbAccountTitleHistory.Value);
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
			DataTable lst = VoucherHead.VoucherFormHistory(vh);
			if (lst.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id");
				dtHistory.Columns.Add("AccountTypeId", typeof(int));
				dtHistory.Columns.Add("VoucherCode", typeof(int));
				dtHistory.Columns.Add("DocumentTypeId");
				dtHistory.Columns.Add("DocumentTypeCode", typeof(string));
				dtHistory.Columns.Add("VoucherDate", typeof(DateTime));
				dtHistory.Columns.Add("ManualBillNo");
				dtHistory.Columns.Add("AccountTitle");
				dtHistory.Columns.Add("AgainstAccount");
				dtHistory.Columns.Add("VoucherAmount", typeof(double));
				dtHistory.Columns.Add("FcyCode");
				dtHistory.Columns.Add("ExchangeRate", typeof(double));
				dtHistory.Columns.Add("FcyAmount", typeof(double));
				dtHistory.Columns.Add("Remarks");
				dtHistory.Columns.Add("EntryUser");
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser");
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("ApprovedUser");
				dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
				dtHistory.Columns.Add("CheqNo");
				dtHistory.Columns.Add("Attachment");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtHistory.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["AccountTypeId"], lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["ManualBillNo"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["AgainstAccount"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["Remarks"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["NoOfAttachments"]);
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
				grdDetailHistory.ClearStructure();
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
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["AccountTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
			DataGridHistory.RootTable.Columns["AgainstAccount"].Visible = false;
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["VoucherAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			DataGridHistory.RootTable.Columns["VoucherCode"].Caption = "V.No";
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Caption = "V.Type";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns["VoucherDate"].Width = 90;
			DataGridHistory.RootTable.Columns["VoucherCode"].Width = 60;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Width = 60;
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("PrintII");
			DataGridHistory.RootTable.Columns["PrintII"].Caption = "PrintII";
			DataGridHistory.RootTable.Columns["PrintII"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["PrintII"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["PrintII"].ButtonText = "PrintII";
			DataGridHistory.RootTable.Columns["PrintII"].Width = 60;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("SaveAs");
			DataGridHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].Width = 70;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns["PrintII"].Position = 1;
			DataGridHistory.RootTable.Columns["Edit"].Position = 2;
			DataGridHistory.RootTable.Columns["SaveAs"].Position = 3;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Position = 4;
			DataGridHistory.FrozenColumns = 5;
			DataGridHistory.RootTable.Columns["Remarks"].Width = 200;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				int ID = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(ID);
			}
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
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			int ID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				ReadById(ID);
			}
			else if (e.Column.Key == "SaveAs")
			{
				if (formright.DoHaveUpdateRights)
				{
					ReadById(ID);
					btnsave.Visible = false;
					btnUpdate.Visible = false;
					btnSaveAs.Visible = true;
				}
			}
			else if (e.Column.Key == "Print")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(ID);
			}
			else if (e.Column.Key == "PrintII")
			{
				CommonServices.BillsPayablesOrReceiveableVoucher102_01(ID, DocumentTypeId);
			}
			else if (e.Column.Key == "Print_102")
			{
				CommonServices.AcRptPaymentReceiptsVoucherSlip_102(ID);
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherDetailByHeaderId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			VoucherHead vh = VoucherHead.GetByID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
			DataTable table = new DataTable();
			table.Columns.Add("AccountCode");
			table.Columns.Add("AccountTitle");
			table.Columns.Add("SubsidiaryAccount");
			table.Columns.Add("Job/Lot");
			table.Columns.Add("InvoiceNo");
			table.Columns.Add("Qty", typeof(double));
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("DebitAmount", typeof(double));
			table.Columns.Add("CreditAmount", typeof(double));
			table.Columns.Add("FcyAmount", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("ReferenceAccount", typeof(string));
			table.Rows.Clear();
			for (int i = 0; i < vh.voucherDetailList.Count; i++)
			{
				if (vh.voucherDetailList[i].DebitAmount > 0.0 || vh.voucherDetailList[i].CreditAmount > 0.0)
				{
					table.Rows.Add(vh.voucherDetailList[i].AccountCode, vh.voucherDetailList[i].AccountTitle, vh.voucherDetailList[i].SubsidiaryAccountTitle, vh.voucherDetailList[i].JobLotDescription, vh.voucherDetailList[i].RefInvoiceNo, vh.voucherDetailList[i].QtyIn, vh.voucherDetailList[i].ItemRate, vh.voucherDetailList[i].DebitAmount, vh.voucherDetailList[i].CreditAmount, vh.voucherDetailList[i].DCurrencyAmount, vh.voucherDetailList[i].Comments, vh.voucherDetailList[i].ReferenceAccount);
				}
			}
			grdDetailHistory.DataSource = table;
			grdDetailHistory.RetrieveStructure();
			grdDetailHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdDetailHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdDetailHistory.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			grdDetailHistory.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdDetailHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailHistory.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailHistory.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			grdDetailHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			grdDetailHistory.RootTable.Columns["CreditAmount"].Caption = "Credit";
			grdDetailHistory.RootTable.Columns["DebitAmount"].Caption = "Debit";
			CommonServices.GridAutoAdjustmentNew(grdDetailHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			VoucherDetailByHeaderId();
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
			GenerateReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport()
	{
		try
		{
			CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
		if (fs != null)
		{
			grd.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void txtQtyDetial_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRateDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAmountDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFcyAmountDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAdvanceTaxAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOtherChargesAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmBillsPayables_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
			{
				GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					Conversion.ToInt((object)checkedRows[i].Cells["Id"].Value.ToString());
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtDocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				Print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtDocDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbAccountDetail.Focus();
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.Up)
			{
				cmbAccountDetail.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grd).Focused)
				{
					((Control)(object)grdSchedule).Focus();
				}
				else if (((Control)(object)grdSchedule).Focused)
				{
					CmbTaxAccount.Focus();
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Grids");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Debit Account in Detail Box");
			dt.Rows.Add("Ctrl+shift+ArrowUp", "For Focus On Sale Tax Account int Sales Tax Tab");
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
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
				grdDetail_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				GridEXRow r = grd.CurrentRow;
				if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null && grd.CurrentColumn.Key == "Delete" && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					dtDetail.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					grd.UpdateData();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(Id);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					ReadById(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Print" && formright.DoHavePrintRights)
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "PrintII" && formright.DoHavePrintRights)
				{
					CommonServices.BillsPayablesOrReceiveableVoucher102_01(Id, DocumentTypeId);
				}
				if (DataGridHistory.CurrentColumn.Key == "SaveAs" && formright.DoHavePrintRights)
				{
					ReadById(Id);
					btnsave.Visible = false;
					btnUpdate.Visible = false;
					btnSaveAs.Visible = true;
				}
				if (DataGridHistory.CurrentColumn.Key == "Attachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLookup_Click(object sender, EventArgs e)
	{
		try
		{
			new frmAccountsAllocateToCustomAccountGroupByDocumentType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTypeBind()
	{
		try
		{
			int[] Ids = new int[8] { 3, 5, 6, 8, 16, 17, 18, 19 };
			var list = (from r in (from row in AllAccountsWithTypeId.AsEnumerable()
					where Ids.Contains(row.Field<int>("AccountTypeId"))
					select new
					{
						AccountTypeId = row.Field<int>("AccountTypeId"),
						AccountType = row.Field<string>("AccountType")
					}).Distinct()
				orderby r.AccountTypeId
				select r).ToList();
			DataTable result = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"AccountType",
						typeof(string)
					}
				}
			};
			foreach (var row2 in list)
			{
				result.Rows.Add(row2.AccountTypeId, row2.AccountType);
			}
			DDL.BindDDL(result, CmbAccountType, "Id", "AccountType", "Account Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbAccountType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbAccountType.Value) > 0)
			{
				int CrId = Conversion.ToInt(cmbAccount.Value);
				DataTable dt = GetAccountsByAccountType(Conversion.ToInt(CmbAccountType.Value).ToString());
				DDL.BindDDL(dt, cmbAccount, "Id", "AccountTitle", "Credit Account", ZeroIndex: true);
				((UltraGridBase)cmbAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				CommonServices.SetComboValue(cmbAccount, "Id", CrId, dt);
			}
			else
			{
				CreditAccountAccountBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable GetAccountsByAccountType(string accountTypeIds)
	{
		try
		{
			int result;
			HashSet<int> accountTypeIdSet = new HashSet<int>((from id in accountTypeIds.Split(',')
				select id.Trim() into id
				where int.TryParse(id, out result)
				select id).Select(int.Parse));
			DataTable dt = new DataTable();
			dt.Columns.AddRange(new DataColumn[5]
			{
				new DataColumn("Id", typeof(int)),
				new DataColumn("AccountTitle", typeof(string)),
				new DataColumn("AccountCode", typeof(string)),
				new DataColumn("AccountTypeId", typeof(int)),
				new DataColumn("NoteTitle", typeof(string))
			});
			foreach (DataRow dr in AllAccountsWithTypeId.Rows)
			{
				int accountTypeId = Conversion.ToInt(dr["AccountTypeId"]);
				if (accountTypeIdSet.Contains(accountTypeId))
				{
					dt.Rows.Add(dr["Id"], dr["AccountTitle"], dr["AccountCode"], accountTypeId, dr["NoteTitle"]);
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception("Error while filtering accounts by type ID: " + ex.Message, ex);
		}
	}

	private void BtnPrintII_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.BillsPayablesOrReceiveableVoucher102_01(RecId, DocumentTypeId);
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
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Expected O, but got Unknown
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Expected O, but got Unknown
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Expected O, but got Unknown
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Expected O, but got Unknown
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Expected O, but got Unknown
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Expected O, but got Unknown
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Expected O, but got Unknown
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Expected O, but got Unknown
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Expected O, but got Unknown
		//IL_09a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Expected O, but got Unknown
		//IL_0a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1f: Expected O, but got Unknown
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Expected O, but got Unknown
		//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a98: Expected O, but got Unknown
		//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa3: Expected O, but got Unknown
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab9: Expected O, but got Unknown
		//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Expected O, but got Unknown
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7f: Expected O, but got Unknown
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0e: Expected O, but got Unknown
		//IL_0c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c50: Expected O, but got Unknown
		//IL_1210: Unknown result type (might be due to invalid IL or missing references)
		//IL_121a: Expected O, but got Unknown
		//IL_1227: Unknown result type (might be due to invalid IL or missing references)
		//IL_1231: Expected O, but got Unknown
		//IL_7b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b93: Expected O, but got Unknown
		//IL_7ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_7baa: Expected O, but got Unknown
		//IL_8f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_8f2e: Expected O, but got Unknown
		//IL_8f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_8f5c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
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
		Appearance appearance172 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		GridEXLayout grdSchedule_DesignTimeLayout = new GridEXLayout();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
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
		GridEXLayout grdDetailHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.frmBillsPayables));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.PanelCustomGroupDetail = new System.Windows.Forms.Panel();
		this.CmbCustomGroup = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.PanelAccountTitleDetail = new System.Windows.Forms.Panel();
		this.cmbAccountDetail = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.lbldetaildrcr = new System.Windows.Forms.Label();
		this.lblbalancedetail = new System.Windows.Forms.Label();
		this.PanelSubsidiaryDetail = new System.Windows.Forms.Panel();
		this.CmbSubsidiaryAccount = new UltraCombo();
		this.lblSubsidiaryaccount = new System.Windows.Forms.Label();
		this.PanelJobLotDetail = new System.Windows.Forms.Panel();
		this.cmbJobLotFile = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.PaanelInvoiceNoDetail = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.txtInvoiceNoDetail = new System.Windows.Forms.TextBox();
		this.PanelQty = new System.Windows.Forms.Panel();
		this.txtQtyDetial = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.PanelRateDetail = new System.Windows.Forms.Panel();
		this.txtRateDetail = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.PanelAmountDetail = new System.Windows.Forms.Panel();
		this.txtAmountDetail = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.PanelFcyAmountDetail = new System.Windows.Forms.Panel();
		this.txtFcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.ReferenceAccountPanel = new System.Windows.Forms.Panel();
		this.CmbReferenceAccount = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.PanelRemarksDetail = new System.Windows.Forms.Panel();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.PanelAddUpdateBtn = new System.Windows.Forms.Panel();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbAccountType = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.cmbCurrency = new UltraCombo();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.CmbProjectId = new UltraCombo();
		this.CmbSubsidiaryAccountCr = new UltraCombo();
		this.lblSubsidiaryaccountCr = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.lblCreditAcBalance = new System.Windows.Forms.TextBox();
		this.txtInvoiceDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbAccount = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.txtInvoiceNo = new System.Windows.Forms.TextBox();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.lblDrCrS = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintII = new System.Windows.Forms.ToolStripButton();
		this.BtnLookup = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.CHKPREVIEW = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.txtVoucherAmountFooter = new System.Windows.Forms.TextBox();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.txtOtherChargesAmount = new System.Windows.Forms.TextBox();
		this.txtAdvanceTaxAmount = new System.Windows.Forms.TextBox();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.CmbTaxType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdSchedule = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.labelNoteTitleDetailAccount = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbOtherChargesAccount = new UltraCombo();
		this.CmbAdvanceTaxAccount = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbTaxAccount = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel11 = new System.Windows.Forms.Panel();
		this.VoucherInfoBox = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchers = new System.Windows.Forms.Label();
		this.lblTotalVouchers = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.lblApprovedVouchers = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbAccountTitleHistory = new UltraCombo();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.rdexpapproved = new System.Windows.Forms.RadioButton();
		this.rdexpmodify = new System.Windows.Forms.RadioButton();
		this.rdexpdocdate = new System.Windows.Forms.RadioButton();
		this.rdexpentry = new System.Windows.Forms.RadioButton();
		this.label28 = new System.Windows.Forms.Label();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label30 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.cmbApproveStatus = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label40 = new System.Windows.Forms.Label();
		this.grdDetailHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label35 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.PanelCustomGroupDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		this.PanelAccountTitleDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountDetail).BeginInit();
		this.PanelSubsidiaryDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).BeginInit();
		this.PanelJobLotDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotFile).BeginInit();
		this.PaanelInvoiceNoDetail.SuspendLayout();
		this.PanelQty.SuspendLayout();
		this.PanelRateDetail.SuspendLayout();
		this.PanelAmountDetail.SuspendLayout();
		this.PanelFcyAmountDetail.SuspendLayout();
		this.ReferenceAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).BeginInit();
		this.PanelRemarksDetail.SuspendLayout();
		this.PanelAddUpdateBtn.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccountCr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSchedule).BeginInit();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherChargesAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceTaxAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.VoucherInfoBox.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1166, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1166, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel16);
		this.tabPage1.Controls.Add(this.groupBox2);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel19);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1158, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 334);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1152, 115);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 311);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1152, 23);
		this.panel16.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1111, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar1.TabIndex = 24;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.groupBox2.Controls.Add(this.flowLayoutPanel1);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 195);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1152, 116);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.flowLayoutPanel1.Controls.Add(this.PanelCustomGroupDetail);
		this.flowLayoutPanel1.Controls.Add(this.PanelAccountTitleDetail);
		this.flowLayoutPanel1.Controls.Add(this.PanelSubsidiaryDetail);
		this.flowLayoutPanel1.Controls.Add(this.PanelJobLotDetail);
		this.flowLayoutPanel1.Controls.Add(this.PaanelInvoiceNoDetail);
		this.flowLayoutPanel1.Controls.Add(this.PanelQty);
		this.flowLayoutPanel1.Controls.Add(this.PanelRateDetail);
		this.flowLayoutPanel1.Controls.Add(this.PanelAmountDetail);
		this.flowLayoutPanel1.Controls.Add(this.PanelFcyAmountDetail);
		this.flowLayoutPanel1.Controls.Add(this.ReferenceAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.PanelRemarksDetail);
		this.flowLayoutPanel1.Controls.Add(this.PanelAddUpdateBtn);
		this.flowLayoutPanel1.Location = new System.Drawing.Point(1, 18);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(1020, 94);
		this.flowLayoutPanel1.TabIndex = 2;
		this.PanelCustomGroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.PanelCustomGroupDetail.Controls.Add(this.label24);
		this.PanelCustomGroupDetail.Location = new System.Drawing.Point(0, 0);
		this.PanelCustomGroupDetail.Margin = new System.Windows.Forms.Padding(0);
		this.PanelCustomGroupDetail.Name = "PanelCustomGroupDetail";
		this.PanelCustomGroupDetail.Size = new System.Drawing.Size(159, 45);
		this.PanelCustomGroupDetail.TabIndex = 0;
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(1, 18);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(158, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 123;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Leave += new System.EventHandler(CmbCustomGroup_Leave);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(2, 2);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(84, 15);
		this.label24.TabIndex = 124;
		this.label24.Text = "Custom Group";
		this.PanelAccountTitleDetail.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountDetail);
		this.PanelAccountTitleDetail.Controls.Add(this.label29);
		this.PanelAccountTitleDetail.Controls.Add(this.label9);
		this.PanelAccountTitleDetail.Controls.Add(this.lbldetaildrcr);
		this.PanelAccountTitleDetail.Controls.Add(this.lblbalancedetail);
		this.PanelAccountTitleDetail.Location = new System.Drawing.Point(159, 0);
		this.PanelAccountTitleDetail.Margin = new System.Windows.Forms.Padding(0);
		this.PanelAccountTitleDetail.Name = "PanelAccountTitleDetail";
		this.PanelAccountTitleDetail.Size = new System.Drawing.Size(272, 45);
		this.PanelAccountTitleDetail.TabIndex = 1;
		this.cmbAccountDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance204;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountDetail).Location = new System.Drawing.Point(1, 19);
		((System.Windows.Forms.Control)(object)this.cmbAccountDetail).Name = "cmbAccountDetail";
		((System.Windows.Forms.Control)(object)this.cmbAccountDetail).Size = new System.Drawing.Size(270, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbAccountDetail).TextChanged += new System.EventHandler(cmbAccountDetail_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbAccountDetail).Leave += new System.EventHandler(cmbAccountDetail_Leave);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(81, 3);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(59, 15);
		this.label29.TabIndex = 122;
		this.label29.Text = "Note Title";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(4, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(76, 15);
		this.label9.TabIndex = 122;
		this.label9.Text = "Account (Dr)";
		this.lbldetaildrcr.AutoSize = true;
		this.lbldetaildrcr.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbldetaildrcr.Location = new System.Drawing.Point(235, 3);
		this.lbldetaildrcr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lbldetaildrcr.Name = "lbldetaildrcr";
		this.lbldetaildrcr.Size = new System.Drawing.Size(36, 15);
		this.lbldetaildrcr.TabIndex = 129;
		this.lbldetaildrcr.Text = "Cr/Dr";
		this.lblbalancedetail.AutoSize = true;
		this.lblbalancedetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblbalancedetail.Location = new System.Drawing.Point(164, 3);
		this.lblbalancedetail.Name = "lblbalancedetail";
		this.lblbalancedetail.Size = new System.Drawing.Size(14, 15);
		this.lblbalancedetail.TabIndex = 130;
		this.lblbalancedetail.Text = "0";
		this.PanelSubsidiaryDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount);
		this.PanelSubsidiaryDetail.Controls.Add(this.lblSubsidiaryaccount);
		this.PanelSubsidiaryDetail.Location = new System.Drawing.Point(431, 0);
		this.PanelSubsidiaryDetail.Margin = new System.Windows.Forms.Padding(0);
		this.PanelSubsidiaryDetail.Name = "PanelSubsidiaryDetail";
		this.PanelSubsidiaryDetail.Size = new System.Drawing.Size(215, 45);
		this.PanelSubsidiaryDetail.TabIndex = 2;
		this.CmbSubsidiaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbSubsidiaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSubsidiaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSubsidiaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Location = new System.Drawing.Point(2, 18);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Name = "CmbSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Leave += new System.EventHandler(CmbSubsidiaryAccount_Leave);
		this.lblSubsidiaryaccount.AutoSize = true;
		this.lblSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryaccount.Location = new System.Drawing.Point(2, 2);
		this.lblSubsidiaryaccount.Name = "lblSubsidiaryaccount";
		this.lblSubsidiaryaccount.Size = new System.Drawing.Size(110, 15);
		this.lblSubsidiaryaccount.TabIndex = 134;
		this.lblSubsidiaryaccount.Text = "Subsidiary Account";
		this.PanelJobLotDetail.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLotFile);
		this.PanelJobLotDetail.Controls.Add(this.label10);
		this.PanelJobLotDetail.Location = new System.Drawing.Point(646, 0);
		this.PanelJobLotDetail.Margin = new System.Windows.Forms.Padding(0);
		this.PanelJobLotDetail.Name = "PanelJobLotDetail";
		this.PanelJobLotDetail.Size = new System.Drawing.Size(114, 45);
		this.PanelJobLotDetail.TabIndex = 3;
		this.cmbJobLotFile.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotFile.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotFile.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotFile.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotFile).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotFile).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLotFile).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLotFile).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLotFile.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLotFile).Location = new System.Drawing.Point(1, 18);
		((System.Windows.Forms.Control)(object)this.cmbJobLotFile).Name = "cmbJobLotFile";
		((System.Windows.Forms.Control)(object)this.cmbJobLotFile).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotFile).TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(2, 2);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(48, 15);
		this.label10.TabIndex = 122;
		this.label10.Text = "Job/Lot";
		this.PaanelInvoiceNoDetail.Controls.Add(this.label19);
		this.PaanelInvoiceNoDetail.Controls.Add(this.txtInvoiceNoDetail);
		this.PaanelInvoiceNoDetail.Location = new System.Drawing.Point(760, 0);
		this.PaanelInvoiceNoDetail.Margin = new System.Windows.Forms.Padding(0);
		this.PaanelInvoiceNoDetail.Name = "PaanelInvoiceNoDetail";
		this.PaanelInvoiceNoDetail.Size = new System.Drawing.Size(93, 45);
		this.PaanelInvoiceNoDetail.TabIndex = 4;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(4, 3);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(77, 15);
		this.label19.TabIndex = 132;
		this.label19.Text = "Bill/Invoice #";
		this.txtInvoiceNoDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNoDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNoDetail.Location = new System.Drawing.Point(1, 21);
		this.txtInvoiceNoDetail.Name = "txtInvoiceNoDetail";
		this.txtInvoiceNoDetail.Size = new System.Drawing.Size(91, 23);
		this.txtInvoiceNoDetail.TabIndex = 3;
		this.PanelQty.Controls.Add(this.txtQtyDetial);
		this.PanelQty.Controls.Add(this.label12);
		this.PanelQty.Location = new System.Drawing.Point(853, 0);
		this.PanelQty.Margin = new System.Windows.Forms.Padding(0);
		this.PanelQty.Name = "PanelQty";
		this.PanelQty.Size = new System.Drawing.Size(84, 45);
		this.PanelQty.TabIndex = 5;
		this.txtQtyDetial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyDetial.Location = new System.Drawing.Point(1, 21);
		this.txtQtyDetial.Name = "txtQtyDetial";
		this.txtQtyDetial.Size = new System.Drawing.Size(82, 23);
		this.txtQtyDetial.TabIndex = 4;
		this.txtQtyDetial.Text = "0";
		this.txtQtyDetial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyDetial.TextChanged += new System.EventHandler(txtQtyDetial_TextChanged);
		this.txtQtyDetial.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyDetial_KeyPress);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(26, 15);
		this.label12.TabIndex = 124;
		this.label12.Text = "Qty";
		this.PanelRateDetail.Controls.Add(this.txtRateDetail);
		this.PanelRateDetail.Controls.Add(this.label14);
		this.PanelRateDetail.Location = new System.Drawing.Point(937, 0);
		this.PanelRateDetail.Margin = new System.Windows.Forms.Padding(0);
		this.PanelRateDetail.Name = "PanelRateDetail";
		this.PanelRateDetail.Size = new System.Drawing.Size(82, 45);
		this.PanelRateDetail.TabIndex = 6;
		this.txtRateDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateDetail.Location = new System.Drawing.Point(1, 21);
		this.txtRateDetail.Name = "txtRateDetail";
		this.txtRateDetail.Size = new System.Drawing.Size(80, 23);
		this.txtRateDetail.TabIndex = 5;
		this.txtRateDetail.Text = "0";
		this.txtRateDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateDetail.TextChanged += new System.EventHandler(txtRateDetail_TextChanged);
		this.txtRateDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateDetail_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(1, 3);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(30, 15);
		this.label14.TabIndex = 126;
		this.label14.Text = "Rate";
		this.PanelAmountDetail.Controls.Add(this.txtAmountDetail);
		this.PanelAmountDetail.Controls.Add(this.label15);
		this.PanelAmountDetail.Location = new System.Drawing.Point(0, 45);
		this.PanelAmountDetail.Margin = new System.Windows.Forms.Padding(0);
		this.PanelAmountDetail.Name = "PanelAmountDetail";
		this.PanelAmountDetail.Size = new System.Drawing.Size(108, 45);
		this.PanelAmountDetail.TabIndex = 7;
		this.txtAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountDetail.Location = new System.Drawing.Point(1, 21);
		this.txtAmountDetail.Name = "txtAmountDetail";
		this.txtAmountDetail.Size = new System.Drawing.Size(106, 23);
		this.txtAmountDetail.TabIndex = 6;
		this.txtAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmountDetail.TextChanged += new System.EventHandler(txtAmountDetail_TextChanged);
		this.txtAmountDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmountDetail_KeyPress);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(51, 15);
		this.label15.TabIndex = 128;
		this.label15.Text = "Amount";
		this.PanelFcyAmountDetail.Controls.Add(this.txtFcyAmountDetail);
		this.PanelFcyAmountDetail.Controls.Add(this.label18);
		this.PanelFcyAmountDetail.Location = new System.Drawing.Point(108, 45);
		this.PanelFcyAmountDetail.Margin = new System.Windows.Forms.Padding(0);
		this.PanelFcyAmountDetail.Name = "PanelFcyAmountDetail";
		this.PanelFcyAmountDetail.Size = new System.Drawing.Size(110, 45);
		this.PanelFcyAmountDetail.TabIndex = 8;
		this.txtFcyAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountDetail.Location = new System.Drawing.Point(1, 22);
		this.txtFcyAmountDetail.Name = "txtFcyAmountDetail";
		this.txtFcyAmountDetail.ShortcutsEnabled = false;
		this.txtFcyAmountDetail.Size = new System.Drawing.Size(109, 23);
		this.txtFcyAmountDetail.TabIndex = 0;
		this.txtFcyAmountDetail.Tag = "FcyAmount";
		this.txtFcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountDetail.TextChanged += new System.EventHandler(txtFcyAmountDetail_TextChanged);
		this.txtFcyAmountDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyAmountDetail_KeyPress);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(72, 15);
		this.label18.TabIndex = 4;
		this.label18.Text = "Fcy Amount";
		this.ReferenceAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceAccount);
		this.ReferenceAccountPanel.Controls.Add(this.label46);
		this.ReferenceAccountPanel.Location = new System.Drawing.Point(218, 45);
		this.ReferenceAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ReferenceAccountPanel.Name = "ReferenceAccountPanel";
		this.ReferenceAccountPanel.Size = new System.Drawing.Size(208, 45);
		this.ReferenceAccountPanel.TabIndex = 9;
		this.CmbReferenceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbReferenceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Location = new System.Drawing.Point(1, 18);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Name = "CmbReferenceAccount";
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).TabIndex = 2;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(2, 2);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(107, 15);
		this.label46.TabIndex = 38;
		this.label46.Text = "Reference Account";
		this.PanelRemarksDetail.Controls.Add(this.txtRemarksDetail);
		this.PanelRemarksDetail.Controls.Add(this.label11);
		this.PanelRemarksDetail.Location = new System.Drawing.Point(426, 45);
		this.PanelRemarksDetail.Margin = new System.Windows.Forms.Padding(0);
		this.PanelRemarksDetail.Name = "PanelRemarksDetail";
		this.PanelRemarksDetail.Size = new System.Drawing.Size(428, 45);
		this.PanelRemarksDetail.TabIndex = 10;
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(1, 21);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(427, 23);
		this.txtRemarksDetail.TabIndex = 7;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(2, 3);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 122;
		this.label11.Text = "Remarks";
		this.PanelAddUpdateBtn.Controls.Add(this.btnAdd);
		this.PanelAddUpdateBtn.Controls.Add(this.btnCancelDetail);
		this.PanelAddUpdateBtn.Controls.Add(this.btnUpdateDetail);
		this.PanelAddUpdateBtn.Location = new System.Drawing.Point(854, 45);
		this.PanelAddUpdateBtn.Margin = new System.Windows.Forms.Padding(0);
		this.PanelAddUpdateBtn.Name = "PanelAddUpdateBtn";
		this.PanelAddUpdateBtn.Size = new System.Drawing.Size(135, 45);
		this.PanelAddUpdateBtn.TabIndex = 11;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(3, 17);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(35, 27);
		this.btnAdd.TabIndex = 8;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnAdd.Leave += new System.EventHandler(btnAdd_Leave);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(69, 18);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(63, 26);
		this.btnCancelDetail.TabIndex = 10;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(btnCancelDetail_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(3, 18);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 26);
		this.btnUpdateDetail.TabIndex = 9;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.panel6.Controls.Add(this.groupBox1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 59);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1152, 136);
		this.panel6.TabIndex = 1;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountType);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.lblExchangeRate);
		this.groupBox1.Controls.Add(this.lblFcyAmount);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.lblFcyCode);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProjectId);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountCr);
		this.groupBox1.Controls.Add(this.lblSubsidiaryaccountCr);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.lblCreditAcBalance);
		this.groupBox1.Controls.Add(this.txtInvoiceDate);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccount);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDocDate);
		this.groupBox1.Controls.Add(this.txtInvoiceNo);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.lblDrCrS);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1016, 127);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.CmbAccountType.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance139;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountType).Location = new System.Drawing.Point(567, 13);
		((System.Windows.Forms.Control)(object)this.CmbAccountType).Name = "CmbAccountType";
		((System.Windows.Forms.Control)(object)this.CmbAccountType).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountType).TabIndex = 575;
		((System.Windows.Forms.Control)(object)this.CmbAccountType).Leave += new System.EventHandler(CmbAccountType_Leave);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(466, 19);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(80, 15);
		this.label41.TabIndex = 576;
		this.label41.Text = "Account Type";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(888, 42);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(125, 23);
		this.txtExchangeRate.TabIndex = 10;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.lblExchangeRate.AutoSize = true;
		this.lblExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblExchangeRate.Location = new System.Drawing.Point(805, 46);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(84, 15);
		this.lblExchangeRate.TabIndex = 573;
		this.lblExchangeRate.Text = "Exchange Rate";
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFcyAmount.Location = new System.Drawing.Point(805, 72);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(72, 15);
		this.lblFcyAmount.TabIndex = 574;
		this.lblFcyAmount.Text = "Fcy Amount";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(888, 68);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(125, 23);
		this.txtFcyAmount.TabIndex = 11;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance165;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance170).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance173).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(888, 13);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 9;
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(805, 19);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 572;
		this.lblFcyCode.Text = "Fcy Code";
		this.CmbProjectId.AllowNull = (DefaultableBoolean)1;
		this.CmbProjectId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProjectId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProjectId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance178;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance184).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProjectId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Location = new System.Drawing.Point(80, 14);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Name = "CmbProjectId";
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Size = new System.Drawing.Size(206, 24);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabStop = false;
		this.CmbSubsidiaryAccountCr.AllowNull = (DefaultableBoolean)1;
		this.CmbSubsidiaryAccountCr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSubsidiaryAccountCr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSubsidiaryAccountCr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance157).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSubsidiaryAccountCr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountCr).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSubsidiaryAccountCr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountCr).Location = new System.Drawing.Point(567, 93);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountCr).Name = "CmbSubsidiaryAccountCr";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountCr).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountCr).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountCr).Leave += new System.EventHandler(CmbSubsidiaryAccountDr_Leave);
		this.lblSubsidiaryaccountCr.AutoSize = true;
		this.lblSubsidiaryaccountCr.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryaccountCr.Location = new System.Drawing.Point(466, 99);
		this.lblSubsidiaryaccountCr.Name = "lblSubsidiaryaccountCr";
		this.lblSubsidiaryaccountCr.Size = new System.Drawing.Size(85, 15);
		this.lblSubsidiaryaccountCr.TabIndex = 143;
		this.lblSubsidiaryaccountCr.Text = "Subsidiary A/C";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(80, 68);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(383, 51);
		this.txtRemarks.TabIndex = 3;
		this.lblCreditAcBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblCreditAcBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditAcBalance.Location = new System.Drawing.Point(567, 68);
		this.lblCreditAcBalance.Name = "lblCreditAcBalance";
		this.lblCreditAcBalance.ReadOnly = true;
		this.lblCreditAcBalance.Size = new System.Drawing.Size(201, 23);
		this.lblCreditAcBalance.TabIndex = 7;
		this.lblCreditAcBalance.Text = "0";
		this.lblCreditAcBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblCreditAcBalance.TextChanged += new System.EventHandler(txtAmountDetail_TextChanged);
		this.lblCreditAcBalance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmountDetail_KeyPress);
		this.txtInvoiceDate.CustomFormat = "dd-MMM-yyyy";
		this.txtInvoiceDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtInvoiceDate.Location = new System.Drawing.Point(340, 42);
		this.txtInvoiceDate.Name = "txtInvoiceDate";
		this.txtInvoiceDate.Size = new System.Drawing.Size(123, 22);
		this.txtInvoiceDate.TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(290, 46);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(51, 15);
		this.label5.TabIndex = 116;
		this.label5.Text = "Bill Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(290, 19);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(42, 15);
		this.label4.TabIndex = 116;
		this.label4.Text = "Bill No";
		this.cmbAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccount).Location = new System.Drawing.Point(567, 40);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Name = "cmbAccount";
		((System.Windows.Forms.Control)(object)this.cmbAccount).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccount).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbAccount).Leave += new System.EventHandler(cmbAccount_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(466, 72);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(96, 15);
		this.label6.TabIndex = 114;
		this.label6.Text = "Account Balance";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(466, 46);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(100, 15);
		this.label7.TabIndex = 114;
		this.label7.Text = "Account Title (Cr)";
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(163, 42);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(123, 22);
		this.txtDocDate.TabIndex = 2;
		this.txtInvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNo.Location = new System.Drawing.Point(340, 15);
		this.txtInvoiceNo.Name = "txtInvoiceNo";
		this.txtInvoiceNo.Size = new System.Drawing.Size(123, 23);
		this.txtInvoiceNo.TabIndex = 4;
		this.txtDocNo.BackColor = System.Drawing.Color.White;
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(80, 42);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(82, 23);
		this.txtDocNo.TabIndex = 1;
		this.txtDocNo.Leave += new System.EventHandler(txtDocNo_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 19);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(67, 15);
		this.label13.TabIndex = 568;
		this.label13.Text = "Cost Center";
		this.lblDrCrS.AutoSize = true;
		this.lblDrCrS.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDrCrS.Location = new System.Drawing.Point(771, 72);
		this.lblDrCrS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblDrCrS.Name = "lblDrCrS";
		this.lblDrCrS.Size = new System.Drawing.Size(18, 15);
		this.lblDrCrS.TabIndex = 130;
		this.lblDrCrS.Text = "Cr";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(6, 69);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 20);
		this.label8.TabIndex = 120;
		this.label8.Text = "Remarks";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 46);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(76, 15);
		this.label2.TabIndex = 112;
		this.label2.Text = "Doc No,Date";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.chkCustomAccounts);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.labelLocationType);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel2.Controls.Add(this.CHKPREVIEW);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1152, 56);
		this.panel2.TabIndex = 0;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(547, 33);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(137, 17);
		this.chkCustomAccounts.TabIndex = 512;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnSaveAs, this.btnUpdate, this.btnDelete, this.btnAttachment, this.Print, this.BtnPrintII, this.BtnLookup,
			this.btnShortCutKey
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1152, 27);
		this.toolStrip1.TabIndex = 6;
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
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "&SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(116, 24);
		this.btnAttachment.Text = "Attachments";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "102-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.BtnPrintII.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintII.Name = "BtnPrintII";
		this.BtnPrintII.Size = new System.Drawing.Size(116, 24);
		this.BtnPrintII.Text = "102_01-Print";
		this.BtnPrintII.Click += new System.EventHandler(BtnPrintII_Click);
		this.BtnLookup.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnLookup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLookup.Name = "BtnLookup";
		this.BtnLookup.Size = new System.Drawing.Size(226, 24);
		this.BtnLookup.Text = "Accounts Allocate To Custom";
		this.BtnLookup.Click += new System.EventHandler(BtnLookup_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(135, 34);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 511;
		this.labelLocationType.Text = "Location Type";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(223, 28);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 510;
		this.CHKPREVIEW.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CHKPREVIEW.AutoSize = true;
		this.CHKPREVIEW.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CHKPREVIEW.ForeColor = System.Drawing.Color.White;
		this.CHKPREVIEW.Location = new System.Drawing.Point(1063, 31);
		this.CHKPREVIEW.Name = "CHKPREVIEW";
		this.CHKPREVIEW.Size = new System.Drawing.Size(84, 20);
		this.CHKPREVIEW.TabIndex = 10;
		this.CHKPREVIEW.Text = "Preview";
		this.CHKPREVIEW.UseVisualStyleBackColor = true;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 32);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(121, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Bills Payables";
		this.panel19.Controls.Add(this.txtTotalAmount);
		this.panel19.Controls.Add(this.txtVoucherAmountFooter);
		this.panel19.Controls.Add(this.txtTaxPercent);
		this.panel19.Controls.Add(this.txtOtherChargesAmount);
		this.panel19.Controls.Add(this.txtAdvanceTaxAmount);
		this.panel19.Controls.Add(this.txtTaxAmount);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.panel19.Controls.Add(this.label1);
		this.panel19.Controls.Add(this.label23);
		this.panel19.Controls.Add(this.panel8);
		this.panel19.Controls.Add(this.labelNoteTitleDetailAccount);
		this.panel19.Controls.Add(this.label26);
		this.panel19.Controls.Add(this.label22);
		this.panel19.Controls.Add(this.label20);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherChargesAccount);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAdvanceTaxAccount);
		this.panel19.Controls.Add(this.label27);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.panel19.Controls.Add(this.label25);
		this.panel19.Controls.Add(this.label17);
		this.panel19.Controls.Add(this.label16);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel19.Location = new System.Drawing.Point(3, 449);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1152, 230);
		this.panel19.TabIndex = 0;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Enabled = false;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(903, 205);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.Size = new System.Drawing.Size(111, 23);
		this.txtTotalAmount.TabIndex = 9;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVoucherAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoucherAmountFooter.Enabled = false;
		this.txtVoucherAmountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVoucherAmountFooter.Location = new System.Drawing.Point(903, 3);
		this.txtVoucherAmountFooter.Name = "txtVoucherAmountFooter";
		this.txtVoucherAmountFooter.Size = new System.Drawing.Size(111, 23);
		this.txtVoucherAmountFooter.TabIndex = 0;
		this.txtVoucherAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxPercent.Enabled = false;
		this.txtTaxPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(903, 27);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(111, 22);
		this.txtTaxPercent.TabIndex = 2;
		this.txtTaxPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherChargesAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherChargesAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherChargesAmount.Location = new System.Drawing.Point(903, 181);
		this.txtOtherChargesAmount.Name = "txtOtherChargesAmount";
		this.txtOtherChargesAmount.Size = new System.Drawing.Size(111, 23);
		this.txtOtherChargesAmount.TabIndex = 8;
		this.txtOtherChargesAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherChargesAmount.TextChanged += new System.EventHandler(txtOtherChargesAmount_TextChanged);
		this.txtOtherChargesAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherChargesAmount_KeyPress);
		this.txtAdvanceTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdvanceTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAdvanceTaxAmount.Location = new System.Drawing.Point(903, 129);
		this.txtAdvanceTaxAmount.Name = "txtAdvanceTaxAmount";
		this.txtAdvanceTaxAmount.Size = new System.Drawing.Size(111, 23);
		this.txtAdvanceTaxAmount.TabIndex = 6;
		this.txtAdvanceTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAdvanceTaxAmount.TextChanged += new System.EventHandler(txtAdvanceTaxAmount_TextChanged);
		this.txtAdvanceTaxAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAdvanceTaxAmount_KeyPress);
		this.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(903, 78);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.Size = new System.Drawing.Size(111, 23);
		this.txtTaxAmount.TabIndex = 4;
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(784, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(92, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TextChanged += new System.EventHandler(CmbTaxType_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_TextChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(653, 31);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 130;
		this.label1.Text = "Sales Tax Type";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(776, 209);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(80, 15);
		this.label23.TabIndex = 128;
		this.label23.Text = "Total Amount";
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdSchedule);
		this.panel8.Controls.Add(this.panel20);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(506, 230);
		this.panel8.TabIndex = 129;
		this.grdSchedule.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdSchedule).BackColor = System.Drawing.Color.White;
		grdSchedule_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSchedule.DesignTimeLayout = grdSchedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSchedule.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdSchedule).Name = "grdSchedule";
		((ScrollableBase)this.grdSchedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Size = new System.Drawing.Size(506, 211);
		((System.Windows.Forms.Control)(object)this.grdSchedule).TabIndex = 0;
		this.grdSchedule.TotalRow = (InheritableBoolean)1;
		this.grdSchedule.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSchedule).VisualStyle = (VisualStyle)4;
		this.grdSchedule.CellUpdated += new ColumnActionEventHandler(grdSchedule_CellUpdated);
		this.grdSchedule.ColumnButtonClick += new ColumnActionEventHandler(grdSchedule_ColumnButtonClick);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label21);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(506, 19);
		this.panel20.TabIndex = 1;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(4, 2);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(134, 14);
		this.label21.TabIndex = 127;
		this.label21.Text = "Amount Distribution";
		this.labelNoteTitleDetailAccount.AutoSize = true;
		this.labelNoteTitleDetailAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelNoteTitleDetailAccount.Location = new System.Drawing.Point(776, 185);
		this.labelNoteTitleDetailAccount.Name = "labelNoteTitleDetailAccount";
		this.labelNoteTitleDetailAccount.Size = new System.Drawing.Size(129, 15);
		this.labelNoteTitleDetailAccount.TabIndex = 128;
		this.labelNoteTitleDetailAccount.Text = "Other Charges Amount";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(776, 133);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(121, 15);
		this.label26.TabIndex = 128;
		this.label26.Text = "Advance Tax Amount";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(776, 7);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(98, 15);
		this.label22.TabIndex = 128;
		this.label22.Text = "Voucher Amount";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(776, 82);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(102, 15);
		this.label20.TabIndex = 128;
		this.label20.Text = "Sales Tax Amount";
		this.CmbOtherChargesAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherChargesAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherChargesAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherChargesAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance190).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance190).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance191;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance197).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherChargesAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherChargesAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOtherChargesAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOtherChargesAccount).Location = new System.Drawing.Point(784, 153);
		((System.Windows.Forms.Control)(object)this.CmbOtherChargesAccount).Name = "CmbOtherChargesAccount";
		((System.Windows.Forms.Control)(object)this.CmbOtherChargesAccount).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherChargesAccount).TabIndex = 7;
		this.CmbAdvanceTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbAdvanceTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAdvanceTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAdvanceTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAdvanceTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceTaxAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAdvanceTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceTaxAccount).Location = new System.Drawing.Point(784, 102);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceTaxAccount).Name = "CmbAdvanceTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbAdvanceTaxAccount).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceTaxAccount).TabIndex = 5;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(653, 159);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(130, 15);
		this.label27.TabIndex = 124;
		this.label27.Text = "Other Charges Account";
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(784, 51);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 3;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(653, 108);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(122, 15);
		this.label25.TabIndex = 124;
		this.label25.Text = "Advance Tax Account";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(879, 31);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(17, 15);
		this.label17.TabIndex = 126;
		this.label17.Text = "%";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(653, 57);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(103, 15);
		this.label16.TabIndex = 124;
		this.label16.Text = "Sales Tax Account";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailHistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1158, 682);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 154);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1152, 347);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 7;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label39);
		this.panel12.Controls.Add(this.ctrlGrdBar3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 132);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1152, 22);
		this.panel12.TabIndex = 8;
		this.label39.AutoSize = true;
		this.label39.BackColor = System.Drawing.Color.Teal;
		this.label39.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(3, 1);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(142, 19);
		this.label39.TabIndex = 53;
		this.label39.Text = "Filtered Records";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1112, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel11.Controls.Add(this.VoucherInfoBox);
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 53);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1152, 79);
		this.panel11.TabIndex = 1;
		this.VoucherInfoBox.Controls.Add(this.label36);
		this.VoucherInfoBox.Controls.Add(this.lblUnApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.lblTotalVouchers);
		this.VoucherInfoBox.Controls.Add(this.label38);
		this.VoucherInfoBox.Controls.Add(this.lblApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.label37);
		this.VoucherInfoBox.Location = new System.Drawing.Point(795, 3);
		this.VoucherInfoBox.Name = "VoucherInfoBox";
		this.VoucherInfoBox.Size = new System.Drawing.Size(221, 72);
		this.VoucherInfoBox.TabIndex = 10;
		this.VoucherInfoBox.TabStop = false;
		this.VoucherInfoBox.Visible = false;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(6, 12);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(85, 15);
		this.label36.TabIndex = 1;
		this.label36.Text = "Total Vouchers";
		this.lblUnApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUnApprovedVouchers.Location = new System.Drawing.Point(136, 47);
		this.lblUnApprovedVouchers.Name = "lblUnApprovedVouchers";
		this.lblUnApprovedVouchers.Size = new System.Drawing.Size(80, 17);
		this.lblUnApprovedVouchers.TabIndex = 6;
		this.lblUnApprovedVouchers.Text = "0";
		this.lblUnApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalVouchers.Location = new System.Drawing.Point(136, 11);
		this.lblTotalVouchers.Name = "lblTotalVouchers";
		this.lblTotalVouchers.Size = new System.Drawing.Size(80, 17);
		this.lblTotalVouchers.TabIndex = 4;
		this.lblTotalVouchers.Text = "0";
		this.lblTotalVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(6, 48);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(126, 15);
		this.label38.TabIndex = 3;
		this.label38.Text = "UnApproved Vouchers";
		this.lblApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedVouchers.Location = new System.Drawing.Point(136, 29);
		this.lblApprovedVouchers.Name = "lblApprovedVouchers";
		this.lblApprovedVouchers.Size = new System.Drawing.Size(80, 17);
		this.lblApprovedVouchers.TabIndex = 5;
		this.lblApprovedVouchers.Text = "0";
		this.lblApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(6, 30);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(111, 15);
		this.label37.TabIndex = 2;
		this.label37.Text = "Approved Vouchers";
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.rdexpapproved);
		this.groupBox3.Controls.Add(this.rdexpmodify);
		this.groupBox3.Controls.Add(this.rdexpdocdate);
		this.groupBox3.Controls.Add(this.rdexpentry);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.label32);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatus);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(790, 72);
		this.groupBox3.TabIndex = 9;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.CmbAccountTitleHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitleHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitleHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitleHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitleHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Location = new System.Drawing.Point(451, 13);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Name = "CmbAccountTitleHistory";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).TabIndex = 4;
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(258, 15);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(92, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.rdexpapproved.AutoSize = true;
		this.rdexpapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.rdexpapproved.Location = new System.Drawing.Point(690, 55);
		this.rdexpapproved.Name = "rdexpapproved";
		this.rdexpapproved.Size = new System.Drawing.Size(103, 17);
		this.rdexpapproved.TabIndex = 9;
		this.rdexpapproved.Text = "Approved Date";
		this.rdexpapproved.UseVisualStyleBackColor = true;
		this.rdexpmodify.AutoSize = true;
		this.rdexpmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.rdexpmodify.Location = new System.Drawing.Point(690, 40);
		this.rdexpmodify.Name = "rdexpmodify";
		this.rdexpmodify.Size = new System.Drawing.Size(89, 17);
		this.rdexpmodify.TabIndex = 8;
		this.rdexpmodify.Text = "Modify Date";
		this.rdexpmodify.UseVisualStyleBackColor = true;
		this.rdexpdocdate.AutoSize = true;
		this.rdexpdocdate.Checked = true;
		this.rdexpdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.rdexpdocdate.Location = new System.Drawing.Point(690, 10);
		this.rdexpdocdate.Name = "rdexpdocdate";
		this.rdexpdocdate.Size = new System.Drawing.Size(72, 17);
		this.rdexpdocdate.TabIndex = 6;
		this.rdexpdocdate.TabStop = true;
		this.rdexpdocdate.Text = "Doc Date";
		this.rdexpdocdate.UseVisualStyleBackColor = true;
		this.rdexpentry.AutoSize = true;
		this.rdexpentry.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.rdexpentry.Location = new System.Drawing.Point(690, 25);
		this.rdexpentry.Name = "rdexpentry";
		this.rdexpentry.Size = new System.Drawing.Size(78, 17);
		this.rdexpentry.TabIndex = 7;
		this.rdexpentry.Text = "Entry Date";
		this.rdexpentry.UseVisualStyleBackColor = true;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(354, 19);
		this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(100, 15);
		this.label28.TabIndex = 147;
		this.label28.Text = "Account Title (Cr)";
		this.btnShowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowHistory.FlatAppearance.BorderSize = 0;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowHistory.Location = new System.Drawing.Point(614, 41);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(71, 26);
		this.btnShowHistory.TabIndex = 10;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.btnShowHistory.Leave += new System.EventHandler(btnShowHistory_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(354, 47);
		this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(95, 15);
		this.label30.TabIndex = 142;
		this.label30.Text = "Approved Status";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(200, 47);
		this.label31.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(45, 15);
		this.label31.TabIndex = 141;
		this.label31.Text = "To Doc";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(200, 19);
		this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(60, 15);
		this.label32.TabIndex = 140;
		this.label32.Text = "From Doc";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(4, 47);
		this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 139;
		this.label33.Text = "To Date";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(4, 19);
		this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(63, 15);
		this.label34.TabIndex = 13;
		this.label34.Text = "From Date";
		this.cmbApproveStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApproveStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Location = new System.Drawing.Point(451, 41);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Name = "cmbApproveStatus";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Size = new System.Drawing.Size(158, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).TabIndex = 5;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(258, 43);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(92, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(70, 43);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(126, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(70, 15);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(126, 22);
		this.FromDateHistory.TabIndex = 0;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label40);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(3, 501);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1152, 22);
		this.panel13.TabIndex = 9;
		this.label40.AutoSize = true;
		this.label40.BackColor = System.Drawing.Color.Teal;
		this.label40.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(3, 5);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(201, 17);
		this.label40.TabIndex = 56;
		this.label40.Text = "Detail of above selected row";
		this.grdDetailHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetailHistory).BackColor = System.Drawing.Color.White;
		grdDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailHistory.DesignTimeLayout = grdDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetailHistory.DynamicFiltering = true;
		this.grdDetailHistory.FilterMode = (FilterMode)2;
		this.grdDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Location = new System.Drawing.Point(3, 523);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Name = "grdDetailHistory";
		((ScrollableBase)this.grdDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Size = new System.Drawing.Size(1152, 156);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).TabIndex = 8;
		this.grdDetailHistory.TotalRow = (InheritableBoolean)1;
		this.grdDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label35);
		this.panel9.Controls.Add(this.toolStrip3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1152, 50);
		this.panel9.TabIndex = 0;
		this.label35.AutoSize = true;
		this.label35.BackColor = System.Drawing.Color.Teal;
		this.label35.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(2, 29);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(184, 19);
		this.label35.TabIndex = 53;
		this.label35.Text = "Bills Payables History";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1152, 27);
		this.toolStrip3.TabIndex = 52;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.btnLoadAll.Image = (System.Drawing.Image)resources.GetObject("btnLoadAll.Image");
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(84, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1166, 711);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmBillsPayables";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmBillsPayables_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillsPayables_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		this.PanelCustomGroupDetail.ResumeLayout(false);
		this.PanelCustomGroupDetail.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		this.PanelAccountTitleDetail.ResumeLayout(false);
		this.PanelAccountTitleDetail.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountDetail).EndInit();
		this.PanelSubsidiaryDetail.ResumeLayout(false);
		this.PanelSubsidiaryDetail.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).EndInit();
		this.PanelJobLotDetail.ResumeLayout(false);
		this.PanelJobLotDetail.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotFile).EndInit();
		this.PaanelInvoiceNoDetail.ResumeLayout(false);
		this.PaanelInvoiceNoDetail.PerformLayout();
		this.PanelQty.ResumeLayout(false);
		this.PanelQty.PerformLayout();
		this.PanelRateDetail.ResumeLayout(false);
		this.PanelRateDetail.PerformLayout();
		this.PanelAmountDetail.ResumeLayout(false);
		this.PanelAmountDetail.PerformLayout();
		this.PanelFcyAmountDetail.ResumeLayout(false);
		this.PanelFcyAmountDetail.PerformLayout();
		this.ReferenceAccountPanel.ResumeLayout(false);
		this.ReferenceAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).EndInit();
		this.PanelRemarksDetail.ResumeLayout(false);
		this.PanelRemarksDetail.PerformLayout();
		this.PanelAddUpdateBtn.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccountCr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSchedule).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherChargesAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceTaxAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.VoucherInfoBox.ResumeLayout(false);
		this.VoucherInfoBox.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
